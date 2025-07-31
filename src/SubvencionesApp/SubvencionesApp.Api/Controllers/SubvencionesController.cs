using Microsoft.AspNetCore.Mvc;
using SubvencionesApp.Application.Interfaces;
using SubvencionesApp.Application.Services;
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

        public SubvencionesController(
            ConvocatoriaService convocatoriaService,
            ConcesionService concesionService,
            ISubvencionSyncService syncService)
        {
            _convocatoriaService = convocatoriaService;
            _concesionService = concesionService;
            _syncService = syncService;
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
            return Ok("Sincronización completada");
        }

        [HttpPost("sync/all")]
        public async Task<IActionResult> SyncAll()
        {
            await _syncService.SyncAllDataAsync();
            return Ok("Sincronización completa realizada");
        }
    }
}