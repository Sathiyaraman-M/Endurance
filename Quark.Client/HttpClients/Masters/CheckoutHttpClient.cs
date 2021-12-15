using Quark.Core.Features.Checkouts.Commands;

namespace Quark.Client.HttpClients.Masters;

public class CheckoutHttpClient
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

    public async Task<IResult<CheckoutResponse>> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{Routes.CheckoutEndpoints.BaseRoute}/{id}");
        return await response.ToResult<CheckoutResponse>();
    }

    public async Task<IResult<List<CheckoutResponse>>> GetCheckInByDateAsync(DateTime date)
    {
        var response = await _httpClient.GetAsync(Routes.CheckoutEndpoints.GetCheckInRoute(date));
        return await response.ToResult<List<CheckoutResponse>>();
    }

    public async Task<IResult<Guid>> AddCheckoutAsync(AddCheckoutCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.CheckoutEndpoints.BaseRoute, command);
        return await response.ToResult<Guid>();
    }

    public async Task<IResult<Guid>> ExtendDaysAsync(ExtendCheckoutCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.CheckoutEndpoints.ExtendCheckoutRoute, command);
        return await response.ToResult<Guid>();
    }

    public async Task<IResult<Guid>> CheckInAsync(CheckInBookCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.CheckoutEndpoints.CheckInRoute, command);
        return await response.ToResult<Guid>();
    }

    public async Task<IResult<Guid>> DeleteCheckoutAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{Routes.CheckoutEndpoints.BaseRoute}/{id}");
        return await response.ToResult<Guid>();
    }
}