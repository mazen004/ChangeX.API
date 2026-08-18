namespace ChangeX.BLL.StatusMachine
{
    public static class WorkflowStatuses
    {
        public const string PendingVendorFeedback = "PendingVendorFeedback";
        public const string ClarificationRequested = "ClarificationRequested";
        public const string PendingEstimateApproval = "PendingEstimateApproval";
        public const string Analysis = "Analysis";
        public const string Design = "Design";
        public const string Development = "Development";
        public const string Testing = "Testing";
        public const string PendingClientApproval = "PendingClientApproval";
        public const string Rework = "Rework";
        public const string Deployment = "Deployment";
        public const string Delivered = "Delivered";
        public const string Rejected = "Rejected";
    }

    public static class WorkflowDecisions
    {
        public const string Approve = "Approve";
        public const string Reject = "Reject";
        public const string RequestClarification = "RequestClarification";
        public const string RequestRework = "RequestRework";
    }

    public static class WorkflowRoles
    {
        public const string Admin = "Admin";
        public const string Client = "Client";
    }

    public static class InvoiceStates
    {
        public const string Pending = "Pending";
        public const string Accepted = "Accepted";
        public const string Rejected = "Rejected";
    }
}
