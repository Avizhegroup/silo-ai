using System.Security.Cryptography;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace SiloAI.UI.Components;
public partial class AppErrorBoundary
{
    public bool ShowException = false;

    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ILogger<AppErrorBoundary> Logger { get; set; }
    [Inject] public IJSRuntime JSRuntime { get; set; }

#if DEBUG
    protected override async Task OnInitializedAsync()
    {
        ShowException = true;
    }
#endif

    protected override async Task OnErrorAsync(Exception ex)
    {
#if DEBUG
        System.Diagnostics.Debugger.Break();
#endif
        Logger.LogWarning(ex, ex.Message);

        if (ex is CryptographicException)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.clear");

            NavigationManager.NavigateTo("/account/login", true);
        }
    }

    public async Task Refresh(MouseEventArgs e)
    {
        NavigationManager.NavigateTo(NavigationManager.Uri, true);
    }

    public async Task GoHome(MouseEventArgs e)
    {
        NavigationManager.NavigateTo("/", true);
    }
}
