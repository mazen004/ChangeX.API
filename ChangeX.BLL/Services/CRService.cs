using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChangeX.BLL.Services
{
    public class CRService : ICRService
    {
        private readonly ApplicationContext _dbContext;

        public CRService(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CR> RequestCRAsync(RequestCRDto dto, Guid clientId)
        {
            if (clientId == Guid.Empty)
            {
                throw new InvalidOperationException("Client ID is required");
            }

            var projectExists = await _dbContext.Projects
                .AsNoTracking()
                .AnyAsync(project =>
                    project.ID == dto.ProjectID && project.ClientID == clientId);

            if (!projectExists)
            {
                throw new KeyNotFoundException(
                    "Project was not found for the specified client");
            }

            var statuses = await _dbContext.CRStatues
                .AsNoTracking()
                .ToListAsync();
            var initialStatus = ResolveInitialStatus(statuses);

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
            return cr;
        }

        public async Task<CR> ChangeStatusAsync(
            Guid crId,
            string targetStatus,
            string actorRole)
        {
            if (string.IsNullOrWhiteSpace(targetStatus))
            {
                throw new InvalidOperationException("Target status is required");
            }

            if (string.IsNullOrWhiteSpace(actorRole))
            {
                throw new InvalidOperationException("Actor role is required");
            }

            var normalizedTarget = targetStatus.Trim();
            var normalizedRole = actorRole.Trim();

            var cr = await _dbContext.CRs
                .Include(changeRequest => changeRequest.CurrentStatus)
                .FirstOrDefaultAsync(changeRequest => changeRequest.ID == crId)
                ?? throw new KeyNotFoundException("CR not found");

            var allowedStatuses = SplitValues(cr.CurrentStatus.AvailableStatuses);
            if (!allowedStatuses.Contains(normalizedTarget))
            {
                throw new InvalidOperationException(
                    $"Cannot transition from '{cr.CurrentStatus.CurrentStatus}' to '{normalizedTarget}'");
            }

            var allowedRoles = SplitValues(cr.CurrentStatus.AccessedBy);
            if (!allowedRoles.Contains(normalizedRole))
            {
                throw new UnauthorizedAccessException(
                    $"'{normalizedRole}' is not allowed to change this status");
            }

            var statuses = await _dbContext.CRStatues.ToListAsync();
            var newStatus = statuses.FirstOrDefault(status =>
                string.Equals(
                    status.CurrentStatus,
                    normalizedTarget,
                    StringComparison.OrdinalIgnoreCase))
                ?? throw new KeyNotFoundException(
                    $"Status '{normalizedTarget}' not found");

            cr.CurrentStatusID = newStatus.ID;
            cr.CurrentStatus = newStatus;
            await _dbContext.SaveChangesAsync();
            return cr;
        }

        public async Task<CR> EstimateCRAsync(Guid crId, EstimateCRDto dto)
        {
            if (dto.EstimatedManHour <= 0 || dto.ManHourRate <= 0)
            {
                throw new InvalidOperationException(
                    "Estimated man-hours and man-hour rate must be greater than zero");
            }

            if (dto.FinishDate < dto.StartDate)
            {
                throw new InvalidOperationException(
                    "Finish date cannot be before start date");
            }

            var cr = await _dbContext.CRs
                .Include(changeRequest => changeRequest.CurrentStatus)
                .FirstOrDefaultAsync(changeRequest => changeRequest.ID == crId)
                ?? throw new KeyNotFoundException("CR not found");

            cr.EstimatedManHour = dto.EstimatedManHour;
            cr.ManHourRate = dto.ManHourRate;
            cr.StartDate = dto.StartDate;
            cr.FinishDate = dto.FinishDate;

            await _dbContext.SaveChangesAsync();
            return cr;
        }

        public async Task<Detail> ClarifyCRAsync(Guid crId, DetailDto dto)
        {
            if (dto.CRID != Guid.Empty && dto.CRID != crId)
            {
                throw new InvalidOperationException(
                    "The CR ID in the request body does not match the route");
            }

            var cr = await _dbContext.CRs
                .Include(changeRequest => changeRequest.CurrentStatus)
                .FirstOrDefaultAsync(changeRequest => changeRequest.ID == crId)
                ?? throw new KeyNotFoundException("CR not found");

            if (string.IsNullOrWhiteSpace(dto.Attachment) &&
                string.IsNullOrWhiteSpace(dto.Comment))
            {
                throw new InvalidOperationException(
                    "A comment or attachment is required");
            }

            var detail = new Detail
            {
                ID = Guid.NewGuid(),
                CRID = crId,
                Attachment = dto.Attachment.Trim(),
                Comment = dto.Comment.Trim(),
                State = cr.CurrentStatus.CurrentStatus,
                UploadedTime = DateTime.UtcNow
            };

            _dbContext.Details.Add(detail);
            await _dbContext.SaveChangesAsync();
            return detail;
        }

        public async Task<Invoice> AcceptEstimateAsync(Guid crId)
        {
            var cr = await _dbContext.CRs
                .AsNoTracking()
                .FirstOrDefaultAsync(changeRequest => changeRequest.ID == crId)
                ?? throw new KeyNotFoundException("CR not found");

            if (cr.EstimatedManHour <= 0 || cr.ManHourRate <= 0)
            {
                throw new InvalidOperationException(
                    "The CR must have a valid estimate before it can be accepted");
            }

            var existingInvoice = await _dbContext.Invoices
                .FirstOrDefaultAsync(invoice => invoice.CRID == crId);

            if (existingInvoice is not null)
            {
                return existingInvoice;
            }

            var invoice = new Invoice
            {
                ID = Guid.NewGuid(),
                CRID = crId,
                Cost = cr.EstimatedManHour * cr.ManHourRate,
                CreatedTime = DateTime.UtcNow,
                State = "Pending"
            };

            _dbContext.Invoices.Add(invoice);
            await _dbContext.SaveChangesAsync();
            return invoice;
        }

        public Task<CR> RejectEstimateAsync(Guid crId)
        {
            return ChangeStatusAsync(crId, "Rejected", "Client");
        }

        private static CRStatus ResolveInitialStatus(IReadOnlyCollection<CRStatus> statuses)
        {
            if (statuses.Count == 0)
            {
                throw new InvalidOperationException("No CR statuses are configured");
            }

            string[] preferredStatuses = ["Pending", "Requested", "Submitted", "New"];
            foreach (var preferredStatus in preferredStatuses)
            {
                var match = statuses.FirstOrDefault(status =>
                    string.Equals(
                        status.CurrentStatus,
                        preferredStatus,
                        StringComparison.OrdinalIgnoreCase));

                if (match is not null)
                {
                    return match;
                }
            }

            var transitionTargets = statuses
                .SelectMany(status => SplitValues(status.AvailableStatuses))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var rootStatuses = statuses
                .Where(status => !transitionTargets.Contains(status.CurrentStatus))
                .ToList();

            return rootStatuses.Count == 1
                ? rootStatuses[0]
                : throw new InvalidOperationException(
                    "An initial CR status could not be determined");
        }

        private static HashSet<string> SplitValues(string values)
        {
            return values
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
    }
}
