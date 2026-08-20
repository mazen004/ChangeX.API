using AutoMapper;
using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;

namespace ChangeX.BLL.Mapping
{
    public class MappingClientProfile : Profile
    {
        public MappingClientProfile()
        {
            CreateMap<ClientDto, Client>().ReverseMap();

        }
    }
}