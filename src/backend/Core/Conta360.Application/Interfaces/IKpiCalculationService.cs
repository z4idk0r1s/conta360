using Conta360.Core.Common;

namespace Conta360.Application.Interfaces
{
    public interface IKpiCalculationService
    {
        Task<OperationResult<decimal>> CalculateProfitMarginAsync(DateTime startDate, DateTime endDate);
        Task<OperationResult<decimal>> CalculateLiquidityRatioAsync(DateTime asOfDate);
        // More KPI methods
    }
}