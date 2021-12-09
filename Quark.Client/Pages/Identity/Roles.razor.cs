using Quark.Core.Responses.Identity;

namespace Quark.Client.Pages.Identity;

public partial class Roles
{
    private List<RoleResponse> _roleList = new();
    private RoleResponse _role = new();
    private string _searchString = "";

    private ClaimsPrincipal _currentUser;
    private bool _canCreateRoles;
    private bool _canEditRoles;
    private bool _canDeleteRoles;
    private bool _canViewRoleClaims;
    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canViewRoleClaims = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.RoleClaims.View)).Succeeded;
        _canCreateRoles = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.Create)).Succeeded;
        _canEditRoles = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.Edit)).Succeeded;
        _canDeleteRoles = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.Delete)).Succeeded;

        await GetRolesAsync();
        _loaded = true;
    }

    private async Task GetRolesAsync()
    {
        var response = await _roleHttpClient.GetRolesAsync();
        if (response.Succeeded)
        {
            _roleList = response.Data.ToList();
        }
        else
        {
            foreach (var message in response.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }

    private async Task Delete(string id)
    {
        if ((await dialogService.ShowMessageBox("Confirm Delete?", "Are you sure want to delete this role?", yesText: "Delete", cancelText: "Cancel")) == true)
        {
            var response = await _roleHttpClient.DeleteAsync(id);
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

    private async Task InvokeModal(string id = null)
    {
        var parameters = new DialogParameters();
        if (id != null)
        {
            _role = _roleList.FirstOrDefault(c => c.Id == id);
            if (_role != null)
            {
                parameters.Add(nameof(RoleModal.RoleModel), new RoleRequest
                {
                    Id = _role.Id,
                    Name = _role.Name,
                    Description = _role.Description
                });
            }
        }
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
        var dialog = dialogService.Show<RoleModal>(id == null ? "Create" : "Edit", parameters, options);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            await Reset();
        }
    }

    private async Task Reset()
    {
        _role = new RoleResponse();
        await GetRolesAsync();
    }

    private bool Search(RoleResponse role)
    {
        if (string.IsNullOrWhiteSpace(_searchString)) return true;
        if (role.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        if (role.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        return false;
    }

    private void ManagePermissions(string roleId)
    {
        navigationManager.NavigateTo($"/identity/role-permissions/{roleId}");
    }
}