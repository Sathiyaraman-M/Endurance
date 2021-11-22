using Quark.Core.Responses.Audit;
using Quark.Shared.Wrapper;

namespace Quark.Client.HttpClients.Audit;

public interface IAuditHttpClient
{
    Task<IResult<IEnumerable<AuditResponse>>> GetAllUserTrailsAsync();
    Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync();

    //Export Features to written
}