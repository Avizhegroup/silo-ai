namespace SiloAI.Application.Api.Features;

/// <summary>
/// Builds a stable owner key used to scope <see cref="AiChatSession"/> rows to the
/// authenticated caller, so a conversationId cannot be used to access another
/// customer's/user's conversation.
/// </summary>
internal static class ChatSessionOwnerKey
{
    public static string ForCustomer(int? customerId) =>
        customerId.HasValue ? $"customer:{customerId.Value}" : "anonymous";

    public static string ForOwnerId(string? ownerId) =>
        string.IsNullOrWhiteSpace(ownerId) ? "anonymous" : $"owner:{ownerId}";
}
