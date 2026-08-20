using AutoMapper;
using ChangeX.BLL.DTOs;
using ChangeX.BLL.DTOs.Users;
using ChangeX.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Mapping
{
    public class MappingCRProfile :  Profile
    {
        public MappingCRProfile()
        {
            CreateMap<CR, CRDto>();
            CreateMap<CR, CRResponseDto>();
        }
    }
}
