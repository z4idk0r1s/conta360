using Conta360.Infrastructure.PGC.Processing;
using Conta360.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging; // Añadir para logging
using Conta360.Domain.Interfaces; // Añadir para IPgcAccountRepository
using Conta360.Core.Common; // Añadir para OperationResult
using Conta360.Domain.Entities; // Añadir para PgcAccount
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Conta360.Application.Services;

namespace Conta360.Infrastructure.PGC.Services
{
    public class PgcTaxonomyService : IPgcTaxonomyService
    {
        private readonly PgcTaxonomyDownloader _downloader;
        private readonly PgcTaxonomyBuilder _builder;
        private readonly IPgcAccountRepository _accountRepository; // Nuevo: Para persistencia
        private readonly IUnitOfWork _unitOfWork; // Nuevo: Para control transaccional
        private readonly ILogger<PgcTaxonomyService> _logger; // Nuevo: Para logging
        private readonly string _extractDirectory;

        // Modalidades soportadas
        private readonly string[] _modalidades = { "normal", "abreviado", "pymes" };

        public PgcTaxonomyService(
            PgcTaxonomyDownloader downloader,
            PgcTaxonomyBuilder builder,
            IPgcAccountRepository accountRepository, // Inyección del repositorio
            IUnitOfWork unitOfWork, // Inyección de la unidad de trabajo
            IOptions<PgcExtractorOptions> options,
            ILogger<PgcTaxonomyService> logger) // Inyección del logger
        {
            _downloader = downloader;
            _builder = builder;
            _accountRepository = accountRepository; // Asignación
            _unitOfWork = unitOfWork; // Asignación
            _logger = logger; // Asignación

            // Validación robusta: nunca null ni vacío
            if (string.IsNullOrWhiteSpace(options.Value.ExtractDirectory))
                throw new ArgumentException("ExtractDirectory no puede ser null o vacío en la configuración.");
            _extractDirectory = options.Value.ExtractDirectory;
        }

        public async Task<OperationResult<List<PgcAccount>>> RunAndGetAccountsAsync()
        {
            _logger.LogInformation("[PgcTaxonomyService] Iniciando proceso de descarga, construcción y persistencia de taxonomía PGC.");

            try
            {
                await _downloader.DownloadAndExtractAsync();
                _logger.LogInformation("[PgcTaxonomyService] Descarga y extracción completadas.");

                var allPgcAccounts = new List<PgcAccount>();

                foreach (var modalidad in _modalidades)
                {
                    var modalidadDir = Path.Combine(_extractDirectory, "EstadosCuentasAnuales", modalidad);
                    if (!Directory.Exists(modalidadDir))
                    {
                        _logger.LogWarning("[PgcTaxonomyService] Directorio de modalidad no encontrado: {ModalidadDir}", modalidadDir);
                        continue;
                    }

                    var xsdFiles = Directory.GetFiles(modalidadDir, "*.xsd", SearchOption.TopDirectoryOnly);
                    foreach (var xsd in xsdFiles)
                    {
                        var baseName = Path.GetFileNameWithoutExtension(xsd);

                        // Busca los XMLs asociados
                        var label = Directory.GetFiles(modalidadDir, $"{baseName}-label-es.xml").FirstOrDefault();
                        if (label == null)
                        {
                            _logger.LogWarning("[PgcTaxonomyService] Archivo de etiquetas no encontrado para {BaseName} en {ModalidadDir}", baseName, modalidadDir);
                            continue;
                        }

                        var presentation = Directory.GetFiles(modalidadDir, $"{baseName}-presentation.xml").FirstOrDefault();
                        if (presentation == null)
                        {
                            _logger.LogWarning("[PgcTaxonomyService] Archivo de presentación no encontrado para {BaseName} en {ModalidadDir}", baseName, modalidadDir);
                            continue;
                        }

                        _logger.LogInformation("[PgcTaxonomyService] Procesando modalidad '{Modalidad}' con XSD: {Xsd}, Label: {Label}, Presentation: {Presentation}", modalidad, xsd, label, presentation);

                        var accountsForModalidad = await _builder.BuildAccountsFromXsdLabelPresentationAsync(xsd, label, presentation);
                        allPgcAccounts.AddRange(accountsForModalidad);
                        _logger.LogInformation("[PgcTaxonomyService] {Count} cuentas construidas para modalidad '{Modalidad}'.", accountsForModalidad.Count, modalidad);
                    }
                }

                if (!allPgcAccounts.Any())
                {
                    _logger.LogWarning("[PgcTaxonomyService] No se encontraron cuentas PGC para persistir después de procesar todas las modalidades.");
                    return OperationResult<List<PgcAccount>>.Fail(new Error("NoAccountsFound", "No se encontraron cuentas PGC para persistir."));
                }

                // Persistencia masiva utilizando el repositorio y el Unit of Work
                _logger.LogInformation("[PgcTaxonomyService] Iniciando persistencia masiva de {Count} cuentas PGC.", allPgcAccounts.Count);

                // No es necesario BeginTransaction ni Commit/Rollback explícitos aquí,
                // ya que BulkInsertOrUpdateAsync maneja su propia transacción interna.
                // Sin embargo, si otras operaciones de SaveChangesAsync se combinaran aquí,
                // entonces sí se necesitaría un BeginTransaction en UnitOfWork.
                // Dado que el requisito es que 'BulkInsertOrUpdateAsync' ya lo gestiona,
                // simplificamos esta parte. Si en el futuro hubiera más operaciones
                // que NO fueran bulk y que necesitaran transaccionalidad con estas,
                // se usaría el _unitOfWork.

                await _accountRepository.BulkInsertOrUpdateAsync(allPgcAccounts);
                // No necesitamos _unitOfWork.CommitAsync() si BulkInsertOrUpdateAsync
                // ya hace el commit internamente. Si el UnitOfWork tuviera un BeginTransaction
                // que envuelva *toda la operación*, entonces sí se usaría.
                // Para mantener la consistencia con el patrón UoW, si *solo* hay operaciones Bulk,
                // la UoW podría ser más un "contexto" para los repositorios.
                // Si queremos que UoW controle una transacción explícita para *múltiples* operaciones
                // Bulk, entonces BulkInsertOrUpdateAsync no debería hacer su propio SaveChanges.

                // Aclaración importante: La nota dice "BulkInsertOrUpdateAsync de EFCore.BulkExtensions
                // maneja internamente el SaveChanges y la transacción". Esto significa que la
                // invocación a este método *ya es una unidad de trabajo autocontenida*.
                // Si la arquitectura requiere que la transacción de UoW sea la predominante
                // y envuelva incluso las operaciones bulk, entonces la implementación de
                // BulkInsertOrUpdateAsync en el repositorio no debería hacer SaveChanges,
                // y el commit sería responsabilidad del IUnitOfWork.
                // DADA LA INFORMACIÓN: "puede ser redundante usar una transacción explícita encima",
                // se asume que BulkInsertOrUpdateAsync gestiona su propia transacción.
                // Si el UnitOfWork debe ser el controlador de transacciones para TODO,
                // el método BulkInsertOrUpdateAsync del repositorio debería
                // simplemente añadir las entidades al contexto y *no* llamar a SaveChanges.
                // Para el propósito de esta refactorización, y priorizando la eficiencia
                // masiva con BulkExtensions, dejaremos que maneje su propia transacción.

                _logger.LogInformation("[PgcTaxonomyService] Persistencia masiva de cuentas PGC completada con éxito.");
                return OperationResult<List<PgcAccount>>.Success(allPgcAccounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PgcTaxonomyService] Error durante el proceso de taxonomía PGC.");
                return OperationResult<List<PgcAccount>>.Fail(new Error("PgcProcessError", $"Error al procesar la taxonomía PGC: {ex.Message}"));
            }
        }
    }
}