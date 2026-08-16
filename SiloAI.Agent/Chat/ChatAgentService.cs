using System.ClientModel;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using SiloAI.Agent.Rag;
using SiloAI.Application.Shared.Contracts.Rag;
using SiloAI.Application.Shared;
using SiloAI.Application.Shared.Features;
using SiloAI.Shared;

using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace SiloAI.Agent.Chat;
public class ChatAgentService(IOptions<OpenAIOptions> options)
{
    private IChatClient chatClient;
    private AIAgent writer;

    public async Task InitChatAgent(List<string>? promptKeys = null, string? modelName = null)
    {
        var instructions = await LoadInstructionsAsync(promptKeys);
        InitChatAgentWithInstructions(instructions, modelName);
    }

    public void InitChatAgentWithInstructions(string instructions, string? modelName = null)
    {
        var model = modelName ?? options.Value.MainModel;
        chatClient = new ChatClient(model,
            new ApiKeyCredential(options.Value.ApiKey),
            new OpenAIClientOptions { Endpoint = new Uri(options.Value.Endpoint) })
            .AsIChatClient();

        writer = new ChatClientAgent(chatClient, new ChatClientAgentOptions
        {
            ChatOptions = new()
            {
                Instructions = instructions,
            }
        });
    }

    public async Task<CopilotMessageDto> SendRequestAndGetResponse(CopilotMessageRequest query)
    {
        var response = await writer.RunAsync(query.Text);

        return new()
        {
            ResponseText = response?.ToString()
        };
    }

    public async Task<(CopilotMessageDto Response, string SerializedSession, ChatTokenUsageDto TokenUsage )> SendWithAgentSessionAsync( string? sessionJson,CopilotMessageRequest query)
    {
        AgentSession session;

        if (sessionJson.HasValue())
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(sessionJson);

            session = await writer.DeserializeSessionAsync(jsonElement);
        }
        else
        {
            session = await writer.CreateSessionAsync();
        }

        var result = await writer.RunAsync(query.Text, session);

        string responseText = result?.ToString();

        var serializedElement = await writer.SerializeSessionAsync(session);

        string serializedSession = serializedElement.GetRawText();

        return (new CopilotMessageDto
            {
                ResponseText = responseText
            },
            serializedSession,
            new ChatTokenUsageDto
            {
                InputTokenCount = result?.Usage?.InputTokenCount ?? 0,
                OutputTokenCount = result?.Usage?.OutputTokenCount ?? 0,
                CachedInputTokenCount = result?.Usage?.CachedInputTokenCount ?? 0,
                TotalTokenCount = result?.Usage?. TotalTokenCount ?? 0
            }
        );
    }

    public async Task<AgentSession> CreateNewSessionAsync()
    {
        return await writer.CreateSessionAsync();
    }

    public async Task<string> SerializeSessionAsync(AgentSession session)
    {
        var serializedElement = await writer.SerializeSessionAsync(session);
        return serializedElement.GetRawText();
    }

    public async Task<string> SendImageAndGetTextAsync(byte[] imageData, string imageMediaType = "image/jpeg", string? promptKey = null)
    {
        if (imageData is null || imageData.Length == 0)
        {
            throw new ArgumentException("Image data cannot be null or empty.", nameof(imageData));
        }

        DataContent? imageContent = new(imageData, imageMediaType);

        List<AIContent>? contents = new()
        {
            imageContent
        };

        string prompt = await LoadInstructionsAsync(new()
        {
            promptKey
        });

        contents.Insert(0, new TextContent(prompt));

        var message = new ChatMessage(ChatRole.User, contents);

        var response = await writer.RunAsync([message]);

        return response?.ToString() ?? string.Empty;
    }

    public async Task<string> SendImageAndGetTextAsync(Stream imageStream, string imageMediaType = "image/jpeg", string? promptText = null)
    {
        if (imageStream is null)
        {
            throw new ArgumentNullException(nameof(imageStream));
        }

        using var memoryStream = new MemoryStream();

        await imageStream.CopyToAsync(memoryStream);

        var imageData = memoryStream.ToArray();

        return await SendImageAndGetTextAsync(imageData, imageMediaType, promptText);
    }

    private async Task<string> LoadInstructionsAsync(List<string>? promptKeys = null)
    {
        var chatDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Chat");

        if (!Directory.Exists(chatDirectoryPath))
        {
            return string.Empty;
        }

        var files = Directory.GetFiles(chatDirectoryPath, "*", SearchOption.TopDirectoryOnly);
        var combinedContent = new List<string>();

        foreach (var filePath in files)
        {
            var fileContent = await File.ReadAllTextAsync(filePath);

            var fileName = Path.GetFileName(filePath);

            if (promptKeys is not null && promptKeys.Count > 0
             && fileName.NotEquals($"chtbot-instructions-main.md"))
            {
                bool shouldInclude = false;
                foreach (var promptKey in promptKeys)
                {
                    if (fileName.Equals($"chtbot-instructions-{promptKey}.md"))
                    {
                        shouldInclude = true;
                        break;
                    }
                }

                if (!shouldInclude)
                {
                    continue;
                }
            }

            combinedContent.Add($"=== {fileName} ===");

            combinedContent.Add(fileContent);

            combinedContent.Add("");
        }

        return string.Join(Environment.NewLine, combinedContent);
    }
}
