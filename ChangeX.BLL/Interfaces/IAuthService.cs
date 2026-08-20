using ChangeX.BLL.DTOs.Users;
using ChangeX.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Services
{
    public interface IAuthService
    {
        public Task<string> Login(User User);
    }
}
