using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SubvencionesApp.Infrastructure.Interfaces;
using SubvencionesApp.Api.Clients;

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
            await SyncAccionesAsync();
            await SyncAgrupacionesAsync();
            await SyncAreasAsync();
            await SyncBeneficiariosAsync();
            await SyncConcesionesAsync();
            await SyncConvocatoriasAsync();
            await SyncEntidadesAsync();
            await SyncEstadosAsync();
            await SyncFormasPagoAsync();
            await SyncLineasAsync();
            await SyncMunicipiosAsync();
            await SyncOrganismosAsync();
            await SyncProgramasAsync();
            await SyncProvinciasAsync();
            await SyncSectoresAsync();
            await SyncSituacionesEntornoAsync();
            await SyncSubtiposSubvencionAsync();
            await SyncTiposBeneficiarioAsync();
            await SyncTiposConvocatoriaAsync();
            await SyncTiposOrganismoAsync();
            await SyncTiposSubvencionAsync();
            await SyncTramosAsync();
            await SyncUnidadesAdministrativasAsync();
            await SyncAyudasAsync();
            await SyncAyudasEstadosAsync();
            await SyncConcesionesDetalleAsync();
            await SyncConvocatoriasDetalleAsync();
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

        public async Task SyncAccionesAsync()
        {
            var apiData = await _apiClient.GetAccionesAsync();
            var dbData = await _unitOfWork.Acciones.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Accion { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.Acciones.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncAgrupacionesAsync()
        {
            var apiData = await _apiClient.GetAgrupacionesAsync();
            var dbData = await _unitOfWork.Agrupaciones.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Agrupacion { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.Agrupaciones.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncAreasAsync()
        {
            var apiData = await _apiClient.GetAreasAsync();
            var dbData = await _unitOfWork.Areas.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Area { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.Areas.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncBeneficiariosAsync()
        {
            var apiData = await _apiClient.GetBeneficiariosAsync();
            var dbData = await _unitOfWork.Beneficiarios.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Beneficiario { Id = dto.Id, Nombre = dto.Nombre, Identificacion = dto.Identificacion });
            await _unitOfWork.Beneficiarios.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncEntidadesAsync()
        {
            var apiData = await _apiClient.GetEntidadesAsync();
            var dbData = await _unitOfWork.Entidades.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Entidad { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.Entidades.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncEstadosAsync()
        {
            var apiData = await _apiClient.GetEstadosAsync();
            var dbData = await _unitOfWork.Estados.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Estado { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.Estados.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncFormasPagoAsync()
        {
            var apiData = await _apiClient.GetFormasPagoAsync();
            var dbData = await _unitOfWork.FormasPago.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new FormaPago { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.FormasPago.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncLineasAsync()
        {
            var apiData = await _apiClient.GetLineasAsync();
            var dbData = await _unitOfWork.Lineas.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Linea { Id = dto.Id, Codigo = dto.Codigo, Nombre = dto.Nombre });
            await _unitOfWork.Lineas.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncMunicipiosAsync()
        {
            var apiData = await _apiClient.GetMunicipiosAsync();
            var dbData = await _unitOfWork.Municipios.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Municipio { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.Municipios.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncOrganismosAsync()
        {
            var apiData = await _apiClient.GetOrganismosAsync();
            var dbData = await _unitOfWork.Organismos.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Organismo { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.Organismos.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncProgramasAsync()
        {
            var apiData = await _apiClient.GetProgramasAsync();
            var dbData = await _unitOfWork.Programas.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Programa { Id = dto.Id, Codigo = dto.Codigo, Descripcion = dto.Descripcion });
            await _unitOfWork.Programas.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncProvinciasAsync()
        {
            var apiData = await _apiClient.GetProvinciasAsync();
            var dbData = await _unitOfWork.Provincias.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Provincia { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.Provincias.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSectoresAsync()
        {
            var apiData = await _apiClient.GetSectoresAsync();
            var dbData = await _unitOfWork.Sectores.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Sector { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.Sectores.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSituacionesEntornoAsync()
        {
            var apiData = await _apiClient.GetSituacionesEntornoAsync();
            var dbData = await _unitOfWork.SituacionesEntorno.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new SituacionEntorno { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.SituacionesEntorno.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSubtiposSubvencionAsync()
        {
            var apiData = await _apiClient.GetSubtiposSubvencionAsync();
            var dbData = await _unitOfWork.SubtiposSubvencion.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new SubtipoSubvencion { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.SubtiposSubvencion.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncTiposBeneficiarioAsync()
        {
            var apiData = await _apiClient.GetTiposBeneficiarioAsync();
            var dbData = await _unitOfWork.TiposBeneficiario.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new TipoBeneficiario { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.TiposBeneficiario.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncTiposConvocatoriaAsync()
        {
            var apiData = await _apiClient.GetTiposConvocatoriaAsync();
            var dbData = await _unitOfWork.TiposConvocatoria.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new TipoConvocatoria { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.TiposConvocatoria.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncTiposOrganismoAsync()
        {
            var apiData = await _apiClient.GetTiposOrganismoAsync();
            var dbData = await _unitOfWork.TiposOrganismo.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new TipoOrganismo { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.TiposOrganismo.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncTiposSubvencionAsync()
        {
            var apiData = await _apiClient.GetTiposSubvencionAsync();
            var dbData = await _unitOfWork.TiposSubvencion.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new TipoSubvencion { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.TiposSubvencion.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncTramosAsync()
        {
            var apiData = await _apiClient.GetTramosAsync();
            var dbData = await _unitOfWork.Tramos.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Tramo { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.Tramos.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncUnidadesAdministrativasAsync()
        {
            var apiData = await _apiClient.GetUnidadesAdministrativasAsync();
            var dbData = await _unitOfWork.UnidadesAdministrativas.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new UnidadAdministrativa { Id = dto.Id, Descripcion = dto.Descripcion });
            await _unitOfWork.UnidadesAdministrativas.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
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

        // Nuevos métodos de sincronización para las entidades que faltaban en el original
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

        public async Task SyncConcesionesDetalleAsync()
        {
            var apiData = await _apiClient.GetConcesionesDetalleAsync();
            var dbData = await _unitOfWork.ConcesionesDetalle.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new ConcesionDetalle { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion, Detalles = dto.Detalles, Importe = dto.Importe, FechaResolucion = dto.FechaResolucion });
            await _unitOfWork.ConcesionesDetalle.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncConvocatoriasDetalleAsync()
        {
            var apiData = await _apiClient.GetConvocatoriasDetalleAsync();
            var dbData = await _unitOfWork.ConvocatoriasDetalle.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new ConvocatoriaDetalle { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion, Detalles = dto.Detalles, Estado = dto.Estado, FechaInicio = dto.FechaInicio, FechaFin = dto.FechaFin, FechaPublicacion = dto.FechaPublicacion });
            await _unitOfWork.ConvocatoriasDetalle.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncFinalidadesAsync()
        {
            var apiData = await _apiClient.GetFinalidadesAsync();
            var dbData = await _unitOfWork.Finalidades.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Finalidad { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion });
            await _unitOfWork.Finalidades.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncGrandesBeneficiariosAsync()
        {
            var apiData = await _apiClient.GetGrandesBeneficiariosAsync();
            var dbData = await _unitOfWork.GrandesBeneficiarios.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new GrandeBeneficiario { Id = dto.Id, Nombre = dto.Nombre, Tipo = dto.Tipo, Importe = dto.Importe });
            await _unitOfWork.GrandesBeneficiarios.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncInstrumentosAsync()
        {
            var apiData = await _apiClient.GetInstrumentosAsync();
            var dbData = await _unitOfWork.Instrumentos.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Instrumento { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion });
            await _unitOfWork.Instrumentos.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncMinimisAsync()
        {
            var apiData = await _apiClient.GetMinimisAsync();
            var dbData = await _unitOfWork.Minimis.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Minimis { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion, Estado = dto.Estado, FechaInicio = dto.FechaInicio, FechaFin = dto.FechaFin });
            await _unitOfWork.Minimis.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncObjetivosAsync()
        {
            var apiData = await _apiClient.GetObjetivosAsync();
            var dbData = await _unitOfWork.Objetivos.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Objetivo { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion });
            await _unitOfWork.Objetivos.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncOrganosCodigoAdminAsync()
        {
            var apiData = await _apiClient.GetOrganosCodigoAdminAsync();
            var dbData = await _unitOfWork.OrganosCodigoAdmin.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new OrganosCodigoAdmin { Id = dto.Id, CodigoAdmin = dto.CodigoAdmin, Nombre = dto.Nombre });
            await _unitOfWork.OrganosCodigoAdmin.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncPartidosPoliticosAsync()
        {
            var apiData = await _apiClient.GetPartidosPoliticosAsync();
            var dbData = await _unitOfWork.PartidosPoliticos.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new PartidoPolitico { Id = dto.Id, Nombre = dto.Nombre, Importe = dto.Importe, Fecha = dto.Fecha });
            await _unitOfWork.PartidosPoliticos.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncPlanesEstrategicosAsync()
        {
            var apiData = await _apiClient.GetPlanesEstrategicosAsync();
            var dbData = await _unitOfWork.PlanesEstrategicos.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new PlanEstrategico { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion, Estado = dto.Estado, FechaAprobacion = dto.FechaAprobacion });
            await _unitOfWork.PlanesEstrategicos.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncPlanesEstrategicosDetalleAsync()
        {
            var apiData = await _apiClient.GetPlanesEstrategicosDetalleAsync();
            var dbData = await _unitOfWork.PlanesEstrategicosDetalle.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new PlanEstrategicoDetalle { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion, Estado = dto.Estado, FechaAprobacion = dto.FechaAprobacion });
            await _unitOfWork.PlanesEstrategicosDetalle.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncPlazosAsync()
        {
            var apiData = await _apiClient.GetPlazosAsync();
            var dbData = await _unitOfWork.Plazos.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Plazo { Id = dto.Id, Nombre = dto.Nombre, FechaInicio = dto.FechaInicio, FechaFin = dto.FechaFin });
            await _unitOfWork.Plazos.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncRegionesAsync()
        {
            var apiData = await _apiClient.GetRegionesAsync();
            var dbData = await _unitOfWork.Regiones.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Region { Id = dto.Id, Nombre = dto.Nombre });
            await _unitOfWork.Regiones.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncReglamentosAsync()
        {
            var apiData = await _apiClient.GetReglamentosAsync();
            var dbData = await _unitOfWork.Reglamentos.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Reglamento { Id = dto.Id, Nombre = dto.Nombre, Tipo = dto.Tipo });
            await _unitOfWork.Reglamentos.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSancionesAsync()
        {
            var apiData = await _apiClient.GetSancionesAsync();
            var dbData = await _unitOfWork.Sanciones.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Sancion { Id = dto.Id, Nombre = dto.Nombre, Motivo = dto.Motivo, Sancion = dto.Sancion, Estado = dto.Estado });
            await _unitOfWork.Sanciones.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSancionesDetalleAsync()
        {
            var apiData = await _apiClient.GetSancionesDetalleAsync();
            var dbData = await _unitOfWork.SancionesDetalle.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new SancionDetalle { Id = dto.Id, Nombre = dto.Nombre, Motivo = dto.Motivo, Sancion = dto.Sancion, Estado = dto.Estado, Detalles = dto.Detalles, FechaResolucion = dto.FechaResolucion });
            await _unitOfWork.SancionesDetalle.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSectoresProductosAsync()
        {
            var apiData = await _apiClient.GetSectoresProductosAsync();
            var dbData = await _unitOfWork.SectoresProductos.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new SectorProducto { Id = dto.Id, Nombre = dto.Nombre, Descripcion = dto.Descripcion });
            await _unitOfWork.SectoresProductos.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncSuscripcionesAsync()
        {
            var apiData = await _apiClient.GetSuscripcionesAsync();
            var dbData = await _unitOfWork.Suscripciones.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Suscripcion { Id = dto.Id, Nombre = dto.Nombre, Email = dto.Email, FechaInicio = dto.FechaInicio, Activa = dto.Activa });
            await _unitOfWork.Suscripciones.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }

        public async Task SyncTercerosAsync()
        {
            var apiData = await _apiClient.GetTercerosAsync();
            var dbData = await _unitOfWork.Terceros.GetAllAsync();
            var dbIds = dbData.Select(e => e.Id).ToHashSet();
            var nuevasEntidades = apiData.Where(dto => !dbIds.Contains(dto.Id)).Select(dto => new Tercero { Id = dto.Id, Nombre = dto.Nombre, Nif = dto.Nif, Tipo = dto.Tipo });
            await _unitOfWork.Terceros.AddRangeAsync(nuevasEntidades);
            await _unitOfWork.CommitAsync();
        }
    }
}