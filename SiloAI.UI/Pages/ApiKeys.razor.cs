namespace SiloAI.UI.Pages;

public partial class ApiKeys
{
    private List<ApiKeyDto>? _keys;
    private string _newKeyLabel = string.Empty;
    private DateTime _newKeyExpiresAt = DateTime.Today.AddYears(1);
    private string? _newKeyValue;
    private string? _errorMessage;
    private bool _isLoading;

    [Inject] public AiApiClient ApiClient { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadKeys();
    }

    private async Task LoadKeys()
    {
        _isLoading = true;
        _errorMessage = null;

        try
        {
            _keys = await ApiClient.GetFromJsonAsync<List<ApiKeyDto>>("admin/api-keys");
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load keys: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task CreateKey()
    {
        if (string.IsNullOrWhiteSpace(_newKeyLabel))
        {
            _errorMessage = "Label is required.";
            return;
        }

        _isLoading = true;
        _errorMessage = null;
        _newKeyValue = null;

        try
        {
            var request = new CreateApiKeyRequest
            {
                Label = _newKeyLabel,
                ExpiresAt = _newKeyExpiresAt.ToUniversalTime()
            };

            var response = await ApiClient.PostAsJsonAsync("admin/api-keys", request);

            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<ApiKeyDto>();

            _newKeyValue = created?.KeyValue;
            _newKeyLabel = string.Empty;
            _newKeyExpiresAt = DateTime.Today.AddYears(1);

            await LoadKeys();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to create key: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task RevokeKey(int id)
    {
        _isLoading = true;
        _errorMessage = null;

        try
        {
            var response = await ApiClient.DeleteAsync($"admin/api-keys/{id}");
            response.EnsureSuccessStatusCode();
            await LoadKeys();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to revoke key: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }
}
