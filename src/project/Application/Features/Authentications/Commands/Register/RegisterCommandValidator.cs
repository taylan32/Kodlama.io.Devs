using Core.Security.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentications.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(u => u.UserForRegisterDto.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.UserForRegisterDto.FirstName).NotEmpty().MaximumLength(20);
            RuleFor(u => u.UserForRegisterDto.LastName).NotEmpty().MaximumLength(20);
            RuleFor(u => u.UserForRegisterDto.Password).NotEmpty().MinimumLength(4);
           
        }
    }
}
