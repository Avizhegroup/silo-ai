using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewChatSessionCommands = SiloAI.Application.Shared.Features.NewChatSessionCommands;
using SiloAI.Api.Auth;
using SiloAI.Application.Api;

namespace SiloAI.Api.Controllers;

[ApiController]
[Route("api/ai/chat")]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
public class AiChatController(IMediator mediator) : ControllerBase
{
    [HttpPost("new-session")]
    public async Task<IActionResult> NewSession([FromBody] NewSessionRequest request)
    {
        try
        {
            var result = await mediator.Send(new NewChatSessionCommand
            {
                PromptKeys = request.PromptKeys,
                CustomerId = GetCustomerId()
            });
            return Ok(result);
        }
        catch (InsufficientCreditException)
        {
            return StatusCode(402, new { Message = "اعتبار مشتری به پایان رسیده است." });
        }
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendChatRequest request)
    {
        try
        {
            var result = await mediator.Send(new SendChatCommand
            {
                SessionJson = request.SessionJson,
                Message = request.Message,
                Username = request.Username,
                PromptKeys = request.PromptKeys,
                CustomerId = GetCustomerId()
            });
            return Ok(result);
        }
        catch (InsufficientCreditException)
        {
            return StatusCode(402, new { Message = "اعتبار مشتری به پایان رسیده است." });
        }
    }

    private int? GetCustomerId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
        return claim is not null && int.TryParse(claim.Value, out var id) ? id : null;
    }
}
