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

namespace Application.Features.GithubAccounts.Commands.UpdateGithubAccount
{
    public class UpdateGithubAccountCommand : IRequest<UpdatedGithubAccountDto>
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public class UpdateGithubAccountCommandHandler : IRequestHandler<UpdateGithubAccountCommand, UpdatedGithubAccountDto>
        {
            private readonly IGithubAccountRepository _githubAccountRepository;
            private readonly IMapper _mapper;
            private readonly GithubAccountBusinessRules _githubAccountBusinessRules;

            public UpdateGithubAccountCommandHandler(IGithubAccountRepository githubAccountRepository, IMapper mapper, GithubAccountBusinessRules githubAccountBusinessRules)
            {
                _githubAccountRepository = githubAccountRepository;
                _mapper = mapper;
                _githubAccountBusinessRules = githubAccountBusinessRules;
            }

            public async Task<UpdatedGithubAccountDto> Handle(UpdateGithubAccountCommand request, CancellationToken cancellationToken)
            {
                await _githubAccountBusinessRules.GithubAccountShouldExist(request.Id);
                await _githubAccountBusinessRules.GitHubAccountUrlCanNotDuplicateWhenUpdated(request.Url);

                GithubAccount githubAccount = await _githubAccountRepository.GetAsync(g => g.Id == request.Id);
                _mapper.Map(request, githubAccount);
                GithubAccount updatedGithubAccount = await _githubAccountRepository.UpdateAsync(githubAccount);
                UpdatedGithubAccountDto updatedGithubAccountDto = _mapper.Map<UpdatedGithubAccountDto>(updatedGithubAccount);


                return updatedGithubAccountDto;
            }
        }

    }
}
