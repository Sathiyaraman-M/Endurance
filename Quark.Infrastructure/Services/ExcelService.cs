using OfficeOpenXml;
using OfficeOpenXml.Style;
using Quark.Core.Interfaces.Services;
using System.Drawing;

namespace Quark.Infrastructure.Services;
public class ExcelService : IExcelService
{
    public async Task<string> ExportAsync<TData>(IEnumerable<TData> data, Dictionary<string, Func<TData, object>> mappings, string sheetName = "Sheet1")
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var p = new ExcelPackage();
        p.Workbook.Properties.Author = "Quark";
        p.Workbook.Worksheets.Add("Audit Trails");

        var ws = p.Workbook.Worksheets[0];
        ws.Name = sheetName;
        ws.Cells.Style.Font.Size = 11;
        ws.Cells.Style.Font.Name = "Calibri";

        var colIndex = 1;
        var rowIndex = 1;

        var headers = mappings.Keys.Select(x => x).ToList();

        foreach (var header in headers)
        {
            var cell = ws.Cells[rowIndex, colIndex];

            var fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.LightBlue);

            var border = cell.Style.Border;
            border.Bottom.Style =
                border.Top.Style =
                    border.Left.Style =
                        border.Right.Style = ExcelBorderStyle.Thin;

            cell.Value = header;

            colIndex++;
        }

        var dataList = data.ToList();
        foreach (var item in dataList)
        {
            colIndex = 1;
            rowIndex++;

            var result = headers.Select(header => mappings[header](item));

            foreach (var value in result)
            {
                ws.Cells[rowIndex, colIndex++].Value = value;
            }
        }

        using (ExcelRange autoFilterCells = ws.Cells[1, 1, dataList.Count + 1, headers.Count])
        {
            autoFilterCells.AutoFilter = true;
            autoFilterCells.AutoFitColumns();
        }


        var byteArray = await p.GetAsByteArrayAsync();
        return Convert.ToBase64String(byteArray);
    }
}