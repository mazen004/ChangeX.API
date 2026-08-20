using AutoMapper;
using ChangeX.BLL.DTOs.Users;
using ChangeX.DAL.Entities;

namespace ChangeX.BLL.Mapping
{
    public class MappingUserProfile : Profile
    {
        public MappingUserProfile()
        {
            CreateMap<User, UserAccountDto>()
                .ForMember(
                    dest => dest.ClientName,
                    opt => opt.MapFrom(src => src.Client.Name)
                );

            CreateMap<User, UserInClientDto>();
            CreateMap<AddUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<LoginDto, User>();
        }
    }
}
