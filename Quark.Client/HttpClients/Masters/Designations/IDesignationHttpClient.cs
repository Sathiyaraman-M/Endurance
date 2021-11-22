using Quark.Core.Features.Designations.Commands;

namespace Quark.Client.HttpClients.Masters.Designations;

public interface IDesignationHttpClient
{
    Task<IResult<List<DesignationResponse>>> GetAllAsync();

    Task<IResult<int>> SaveAsync(AddEditDesignationCommand request);

    Task<IResult<string>> ExportToExcelAsync(string searchString = "");

    Task<IResult<int>> DeleteAsync(int id);
}