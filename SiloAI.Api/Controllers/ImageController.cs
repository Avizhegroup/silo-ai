using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SiloAI.Agent.Rag;
using SiloAI.Api.Auth;
using SiloAI.Application.Api;

namespace SiloAI.Api.Controllers;

[ApiController]
[Route("api/image")]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
public class ImageController(IMediator mediator) : ControllerBase
{
    [HttpPost("ocr")]
    public async Task<IActionResult> Ocr([FromForm] IFormFile imageData, [FromForm] string mediaType, [FromForm] string? promptKey)
    {
        if (imageData is null || imageData.Length == 0)
            return BadRequest("Image data is required.");

        using var ms = new MemoryStream();
        await imageData.CopyToAsync(ms);

        try
        {
            var result = await mediator.Send(new OcrCommand
            {
                ImageData = ms.ToArray(),
                MediaType = mediaType,
                PromptKey = promptKey,
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
