using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChangeX.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [Route("api/CRAdmin/{crId}/accept")]
    [Route("api/CRAdmin/{crId}/reject")]
    [Route("api/CRAdmin/{crId}/request-clarification")]
    [ApiController]
    public class RequestClarificationController : ControllerBase
    {
        // AcceptCR
        async Task<await> crService.ChangeStatusAsync(crId, "Accepted", "Admin");

        // RejectCR
        async Task<await> crService.ChangeStatusAsync(crId, "Rejected", "Admin");

        // RequestClarification
        async Task<await> crService.ChangeStatusAsync(crId, "ClarificationRequested", "Admin");
    }

    internal class await
    {

    }
}
