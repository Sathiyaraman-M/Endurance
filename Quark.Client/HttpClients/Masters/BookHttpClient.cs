using Quark.Core.Features.Books.Commands;

namespace Quark.Client.HttpClients.Masters;

public class BookHttpClient
{
    private readonly HttpClient _httpClient;

    public BookHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult<BookResponse>> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{Routes.BookEndpoints.BaseRoute}/{id}");
        return await response.ToResult<BookResponse>();
    }

    public async Task<PaginatedResult<BookResponse>> GetAllPaginatedAsync(PagedRequest request)
    {
        var response = await _httpClient.GetAsync(Routes.BookEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.OrderBy));
        return await response.ToPaginatedResult<BookResponse>();
    }

    public async Task<IResult<Guid>> SaveAsync(AddEditBookCommand request)
    {
        var response = await _httpClient.PostAsJsonAsync(Routes.BookEndpoints.BaseRoute, request);
        return await response.ToResult<Guid>();
    }

    public async Task<IResult<Guid>> DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{Routes.BookEndpoints.BaseRoute}/{id}");
        return await response.ToResult<Guid>();
    }

    public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
    {
        var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
            ? Routes.BookEndpoints.Export : Routes.BookEndpoints.ExportFiltered(searchString));
        return await response.ToResult<string>();
    }
}