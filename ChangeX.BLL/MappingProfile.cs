using AutoMapper;
using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChangeX.BLL.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClientDto, Client>().ReverseMap();
        }
    }
}