using Quark.Core.Features.Patrons.Commands;

namespace Quark.Client.Pages.Patrons;

public partial class Patrons
{
    private List<PatronResponse> _patrons { get; set; }
    private PatronResponse _patron { get; set; }
    private string _searchString { get; set; }
    private MudTable<PatronResponse> mudTable { get; set; }
    private int _totalItems;
    private int _currentPage;

    private ClaimsPrincipal _currentUser;
    private bool _canView;
    private bool _canCreate;
    private bool _canEdit;
    private bool _canDelete;
    private bool _canExport;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canView = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Patrons.View)).Succeeded;
        _canCreate = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Patrons.Create)).Succeeded;
        _canEdit = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Patrons.Edit)).Succeeded;
        _canDelete = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Patrons.Delete)).Succeeded;
        _canExport = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Patrons.Delete)).Succeeded;
    }

    private async Task<TableData<PatronResponse>> OnServerReloadAsync(TableState tableState)
    {
        if (!string.IsNullOrWhiteSpace(_searchString))
        {
            tableState.Page = 0;
        }
        await LoadDataAsync(tableState.Page, tableState.PageSize, tableState);
        return new TableData<PatronResponse> { TotalItems = _totalItems, Items = _patrons };
    }

    private async Task LoadDataAsync(int page, int pageSize, TableState tableState)
    {
        string[] orderings = null;
        if (!string.IsNullOrEmpty(tableState.SortLabel))
        {
            orderings = tableState.SortDirection != SortDirection.None ? new[] { $"{tableState.SortLabel} {tableState.SortDirection}" } : new[] { $"{tableState.SortLabel}" };
        }
        var response = await _patronHttpClient.GetAllPaginatedAsync(new Core.Requests.PagedRequest
        {
            PageNumber = page + 1,
            PageSize = pageSize,
            SearchString = _searchString,
            OrderBy = orderings
        });
        if (response.Succeeded)
        {
            _totalItems = response.TotalCount;
            _currentPage = response.CurrentPage;
            _patrons = response.Data;
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }

    private async Task InvokeModal(Guid id)
    {
        var parameters = new DialogParameters();
        if (id != Guid.Empty)
        {
            _patron = _patrons.FirstOrDefault(c => c.Id == id);
            if (_patron != null)
            {
                parameters.Add(nameof(AddEditPatronModal.Model), new AddEditPatronCommand
                {
                    Id = _patron.Id,
                    RegisterId = _patron.RegisterId,
                    FirstName = _patron.FirstName,
                    LastName = _patron.LastName,
                    Address = _patron.Address,
                    DateOfBirth = _patron.DateOfBirth,
                    Email = _patron.Email,
                    Mobile = _patron.Mobile,
                    Issued = _patron.Issued,
                    MultipleCheckoutLimit = _patron.MultipleCheckoutLimit
                });
            }
        }
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true, Position = DialogPosition.TopCenter };
        var dialog = dialogService.Show<AddEditPatronModal>(id == Guid.Empty ? "Add" : "Update", parameters, options);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            OnSearch("");
        }
    }

    private async Task Delete(Guid Id)
    {
        if ((await dialogService.ShowMessageBox("Confirm Delete?", "Are you sure want to delete this patron?", yesText: "Delete", cancelText: "Cancel")) == true)
        {
            var response = await _patronHttpClient.DeleteAsync(Id);
            OnSearch("");
            foreach (var message in response.Messages)
            {
                if (response.Succeeded)
                {
                    snackbar.Add(message, Severity.Success);
                }
                else
                {
                    snackbar.Add(message, Severity.Error);
                }
            }
        }
    }

    private async Task ExportToExcelAsync()
    {
        var response = await _patronHttpClient.ExportToExcelAsync(_searchString);
        if (response.Succeeded)
        {
            await jsRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = response.Data,
                FileName = $"{nameof(Patrons).ToLower()}_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx",
                MimeType = ApplicationConstants.MimeTypes.OpenXml
            });
            snackbar.Add(string.IsNullOrWhiteSpace(_searchString) ? "Patrons exported" : "Filtered Patrons exported", Severity.Success);
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        mudTable.ReloadServerData();
    }
}