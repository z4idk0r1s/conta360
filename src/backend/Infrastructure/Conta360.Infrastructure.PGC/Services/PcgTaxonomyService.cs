using Conta360.Infrastructure.PGC.Processing;
using Conta360.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Conta360.Domain.Interfaces;
using Conta360.Core.Common;
using Conta360.Domain.Entities;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Conta360.Core.Interfaces;
using Conta360.Application.Services;

namespace Conta360.Infrastructure.PGC.Services
{
    public class PgcTaxonomyService : IPgcTaxonomyService
    {
        private readonly IPgcTaxonomyDownloader _downloader;
        private readonly PgcTaxonomyBuilder _builder;
        private readonly IPgcAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PgcTaxonomyService> _logger;
        private readonly string _extractDirectory;

        private readonly string[] _modalidades = { "normal", "abreviado", "pymes" };

        public PgcTaxonomyService(
            IPgcTaxonomyDownloader downloader,
            PgcTaxonomyBuilder builder,
            IPgcAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            IOptions<PgcExtractorOptions> options,
            ILogger<PgcTaxonomyService> logger)
        {
            _downloader = downloader;
            _builder = builder;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(options.Value.ExtractDirectory))
                throw new ArgumentException("ExtractDirectory no puede ser null o vacío en la configuración.");
            _extractDirectory = options.Value.ExtractDirectory;
        }

        public async Task<OperationResult<List<PgcAccount>>> RunAndGetAccountsAsync()
        {
            _logger.LogInformation("[PgcTaxonomyService] Iniciando proceso de descarga, construcción y persistencia de taxonomía PGC.");

            if (!Directory.Exists(_extractDirectory))
            {
                try
                {
                    Directory.CreateDirectory(_extractDirectory);
                    _logger.LogInformation("[PgcTaxonomyService] Directorio de extracción creado: {ExtractDirectory}", _extractDirectory);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[PgcTaxonomyService] Error al crear el directorio de extracción: {ExtractDirectory}", _extractDirectory);
                    return OperationResult.Failure<List<PgcAccount>>(new Error("DirectoryCreationError", $"Error al crear el directorio de extracción: {ex.Message}"));
                }
            }

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
                    
                    if (!xsdFiles.Any())
                    {
                         _logger.LogWarning("[PgcTaxonomyService] No se encontraron archivos XSD en el directorio de modalidad: {ModalidadDir}", modalidadDir);
                         continue;
                    }

                    foreach (var xsd in xsdFiles)
                    {
                        var baseName = Path.GetFileNameWithoutExtension(xsd);

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

                        // ¡Añade 'await' aquí!
                        var accountsForModalidad = await _builder.BuildAccountsFromXsdLabelPresentation(xsd, label, presentation);
                        allPgcAccounts.AddRange(accountsForModalidad);
                        _logger.LogInformation("[PgcTaxonomyService] {Count} cuentas construidas para modalidad '{Modalidad}'.", accountsForModalidad.Count, modalidad);
                    }
                }

                if (!allPgcAccounts.Any())
                {
                    _logger.LogWarning("[PgcTaxonomyService] No se encontraron cuentas PGC para persistir después de procesar todas las modalidades.");
                    return OperationResult.Failure<List<PgcAccount>>(new Error("NoAccountsFound", "No se encontraron cuentas PGC para persistir."));
                }

                _logger.LogInformation("[PgcTaxonomyService] Iniciando persistencia masiva de {Count} cuentas PGC.", allPgcAccounts.Count);

                await _accountRepository.BulkInsertOrUpdateAsync(allPgcAccounts);
                
                _logger.LogInformation("[PgcTaxonomyService] Persistencia masiva de cuentas PGC completada con éxito.");
                return OperationResult.Success(allPgcAccounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PgcTaxonomyService] Error durante el proceso de taxonomía PGC.");
                return OperationResult.Failure<List<PgcAccount>>(new Error("PgcProcessError", $"Error al procesar la taxonomía PGC: {ex.Message}"));
            }
            finally
            {
                if (Directory.Exists(_extractDirectory))
                {
                    try
                    {
                        Directory.Delete(_extractDirectory, true);
                        _logger.LogInformation("[PgcTaxonomyService] Directorio de extracción temporal '{ExtractDirectory}' limpiado.", _extractDirectory);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "[PgcTaxonomyService] Error al limpiar el directorio de extracción temporal '{ExtractDirectory}'.", _extractDirectory);
                    }
                }
            }
        }
    }
}