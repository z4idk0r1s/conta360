using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

// Usings de los módulos Excel y A3Cash
using Conta360.Infrastructure.Excel.Configuration;
using Conta360.Infrastructure.Excel.Models.ResumenFiscal; // Para ResumenFiscalResponse
using Conta360.Infrastructure.A3Cash.Interfaces;
using Conta360.Infrastructure.Excel.Services.Interfaces; // Para IA3FileGenerator
using Conta360.Infrastructure.Excel.Services.Implementation;

namespace Conta360.Infrastructure.Reporting.Services
{
    // Opcional: Podrías definir una interfaz IExcelToA3IntegrationService
    // si más adelante planeas tener múltiples implementaciones o mockearlo.
    // public interface IExcelToA3IntegrationService
    // {
    //    Task<string> GenerateA3FileFromExcelAsync(string excelFilePath, string a3OutputFilename = "SUENLACE_FGLD.DAT", CancellationToken cancellationToken = default);
    // }

    /// <summary>
    /// Servicio de orquestación para procesar un archivo Excel y generar un fichero de asientos contables A3.
    /// Este servicio integra las funcionalidades de Conta360.Infrastructure.Excel y Conta360.Infrastructure.A3Cash.
    /// </summary>
    public class ExcelToA3IntegrationService // : IExcelToA3IntegrationService (si defines la interfaz)
    {
        private readonly IExcelFiscalProcessor _excelProcessor;
        private readonly IA3FileGenerator _a3FileGenerator;
        private readonly ILogger<ExcelToA3IntegrationService> _logger;

        public ExcelToA3IntegrationService(
            IExcelFiscalProcessor excelProcessor,
            IA3FileGenerator a3FileGenerator,
            ILogger<ExcelToA3IntegrationService> logger)
        {
            _excelProcessor = excelProcessor ?? throw new ArgumentNullException(nameof(excelProcessor));
            _a3FileGenerator = a3FileGenerator ?? throw new ArgumentNullException(nameof(a3FileGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Procesa el archivo Excel y genera el fichero de asientos contables para A3.
        /// </summary>
        /// <param name="excelFilePath">Ruta completa del archivo Excel a procesar.</param>
        /// <param name="a3OutputFilename">Nombre deseado para el fichero de salida A3. Por defecto: "SUENLACE_FGLD.DAT".</param>
        /// <param name="cancellationToken">Token de cancelación para operaciones asíncronas.</param>
        /// <returns>La ruta completa al fichero A3 generado.</returns>
        /// <exception cref="InvalidOperationException">Si no se pueden obtener datos del Excel o si los datos son insuficientes.</exception>
        /// <exception cref="Exception">Propaga cualquier otra excepción durante el procesamiento o la generación del archivo.</exception>
        public async Task<string> GenerateA3FileFromExcelAsync(
            string excelFilePath,
            string a3OutputFilename = "SUENLACE_FGLD.DAT",
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Iniciando orquestación: Procesando Excel y generando fichero A3. Excel: '{ExcelPath}', A3 Output: '{A3Output}'", excelFilePath, a3OutputFilename);

            try
            {
                // Paso 1: Procesar el archivo Excel para obtener el resumen fiscal
                ResumenFiscalResponse resumenFiscal = await _excelProcessor.ProcesarResumenFiscalAsync(cancellationToken);

                if (resumenFiscal == null || !resumenFiscal.DetallesPorDia.Any())
                {
                    _logger.LogWarning("No se obtuvieron datos diarios del resumen fiscal de Excel en '{ExcelPath}'.", excelFilePath);
                    throw new InvalidOperationException($"No hay datos para procesar desde el resumen fiscal del archivo '{excelFilePath}'.");
                }

                // Paso 2: Transformar los DetallesPorDia en el Dictionary<DateOnly, decimal>
                //         que IA3FileGenerator espera.
                //         Aquí sumamos la Base Imponible y Cuota de IVA de todos los tipos
                //         para obtener un total diario que se registrará como ingreso.
                var dailyTotalsForA3 = resumenFiscal.DetallesPorDia
                    .ToDictionary(
                        d => DateOnly.FromDateTime(d.Fecha), // Clave: Fecha
                        d => d.BaseImponible4 + d.CuotaIva4 +
                             d.BaseImponible10 + d.CuotaIva10 +
                             d.BaseImponible21 + d.CuotaIva21 // Valor: Suma de todos los importes relevantes
                    );

                _logger.LogInformation("Datos diarios transformados. Se generarán {Count} asientos para A3.", dailyTotalsForA3.Count);

                // Paso 3: Generar el fichero A3 usando el módulo A3Cash
                string generatedFilePath = await _a3FileGenerator.ProcessAndGenerateAsync(dailyTotalsForA3, a3OutputFilename);

                _logger.LogInformation("Orquestación completada exitosamente. Fichero A3 generado en: '{FilePath}'", generatedFilePath);
                return generatedFilePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la orquestación de la generación del fichero A3 desde Excel.");
                throw; // Re-lanzar la excepción para que sea manejada por la capa superior
            }
        }
    }
}