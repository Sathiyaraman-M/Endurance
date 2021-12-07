using Quark.Core.Features.Designations.Commands;

namespace Quark.Client.Pages.Designations;

public partial class Designations
{
    private List<DesignationResponse> _designations { get; set; }
    private DesignationResponse _designation { get; set; }
    private string _searchString { get; set; }

    private ClaimsPrincipal _currentUser;
    private bool _loaded;
    private bool _canCreate;
    private bool _canEdit;
    private bool _canDelete;
    private bool _canExport;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canCreate = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Designations.Create)).Succeeded;
        _canEdit = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Designations.Edit)).Succeeded;
        _canDelete = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Designations.Delete)).Succeeded;
        _canExport = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Designations.Export)).Succeeded;
        await GetDesignationsAsync();
        _loaded = true;
    }

    private async Task GetDesignationsAsync()
    {
        var response = await _designationHttpClient.GetAllAsync();
        if (response.Succeeded)
        {
            _designations = response.Data;
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }

    private async Task Delete(Guid Id)
    {
        if ((await dialogService.ShowMessageBox("Confirm Delete?", "Are you sure want to delete this designation?", yesText: "Delete", cancelText: "Cancel")) == true)
        {
            var response = await _designationHttpClient.DeleteAsync(Id);
            await Reset();
            if (response.Succeeded)
            {
                snackbar.Add(response.Messages[0], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    snackbar.Add(message, Severity.Error);
                }
            }
        }
    }
    private async Task InvokeModal(Guid id)
    {
        var parameters = new DialogParameters();
        if (id != Guid.Empty)
        {
            _designation = _designations.FirstOrDefault(c => c.Id == id);
            if (_designation != null)
            {
                parameters.Add(nameof(AddEditDesignationModal.Model), new AddEditDesignationCommand
                {
                    Id = _designation.Id,
                    Name = _designation.Name
                });
            }
        }
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true, Position = DialogPosition.TopCenter };
        var dialog = dialogService.Show<AddEditDesignationModal>(id == Guid.Empty ? "Create" : "Edit", parameters, options);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await Reset();
        }
    }

    private async Task ExportToExcelAsync()
    {
        var response = await _designationHttpClient.ExportToExcelAsync(_searchString);
        if (response.Succeeded)
        {
            await jsRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = response.Data,
                FileName = $"{nameof(Designations).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                MimeType = ApplicationConstants.MimeTypes.OpenXml
            });
            snackbar.Add(string.IsNullOrWhiteSpace(_searchString) ? "Designations exported" : "Filtered Designations exported", Severity.Success);
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }

    private async Task Reset()
    {
        _designation = new DesignationResponse();
        await GetDesignationsAsync();
    }

    private bool Search(DesignationResponse designation)
    {
        if (string.IsNullOrWhiteSpace(_searchString)) return true;
        if (designation.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        return false;
    }
}