using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiloAI.Api.Controllers;

[ApiController]
[Route("admin/api-keys")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ApiKeysController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetAllApiKeysQuery(), cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateApiKeyRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateApiKeyForCustomerCommand
        {
            Label = request.Label,
            ExpiresAt = request.ExpiresAt,
            CustomerId = request.CustomerId
        }, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Revoke(int id, CancellationToken cancellationToken)
    {
        var found = await mediator.Send(new RevokeApiKeyCommand { Id = id }, cancellationToken);
        return found ? NoContent() : NotFound();
    }
}
