using Quark.Client.Extensions;
using Quark.Core.Features.Checkouts.Commands;
using Quark.Core.Requests;
using Quark.Core.Responses;
using Quark.Shared.Constants;
using Quark.Shared.Wrapper;
using System.Net.Http.Json;

namespace Quark.Client.HttpClients.Masters.Checkouts;

public class CheckoutHttpClient : ICheckoutHttpClient
{
    private readonly HttpClient _httpClient;

    public CheckoutHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginatedResult<CheckoutResponse>> GetAllPaginatedAsync(PagedRequest request)
    {
        var response = await _httpClient.GetAsync(Routes.CheckoutEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.OrderBy));
        return await response.ToPaginatedResult<CheckoutResponse>();
    }

    public async Task<IResult<CheckoutResponse>> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{Routes.CheckoutEndpoints.BaseRoute}/{id}");
        return await response.ToResult<CheckoutResponse>();
    }

    public async Task<IResult<int>> AddCheckoutAsync(AddCheckoutCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.CheckoutEndpoints.BaseRoute, command);
        return await response.ToResult<int>();
    }

    public async Task<IResult<int>> ExtendDaysAsync(ExtendCheckoutCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.CheckoutEndpoints.ExtendCheckoutRoute, command);
        return await response.ToResult<int>();
    }

    public async Task<IResult<int>> CheckInAsync(CheckInBookCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.CheckoutEndpoints.CheckInRoute, command);
        return await response.ToResult<int>();
    }

    public async Task<IResult<int>> DeleteCheckoutAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{Routes.CheckoutEndpoints.BaseRoute}/{id}");
        return await response.ToResult<int>();
    }
}