using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GithubAccounts.Rules
{
    public class GithubAccountBusinessRules
    {
        private readonly IGithubAccountRepository _githubAccountRepository;
        private readonly IUserRepository _userRepository;

        public GithubAccountBusinessRules(IGithubAccountRepository githubAccountRepository, IUserRepository userRepository)
        {
            _githubAccountRepository = githubAccountRepository;
            _userRepository = userRepository;
        }

        public async Task UserShouldExists(int id)
        {
            User? user = await _userRepository.GetAsync(u => u.Id == id);
            if(user == null)
            {
                throw new BusinessException("User does not exist.");
            }
        }

        public async Task GitHubAccountUrlCanNotDuplicateWhenInserted(string url)
        {
            IPaginate<GithubAccount> accounts = await _githubAccountRepository.GetListAsync(g => g.Url == url);
            if (accounts.Items.Any()) throw new BusinessException("Github account has already added before");
        }

        public async Task GitHubAccountUrlCanNotDuplicateWhenUpdated(string url)
        {
            IPaginate<GithubAccount> accounts = await _githubAccountRepository.GetListAsync(g => g.Url == url);
            if (accounts.Items.Any()) throw new BusinessException("Github account has already added before");
        }
    

        public async Task GithubAccountShouldExist(int id)
        {
            GithubAccount githubAccount = await _githubAccountRepository.GetAsync(g => g.Id == id);
            if(githubAccount == null)
            {
                throw new BusinessException("Github account does not exists");
            }
        }

    }
}
