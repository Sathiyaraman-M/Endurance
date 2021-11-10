using Quark.Core.Requests.Identity;
using Quark.Core.Responses.Identity;
using Quark.Shared.Wrapper;

namespace Quark.Client.Managers.Identity.Roles;

public interface IRoleManager
{
    Task<IResult<List<RoleResponse>>> GetRolesAsync();

    Task<IResult<string>> SaveAsync(RoleRequest role);

    Task<IResult<string>> DeleteAsync(string id);

    Task<IResult<PermissionResponse>> GetPermissionsAsync(string roleId);

    Task<IResult<string>> UpdatePermissionsAsync(PermissionRequest request);
}