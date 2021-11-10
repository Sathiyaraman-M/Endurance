using Quark.Client.Extensions;
using Quark.Core.Features.Books.Commands;
using Quark.Core.Requests;
using Quark.Core.Responses;
using Quark.Shared.Constants;
using Quark.Shared.Wrapper;
using System.Net.Http.Json;

namespace Quark.Client.Managers.Masters.Books;

public class BookManager : IBookManager
{
    private readonly HttpClient _httpClient;

    public BookManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult<BookResponse>> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{Routes.BookEndpoints.BaseRoute}/{id}");
        return await response.ToResult<BookResponse>();
    }

    public async Task<PaginatedResult<BookResponse>> GetAllPaginatedAsync(PagedRequest request)
    {
        var response = await _httpClient.GetAsync(Routes.BookEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.OrderBy));
        return await response.ToPaginatedResult<BookResponse>();
    }

    public async Task<IResult<int>> SaveAsync(AddEditBookCommand request)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.BookEndpoints.BaseRoute, request);
        return await response.ToResult<int>();
    }

    public async Task<IResult<string>> UpdateConditionAsync(ChangeBookConditionCommand request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Routes.BookEndpoints.BaseRoute}/{request.Id}", request);
        return await response.ToResult<string>();
    }

    public async Task<IResult<int>> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{Routes.BookEndpoints.BaseRoute}/{id}");
        return await response.ToResult<int>();
    }
}