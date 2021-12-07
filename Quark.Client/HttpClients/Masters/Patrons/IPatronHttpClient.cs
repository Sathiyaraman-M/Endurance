using Quark.Core.Features.Patrons.Commands;

namespace Quark.Client.HttpClients.Masters.Patrons;

public interface IPatronHttpClient
{
    Task<IResult<Guid>> DeleteAsync(Guid id);
    Task<PaginatedResult<PatronResponse>> GetAllPaginatedAsync(PagedRequest request);
    Task<IResult<PatronResponse>> GetByIdAsync(Guid id);
    Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    Task<IResult<Guid>> SaveAsync(AddEditPatronCommand command);
}