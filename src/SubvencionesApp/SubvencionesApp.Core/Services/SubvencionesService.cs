using SubvencionesApp.Core.Dtos;
using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Api;
using SubvencionesApp.Infrastructure.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Core.Services
{
    public class SubvencionesService : ISubvencionesService
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
    }
}