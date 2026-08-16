using Microsoft.AspNetCore.Mvc;
using LoginCommand = SiloAI.Application.Shared.Features.LoginCommand;

namespace SiloAI.Api.Controllers;

[ApiController]
[Route("admin/auth")]
public class AiAdminAuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SiloAI.Identity.Server.Dtos.AiLoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { Successful = false, Message = "Username and password are required." });

        var result = await mediator.Send(new LoginCommand
        {
            Username = request.Username,
            Password = request.Password
        });

        if (!result.Successful)
            return Unauthorized(new { result.Successful, Message = result.Message });

        return Ok(new { result.Successful, Value = result.Token });
    }
}
