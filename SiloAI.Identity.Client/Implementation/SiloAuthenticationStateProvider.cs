using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SiloAI.Application.Shared;

namespace SiloAI.Identity.Client;

public partial class SiloAuthenticationStateProvider(IClaimManager ClaimManager
    , ProtectedLocalStorage Storage)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try // this try-catch is used because of pre-rendering of blazor that throws excption on localStorage call
        {
            var tokenStorageResult = await Storage.GetAsync<string>("jwt");

            if (!tokenStorageResult.Success)
            {
                return new(new(new ClaimsIdentity()));
            }
            else
            {
                string? signTime = (await Storage.GetAsync<string>("signTime")).Value;

                if ((DateTime.Now - DateTime.Parse(signTime)).TotalHours > 10)
                {
                    return new(new(new ClaimsIdentity()));
                }
            }

            return new(new(new ClaimsIdentity(GetClaims(tokenStorageResult.Value), "jwt")));
        }
        catch (Exception ex)
        {
            return new(new(new ClaimsIdentity()));
        }
    }

    public async Task SetUserAuthenticated(string token)
    {
        var claims = GetClaims(token);

        string signTime = DateTime.Now.ToString();

        string userId = claims.FirstOrDefault(p => p.Type == "nameid").Value;

        string username = claims.FirstOrDefault(p => p.Type == "unique_name").Value;

        string roleName = claims.FirstOrDefault(p => p.Type == "role").Value;

        await Storage.SetAsync("token", userId);

        await Storage.SetAsync("jwt", token);

        await Storage.SetAsync("username", username);

        await Storage.SetAsync("signTime", signTime);

        await Storage.SetAsync("role", roleName);

        if (roleName.ToLower().NotEquals("install")
            && roleName.ToLower().NotEquals("shop"))
        {
            await ClaimManager.GetUserClaims();
        }

        var authUser = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var authState = Task.FromResult(new AuthenticationState(authUser));

        NotifyAuthenticationStateChanged(authState);
    }

    public async Task SetUserLoggedOut()
    {
        await Storage.DeleteAsync("token");

        await Storage.DeleteAsync("username");

        await Storage.DeleteAsync("jwt");

        await Storage.DeleteAsync("signTime");

        await ClaimManager.ClearDataLists();

        var anonUser = new ClaimsPrincipal(new ClaimsIdentity());

        var authState = Task.FromResult(new AuthenticationState(anonUser));

        NotifyAuthenticationStateChanged(authState);
    }

    private IEnumerable<Claim> GetClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var jwtToken = (JwtSecurityToken)handler.ReadToken(token);

        Claim[] claims = jwtToken.Claims.ToArray();

        return claims;
    }
}
