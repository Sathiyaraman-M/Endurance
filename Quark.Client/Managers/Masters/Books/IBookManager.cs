using Quark.Core.Features.Books.Commands;
using Quark.Core.Requests;
using Quark.Core.Responses;
using Quark.Shared.Wrapper;

namespace Quark.Client.Managers.Masters.Books;

public interface IBookManager
{
    Task<IResult<BookResponse>> GetByIdAsync(int id);

    Task<PaginatedResult<BookResponse>> GetAllPaginatedAsync(PagedRequest pagedRequest);

    Task<IResult<int>> SaveAsync(AddEditBookCommand request);

    Task<IResult<string>> UpdateConditionAsync(ChangeBookConditionCommand request);

    Task<IResult<int>> DeleteAsync(int id);
}