using Microsoft.AspNetCore.Identity;
using SiloAI.Identity.Client;
using SiloAI.Application.Shared.Features.Dtos;
namespace SiloAI.UI.Services;

/// <summary>
/// Simplified claim manager for the AI Admin panel.
/// Returns static admin role — no external API call needed.
/// </summary>
public class AiClaimManager : IClaimManager
{
    public Task<List<Claim>> GetUserClaims()
    {
        return Task.FromResult(new List<Claim>());
    }

    public Task<List<IdentityRole>> GetUserRoles()
    {
        return Task.FromResult(new List<IdentityRole>
        {
            new() { Name = "Admin" }
        });
    }

    public Task ClearDataLists()
    {
        return Task.CompletedTask;
    }

    public Task<bool> IsUserAdmin()
    {
        return Task.FromResult(true);
    }

    public Task<List<NavbarAllTitle>> GetAllLinks()
    {
        return Task.FromResult(new List<NavbarAllTitle>());
    }

    public Task<string> GetUrlTitle(string fullUrl)
    {
        return Task.FromResult(string.Empty);
    }
}
