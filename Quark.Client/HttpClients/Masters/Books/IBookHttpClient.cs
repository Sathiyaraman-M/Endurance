using Quark.Core.Features.Books.Commands;

namespace Quark.Client.HttpClients.Masters.Books;

public interface IBookHttpClient
{
    Task<IResult<BookResponse>> GetByIdAsync(int id);

    Task<PaginatedResult<BookResponse>> GetAllPaginatedAsync(PagedRequest pagedRequest);

    Task<IResult<int>> SaveAsync(AddEditBookCommand request);

    Task<IResult<string>> UpdateConditionAsync(ChangeBookConditionCommand request);

    Task<IResult<string>> ExportToExcelAsync(string searchString = "");

    Task<IResult<int>> DeleteAsync(int id);
}