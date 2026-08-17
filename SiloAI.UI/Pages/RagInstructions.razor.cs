using Microsoft.AspNetCore.Components.Forms;

namespace SiloAI.UI.Pages;

public partial class RagInstructions
{
    private List<RagInstructionDto>? _instructions;
    private bool _isLoading;
    private bool _isSaving;

    private Guid? _editingId;
    private RagDocType _docType = RagDocType.GeneralChat;
    private string _key = string.Empty;
    private string _category = string.Empty;
    private string _tags = string.Empty;
    private string _content = string.Empty;
    private bool _isActive = true;

    private const long MaxUploadSize = 5 * 1024 * 1024;

    [Inject] public AiApiClient ApiClient { get; set; }
    [CascadingParameter] public TelerikNotification Notification { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadInstructionsAsync();
    }

    private async Task LoadInstructionsAsync()
    {
        _isLoading = true;
        try
        {
            _instructions = await ApiClient.GetFromJsonAsync<List<RagInstructionDto>>("api/rag/instructions");
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در بارگذاری لیست: {ex.Message}", "error");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task OnFileSelected(InputFileChangeEventArgs args)
    {
        try
        {
            using var stream = args.File.OpenReadStream(MaxUploadSize);
            using var reader = new StreamReader(stream);
            _content = await reader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در خواندن فایل: {ex.Message}", "error");
        }
    }

    private void StartEdit(RagInstructionDto instruction)
    {
        _editingId = instruction.Id;
        _docType = instruction.DocType;
        _key = instruction.Key ?? string.Empty;
        _category = instruction.Category ?? string.Empty;
        _tags = instruction.Tags ?? string.Empty;
        _content = instruction.Content;
        _isActive = instruction.IsActive;
    }

    private void ResetForm()
    {
        _editingId = null;
        _docType = RagDocType.GeneralChat;
        _key = string.Empty;
        _category = string.Empty;
        _tags = string.Empty;
        _content = string.Empty;
        _isActive = true;
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(_content)) return;

        _isSaving = true;
        try
        {
            HttpResponseMessage response;

            if (_editingId is null)
            {
                response = await ApiClient.PostAsJsonAsync("api/rag/instructions", new CreateRagInstructionCommand
                {
                    DocType = _docType,
                    Key = string.IsNullOrWhiteSpace(_key) ? null : _key.Trim(),
                    Category = _category,
                    Tags = _tags,
                    Content = _content
                });
            }
            else
            {
                response = await ApiClient.PutAsJsonAsync($"api/rag/instructions/{_editingId}", new UpdateRagInstructionCommand
                {
                    DocType = _docType,
                    Key = string.IsNullOrWhiteSpace(_key) ? null : _key.Trim(),
                    Category = _category,
                    Tags = _tags,
                    Content = _content,
                    IsActive = _isActive
                });
            }

            response.EnsureSuccessStatusCode();

            Notification.Show(_editingId is null ? "دستورالعمل با موفقیت ثبت شد." : "دستورالعمل با موفقیت به‌روزرسانی شد.", "success");

            ResetForm();
            await LoadInstructionsAsync();
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در ذخیره‌سازی: {ex.Message}", "error");
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task DeleteAsync(Guid id)
    {
        _isSaving = true;
        try
        {
            var response = await ApiClient.DeleteAsync($"api/rag/instructions/{id}");
            response.EnsureSuccessStatusCode();

            if (_editingId == id)
                ResetForm();

            await LoadInstructionsAsync();
        }
        catch (Exception ex)
        {
            Notification.Show($"خطا در حذف: {ex.Message}", "error");
        }
        finally
        {
            _isSaving = false;
        }
    }
}
