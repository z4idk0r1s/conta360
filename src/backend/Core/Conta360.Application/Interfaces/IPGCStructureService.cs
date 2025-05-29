using Conta360.Core.Common;

namespace Conta360.Application.Interfaces
{
    public interface IPGCStructureService
    {
        Task<OperationResult<string>> GetPGCStructureAsync(string year);
        Task<OperationResult> ProcessPGCStructureAsync(string rawData);
    }
}