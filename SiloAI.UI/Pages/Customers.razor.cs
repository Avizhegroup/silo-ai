namespace SiloAI.UI.Pages;

public partial class Customers
{
    private List<CustomerDto>? _customers;
    private string _newName = string.Empty;
    private decimal _newCredit = 0;
    private bool _isLoading;

    private int? _editingId;
    private string _editName = string.Empty;
    private decimal _editCredit;

    private CustomerDto? _selectedCustomer;
    private bool _showApiKeys;
    private List<ApiKeyDto>? _customerKeys;
    private string _newKeyLabel = string.Empty;
    private DateTime _newKeyExpiresAt = DateTime.Today.AddYears(1);
    private string? _newKeyValue;
    private bool _isKeyLoading;

    [Inject] public AiApiClient ApiClient { get; set; }
    [CascadingParameter] public TelerikNotification Notification { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadCustomers();
    }

    private async Task LoadCustomers()
    {
        _isLoading = true;
        try
        {
            _customers = await ApiClient.GetFromJsonAsync<List<CustomerDto>>("admin/customers");
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در بارگذاری مشتریان: {ex.Message}", "error");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task CreateCustomer()
    {
        if (string.IsNullOrWhiteSpace(_newName))
        {
            Notification.Show("نام مشتری الزامی است.", "error");
            return;
        }
        _isLoading = true;
        try
        {
            var response = await ApiClient.PostAsJsonAsync("admin/customers", new CreateCustomerRequest
            {
                Name = _newName,
                RemainingCredit = _newCredit
            });
            response.EnsureSuccessStatusCode();
            _newName = string.Empty;
            _newCredit = 0;
            await LoadCustomers();
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در ثبت مشتری: {ex.Message}", "error");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void StartEdit(CustomerDto customer)
    {
        _editingId = customer.Id;
        _editName = customer.Name;
        _editCredit = customer.RemainingCredit;
    }

    private void CancelEdit()
    {
        _editingId = null;
    }

    private async Task SaveEdit(int id)
    {
        _isLoading = true;
        try
        {
            var response = await ApiClient.PutAsJsonAsync($"admin/customers/{id}", new UpdateCustomerRequest
            {
                Name = _editName,
                RemainingCredit = _editCredit
            });
            response.EnsureSuccessStatusCode();
            _editingId = null;
            await LoadCustomers();
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در ویرایش مشتری: {ex.Message}", "error");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task DeleteCustomer(int id)
    {
        _isLoading = true;
        try
        {
            var response = await ApiClient.DeleteAsync($"admin/customers/{id}");
            response.EnsureSuccessStatusCode();
            await LoadCustomers();
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در حذف مشتری: {ex.Message}", "error");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task ShowApiKeys(CustomerDto customer)
    {
        _selectedCustomer = customer;
        _showApiKeys = true;
        _newKeyValue = null;
        await LoadCustomerApiKeys(customer.Id);
    }

    private async Task LoadCustomerApiKeys(int customerId)
    {
        _isKeyLoading = true;
        try
        {
            _customerKeys = await ApiClient.GetFromJsonAsync<List<ApiKeyDto>>($"admin/customers/{customerId}/api-keys");
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در بارگذاری کلیدها: {ex.Message}", "error");
        }
        finally
        {
            _isKeyLoading = false;
        }
    }

    private async Task CreateApiKey()
    {
        if (string.IsNullOrWhiteSpace(_newKeyLabel))
        {
            Notification.Show("عنوان کلید الزامی است.", "error");
            return;
        }
        _isKeyLoading = true;
        _newKeyValue = null;
        try
        {
            var response = await ApiClient.PostAsJsonAsync("admin/api-keys", new CreateApiKeyRequest
            {
                Label = _newKeyLabel,
                ExpiresAt = _newKeyExpiresAt.ToUniversalTime(),
                CustomerId = _selectedCustomer!.Id
            });
            response.EnsureSuccessStatusCode();
            var created = await response.Content.ReadFromJsonAsync<ApiKeyDto>();
            _newKeyValue = created?.KeyValue;
            _newKeyLabel = string.Empty;
            _newKeyExpiresAt = DateTime.Today.AddYears(1);
            await LoadCustomerApiKeys(_selectedCustomer.Id);
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در ایجاد کلید: {ex.Message}", "error");
        }
        finally
        {
            _isKeyLoading = false;
        }
    }

    private async Task RevokeApiKey(int keyId)
    {
        _isKeyLoading = true;
        try
        {
            var response = await ApiClient.DeleteAsync($"admin/api-keys/{keyId}");
            response.EnsureSuccessStatusCode();
            await LoadCustomerApiKeys(_selectedCustomer!.Id);
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در لغو کلید: {ex.Message}", "error");
        }
        finally
        {
            _isKeyLoading = false;
        }
    }
}
