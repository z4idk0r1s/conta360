/*using SubvencionesApp.Application.Interfaces;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using System;

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
            await SyncMasterDataAsync();
        }

        public async Task SyncBeneficiariosAsync()
        {
            var externas = await _externalService.GetBeneficiariosAsync();
            var db = await _unitOfWork.Beneficiarios.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => new Beneficiario
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Nif = e.Nif,
                    Tipo = e.Tipo,
                    FechaAlta = e.FechaAlta,
                    FechaBaja = e.FechaBaja
                });

            await _unitOfWork.Beneficiarios.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncMasterDataAsync()
        {
            var externas = await _externalService.GetMasterDataAsync();
            var db = await _unitOfWork.MasterData.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => new MasterData
                {
                    Id = e.Id,
                    Clave = e.Clave,
                    Valor = e.Valor,
                    Descripcion = e.Descripcion
                });

            await _unitOfWork.MasterData.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncAyudasAsync()
        {
            var externas = await _externalService.GetAyudasAsync();
            var db = await _unitOfWork.Ayudas.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => new Ayuda
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    OrganismoId = e.OrganismoId,
                    RegionId = e.RegionId,
                    TipoBeneficiarioId = e.TipoBeneficiarioId,
                    InstrumentoId = e.InstrumentoId
                });

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
                .Select(e => new AyudaEstado
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    InstrumentoId = e.InstrumentoId,
                    TipoBeneficiarioId = e.TipoBeneficiarioId,
                    Estado = e.Estado
                });

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
                .Select(e => new ConcesionDetalle
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    Detalles = e.Detalles,
                    Importe = e.Importe,
                    BeneficiarioId = e.BeneficiarioId,
                    OrganismoId = e.OrganismoId,
                    FechaResolucion = e.FechaResolucion
                });

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
                .Select(e => new ConvocatoriaDetalle
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    Detalles = e.Detalles,
                    Estado = e.Estado,
                    FechaInicio = e.FechaInicio,
                    FechaFin = e.FechaFin,
                    FechaPublicacion = e.FechaPublicacion,
                    OrganismoId = e.OrganismoId,
                    RegionId = e.RegionId,
                    TipoBeneficiarioId = e.TipoBeneficiarioId,
                    InstrumentoId = e.InstrumentoId
                });

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
                .Select(e => new Finalidad
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion
                });

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
                .Select(e => new GrandeBeneficiario
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Tipo = e.Tipo,
                    Importe = e.Importe
                });

            await _unitOfWork.GrandesBeneficiarios.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncInstrumentosAsync()
        {
            var externas = await _externalService.GetInstrumentosAsync();
            var db = await _unitOfWork.Instrumentos.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => new Instrumento
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion
                });

            await _unitOfWork.Instrumentos.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncMinimisAsync()
        {
            var externas = await _externalService.GetMinimisAsync();
            var db = await _unitOfWork.Minimis.GetAllAsync();
            var dbIds = db.Select(e => e.Id).ToHashSet();

            var nuevas = externas
                .Where(e => !dbIds.Contains(e.Id))
                .Select(e => new Minimis
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    Estado = e.Estado,
                    FechaInicio = e.FechaInicio,
                    FechaFin = e.FechaFin
                });

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
                .Select(e => new Objetivo
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion
                });

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
                .Select(e => new OrganosCodigoAdmin
                {
                    Id = e.Id,
                    CodigoAdmin = e.CodigoAdmin,
                    Nombre = e.Nombre
                });

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
                .Select(e => new PartidoPolitico
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Importe = e.Importe,
                    Fecha = e.Fecha,
                    OrganismoId = e.OrganismoId
                });

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
                .Select(e => new PlanEstrategico
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    Estado = e.Estado,
                    FechaAprobacion = e.FechaAprobacion
                });

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
                .Select(e => new PlanEstrategicoDetalle
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    Estado = e.Estado,
                    FechaAprobacion = e.FechaAprobacion
                });

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
                .Select(e => new Plazo
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    FechaInicio = e.FechaInicio,
                    FechaFin = e.FechaFin,
                    ConvocatoriaId = e.ConvocatoriaId
                });

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
                .Select(e => new Region
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                });

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
                .Select(e => new Reglamento
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Tipo = e.Tipo
                });

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
                .Select(e => new Sancion
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Motivo = e.Motivo,
                    Sancion = e.Sancion,
                    Estado = e.Estado
                });

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
                .Select(e => new SancionDetalle
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Motivo = e.Motivo,
                    Sancion = e.Sancion,
                    Estado = e.Estado,
                    Detalles = e.Detalles,
                    FechaResolucion = e.FechaResolucion,
                    OrganismoId = e.OrganismoId
                });

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
                .Select(e => new SectorProducto
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion
                });

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
                .Select(e => new Suscripcion
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Email = e.Email,
                    FechaInicio = e.FechaInicio,
                    Activa = e.Activa
                });

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
                .Select(e => new Tercero
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Nif = e.Nif,
                    Tipo = e.Tipo
                });

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
                .Select(e => new TipoBeneficiario
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion
                });

            await _unitOfWork.TiposBeneficiario.AddRangeAsync(nuevas);
            await _unitOfWork.CommitAsync();
        }
    }
}*/