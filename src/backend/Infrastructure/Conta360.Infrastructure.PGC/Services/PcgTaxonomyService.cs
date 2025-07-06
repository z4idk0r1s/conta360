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
using Conta360.Application.Services; // Asegúrate de que este using sea correcto para PgcExtractorOptions y PgcTaxonomyBuilder
using System.Threading;

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
        private readonly string _actualExtractedRootDirectory; // Esta será la base donde esperamos 'EstadosCuentasAnuales'
        private readonly PgcExtractorOptions _options;

        // Se añade EIRL si existe en tu ZIP y necesitas procesarlo.
        // Si la taxonomía EIRL no está dentro de estas carpetas, se omitirá su procesamiento.
        private readonly string[] _modalidades = { "normal", "abreviado", "pymes", "comun", "eirl" }; 

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
            _options = options.Value;

            // --- Validaciones Rigurosas de Configuración ---
            if (string.IsNullOrWhiteSpace(_options.ExtractDirectory))
            {
                _logger.LogError("[PgcTaxonomyService] La configuración 'ExtractDirectory' no puede ser nula o vacía.");
                throw new ArgumentException("ExtractDirectory no puede ser null o vacío en la configuración.", nameof(_options.ExtractDirectory));
            }
            if (string.IsNullOrWhiteSpace(_options.TaxonomyZipUrl))
            {
                _logger.LogError("[PgcTaxonomyService] La configuración 'TaxonomyZipUrl' no puede ser nula o vacía.");
                throw new ArgumentException("TaxonomyZipUrl no puede ser null o vacío en la configuración.", nameof(_options.TaxonomyZipUrl));
            }
            // Aunque ZipFileName no se usa para derivar el nombre de la carpeta extraída,
            // si tu downloader lo utiliza para el nombre del archivo local, es bueno validarlo.
            // Si el downloader usa la URL para el nombre de archivo local, puedes relajar esta validación.
            if (string.IsNullOrWhiteSpace(_options.ZipFileName))
            {
                _logger.LogWarning("[PgcTaxonomyService] La configuración 'ZipFileName' está vacía o nula. Asegúrate de que el downloader pueda manejar esto, o considera configurarla si el nombre del archivo local es crítico.");
            }

            _extractDirectory = _options.ExtractDirectory;

            // --- Determinación Robusta del Directorio Raíz de Extracción ---
            // Tomamos el nombre del archivo ZIP de la URL, que es lo que se espera que WinZip/7-Zip
            // use para crear la carpeta principal al descomprimir.
            var uri = new Uri(_options.TaxonomyZipUrl);
            var zipFileNameFromUrl = Path.GetFileName(uri.LocalPath); // Obtiene "taxonomiaPGC2007_v1.7.0_20240101_r1-EIRL.zip"
            var extractedZipFolderName = Path.GetFileNameWithoutExtension(zipFileNameFromUrl); // Obtiene "taxonomiaPGC2007_v1.7.0_20240101_r1-EIRL"
            
            // La ruta completa hasta el directorio "taxonomia" que contiene "EstadosCuentasAnuales"
            // Ejemplo: C:\ruta\de\extraccion\taxonomiaPGC2007_v1.7.0_20240101_r1-EIRL\taxonomia
            _actualExtractedRootDirectory = Path.Combine(_extractDirectory, extractedZipFolderName, "taxonomia");

            _logger.LogInformation("[PgcTaxonomyService][DIAGNÓSTICO] Directorio de extracción base (configurado): {ExtractDirectory}", _extractDirectory);
            _logger.LogInformation("[PgcTaxonomyService][DIAGNÓSTICO] Nombre de archivo ZIP derivado de URL (esperado para la carpeta): {extractedZipFolderName}", extractedZipFolderName);
            _logger.LogInformation("[PgcTaxonomyService][DIAGNÓSTICO] Carpeta raíz descomprimida esperada (que contiene 'EstadosCuentasAnuales'): {ActualExtractedRootDirectory}", _actualExtractedRootDirectory);
        }

        public async Task<OperationResult<List<PgcAccount>>> RunAndGetAccountsAsync()
        {
            _logger.LogInformation("[PgcTaxonomyService] Iniciando el proceso completo de gestión de taxonomía PGC.");

            // --- Paso 1: Preparación del Directorio de Extracción ---
            if (!Directory.Exists(_extractDirectory))
            {
                try
                {
                    Directory.CreateDirectory(_extractDirectory);
                    _logger.LogInformation("[PgcTaxonomyService] Directorio de extracción base creado: {ExtractDirectory}", _extractDirectory);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[PgcTaxonomyService] Error crítico al crear el directorio de extracción base: {ExtractDirectory}. No se puede continuar.", _extractDirectory);
                    return OperationResult.Failure<List<PgcAccount>>(new Error("DirectoryCreationError", $"Error al crear el directorio de extracción base: {ex.Message}"));
                }
            }

            try
            {
                // --- Paso 2: Descarga y Extracción de la Taxonomía ---
                _logger.LogInformation("[PgcTaxonomyService] Iniciando descarga y extracción de la taxonomía desde '{TaxonomyZipUrl}'.", _options.TaxonomyZipUrl);
                
                // Aquí, tu IPgcTaxonomyDownloader debería descargar el ZIP y extraerlo.
                // Es CRÍTICO que el downloader al extraer el ZIP genere la carpeta 'taxonomiaPGC2007_v1.7.0_20240101_r1-EIRL'
                // directamente dentro de _extractDirectory. Por ejemplo:
                // Si _extractDirectory = "./Data/PGC"
                // Y ZipFileName = "taxonomiaPGC.zip"
                // El downloader debería guardar el ZIP como "./Data/PGC/taxonomiaPGC.zip"
                // Y al descomprimirlo, crear "./Data/PGC/taxonomiaPGC2007_v1.7.0_20240101_r1-EIRL/..."
                await _downloader.DownloadAndExtractAsync(); 
                _logger.LogInformation("[PgcTaxonomyService] Descarga y extracción completadas con éxito.");

                // --- Verificar Estructura de Directorios Post-Extracción ---
                if (!Directory.Exists(_actualExtractedRootDirectory))
                {
                    _logger.LogError("[PgcTaxonomyService] La carpeta raíz descomprimida esperada '{ActualExtractedRootDirectory}' (que debería contener 'EstadosCuentasAnuales') no se encontró después de la extracción. Esto indica un posible problema con el archivo ZIP o su estructura interna esperada.", _actualExtractedRootDirectory);
                    return OperationResult.Failure<List<PgcAccount>>(new Error("ExtractionRootNotFound", $"La carpeta raíz descomprimida '{_actualExtractedRootDirectory}' no se encontró. Confirma que el ZIP se descomprime en una carpeta con el nombre esperado y que la subcarpeta 'taxonomia' existe dentro de ella."));
                }
                _logger.LogInformation("[PgcTaxonomyService] Carpeta raíz descomprimida verificada: {ActualExtractedRootDirectory}.", _actualExtractedRootDirectory);

                var allPgcAccounts = new List<PgcAccount>();

                // --- Mapeo de modalidad a XSD completo de entrada ---
                // Estos son los XSDs que importan todos los demás y que ParseCodesFromXsd debe compilar.
                var entryPointXsds = new Dictionary<string, string>
                {
                    { "normal", Path.Combine(_actualExtractedRootDirectory, "pgc07-normal-completo.xsd") },
                    { "abreviado", Path.Combine(_actualExtractedRootDirectory, "pgc07-abreviado-completo.xsd") },
                    { "pymes", Path.Combine(_actualExtractedRootDirectory, "pgc07-pymes-completo.xsd") },
                    { "mixto", Path.Combine(_actualExtractedRootDirectory, "pgc07-mixto-completo.xsd") }, // Añadido
                    { "eirl", Path.Combine(_actualExtractedRootDirectory, "pgc07-eirl-completo.xsd") }
                };


                // --- Paso 3: Procesamiento de Cada Modalidad de Taxonomía ---
                foreach (var modalidad in _modalidades)
                {
                    // Obtener el XSD completo de entrada para esta modalidad
                    if (!entryPointXsds.TryGetValue(modalidad, out string? mainEntryPointXsdPath) || !File.Exists(mainEntryPointXsdPath))
                    {
                        _logger.LogWarning("[PgcTaxonomyService] No se encontró o no existe el XSD de punto de entrada completo para la modalidad: '{Modalidad}'. Se omitirá esta modalidad.", modalidad);
                        continue;
                    }

                    // La ruta a las modalidades (para label y presentation) debe ser _actualExtractedRootDirectory + "\EstadosCuentasAnuales\" + modalidad
                    var modalidadDir = Path.Combine(_actualExtractedRootDirectory, "EstadosCuentasAnuales", modalidad);
                    _logger.LogInformation("[PgcTaxonomyService] Iniciando procesamiento para modalidad: '{Modalidad}' en '{ModalidadDir}' usando XSD de entrada: '{EntryPointXsd}'.", modalidad, modalidadDir, mainEntryPointXsdPath);

                    if (!Directory.Exists(modalidadDir))
                    {
                        _logger.LogWarning("[PgcTaxonomyService] Directorio de modalidad no encontrado para '{Modalidad}': {ModalidadDir}. Se omitirá esta modalidad.", modalidad, modalidadDir);
                        continue;
                    }

                    // Buscamos todos los archivos XSD específicos de cada estado dentro del directorio de la modalidad.
                    // Estos son los que referencian los archivos de presentación correspondientes.
                    var xsdFiles = Directory.GetFiles(modalidadDir, "*.xsd", SearchOption.TopDirectoryOnly);
                    
                    if (!xsdFiles.Any())
                    {
                        _logger.LogWarning("[PgcTaxonomyService] No se encontraron archivos XSD en el directorio de modalidad '{Modalidad}': {ModalidadDir}. Se omitirá esta modalidad.", modalidad, modalidadDir);
                        continue;
                    }

                    // Se espera un solo archivo de labels general por modalidad, típicamente nombrado como pgc-07-{inicial}-2024-01-01-label-es.xml
                    string generalLabelBaseName = $"pgc-07-{modalidad[0]}-2024-01-01";
                    string? labelPath = Directory.GetFiles(modalidadDir, $"{generalLabelBaseName}-label-es.xml").FirstOrDefault();

                    if (labelPath == null)
                    {
                        _logger.LogWarning("[PgcTaxonomyService] Archivo de etiquetas general para la modalidad '{Modalidad}' en {ModalidadDir} no encontrado. Se omitirá el procesamiento de esta modalidad.", modalidad, modalidadDir);
                        continue;
                    }

                    foreach (var xsd in xsdFiles)
                    {
                        var baseName = Path.GetFileNameWithoutExtension(xsd);
                        string? presentationPath = null;

                        // La lógica para el XSD principal de la modalidad (ej: pgc-07-n-2024-01-01)
                        // Para estos XSD, no se espera un archivo de presentación específico, por lo que 'presentationPath' será null.
                        if (baseName.Equals(generalLabelBaseName))
                        {
                            _logger.LogInformation("[PgcTaxonomyService] Manejando XSD principal de la modalidad: {XsdFileName}. No se espera archivo de presentación.", Path.GetFileName(xsd));
                            // presentationPath se mantiene null.
                        }
                        else // Para los XSD de módulos (m1-balance, m2-pyg, m3-patnetA, m4-flujefec, etc.)
                        {
                            presentationPath = Directory.GetFiles(modalidadDir, $"{baseName}-presentation.xml").FirstOrDefault();
                            if (presentationPath == null)
                            {
                                _logger.LogWarning("[PgcTaxonomyService] Archivo de presentación '{PresentationFileName}' no encontrado para XSD '{XsdFileName}' en {ModalidadDir}. Se omitirá el procesamiento de este XSD.", $"{baseName}-presentation.xml", Path.GetFileName(xsd), modalidadDir);
                                continue;
                            }
                        }

                        _logger.LogInformation("[PgcTaxonomyService] Procesando taxonomía para modalidad '{Modalidad}' con XSD completo: {MainEntryPointXsdFileName}, XSD específico (para presentación): {XsdFileName}, Label: {LabelFileName}, Presentation: {PresentationFileName}.", 
                            modalidad, 
                            Path.GetFileName(mainEntryPointXsdPath), 
                            Path.GetFileName(xsd), 
                            Path.GetFileName(labelPath), 
                            presentationPath != null ? Path.GetFileName(presentationPath) : "N/A (XSD principal)");

                        // --- Construcción de Cuentas PGC ---
                        // **IMPORTANTE**: Aquí pasamos el XSD "completo" para que el builder obtenga TODOS los conceptos.
                        // El `xsd` (el específico del loop) se sigue pasando si el builder lo usa para algo más que extraer conceptos (ej. para la vista específica del balance/pyg).
                        // Sin embargo, si `ParseCodesFromXsd` solo necesita el 'completo', podemos refinar esto aún más.
                        // Según la implementación del builder que proponemos, 'ParseCodesFromXsd' solo usa el 'xsdPath' principal que le llega.
                        // Entonces, en BuildAccountsFromXsdLabelPresentation, el primer parámetro debe ser el mainEntryPointXsdPath.
                        var accountsForModalidad = await _builder.BuildAccountsFromXsdLabelPresentation(mainEntryPointXsdPath, labelPath, presentationPath);
                        allPgcAccounts.AddRange(accountsForModalidad);
                        _logger.LogInformation("[PgcTaxonomyService] Se construyeron {Count} cuentas para la modalidad '{Modalidad}' desde '{XsdFileName}'.", accountsForModalidad.Count, modalidad, Path.GetFileName(xsd));
                    }
                }

                // --- Verificación Final de Datos Procesados ---
                if (!allPgcAccounts.Any())
                {
                    _logger.LogWarning("[PgcTaxonomyService] No se encontraron cuentas PGC para persistir después de procesar todas las modalidades. El proceso finalizó sin datos útiles.");
                    return OperationResult.Failure<List<PgcAccount>>(new Error("NoAccountsFound", "No se encontraron cuentas PGC para persistir. Asegúrate de que los archivos de la taxonomía son correctos y están presentes en la estructura esperada del ZIP."));
                }

                _logger.LogInformation("[PgcTaxonomyService] Se han construido un total de {Count} cuentas PGC. Iniciando persistencia masiva.", allPgcAccounts.Count);

                // --- Paso 4: Persistencia Masiva de Cuentas PGC ---
                await _accountRepository.BulkInsertOrUpdateAsync(allPgcAccounts);
                
                // --- Gestión de Transacciones (si IUnitOfWork lo permite) ---
                // Si tu IUnitOfWork tiene un método para confirmar cambios y agrupar operaciones,
                // este es el lugar ideal para llamarlo, asegurando que toda la persistencia sea atómica.
                // Ejemplo: await _unitOfWork.CommitAsync();
                // Nota: La implementación de IUnitOfWork determinará si es necesario o si BulkInsertOrUpdateAsync ya maneja transacciones internas.

                _logger.LogInformation("[PgcTaxonomyService] Persistencia masiva de cuentas PGC completada con éxito. Proceso de taxonomía finalizado.");
                return OperationResult.Success(allPgcAccounts);
            }
            catch (Exception ex)
            {
                // --- Manejo Centralizado de Errores Críticos ---
                _logger.LogError(ex, "[PgcTaxonomyService] Un error inesperado ocurrió durante el proceso de taxonomía PGC. Mensaje: {ErrorMessage}", ex.Message);
                return OperationResult.Failure<List<PgcAccount>>(new Error("PgcProcessError", $"Error al procesar la taxonomía PGC: {ex.Message}. Consulte los logs para más detalles."));
            }
        }
    }
}