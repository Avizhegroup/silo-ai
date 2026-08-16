using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

namespace SiloAI.UI.Services;

/// <summary>
/// Scoped HTTP client for the AI API that automatically attaches the JWT bearer token.
/// </summary>
public class AiApiClient(IHttpClientFactory factory, ProtectedLocalStorage storage)
{
    private async Task<HttpClient> GetHttpClientAsync()
    {
        var client = factory.CreateClient("AiApi");

        try
        {
            var tokenResult = await storage.GetAsync<string>("jwt");

            if (tokenResult.Success && !string.IsNullOrWhiteSpace(tokenResult.Value))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", tokenResult.Value);
            }
        }
        catch
        {
        }

        return client;
    }

    public async Task<T?> GetFromJsonAsync<T>(string requestUri)
    {
        var client = await GetHttpClientAsync();

        return await client.GetFromJsonAsync<T>(requestUri);
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value)
    {
        var client = await GetHttpClientAsync();

        return await client.PostAsJsonAsync(requestUri, value);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
    {
        var client = await GetHttpClientAsync();

        return await client.DeleteAsync(requestUri);
    }

    public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T value)
    {
        var client = await GetHttpClientAsync();
        return await client.PutAsJsonAsync(requestUri, value);
    }

    public async Task<HttpResponseMessage> PostMultipartAsync(string requestUri, MultipartFormDataContent content)
    {
        var client = await GetHttpClientAsync();
        return await client.PostAsync(requestUri, content);
    }
}
