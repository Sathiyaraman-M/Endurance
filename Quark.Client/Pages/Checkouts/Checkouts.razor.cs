using Quark.Core.Features.Checkouts.Commands;

namespace Quark.Client.Pages.Checkouts;

public partial class Checkouts
{
    private List<CheckoutResponse> _checkouts { get; set; }
    private CheckoutResponse _checkout { get; set; }
    private string _searchString { get; set; }
    private MudTable<CheckoutResponse> mudTable { get; set; }
    private int _totalItems;
    private int _currentPage;

    private AddCheckoutCommand Checkout = new();
    private MudDatePicker picker1;
    private MudDatePicker picker2;

    private ClaimsPrincipal _currentUser;
    private bool _canCreate;
    private bool _canView;
    private bool _canEdit;
    private bool _canDelete;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canCreate = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Checkouts.Create)).Succeeded;
        _canView = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Checkouts.View)).Succeeded;
        _canEdit = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Checkouts.Edit)).Succeeded;
        _canDelete = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Checkouts.Delete)).Succeeded;
    }

    private async Task<TableData<CheckoutResponse>> OnServerReloadAsync(TableState tableState)
    {
        if (!string.IsNullOrWhiteSpace(_searchString))
        {
            tableState.Page = 0;
        }
        await LoadDataAsync(tableState.Page, tableState.PageSize, tableState);
        return new TableData<CheckoutResponse> { TotalItems = _totalItems, Items = _checkouts };
    }

    private async Task LoadDataAsync(int page, int pageSize, TableState tableState)
    {
        string[] orderings = null;
        if (!string.IsNullOrEmpty(tableState.SortLabel))
        {
            orderings = tableState.SortDirection != SortDirection.None ? new[] { $"{tableState.SortLabel} {tableState.SortDirection}" } : new[] { $"{tableState.SortLabel}" };
        }
        var response = await _checkoutHttpClient.GetAllPaginatedAsync(new Core.Requests.PagedRequest
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
            _checkouts = response.Data;
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }

    private async Task AddCheckoutAsync()
    {
        var response = await _checkoutHttpClient.AddCheckoutAsync(Checkout);
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
        await mudTable.ReloadServerData();
    }

    private async Task InvokeCheckInModal()
    {
        var parameters = new DialogParameters();
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true, Position = DialogPosition.TopCenter };
        var dialog = dialogService.Show<CheckInBookModal>("Check In Book", parameters, options);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            OnSearch("");
        }
    }

    private void ResetModel()
    {
        Checkout.BookBarcode = "";
        Checkout.PatronRegisterId = "";
        Checkout.CheckedOutSince = DateTime.Now;
        Checkout.ExpectedCheckInDate = DateTime.Now.AddDays(15);
    }

    private async Task Delete(Guid Id)
    {
        if ((await dialogService.ShowMessageBox("Confirm Delete?", "Are you sure want to delete this checkout?", yesText: "Delete", cancelText: "Cancel")) == true)
        {
            var response = await _checkoutHttpClient.DeleteCheckoutAsync(Id);
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

    private void OnSearch(string text)
    {
        _searchString = text;
        mudTable.ReloadServerData();
    }
}