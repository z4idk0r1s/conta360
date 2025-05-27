using System.Collections.Generic;
using System;

namespace Conta360.Shared.Models.DTOs.EmittedInvoiceDto
{
    public class EmittedInvoiceDto
    {
        public string Id { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceLineDto> Lines { get; set; } = new List<InvoiceLineDto>();
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public DateTime? IssueDate { get; set; }


        public string AutoliquidacionEjercicio { get; set; }
        public string AutoliquidacionPeriodo { get; set; }
        public string ActividadCodigo { get; set; }
        public string ActividadTipo { get; set; }
        public string ActividadGrupoOEpígrafe { get; set; }
        public string TipoFactura { get; set; }
        public string ConceptoIngreso { get; set; }
        public bool? IngresoComputable { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string SerieFactura { get; set; }
        public string NumeroFactura { get; set; }
        public string NumeroFinalFactura { get; set; }
        public string TipoNifDestinatario { get; set; }
        public string CodigoPaisDestinatario { get; set; }
        public string IdentificacionDestinatario { get; set; }
        public string NombreDestinatario { get; set; }
        public string ClaveOperacion { get; set; }
        public string CalificacionOperacion { get; set; }
        public bool? OperacionExenta { get; set; }
        public decimal TotalFactura { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal TipoIVA { get; set; }
        public decimal CuotaIVARepercutida { get; set; }
        public decimal TipoRecargoEq { get; set; }
        public decimal CuotaRecargoEq { get; set; }
        public DateTime? FechaCobro { get; set; }
        public decimal? ImporteCobro { get; set; }
        public string MedioUtilizadoCobro { get; set; }
        public string IdentificacionMedioUtilizadoCobro { get; set; }
        public string TipoRetencionIRPF { get; set; }
        public decimal? ImporteRetenidoIRPF { get; set; }
        public bool? RegistroAcuerdoFacturacion { get; set; }
    }
}
