using AutoMapper;
using Core_Entity;
using Core_Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.AutoMapper.Profiles
{
    public class GroupProfile : Profile, IProfile
    {
        public GroupProfile()
        {
            CreateMap<Group, Dto_Group>()
                .ForMember(p => p.GroupName, opt => opt.MapFrom(s => s.Name))
                ;
            CreateMap<Dto_Group, Group>()
                .ForMember(p => p.Name, opt => opt.MapFrom(s => s.GroupName));
            ;
        }
    }
}
