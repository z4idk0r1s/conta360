using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Conta360.Infrastructure.Excel.Configuration;
using Conta360.Infrastructure.Excel.Models.ResumenFiscal;
using Conta360.Infrastructure.Excel.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conta360.Infrastructure.Excel.Services.Implementation
{
    public class ExcelFiscalProcessor : IExcelFiscalProcessor
    {
        private readonly ExcelSettings _settings;
        private readonly ILogger<ExcelFiscalProcessor> _logger;

        // Constantes de columnas excell
        private const int ColBaseImponible4 = 18;  // Columna R
        private const int ColCuotaIva4 = 20;      // Columna T
        private const int ColBaseImponible10 = 24; // Columna X
        private const int ColCuotaIva10 = 26;     // Columna Z
        private const int ColBaseImponible21 = 29; // Columna AC
        private const int ColCuotaIva21 = 32;     // Columna AF
        
        // Columna para el texto "Total (Fecha)" en los detalles diarios
        private const int ColNombreTotalDiario = 5; // Columna E

        public ExcelFiscalProcessor(
            IOptions<ExcelSettings> settings,
            ILogger<ExcelFiscalProcessor> logger)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResumenFiscalResponse> ProcesarResumenFiscalAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await Task.Run(() =>
                {
                    using var workbook = new XLWorkbook(_settings.RutaExcel);
                    var worksheet = workbook.Worksheet(_settings.HojaResumen)
                                             ?? throw new InvalidOperationException($"No se encontró la hoja: {_settings.HojaResumen}");

                    // =========================================================
                    // PRIMERA LÓGICA: Extracción de datos de cabecera (Filas 9-16)
                    // =========================================================
                    var (empresa, nombreComercial, fechaDesde, fechaHasta, totalBaseMes, totalImpuestosMes, totalMes) = ExtraerDatosCabecera(worksheet);

                    // =========================================================
                    // SEGUNDA LÓGICA: Extracción de datos diarios (A partir de la fila ~23)
                    // =========================================================
                    var detallesDiarios = new List<DetalleDiario>();
                    
                    int currentRow = _settings.FilaInicioResumen; 
                    int ultimaFila = worksheet.LastRowUsed()?.RowNumber() ?? 10000;

                    while (currentRow <= ultimaFila && !cancellationToken.IsCancellationRequested)
                    {
                        var celdaTextoDiario = worksheet.Cell(currentRow, ColNombreTotalDiario).GetString().Trim();

                        if (EsLineaTotalDiario(celdaTextoDiario))
                        {
                            var detalle = ExtraerDetalleDiario(worksheet.Row(currentRow));
                            if (detalle != null)
                                detallesDiarios.Add(detalle);
                        }
                        currentRow++;
                    }

                    if (!detallesDiarios.Any())
                    {
                         _logger.LogWarning("No se obtuvieron datos diarios del resumen fiscal de Excel en '{Ruta}'.", _settings.RutaExcel);
                         throw new InvalidOperationException($"No hay datos diarios para procesar desde el resumen fiscal del archivo '{_settings.RutaExcel}'.");
                    }

                    // =========================================================
                    // CALCULO DE TOTALES GENERALES A PARTIR DE LOS DETALLES DIARIOS
                    // =========================================================
                    // Ahora ExtraerTotalesGenerales() recibirá la lista de detalles diarios
                    // para calcular los totales.
                    var totalesGenerales = ExtraerTotalesGenerales(detallesDiarios); 


                    // =========================================================
                    // CONSTRUCCIÓN DE LA RESPUESTA FINAL
                    // =========================================================
                    return new ResumenFiscalResponse
                    {
                        FechaInforme = DateTime.UtcNow,
                        FechaDesde = fechaDesde,
                        FechaHasta = fechaHasta,
                        Usuario = Environment.UserName,
                        Empresa = empresa,
                        NombreComercial = nombreComercial,
                        Totales = totalesGenerales, // Esto ahora contendrá los totales calculados
                        DetallesPorDia = detallesDiarios.OrderBy(d => d.Fecha).ToList()
                    };

                }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando archivo Excel: {Ruta}", _settings.RutaExcel);
                throw new ApplicationException("Error crítico al procesar el resumen fiscal. Detalles en el log.", ex);
            }
        }

        private static (string empresa, string nombreComercial, DateTime fechaDesde, DateTime fechaHasta, decimal totalBaseMes, decimal totalImpuestosMes, decimal totalMes) ExtraerDatosCabecera(IXLWorksheet worksheet)
        {
            try
            {
                var empresa = worksheet.Cell("C12").GetString().Trim();
                var nombreComercial = worksheet.Cell("C14").GetString().Trim();
                var fechaDesde = LeerFechaSegura(worksheet.Cell("V12"));
                var fechaHasta = LeerFechaSegura(worksheet.Cell("V14"));
                
                var totalBaseMes = ParseDecimalSeguro(worksheet.Cell("AE11"));
                var totalImpuestosMes = ParseDecimalSeguro(worksheet.Cell("AE13"));
                var totalMes = ParseDecimalSeguro(worksheet.Cell("AE15"));

                if (string.IsNullOrWhiteSpace(empresa) || string.IsNullOrWhiteSpace(nombreComercial))
                    throw new InvalidOperationException("Los datos de la cabecear son erróneos.");

                return (empresa, nombreComercial, fechaDesde, fechaHasta, totalBaseMes, totalImpuestosMes, totalMes);
            }
            catch (Exception ex)
            {
                throw new FormatException("Error leyendo cabecera del Excel.", ex);
            }
        }

        private static DateTime LeerFechaSegura(IXLCell cell)
        {
            if (cell == null || cell.IsEmpty())
                throw new InvalidOperationException($"La celda de fecha '{cell?.Address}' está vacía o no existe.");

            try
            {
                if (cell.DataType == XLDataType.DateTime)
                    return cell.GetDateTime();

                if (cell.DataType == XLDataType.Number)
                    return DateTime.FromOADate(cell.GetDouble());

                var valorTexto = cell.GetString().Trim();
                var formatos = new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "dd-MM-yyyy", "M/d/yyyy" };

                if (DateTime.TryParseExact(valorTexto, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha))
                    return fecha;

                if (DateTime.TryParse(valorTexto, out fecha))
                    return fecha;

                throw new FormatException($"Formato de fecha no reconocido: '{valorTexto}' en celda {cell.Address}");
            }
            catch (Exception ex)
            {
                throw new FormatException($"Error interpretando la fecha en la celda: {cell.Address}. Valor: '{cell?.GetString()}'.", ex);
            }
        }

        private static bool EsLineaTotalDiario(string textoCelda)
        {
            if (string.IsNullOrWhiteSpace(textoCelda))
                return false;

            var pattern = @"^Total\s*\(\s*\d{1,2}[/\-]\d{1,2}[/\-]\d{2,4}\s*\)";
            return Regex.IsMatch(textoCelda, pattern, RegexOptions.IgnoreCase);
        }

        private static DetalleDiario? ExtraerDetalleDiario(IXLRow row)
        {
            try
            {
                var texto = row.Cell(ColNombreTotalDiario).GetString();
                var match = Regex.Match(texto, @"\(([^)]+)\)");

                if (!match.Success)
                    return null;

                var fechaStr = match.Groups[1].Value.Trim();
                var formatos = new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "dd-MM-yyyy", "M/d/yyyy" };

                if (!DateTime.TryParseExact(fechaStr, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha) &&
                    !DateTime.TryParse(fechaStr, out fecha))
                {
                    return null;
                }

                return new DetalleDiario
                {
                    Fecha = fecha,
                    BaseImponible4 = ParseDecimalSeguro(row.Cell(ColBaseImponible4)),
                    CuotaIva4 = ParseDecimalSeguro(row.Cell(ColCuotaIva4)),
                    BaseImponible10 = ParseDecimalSeguro(row.Cell(ColBaseImponible10)),
                    CuotaIva10 = ParseDecimalSeguro(row.Cell(ColCuotaIva10)),
                    BaseImponible21 = ParseDecimalSeguro(row.Cell(ColBaseImponible21)),
                    CuotaIva21 = ParseDecimalSeguro(row.Cell(ColCuotaIva21))
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al extraer detalle diario en fila {row.RowNumber()}: {ex.Message}");
                return null;
            }
        }

        // Método modificado para CALCULAR los totales generales a partir de la lista de detalles diarios.
        // Ya no lee de una fila específica del Excel.
        private static TotalesGenerales ExtraerTotalesGenerales(List<DetalleDiario> detallesDiarios)
        {
            // Suma todas las propiedades de BaseImponible y CuotaIva de cada detalle diario
            return new TotalesGenerales
            {
                BaseImponible4 = detallesDiarios.Sum(d => d.BaseImponible4),
                CuotaIva4 = detallesDiarios.Sum(d => d.CuotaIva4),
                BaseImponible10 = detallesDiarios.Sum(d => d.BaseImponible10),
                CuotaIva10 = detallesDiarios.Sum(d => d.CuotaIva10),
                BaseImponible21 = detallesDiarios.Sum(d => d.BaseImponible21),
                CuotaIva21 = detallesDiarios.Sum(d => d.CuotaIva21)
                // Las propiedades TotalIvaX, BaseImponibleTotal, CuotaIvaTotal, ImporteTotal
                // se calculan automáticamente en el record TotalesGenerales si están definidas allí.
            };
        }

        private static decimal ParseDecimalSeguro(IXLCell cell)
        {
            try
            {
                if (cell == null || cell.IsEmpty())
                    return 0m;

                if (cell.DataType == XLDataType.Number)
                    return Convert.ToDecimal(cell.GetDouble());

                var valor = cell.GetString()
                    .Replace("€", "")
                    .Replace(".", "") // Elimina el separador de miles
                    .Replace(",", ".") // Reemplaza la coma decimal por punto
                    .Trim();

                return decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out var resultado)
                    ? resultado
                    : 0m;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al parsear decimal de la celda {cell?.Address} con valor '{cell?.GetString()}': {ex.Message}");
                return 0m;
            }
        }
    }
}