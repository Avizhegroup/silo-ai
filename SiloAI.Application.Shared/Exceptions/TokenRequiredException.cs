namespace SiloAI.Application.Shared;
public class TokenRequiredException : Exception
{
    public TokenRequiredException()
        : base("توکن یافت نشد")
    { }
}
