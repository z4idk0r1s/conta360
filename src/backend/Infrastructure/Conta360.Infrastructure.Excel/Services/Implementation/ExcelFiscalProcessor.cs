using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public ExcelFiscalProcessor(
            IOptions<ExcelSettings> settings,
            ILogger<ExcelFiscalProcessor> logger)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<ResumenFiscalResponse> ProcesarResumenFiscalAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var workbook = new XLWorkbook(_settings.RutaExcel);
                var worksheet = workbook.Worksheet(_settings.HojaResumen);

                var (empresa, nombreComercial, fechaDesde, fechaHasta) =
                    ExtraerDatosCabecera(worksheet);

                var detallesDiarios = new List<DetalleDiario>();
                var currentRow = _settings.FilaInicioResumen;

                while (!cancellationToken.IsCancellationRequested)
                {
                    var firstCell = worksheet.Cell(currentRow, 1).GetString().Trim();

                    // Si llegamos a la fila de totales
                    if (string.Equals(firstCell, "Total", StringComparison.OrdinalIgnoreCase))
                    {
                        var totalesGenerales = ExtraerTotalesGenerales(worksheet.Row(currentRow));

                        return Task.FromResult(new ResumenFiscalResponse
                        {
                            FechaInforme = DateTime.UtcNow,
                            FechaDesde = fechaDesde,
                            FechaHasta = fechaHasta,
                            Usuario = Environment.UserName,
                            Empresa = empresa,
                            NombreComercial = nombreComercial,
                            Totales = totalesGenerales,
                            DetallesPorDia = detallesDiarios
                                .OrderBy(d => d.Fecha)
                                .ToList()
                        });
                    }

                    // Si es una línea de total diario
                    if (firstCell.StartsWith("Total", StringComparison.OrdinalIgnoreCase) &&
                        firstCell.Contains("(") &&
                        firstCell.Contains(")"))
                    {
                        var detalleDiario = ExtraerDetalleDiario(worksheet.Row(currentRow));
                        if (detalleDiario != null)
                        {
                            detallesDiarios.Add(detalleDiario);
                        }
                    }

                    currentRow++;
                }

                throw new InvalidOperationException(
                    "No se encontró la fila de totales en el archivo Excel.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error procesando archivo Excel: {Path}",
                    _settings.RutaExcel);
                throw;
            }
        }

        private static (string empresa, string nombreComercial, DateTime fechaDesde, DateTime fechaHasta) ExtraerDatosCabecera(IXLWorksheet worksheet)
        {
            var empresa = worksheet.Cell("B4").GetString().Trim();
            var nombreComercial = worksheet.Cell("B5").GetString().Trim();

            var fechaDesde = LeerFechaRobusta(worksheet.Cell("G4"));
            var fechaHasta = LeerFechaRobusta(worksheet.Cell("G5"));

            return (empresa, nombreComercial, fechaDesde, fechaHasta);
        }

        private static DateTime LeerFechaRobusta(IXLCell cell)
        {
            if (cell == null || cell.IsEmpty())
                throw new InvalidOperationException("La celda de fecha está vacía.");

            if (cell.DataType == XLDataType.DateTime)
                return cell.GetDateTime();

            // Serial de Excel (número)
            if (cell.DataType == XLDataType.Number)
            {
                try
                {
                    return cell.GetDateTime();
                }
                catch
                {
                    // Fallback si no es serial de fecha
                }
            }

            var fechaStr = cell.GetString()?.Trim();
            if (string.IsNullOrWhiteSpace(fechaStr))
                throw new InvalidOperationException("La celda de fecha contiene solo espacios o está vacía.");

            DateTime fecha;
            var formatos = new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "M/d/yyyy", "dd-MM-yyyy" };

            if (DateTime.TryParseExact(fechaStr, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                return fecha;

            if (DateTime.TryParse(fechaStr, out fecha))
                return fecha;

            throw new FormatException($"El valor '{fechaStr}' no es reconocible como fecha.");
        }

        private static DetalleDiario? ExtraerDetalleDiario(IXLRow row)
        {
            var cellValue = row.Cell(1).GetString();
            if (string.IsNullOrWhiteSpace(cellValue) || !cellValue.Contains("(") || !cellValue.Contains(")"))
                return null;

            var fechaStr = cellValue.Split('(', ')')[1].Trim();

            DateTime fecha;
            var formatos = new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "M/d/yyyy", "dd-MM-yyyy" };

            if (!DateTime.TryParseExact(fechaStr, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha) &&
                !DateTime.TryParse(fechaStr, out fecha))
            {
                return null;
            }

            return new DetalleDiario
            {
                Fecha = fecha,
                BaseImponible4 = ParseDecimal(row.Cell(8)),
                CuotaIva4 = ParseDecimal(row.Cell(9)),
                BaseImponible10 = ParseDecimal(row.Cell(11)),
                CuotaIva10 = ParseDecimal(row.Cell(12)),
                BaseImponible21 = ParseDecimal(row.Cell(14)),
                CuotaIva21 = ParseDecimal(row.Cell(15))
            };
        }

        private static TotalesGenerales ExtraerTotalesGenerales(IXLRow row)
        {
            return new TotalesGenerales
            {
                BaseImponible4 = ParseDecimal(row.Cell(8)),
                CuotaIva4 = ParseDecimal(row.Cell(9)),
                BaseImponible10 = ParseDecimal(row.Cell(11)),
                CuotaIva10 = ParseDecimal(row.Cell(12)),
                BaseImponible21 = ParseDecimal(row.Cell(14)),
                CuotaIva21 = ParseDecimal(row.Cell(15))
            };
        }

        private static decimal ParseDecimal(IXLCell cell)
        {
            if (cell == null || cell.IsEmpty())
                return 0m;

            // Si es número directo
            if (cell.DataType == XLDataType.Number)
                return Convert.ToDecimal(cell.GetDouble());

            var value = cell.GetString().Trim();
            if (string.IsNullOrWhiteSpace(value)) return 0m;

            value = value
                .Replace("€", "")
                .Replace(".", "")
                .Replace(",", ".")
                .Trim();

            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                ? result
                : 0m;
        }
    }
}