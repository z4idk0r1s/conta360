using Conta360.Shared.Models.DTOs;
using Microsoft.Extensions.Logging;


namespace Conta360.Shared.Models.Validation
{
    public class ValidationEngine : IValidationEngine
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ValidationEngine> _logger;

        public ValidationEngine(IServiceProvider serviceProvider, ILogger<ValidationEngine> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<ValidationResult> ValidateEmittedInvoiceAsync(EmittedInvoiceDto invoice)
        {
            var rules = _serviceProvider.GetServices<IValidationRule<EmittedInvoiceDto>>();
            var result = new ValidationResult { Source = "EmittedInvoice", EntityId = invoice.Id };

            foreach (var rule in rules)
            {
                try
                {
                    var ruleResult = await rule.ValidateAsync(invoice);
                    if (!ruleResult.IsValid)
                    {
                        result.Errors.AddRange(ruleResult.Errors);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error validating rule {RuleName} for invoice {InvoiceId}", rule.RuleName, invoice.Id);
                    result.AddError("VAL001", $"Error interno en la validación: {rule.RuleName}");
                }
            }

            return result;
        }

        public async Task<ValidationResult> ValidateReceivedInvoiceAsync(ReceivedInvoiceDto invoice)
        {
            var rules = _serviceProvider.GetServices<IValidationRule<ReceivedInvoiceDto>>();
            var result = new ValidationResult { Source = "ReceivedInvoice", EntityId = invoice.Id };

            foreach (var rule in rules)
            {
                try
                {
                    var ruleResult = await rule.ValidateAsync(invoice);
                    if (!ruleResult.IsValid)
                    {
                        result.Errors.AddRange(ruleResult.Errors);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error validating rule {RuleName} for invoice {InvoiceId}", rule.RuleName, invoice.Id);
                    result.AddError("VAL001", $"Error interno en la validación: {rule.RuleName}");
                }
            }

            return result;
        }

        public async Task<ValidationResult> ValidateAccountingEntryAsync(AccountingEntryDto entry)
        {
            var rules = _serviceProvider.GetServices<IValidationRule<AccountingEntryDto>>();
            var result = new ValidationResult { Source = "AccountingEntry", EntityId = entry.Id };

            foreach (var rule in rules)
            {
                try
                {
                    var ruleResult = await rule.ValidateAsync(entry);
                    if (!ruleResult.IsValid)
                    {
                        result.Errors.AddRange(ruleResult.Errors);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error validating rule {RuleName} for entry {EntryId}", rule.RuleName, entry.Id);
                    result.AddError("VAL001", $"Error interno en la validación: {rule.RuleName}");
                }
            }

            return result;
        }

        public async Task<IEnumerable<ValidationResult>> ValidateAllAsync(ValidationContext context)
        {
            var results = new List<ValidationResult>();

            if (context.EmittedInvoices != null)
            {
                foreach (var invoice in context.EmittedInvoices)
                {
                    results.Add(await ValidateEmittedInvoiceAsync(invoice));
                }
            }

            if (context.ReceivedInvoices != null)
            {
                foreach (var invoice in context.ReceivedInvoices)
                {
                    results.Add(await ValidateReceivedInvoiceAsync(invoice));
                }
            }

            if (context.AccountingEntries != null)
            {
                foreach (var entry in context.AccountingEntries)
                {
                    results.Add(await ValidateAccountingEntryAsync(entry));
                }
            }

            return results;
        }
    }
}
