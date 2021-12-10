using Quark.Core.Responses.Identity;

namespace Quark.Client.Pages.Identity;

public partial class UserRoles
{
    [Parameter] public string Id { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string Description { get; set; }
    public List<UserRoleModel> UserRolesList { get; set; } = new();

    private UserRoleModel _userRole = new();
    private string _searchString = "";

    private ClaimsPrincipal _currentUser;
    private bool _canEditUsers;
    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canEditUsers = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Edit)).Succeeded;

        var userId = Id;
        var result = await userHttpClient.GetAsync(userId);
        if (result.Succeeded)
        {
            var user = result.Data;
            if (user != null)
            {
                Title = $"{user.FullName}";
                Description = string.Format("Manage {0}'s Roles", user.FullName);
                var response = await userHttpClient.GetRolesAsync(user.Id);
                UserRolesList = response.Data.UserRoles;
            }
        }

        _loaded = true;
    }

    private async Task SaveAsync()
    {
        var request = new UpdateUserRolesRequest()
        {
            UserId = Id,
            UserRoles = UserRolesList
        };
        var result = await userHttpClient.UpdateRolesAsync(request);
        if (result.Succeeded)
        {
            snackbar.Add(result.Messages[0], Severity.Success);
            navigationManager.NavigateTo("/identity/users");
        }
        else
        {
            foreach (var error in result.Messages)
            {
                snackbar.Add(error, Severity.Error);
            }
        }
    }

    private bool Search(UserRoleModel userRole)
    {
        if (string.IsNullOrWhiteSpace(_searchString)) return true;
        if (userRole.RoleName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        if (userRole.RoleDescription?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        return false;
    }
}