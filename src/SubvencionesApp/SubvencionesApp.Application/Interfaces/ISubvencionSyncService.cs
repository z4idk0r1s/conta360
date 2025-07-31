using System.Threading.Tasks;

namespace SubvencionesApp.Application.Interfaces
{
    public interface ISubvencionSyncService
    {
        Task SyncConvocatoriasAsync();
        Task SyncConcesionesAsync();
        Task SyncBeneficiariosAsync();
        Task SyncAllMasterDataAsync();
        Task SyncAllDataAsync();
    }
}