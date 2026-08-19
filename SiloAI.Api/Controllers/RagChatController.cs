using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SiloAI.Agent.Rag;
using SiloAI.Api.Auth;
using SiloAI.Application.Api;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace SiloAI.Api.Controllers;

[ApiController]
[Route("api/rag/chat")]
[Authorize(AuthenticationSchemes =
    $"{JwtBearerDefaults.AuthenticationScheme},{ApiKeyAuthenticationHandler.SchemeName}")]
public class RagChatController(
    IMediator mediator,
    IOptions<OpenAIOptions> openAiOptions) : ControllerBase
{
    private static readonly string _ragSystemPrompt;
    private static readonly string _ragSystemPromptMainChat;
    private static readonly string _augmentedMessageTemplate;

    static RagChatController()
    {
        var sections = LoadPromptSections("SiloAI.Api.Prompts.rag-prompts.txt");

        _ragSystemPrompt = sections["SystemPrompt"];

        _ragSystemPromptMainChat = sections["SystemPromptMainChat"];

        _augmentedMessageTemplate = sections["AugmentedMessageTemplate"];
    }

    [HttpPost("new-session")]
    public async Task<IActionResult> NewSession(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RagChatNewSessionCommand
        {
            SystemPrompt = _ragSystemPrompt,
            RagModel = openAiOptions.Value.RagModel,
            OwnerId = GetOwnerId()
        }, cancellationToken);

        return Ok(result);
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] RagChatRequest request, CancellationToken cancellationToken)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Message))
            return BadRequest(new { error = "Message is required." });

        try
        {
            var result = await mediator.Send(new RagChatSendCommand
            {
                ConversationId = request.ConversationId,
                Message = request.Message,
                TopK = request.TopK,
                IsMainChat = request.IsMainChat,
                DocType = request.DocType,
                Key = request.Key,
                SystemPrompt = _ragSystemPrompt,
                SystemPromptMainChat = _ragSystemPromptMainChat,
                AugmentedMessageTemplate = _augmentedMessageTemplate,
                RagModel = openAiOptions.Value.RagModel,
                Username = User?.Identity?.Name ?? string.Empty,
                OwnerId = GetOwnerId()
            }, cancellationToken);

            return Ok(result);
        }
        catch (ConversationNotFoundException)
        {
            return NotFound(new { error = "مکالمه یافت نشد یا دسترسی به آن مجاز نیست." });
        }
    }

    private string GetOwnerId()
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == "CustomerId")?.Value;
        if (!string.IsNullOrEmpty(customerId))
            return $"customer:{customerId}";

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
            return $"user:{userId}";

        return User?.Identity?.Name ?? string.Empty;
    }

    private static Dictionary<string, string> LoadPromptSections(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded prompt file '{resourceName}' not found.");
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var content = reader.ReadToEnd();

        var sections = new Dictionary<string, string>();
        var parts = content.Split("### SECTION:", StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            var headerEnd = part.IndexOf(" ###");
            if (headerEnd < 0) continue;
            var key = part[..headerEnd].Trim();
            var value = part[(headerEnd + 4)..].Trim();
            sections[key] = value;
        }

        return sections;
    }
}
