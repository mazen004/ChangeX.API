<<<<<<< HEAD
﻿//using ChangeX.DAL.Database;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
=======
using ChangeX.DAL.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
>>>>>>> 0179ccc73548077430d1bc7acfd45b3f7302ce2e

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

<<<<<<< HEAD
//            var availableStatuses = currentStatus.AvailableStatuses
//                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
//                .ToList();
=======
            var availableStatuses = (currentStatus.AvailableStatusIDs ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
>>>>>>> 0179ccc73548077430d1bc7acfd45b3f7302ce2e

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

<<<<<<< HEAD
//            var availableStatuses = cr.CurrentStatus.AvailableStatuses
//                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
//                .ToList();
=======
            var availableStatuses = (cr.CurrentStatus?.AvailableStatusIDs ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
>>>>>>> 0179ccc73548077430d1bc7acfd45b3f7302ce2e

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
