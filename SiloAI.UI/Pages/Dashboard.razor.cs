namespace SiloAI.UI.Pages;

public partial class Dashboard
{
    private List<ApiKeyDto>? _recentKeys;
    private int _activeKeys;
    private int _totalKeys;
    private int _revokedKeys;
    private bool _isLoading = true;
    private string? _errorMessage;

    [Inject] public AiApiClient ApiClient { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadStats();
    }

    private async Task LoadStats()
    {
        _isLoading = true;

        try
        {
            var keys = await ApiClient.GetFromJsonAsync<List<ApiKeyDto>>("admin/api-keys");

            if (keys is not null)
            {
                _totalKeys = keys.Count;
                _activeKeys = keys.Count(k => !k.IsRevoked && k.ExpiresAt > DateTime.UtcNow);
                _revokedKeys = keys.Count(k => k.IsRevoked);
                _recentKeys = keys.Take(5).ToList();
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Could not load stats: {ex.Message}";
            _recentKeys = [];
        }
        finally
        {
            _isLoading = false;
        }
    }
}
