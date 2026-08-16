namespace SiloAI.Identity.Server;

public interface IAiJwtService
{
    Task<string?> AuthenticateAsync(string username, string password);
}
