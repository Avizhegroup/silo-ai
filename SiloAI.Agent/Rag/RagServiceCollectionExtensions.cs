using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiloAI.Application.Shared.Contracts.Rag;

namespace SiloAI.Agent.Rag;

public static class RagServiceCollectionExtensions
{
    /// <summary>
    /// Registers all RAG infrastructure services (chunking, embedding, indexing, search) and
    /// binds the <see cref="RagOptions"/> + <see cref="OpenAIOptions"/> from configuration.
    /// </summary>
    public static IServiceCollection AddRagServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        var ragSection = configuration.GetSection(RagOptions.SectionName);
        services.Configure<RagOptions>(o =>
        {
            o.ChunkSize = ParseInt(ragSection[nameof(RagOptions.ChunkSize)], o.ChunkSize);
            o.ChunkOverlap = ParseInt(ragSection[nameof(RagOptions.ChunkOverlap)], o.ChunkOverlap);
            o.MaxFileSize = ParseLong(ragSection[nameof(RagOptions.MaxFileSize)], o.MaxFileSize);
            var exts = ragSection.GetSection(nameof(RagOptions.SupportedExtensions))
                .GetChildren()
                .Select(c => c.Value)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Select(v => v!.ToLowerInvariant())
                .ToArray();
            if (exts.Length > 0) o.SupportedExtensions = exts;
        });

        var openAiSection = configuration.GetSection(OpenAIOptions.SectionName);
        services.Configure<OpenAIOptions>(o =>
        {
            o.ApiKey = openAiSection[nameof(OpenAIOptions.ApiKey)] ?? o.ApiKey;
            o.EmbeddingModel = openAiSection[nameof(OpenAIOptions.EmbeddingModel)] ?? o.EmbeddingModel;
            o.EmbeddingDimensions = ParseInt(openAiSection[nameof(OpenAIOptions.EmbeddingDimensions)], o.EmbeddingDimensions);
            o.Endpoint = openAiSection[nameof(OpenAIOptions.Endpoint)];
            o.MainModel = openAiSection[nameof(OpenAIOptions.MainModel)];
            o.VoiceModel = openAiSection[nameof(OpenAIOptions.VoiceModel)];
            o.RagModel = openAiSection[nameof(OpenAIOptions.RagModel)];
        });

        services.AddScoped<ITextExtractionService, TxtTextExtractionService>();
        services.AddScoped<ITextExtractionService, MarkdownTextExtractionService>();
        services.AddScoped<ITextExtractionDispatcher, TextExtractionDispatcher>();

        services.AddScoped<ITextChunkingService, TextChunkingService>();
        services.AddScoped<IEmbeddingService, OpenAIEmbeddingService>();
        services.AddScoped<IRagIndexingService, RagIndexingService>();
        services.AddScoped<IRagSearchService, RagSearchService>();

        return services;
    }

    private static int ParseInt(string? value, int fallback) =>
        int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : fallback;

    private static long ParseLong(string? value, long fallback) =>
        long.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : fallback;
}
