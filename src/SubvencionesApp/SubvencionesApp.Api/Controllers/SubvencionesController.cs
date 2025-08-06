using Microsoft.AspNetCore.Mvc;
using SubvencionesApp.Application.Interfaces;
using SubvencionesApp.Application.Services;
using SubvencionesApp.Application.UseCases;
using System.Threading.Tasks;

namespace SubvencionesApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubvencionesController : ControllerBase
    {
        private readonly ConvocatoriaService _convocatoriaService;
        private readonly ConcesionService _concesionService;
        private readonly ISubvencionSyncService _syncService;

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

        public SubvencionesController(
            ConvocatoriaService convocatoriaService,
            ConcesionService concesionService,
            ISubvencionSyncService syncService,
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
            _convocatoriaService = convocatoriaService;
            _concesionService = concesionService;
            _syncService = syncService;
            // Asignación de nuevos servicios
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

        [HttpGet("convocatorias")]
        public async Task<IActionResult> GetConvocatorias()
        {
            var convocatorias = await _convocatoriaService.GetAllAsync();
            return Ok(convocatorias);
        }

        [HttpPost("sync/convocatorias")]
        public async Task<IActionResult> SyncConvocatorias()
        {
            await _syncService.SyncConvocatoriasAsync();
            return Ok("Sincronización de Convocatorias completada");
        }

        [HttpPost("sync/all")]
        public async Task<IActionResult> SyncAll()
        {
            await _syncService.SyncAllDataAsync();
            return Ok("Sincronización completa realizada");
        }

        // Nuevos endpoints GET para las nuevas entidades
        [HttpGet("ayudas")]
        public async Task<IActionResult> GetAyudas() => Ok(await _ayudaService.GetAllAsync());
        
        [HttpGet("ayudas-estados")]
        public async Task<IActionResult> GetAyudasEstados() => Ok(await _ayudaEstadoService.GetAllAsync());

        [HttpGet("concesiones-detalle")]
        public async Task<IActionResult> GetConcesionesDetalle() => Ok(await _concesionDetalleService.GetAllAsync());

        [HttpGet("convocatorias-detalle")]
        public async Task<IActionResult> GetConvocatoriasDetalle() => Ok(await _convocatoriaDetalleService.GetAllAsync());
        
        [HttpGet("finalidades")]
        public async Task<IActionResult> GetFinalidades() => Ok(await _finalidadService.GetAllAsync());

        [HttpGet("grandes-beneficiarios")]
        public async Task<IActionResult> GetGrandesBeneficiarios() => Ok(await _grandeBeneficiarioService.GetAllAsync());

        [HttpGet("instrumentos")]
        public async Task<IActionResult> GetInstrumentos() => Ok(await _instrumentoService.GetAllAsync());

        [HttpGet("minimis")]
        public async Task<IActionResult> GetMinimis() => Ok(await _minimisService.GetAllAsync());

        [HttpGet("objetivos")]
        public async Task<IActionResult> GetObjetivos() => Ok(await _objetivoService.GetAllAsync());

        [HttpGet("organos-codigo-admin")]
        public async Task<IActionResult> GetOrganosCodigoAdmin() => Ok(await _organosCodigoAdminService.GetAllAsync());

        [HttpGet("partidos-politicos")]
        public async Task<IActionResult> GetPartidosPoliticos() => Ok(await _partidoPoliticoService.GetAllAsync());

        [HttpGet("planes-estrategicos")]
        public async Task<IActionResult> GetPlanesEstrategicos() => Ok(await _planEstrategicoService.GetAllAsync());

        [HttpGet("planes-estrategicos-detalle")]
        public async Task<IActionResult> GetPlanesEstrategicosDetalle() => Ok(await _planEstrategicoDetalleService.GetAllAsync());

        [HttpGet("plazos")]
        public async Task<IActionResult> GetPlazos() => Ok(await _plazoService.GetAllAsync());

        [HttpGet("regiones")]
        public async Task<IActionResult> GetRegiones() => Ok(await _regionService.GetAllAsync());

        [HttpGet("reglamentos")]
        public async Task<IActionResult> GetReglamentos() => Ok(await _reglamentoService.GetAllAsync());

        [HttpGet("sanciones")]
        public async Task<IActionResult> GetSanciones() => Ok(await _sancionService.GetAllAsync());

        [HttpGet("sanciones-detalle")]
        public async Task<IActionResult> GetSancionesDetalle() => Ok(await _sancionDetalleService.GetAllAsync());

        [HttpGet("sectores-productos")]
        public async Task<IActionResult> GetSectoresProductos() => Ok(await _sectorProductoService.GetAllAsync());

        [HttpGet("suscripciones")]
        public async Task<IActionResult> GetSuscripciones() => Ok(await _suscripcionService.GetAllAsync());

        [HttpGet("terceros")]
        public async Task<IActionResult> GetTerceros() => Ok(await _terceroService.GetAllAsync());

        [HttpGet("tipos-beneficiario")]
        public async Task<IActionResult> GetTiposBeneficiario() => Ok(await _tipoBeneficiarioService.GetAllAsync());


        // Nuevos endpoints POST para sincronización
        [HttpPost("sync/ayudas")]
        public async Task<IActionResult> SyncAyudas()
        {
            await _syncService.SyncAyudasAsync();
            return Ok("Sincronización de Ayudas completada");
        }

        [HttpPost("sync/ayudas-estados")]
        public async Task<IActionResult> SyncAyudasEstados()
        {
            await _syncService.SyncAyudasEstadosAsync();
            return Ok("Sincronización de AyudasEstados completada");
        }
        
        [HttpPost("sync/concesiones-detalle")]
        public async Task<IActionResult> SyncConcesionDetalle()
        {
            await _syncService.SyncConcesionDetalleAsync();
            return Ok("Sincronización de ConcesionesDetalle completada");
        }

        [HttpPost("sync/convocatorias-detalle")]
        public async Task<IActionResult> SyncConvocatoriaDetalle()
        {
            await _syncService.SyncConvocatoriaDetalleAsync();
            return Ok("Sincronización de ConvocatoriasDetalle completada");
        }

        [HttpPost("sync/finalidades")]
        public async Task<IActionResult> SyncFinalidades()
        {
            await _syncService.SyncFinalidadesAsync();
            return Ok("Sincronización de Finalidades completada");
        }

        [HttpPost("sync/grandes-beneficiarios")]
        public async Task<IActionResult> SyncGrandesBeneficiarios()
        {
            await _syncService.SyncGrandesBeneficiariosAsync();
            return Ok("Sincronización de GrandesBeneficiarios completada");
        }
        
        [HttpPost("sync/instrumentos")]
        public async Task<IActionResult> SyncInstrumentos()
        {
            await _syncService.SyncInstrumentosAsync();
            return Ok("Sincronización de Instrumentos completada");
        }

        [HttpPost("sync/minimis")]
        public async Task<IActionResult> SyncMinimis()
        {
            await _syncService.SyncMinimisAsync();
            return Ok("Sincronización de Minimis completada");
        }

        [HttpPost("sync/objetivos")]
        public async Task<IActionResult> SyncObjetivos()
        {
            await _syncService.SyncObjetivosAsync();
            return Ok("Sincronización de Objetivos completada");
        }
        
        [HttpPost("sync/organos-codigo-admin")]
        public async Task<IActionResult> SyncOrganosCodigoAdmin()
        {
            await _syncService.SyncOrganosCodigoAdminAsync();
            return Ok("Sincronización de OrganosCodigoAdmin completada");
        }

        [HttpPost("sync/partidos-politicos")]
        public async Task<IActionResult> SyncPartidosPoliticos()
        {
            await _syncService.SyncPartidosPoliticosAsync();
            return Ok("Sincronización de PartidosPoliticos completada");
        }
        
        [HttpPost("sync/planes-estrategicos")]
        public async Task<IActionResult> SyncPlanesEstrategicos()
        {
            await _syncService.SyncPlanesEstrategicosAsync();
            return Ok("Sincronización de PlanesEstrategicos completada");
        }

        [HttpPost("sync/planes-estrategicos-detalle")]
        public async Task<IActionResult> SyncPlanesEstrategicosDetalle()
        {
            await _syncService.SyncPlanesEstrategicosDetalleAsync();
            return Ok("Sincronización de PlanesEstrategicosDetalle completada");
        }

        [HttpPost("sync/plazos")]
        public async Task<IActionResult> SyncPlazos()
        {
            await _syncService.SyncPlazosAsync();
            return Ok("Sincronización de Plazos completada");
        }
        
        [HttpPost("sync/regiones")]
        public async Task<IActionResult> SyncRegiones()
        {
            await _syncService.SyncRegionesAsync();
            return Ok("Sincronización de Regiones completada");
        }

        [HttpPost("sync/reglamentos")]
        public async Task<IActionResult> SyncReglamentos()
        {
            await _syncService.SyncReglamentosAsync();
            return Ok("Sincronización de Reglamentos completada");
        }

        [HttpPost("sync/sanciones")]
        public async Task<IActionResult> SyncSanciones()
        {
            await _syncService.SyncSancionesAsync();
            return Ok("Sincronización de Sanciones completada");
        }

        [HttpPost("sync/sanciones-detalle")]
        public async Task<IActionResult> SyncSancionesDetalle()
        {
            await _syncService.SyncSancionesDetalleAsync();
            return Ok("Sincronización de SancionesDetalle completada");
        }

        [HttpPost("sync/sectores-productos")]
        public async Task<IActionResult> SyncSectoresProductos()
        {
            await _syncService.SyncSectoresProductosAsync();
            return Ok("Sincronización de SectoresProductos completada");
        }
        
        [HttpPost("sync/suscripciones")]
        public async Task<IActionResult> SyncSuscripciones()
        {
            await _syncService.SyncSuscripcionesAsync();
            return Ok("Sincronización de Suscripciones completada");
        }
        
        [HttpPost("sync/terceros")]
        public async Task<IActionResult> SyncTerceros()
        {
            await _syncService.SyncTercerosAsync();
            return Ok("Sincronización de Terceros completada");
        }

        [HttpPost("sync/tipos-beneficiario")]
        public async Task<IActionResult> SyncTiposBeneficiario()
        {
            await _syncService.SyncTiposBeneficiarioAsync();
            return Ok("Sincronización de TiposBeneficiario completada");
        }
    }
}