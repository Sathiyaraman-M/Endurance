using Quark.Core.Responses.Audit;

namespace Quark.Core.Interfaces.Services;

public interface IAuditService
{
    Task<IResult<List<AuditResponse>>> GetCurrentUserTrailsAsync(string userId = "");
}