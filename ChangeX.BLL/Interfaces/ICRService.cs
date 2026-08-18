using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Interfaces
{
    public interface ICRService
    {
        Task<CR> RequestCRAsync(RequestCRDto dto, Guid clientId);
        Task<CR> ChangeStatusAsync(Guid crId, string targetStatus, string actorRole);
        Task<CR> EstimateCRAsync(Guid crId, EstimateCRDto dto);
        Task<Detail> ClarifyCRAsync(Guid crId, DetailDto dto);
        Task<Invoice> AcceptEstimateAsync(Guid crId);
        Task<CR> RejectEstimateAsync(Guid crId);
    }
}
