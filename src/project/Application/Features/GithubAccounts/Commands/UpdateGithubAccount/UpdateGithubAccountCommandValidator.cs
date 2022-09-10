using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GithubAccounts.Commands.UpdateGithubAccount
{
    public class UpdateGithubAccountCommandValidator : AbstractValidator<UpdateGithubAccountCommand>
    {
        public UpdateGithubAccountCommandValidator()
        {
            RuleFor(g => g.Url).NotEmpty();
            
        }
    }
}
