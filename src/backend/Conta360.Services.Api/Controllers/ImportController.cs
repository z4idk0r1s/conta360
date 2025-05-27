using Microsoft.AspNetCore.Mvc;
using Conta360.Application.Interfaces;


namespace Conta360.Services.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly IConta360Service _conta360Service;
        private readonly IExcelProcessor _excelProcessor;

        public ImportController(IConta360Service conta360Service, IExcelProcessor excelProcessor)
        {
            _conta360Service = conta360Service;
            _excelProcessor = excelProcessor;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            using (var stream = file.OpenReadStream())
            {
                try
                {
                    var (emittedInvoices, receivedInvoices) = await _excelProcessor.ProcessExcelFile(stream);
                    var validationResult = await _conta360Service.ValidateInvoices(emittedInvoices, receivedInvoices);

                    return Ok(new
                    {
                        Success = !validationResult.HasErrors,
                        Errors = validationResult.Errors
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Error = "Error processing Excel file", Details = ex.Message });
                }
            }
        }
    }
}
