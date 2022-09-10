using Application.Features.Authentications.Dtos;
using Application.Features.Authentications.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Enums;
using Core.Security.Hashing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentications.Commands.Register
{
    public class RegisterCommand : IRequest<RegisteredUserDto>
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public string IpAddress { get; set; }

        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly AuthenticationBusinessRules _authenticationBusinessRules;

            public RegisterCommandHandler(IUserRepository userRepository, IMapper mapper, AuthenticationBusinessRules authenticationBusinessRules)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _authenticationBusinessRules = authenticationBusinessRules;
            }

            public async Task<RegisteredUserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                await _authenticationBusinessRules.EmailCanNotDuplicatedWhenRegistered(request.UserForRegisterDto.Email);

                byte[] passwordHash, passwordSalt;

                HashingHelper.CreatePasswordHash(request.UserForRegisterDto.Password, out passwordHash, out passwordSalt);

                User user = new User
                {
                    Email = request.UserForRegisterDto.Email,
                    FirstName = request.UserForRegisterDto.FirstName,
                    LastName = request.UserForRegisterDto.LastName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true,
                    AuthenticatorType = AuthenticatorType.Email
                };

                User createdUser = await _userRepository.AddAsync(user);

                RegisteredUserDto mappedUser = _mapper.Map<RegisteredUserDto>(createdUser);

                return mappedUser;
            }
        }

    }
}
