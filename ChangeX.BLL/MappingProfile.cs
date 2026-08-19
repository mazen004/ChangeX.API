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

            CreateMap<CRDto, CR>();
            CreateMap<CR, CRResponseDto>()
                .ForMember(dest => dest.CurrentStatusName, opt => opt.MapFrom(src => src.CurrentStatus != null ? src.CurrentStatus.CurrentStatus : string.Empty))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : string.Empty));
        }
    }
}