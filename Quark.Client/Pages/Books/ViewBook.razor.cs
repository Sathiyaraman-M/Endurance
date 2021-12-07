namespace Quark.Client.Pages.Books;

public partial class ViewBook
{
    [Parameter]
    public Guid Id { get; set; }
    private BookResponse Book;
    private string Condition;

    private ClaimsPrincipal _currentUser;
    private bool _loaded = false;
    private bool _canEdit;
    private bool _canDelete;

    protected override async Task OnParametersSetAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canEdit = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Books.Edit)).Succeeded;
        _canDelete = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Books.Delete)).Succeeded;
        var response = await _bookHttpClient.GetByIdAsync(Id);
        if (response.Succeeded)
        {
            Book = response.Data;
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
        _loaded = true;
    }

    private async Task Delete()
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
                StateHasChanged();
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
}