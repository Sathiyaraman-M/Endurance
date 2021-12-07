namespace Quark.Client.Pages.Books;
public partial class Books
{
    private List<BookResponse> _books;
    private BookResponse _book;
    private string _searchString;
    private MudTable<BookResponse> MudTable { get; set; }
    private int _totalItems;
    private int _currentPage;

    private ClaimsPrincipal _currentUser;
    private bool _canCreate;
    private bool _canView;
    private bool _canEdit;
    private bool _canDelete;
    private bool _canExport;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canCreate = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Books.Create)).Succeeded;
        _canView = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Books.View)).Succeeded;
        _canEdit = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Books.Edit)).Succeeded;
        _canDelete = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Books.Delete)).Succeeded;
        _canExport = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Books.Export)).Succeeded;
    }

    private async Task<TableData<BookResponse>> OnServerReloadAsync(TableState tableState)
    {
        if (!string.IsNullOrWhiteSpace(_searchString))
        {
            tableState.Page = 0;
        }
        await LoadDataAsync(tableState.Page, tableState.PageSize, tableState);
        return new TableData<BookResponse> { TotalItems = _totalItems, Items = _books };
    }

    private async Task LoadDataAsync(int page, int pageSize, TableState tableState)
    {
        string[] orderings = null;
        if (!string.IsNullOrEmpty(tableState.SortLabel))
        {
            orderings = tableState.SortDirection != SortDirection.None ? new[] { $"{tableState.SortLabel} {tableState.SortDirection}" } : new[] { $"{tableState.SortLabel}" };
        }
        var response = await _bookHttpClient.GetAllPaginatedAsync(new Core.Requests.PagedRequest
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
            _books = response.Data;
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
        if ((await dialogService.ShowMessageBox("Confirm Delete?", "Are you sure want to delete this book?", yesText: "Delete", cancelText: "Cancel")) == true)
        {
            var response = await _bookHttpClient.DeleteAsync(Id);
            if (response.Succeeded)
            {
                foreach (var message in response.Messages)
                {
                    snackbar.Add(message, Severity.Success);
                }
                OnSearch("");
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

    private async Task ExportToExcelAsync()
    {
        var response = await _bookHttpClient.ExportToExcelAsync(_searchString);
        if (response.Succeeded)
        {
            await jsRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = response.Data,
                FileName = $"{nameof(Books).ToLower()}_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx",
                MimeType = ApplicationConstants.MimeTypes.OpenXml
            });
            snackbar.Add(string.IsNullOrWhiteSpace(_searchString) ? "Books exported" : "Filtered Books exported", Severity.Success);
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
        MudTable.ReloadServerData();
    }
}
