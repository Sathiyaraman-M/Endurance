using Quark.Core.Responses.Identity;

namespace Quark.Client.Pages.Identity;

public partial class Users
{
    private List<UserResponse> _userList = new();
    private UserResponse _user = new();
    private string _searchString = "";

    private ClaimsPrincipal _currentUser;
    private bool _canCreate;
    private bool _canExport;
    private bool _canViewRoles;
    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canCreate = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Create)).Succeeded;
        _canExport = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Export)).Succeeded;
        _canViewRoles = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.View)).Succeeded;

        await GetUsersAsync();
        _loaded = true;
    }

    private async Task GetUsersAsync()
    {
        var response = await userHttpClient.GetAllAsync();
        if (response.Succeeded)
        {
            _userList = response.Data.ToList();
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }

    private bool Search(UserResponse user)
    {
        if (string.IsNullOrWhiteSpace(_searchString)) return true;
        if (user.FullName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        if (user.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        if (user.PhoneNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        if (user.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        return false;
    }

    private async Task ExportToExcel()
    {
        var base64 = await userHttpClient.ExportToExcelAsync(_searchString);
        await jsRuntime.InvokeVoidAsync("Download", new
        {
            ByteArray = base64,
            FileName = $"{nameof(Users).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
            MimeType = ApplicationConstants.MimeTypes.OpenXml
        });
        snackbar.Add(string.IsNullOrWhiteSpace(_searchString)
            ? "Users exported"
            : "Filtered Users exported", Severity.Success);
    }

    private async Task InvokeModal()
    {
        var parameters = new DialogParameters();
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
        var dialog = dialogService.Show<RegisterUserModal>("Register New User", parameters, options);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await GetUsersAsync();
        }
    }

    private void ViewProfile(string userId)
    {
        navigationManager.NavigateTo($"/administration/user-profile/{userId}");
    }

    private void ManageRoles(string userId, string email)
    {
        if (email == "admin@wayne-enterprises.com") snackbar.Add("Not Allowed.", Severity.Error);
        else navigationManager.NavigateTo($"/administration/identity/user-roles/{userId}");
    }
}