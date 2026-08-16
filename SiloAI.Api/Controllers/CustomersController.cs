using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiloAI.Api.Controllers;

[ApiController]
[Route("admin/customers")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CustomersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetAllCustomersQuery(), cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var customer = await mediator.Send(new GetCustomerByIdQuery { Id = id }, cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCustomerCommand
        {
            Name = request.Name,
            RemainingCredit = request.RemainingCredit
        }, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateCustomerCommand
        {
            Id = id,
            Name = request.Name,
            RemainingCredit = request.RemainingCredit
        }, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var found = await mediator.Send(new DeleteCustomerCommand { Id = id }, cancellationToken);
        return found ? NoContent() : NotFound();
    }

    [HttpGet("{id:int}/api-keys")]
    public async Task<IActionResult> GetApiKeys(int id, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetCustomerApiKeysQuery { CustomerId = id }, cancellationToken));
}
