using Application.Features.GithubAccounts.Commands.CreateGithubAccount;
using Application.Features.GithubAccounts.Commands.DeleteGithubAccount;
using Application.Features.GithubAccounts.Commands.UpdateGithubAccount;
using Application.Features.GithubAccounts.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubAccountsController : BaseController
    {

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateGithubAccountCommand createGithubAccountCommand)
        {
            CreatedGithubAccountDto result = await Mediator.Send(createGithubAccountCommand);
            return Created("/api/GithubAccounts/", result);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateGithubAccountCommand updateGithubAccountCommand)
        {
            UpdatedGithubAccountDto result = await Mediator.Send(updateGithubAccountCommand);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteGithubAccountCommand deleteGithubAccountCommand)
        {
            DeletedGithubAccountDto result = await Mediator.Send(deleteGithubAccountCommand);
            return Ok(result);
        }

    }
}
