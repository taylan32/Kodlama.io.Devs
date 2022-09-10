using Application.Features.GithubAccounts.Dtos;
using Application.Features.GithubAccounts.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GithubAccounts.Commands.CreateGithubAccount
{
    public class CreateGithubAccountCommand : IRequest<CreatedGithubAccountDto>
    {
        public int UserId { get; set; }
        public string Url { get; set; }

        public class CreateGithubAccountCommandHandler : IRequestHandler<CreateGithubAccountCommand, CreatedGithubAccountDto>
        {
            private readonly IGithubAccountRepository _githubAccountRepository;
            private readonly IMapper _mapper;
            private readonly GithubAccountBusinessRules _githubAccountBusinessRules;

            public CreateGithubAccountCommandHandler(IGithubAccountRepository githubAccountRepository, IMapper mapper, GithubAccountBusinessRules githubAccountBusinessRules)
            {
                _githubAccountRepository = githubAccountRepository;
                _mapper = mapper;
                _githubAccountBusinessRules = githubAccountBusinessRules;
            }

            public async Task<CreatedGithubAccountDto> Handle(CreateGithubAccountCommand request, CancellationToken cancellationToken)
            {
                await _githubAccountBusinessRules.UserShouldExists(request.UserId);
                await _githubAccountBusinessRules.GitHubAccountUrlCanNotDuplicateWhenInserted(request.Url);

                GithubAccount mappedGithubAccount = _mapper.Map<GithubAccount>(request);
                GithubAccount createdGithubAccount = await _githubAccountRepository.AddAsync(mappedGithubAccount);
                CreatedGithubAccountDto createdGithubAccountDto = _mapper.Map<CreatedGithubAccountDto>(createdGithubAccount);

                return createdGithubAccountDto;
            }
        }
    }
}
