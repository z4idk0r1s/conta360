using Conta360.Application.Interfaces;
using Conta360.Core.Common;
using System.IO;
using ClosedXML.Excel;
using EPPlus.Core; // Assuming EPPlus might be used, though ClosedXML is primary
using System.Reflection; // For mapping T to columns

namespace Conta360.Infrastructure.Excel.Services
{
    public class ExcelProcessor : IExcelProcessor
    {
        public Task<OperationResult<IEnumerable<T>>> ReadDataFromExcelAsync<T>(Stream fileStream, string sheetName)
        {
            // Placeholder implementation for reading Excel data
            using (var workbook = new XLWorkbook(fileStream))
            {
                var worksheet = workbook.Worksheet(sheetName);
                if (worksheet == null)
                {
                    return Task.FromResult(OperationResult<IEnumerable<T>>.Failure(new Error("Excel.SheetNotFound", $"Sheet '{sheetName}' not found.")));
                }

                var data = new List<T>();
                // Simplified: assumes first row is headers, subsequent rows are data
                var headerRow = worksheet.FirstRowUsed();
                var firstDataRow = worksheet.FirstRowUsed().RowBelow();

                if (headerRow == null || firstDataRow == null)
                {
                     return Task.FromResult(OperationResult<IEnumerable<T>>.Success(data.AsEnumerable()));
                }

                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var headerMap = new Dictionary<string, int>();
                foreach (var cell in headerRow.CellsUsed())
                {
                    headerMap[cell.GetString().Trim()] = cell.Address.ColumnNumber;
                }

                foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
                {
                    try
                    {
                        var item = Activator.CreateInstance<T>();
                        foreach (var prop in properties)
                        {
                            if (headerMap.TryGetValue(prop.Name, out var colIndex))
                            {
                                var cell = row.Cell(colIndex);
                                if (cell != null && !cell.IsEmpty())
                                {
                                    var convertedValue = Convert.ChangeType(cell.GetValue<string>(), prop.PropertyType);
                                    prop.SetValue(item, convertedValue);
                                }
                            }
                        }
                        data.Add(item);
                    }
                    catch (Exception ex)
                    {
                        // Log error or add to result errors
                        return Task.FromResult(OperationResult<IEnumerable<T>>.Failure(new Error("Excel.ReadError", $"Error reading row: {ex.Message}")));
                    }
                }
                return Task.FromResult(OperationResult<IEnumerable<T>>.Success(data.AsEnumerable()));
            }
        }

        public Task<OperationResult> WriteDataToExcelAsync<T>(IEnumerable<T> data, Stream outputStream, string sheetName)
        {
            // Placeholder implementation for writing Excel data
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet(sheetName);
                worksheet.Cell(1, 1).InsertTable(data); // Simple insert table
                workbook.SaveAs(outputStream);
            }
            return Task.FromResult(OperationResult.Success());
        }
    }
}