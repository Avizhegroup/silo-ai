
namespace SiloAI.Application.Shared.Features;
public class ChatTokenUsageDto
{
    public long InputTokenCount { get; set; }
    public long OutputTokenCount { get; set; }
    public long CachedInputTokenCount { get; set; }
    public long TotalTokenCount { get; set; }
}