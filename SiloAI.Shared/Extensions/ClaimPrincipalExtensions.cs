using System.Security.Claims;

namespace SiloAI.Shared;
public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        var claims = ((ClaimsIdentity)principal.Identity).Claims;

        if (claims.Any())
        {
            return claims.Skip(1).First().Value;
        }

        return null;
    }

    public static string GetUsername(this ClaimsPrincipal principal)
    {
        var claims = ((ClaimsIdentity)principal.Identity).Claims;

        if (claims.Any())
        {
            return claims.First().Value;
        }

        return null;
    }

    public static string GetUserPersianName(this ClaimsPrincipal principal)
    {
        var claims = ((ClaimsIdentity)principal.Identity).Claims;

        var claim = claims.FirstOrDefault(p => p.Type == "family_name");

        if (claim is not null)
        {
            return claim.Value;
        }

        return null;
    }

    public static string GetUserRoleName(this ClaimsPrincipal principal)
    {
        var claims = ((ClaimsIdentity)principal.Identity).Claims;

        var claim = claims.FirstOrDefault(p => p.Type == "role");

        if (claim is not null)
        {
            return claim.Value;
        }

        return null;
    }
    
    public static string GetUserImage(this ClaimsPrincipal principal)
    {
        var claims = ((ClaimsIdentity)principal.Identity).Claims;

        var claim = claims.FirstOrDefault(p => p.Type == ClaimTypes.Locality);

        if (claim is not null)
        {
            return claim.Value;
        }

        return null;
    }
}
