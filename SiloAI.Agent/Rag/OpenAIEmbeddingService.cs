using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Embeddings;
using SiloAI.Application.Shared.Contracts.Rag;
using System.ClientModel;

namespace SiloAI.Agent.Rag;

/// <summary>
/// <see cref="IEmbeddingService"/> backed by the official OpenAI .NET client. Supports a
/// custom endpoint (Azure OpenAI / GitHub Models gateway) via <see cref="OpenAIOptions.Endpoint"/>.
/// </summary>
public class OpenAIEmbeddingService : IEmbeddingService
{
    private readonly OpenAIOptions _options;
    private readonly EmbeddingClient _client;

    public OpenAIEmbeddingService(IOptions<OpenAIOptions> options)
    {
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("OpenAI:ApiKey is not configured.");
        }

        var credential = new ApiKeyCredential(_options.ApiKey);
        var clientOptions = new OpenAIClientOptions();
        if (!string.IsNullOrWhiteSpace(_options.Endpoint))
        {
            clientOptions.Endpoint = new Uri(_options.Endpoint);
        }

        _client = new EmbeddingClient(_options.EmbeddingModel, credential, clientOptions);
    }

    public string ModelName => _options.EmbeddingModel;

    public int Dimensions => _options.EmbeddingDimensions;

    public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken)
    {
        var result = await _client.GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);
        return result.Value.ToFloats().ToArray();
    }

    public async Task<IReadOnlyList<float[]>> GenerateEmbeddingsAsync(IReadOnlyList<string> texts, CancellationToken cancellationToken)
    {
        if (texts is null || texts.Count == 0) return [];

        var result = await _client.GenerateEmbeddingsAsync(texts, cancellationToken: cancellationToken);
        return [.. result.Value.Select(e => e.ToFloats().ToArray())];
    }
}
