using Quark.Core.Features.Books.Commands;

namespace Quark.Client.HttpClients.Masters.Books;

public interface IBookHttpClient
{
    Task<IResult<BookResponse>> GetByIdAsync(Guid id);

    Task<PaginatedResult<BookResponse>> GetAllPaginatedAsync(PagedRequest pagedRequest);

    Task<IResult<Guid>> SaveAsync(AddEditBookCommand request);
    
    Task<IResult<Guid>> SaveHeaderAsync(AddEditBookHeaderCommand request);

    Task<IResult<string>> UpdateConditionAsync(ChangeBookConditionCommand request);

    Task<IResult<string>> ExportToExcelAsync(string searchString = "");

    Task<IResult<Guid>> DeleteAsync(Guid id);
}