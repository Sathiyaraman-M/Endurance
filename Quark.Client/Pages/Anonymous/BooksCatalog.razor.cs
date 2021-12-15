namespace Quark.Client.Pages.Anonymous;

public partial class BooksCatalog
{
    private List<BookResponse> _books;
    private string _searchString;
    private MudTable<BookResponse> MudTable { get; set; }
    private int _totalItems;
    private int _currentPage;

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
        var response = await _bookHttpClient.GetAllPaginatedAsync(new PagedRequest
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

    private void OnSearch(string text)
    {
        _searchString = text;
        MudTable.ReloadServerData();
    }
}