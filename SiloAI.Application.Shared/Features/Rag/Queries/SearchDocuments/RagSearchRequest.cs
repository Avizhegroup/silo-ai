namespace SiloAI.Application.Shared.Features;

public class RagSearchRequest
{
    public string Query { get; set; }
    public int TopK { get; set; } = 10;
    public RagDocType? DocType { get; set; }
    public string? Key { get; set; }
}
