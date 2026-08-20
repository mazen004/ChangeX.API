using ChangeX.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Interfaces
{
    public interface IStatusService
    {
        Task<CRStatus> GetCurrentStatus(Guid CRID);
        Task<List<Guid>> GetAvailableStatus(Guid CRID);
     
    }
}
