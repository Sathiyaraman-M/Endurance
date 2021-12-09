using AutoMapper;
using Quark.Core;
using Quark.Core.Responses.Identity;

namespace Quark.Client.Pages.Identity;

public partial class RolePermissions
{
    [Parameter] public string Id { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string Description { get; set; }

    private PermissionResponse _model;
    private Dictionary<string, List<RoleClaimResponse>> GroupedRoleClaims { get; } = new();
    private IMapper _mapper;
    private RoleClaimResponse _roleClaims = new();
    private RoleClaimResponse _selectedItem = new();
    private string _searchString = "";

    private ClaimsPrincipal _currentUser;
    private bool _canEditRolePermissions;
    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await authenticationHttpClient.CurrentUser();
        _canEditRolePermissions = (await authorizationService.AuthorizeAsync(_currentUser, Permissions.RoleClaims.Edit)).Succeeded;
        await GetRolePermissionsAsync();
        _loaded = true;
    }

    private async Task GetRolePermissionsAsync()
    {
        _mapper = new MapperConfiguration(c => { c.AddProfile<PermissionProfile>(); c.AddProfile<RoleClaimProfile>(); }).CreateMapper();
        var roleId = Id;
        var result = await _roleHttpClient.GetPermissionsAsync(roleId);
        if (result.Succeeded)
        {
            _model = result.Data;
            GroupedRoleClaims.Add("All Permissions", _model.RoleClaims);
            foreach (var claim in _model.RoleClaims)
            {
                if (GroupedRoleClaims.ContainsKey(claim.Group))
                {
                    GroupedRoleClaims[claim.Group].Add(claim);
                }
                else
                {
                    GroupedRoleClaims.Add(claim.Group, new List<RoleClaimResponse> { claim });
                }
            }
            if (_model != null)
            {
                Description = string.Format("Manage {0} {1}'s Permissions", _model.RoleId, _model.RoleName);
            }
        }
        else
        {
            foreach (var error in result.Messages)
            {
                snackbar.Add(error, Severity.Error);
            }
            navigationManager.NavigateTo("/identity/roles");
        }
    }

    private async Task SaveAsync()
    {
        var request = _mapper.Map<PermissionResponse, PermissionRequest>(_model);
        var result = await _roleHttpClient.UpdatePermissionsAsync(request);
        if (result.Succeeded)
        {
            snackbar.Add(result.Messages[0], Severity.Success);
            navigationManager.NavigateTo("/identity/roles");
        }
        else
        {
            foreach (var error in result.Messages)
            {
                snackbar.Add(error, Severity.Error);
            }
        }
    }

    private bool Search(RoleClaimResponse roleClaims)
    {
        if (string.IsNullOrWhiteSpace(_searchString)) return true;
        if (roleClaims.Value?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        if (roleClaims.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }
        return false;
    }

    private Color GetGroupBadgeColor(int selected, int all)
    {
        if (selected == 0)
            return Color.Error;

        if (selected == all)
            return Color.Success;

        return Color.Info;
    }
}