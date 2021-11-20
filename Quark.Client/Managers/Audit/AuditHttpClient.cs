using Quark.Client.Extensions;
using Quark.Core.Responses.Audit;
using Quark.Shared.Constants;
using Quark.Shared.Wrapper;

namespace Quark.Client.Managers.Audit;

public class AuditHttpClient : IAuditHttpClient
{
    private readonly HttpClient _httpClient;

    public AuditHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult<IEnumerable<AuditResponse>>> GetAllUserTrailsAsync()
    {
        var response = await _httpClient.GetAsync(Routes.AuditTrailEndpoints.GetAllTrails);
        return await response.ToResult<IEnumerable<AuditResponse>>();
    }

    public async Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync()
    {
        var response = await _httpClient.GetAsync(Routes.AuditTrailEndpoints.GetCurrentUserTrails);
        return await response.ToResult<IEnumerable<AuditResponse>>();
    }
}