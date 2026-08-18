using ChangeX.BLL.DTOs;

namespace ChangeX.BLL.Interfaces
{
    public interface ICRService
    {
        Task<CRWorkflowResponseDto> RequestCRAsync(RequestCRDto dto, Guid clientId);
        Task<CRWorkflowResponseDto> SubmitAdminFeedbackAsync(
            Guid crId,
            AdminFeedbackDto dto);
        Task<CRWorkflowResponseDto> SubmitClarificationAsync(
            Guid crId,
            DetailDto dto);
        Task<CRWorkflowResponseDto> SubmitEstimateDecisionAsync(
            Guid crId,
            EstimateDecisionDto dto);
        Task<CRWorkflowResponseDto> ChangeStageAsync(Guid crId, ChangeStageDto dto);
        Task<CRWorkflowResponseDto> SubmitClientApprovalAsync(
            Guid crId,
            ClientApprovalDto dto);
        Task<CRWorkflowResponseDto> GetWorkflowAsync(Guid crId);
    }
}
