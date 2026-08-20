
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

        public async Task<IEnumerable<CR>> GetAll(Expression<Func<CR, bool>>? predicate)
        {
            IQueryable<CR> query = dbcontext.CRs
                .AsNoTracking()
                .Include(c => c.CurrentStatus)
                .Include(c => c.Project);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync();
        }

        public async Task<CR?> GetByID(Guid ID)
        {
            return await dbcontext.CRs
                .AsNoTracking()
                .Include(c => c.CurrentStatus)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(c => c.ID == ID);
        }

        public async Task<CR> Create(CR cr)
        {
           await dbcontext.CRs.AddAsync(cr);
           await dbcontext.SaveChangesAsync();
           return cr;
        }

        public async Task<CR> Update(CR cr)
        {
           var existingCr = await dbcontext.CRs.FindAsync(cr.ID);
           if (existingCr == null)
           {
               throw new ArgumentException("CR not found");
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
           return existingCr;
        }

        public async Task Delete(Guid ID)
        {   
            var cr = await dbcontext.CRs.FindAsync(ID);
            if (cr == null)
            {
                throw new ArgumentException("cr not found");
            }
            dbcontext.Remove(cr);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<CR> ChangeStatus( Guid TargetStatusID,CR currentCR)
        {
            var currentStatus = currentCR.CurrentStatus;

            var availableStatus = currentStatus.AvailableStatusIDs.Split(",").Select(Guid.Parse).ToList();

            if ( availableStatus==null) {
                throw new Exception($"Status not found.");
            }
            if ( currentCR==null) {
                throw new Exception($"CR not found.");
            }

            foreach (var status in availableStatus)
            {
                if (status == TargetStatusID)
                {
                 currentCR.CurrentStatusID=TargetStatusID; 
                   
                }
            }
            if(currentCR.CurrentStatusID != TargetStatusID)
            {
                //return new Exception("Target status not accessible");
            }
            return currentCR;

        }
    }
}


