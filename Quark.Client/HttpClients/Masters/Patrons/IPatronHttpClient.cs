using Quark.Core.Features.Patrons.Commands;
using Quark.Core.Requests;
using Quark.Core.Responses;
using Quark.Shared.Wrapper;

namespace Quark.Client.HttpClients.Masters.Patrons;

public interface IPatronHttpClient
{
    Task<IResult<int>> DeleteAsync(int id);
    Task<PaginatedResult<PatronResponse>> GetAllPaginatedAsync(PagedRequest request);
    Task<IResult<PatronResponse>> GetByIdAsync(int id);
    Task<IResult<int>> SaveAsync(AddEditPatronCommand command);
}