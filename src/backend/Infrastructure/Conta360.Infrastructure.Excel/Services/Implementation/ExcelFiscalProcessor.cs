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
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<ResumenFiscalResponse> ProcesarResumenFiscalAsync(
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
                    if (firstCell == "Total")
                    {
                        var totalesGenerales = ExtraerTotalesGenerales(
                            worksheet.Row(currentRow));
                        
                        return new ResumenFiscalResponse
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
                        };
                    }

                    // Si es una línea de total diario
                    if (firstCell.StartsWith("Total") && 
                        firstCell.Contains("(") && 
                        firstCell.Contains(")"))
                    {
                        var detalleDiario = ExtraerDetalleDiario(
                            worksheet.Row(currentRow));
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

        private static (string empresa, string nombreComercial, 
            DateTime fechaDesde, DateTime fechaHasta) ExtraerDatosCabecera(
            IXLWorksheet worksheet)
        {
            var empresa = worksheet.Cell("B4").GetString().Trim();
            var nombreComercial = worksheet.Cell("B5").GetString().Trim();
            
            var fechaDesde = DateTime.ParseExact(
                worksheet.Cell("G4").GetString().Trim(),
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture);
            
            var fechaHasta = DateTime.ParseExact(
                worksheet.Cell("G5").GetString().Trim(),
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture);

            return (empresa, nombreComercial, fechaDesde, fechaHasta);
        }

        private static DetalleDiario? ExtraerDetalleDiario(IXLRow row)
        {
            var fechaStr = row.Cell(1).GetString()
                .Split('(', ')')[1]
                .Trim();

            if (!DateTime.TryParseExact(
                fechaStr,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var fecha))
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
            var value = cell.GetString().Trim();
            if (string.IsNullOrWhiteSpace(value)) return 0m;

            value = value
                .Replace("€", "")
                .Replace(".", "")
                .Replace(",", ".")
                .Trim();

            return decimal.TryParse(
                value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var result) ? result : 0m;
        }
    }
}