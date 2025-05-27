using System.Threading.Tasks;
using System.Collections.Generic;
using shared_models;
using Conta360Service.Validation.Interfaces;
using AutoMapper;
using Conta360Service.Validation.Models;

namespace Conta360.Services.Api
{
    public class LocalConta360Service : IConta360Service
    {
        private readonly IValidationEngine _validationEngine;
        private readonly IMapper _mapper;

        public LocalConta360Service(IValidationEngine validationEngine, IMapper mapper)
        {
            _validationEngine = validationEngine;
            _mapper = mapper;
        }

        public async Task<ValidationResult> ValidateInvoices(IEnumerable<EmittedInvoiceDto> emittedInvoices,
                                                        IEnumerable<ReceivedInvoiceDto> receivedInvoices)
        {
            var validationResult = new ValidationResult();

            // Validate emitted invoices
            foreach (var invoice in emittedInvoices)
            {
                var validation = await _validationEngine.ValidateEmittedInvoiceAsync(invoice);
                validationResult.AddRange(validation.Errors);
            }

            // Validate received invoices
            foreach (var invoice in receivedInvoices)
            {
                var validation = await _validationEngine.ValidateReceivedInvoiceAsync(invoice);
                validationResult.AddRange(validation.Errors);
            }

            return validationResult;
        }

        public async Task<ValidationResult> ValidateAccountingEntries(IEnumerable<AccountingEntryDto> entries)
        {
            var validationResult = new ValidationResult();

            foreach (var entry in entries)
            {
                var validation = await _validationEngine.ValidateAccountingEntryAsync(entry);
                validationResult.AddRange(validation.Errors);
            }

            return validationResult;
        }

        public Task<bool> IsValidPeriod(int year, int month)
        {
            // Implement offline period validation logic
            return Task.FromResult(true);
        }
    }
}
