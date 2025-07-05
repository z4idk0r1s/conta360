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
        private readonly string _actualExtractedRootDirectory; // Nueva propiedad para la carpeta raíz del ZIP
        private readonly PgcExtractorOptions _options; // Para acceder a TaxonomyZipUrl y ZipFileName

        // Se añade EIRL si existe en tu ZIP y necesitas procesarlo.
        // Si la taxonomía EIRL no está dentro de estas carpetas, se omitirá su procesamiento.
        private readonly string[] _modalidades = { "normal", "abreviado", "pymes", "eirl" }; 

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
            _options = options.Value; // Asignar las opciones

            if (string.IsNullOrWhiteSpace(_options.ExtractDirectory))
            {
                _logger.LogError("[PgcTaxonomyService] La configuración 'ExtractDirectory' no puede ser nula o vacía.");
                throw new ArgumentException("ExtractDirectory no puede ser null o vacío en la configuración.", nameof(_options.ExtractDirectory));
            }
            // Se asegura que ZipFileName también esté configurado si se va a usar para derivar la ruta.
            if (string.IsNullOrWhiteSpace(_options.ZipFileName))
            {
                 _logger.LogError("[PgcTaxonomyService] La configuración 'ZipFileName' no puede ser nula o vacía.");
                throw new ArgumentException("ZipFileName no puede ser null o vacío en la configuración.", nameof(_options.ZipFileName));
            }
            // Se asegura que TaxonomyZipUrl también esté configurado para inferir la carpeta raíz.
            if (string.IsNullOrWhiteSpace(_options.TaxonomyZipUrl))
            {
                 _logger.LogError("[PgcTaxonomyService] La configuración 'TaxonomyZipUrl' no puede ser nula o vacía.");
                throw new ArgumentException("TaxonomyZipUrl no puede ser null o vacío en la configuración.", nameof(_options.TaxonomyZipUrl));
            }

            _extractDirectory = _options.ExtractDirectory;

            var uri = new Uri(_options.TaxonomyZipUrl);
            var zipFileNameFromUrl = Path.GetFileName(uri.LocalPath);
            var extractedRootFolderName = Path.GetFileNameWithoutExtension(zipFileNameFromUrl);
            
            _actualExtractedRootDirectory = Path.Combine(_extractDirectory, extractedRootFolderName);

            _logger.LogInformation("[PgcTaxonomyService][DIAGNOSTICO] Directorio de extracción base: {ExtractDirectory}", _extractDirectory);
            _logger.LogInformation("[PgcTaxonomyService][DIAGNOSTICO] Carpeta raíz descomprimida esperada: {ActualExtractedRootDirectory}", _actualExtractedRootDirectory);
        }

        public async Task<OperationResult<List<PgcAccount>>> RunAndGetAccountsAsync()
        {
            _logger.LogInformation("[PgcTaxonomyService] Iniciando proceso de descarga, construcción y persistencia de taxonomía PGC.");

            // Crear el directorio base de extracción si no existe
            if (!Directory.Exists(_extractDirectory))
            {
                try
                {
                    Directory.CreateDirectory(_extractDirectory);
                    _logger.LogInformation("[PgcTaxonomyService] Directorio de extracción base creado: {ExtractDirectory}", _extractDirectory);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[PgcTaxonomyService] Error al crear el directorio de extracción base: {ExtractDirectory}", _extractDirectory);
                    return OperationResult.Failure<List<PgcAccount>>(new Error("DirectoryCreationError", $"Error al crear el directorio de extracción base: {ex.Message}"));
                }
            }

            try
            {
                await _downloader.DownloadAndExtractAsync(); 
                _logger.LogInformation("[PgcTaxonomyService] Descarga y extracción completadas.");

                // Verificar que el directorio raíz descomprimido existe después de la extracción
                if (!Directory.Exists(_actualExtractedRootDirectory))
                {
                     _logger.LogError("[PgcTaxonomyService] La carpeta raíz descomprimida '{ActualExtractedRootDirectory}' no se encontró después de la extracción. Posible problema con el ZIP o su estructura interna.", _actualExtractedRootDirectory);
                    return OperationResult.Failure<List<PgcAccount>>(new Error("ExtractionRootNotFound", $"La carpeta raíz descomprimida '{_actualExtractedRootDirectory}' no se encontró. Posible problema con el ZIP o su estructura."));
                }
                _logger.LogInformation("[PgcTaxonomyService] Carpeta raíz descomprimida encontrada: {ActualExtractedRootDirectory}", _actualExtractedRootDirectory);


                var allPgcAccounts = new List<PgcAccount>();

                foreach (var modalidad in _modalidades)
                {
                    // Se usa _actualExtractedRootDirectory como base
                    var modalidadDir = Path.Combine(_actualExtractedRootDirectory, "EstadosCuentasAnuales", modalidad);
                    
                    if (!Directory.Exists(modalidadDir))
                    {
                        _logger.LogWarning("[PgcTaxonomyService] Directorio de modalidad no encontrado: {ModalidadDir}", modalidadDir);
                        continue;
                    }

                    // Buscar los archivos XSD específicos de la modalidad dentro de su directorio.
                    // Generalmente, solo habrá un XSD relevante por modalidad.
                    var xsdFiles = Directory.GetFiles(modalidadDir, "*.xsd", SearchOption.TopDirectoryOnly);
                    
                    if (!xsdFiles.Any())
                    {
                        _logger.LogWarning("[PgcTaxonomyService] No se encontraron archivos XSD en el directorio de modalidad: {ModalidadDir}", modalidadDir);
                        continue;
                    }

                    foreach (var xsd in xsdFiles)
                    {
                        var baseName = Path.GetFileNameWithoutExtension(xsd);

                        // Buscar los archivos de etiquetas y presentación asociados al XSD encontrado
                        string? label = Directory.GetFiles(modalidadDir, $"{baseName}-label-es.xml").FirstOrDefault();
                        if (string.IsNullOrEmpty(label))
                        {
                            _logger.LogWarning("[PgcTaxonomyService] Archivo de etiquetas no encontrado para {BaseName} en {ModalidadDir}. Saltando XSD.", baseName, modalidadDir);
                            continue;
                        }

                        string? presentation = Directory.GetFiles(modalidadDir, $"{baseName}-presentation.xml").FirstOrDefault();
                        if (string.IsNullOrEmpty(presentation))
                        {
                            _logger.LogWarning("[PgcTaxonomyService] Archivo de presentación no encontrado para {BaseName} en {ModalidadDir}. Saltando XSD.", baseName, modalidadDir);
                            continue;
                        }

                        _logger.LogInformation("[PgcTaxonomyService] Procesando modalidad '{Modalidad}' con XSD: {Xsd}, Label: {Label}, Presentation: {Presentation}", modalidad, xsd, label, presentation);

                        var accountsForModalidad = await _builder.BuildAccountsFromXsdLabelPresentation(xsd, label, presentation);
                        allPgcAccounts.AddRange(accountsForModalidad);
                        _logger.LogInformation("[PgcTaxonomyService] {Count} cuentas construidas para modalidad '{Modalidad}'.", accountsForModalidad.Count, modalidad);
                    }
                }

                if (!allPgcAccounts.Any())
                {
                    _logger.LogWarning("[PgcTaxonomyService] No se encontraron cuentas PGC para persistir después de procesar todas las modalidades.");
                    return OperationResult.Failure<List<PgcAccount>>(new Error("NoAccountsFound", "No se encontraron cuentas PGC para persistir. Asegúrate de que los archivos de la taxonomía son correctos y están presentes."));
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
        }
    }
}