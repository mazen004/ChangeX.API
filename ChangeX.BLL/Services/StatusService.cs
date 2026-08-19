using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Services
{
    public class StatusService
    {
        private readonly ApplicationContext dbcontext;

        public StatusService(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        
        public async Task<CRStatus> GetCurrentStatus(Guid ID)
        {
            var Status = await dbcontext.CRStatues
                .Where(c => c.ID == ID)
                .FirstOrDefaultAsync();
            if (Status == null)
                throw new Exception($"Status not found.");
            return Status;
        }
        public async Task<string[]> GetAvailableStatus(Guid ID)
        {
            var Status = await dbcontext.CRStatues
               .Where(c => c.ID == ID)
               .FirstOrDefaultAsync();
            return Status.AvailableStatusIDs.Split(",");
        }
        //public async Task<CRStatus> ChangeStatus(CRStatus status)
        //{

        //}
    }
}
