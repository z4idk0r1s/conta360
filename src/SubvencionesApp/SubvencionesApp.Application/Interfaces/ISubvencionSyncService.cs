using System.Threading.Tasks;

namespace SubvencionesApp.Application.Interfaces
{
    public interface ISubvencionSyncService
    {
        Task SyncConvocatoriasAsync();
        Task SyncConcesionesAsync();
        Task SyncAllDataAsync();
        Task SyncBeneficiariosAsync();
        Task SyncMasterDataAsync();
        //
        Task SyncAyudasAsync();
        Task SyncAyudasEstadosAsync();
        Task SyncConcesionDetalleAsync();
        Task SyncConvocatoriaDetalleAsync();
        Task SyncFinalidadesAsync();
        Task SyncGrandesBeneficiariosAsync();
        Task SyncInstrumentosAsync();
        Task SyncMinimisAsync();
        Task SyncObjetivosAsync();
        Task SyncOrganosCodigoAdminAsync();
        Task SyncPartidosPoliticosAsync();
        Task SyncPlanesEstrategicosAsync();
        Task SyncPlanesEstrategicosDetalleAsync();
        Task SyncPlazosAsync();
        Task SyncRegionesAsync();
        Task SyncReglamentosAsync();
        Task SyncSancionesAsync();
        Task SyncSancionesDetalleAsync();
        Task SyncSectoresProductosAsync();
        Task SyncSuscripcionesAsync();
        Task SyncTercerosAsync();
        Task SyncTiposBeneficiarioAsync();
    }
}