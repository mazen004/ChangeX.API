//using ChangeX.DAL.Database;
//using ChangeX.DAL.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace ChangeX.BLL.StatusMachine
//{
//    public static class StatusWorkflowSeeder
//    {
//        public static async Task<int> SeedAsync(ApplicationContext dbContext)
//        {
//            var existingStatuses = await dbContext.CRStatues
//                .AsNoTracking()
//                .Select(status => status.CurrentStatus)
//                .ToListAsync();
//            var existingNames = existingStatuses
//                .ToHashSet(StringComparer.OrdinalIgnoreCase);

//            var missingStatuses = GetStatusDefinitions()
//                .Where(status => !existingNames.Contains(status.CurrentStatus))
//                .ToList();

//            if (missingStatuses.Count == 0)
//            {
//                return 0;
//            }

//            dbContext.CRStatues.AddRange(missingStatuses);
//            await dbContext.SaveChangesAsync();
//            return missingStatuses.Count;
//        }

//        private static IReadOnlyCollection<CRStatus> GetStatusDefinitions()
//        {
//            return
//            [
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd001",
//                    WorkflowStatuses.PendingVendorFeedback,
//                    Join(
//                        WorkflowStatuses.PendingEstimateApproval,
//                        WorkflowStatuses.Rejected,
//                        WorkflowStatuses.ClarificationRequested),
//                    WorkflowRoles.Admin),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd002",
//                    WorkflowStatuses.ClarificationRequested,
//                    WorkflowStatuses.PendingVendorFeedback,
//                    WorkflowRoles.Client),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd003",
//                    WorkflowStatuses.PendingEstimateApproval,
//                    Join(WorkflowStatuses.Analysis, WorkflowStatuses.Rejected),
//                    WorkflowRoles.Client),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd004",
//                    WorkflowStatuses.Analysis,
//                    WorkflowStatuses.Design,
//                    WorkflowRoles.Admin),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd005",
//                    WorkflowStatuses.Design,
//                    WorkflowStatuses.Development,
//                    WorkflowRoles.Admin),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd006",
//                    WorkflowStatuses.Development,
//                    WorkflowStatuses.Testing,
//                    WorkflowRoles.Admin),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd007",
//                    WorkflowStatuses.Testing,
//                    WorkflowStatuses.PendingClientApproval,
//                    WorkflowRoles.Admin),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd008",
//                    WorkflowStatuses.PendingClientApproval,
//                    Join(WorkflowStatuses.Deployment, WorkflowStatuses.Rework),
//                    WorkflowRoles.Client),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd009",
//                    WorkflowStatuses.Rework,
//                    WorkflowStatuses.Analysis,
//                    WorkflowRoles.Admin),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd010",
//                    WorkflowStatuses.Deployment,
//                    WorkflowStatuses.Delivered,
//                    WorkflowRoles.Admin),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd011",
//                    WorkflowStatuses.Delivered,
//                    string.Empty,
//                    WorkflowRoles.Admin),
//                Create(
//                    "84f994d5-4b1d-4c3f-96f4-795251bcd012",
//                    WorkflowStatuses.Rejected,
//                    string.Empty,
//                    string.Empty)
//            ];
//        }

//        private static CRStatus Create(
//            string id,
//            string currentStatus,
//            string availableStatuses,
//            string accessedBy)
//        {
//            return new CRStatus
//            {
//                ID = Guid.Parse(id),
//                CurrentStatus = currentStatus,
//                AvailableStatuses = availableStatuses,
//                AccessedBy = accessedBy
//            };
//        }

//        private static string Join(params string[] statuses)
//        {
//            return string.Join(',', statuses);
//        }
//    }
//}
