using SubvencionesApp.Application.Interfaces;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.Services
{
    public class SubvencionSyncService : ISubvencionSyncService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExternalSubvencionesService _externalService;

        public SubvencionSyncService(
            IUnitOfWork unitOfWork, 
            IExternalSubvencionesService externalService)
        {
            _unitOfWork = unitOfWork;
            _externalService = externalService;
        }

        public async Task SyncConvocatoriasAsync()
        {
            var convocatoriasExternas = await _externalService.GetConvocatoriasAsync();
            var convocatoriasDb = await _unitOfWork.Convocatorias.GetAllAsync();
            var convocatoriasDbIds = convocatoriasDb.Select(c => c.Id).ToHashSet();

            var nuevasConvocatorias = convocatoriasExternas
                .Where(c => !convocatoriasDbIds.Contains(c.Id))
                .Select(c => new Convocatoria
                {
                    Id = c.Id,
                    Objeto = c.Objeto,
                    Extracto = c.Extracto,
                    Enlace = c.Enlace,
                    ReferenciaBDNS = c.ReferenciaBDNS,
                    Ejercicio = c.Ejercicio,
                    FechaPublicacion = c.FechaPublicacion,
                    TipoConvocatoriaId = c.TipoConvocatoriaId,
                    TipoSubvencionId = c.TipoSubvencionId,
                    OrganismoId = c.OrganismoId,
                    SituacionEntornoId = c.SituacionEntornoId
                });

            await _unitOfWork.Convocatorias.AddRangeAsync(nuevasConvocatorias);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncConcesionesAsync()
        {
            var concesionesExternas = await _externalService.GetConcesionesAsync();
            var concesionesDb = await _unitOfWork.Concesiones.GetAllAsync();
            var concesionesDbIds = concesionesDb.Select(c => c.IdConcesion).ToHashSet();

            var nuevasConcesiones = concesionesExternas
                .Where(c => !concesionesDbIds.Contains(c.IdConcesion))
                .Select(c => new Concesion
                {
                    IdConcesion = c.IdConcesion,
                    ReferenciaBDNS = c.ReferenciaBDNS,
                    ReferenciaPublicacion = c.ReferenciaPublicacion,
                    Importe = c.Importe,
                    Ejercicio = c.Ejercicio,
                    FechaConcesion = c.FechaConcesion,
                    BeneficiarioId = c.BeneficiarioId,
                    ConvocatoriaId = c.ConvocatoriaId
                });

            await _unitOfWork.Concesiones.AddRangeAsync(nuevasConcesiones);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncAllDataAsync()
        {
            await SyncConvocatoriasAsync();
            await SyncConcesionesAsync();
            // Sincronizar otras entidades según necesidad
        }
    }
}