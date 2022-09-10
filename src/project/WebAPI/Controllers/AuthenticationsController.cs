using Application.Features.Authentications.Commands.Login;
using Application.Features.Authentications.Commands.Register;
using Application.Features.Authentications.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : BaseController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
        {
            RegisteredUserDto result =  await Mediator.Send(registerCommand);
            return Created("/api/authentications/register", result);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
        {
            LoggedInUserDto result = await Mediator.Send(loginCommand);
            return Ok(result);
        }

    }
}
