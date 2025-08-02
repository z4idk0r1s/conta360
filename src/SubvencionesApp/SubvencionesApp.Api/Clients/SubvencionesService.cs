using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SubvencionesApp.Infrastructure.Interfaces;
using SubvencionesApp.Api.Clients;
using System;

namespace SubvencionesApp.Application.UseCases
{
    public class SubvencionesService : ISubvencionesService, ISubvencionSyncService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly InfoSubvencionesApiClient _apiClient;

        public SubvencionesService(IUnitOfWork unitOfWork, InfoSubvencionesApiClient apiClient)
        {
            _unitOfWork = unitOfWork;
            _apiClient = apiClient;
        }

        public async Task SyncConvocatoriasAsync()
        {
            var convocatoriasApi = await _apiClient.GetConvocatoriasAsync();
            var convocatoriasDb = await _unitOfWork.Convocatorias.GetAllAsync();
            var convocatoriasDbIds = convocatoriasDb.Select(c => c.Id).ToHashSet();

            var nuevasConvocatorias = convocatoriasApi
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
                    TipoConvocatoriaId = c.TipoConvocatoria?.Id,
                    TipoSubvencionId = c.TipoSubvencion?.Id,
                    OrganismoId = c.Organismo?.Id,
                    SituacionEntornoId = c.SituacionEntorno?.Id
                });

            await _unitOfWork.Convocatorias.AddRangeAsync(nuevasConvocatorias);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncConcesionesAsync()
        {
            var concesionesApi = await _apiClient.GetConcesionesAsync();
            var concesionesDb = await _unitOfWork.Concesiones.GetAllAsync();
            var concesionesDbIds = concesionesDb.Select(c => c.IdConcesion).ToHashSet();

            var nuevasConcesiones = concesionesApi
                .Where(c => !concesionesDbIds.Contains(c.IdConcesion))
                .Select(c => new Concesion
                {
                    IdConcesion = c.IdConcesion,
                    ReferenciaBDNS = c.ReferenciaBDNS,
                    ReferenciaPublicacion = c.ReferenciaPublicacion,
                    Importe = c.Importe,
                    Ejercicio = c.Ejercicio,
                    FechaConcesion = c.FechaConcesion,
                    BeneficiarioId = c.Beneficiario?.Id,
                    ConvocatoriaId = c.Convocatoria?.Id
                });

            await _unitOfWork.Concesiones.AddRangeAsync(nuevasConcesiones);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncAllDataAsync()
        {
            var syncTasks = new List<Task>
            {
                SyncAccionesAsync(),
                SyncAgrupacionesAsync(),
                SyncAreasAsync(),
                SyncBeneficiariosAsync(),
                SyncConcesionesAsync(),
                SyncConvocatoriasAsync(),
                SyncEntidadesAsync(),
                SyncEstadosAsync(),
                SyncFormasPagoAsync(),
                SyncLineasAsync(),
                SyncMunicipiosAsync(),
                SyncOrganismosAsync(),
                SyncProgramasAsync(),
                SyncProvinciasAsync(),
                SyncSectoresAsync(),
                SyncSituacionesEntornoAsync(),
                SyncSubtiposSubvencionAsync(),
                SyncTiposBeneficiarioAsync(),
                SyncTiposConvocatoriaAsync(),
                SyncTiposOrganismoAsync(),
                SyncTiposSubvencionAsync(),
                SyncTramosAsync(),
                SyncUnidadesAdministrativasAsync(),
                SyncAyudasAsync(),
                SyncAyudasEstadosAsync(),
                SyncConcesionesDetalleAsync(),
                SyncConvocatoriasDetalleAsync(),
                SyncFinalidadesAsync(),
                SyncGrandesBeneficiariosAsync(),
                SyncInstrumentosAsync(),
                SyncMinimisAsync(),
                SyncObjetivosAsync(),
                SyncOrganosCodigoAdminAsync(),
                SyncPartidosPoliticosAsync(),
                SyncPlanesEstrategicosAsync(),
                SyncPlanesEstrategicosDetalleAsync(),
                SyncPlazosAsync(),
                SyncRegionesAsync(),
                SyncReglamentosAsync(),
                SyncSancionesAsync(),
                SyncSancionesDetalleAsync(),
                SyncSectoresProductosAsync(),
                SyncSuscripcionesAsync(),
                SyncTercerosAsync()
            };
            await Task.WhenAll(syncTasks);
        }
        
        // Métodos de obtención de datos (existentes)

        public async Task<IEnumerable<AccionDto>> GetAccionesAsync()
        {
            var acciones = await _unitOfWork.Acciones.GetAllAsync();
            return acciones.Select(a => new AccionDto { Id = a.Id, Descripcion = a.Descripcion });
        }

        public async Task<IEnumerable<AgrupacionDto>> GetAgrupacionesAsync()
        {
            var agrupaciones = await _unitOfWork.Agrupaciones.GetAllAsync();
            return agrupaciones.Select(a => new AgrupacionDto { Id = a.Id, Descripcion = a.Descripcion });
        }

        public async Task<IEnumerable<AreaDto>> GetAreasAsync()
        {
            var areas = await _unitOfWork.Areas.GetAllAsync();
            return areas.Select(a => new AreaDto { Id = a.Id, Descripcion = a.Descripcion });
        }

        public async Task<IEnumerable<BeneficiarioDto>> GetBeneficiariosAsync()
        {
            var beneficiarios = await _unitOfWork.Beneficiarios.GetAllAsync();
            return beneficiarios.Select(b => new BeneficiarioDto { Id = b.Id, Nombre = b.Nombre, Identificacion = b.Identificacion });
        }

        public async Task<IEnumerable<ConcesionDto>> GetConcesionesAsync()
        {
            var concesiones = await _unitOfWork.Concesiones.GetAllAsync();
            return concesiones.Select(c => new ConcesionDto { IdConcesion = c.IdConcesion, ReferenciaBDNS = c.ReferenciaBDNS, ReferenciaPublicacion = c.ReferenciaPublicacion, Importe = c.Importe, Ejercicio = c.Ejercicio, FechaConcesion = c.FechaConcesion });
        }

        public async Task<IEnumerable<ConvocatoriaDto>> GetConvocatoriasAsync()
        {
            var convocatorias = await _unitOfWork.Convocatorias.GetAllAsync();
            return convocatorias.Select(c => new ConvocatoriaDto { Id = c.Id, Objeto = c.Objeto, Extracto = c.Extracto, Enlace = c.Enlace, ReferenciaBDNS = c.ReferenciaBDNS, Ejercicio = c.Ejercicio, FechaPublicacion = c.FechaPublicacion });
        }

        public async Task<IEnumerable<DatosEstadisticosDto>> GetDatosEstadisticosAsync()
        {
            var datosEstadisticos = await _apiClient.GetDatosEstadisticosAsync();
            return datosEstadisticos.Select(d => new DatosEstadisticosDto { Descripcion = d.Descripcion, TotalConcesiones = d.TotalConcesiones, ImporteTotal = d.ImporteTotal });
        }

        public async Task<IEnumerable<EntidadDto>> GetEntidadesAsync()
        {
            var entidades = await _unitOfWork.Entidades.GetAllAsync();
            return entidades.Select(e => new EntidadDto { Id = e.Id, Descripcion = e.Descripcion });
        }

        public async Task<IEnumerable<EstadoDto>> GetEstadosAsync()
        {
            var estados = await _unitOfWork.Estados.GetAllAsync();
            return estados.Select(e => new EstadoDto { Id = e.Id, Descripcion = e.Descripcion });
        }

        public async Task<IEnumerable<FormaPagoDto>> GetFormasPagoAsync()
        {
            var formasPago = await _unitOfWork.FormasPago.GetAllAsync();
            return formasPago.Select(f => new FormaPagoDto { Id = f.Id, Descripcion = f.Descripcion });
        }

        public async Task<IEnumerable<LineaDto>> GetLineasAsync()
        {
            var lineas = await _unitOfWork.Lineas.GetAllAsync();
            return lineas.Select(l => new LineaDto { Id = l.Id, Codigo = l.Codigo, Nombre = l.Nombre });
        }

        public async Task<IEnumerable<MunicipioDto>> GetMunicipiosAsync()
        {
            var municipios = await _unitOfWork.Municipios.GetAllAsync();
            return municipios.Select(m => new MunicipioDto { Id = m.Id, Descripcion = m.Descripcion });
        }

        public async Task<IEnumerable<OrganismoDto>> GetOrganismosAsync()
        {
            var organismos = await _unitOfWork.Organismos.GetAllAsync();
            return organismos.Select(o => new OrganismoDto { Id = o.Id, Descripcion = o.Descripcion });
        }

        public async Task<IEnumerable<ProgramaDto>> GetProgramasAsync()
        {
            var programas = await _unitOfWork.Programas.GetAllAsync();
            return programas.Select(p => new ProgramaDto { Id = p.Id, Codigo = p.Codigo, Descripcion = p.Descripcion });
        }

        public async Task<IEnumerable<ProvinciaDto>> GetProvinciasAsync()
        {
            var provincias = await _unitOfWork.Provincias.GetAllAsync();
            return provincias.Select(p => new ProvinciaDto { Id = p.Id, Descripcion = p.Descripcion });
        }

        public async Task<IEnumerable<SectorDto>> GetSectoresAsync()
        {
            var sectores = await _unitOfWork.Sectores.GetAllAsync();
            return sectores.Select(s => new SectorDto { Id = s.Id, Descripcion = s.Descripcion });
        }

        public async Task<IEnumerable<SituacionEntornoDto>> GetSituacionesEntornoAsync()
        {
            var situacionesEntorno = await _unitOfWork.SituacionesEntorno.GetAllAsync();
            return situacionesEntorno.Select(s => new SituacionEntornoDto { Id = s.Id, Descripcion = s.Descripcion });
        }

        public async Task<IEnumerable<SubtipoSubvencionDto>> GetSubtiposSubvencionAsync()
        {
            var subtiposSubvencion = await _unitOfWork.SubtiposSubvencion.GetAllAsync();
            return subtiposSubvencion.Select(s => new SubtipoSubvencionDto { Id = s.Id, Descripcion = s.Descripcion });
        }

        public async Task<IEnumerable<TipoBeneficiarioDto>> GetTiposBeneficiarioAsync()
        {
            var tiposBeneficiario = await _unitOfWork.TiposBeneficiario.GetAllAsync();
            return tiposBeneficiario.Select(t => new TipoBeneficiarioDto { Id = t.Id, Descripcion = t.Descripcion });
        }

        public async Task<IEnumerable<TipoConvocatoriaDto>> GetTiposConvocatoriaAsync()
        {
            var tiposConvocatoria = await _unitOfWork.TiposConvocatoria.GetAllAsync();
            return tiposConvocatoria.Select(t => new TipoConvocatoriaDto { Id = t.Id, Descripcion = t.Descripcion });
        }

        public async Task<IEnumerable<TipoOrganismoDto>> GetTiposOrganismoAsync()
        {
            var tiposOrganismo = await _unitOfWork.TiposOrganismo.GetAllAsync();
            return tiposOrganismo.Select(t => new TipoOrganismoDto { Id = t.Id, Descripcion = t.Descripcion });
        }

        public async Task<IEnumerable<TipoSubvencionDto>> GetTiposSubvencionAsync()
        {
            var tiposSubvencion = await _unitOfWork.TiposSubvencion.GetAllAsync();
            return tiposSubvencion.Select(t => new TipoSubvencionDto { Id = t.Id, Descripcion = t.Descripcion });
        }

        public async Task<IEnumerable<TramoDto>> GetTramosAsync()
        {
            var tramos = await _unitOfWork.Tramos.GetAllAsync();
            return tramos.Select(t => new TramoDto { Id = t.Id, Descripcion = t.Descripcion });
        }

        public async Task<IEnumerable<UnidadAdministrativaDto>> GetUnidadesAdministrativasAsync()
        {
            var unidadesAdministrativas = await _unitOfWork.UnidadesAdministrativas.GetAllAsync();
            return unidadesAdministrativas.Select(u => new UnidadAdministrativaDto { Id = u.Id, Descripcion = u.Descripcion });
        }

        // Nuevos métodos de sincronización
        
        private async Task SyncGenericAsync<TEntity, TDto>(
            Func<Task<IEnumerable<TDto>>> getApiData,
            Func<Task<IEnumerable<TEntity>>> getDbData,
            Func<TEntity, object> getDbId,
            Func<TDto, TEntity> mapToEntity,
            Func<IEnumerable<TEntity>, Task> addRange,
            Func<Task> commit)
        {
            var apiData = await getApiData();
            var dbData = await getDbData();
            var dbIds = dbData.Select(getDbId).ToHashSet();

            var nuevasEntidades = apiData
                .Where(dto => !dbIds.Contains(GetIdFromDto(dto)))
                .Select(mapToEntity);

            await addRange(nuevasEntidades);
            await commit();
        }

        private object GetIdFromDto(object dto)
        {
            var type = dto.GetType();
            var idProperty = type.GetProperty("Id");
            return idProperty?.GetValue(dto);
        }

        public async Task SyncAccionesAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetAccionesAsync,
                _unitOfWork.Acciones.GetAllAsync,
                e => e.Id,
                dto => new Accion { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.Acciones.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncAgrupacionesAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetAgrupacionesAsync,
                _unitOfWork.Agrupaciones.GetAllAsync,
                e => e.Id,
                dto => new Agrupacion { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.Agrupaciones.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncAreasAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetAreasAsync,
                _unitOfWork.Areas.GetAllAsync,
                e => e.Id,
                dto => new Area { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.Areas.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncBeneficiariosAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetBeneficiariosAsync,
                _unitOfWork.Beneficiarios.GetAllAsync,
                e => e.Id,
                dto => new Beneficiario { Id = dto.Id, Nombre = dto.Nombre, Identificacion = dto.Identificacion },
                _unitOfWork.Beneficiarios.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncEntidadesAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetEntidadesAsync,
                _unitOfWork.Entidades.GetAllAsync,
                e => e.Id,
                dto => new Entidad { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.Entidades.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncEstadosAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetEstadosAsync,
                _unitOfWork.Estados.GetAllAsync,
                e => e.Id,
                dto => new Estado { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.Estados.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncFormasPagoAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetFormasPagoAsync,
                _unitOfWork.FormasPago.GetAllAsync,
                e => e.Id,
                dto => new FormaPago { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.FormasPago.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncLineasAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetLineasAsync,
                _unitOfWork.Lineas.GetAllAsync,
                e => e.Id,
                dto => new Linea { Id = dto.Id, Codigo = dto.Codigo, Nombre = dto.Nombre },
                _unitOfWork.Lineas.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncMunicipiosAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetMunicipiosAsync,
                _unitOfWork.Municipios.GetAllAsync,
                e => e.Id,
                dto => new Municipio { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.Municipios.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncOrganismosAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetOrganismosAsync,
                _unitOfWork.Organismos.GetAllAsync,
                e => e.Id,
                dto => new Organismo { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.Organismos.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncProgramasAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetProgramasAsync,
                _unitOfWork.Programas.GetAllAsync,
                e => e.Id,
                dto => new Programa { Id = dto.Id, Codigo = dto.Codigo, Descripcion = dto.Descripcion },
                _unitOfWork.Programas.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncProvinciasAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetProvinciasAsync,
                _unitOfWork.Provincias.GetAllAsync,
                e => e.Id,
                dto => new Provincia { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.Provincias.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncSectoresAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetSectoresAsync,
                _unitOfWork.Sectores.GetAllAsync,
                e => e.Id,
                dto => new Sector { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.Sectores.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncSituacionesEntornoAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetSituacionesEntornoAsync,
                _unitOfWork.SituacionesEntorno.GetAllAsync,
                e => e.Id,
                dto => new SituacionEntorno { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.SituacionesEntorno.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncSubtiposSubvencionAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetSubtiposSubvencionAsync,
                _unitOfWork.SubtiposSubvencion.GetAllAsync,
                e => e.Id,
                dto => new SubtipoSubvencion { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.SubtiposSubvencion.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncTiposBeneficiarioAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetTiposBeneficiarioAsync,
                _unitOfWork.TiposBeneficiario.GetAllAsync,
                e => e.Id,
                dto => new TipoBeneficiario { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.TiposBeneficiario.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncTiposConvocatoriaAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetTiposConvocatoriaAsync,
                _unitOfWork.TiposConvocatoria.GetAllAsync,
                e => e.Id,
                dto => new TipoConvocatoria { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.TiposConvocatoria.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncTiposOrganismoAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetTiposOrganismoAsync,
                _unitOfWork.TiposOrganismo.GetAllAsync,
                e => e.Id,
                dto => new TipoOrganismo { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.TiposOrganismo.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncTiposSubvencionAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetTiposSubvencionAsync,
                _unitOfWork.TiposSubvencion.GetAllAsync,
                e => e.Id,
                dto => new TipoSubvencion { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.TiposSubvencion.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncTramosAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetTramosAsync,
                _unitOfWork.Tramos.GetAllAsync,
                e => e.Id,
                dto => new Tramo { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.Tramos.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        public async Task SyncUnidadesAdministrativasAsync()
        {
            await SyncGenericAsync(
                _apiClient.GetUnidadesAdministrativasAsync,
                _unitOfWork.UnidadesAdministrativas.GetAllAsync,
                e => e.Id,
                dto => new UnidadAdministrativa { Id = dto.Id, Descripcion = dto.Descripcion },
                _unitOfWork.UnidadesAdministrativas.AddRangeAsync,
                _unitOfWork.CommitAsync);
        }

        // Nuevos métodos de obtención de datos

        public async Task<IEnumerable<AyudaDto>> GetAyudasAsync()
        {
            var ayudas = await _unitOfWork.Ayudas.GetAllAsync();
            return ayudas.Select(a => new AyudaDto { Id = a.Id, Nombre = a.Nombre, Descripcion = a.Descripcion });
        }

        public async Task<IEnumerable<AyudaEstadoDto>> GetAyudasEstadosAsync()
        {
            var ayudasEstados = await _unitOfWork.AyudasEstados.GetAllAsync();
            return ayudasEstados.Select(a => new AyudaEstadoDto { Id = a.Id, Nombre = a.Nombre, Descripcion = a.Descripcion, Estado = a.Estado });
        }

        public async Task<IEnumerable<ConcesionDetalleDto>> GetConcesionesDetalleAsync()
        {
            var concesionesDetalle = await _unitOfWork.ConcesionesDetalle.GetAllAsync();
            return concesionesDetalle.Select(c => new ConcesionDetalleDto { Id = c.Id, Nombre = c.Nombre, Descripcion = c.Descripcion, Detalles = c.Detalles, Importe = c.Importe, FechaResolucion = c.FechaResolucion });
        }

        public async Task<IEnumerable<ConvocatoriaDetalleDto>> GetConvocatoriasDetalleAsync()
        {
            var convocatoriasDetalle = await _unitOfWork.ConvocatoriasDetalle.GetAllAsync();
            return convocatoriasDetalle.Select(c => new ConvocatoriaDetalleDto { Id = c.Id, Nombre = c.Nombre, Descripcion = c.Descripcion, Detalles = c.Detalles, Estado = c.Estado, FechaInicio = c.FechaInicio, FechaFin = c.FechaFin, FechaPublicacion = c.FechaPublicacion });
        }

        public async Task<IEnumerable<FinalidadDto>> GetFinalidadesAsync()
        {
            var finalidades = await _unitOfWork.Finalidades.GetAllAsync();
            return finalidades.Select(f => new FinalidadDto { Id = f.Id, Nombre = f.Nombre, Descripcion = f.Descripcion });
        }

        public async Task<IEnumerable<GrandeBeneficiarioDto>> GetGrandesBeneficiariosAsync()
        {
            var grandesBeneficiarios = await _unitOfWork.GrandesBeneficiarios.GetAllAsync();
            return grandesBeneficiarios.Select(g => new GrandeBeneficiarioDto { Id = g.Id, Nombre = g.Nombre, Tipo = g.Tipo, Importe = g.Importe });
        }

        public async Task<IEnumerable<InstrumentoDto>> GetInstrumentosAsync()
        {
            var instrumentos = await _unitOfWork.Instrumentos.GetAllAsync();
            return instrumentos.Select(i => new InstrumentoDto { Id = i.Id, Nombre = i.Nombre, Descripcion = i.Descripcion });
        }

        public async Task<IEnumerable<MinimisDto>> GetMinimisAsync()
        {
            var minimis = await _unitOfWork.Minimis.GetAllAsync();
            return minimis.Select(m => new MinimisDto { Id = m.Id, Nombre = m.Nombre, Descripcion = m.Descripcion, Estado = m.Estado, FechaInicio = m.FechaInicio, FechaFin = m.FechaFin });
        }

        public async Task<IEnumerable<ObjetivoDto>> GetObjetivosAsync()
        {
            var objetivos = await _unitOfWork.Objetivos.GetAllAsync();
            return objetivos.Select(o => new ObjetivoDto { Id = o.Id, Nombre = o.Nombre, Descripcion = o.Descripcion });
        }

        public async Task<IEnumerable<OrganosCodigoAdminDto>> GetOrganosCodigoAdminAsync()
        {
            var organosCodigoAdmin = await _unitOfWork.OrganosCodigoAdmin.GetAllAsync();
            return organosCodigoAdmin.Select(o => new OrganosCodigoAdminDto { Id = o.Id, CodigoAdmin = o.CodigoAdmin, Nombre = o.Nombre });
        }

        public async Task<IEnumerable<PartidoPoliticoDto>> GetPartidosPoliticosAsync()
        {
            var partidosPoliticos = await _unitOfWork.PartidosPoliticos.GetAllAsync();
            return partidosPoliticos.Select(p => new PartidoPoliticoDto { Id = p.Id, Nombre = p.Nombre, Importe = p.Importe, Fecha = p.Fecha });
        }

        public async Task<IEnumerable<PlanEstrategicoDto>> GetPlanesEstrategicosAsync()
        {
            var planesEstrategicos = await _unitOfWork.PlanesEstrategicos.GetAllAsync();
            return planesEstrategicos.Select(p => new PlanEstrategicoDto { Id = p.Id, Nombre = p.Nombre, Descripcion = p.Descripcion, Estado = p.Estado, FechaAprobacion = p.FechaAprobacion });
        }

        public async Task<IEnumerable<PlanEstrategicoDetalleDto>> GetPlanesEstrategicosDetalleAsync()
        {
            var planesEstrategicosDetalle = await _unitOfWork.PlanesEstrategicosDetalle.GetAllAsync();
            return planesEstrategicosDetalle.Select(p => new PlanEstrategicoDetalleDto { Id = p.Id, Nombre = p.Nombre, Descripcion = p.Descripcion, Estado = p.Estado, FechaAprobacion = p.FechaAprobacion });
        }

        public async Task<IEnumerable<PlazoDto>> GetPlazosAsync()
        {
            var plazos = await _unitOfWork.Plazos.GetAllAsync();
            return plazos.Select(p => new PlazoDto { Id = p.Id, Nombre = p.Nombre, FechaInicio = p.FechaInicio, FechaFin = p.FechaFin });
        }

        public async Task<IEnumerable<RegionDto>> GetRegionesAsync()
        {
            var regiones = await _unitOfWork.Regiones.GetAllAsync();
            return regiones.Select(r => new RegionDto { Id = r.Id, Nombre = r.Nombre });
        }

        public async Task<IEnumerable<ReglamentoDto>> GetReglamentosAsync()
        {
            var reglamentos = await _unitOfWork.Reglamentos.GetAllAsync();
            return reglamentos.Select(r => new ReglamentoDto { Id = r.Id, Nombre = r.Nombre, Tipo = r.Tipo });
        }

        public async Task<IEnumerable<SancionDto>> GetSancionesAsync()
        {
            var sanciones = await _unitOfWork.Sanciones.GetAllAsync();
            return sanciones.Select(s => new SancionDto { Id = s.Id, Nombre = s.Nombre, Motivo = s.Motivo, Sancion = s.Sancion, Estado = s.Estado });
        }

        public async Task<IEnumerable<SancionDetalleDto>> GetSancionesDetalleAsync()
        {
            var sancionesDetalle = await _unitOfWork.SancionesDetalle.GetAllAsync();
            return sancionesDetalle.Select(s => new SancionDetalleDto { Id = s.Id, Nombre = s.Nombre, Motivo = s.Motivo, Sancion = s.Sancion, Estado = s.Estado, Detalles = s.Detalles, FechaResolucion = s.FechaResolucion });
        }

        public async Task<IEnumerable<SectorProductoDto>> GetSectoresProductosAsync()
        {
            var sectoresProductos = await _unitOfWork.SectoresProductos.GetAllAsync();
            return sectoresProductos.Select(s => new SectorProductoDto { Id = s.Id, Nombre = s.Nombre, Descripcion = s.Descripcion });
        }

        public async Task<IEnumerable<SuscripcionDto>> GetSuscripcionesAsync()
        {
            var suscripciones = await _unitOfWork.Suscripciones.GetAllAsync();
            return suscripciones.Select(s => new SuscripcionDto { Id = s.Id, Nombre = s.Nombre, Email = s.Email, FechaInicio = s.FechaInicio, Activa = s.Activa });
        }

        public async Task<IEnumerable<TerceroDto>> GetTercerosAsync()
        {
            var terceros = await _unitOfWork.Terceros.GetAllAsync();
            return terceros.Select(t => new TerceroDto { Id = t.Id, Nombre = t.Nombre, Nif = t.Nif, Tipo = t.Tipo });
        }

        public async Task SyncAyudasAsync()
        {
            var apiData = await _apiClient.GetAyudasAsync();
            var dbData = await _unitOfWork.Ayudas.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Ayuda { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion });
            await _unitOfWork.Ayudas.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncAyudasEstadosAsync()
        {
            var apiData = await _apiClient.GetAyudasEstadosAsync();
            var dbData = await _unitOfWork.AyudasEstados.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new AyudaEstado { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion, Estado = dto.Estado });
            await _unitOfWork.AyudasEstados.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }
    }
}
