using Quark.Core.Responses.Identity;

namespace Quark.Client.HttpClients.Identity.Roles;

public interface IRoleHttpClient
{
    Task<IResult<List<RoleResponse>>> GetRolesAsync();

    Task<IResult<string>> SaveAsync(RoleRequest role);

    Task<IResult<string>> DeleteAsync(string id);

    Task<IResult<PermissionResponse>> GetPermissionsAsync(string roleId);

    Task<IResult<string>> UpdatePermissionsAsync(PermissionRequest request);
}