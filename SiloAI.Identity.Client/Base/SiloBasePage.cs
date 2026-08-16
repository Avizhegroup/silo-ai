using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace SiloAI.Identity.Client.Base;
public class SiloBasePage : ComponentBase
{
    public bool mustCheckAccess = true;
    public bool IsFiltersShown = true;
    public SiloInnerPagePermission InnerPermissions = new();
    public string PageTitle = string.Empty;

    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public SiloAuthenticationStateProvider AuthState { get; set; }
    [Inject] public IClaimManager ClaimManager { get; set; }

    public async Task CheckAccess()
    {
        if (await IsLoginTimeout(await AuthState.GetAuthenticationStateAsync()))
        {
            NavigationManager.NavigateTo("/account/login", true);

            return;
        }

        if (await ClaimManager.IsUserAdmin())
        {
            return;
        }

        var validUrls = (await ClaimManager.GetUserClaims()).Select(p => p.Value.Trim());

        string pattern = string.Join("|", validUrls.Select(url => Regex.Escape(url)));

        string currentUrl = NavigationManager.Uri;

        if (!Regex.IsMatch(currentUrl, pattern))
        {
            NavigationManager.NavigateTo("/account/deadend", true);
        }
    }

    public async Task SetPageTitle()
    {
        PageTitle = await ClaimManager.GetUrlTitle(NavigationManager.Uri);
    }

    protected virtual Task SiloInitializer()
    {
        return Task.CompletedTask;
    }

    private async Task<bool> IsLoginTimeout(AuthenticationState state)
    {
        if (!state.User.Claims.Any())
        {
            return true;
        }

        return false;
    }
}
