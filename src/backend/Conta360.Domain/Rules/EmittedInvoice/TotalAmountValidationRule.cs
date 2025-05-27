using FluentValidation;
using FluentValidation.Results;
using Conta360.Shared.Models.DTOs;
using Conta360.Domain.Interfaces;
using Conta360.Shared.Models.Validation;


namespace Conta360.Domain.Rules.EmittedInvoice
{
    public class TotalAmountValidationRule : IValidationRule<EmittedInvoiceDto>
    {
        public string RuleName => "TotalAmountValidation";
        public string ErrorMessage => "El importe total de la factura no coincide con la suma de las líneas";

        public async Task<ValidationResult> ValidateAsync(EmittedInvoiceDto invoice)
        {
            return await Task.Run(() =>
            {
                var result = new ValidationResult
                {
                    Source = "EmittedInvoice",
                    EntityId = invoice.Id
                };

                var calculatedTotal = 0m;
                foreach (var line in invoice.Lines)
                {
                    calculatedTotal += line.Amount * (1 + line.TaxRate / 100);
                }

                if (calculatedTotal != invoice.TotalAmount)
                {
                    result.AddError(
                        "INV001",
                        $"El importe total ({invoice.TotalAmount}) no coincide con la suma de las líneas ({calculatedTotal})"
                    );
                }

                return result;
            });
        }
    }
}
