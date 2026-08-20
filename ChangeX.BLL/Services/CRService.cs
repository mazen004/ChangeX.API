
using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChangeX.BLL.Services
{
    public class CRService : ICRServices
    {
        private readonly ApplicationContext dbcontext;

        public CRService(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<ServiceResponse<IEnumerable<CR>>> GetAll(Expression<Func<CR, bool>>? predicate)
        {
            IQueryable<CR> query = dbcontext.CRs
                .AsNoTracking()
                .Include(c => c.CurrentStatus)
                .Include(c => c.Project);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            var result = await query.ToListAsync();
            return ServiceResponse<IEnumerable<CR>>.Ok(result, "Get all CRs");
        }

        public async Task<ServiceResponse<CR>> GetByID(Guid ID)
        {
            var cr = await dbcontext.CRs
                .AsNoTracking()
                .Include(c => c.CurrentStatus)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(c => c.ID == ID);
            if (cr == null)
                return ServiceResponse<CR>.Fail("CR not found.", 404);
            return ServiceResponse<CR>.Ok(cr, "CR found");
        }

        public async Task<ServiceResponse<CR>> Create(CR cr)
        {
           await dbcontext.CRs.AddAsync(cr);
           await dbcontext.SaveChangesAsync();
           return ServiceResponse<CR>.Ok(cr, "CR created successfully");
        }

        public async Task<ServiceResponse<CR>> Update(CR cr)
        {
           var existingCr = await dbcontext.CRs.FindAsync(cr.ID);
           if (existingCr == null)
           {
               return ServiceResponse<CR>.Fail("CR not found", 404);
           }

            existingCr.Name = cr.Name;
            existingCr.Priority = cr.Priority;
            existingCr.Scope = cr.Scope;
            existingCr.Description = cr.Description;
            existingCr.EstimatedManHour = cr.EstimatedManHour;
            existingCr.ManHourRate = cr.ManHourRate;
            existingCr.StartDate = cr.StartDate;
            existingCr.FinishDate = cr.FinishDate;
            existingCr.CurrentStatusID = cr.CurrentStatusID;
            existingCr.ProjectID = cr.ProjectID;

            await dbcontext.SaveChangesAsync();
           return ServiceResponse<CR>.Ok(existingCr, "CR updated successfully");
        }

        public async Task<ServiceResponse<bool>> Delete(Guid ID)
        {   
            var cr = await dbcontext.CRs.FindAsync(ID);
            if (cr == null)
            {
                return ServiceResponse<bool>.Fail("CR not found", 404);
            }
            dbcontext.Remove(cr);
            await dbcontext.SaveChangesAsync();
            return ServiceResponse<bool>.Ok(true, "CR deleted successfully");
        }

        public async Task<ServiceResponse<CR>> ChangeStatus( Guid TargetStatusID,CR currentCR)
        {
            var currentStatus = currentCR.CurrentStatus;

            var availableStatus = currentStatus.AvailableStatusIDs.Split(",").Select(Guid.Parse).ToList();

            if ( availableStatus==null) {
                return ServiceResponse<CR>.Fail("Status not found.");
            }
            if ( currentCR==null) {
                return ServiceResponse<CR>.Fail("CR not found.");
            }

            foreach (var status in availableStatus)
            {
                if (status == TargetStatusID)
                {
                 currentCR.CurrentStatusID=TargetStatusID; 
                 currentCR.CurrentStatus = await dbcontext.CRStatues.FindAsync(TargetStatusID);
                }
            }
            if(currentCR.CurrentStatusID != TargetStatusID)
            {
                return ServiceResponse<CR>.Fail("Target status not accessible");
            }
            await dbcontext.SaveChangesAsync();
            return ServiceResponse<CR>.Ok(currentCR, "CR status changed successfully");

        }
    }
}


