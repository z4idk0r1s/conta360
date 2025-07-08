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
        private const int ColBaseImponible4 = 8;
        private const int ColCuotaIva4 = 9;
        private const int ColBaseImponible10 = 11;
        private const int ColCuotaIva10 = 12;
        private const int ColBaseImponible21 = 14;
        private const int ColCuotaIva21 = 15;
        private const int ColNombreTotal = 1;

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

                    var (empresa, nombreComercial, fechaDesde, fechaHasta) = ExtraerDatosCabecera(worksheet);

                    var detallesDiarios = new List<DetalleDiario>();
                    int currentRow = _settings.FilaInicioResumen;
                    int ultimaFila = worksheet.LastRowUsed()?.RowNumber() ?? 10000;

                    while (currentRow <= ultimaFila && !cancellationToken.IsCancellationRequested)
                    {
                        var celdaTexto = worksheet.Cell(currentRow, ColNombreTotal).GetString().Trim();

                        // Total general
                        if (string.Equals(celdaTexto, "Total", StringComparison.OrdinalIgnoreCase))
                        {
                            var totales = ExtraerTotalesGenerales(worksheet.Row(currentRow));
                            return new ResumenFiscalResponse
                            {
                                FechaInforme = DateTime.UtcNow,
                                FechaDesde = fechaDesde,
                                FechaHasta = fechaHasta,
                                Usuario = Environment.UserName,
                                Empresa = empresa,
                                NombreComercial = nombreComercial,
                                Totales = totales,
                                DetallesPorDia = detallesDiarios.OrderBy(d => d.Fecha).ToList()
                            };
                        }

                        // Totales diarios
                        if (EsLineaTotalDiario(celdaTexto))
                        {
                            var detalle = ExtraerDetalleDiario(worksheet.Row(currentRow));
                            if (detalle != null)
                                detallesDiarios.Add(detalle);
                        }

                        currentRow++;
                    }

                    throw new InvalidOperationException("No se encontró la fila de totales generales en el Excel.");
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando archivo Excel: {Ruta}", _settings.RutaExcel);
                throw new ApplicationException("Error crítico al procesar el resumen fiscal. Detalles en el log.", ex);
            }
        }

        private static (string empresa, string nombreComercial, DateTime fechaDesde, DateTime fechaHasta) ExtraerDatosCabecera(IXLWorksheet worksheet)
        {
            try
            {
                var empresa = worksheet.Cell("B4").GetString().Trim();
                var nombreComercial = worksheet.Cell("B5").GetString().Trim();
                var fechaDesde = LeerFechaSegura(worksheet.Cell("G4"));
                var fechaHasta = LeerFechaSegura(worksheet.Cell("G5"));

                if (string.IsNullOrWhiteSpace(empresa) || string.IsNullOrWhiteSpace(nombreComercial))
                    throw new InvalidOperationException("Los datos de empresa o nombre comercial están vacíos.");

                return (empresa, nombreComercial, fechaDesde, fechaHasta);
            }
            catch (Exception ex)
            {
                throw new FormatException("Error leyendo cabecera del Excel.", ex);
            }
        }

        private static DateTime LeerFechaSegura(IXLCell cell)
        {
            if (cell == null || cell.IsEmpty())
                throw new InvalidOperationException("La celda de fecha está vacía.");

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

                throw new FormatException($"Formato de fecha no reconocido: '{valorTexto}'");
            }
            catch (Exception ex)
            {
                throw new FormatException($"Error interpretando la fecha en la celda: {cell.Address}", ex);
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
                var texto = row.Cell(ColNombreTotal).GetString();
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
            catch
            {
                return null;
            }
        }

        private static TotalesGenerales ExtraerTotalesGenerales(IXLRow row)
        {
            return new TotalesGenerales
            {
                BaseImponible4 = ParseDecimalSeguro(row.Cell(ColBaseImponible4)),
                CuotaIva4 = ParseDecimalSeguro(row.Cell(ColCuotaIva4)),
                BaseImponible10 = ParseDecimalSeguro(row.Cell(ColBaseImponible10)),
                CuotaIva10 = ParseDecimalSeguro(row.Cell(ColCuotaIva10)),
                BaseImponible21 = ParseDecimalSeguro(row.Cell(ColBaseImponible21)),
                CuotaIva21 = ParseDecimalSeguro(row.Cell(ColCuotaIva21))
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
                    .Replace(".", "")
                    .Replace(",", ".")
                    .Trim();

                return decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out var resultado)
                    ? resultado
                    : 0m;
            }
            catch
            {
                return 0m;
            }
        }
    }
}
