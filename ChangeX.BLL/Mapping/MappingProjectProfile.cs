using ChangeX.DAL.Entities;
using ChangeX.BLL.DTOs;
using AutoMapper;

namespace ChangeX.BLL.Mapping
{
    public class MappingProjectProfile : Profile
    {
        public MappingProjectProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<CreateProjectDto, Project>();
        }
    }
}
