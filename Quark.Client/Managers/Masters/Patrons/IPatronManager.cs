using Quark.Core.Features.Patrons.Commands;
using Quark.Core.Requests;
using Quark.Core.Responses;
using Quark.Shared.Wrapper;

namespace Quark.Client.Managers.Masters.Patrons;

public interface IPatronManager
{
    Task<IResult<int>> DeleteAsync(int id);
    Task<PaginatedResult<PatronResponse>> GetAllPaginatedAsync(PagedRequest request);
    Task<IResult<PatronResponse>> GetByIdAsync(int id);
    Task<IResult<int>> SaveAsync(AddEditPatronCommand command);
}