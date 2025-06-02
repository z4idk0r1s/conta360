using Conta360.Application.Interfaces;
using Conta360.Core.Common;
using ClosedXML.Excel;
using System.Reflection;

namespace Conta360.Infrastructure.Excel.Services
{
    public class ExcelProcessor : IExcelProcessor
    {
        public Task<OperationResult<IEnumerable<T>>> ReadDataFromExcelAsync<T>(Stream fileStream, string sheetName)
        {
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet(sheetName);

            if (worksheet == null)
            {
                return Task.FromResult(OperationResult.Failure<IEnumerable<T>>(
                    new Error("Excel.SheetNotFound", $"Sheet '{sheetName}' not found.")));
            }

            var rows = worksheet.RowsUsed().ToList();
            if (rows.Count < 2)
            {
                return Task.FromResult(OperationResult.Success(Enumerable.Empty<T>()));
            }

            var headerRow = rows.First();
            var dataRows = rows.Skip(1);
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var headerMap = headerRow.CellsUsed()
                                     .ToDictionary(c => c.GetString().Trim(), c => c.Address.ColumnNumber);

            var resultList = new List<T>();

            foreach (var row in dataRows)
            {
                try
                {
                    var instance = Activator.CreateInstance<T>();
                    foreach (var prop in properties)
                    {
                        if (headerMap.TryGetValue(prop.Name, out var colIndex))
                        {
                            var cell = row.Cell(colIndex);
                            if (!cell.IsEmpty())
                            {
                                var cellValue = cell.GetValue<string>();
                                object? converted = ConvertValue(cellValue, prop.PropertyType);
                                prop.SetValue(instance, converted);
                            }
                        }
                    }
                    resultList.Add(instance);
                }
                catch (Exception ex)
                {
                    return Task.FromResult(OperationResult.Failure<IEnumerable<T>>(
                        new Error("Excel.ReadError", $"Failed to parse row: {ex.Message}")));
                }
            }

            return Task.FromResult(OperationResult.Success<IEnumerable<T>>(resultList));
        }

        public Task<OperationResult> WriteDataToExcelAsync<T>(IEnumerable<T> data, Stream outputStream, string sheetName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            // Escribir encabezados
            for (int i = 0; i < properties.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
            }

            // Escribir datos
            int row = 2;
            foreach (var item in data)
            {
                for (int col = 0; col < properties.Count; col++)
                {
                    var propValue = properties[col].GetValue(item);

                    // Convertimos explícitamente al tipo soportado por ClosedXML
                    if (propValue is string strVal)
                        worksheet.Cell(row, col + 1).Value = strVal;
                    else if (propValue is int intVal)
                        worksheet.Cell(row, col + 1).Value = intVal;
                    else if (propValue is decimal decVal)
                        worksheet.Cell(row, col + 1).Value = decVal;
                    else if (propValue is double dblVal)
                        worksheet.Cell(row, col + 1).Value = dblVal;
                    else if (propValue is DateTime dtVal)
                        worksheet.Cell(row, col + 1).Value = dtVal;
                    else if (propValue is bool boolVal)
                        worksheet.Cell(row, col + 1).Value = boolVal;
                    else
                        worksheet.Cell(row, col + 1).Value = propValue?.ToString() ?? string.Empty;
                }
                row++;
            }

            workbook.SaveAs(outputStream);
            return Task.FromResult(OperationResult.Success());
        }

        private object? ConvertValue(string value, Type targetType)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            try
            {
                return Convert.ChangeType(value, underlyingType);
            }
            catch
            {
                if (underlyingType == typeof(DateTime) && DateTime.TryParse(value, out var dt)) return dt;
                if (underlyingType == typeof(int) && int.TryParse(value, out var i)) return i;
                if (underlyingType == typeof(decimal) && decimal.TryParse(value, out var d)) return d;
                if (underlyingType == typeof(bool) && bool.TryParse(value, out var b)) return b;
                if (underlyingType == typeof(double) && double.TryParse(value, out var dbl)) return dbl;
                return value;
            }
        }
    }
}
