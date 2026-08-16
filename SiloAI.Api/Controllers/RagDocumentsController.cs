using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SiloAI.Agent.Rag;

namespace SiloAI.Api.Controllers;

[ApiController]
[Route("api/rag/documents")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RagDocumentsController(
    IMediator mediator,
    IOptions<RagOptions> ragOptions) : ControllerBase
{
    private readonly RagOptions _ragOptions = ragOptions.Value;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetAllRagDocumentsQuery(), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRagDocumentByIdQuery { Id = id }, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [RequestSizeLimit(long.MaxValue)]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file,
        [FromForm] RagDocType? docType,
        [FromForm] string? key,
        [FromForm] string? category,
        [FromForm] string? tags,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "File is required." });

        if (file.Length > _ragOptions.MaxFileSize)
            return BadRequest(new { error = $"File exceeds maximum size of {_ragOptions.MaxFileSize} bytes." });

        var extension = (Path.GetExtension(file.FileName) ?? string.Empty).ToLowerInvariant();
        if (!_ragOptions.SupportedExtensions.Any(e => string.Equals(e, extension, StringComparison.OrdinalIgnoreCase)))
            return BadRequest(new { error = $"Extension '{extension}' is not supported." });

        using var memory = new MemoryStream();
        await using var uploadStream = file.OpenReadStream();
        await uploadStream.CopyToAsync(memory, cancellationToken);

        var result = await mediator.Send(new UploadRagDocumentCommand
        {
            FileContent = memory.ToArray(),
            FileName = file.FileName,
            ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
            DocType = docType,
            Key = key,
            Category = category,
            Tags = tags,
            CreatorUserId = User?.Identity?.Name
        }, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var found = await mediator.Send(new DeleteRagDocumentCommand { Id = id }, cancellationToken);
        return found ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/rebuild")]
    [RequestSizeLimit(long.MaxValue)]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public async Task<IActionResult> Rebuild(
        Guid id,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "File is required to rebuild embeddings." });

        if (file.Length > _ragOptions.MaxFileSize)
            return BadRequest(new { error = $"File exceeds maximum size of {_ragOptions.MaxFileSize} bytes." });

        using var memory = new MemoryStream();
        await using var uploadStream = file.OpenReadStream();
        await uploadStream.CopyToAsync(memory, cancellationToken);

        try
        {
            var result = await mediator.Send(new RebuildRagDocumentCommand
            {
                Id = id,
                FileContent = memory.ToArray(),
                FileName = file.FileName,
                ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? string.Empty : file.ContentType,
                UpdaterUserId = User?.Identity?.Name
            }, cancellationToken);

            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] RagSearchRequest request, CancellationToken cancellationToken)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Query))
            return BadRequest(new { error = "Query is required." });

        var result = await mediator.Send(new SearchRagDocumentsQuery
        {
            Query = request.Query,
            TopK = request.TopK,
            DocType = request.DocType,
            Key = request.Key
        }, cancellationToken);

        return Ok(result);
    }
}
