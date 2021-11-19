namespace Quark.Core.Interfaces.Services;

public interface IExcelService
{
    Task<string> ExportAsync<TData>(IEnumerable<TData> data, Dictionary<string, Func<TData, object>> mappings, string sheetName = "Sheet1");
}