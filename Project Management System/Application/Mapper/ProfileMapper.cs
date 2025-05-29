using Application.Dto;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentRegDto, Student>().ReverseMap();
            CreateMap<ProjectGroupCreateDto, ProjectGroup>().ReverseMap();
                
        }
    }
}
