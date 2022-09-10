using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Core.Security.Entities;
using Core.Security.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentications.Rules
{
    public class AuthenticationBusinessRules
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationBusinessRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task EmailCanNotDuplicatedWhenRegistered(string email)
        {
            IPaginate<User> users = await _userRepository.GetListAsync(u => u.Email == email);
            if (users.Items.Any()) throw new BusinessException("Email already in use.");
        }

        public async Task EmailAndPasswordShouldMatchWhenLoggedIn(string email, string password)
        {

            User? user = await _userRepository.GetAsync(u => u.Email == email);

            if (user == null || !HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new AuthorizationException("Email or password is wrong");
            }
        }
        public async Task UserShouldExist(string email)
        {
            User? user = await _userRepository.GetAsync(u => u.Email == email);
            if (user == null)
            {
                throw new BusinessException("User does not exist.");
            }
        }

    }
}
