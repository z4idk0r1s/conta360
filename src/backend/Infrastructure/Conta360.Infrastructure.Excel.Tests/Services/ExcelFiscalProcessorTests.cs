using System;
using System.Threading.Tasks;
using Conta360.Infrastructure.Excel.Configuration;
using Conta360.Infrastructure.Excel.Services.Implementation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Conta360.Infrastructure.Excel.Tests.Services
{
    public class ExcelFiscalProcessorTests
    {
        private readonly IOptions<ExcelSettings> _settings;
        private readonly ILogger<ExcelFiscalProcessor> _logger;

        public ExcelFiscalProcessorTests()
        {
            _settings = Options.Create(new ExcelSettings
            {
                RutaExcel = "Fixtures/ResumenFiscal.xlsx", // Asegúrate que existe
                HojaResumen = "Sheet1",
                FilaInicioResumen = 11
            });

            _logger = Mock.Of<ILogger<ExcelFiscalProcessor>>();
        }

        [Fact]
        public async Task ProcesarResumenFiscalAsync_DebeRetornarDatosCorrectos()
        {
            // Arrange
            var processor = new ExcelFiscalProcessor(_settings, _logger);

            // Act
            var resultado = await processor.ProcesarResumenFiscalAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("RELU MARIAN PAPALET", resultado.Empresa);
            Assert.Equal("CARNICERIA LOS HERMANOS ALBACETE", resultado.NombreComercial);

            // Verificar totales
            Assert.Equal(45700.65m, resultado.Totales.BaseImponibleTotal);
            Assert.Equal(4761.18m, resultado.Totales.CuotaIvaTotal);
            Assert.Equal(50461.83m, resultado.Totales.ImporteTotal);

            // Verificar primer día (revisar m de cantidades que significa)
            var primerDia = resultado.DetallesPorDia[0];
            Assert.Equal(new DateTime(2025, 5, 1), primerDia.Fecha);
            Assert.Equal(30.00m, primerDia.BaseImponible4);
            Assert.Equal(1.19m, primerDia.CuotaIva4);
        }
    }
}