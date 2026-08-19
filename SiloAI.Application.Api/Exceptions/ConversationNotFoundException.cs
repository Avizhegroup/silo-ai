namespace SiloAI.Application.Api;

public class ConversationNotFoundException : Exception
{
    public ConversationNotFoundException()
        : base("مکالمه یافت نشد یا دسترسی به آن مجاز نیست.")
    {
    }
}
