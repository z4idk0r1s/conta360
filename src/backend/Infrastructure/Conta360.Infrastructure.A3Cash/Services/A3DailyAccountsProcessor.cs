using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; 
using Conta360.Infrastructure.A3Cash.Interfaces;
using Conta360.Infrastructure.A3Cash.Models;
using Conta360.Infrastructure.A3Cash.Configuration;

namespace Conta360.Infrastructure.A3Cash.Services
{
    /// <summary>
    /// Clase integral para procesar un objeto de datos diarios (fechas e importes)
    /// y generar un fichero de asientos contables compatible con A3 (formato ASCII 512).
    /// </summary>
    public class A3DailyAccountsProcessor : IA3FileGenerator
    {
        private readonly ILogger<A3DailyAccountsProcessor> _logger;
        private readonly A3FileFormatter _fileFormatter;
        private readonly A3CashSettings _settings;

        private readonly string _companyCode;
        private readonly string _defaultDebitAccount;
        private readonly string _defaultDebitAccountDescription;
        private readonly string _defaultCreditAccount;
        private readonly string _defaultCreditAccountDescription;
        private readonly string _outputDirectory;
        private readonly string _defaultDocumentReference;
        private readonly string _baseEntryDescription;

        public A3DailyAccountsProcessor(
            ILogger<A3DailyAccountsProcessor> logger,
            A3FileFormatter fileFormatter,
            IOptions<A3CashSettings> settings) // Inyección de IOptions<A3CashSettings>
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileFormatter = fileFormatter ?? throw new ArgumentNullException(nameof(fileFormatter));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings)); // Obtener la instancia de configuración

            // Validaciones de configuración, usando _settings
            if (string.IsNullOrWhiteSpace(_settings.CompanyCode) || _settings.CompanyCode.Length != 5 || !int.TryParse(_settings.CompanyCode, out _))
            {
                throw new ArgumentException($"El código de empresa '{_settings.CompanyCode}' debe ser una cadena de 5 dígitos numéricos.", nameof(settings.Value.CompanyCode));
            }
            if (string.IsNullOrWhiteSpace(_settings.DefaultDebitAccount)) throw new ArgumentException("La cuenta de débito por defecto no puede estar vacía.", nameof(settings.Value.DefaultDebitAccount));
            if (string.IsNullOrWhiteSpace(_settings.DefaultCreditAccount)) throw new ArgumentException("La cuenta de crédito por defecto no puede estar vacía.", nameof(settings.Value.DefaultCreditAccount));

            // Asignar los valores de _settings a los campos readonly para uso interno
            _companyCode = _settings.CompanyCode;
            _defaultDebitAccount = _settings.DefaultDebitAccount;
            _defaultDebitAccountDescription = _settings.DefaultDebitAccountDescription;
            _defaultCreditAccount = _settings.DefaultCreditAccount;
            _defaultCreditAccountDescription = _settings.DefaultCreditAccountDescription;
            _outputDirectory = _settings.OutputDirectory;
            _defaultDocumentReference = _settings.DefaultDocumentReference;
            _baseEntryDescription = _settings.BaseEntryDescription;

            CreateOutputDirectory();
            _logger.LogInformation($"A3DailyAccountsProcessor inicializado para empresa '{_companyCode}' en '{_outputDirectory}'");
        }

        private void CreateOutputDirectory()
        {
            try
            {
                Directory.CreateDirectory(_outputDirectory);
                _logger.LogInformation($"Directorio de salida preparado: '{_outputDirectory}'");
            }
            catch (IOException ex)
            {
                _logger.LogCritical($"Error fatal: No se pudo crear el directorio de salida '{_outputDirectory}'. {ex.Message}");
                throw;
            }
        }

        private List<AccountingEntry> CreateDailyEntries(Dictionary<DateOnly, decimal> dailyData)
        {
            var allEntries = new List<AccountingEntry>(dailyData.Count * 2); // Pre-asignar capacidad

            foreach (var item in dailyData.OrderBy(item => item.Key)) // Ordenar para consistencia
            {
                DateOnly entryDateOnly = item.Key;
                decimal amount = item.Value;
                DateTime entryDateTime = entryDateOnly.ToDateTime(TimeOnly.MinValue);
                string finalEntryDescription = $"{_baseEntryDescription} {entryDateOnly.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}";

                // Apunte al Haber (Crédito)
                allEntries.Add(new AccountingEntry(
                    entryDate: entryDateTime,
                    account: _defaultCreditAccount,
                    accountDescription: _defaultCreditAccountDescription,
                    importType: 'H',
                    documentReference: _defaultDocumentReference,
                    entryLineType: 'I', // Este es el primer apunte del asiento
                    entryDescription: finalEntryDescription,
                    amount: amount));

                // Apunte al Debe (Débito)
                allEntries.Add(new AccountingEntry(
                    entryDate: entryDateTime,
                    account: _defaultDebitAccount,
                    accountDescription: _defaultDebitAccountDescription,
                    importType: 'D',
                    documentReference: _defaultDocumentReference,
                    entryLineType: 'U', // Este es el último apunte del asiento (para dos líneas)
                    entryDescription: finalEntryDescription,
                    amount: amount));
            }
            return allEntries;
        }

        private async Task<string> WriteFileAsync(List<AccountingEntry> entries, string fileName)
        {
            if (entries is not { Count: > 0 })
            {
                _logger.LogWarning($"No hay apuntes para escribir en el fichero '{fileName}'.");
                return Path.Combine(_outputDirectory, fileName);
            }

            string filePath = Path.Combine(_outputDirectory, fileName);
            _logger.LogInformation($"Iniciando escritura del fichero '{filePath}' con {entries.Count} apuntes.");

            try
            {
                // Usar StreamWriter con la codificación específica para escribir texto directamente
                await using (var writer = new StreamWriter(filePath, false, Encoding.GetEncoding("windows-1252")))
                {
                    foreach (var entry in entries)
                    {
                        var line = _fileFormatter.BuildLine(entry); // BuildLine ahora devuelve string
                        await writer.WriteAsync(line);
                    }
                }

                _logger.LogInformation($"✅ Fichero '{filePath}' generado exitosamente.");
                Console.WriteLine($"✅ Fichero '{filePath}' generado exitosamente.");
                return filePath;
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, $"Error al escribir el fichero '{filePath}': {ex.Message}");
                Console.WriteLine($"❌ Error al escribir el fichero '{filePath}': {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al escribir el fichero '{filePath}': {ex.Message}");
                Console.WriteLine($"❌ Error inesperado al escribir el fichero '{filePath}': {ex.Message}");
                throw;
            }
        }

        public async Task<string> ProcessAndGenerateAsync(Dictionary<DateOnly, decimal> dailyData, string outputFilename = "SUENLACE_FGLD.DAT") // Nombre de archivo por defecto actualizado
        {
            _logger.LogInformation($"Iniciando el procesamiento de datos diarios para generar '{outputFilename}'.");

            if (dailyData == null)
            {
                throw new ArgumentNullException(nameof(dailyData), "El objeto 'dailyData' no puede ser nulo.");
            }
            if (!dailyData.Any())
            {
                _logger.LogWarning("El objeto de datos diarios está vacío. No se generará ningún asiento.");
                Console.WriteLine("❗ Objeto de datos diarios vacío. No se generará ningún fichero.");
                return Path.Combine(_outputDirectory, outputFilename); // Retorna la ruta esperada aunque no se cree el archivo
            }

            try
            {
                List<AccountingEntry> entriesToWrite = CreateDailyEntries(dailyData);
                string generatedFilePath = await WriteFileAsync(entriesToWrite, outputFilename);
                _logger.LogInformation("Procesamiento y generación de fichero completados exitosamente.");
                return generatedFilePath;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Error crítico en A3DailyAccountsProcessor.ProcessAndGenerateAsync: {ex.Message}");
                Console.WriteLine($"❌ Error al procesar y generar el fichero: {ex.Message}");
                throw;
            }
        }
    }
}