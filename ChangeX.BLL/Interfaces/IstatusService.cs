using ChangeX.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Interfaces
{
    internal interface IstatusService
    {
        Task<CRStatus> GetCurrentStatus(Guid ID);
        Task<CRStatus> GetAvailableStatus(Guid ID);
        Task<CRStatus> ChangeStatus(CRStatus status);
    }
}
