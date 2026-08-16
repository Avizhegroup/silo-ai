namespace SiloAI.UI.Pages;

public partial class Index
{
    private List<ApiKeyDto>? _recentKeys;
    private List<CustomerKeyStat> _customerStats = [];
    private Dictionary<int, string> _customerMap = [];
    private int _totalCustomers;
    private int _activeKeys;
    private int _totalKeys;
    private int _revokedKeys;
    private bool _isLoading = true;

    [Inject] public AiApiClient ApiClient { get; set; }
    [CascadingParameter] public TelerikNotification Notification { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadStats();
    }

    private async Task LoadStats()
    {
        _isLoading = true;

        try
        {
            var customersTask = ApiClient.GetFromJsonAsync<List<CustomerDto>>("admin/customers");
            var keysTask = ApiClient.GetFromJsonAsync<List<ApiKeyDto>>("admin/api-keys");

            await Task.WhenAll(customersTask, keysTask);

            var customers = customersTask.Result ?? [];
            var keys = keysTask.Result ?? [];

            _customerMap = customers.ToDictionary(c => c.Id, c => c.Name);
            _totalCustomers = customers.Count;

            _totalKeys = keys.Count;
            _activeKeys = keys.Count(k => !k.IsRevoked && k.ExpiresAt > DateTime.UtcNow);
            _revokedKeys = keys.Count(k => k.IsRevoked);

            _recentKeys = keys.Take(5).ToList();

            _customerStats = customers
                .Where(c => keys.Any(k => k.CustomerId == c.Id))
                .Select(c =>
                {
                    var customerKeys = keys.Where(k => k.CustomerId == c.Id).ToList();
                    return new CustomerKeyStat(
                        c.Name,
                        customerKeys.Count(k => !k.IsRevoked && k.ExpiresAt > DateTime.UtcNow),
                        customerKeys.Count(k => !k.IsRevoked && k.ExpiresAt <= DateTime.UtcNow),
                        customerKeys.Count(k => k.IsRevoked)
                    );
                })
                .ToList();
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در بارگذاری آمار: {ex.Message}", "error");
            _recentKeys = [];
        }
        finally
        {
            _isLoading = false;
        }
    }

    private record CustomerKeyStat(string CustomerName, int ActiveKeys, int ExpiredKeys, int RevokedKeys);
}
