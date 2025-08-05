using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.Interfaces;
using SubvencionesApp.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.Services
{
    public class SubvencionQueryService : ISubvencionQueryService
    {
        private readonly AccionService _accionService;
        private readonly AgrupacionService _agrupacionService;
        private readonly AreaService _areaService;
        private readonly BeneficiarioService _beneficiarioService;
        private readonly ConcesionService _concesionService;
        private readonly ConvocatoriaService _convocatoriaService;
        private readonly DatosEstadisticosService _datosEstadisticosService;
        private readonly EntidadService _entidadService;
        private readonly EstadoService _estadoService;
        private readonly FormaPagoService _formaPagoService;
        private readonly LineaService _lineaService;
        private readonly MunicipioService _municipioService;
        private readonly OrganismoService _organismoService;
        private readonly ProgramaService _programaService;
        private readonly ProvinciaService _provinciaService;
        private readonly SectorService _sectorService;
        private readonly SituacionEntornoService _situacionEntornoService;
        private readonly SubtipoSubvencionService _subtipoSubvencionService;
        private readonly TipoConvocatoriaService _tipoConvocatoriaService;
        private readonly TipoOrganismoService _tipoOrganismoService;
        private readonly TipoSubvencionService _tipoSubvencionService;
        private readonly TramoService _tramoService;
        private readonly UnidadAdministrativaService _unidadAdministrativaService;

        // Nuevos servicios
        private readonly AyudaService _ayudaService;
        private readonly AyudaEstadoService _ayudaEstadoService;
        private readonly ConcesionDetalleService _concesionDetalleService;
        private readonly ConvocatoriaDetalleService _convocatoriaDetalleService;
        private readonly FinalidadService _finalidadService;
        private readonly GrandeBeneficiarioService _grandeBeneficiarioService;
        private readonly InstrumentoService _instrumentoService;
        private readonly MinimisService _minimisService;
        private readonly ObjetivoService _objetivoService;
        private readonly OrganosCodigoAdminService _organosCodigoAdminService;
        private readonly PartidoPoliticoService _partidoPoliticoService;
        private readonly PlanEstrategicoService _planEstrategicoService;
        private readonly PlanEstrategicoDetalleService _planEstrategicoDetalleService;
        private readonly PlazoService _plazoService;
        private readonly RegionService _regionService;
        private readonly ReglamentoService _reglamentoService;
        private readonly SancionService _sancionService;
        private readonly SancionDetalleService _sancionDetalleService;
        private readonly SectorProductoService _sectorProductoService;
        private readonly SuscripcionService _suscripcionService;
        private readonly TerceroService _terceroService;
        private readonly TipoBeneficiarioService _tipoBeneficiarioService;


        public SubvencionQueryService(
            AccionService accionService,
            AgrupacionService agrupacionService,
            AreaService areaService,
            BeneficiarioService beneficiarioService,
            ConcesionService concesionService,
            ConvocatoriaService convocatoriaService,
            DatosEstadisticosService datosEstadisticosService,
            EntidadService entidadService,
            EstadoService estadoService,
            FormaPagoService formaPagoService,
            LineaService lineaService,
            MunicipioService municipioService,
            OrganismoService organismoService,
            ProgramaService programaService,
            ProvinciaService provinciaService,
            SectorService sectorService,
            SituacionEntornoService situacionEntornoService,
            SubtipoSubvencionService subtipoSubvencionService,
            TipoConvocatoriaService tipoConvocatoriaService,
            TipoOrganismoService tipoOrganismoService,
            TipoSubvencionService tipoSubvencionService,
            TramoService tramoService,
            UnidadAdministrativaService unidadAdministrativaService,
            // Nuevas inyecciones
            AyudaService ayudaService,
            AyudaEstadoService ayudaEstadoService,
            ConcesionDetalleService concesionDetalleService,
            ConvocatoriaDetalleService convocatoriaDetalleService,
            FinalidadService finalidadService,
            GrandeBeneficiarioService grandeBeneficiarioService,
            InstrumentoService instrumentoService,
            MinimisService minimisService,
            ObjetivoService objetivoService,
            OrganosCodigoAdminService organosCodigoAdminService,
            PartidoPoliticoService partidoPoliticoService,
            PlanEstrategicoService planEstrategicoService,
            PlanEstrategicoDetalleService planEstrategicoDetalleService,
            PlazoService plazoService,
            RegionService regionService,
            ReglamentoService reglamentoService,
            SancionService sancionService,
            SancionDetalleService sancionDetalleService,
            SectorProductoService sectorProductoService,
            SuscripcionService suscripcionService,
            TerceroService terceroService,
            TipoBeneficiarioService tipoBeneficiarioService)
        {
            _accionService = accionService;
            _agrupacionService = agrupacionService;
            _areaService = areaService;
            _beneficiarioService = beneficiarioService;
            _concesionService = concesionService;
            _convocatoriaService = convocatoriaService;
            _datosEstadisticosService = datosEstadisticosService;
            _entidadService = entidadService;
            _estadoService = estadoService;
            _formaPagoService = formaPagoService;
            _lineaService = lineaService;
            _municipioService = municipioService;
            _organismoService = organismoService;
            _programaService = programaService;
            _provinciaService = provinciaService;
            _sectorService = sectorService;
            _situacionEntornoService = situacionEntornoService;
            _subtipoSubvencionService = subtipoSubvencionService;
            _tipoConvocatoriaService = tipoConvocatoriaService;
            _tipoOrganismoService = tipoOrganismoService;
            _tipoSubvencionService = tipoSubvencionService;
            _tramoService = tramoService;
            _unidadAdministrativaService = unidadAdministrativaService;

            // Nuevos servicios
            _ayudaService = ayudaService;
            _ayudaEstadoService = ayudaEstadoService;
            _concesionDetalleService = concesionDetalleService;
            _convocatoriaDetalleService = convocatoriaDetalleService;
            _finalidadService = finalidadService;
            _grandeBeneficiarioService = grandeBeneficiarioService;
            _instrumentoService = instrumentoService;
            _minimisService = minimisService;
            _objetivoService = objetivoService;
            _organosCodigoAdminService = organosCodigoAdminService;
            _partidoPoliticoService = partidoPoliticoService;
            _planEstrategicoService = planEstrategicoService;
            _planEstrategicoDetalleService = planEstrategicoDetalleService;
            _plazoService = plazoService;
            _regionService = regionService;
            _reglamentoService = reglamentoService;
            _sancionService = sancionService;
            _sancionDetalleService = sancionDetalleService;
            _sectorProductoService = sectorProductoService;
            _suscripcionService = suscripcionService;
            _terceroService = terceroService;
            _tipoBeneficiarioService = tipoBeneficiarioService;
        }

        public async Task<IEnumerable<ConvocatoriaDto>> GetConvocatoriasAsync()
        {
            return await _convocatoriaService.GetAllAsync();
        }

        public async Task<IEnumerable<ConcesionDto>> GetConcesionesAsync()
        {
            return await _concesionService.GetAllAsync();
        }

        public async Task<IEnumerable<BeneficiarioDto>> GetBeneficiariosAsync()
        {
            return await _beneficiarioService.GetAllAsync();
        }

        public async Task<ConvocatoriaDto?> GetConvocatoriaByIdAsync(long id)
        {
            return await _convocatoriaService.GetByIdAsync(id);
        }

        public async Task<ConcesionDto?> GetConcesionByIdAsync(long id)
        {
            return await _concesionService.GetByIdAsync(id);
        }

        public async Task<BeneficiarioDto?> GetBeneficiarioByIdAsync(long id)
        {
            return await _beneficiarioService.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ConvocatoriaDto>> SearchConvocatoriasByTextAsync(string searchText)
        {
            return await _convocatoriaService.SearchByTextAsync(searchText);
        }

        public async Task<IEnumerable<ConcesionDto>> GetConcesionesByEjercicioAsync(int ejercicio)
        {
            return await _concesionService.GetByEjercicioAsync(ejercicio);
        }

        public async Task<DatosEstadisticosDto> GetEstadisticasGeneralesAsync()
        {
            return await _datosEstadisticosService.GetEstadisticasGeneralesAsync();
        }

        // Implementaciones de los nuevos métodos
        public async Task<IEnumerable<AyudaDto>> GetAyudasAsync() => await _ayudaService.GetAllAsync();
        public async Task<IEnumerable<AyudaEstadoDto>> GetAyudasEstadosAsync() => await _ayudaEstadoService.GetAllAsync();
        public async Task<IEnumerable<ConcesionDetalleDto>> GetConcesionesDetalleAsync() => await _concesionDetalleService.GetAllAsync();
        public async Task<IEnumerable<ConvocatoriaDetalleDto>> GetConvocatoriasDetalleAsync() => await _convocatoriaDetalleService.GetAllAsync();
        public async Task<IEnumerable<FinalidadDto>> GetFinalidadesAsync() => await _finalidadService.GetAllAsync();
        public async Task<IEnumerable<GrandeBeneficiarioDto>> GetGrandesBeneficiariosAsync() => await _grandeBeneficiarioService.GetAllAsync();
        public async Task<IEnumerable<InstrumentoDto>> GetInstrumentosAsync() => await _instrumentoService.GetAllAsync();
        public async Task<IEnumerable<MinimisDto>> GetMinimisAsync() => await _minimisService.GetAllAsync();
        public async Task<IEnumerable<ObjetivoDto>> GetObjetivosAsync() => await _objetivoService.GetAllAsync();
        public async Task<IEnumerable<OrganosCodigoAdminDto>> GetOrganosCodigoAdminAsync() => await _organosCodigoAdminService.GetAllAsync();
        public async Task<IEnumerable<PartidoPoliticoDto>> GetPartidosPoliticosAsync() => await _partidoPoliticoService.GetAllAsync();
        public async Task<IEnumerable<PlanEstrategicoDto>> GetPlanesEstrategicosAsync() => await _planEstrategicoService.GetAllAsync();
        public async Task<IEnumerable<PlanEstrategicoDetalleDto>> GetPlanesEstrategicosDetalleAsync() => await _planEstrategicoDetalleService.GetAllAsync();
        public async Task<IEnumerable<PlazoDto>> GetPlazosAsync() => await _plazoService.GetAllAsync();
        public async Task<IEnumerable<RegionDto>> GetRegionesAsync() => await _regionService.GetAllAsync();
        public async Task<IEnumerable<ReglamentoDto>> GetReglamentosAsync() => await _reglamentoService.GetAllAsync();
        public async Task<IEnumerable<SancionDto>> GetSancionesAsync() => await _sancionService.GetAllAsync();
        public async Task<IEnumerable<SancionDetalleDto>> GetSancionesDetalleAsync() => await _sancionDetalleService.GetAllAsync();
        public async Task<IEnumerable<SectorProductoDto>> GetSectoresProductosAsync() => await _sectorProductoService.GetAllAsync();
        public async Task<IEnumerable<SuscripcionDto>> GetSuscripcionesAsync() => await _suscripcionService.GetAllAsync();
        public async Task<IEnumerable<TerceroDto>> GetTercerosAsync() => await _terceroService.GetAllAsync();
        public async Task<IEnumerable<TipoBeneficiarioDto>> GetTiposBeneficiarioAsync() => await _tipoBeneficiarioService.GetAllAsync();
    }
}