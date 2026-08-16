using Microsoft.Extensions.Logging;
using SiloAI.Identity.Server.Utilities;

namespace SiloAI.Identity.Server.Services;

public class AiIdentityBusiness(AiApiContext context, ILogger<AiIdentityBusiness> logger)
{
    public async Task<AiAdminUser?> ValidateCredentialsAsync(string username, string password)
    {
        logger.LogInformation("ValidateCredentials for username: {Username}", username);

        var user = await context.AiAdminUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

        if (user is null)
        {
            return null;
        }

        if (!AiCryptoTools.ValidatePasswordSha256(user.PasswordHash, password))
        {
            return null;
        }

        return user;
    }
}
