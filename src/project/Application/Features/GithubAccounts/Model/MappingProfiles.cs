using Application.Features.GithubAccounts.Commands.CreateGithubAccount;
using Application.Features.GithubAccounts.Commands.DeleteGithubAccount;
using Application.Features.GithubAccounts.Commands.UpdateGithubAccount;
using Application.Features.GithubAccounts.Dtos;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GithubAccounts.Model
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<GithubAccount, CreateGithubAccountCommand>().ReverseMap();
            CreateMap<GithubAccount, CreatedGithubAccountDto>().ReverseMap();
            CreateMap<GithubAccount, UpdatedGithubAccountDto>().ReverseMap();
            CreateMap<GithubAccount, UpdateGithubAccountCommand>().ReverseMap();
            CreateMap<GithubAccount, DeleteGithubAccountCommand>().ReverseMap();
            CreateMap<GithubAccount, DeletedGithubAccountDto>().ReverseMap();
        }
    }
}
