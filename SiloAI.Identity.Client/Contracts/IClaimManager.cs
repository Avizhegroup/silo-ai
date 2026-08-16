using Microsoft.AspNetCore.Identity;
using SiloAI.Application.Shared.Features;
using SiloAI.Application.Shared.Features.Dtos;
namespace SiloAI.Identity.Client;

public interface IClaimManager
{
    Task<List<Claim>> GetUserClaims();
    Task<List<IdentityRole>> GetUserRoles();
    Task ClearDataLists();
    Task<bool> IsUserAdmin();
    Task<List<NavbarAllTitle>> GetAllLinks();
    Task<string> GetUrlTitle(string fullUrl);
}
