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
            var Status = dbcontext.CRStatues.Where(c => c.ID == ID);
                
            if (Status == null)
                throw new Exception($"Status not found.");
            return await Status.FirstOrDefaultAsync();
        }
        public async Task<List<Guid>> GetAvailableStatus(Guid ID)
        {
            var Status = await dbcontext.CRStatues
               .Where(c => c.ID == ID)
               .FirstOrDefaultAsync();
            return Status.AvailableStatusIDs.Split(",").Select(Guid.Parse).ToList();
        }
        
    }
}
