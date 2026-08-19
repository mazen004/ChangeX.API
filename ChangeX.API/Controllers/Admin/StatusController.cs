//using ChangeX.DAL.Database;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace ChangeX.API.Controllers.Admin
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StatusController : ControllerBase
//    {
//        private readonly ApplicationContext _dbContext;

//        public StatusController(ApplicationContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        [HttpGet("{id:guid}")]
//        public async Task<IActionResult> GetAvailableStatus(Guid id)
//        {
//            var currentStatus = await _dbContext.CRStatues
//                .AsNoTracking()
//                .FirstOrDefaultAsync(status => status.ID == id);

//            if (currentStatus is null)
//            {
//                return NotFound(new { message = "No status found" });
//            }

//            var availableStatuses = currentStatus.AvailableStatuses
//                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
//                .ToList();

//            return Ok(new
//            {
//                id = currentStatus.ID,
//                currentStatus = currentStatus.CurrentStatus,
//                availableStatuses,
//                accessedBy = currentStatus.AccessedBy
//            });
//        }

//        [HttpGet("cr/{crId:guid}")]
//        public async Task<IActionResult> GetAvailableStatusByCR(Guid crId)
//        {
//            var cr = await _dbContext.CRs
//                .AsNoTracking()
//                .Include(changeRequest => changeRequest.CurrentStatus)
//                .FirstOrDefaultAsync(changeRequest => changeRequest.ID == crId);

//            if (cr is null)
//            {
//                return NotFound(new { message = "CR not found" });
//            }

//            var availableStatuses = cr.CurrentStatus.AvailableStatuses
//                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
//                .ToList();

//            return Ok(new
//            {
//                crId = cr.ID,
//                currentStatus = cr.CurrentStatus.CurrentStatus,
//                availableStatuses,
//                accessedBy = cr.CurrentStatus.AccessedBy
//            });
//        }
//    }
//}
