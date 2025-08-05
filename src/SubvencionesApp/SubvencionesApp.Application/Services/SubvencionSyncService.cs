using AutoMapper;
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.Interfaces;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.Services
{
    public class SubvencionSyncService : ISubvencionSyncService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExternalSubvencionesService _externalService;
        private readonly IMapper _mapper;

        public SubvencionSyncService(
            IUnitOfWork unitOfWork,
            IExternalSubvencionesService externalService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _externalService = externalService;
            _mapper = mapper;
        }
        
        public Task SyncInstrumentosAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SyncConvocatoriasAsync()
        {
            var externas = await _externalService.GetConvocatoriasAsync();
            var db = await _unitOfWork.Convocatorias.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Convocatoria>(e));

            await _unitOfWork.Convocatorias.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncConcesionesAsync()
        {
            var externas = await _externalService.GetConcesionesAsync();
            var db = await _unitOfWork.Concesiones.GetAllAsync();
            var dbIds = db.Select(e => e.IdConcesion).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.IdConcesion))
                .Select(e => _mapper.Map<Concesion>(e));

            await _unitOfWork.Concesiones.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncAllDataAsync()
        {
            await SyncConvocatoriasAsync();
            await SyncConcesionesAsync();
            await SyncAyudasAsync();
            await SyncAyudasEstadosAsync();
            await SyncConcesionDetalleAsync();
            await SyncConvocatoriaDetalleAsync();
            await SyncFinalidadesAsync();
            await SyncGrandesBeneficiariosAsync();
            await SyncInstrumentosAsync();
            await SyncMinimisAsync();
            await SyncObjetivosAsync();
            await SyncOrganosCodigoAdminAsync();
            await SyncPartidosPoliticosAsync();
            await SyncPlanesEstrategicosAsync();
            await SyncPlanesEstrategicosDetalleAsync();
            await SyncPlazosAsync();
            await SyncRegionesAsync();
            await SyncReglamentosAsync();
            await SyncSancionesAsync();
            await SyncSancionesDetalleAsync();
            await SyncSectoresProductosAsync();
            await SyncSuscripcionesAsync();
            await SyncTercerosAsync();
            await SyncTiposBeneficiarioAsync();
            await SyncBeneficiariosAsync();
        }

        public async Task SyncBeneficiariosAsync()
        {
            var externas = await _externalService.GetBeneficiariosAsync();
            var db = await _unitOfWork.Beneficiarios.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Beneficiario>(e));

            await _unitOfWork.Beneficiarios.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncAyudasAsync()
        {
            var externas = await _externalService.GetAyudasAsync();
            var db = await _unitOfWork.Ayudas.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Ayuda>(e));

            await _unitOfWork.Ayudas.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncAyudasEstadosAsync()
        {
            var externas = await _externalService.GetAyudasEstadosAsync();
            var db = await _unitOfWork.AyudasEstados.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<AyudaEstado>(e));

            await _unitOfWork.AyudasEstados.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncConcesionDetalleAsync()
        {
            var externas = await _externalService.GetConcesionesDetalleAsync();
            var db = await _unitOfWork.ConcesionesDetalle.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<ConcesionDetalle>(e));

            await _unitOfWork.ConcesionesDetalle.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncConvocatoriaDetalleAsync()
        {
            var externas = await _externalService.GetConvocatoriasDetalleAsync();
            var db = await _unitOfWork.ConvocatoriasDetalle.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<ConvocatoriaDetalle>(e));

            await _unitOfWork.ConvocatoriasDetalle.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncFinalidadesAsync()
        {
            var externas = await _externalService.GetFinalidadesAsync();
            var db = await _unitOfWork.Finalidades.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Finalidad>(e));

            await _unitOfWork.Finalidades.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncGrandesBeneficiariosAsync()
        {
            var externas = await _externalService.GetGrandesBeneficiariosAsync();
            var db = await _unitOfWork.GrandesBeneficiarios.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<GrandeBeneficiario>(e));

            await _unitOfWork.GrandesBeneficiarios.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncMinimisAsync()
        {
            var externas = await _externalService.GetMinimisAsync();
            var db = await _unitOfWork.Minimis.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Minimis>(e));

            await _unitOfWork.Minimis.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncObjetivosAsync()
        {
            var externas = await _externalService.GetObjetivosAsync();
            var db = await _unitOfWork.Objetivos.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Objetivo>(e));

            await _unitOfWork.Objetivos.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncOrganosCodigoAdminAsync()
        {
            var externas = await _externalService.GetOrganosCodigoAdminAsync();
            var db = await _unitOfWork.OrganosCodigoAdmin.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<OrganosCodigoAdmin>(e));

            await _unitOfWork.OrganosCodigoAdmin.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncPartidosPoliticosAsync()
        {
            var externas = await _externalService.GetPartidosPoliticosAsync();
            var db = await _unitOfWork.PartidosPoliticos.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<PartidoPolitico>(e));

            await _unitOfWork.PartidosPoliticos.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncPlanesEstrategicosAsync()
        {
            var externas = await _externalService.GetPlanesEstrategicosAsync();
            var db = await _unitOfWork.PlanesEstrategicos.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<PlanEstrategico>(e));

            await _unitOfWork.PlanesEstrategicos.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncPlanesEstrategicosDetalleAsync()
        {
            var externas = await _externalService.GetPlanesEstrategicosDetalleAsync();
            var db = await _unitOfWork.PlanesEstrategicosDetalle.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<PlanEstrategicoDetalle>(e));

            await _unitOfWork.PlanesEstrategicosDetalle.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncPlazosAsync()
        {
            var externas = await _externalService.GetPlazosAsync();
            var db = await _unitOfWork.Plazos.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Plazo>(e));

            await _unitOfWork.Plazos.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncRegionesAsync()
        {
            var externas = await _externalService.GetRegionesAsync();
            var db = await _unitOfWork.Regiones.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Region>(e));

            await _unitOfWork.Regiones.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncReglamentosAsync()
        {
            var externas = await _externalService.GetReglamentosAsync();
            var db = await _unitOfWork.Reglamentos.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Reglamento>(e));

            await _unitOfWork.Reglamentos.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSancionesAsync()
        {
            var externas = await _externalService.GetSancionesAsync();
            var db = await _unitOfWork.Sanciones.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Sancion>(e));

            await _unitOfWork.Sanciones.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSancionesDetalleAsync()
        {
            var externas = await _externalService.GetSancionesDetalleAsync();
            var db = await _unitOfWork.SancionesDetalle.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<SancionDetalle>(e));

            await _unitOfWork.SancionesDetalle.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSectoresProductosAsync()
        {
            var externas = await _externalService.GetSectoresProductosAsync();
            var db = await _unitOfWork.SectoresProductos.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<SectorProducto>(e));

            await _unitOfWork.SectoresProductos.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSuscripcionesAsync()
        {
            var externas = await _externalService.GetSuscripcionesAsync();
            var db = await _unitOfWork.Suscripciones.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Suscripcion>(e));

            await _unitOfWork.Suscripciones.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncTercerosAsync()
        {
            var externas = await _externalService.GetTercerosAsync();
            var db = await _unitOfWork.Terceros.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<Tercero>(e));

            await _unitOfWork.Terceros.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncTiposBeneficiarioAsync()
        {
            var externas = await _externalService.GetTiposBeneficiarioAsync();
            var db = await _unitOfWork.TiposBeneficiario.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => _mapper.Map<TipoBeneficiario>(e));

            await _unitOfWork.TiposBeneficiario.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }
    }
}