using AutoMapper;
using ChangeX.BLL.DTOs;
using ChangeX.DAL.Entities;

namespace ChangeX.BLL.Mapping
{
    public class MappingDetailProfile : Profile
    {
        public MappingDetailProfile()
        {
            CreateMap<DetailDto, Detail>()
                .ForMember(destination => destination.ID, options => options.Ignore())
                .ForMember(destination => destination.CR, options => options.Ignore())
                .ForMember(destination => destination.State, options => options.Ignore())
                .ForMember(destination => destination.UploadedTime, options => options.Ignore());
        }
    }
}
