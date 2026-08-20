using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Services
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationContext dbcontext;

        public StatusService(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        
        public async Task<CRStatus> GetCurrentStatus(Guid CRID)
        {
            var cr = await dbcontext.CRs
                .AsNoTracking()
                .Include(cr => cr.CurrentStatus)
                .FirstOrDefaultAsync(cr => cr.ID == CRID);

            if (cr == null)
            {
                throw new KeyNotFoundException("CR not found.");
            }

            return cr.CurrentStatus;
        }

        public async Task<List<Guid>> GetAvailableStatus(Guid CRID)
        {
            var status = await GetCurrentStatus(CRID);

            if (string.IsNullOrWhiteSpace(status.AvailableStatusIDs))
            {
                return [];
            }

            return status.AvailableStatusIDs
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(Guid.Parse)
                .ToList();
        }
        
    }
}
