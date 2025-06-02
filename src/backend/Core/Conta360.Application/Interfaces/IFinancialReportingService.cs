using Conta360.Core.Common;
using System.IO;

namespace Conta360.Application.Interfaces
{
    public interface IFinancialReportingService
    {
        Task<OperationResult<Stream>> GenerateBalanceSheetAsync(DateTime asOfDate);
        Task<OperationResult<Stream>> GenerateIncomeStatementAsync(DateTime startDate, DateTime endDate);
    }
}