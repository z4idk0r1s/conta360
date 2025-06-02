using Conta360.Core.Common;
using System.IO;

namespace Conta360.Application.Interfaces
{
    public interface IExcelProcessor
    {
        Task<OperationResult<IEnumerable<T>>> ReadDataFromExcelAsync<T>(Stream fileStream, string sheetName);
        Task<OperationResult> WriteDataToExcelAsync<T>(IEnumerable<T> data, Stream outputStream, string sheetName);
    }
}