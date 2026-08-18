using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using ChangeX.BLL.StatusMachine;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.BLL.Services
{
    public class CRService : ICRService
    {
        private static readonly HashSet<string> AdminStageTargets = new(
            StringComparer.OrdinalIgnoreCase)
        {
            WorkflowStatuses.Design,
            WorkflowStatuses.Development,
            WorkflowStatuses.Testing,
            WorkflowStatuses.PendingClientApproval,
            WorkflowStatuses.Analysis,
            WorkflowStatuses.Delivered
        };

        private readonly ApplicationContext _dbContext;

        public CRService(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CRWorkflowResponseDto> RequestCRAsync(
            RequestCRDto dto,
            Guid clientId)
        {
            ValidateRequest(dto, clientId);

            var projectExists = await _dbContext.Projects
                .AsNoTracking()
                .AnyAsync(project =>
                    project.ID == dto.ProjectID && project.ClientID == clientId);

            if (!projectExists)
            {
                throw new KeyNotFoundException(
                    "Project was not found for the specified client");
            }

            var initialStatus = await GetStatusAsync(
                WorkflowStatuses.PendingVendorFeedback);

            var cr = new CR
            {
                ID = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Priority = dto.Priority.Trim(),
                Scope = dto.Scope.Trim(),
                Description = dto.Description.Trim(),
                ProjectID = dto.ProjectID,
                CurrentStatusID = initialStatus.ID
            };

            _dbContext.CRs.Add(cr);
            await _dbContext.SaveChangesAsync();

            cr.CurrentStatus = initialStatus;
            return await BuildResponseAsync(cr);
        }

        public async Task<CRWorkflowResponseDto> SubmitAdminFeedbackAsync(
            Guid crId,
            AdminFeedbackDto dto)
        {
            var cr = await GetCRForUpdateAsync(crId);
            EnsureCurrentStatus(cr, WorkflowStatuses.PendingVendorFeedback);

            if (Matches(dto.Decision, WorkflowDecisions.Approve))
            {
                ValidateEstimate(dto.Estimate);

                if (dto.InvoiceCost is null or <= 0)
                {
                    throw new InvalidOperationException(
                        "Invoice cost must be greater than zero when feedback is approved");
                }

                var invoiceExists = await _dbContext.Invoices
                    .AnyAsync(invoice => invoice.CRID == crId);
                if (invoiceExists)
                {
                    throw new InvalidOperationException(
                        "An invoice already exists for this CR");
                }

                ApplyEstimate(cr, dto.Estimate!);
                _dbContext.Invoices.Add(new Invoice
                {
                    ID = Guid.NewGuid(),
                    CRID = crId,
                    Cost = dto.InvoiceCost.Value,
                    CreatedTime = DateTime.UtcNow,
                    State = InvoiceStates.Pending
                });

                await ApplyTransitionAsync(
                    cr,
                    WorkflowStatuses.PendingEstimateApproval,
                    WorkflowRoles.Admin);
            }
            else if (Matches(dto.Decision, WorkflowDecisions.Reject))
            {
                await ApplyTransitionAsync(
                    cr,
                    WorkflowStatuses.Rejected,
                    WorkflowRoles.Admin);
            }
            else if (Matches(
                dto.Decision,
                WorkflowDecisions.RequestClarification))
            {
                if (string.IsNullOrWhiteSpace(dto.Message))
                {
                    throw new InvalidOperationException(
                        "A clarification message is required");
                }

                _dbContext.Details.Add(CreateDetail(
                    crId,
                    string.Empty,
                    dto.Message,
                    WorkflowStatuses.ClarificationRequested));

                await ApplyTransitionAsync(
                    cr,
                    WorkflowStatuses.ClarificationRequested,
                    WorkflowRoles.Admin);
            }
            else
            {
                throw new InvalidOperationException(
                    "Decision must be Approve, Reject, or RequestClarification");
            }

            await _dbContext.SaveChangesAsync();
            return await BuildResponseAsync(cr);
        }

        public async Task<CRWorkflowResponseDto> SubmitClarificationAsync(
            Guid crId,
            DetailDto dto)
        {
            if (dto.CRID != Guid.Empty && dto.CRID != crId)
            {
                throw new InvalidOperationException(
                    "The CR ID in the request body does not match the route");
            }

            var attachment = dto.Attachment?.Trim() ?? string.Empty;
            var comment = dto.Comment?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(attachment) &&
                string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException(
                    "A comment or attachment is required");
            }

            var cr = await GetCRForUpdateAsync(crId);
            EnsureCurrentStatus(cr, WorkflowStatuses.ClarificationRequested);

            _dbContext.Details.Add(CreateDetail(
                crId,
                attachment,
                comment,
                WorkflowStatuses.PendingVendorFeedback));

            await ApplyTransitionAsync(
                cr,
                WorkflowStatuses.PendingVendorFeedback,
                WorkflowRoles.Client);

            await _dbContext.SaveChangesAsync();
            return await BuildResponseAsync(cr);
        }

        public async Task<CRWorkflowResponseDto> SubmitEstimateDecisionAsync(
            Guid crId,
            EstimateDecisionDto dto)
        {
            var cr = await GetCRForUpdateAsync(crId);
            EnsureCurrentStatus(cr, WorkflowStatuses.PendingEstimateApproval);

            var invoice = await _dbContext.Invoices
                .FirstOrDefaultAsync(existingInvoice => existingInvoice.CRID == crId)
                ?? throw new KeyNotFoundException("Invoice not found for this CR");

            if (Matches(dto.Decision, WorkflowDecisions.Approve))
            {
                invoice.State = InvoiceStates.Accepted;
                await ApplyTransitionAsync(
                    cr,
                    WorkflowStatuses.Analysis,
                    WorkflowRoles.Client);
            }
            else if (Matches(dto.Decision, WorkflowDecisions.Reject))
            {
                invoice.State = InvoiceStates.Rejected;
                await ApplyTransitionAsync(
                    cr,
                    WorkflowStatuses.Rejected,
                    WorkflowRoles.Client);
            }
            else
            {
                throw new InvalidOperationException(
                    "Decision must be Approve or Reject");
            }

            await _dbContext.SaveChangesAsync();
            return await BuildResponseAsync(cr);
        }

        public async Task<CRWorkflowResponseDto> ChangeStageAsync(
            Guid crId,
            ChangeStageDto dto)
        {
            var targetStatus = dto.TargetStatus?.Trim() ?? string.Empty;
            if (!AdminStageTargets.Contains(targetStatus))
            {
                throw new InvalidOperationException(
                    "The requested status is not an admin stage transition");
            }

            var cr = await GetCRForUpdateAsync(crId);
            await ApplyTransitionAsync(
                cr,
                targetStatus,
                WorkflowRoles.Admin);

            await _dbContext.SaveChangesAsync();
            return await BuildResponseAsync(cr);
        }

        public async Task<CRWorkflowResponseDto> SubmitClientApprovalAsync(
            Guid crId,
            ClientApprovalDto dto)
        {
            var cr = await GetCRForUpdateAsync(crId);
            EnsureCurrentStatus(cr, WorkflowStatuses.PendingClientApproval);

            if (Matches(dto.Decision, WorkflowDecisions.Approve))
            {
                await ApplyTransitionAsync(
                    cr,
                    WorkflowStatuses.Deployment,
                    WorkflowRoles.Client);
            }
            else if (Matches(dto.Decision, WorkflowDecisions.RequestRework))
            {
                if (string.IsNullOrWhiteSpace(dto.Message))
                {
                    throw new InvalidOperationException(
                        "A rework message is required");
                }

                _dbContext.Details.Add(CreateDetail(
                    crId,
                    string.Empty,
                    dto.Message,
                    WorkflowStatuses.Rework));

                await ApplyTransitionAsync(
                    cr,
                    WorkflowStatuses.Rework,
                    WorkflowRoles.Client);
            }
            else
            {
                throw new InvalidOperationException(
                    "Decision must be Approve or RequestRework");
            }

            await _dbContext.SaveChangesAsync();
            return await BuildResponseAsync(cr);
        }

        public async Task<CRWorkflowResponseDto> GetWorkflowAsync(Guid crId)
        {
            var cr = await _dbContext.CRs
                .AsNoTracking()
                .Include(changeRequest => changeRequest.CurrentStatus)
                .FirstOrDefaultAsync(changeRequest => changeRequest.ID == crId)
                ?? throw new KeyNotFoundException("CR not found");

            return await BuildResponseAsync(cr);
        }

        private async Task ApplyTransitionAsync(
            CR cr,
            string targetStatus,
            string actorRole)
        {
            if (string.IsNullOrWhiteSpace(targetStatus))
            {
                throw new InvalidOperationException("Target status is required");
            }

            var allowedStatuses = SplitValues(cr.CurrentStatus.AvailableStatuses);
            if (!allowedStatuses.Contains(targetStatus))
            {
                throw new InvalidOperationException(
                    $"Cannot transition from '{cr.CurrentStatus.CurrentStatus}' to '{targetStatus}'");
            }

            var allowedRoles = SplitValues(cr.CurrentStatus.AccessedBy);
            if (!allowedRoles.Contains(actorRole))
            {
                throw new UnauthorizedAccessException(
                    $"'{actorRole}' is not allowed to change this status");
            }

            var newStatus = await GetStatusAsync(targetStatus);
            cr.CurrentStatusID = newStatus.ID;
            cr.CurrentStatus = newStatus;
        }

        private async Task<CR> GetCRForUpdateAsync(Guid crId)
        {
            return await _dbContext.CRs
                .Include(changeRequest => changeRequest.CurrentStatus)
                .FirstOrDefaultAsync(changeRequest => changeRequest.ID == crId)
                ?? throw new KeyNotFoundException("CR not found");
        }

        private async Task<CRStatus> GetStatusAsync(string statusName)
        {
            var statuses = await _dbContext.CRStatues.ToListAsync();

            return statuses.FirstOrDefault(status =>
                string.Equals(
                    status.CurrentStatus,
                    statusName,
                    StringComparison.OrdinalIgnoreCase))
                ?? throw new KeyNotFoundException(
                    $"Status '{statusName}' is not configured");
        }

        private async Task<CRWorkflowResponseDto> BuildResponseAsync(CR cr)
        {
            var invoice = await _dbContext.Invoices
                .AsNoTracking()
                .Where(existingInvoice => existingInvoice.CRID == cr.ID)
                .OrderByDescending(existingInvoice => existingInvoice.CreatedTime)
                .FirstOrDefaultAsync();
            var details = await _dbContext.Details
                .AsNoTracking()
                .Where(detail => detail.CRID == cr.ID)
                .OrderBy(detail => detail.UploadedTime)
                .ToListAsync();

            return new CRWorkflowResponseDto
            {
                ID = cr.ID,
                Name = cr.Name,
                Priority = cr.Priority,
                Scope = cr.Scope,
                Description = cr.Description,
                EstimatedManHour = cr.EstimatedManHour,
                ManHourRate = cr.ManHourRate,
                StartDate = cr.StartDate,
                FinishDate = cr.FinishDate,
                ProjectID = cr.ProjectID,
                CurrentStatus = cr.CurrentStatus.CurrentStatus,
                AvailableStatuses = SplitValues(cr.CurrentStatus.AvailableStatuses)
                    .ToList(),
                Invoice = invoice is null
                    ? null
                    : new InvoiceWorkflowDto
                    {
                        ID = invoice.ID,
                        Cost = invoice.Cost,
                        CreatedTime = invoice.CreatedTime,
                        State = invoice.State
                    },
                Details = details.Select(detail => new DetailWorkflowDto
                {
                    ID = detail.ID,
                    Attachment = detail.Attachment,
                    Comment = detail.Comment,
                    State = detail.State,
                    UploadedTime = detail.UploadedTime
                }).ToList()
            };
        }

        private static void ValidateRequest(RequestCRDto dto, Guid clientId)
        {
            if (clientId == Guid.Empty)
            {
                throw new InvalidOperationException("Client ID is required");
            }

            if (dto.ProjectID == Guid.Empty)
            {
                throw new InvalidOperationException("Project ID is required");
            }

            if (string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Priority) ||
                string.IsNullOrWhiteSpace(dto.Scope) ||
                string.IsNullOrWhiteSpace(dto.Description))
            {
                throw new InvalidOperationException(
                    "Name, priority, scope, and description are required");
            }
        }

        private static void ValidateEstimate(EstimateCRDto? estimate)
        {
            if (estimate is null)
            {
                throw new InvalidOperationException(
                    "Estimate data is required when feedback is approved");
            }

            if (estimate.EstimatedManHour <= 0 || estimate.ManHourRate <= 0)
            {
                throw new InvalidOperationException(
                    "Estimated man-hours and man-hour rate must be greater than zero");
            }

            if (estimate.StartDate == default || estimate.FinishDate == default)
            {
                throw new InvalidOperationException(
                    "Estimate start and finish dates are required");
            }

            if (estimate.FinishDate < estimate.StartDate)
            {
                throw new InvalidOperationException(
                    "Finish date cannot be before start date");
            }
        }

        private static void ApplyEstimate(CR cr, EstimateCRDto estimate)
        {
            cr.EstimatedManHour = estimate.EstimatedManHour;
            cr.ManHourRate = estimate.ManHourRate;
            cr.StartDate = estimate.StartDate;
            cr.FinishDate = estimate.FinishDate;
        }

        private static Detail CreateDetail(
            Guid crId,
            string attachment,
            string? comment,
            string state)
        {
            return new Detail
            {
                ID = Guid.NewGuid(),
                CRID = crId,
                Attachment = attachment.Trim(),
                Comment = comment?.Trim() ?? string.Empty,
                State = state,
                UploadedTime = DateTime.UtcNow
            };
        }

        private static void EnsureCurrentStatus(CR cr, string expectedStatus)
        {
            if (!string.Equals(
                cr.CurrentStatus.CurrentStatus,
                expectedStatus,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"This action requires status '{expectedStatus}', but the current status is '{cr.CurrentStatus.CurrentStatus}'");
            }
        }

        private static bool Matches(string? actual, string expected)
        {
            return string.Equals(
                actual?.Trim(),
                expected,
                StringComparison.OrdinalIgnoreCase);
        }

        private static HashSet<string> SplitValues(string values)
        {
            return values
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
    }
}
