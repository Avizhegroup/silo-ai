using System.ComponentModel.DataAnnotations;

namespace SiloAI.UI.Pages.Account;

public partial class Login
{
    private readonly LoginModel _model = new();
    private bool _isLoading;

    [Inject] public IAiAuthenticationService AuthenticationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }
    [CascadingParameter] public TelerikNotification Notification { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthStateProvider.GetAuthenticationStateAsync();

        if (state.User.Identity?.IsAuthenticated == true)
        {
            NavigationManager.NavigateTo("/", forceLoad: false);
        }
    }

    private async Task OnLoginSubmit()
    {
        _isLoading = true;

        try
        {
            bool success = await AuthenticationService.Login(_model.Username, _model.Password);

            if (success)
            {
                NavigationManager.NavigateTo("/", forceLoad: true);
            }
            else
            {
                Notification.Show("نام کاربری یا رمز عبور نامعتبر است.", "error");
            }
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در ورود: {ex.Message}", "error");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private sealed class LoginModel
    {
        [Required(ErrorMessage = "نام کاربری الزامی است.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        public string Password { get; set; } = string.Empty;
    }
}
