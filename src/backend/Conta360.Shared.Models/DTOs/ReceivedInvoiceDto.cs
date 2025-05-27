using System.Collections.Generic;
using System;

namespace Conta360.Shared.Models.DTOs.ReceivedInvoiceDto
{
    public class ReceivedInvoiceDto
    {
        public string Id { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceLineDto> Lines { get; set; } = new List<InvoiceLineDto>();
        public string InvoiceNumber { get; set; }
        public string SupplierName { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public DateTime? IssueDate { get; set; }


        public string AutoliquidacionEjercicio { get; set; }
        public string AutoliquidacionPeriodo { get; set; }
        public string ActividadTipo { get; set; }
        public string ActividadGrupoOepigrafe { get; set; }
        public string TipoFactura { get; set; }
        public string ConceptoGasto { get; set; }
        public decimal? GastoDeducible { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string SerieNumeroFacturaExpedidor { get; set; }
        public string NumeroFinalFacturaExpedidor { get; set; }
        public string NumeroRecepcion { get; set; }
        public string NumeroRecepcionFinal { get; set; }
        public string TipoNIFExpedidor { get; set; }
        public string CodigoPaisNIFExpedidor { get; set; }
        public string IdentificacionNIFExpedidor { get; set; }
        public string NombreExpedidor { get; set; }
        public string ClaveOperacion { get; set; }
        public decimal? TotalFactura { get; set; }
        public decimal? BaseImponible { get; set; }
        public decimal? TipoIVA { get; set; }
        public decimal? CuotaIVASoportado { get; set; }
        public decimal? CuotaDeducible { get; set; }
        public decimal? TipoRecargoEq { get; set; }
        public decimal? CuotaRecargoEq { get; set; }
        public DateTime? FechaPago { get; set; }
        public decimal? ImportePago { get; set; }
        public string MedioUtilizadoPago { get; set; }
        public string IdentificacionMedioUtilizadoPago { get; set; }
        public string TipoRetencionIRPF { get; set; }
        public decimal? ImporteRetenidoIRPF { get; set; }
    }
}
