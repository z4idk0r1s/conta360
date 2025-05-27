
namespace Conta360.Shared.Models.DTOs
{
    public class ReceivedInvoiceDto
    {
        // Propiedades básicas de la factura recibida (TODAS REQUERIDAS)
        public required string Id { get; set; }
        public required decimal TotalAmount { get; set; }
        public required List<InvoiceLineDto> Lines { get; set; } = new List<InvoiceLineDto>(); // Siempre se inicializa
        public required string InvoiceNumber { get; set; }
        public required string SupplierName { get; set; }
        public required decimal Amount { get; set; }
        public required string Date { get; set; } // OJO: Si es una fecha, considera DateTime. En este caso, si es string y requerido, así queda.
        public required DateTime IssueDate { get; set; }

        // Propiedades relacionadas con la autoliquidación y actividad (TODAS REQUERIDAS)
        public required string AutoliquidacionEjercicio { get; set; }
        public required string AutoliquidacionPeriodo { get; set; }
        public required string ActividadTipo { get; set; }
        public required string ActividadGrupoOepigrafe { get; set; }
        public required string TipoFactura { get; set; }
        public required string ConceptoGasto { get; set; }
        public decimal GastoDeducible { get; set; } // No anulable
        public DateTime FechaExpedicion { get; set; } // No anulable
        public DateTime FechaOperacion { get; set; } // No anulable

        // Propiedades de identificación de la factura/expedidor (TODAS REQUERIDAS)
        public required string SerieNumeroFacturaExpedidor { get; set; }
        public required string NumeroFinalFacturaExpedidor { get; set; }
        public required string NumeroRecepcion { get; set; }
        public required string NumeroRecepcionFinal { get; set; }
        public required string TipoNIFExpedidor { get; set; }
        public required string CodigoPaisNIFExpedidor { get; set; }
        public required string IdentificacionNIFExpedidor { get; set; }
        public required string NombreExpedidor { get; set; }
        public required string ClaveOperacion { get; set; }

        // Totales y conceptos de IVA/Recargo (TODAS REQUERIDAS)
        public decimal TotalFactura { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal TipoIVA { get; set; }
        public decimal CuotaIVASoportado { get; set; }
        public decimal CuotaDeducible { get; set; }
        public decimal TipoRecargoEq { get; set; }
        public decimal CuotaRecargoEq { get; set; }

        // Propiedades de pago y retención (TODAS REQUERIDAS)
        public DateTime FechaPago { get; set; }
        public decimal ImportePago { get; set; }
        public required string MedioUtilizadoPago { get; set; }
        public required string IdentificacionMedioUtilizadoPago { get; set; }
        public required string TipoRetencionIRPF { get; set; }
        public decimal ImporteRetenidoIRPF { get; set; }
    }
}