using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiloAI.Api.Controllers;

[ApiController]
[Route("api/rag/instructions")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RagInstructionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetAllRagInstructionsQuery(), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRagInstructionByIdQuery { Id = id }, cancellationToken);
       
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRagInstructionCommand command, CancellationToken cancellationToken)
    {
        command.CreatorUserId = User?.Identity?.Name;
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRagInstructionCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        command.UpdaterUserId = User?.Identity?.Name;
        var result = await mediator.Send(command, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var found = await mediator.Send(new DeleteRagInstructionCommand { Id = id }, cancellationToken);
        return found ? NoContent() : NotFound();
    }
}
