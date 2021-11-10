using Quark.Core.Requests.Identity;
using Quark.Core.Responses.Identity;
using Quark.Shared.Wrapper;

namespace Quark.Core.Interfaces.Services.Identity;

public interface IRoleService
{
    Task<Result<List<RoleResponse>>> GetAllAsync();

    Task<int> GetCountAsync();

    Task<Result<RoleResponse>> GetByIdAsync(string id);

    Task<Result<string>> SaveAsync(RoleRequest request);

    Task<Result<string>> DeleteAsync(string id);

    Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId);

    Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request);
}