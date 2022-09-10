using Application.Features.Authentications.Commands.Register;
using Application.Features.Authentications.Dtos;
using AutoMapper;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentications.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, RegisterCommand>().ReverseMap();
            CreateMap<User, RegisteredUserDto>().ReverseMap();
        }
    }
}
