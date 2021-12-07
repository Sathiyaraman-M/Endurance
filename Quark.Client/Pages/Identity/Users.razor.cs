using Microsoft.AspNetCore.Authorization;
using Quark.Core.Responses.Identity;

namespace Quark.Client.Pages.Identity;

public partial class Users
{
    private List<UserResponse> _userList = new();
    private UserResponse _user = new();
    private string _searchString = "";

    private ClaimsPrincipal _currentUser;
    private bool _canCreateUsers;
    private bool _canExportUsers;
    private bool _canViewRoles;
    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canCreateUsers = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Create)).Succeeded;
        _canExportUsers = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Export)).Succeeded;
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
}