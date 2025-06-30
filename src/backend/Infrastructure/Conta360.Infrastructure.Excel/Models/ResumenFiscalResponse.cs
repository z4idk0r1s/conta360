using System;
using System.Collections.Generic;

namespace Conta360.Infrastructure.Excel.Models.ResumenFiscal
{
    public class ResumenFiscalResponse
    {
        public DateTime FechaInforme { get; init; }
        public DateTime FechaDesde { get; init; }
        public DateTime FechaHasta { get; init; }
        public string Usuario { get; init; } = string.Empty;
        public string Empresa { get; init; } = string.Empty;
        public string NombreComercial { get; init; } = string.Empty;
        public TotalesGenerales Totales { get; init; } = new();
        public List<DetalleDiario> DetallesPorDia { get; init; } = new();
    }
}