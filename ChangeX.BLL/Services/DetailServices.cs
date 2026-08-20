using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChangeX.BLL.Services
{
    public class DetailServices : IDetailServices
    {
        private readonly ApplicationContext dbcontext;

        public DetailServices(ApplicationContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<ServiceResponse<IEnumerable<Detail>>> GetAll(
            Expression<Func<Detail, bool>>? predicate)
        {
            IQueryable<Detail> query = dbcontext.Details
                .AsNoTracking()
                .Include(d => d.CR)
                    .ThenInclude(cr => cr.CurrentStatus)
                .Include(d => d.CR)
                    .ThenInclude(cr => cr.Project);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var details = await query.ToListAsync();
            return ServiceResponse<IEnumerable<Detail>>.Ok(details, "Get all details");
        }

        public async Task<ServiceResponse<Detail>> GetByID(Guid ID)
        {
            var detail = await dbcontext.Details
                .AsNoTracking()
                .Include(d => d.CR)
                    .ThenInclude(cr => cr.CurrentStatus)
                .Include(d => d.CR)
                    .ThenInclude(cr => cr.Project)
                .FirstOrDefaultAsync(d => d.ID == ID);

            if (detail == null)
            {
                return ServiceResponse<Detail>.Fail("Detail not found.", 404);
            }

            return ServiceResponse<Detail>.Ok(detail, "Detail found");
        }

        public async Task<ServiceResponse<Detail>> Create(Detail detail)
        {
            var cr = await dbcontext.CRs
                .AsNoTracking()
                .Include(cr => cr.CurrentStatus)
                .FirstOrDefaultAsync(cr => cr.ID == detail.CRID);

            if (cr == null)
            {
                return ServiceResponse<Detail>.Fail("CR not found.", 404);
            }

            detail.State = cr.CurrentStatus.CurrentStatus;
            detail.UploadedTime = DateTime.Now;

            await dbcontext.Details.AddAsync(detail);
            await dbcontext.SaveChangesAsync();

            return ServiceResponse<Detail>.Ok(detail, "Detail created successfully");
        }

        public async Task<ServiceResponse<Detail>> Update(Detail detail)
        {
            var existingDetail = await dbcontext.Details.FindAsync(detail.ID);
            if (existingDetail == null)
            {
                return ServiceResponse<Detail>.Fail("Detail not found.", 404);
            }

            var cr = await dbcontext.CRs
                .AsNoTracking()
                .Include(cr => cr.CurrentStatus)
                .FirstOrDefaultAsync(cr => cr.ID == detail.CRID);

            if (cr == null)
            {
                return ServiceResponse<Detail>.Fail("CR not found.", 404);
            }

            existingDetail.CRID = detail.CRID;
            existingDetail.Attachment = detail.Attachment;
            existingDetail.Comment = detail.Comment;
            existingDetail.State = cr.CurrentStatus.CurrentStatus;

            await dbcontext.SaveChangesAsync();

            return ServiceResponse<Detail>.Ok(existingDetail, "Detail updated successfully");
        }

        public async Task<ServiceResponse<bool>> Delete(Guid ID)
        {
            var detail = await dbcontext.Details.FindAsync(ID);
            if (detail == null)
            {
                return ServiceResponse<bool>.Fail("Detail not found.", 404);
            }

            dbcontext.Details.Remove(detail);
            await dbcontext.SaveChangesAsync();

            return ServiceResponse<bool>.Ok(true, "Detail deleted successfully");
        }
    }
}
