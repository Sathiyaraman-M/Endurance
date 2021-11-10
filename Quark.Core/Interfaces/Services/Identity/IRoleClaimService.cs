using Quark.Core.Requests.Identity;
using Quark.Core.Responses.Identity;
using Quark.Shared.Wrapper;

namespace Quark.Core.Interfaces.Services.Identity;

public interface IRoleClaimService
{
    Task<Result<List<RoleClaimResponse>>> GetAllAsync();

    Task<int> GetCountAsync();

    Task<Result<RoleClaimResponse>> GetByIdAsync(int id);

    Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

    Task<Result<string>> SaveAsync(RoleClaimRequest request);

    Task<Result<string>> DeleteAsync(int id);
}