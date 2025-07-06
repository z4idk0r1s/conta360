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
using System.Text.RegularExpressions; // Añadir para expresiones regulares

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
        private readonly string _actualExtractedRootDirectory; // Esta será la base donde esperamos 'EstadosCuentasAnuales' y los XSD 'completo'
        private readonly PgcExtractorOptions _options;
        private readonly string _taxonomyDateSuffix; // Nueva variable para almacenar el sufijo de fecha

        // Se añade EIRL si existe en tu ZIP y necesitas procesarlo.
        private readonly string[] _modalidades = { "normal", "abreviado", "pymes", "comun", "eirl"}; 

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
            if (string.IsNullOrWhiteSpace(_options.ZipFileName))
            {
                _logger.LogWarning("[PgcTaxonomyService] La configuración 'ZipFileName' está vacía o nula. Esto puede afectar cómo el downloader nombra el archivo ZIP local. Si no es un problema para tu downloader, puedes ignorar esta advertencia.");
            }

            _extractDirectory = _options.ExtractDirectory;

            // --- Determinación Robusta del Directorio Raíz de Extracción ---
            var uri = new Uri(_options.TaxonomyZipUrl);
            var zipFileNameFromUrl = Path.GetFileName(uri.LocalPath); 
            var extractedZipFolderName = Path.GetFileNameWithoutExtension(zipFileNameFromUrl); 
            
            // La ruta completa hasta el directorio "taxonomia" que contiene "EstadosCuentasAnuales"
            // y los archivos XSD 'completo' (ej: pgc07-normal-completo.xsd).
            // Ejemplo: C:\ruta\de\extraccion\taxonomiaPGC2007_v1.7.0_20240101_r1-EIRL\taxonomia
            _actualExtractedRootDirectory = Path.Combine(_extractDirectory, extractedZipFolderName, "taxonomia");

            // --- Extraer el sufijo de fecha de la carpeta del ZIP ---
            // Buscamos un patrón YYYYMMDD en el nombre de la carpeta extraída.
            var match = Regex.Match(extractedZipFolderName, @"(\d{8})");
            if (match.Success)
            {
                _taxonomyDateSuffix = match.Groups[1].Value;
                _logger.LogInformation("[PgcTaxonomyService][DIAGNÓSTICO] Sufijo de fecha de taxonomía detectado: {TaxonomyDateSuffix}", _taxonomyDateSuffix);
            }
            else
            {
                _taxonomyDateSuffix = "2024-01-01"; // Valor por defecto actualizado.
                _logger.LogWarning("[PgcTaxonomyService][DIAGNÓSTICO] No se pudo extraer el sufijo de fecha (YYYY-MM-DD) del nombre de la carpeta ZIP '{ZipFolderName}'. Se usará el valor por defecto: {DefaultSuffix}", extractedZipFolderName, _taxonomyDateSuffix);
            }

            _logger.LogInformation("[PgcTaxonomyService][DIAGNÓSTICO] Directorio de extracción base (configurado): {ExtractDirectory}", _extractDirectory);
            _logger.LogInformation("[PgcTaxonomyService][DIAGNÓSTICO] Nombre de archivo ZIP derivado de URL (esperado para la carpeta): {extractedZipFolderName}", extractedZipFolderName);
            _logger.LogInformation("[PgcTaxonomyService][DIAGNÓSTICO] Carpeta raíz descomprimida esperada (que contiene 'EstadosCuentasAnuales' y los XSD 'completo'): {ActualExtractedRootDirectory}", _actualExtractedRootDirectory);
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
                
                // Asegúrate de que el downloader se encarga de descomprimir el contenido
                // del ZIP en la estructura de directorios que esperas.
                await _downloader.DownloadAndExtractAsync(); 
                _logger.LogInformation("[PgcTaxonomyService] Descarga y extracción completadas con éxito.");

                // --- Verificar Estructura de Directorios Post-Extracción ---
                // Esta validación ahora es crucial, ya que _actualExtractedRootDirectory es la base para todo lo demás.
                if (!Directory.Exists(_actualExtractedRootDirectory))
                {
                    _logger.LogError("[PgcTaxonomyService] La carpeta raíz descomprimida esperada '{ActualExtractedRootDirectory}' (que debería contener 'EstadosCuentasAnuales' y los XSD 'completo') no se encontró después de la extracción. Esto indica un posible problema con el archivo ZIP o su estructura interna esperada.", _actualExtractedRootDirectory);
                    return OperationResult.Failure<List<PgcAccount>>(new Error("ExtractionRootNotFound", $"La carpeta raíz descomprimida '{_actualExtractedRootDirectory}' no se encontró. Confirma que el ZIP se descomprime en una carpeta con el nombre esperado y que la subcarpeta 'taxonomia' existe dentro de ella."));
                }
                _logger.LogInformation("[PgcTaxonomyService] Carpeta raíz descomprimida verificada: {ActualExtractedRootDirectory}.", _actualExtractedRootDirectory);

                var allPgcAccounts = new List<PgcAccount>();
                var processedCodes = new HashSet<string>(); // Para evitar duplicados si los XSDs de entrada se solapan.

                // --- Mapeo de modalidad a XSD completo de entrada ---
                // Estos son los XSDs que importan todos los demás y que ParseCodesFromXsd debe compilar para obtener TODOS los conceptos.
                // Estos archivos usan el prefijo 'pgc07' y NO LLEVAN SUFIJO DE FECHA. Están directamente en _actualExtractedRootDirectory.
                var entryPointXsds = new Dictionary<string, string>
                {
                    { "normal", Path.Combine(_actualExtractedRootDirectory, "pgc07-normal-completo.xsd") },
                    { "abreviado", Path.Combine(_actualExtractedRootDirectory, "pgc07-abreviado-completo.xsd") },
                    { "pymes", Path.Combine(_actualExtractedRootDirectory, "pgc07-pymes-completo.xsd") },
                    { "comun", Path.Combine(_actualExtractedRootDirectory, "pgc07-comun-completo.xsd") },
                    { "eirl", Path.Combine(_actualExtractedRootDirectory, "pgc07-eirl-completo.xsd") }
                };

                // --- Paso 3: Procesamiento de Cada Modalidad de Taxonomía ---
                foreach (var modalidad in _modalidades)
                {
                    // Obtener el XSD completo de entrada para esta modalidad
                    if (!entryPointXsds.TryGetValue(modalidad, out string? mainEntryPointXsdPath) || !File.Exists(mainEntryPointXsdPath))
                    {
                        // Mensaje de advertencia ajustado para reflejar que el XSD de entrada no lleva sufijo de fecha.
                        _logger.LogWarning("[PgcTaxonomyService] No se encontró o no existe el XSD de punto de entrada completo para la modalidad: '{Modalidad}' en la ruta esperada '{Path}'. Se omitirá esta modalidad.", modalidad, mainEntryPointXsdPath);
                        continue;
                    }

                    // El directorio de la modalidad está dentro de "EstadosCuentasAnuales"
                    var modalidadDir = Path.Combine(_actualExtractedRootDirectory, "EstadosCuentasAnuales", modalidad);
                    _logger.LogInformation("[PgcTaxonomyService] Iniciando procesamiento para modalidad: '{Modalidad}' en '{ModalidadDir}' usando XSD de entrada: '{EntryPointXsd}'.", modalidad, modalidadDir, Path.GetFileName(mainEntryPointXsdPath));

                    if (!Directory.Exists(modalidadDir))
                    {
                        _logger.LogWarning("[PgcTaxonomyService] Directorio de modalidad no encontrado para '{Modalidad}': {ModalidadDir}. Se omitirá esta modalidad.", modalidad, modalidadDir);
                        continue;
                    }

                    // --- LÓGICA DE SELECCIÓN DE UN ÚNICO ARCHIVO DE ETIQUETAS (LABEL) ---
                    // Los archivos de labels dentro de las carpetas de modalidad usan el prefijo 'pgc-07' y SÍ LLEVAN SUFIJO DE FECHA.
                    string? primaryLabelPath = null;
                    // El sufijo de fecha para linkbases es YYYYMMDD.
                    string generalLabelBaseName = $"pgc-07-{modalidad[0]}-{_taxonomyDateSuffix}"; 

                    // 1. Prioridad: Buscar el label principal de la modalidad (sin sufijos de módulos, ej: -m4-flujefec)
                    primaryLabelPath = Directory.GetFiles(modalidadDir, $"{generalLabelBaseName}-label-es.xml").FirstOrDefault();
                    if (primaryLabelPath == null)
                    {
                        primaryLabelPath = Directory.GetFiles(modalidadDir, $"{generalLabelBaseName}-label-es-code.xml").FirstOrDefault();
                    }

                    // 2. Si no se encuentra el general, buscar cualquier otro label de la modalidad que siga el patrón 'pgc-07-'.
                    // Esto es útil para casos como 'comun' o si el label principal tiene un sufijo.
                    if (primaryLabelPath == null)
                    {
                        // Se amplía la búsqueda para cualquier archivo de label que contenga la fecha detectada
                        primaryLabelPath = Directory.EnumerateFiles(modalidadDir, $"pgc-07-*-{_taxonomyDateSuffix}-label-es.xml", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    }
                    if (primaryLabelPath == null)
                    {
                        primaryLabelPath = Directory.EnumerateFiles(modalidadDir, $"pgc-07-*-{_taxonomyDateSuffix}-label-es-code.xml", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    }

                    if (primaryLabelPath == null)
                    {
                        _logger.LogWarning("[PgcTaxonomyService] No se pudo encontrar un archivo de etiquetas (label-es.xml o label-es-code.xml) principal o alternativo para la modalidad '{Modalidad}' con sufijo '{Suffix}' en {ModalidadDir}. Se omitirá el procesamiento de esta modalidad.", modalidad, _taxonomyDateSuffix, modalidadDir);
                        continue;
                    }

                    _logger.LogInformation("[PgcTaxonomyService] Se seleccionó el archivo de etiquetas: {LabelFileName} para la modalidad '{Modalidad}'.", Path.GetFileName(primaryLabelPath), modalidad);

                    // --- RECOLECTAR TODOS LOS ARCHIVOS DE PRESENTACIÓN PARA LA MODALIDAD ---
                    // Los archivos de presentación dentro de las carpetas de modalidad también usan el prefijo 'pgc-07' y SÍ LLEVAN SUFIJO DE FECHA.
                    // Aseguramos que también busquen por la fecha dinámica.
                    var presentationPathsForModalidad = Directory.EnumerateFiles(modalidadDir, $"pgc-07-*-{_taxonomyDateSuffix}-presentation.xml", SearchOption.TopDirectoryOnly).ToList(); 

                    // --- TRATAMIENTO ESPECÍFICO PARA ETCPS (si existe como subcarpeta dentro de la modalidad) ---
                    // El ETCPS puede estar anidado en 'normal' y 'comun', y sus archivos usan el prefijo 'pgc07' y SÍ LLEVAN SUFIJO DE FECHA.
                    if (modalidad == "normal" || modalidad == "comun") 
                    {
                        var etcpnDir = Path.Combine(modalidadDir, "EstadoTotalCambiosPatrimonioNeto"); 
                        if (Directory.Exists(etcpnDir))
                        {
                            // Prefijos específicos para archivos de ETCPS: pgc07n- o pgc07c-. También con sufijo de fecha.
                            string etcpnPrefix = $"pgc07{modalidad[0]}-"; 
                            var etcpnPresentation = Directory.GetFiles(etcpnDir, $"{etcpnPrefix}*-{_taxonomyDateSuffix}-presentation.xml").FirstOrDefault(); 
                            
                            if (etcpnPresentation != null)
                            {
                                presentationPathsForModalidad.Add(etcpnPresentation);
                                _logger.LogInformation("[PgcTaxonomyService] Se añadió el archivo de presentación ETCPS de la subcarpeta '{Modalidad}': {EtcpnPresentation}", modalidad, Path.GetFileName(etcpnPresentation));
                            } else {
                                _logger.LogWarning("[PgcTaxonomyService] No se encontró el archivo de presentación ETCPS ({EtcpnPrefix}*-{Suffix}-presentation.xml) en la subcarpeta esperada '{EtcpnDir}' para la modalidad '{Modalidad}'.", etcpnPrefix, _taxonomyDateSuffix, etcpnDir, modalidad);
                            }
                            // No se añaden labels de ETCPS aquí, ya que el builder solo soporta uno.
                            // Se asume que el label principal de la modalidad es suficiente para sus conceptos.
                        } else {
                            _logger.LogInformation("[PgcTaxonomyService] La subcarpeta ETCPS '{EtcpnDir}' no se encontró para la modalidad '{Modalidad}'.", etcpnDir, modalidad);
                        }
                    }
                    
                    _logger.LogInformation("[PgcTaxonomyService] Se encontraron {Count} archivos de presentación para la modalidad '{Modalidad}'.", presentationPathsForModalidad.Count, modalidad);
                    
                    // --- Construcción de Cuentas PGC para la modalidad (una sola llamada al Builder) ---
                    // Se pasa el XSD de punto de entrada completo para que el builder obtenga TODOS los conceptos.
                    // Se pasa el único archivo de etiquetas general/principal para la modalidad.
                    // Se pasa la lista completa de archivos de presentación para que el builder combine las jerarquías.
                    var accountsForModalidad = await _builder.BuildAccountsFromXsdLabelPresentation(
                        mainEntryPointXsdPath, 
                        primaryLabelPath, 
                        presentationPathsForModalidad
                    );

                    // Añadir solo las cuentas que no hemos procesado ya (por si hay solapamiento entre XSDs de entrada, aunque no debería ser el caso ideal)
                    foreach (var account in accountsForModalidad)
                    {
                        if (processedCodes.Add(account.Code))
                        {
                            allPgcAccounts.Add(account);
                        } else {
                            _logger.LogDebug("[PgcTaxonomyService] Cuenta '{Code}' ya procesada. Se omitirá duplicado.", account.Code);
                        }
                    }
                    _logger.LogInformation("[PgcTaxonomyService] Se procesaron {Count} cuentas (únicas) para la modalidad '{Modalidad}'. Total acumulado: {TotalCount}.", accountsForModalidad.Count, modalidad, allPgcAccounts.Count);
                }

                // --- Verificación Final de Datos Procesados ---
                if (!allPgcAccounts.Any())
                {
                    _logger.LogWarning("[PgcTaxonomyService] No se encontraron cuentas PGC para persistir después de procesar todas las modalidades. El proceso finalizó sin datos útiles.");
                    return OperationResult.Failure<List<PgcAccount>>(new Error("NoAccountsFound", "No se encontraron cuentas PGC para persistir. Asegúrate de que los archivos de la taxonomía son correctos y están presentes en la estructura esperada del ZIP."));
                }

                _logger.LogInformation("[PgcTaxonomyService] Se han construido un total de {Count} cuentas PGC únicas. Iniciando persistencia masiva.", allPgcAccounts.Count);

                // --- Paso 4: Persistencia Masiva de Cuentas PGC ---
                await _accountRepository.BulkInsertOrUpdateAsync(allPgcAccounts);
                
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