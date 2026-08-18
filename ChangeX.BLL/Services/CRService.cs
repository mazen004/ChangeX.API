using ChangeX.DAL.Database;
using ChangeX.DAL.Entities;
// using ChangeX.BLL.DTOs;
using ChangeX.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ChangeX.BLL.DTOs;

namespace ChangeX.BLL.Services
{
    
    public class CRService : ICRService
    {
        private readonly ApplicationContext DBContext;

        public CRService(ApplicationContext DbContext)
        {
            DBContext = DbContext;
        }

        public Task<Invoice> AcceptEstimateAsync(Guid crId)
        {
            throw new NotImplementedException();
        }

        public async Task<CR> ChangeStatusAsync(Guid crId, string targetStatus, string actorRole)
        {
            var cr = await DBContext.CRs
                .Include(c => c.CurrentStatus)
                .FirstOrDefaultAsync(c => c.ID == crId)
                ?? throw new KeyNotFoundException("CR not found");

            var current = cr.CurrentStatus;
            var allowed = current.AvailableStatuses
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();

            if (!allowed.Contains(targetStatus))
                throw new InvalidOperationException(
                    $"Cannot transition from '{current.CurrentStatus}' to '{targetStatus}'");

            if (!string.Equals(current.AccessedBy, actorRole, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException(
                    $"'{actorRole}' is not allowed to change this status");

            var newStatus = await DBContext.CRStatues
                .FirstOrDefaultAsync(s => s.CurrentStatus == targetStatus)
                ?? throw new KeyNotFoundException($"Status '{targetStatus}' not found");

            cr.CurrentStatusID = newStatus.ID;
            await DBContext.SaveChangesAsync();
            return cr;
        }

        public Task<Detail> ClarifyCRAsync(Guid crId, DetailDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<CR> EstimateCRAsync(Guid crId, EstimateCRDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<CR> RejectEstimateAsync(Guid crId)
        {
            throw new NotImplementedException();
        }

        public Task<CR> RequestCRAsync(RequestCRDto dto, Guid clientId)
        {
            throw new NotImplementedException();
        }
    }
}
