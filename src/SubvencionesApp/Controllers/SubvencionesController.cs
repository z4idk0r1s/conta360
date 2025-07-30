using Microsoft.AspNetCore.Mvc;
using SubvencionesApp.Core.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class SubvencionesController : ControllerBase
{
    private readonly ISubvencionesService _subvencionesService;

    public SubvencionesController(ISubvencionesService subvencionesService)
    {
        _subvencionesService = subvencionesService;
    }

    [HttpPost("sincronizar")]
    public async Task<IActionResult> SincronizarDatos()
    {
        try
        {
            await _subvencionesService.SincronizarDatosAsync();
            return Ok("Sincronización de subvenciones iniciada correctamente.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al sincronizar los datos: {ex.Message}");
        }
    }
}