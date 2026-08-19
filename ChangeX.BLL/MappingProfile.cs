using AutoMapper;
using ChangeX.BLL.DTOs;
using ChangeX.BLL.DTOs.Users;
using ChangeX.DAL.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChangeX.BLL.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClientDto, Client>().ReverseMap();
            CreateMap<User, UserAccountDto>()
                .ForMember(
                    dest => dest.ClientName,
                    opt => opt.MapFrom(src => src.Client.Name)
                );

            CreateMap<User, UserInClientDto>();

            // DTOs -> User Entity
            CreateMap<AddUserDto, User>();

            CreateMap<UpdateUserDto, User>();

        }
    }
    //public class UserProfile : Profile
    //{
    //    public UserProfile()
    //    {
    //        CreateMap<User, UserAccountDto>()
    //            .ForMember(
    //                dest => dest.ClientName,
    //                opt => opt.MapFrom(src => src.Client.Name)
    //            );

    //        CreateMap<User, UserInClientDto>();
    //    }
    //}
}