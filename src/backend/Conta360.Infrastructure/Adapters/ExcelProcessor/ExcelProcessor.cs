using System.Text;
using System.Data;
using AutoMapper;
using Conta360.Application.Interfaces;
using Conta360.Shared.Models.DTOs;


namespace Conta360.Infrastructure.Adapters.ExcelProcessor
{
    public class ExcelProcessor : IExcelProcessor
    {
        private readonly IMapper _mapper;

        public ExcelProcessor(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<(IEnumerable<EmittedInvoiceDto>, IEnumerable<ReceivedInvoiceDto>)> ProcessExcelFile(Stream fileStream)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                var emittedInvoices = new List<EmittedInvoiceDto>();
                var receivedInvoices = new List<ReceivedInvoiceDto>();

                do
                {
                    if (reader.Name.Contains("Emitidas", StringComparison.OrdinalIgnoreCase))
                    {
                        emittedInvoices.AddRange(await ProcessEmittedInvoicesSheetAsync(reader));
                    }
                    else if (reader.Name.Contains("Recibidas", StringComparison.OrdinalIgnoreCase))
                    {
                        receivedInvoices.AddRange(await ProcessReceivedInvoicesSheetAsync(reader));
                    }
                } while (reader.NextResult());

                return (emittedInvoices, receivedInvoices);
            }
        }

        private async Task<IEnumerable<EmittedInvoiceDto>> ProcessEmittedInvoicesSheetAsync(IExcelDataReader reader)
        {
            return await Task.Run(() =>
            {
                var invoices = new List<EmittedInvoiceDto>();
                bool isHeaderRow = true;

                while (reader.Read())
                {
                    if (isHeaderRow)
                    {
                        isHeaderRow = false;
                        continue;
                    }

                    var invoice = new EmittedInvoiceDto
                    {
                        InvoiceNumber = reader.GetString(0),
                        IssueDate = reader.GetDateTime(1),
                        Amount = (decimal)reader.GetDouble(2),
                        // Agrega el resto de los mapeos reales aquí según columnas
                    };

                    invoices.Add(invoice);
                }

                return invoices;
            });
        }

        private async Task<IEnumerable<ReceivedInvoiceDto>> ProcessReceivedInvoicesSheetAsync(IExcelDataReader reader)
        {
            return await Task.Run(() =>
            {
                var invoices = new List<ReceivedInvoiceDto>();
                bool isHeaderRow = true;

                while (reader.Read())
                {
                    if (isHeaderRow)
                    {
                        isHeaderRow = false;
                        continue;
                    }

                    var invoice = new ReceivedInvoiceDto
                    {
                        InvoiceNumber = reader.GetString(0),
                        Date = reader.GetDateTime(1).ToString("yyyy-MM-dd"),
                        Amount = (decimal)reader.GetDouble(2),
                        // Agrega el resto de los mapeos reales aquí según columnas
                    };

                    invoices.Add(invoice);
                }

                return invoices;
            });
        }
    }
}
