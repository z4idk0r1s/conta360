using System.Collections.Generic;
using System;

namespace Conta360.Shared.Models.DTOs
{
    public class EmittedInvoiceDto
    {
        // Propiedades básicas de la factura emitida (TODAS REQUERIDAS)
        public required string Id { get; set; }
        public required decimal TotalAmount { get; set; }
        public required List<InvoiceLineDto> Lines { get; set; } = new List<InvoiceLineDto>(); // Se inicializa siempre
        public required string InvoiceNumber { get; set; }
        public required string CustomerName { get; set; }
        public required decimal Amount { get; set; }
        public required string Date { get; set; } 
        public required DateTime IssueDate { get; set; }  

        // Propiedades relacionadas con la autoliquidación y actividad (TODAS REQUERIDAS)
        public required string AutoliquidacionEjercicio { get; set; }
        public required string AutoliquidacionPeriodo { get; set; }
        public required string ActividadCodigo { get; set; }
        public required string ActividadTipo { get; set; }
        public required string ActividadGrupoOEpigrafe { get; set; } 
        public required string TipoFactura { get; set; }
        public required string ConceptoIngreso { get; set; }
        public bool IngresoComputable { get; set; }  
        public DateTime FechaExpedicion { get; set; } 
        public DateTime FechaOperacion { get; set; } 

        // Propiedades de identificación de la factura/destinatario (TODAS REQUERIDAS)
        public required string SerieFactura { get; set; }
        public required string NumeroFactura { get; set; }
        public required string NumeroFinalFactura { get; set; }
        public required string TipoNifDestinatario { get; set; }
        public required string CodigoPaisDestinatario { get; set; }
        public required string IdentificacionDestinatario { get; set; }
        public required string NombreDestinatario { get; set; }
        public required string ClaveOperacion { get; set; }
        public required string CalificacionOperacion { get; set; }
        public bool OperacionExenta { get; set; }  

        // Totales y conceptos de IVA/Recargo (TODAS REQUERIDAS)
        public required decimal TotalFactura { get; set; }
        public required decimal BaseImponible { get; set; }
        public required decimal TipoIVA { get; set; }
        public required decimal CuotaIVARepercutida { get; set; }
        public required decimal TipoRecargoEq { get; set; }
        public required decimal CuotaRecargoEq { get; set; }

        // Propiedades de cobro y retención (TODAS REQUERIDAS)
        public DateTime FechaCobro { get; set; } 
        public decimal ImporteCobro { get; set; } 
        public required string MedioUtilizadoCobro { get; set; }
        public required string IdentificacionMedioUtilizadoCobro { get; set; }
        public required string TipoRetencionIRPF { get; set; }
        public decimal ImporteRetenidoIRPF { get; set; } 
        public bool RegistroAcuerdoFacturacion { get; set; }  
    }
}