using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SiloAI.Identity.Server.Services;
using SiloAI.Identity.Server.Utilities;
using Microsoft.IdentityModel.Tokens;

namespace SiloAI.Identity.Server;

public class AiJwtService(AiIdentityBusiness identityBusiness) : IAiJwtService
{
    private const string Issuer = "SiloAiIdentity";
    private const string Audience = "SiloAiAdmin";
    private const string SigningKey = "S1L0A1I2D3E4N5T6I7T8Y9K0E1Y2P3R4";

    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        var user = await identityBusiness.ValidateCredentialsAsync(username, password);

        if (user is null)
        {
            return null;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Surname, user.Name),
            new(ClaimTypes.Role, "Admin")
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = Issuer,
            Audience = Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = AiCryptoTools.GetJwtCredential(SigningKey),
            Subject = new ClaimsIdentity(claims),
            Claims = claims.ToDictionary(c => c.Type, c => (object)c.Value)
        };

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }
}
