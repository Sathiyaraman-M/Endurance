using Quark.Core.Responses.Audit;
using Quark.Shared.Wrapper;

namespace Quark.Core.Interfaces.Services;

public interface IAuditService
{
    Task<IResult<List<AuditResponse>>> GetCurrentUserTrailsAsync(string userId = "");
}