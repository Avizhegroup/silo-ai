using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace SiloAI.Api.Auth;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions { }

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<ApiKeyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    AiApiContext context)
    : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder)
{
    public const string SchemeName = "ApiKey";
    private const string HeaderName = "X-Api-Key";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HeaderName, out var keyValues))
            return AuthenticateResult.Fail("API key header not found");

        var key = keyValues.FirstOrDefault();

        if (string.IsNullOrEmpty(key))
            return AuthenticateResult.Fail("API key is empty");

        var keyHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(key))).ToLowerInvariant();

        var apiKey = await context.AiApiKeys
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.KeyValue == keyHash && !k.IsRevoked && k.ExpiresAt > DateTime.UtcNow);

        if (apiKey is null)
            return AuthenticateResult.Fail("Invalid or expired API key");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, apiKey.Label)
        };

        if (apiKey.CustomerId.HasValue)
            claims.Add(new Claim("CustomerId", apiKey.CustomerId.Value.ToString()));

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return AuthenticateResult.Success(ticket);
    }
}
