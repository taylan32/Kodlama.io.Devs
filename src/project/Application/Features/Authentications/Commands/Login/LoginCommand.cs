using Application.Features.Authentications.Dtos;
using Application.Features.Authentications.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Persistence.Paging;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentications.Commands.Login
{
    public class LoginCommand : IRequest<LoggedInUserDto>
    {
        public UserForLoginDto UserForLoginDto { get; set; }
        public string IpAddress { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoggedInUserDto>
        {
            private readonly IUserRepository _userRepository;
           // private readonly IMapper _mapper;
            private readonly AuthenticationBusinessRules _authenticationBusinessRules;
            private readonly ITokenHelper _tokenHelper;
            private readonly IUserOperationClaimRepository _userOperationClaimRepository;

            public LoginCommandHandler(IUserRepository userRepository, AuthenticationBusinessRules authenticationBusinessRules, ITokenHelper tokenHelper, IUserOperationClaimRepository userOperationClaimRepository)
            {
                _userRepository = userRepository;
                _authenticationBusinessRules = authenticationBusinessRules;
                _tokenHelper = tokenHelper;
                _userOperationClaimRepository = userOperationClaimRepository;
            }

            public async Task<LoggedInUserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
            {

                await _authenticationBusinessRules.UserShouldExist(request.UserForLoginDto.Email);
                await _authenticationBusinessRules.EmailAndPasswordShouldMatchWhenLoggedIn(request.UserForLoginDto.Email, request.UserForLoginDto.Password);


                User? user = await _userRepository.GetAsync(u => u.Email == request.UserForLoginDto.Email);
                IPaginate<UserOperationClaim> userOperationClaims = await _userOperationClaimRepository.GetListAsync(c => c.UserId == user.Id);
                IList<OperationClaim> operationClaims = new List<OperationClaim>();
                foreach (UserOperationClaim claim in userOperationClaims.Items)
                {
                    operationClaims.Add(claim.OperationClaim);
                }
                
                AccessToken accessToken = _tokenHelper.CreateToken(user, operationClaims);
                RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, request.IpAddress);

                LoggedInUserDto loggedInUserDto = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                return loggedInUserDto;
            }
        }

    }
}
