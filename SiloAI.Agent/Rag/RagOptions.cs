namespace SiloAI.Agent.Rag;

/// <summary>
/// Strongly-typed configuration for the RAG knowledge base feature, bound from the
/// "RAG" section of <c>appsettings.json</c>.
/// </summary>
public class RagOptions
{
    public const string SectionName = "RAG";

    /// <summary>Target chunk size in tokens.</summary>
    public int ChunkSize { get; set; } = 800;

    /// <summary>Token overlap between adjacent chunks.</summary>
    public int ChunkOverlap { get; set; } = 100;

    /// <summary>Maximum allowed upload file size in bytes.</summary>
    public long MaxFileSize { get; set; } = 25 * 1024 * 1024;

    /// <summary>File extensions that the indexer is allowed to ingest (lower-case, includes dot).</summary>
    public string[] SupportedExtensions { get; set; } = [".txt", ".md"];
}

/// <summary>
/// Strongly-typed OpenAI configuration for embedding / chat usage.
/// </summary>
public class OpenAIOptions
{
    public const string SectionName = "OpenAI";

    public string? ApiKey { get; set; }

    public string EmbeddingModel { get; set; } = "text-embedding-3-small";

    /// <summary>
    /// Dimension count of the embedding vector. Must match the size of the VECTOR(N) column
    /// declared in the EF migration. 1536 for text-embedding-3-small, 3072 for -3-large.
    /// </summary>
    public int EmbeddingDimensions { get; set; } = 1536;

    /// <summary>Optional custom endpoint (e.g. Azure OpenAI, GitHub Models gateway).</summary>
    public string? Endpoint { get; set; }

    /// <summary>Model used for general chat interactions.</summary>
    public string? MainModel { get; set; }

    /// <summary>Model used for voice / OCR interactions.</summary>
    public string? VoiceModel { get; set; }

    /// <summary>Model used for RAG (retrieval-augmented generation) chat.</summary>
    public string? RagModel { get; set; }
}
