using SiloAI.Identity.Client;
namespace SiloAI.UI.Services;

public interface IAiAuthenticationService
{
    Task<bool> Login(string username, string password);

    Task Logout();
}

public class AiAuthenticationService(
    IHttpClientFactory httpClientFactory,
    SiloAuthenticationStateProvider authStateProvider) : IAiAuthenticationService
{
    public async Task<bool> Login(string username, string password)
    {
        var http = httpClientFactory.CreateClient("AiApi");

        var response = await http.PostAsJsonAsync("admin/auth/login", new
        {
            Username = username,
            Password = password
        });

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (result is null || !result.Successful || string.IsNullOrWhiteSpace(result.Value))
        {
            return false;
        }

        await authStateProvider.SetUserAuthenticated(result.Value);

        return true;
    }

    public async Task Logout()
    {
        await authStateProvider.SetUserLoggedOut();
    }

    private sealed class LoginResponse
    {
        public bool Successful { get; set; }

        public string Value { get; set; }
    }
}
