using Quark.Client.Extensions;
using Quark.Core.Features.Patrons.Commands;
using Quark.Core.Requests;
using Quark.Core.Responses;
using Quark.Shared.Constants;
using Quark.Shared.Wrapper;
using System.Net.Http.Json;

namespace Quark.Client.HttpClients.Masters.Patrons;

public class PatronHttpClient : IPatronHttpClient
{
    private readonly HttpClient _httpClient;

    public PatronHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginatedResult<PatronResponse>> GetAllPaginatedAsync(PagedRequest request)
    {
        var response = await _httpClient.GetAsync(Routes.PatronEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.OrderBy));
        return await response.ToPaginatedResult<PatronResponse>();
    }

    public async Task<IResult<PatronResponse>> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{Routes.PatronEndpoints.BaseRoute}/{id}");
        return await response.ToResult<PatronResponse>();
    }

    public async Task<IResult<int>> SaveAsync(AddEditPatronCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.PatronEndpoints.BaseRoute, command);
        return await response.ToResult<int>();
    }

    public async Task<IResult<int>> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{Routes.PatronEndpoints.BaseRoute}/{id}");
        return await response.ToResult<int>();
    }
}