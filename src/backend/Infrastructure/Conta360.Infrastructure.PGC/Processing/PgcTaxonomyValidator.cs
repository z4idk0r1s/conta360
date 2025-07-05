using System.IO.Compression;
using System.Xml;
using System.Xml.Schema;
using Conta360.Core.Common;
using Conta360.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Conta360.Application.Services;
using System.IO; // Asegúrate de tener este using

namespace Conta360.Infrastructure.PGC.Processing;

public class PgcTaxonomyValidator : IPgcTaxonomyValidator
{
    private readonly ILogger<PgcTaxonomyValidator> _logger;
    private readonly string _schemaBasePath;
    private readonly HttpClient _httpClient; // Inyectar HttpClient

    private static readonly Dictionary<PgcModelType, string> SchemaUrls = new()
    {
        { PgcModelType.Normal, "https://www.icac.gob.es/sites/default/files/pgc2007/v170/pgc07-normal-completo.xsd" },
        { PgcModelType.Abreviado, "https://www.icac.gob.es/sites/default/files/pgc2007/v170/pgc07-abreviado-completo.xsd" },
        { PgcModelType.Pyme, "https://www.icac.gob.es/sites/default/files/pgc2007/v170/pgc07-pymes-completo.xsd" },
        { PgcModelType.Mixto, "https://www.icac.gob.es/sites/default/files/pgc2007/v170/pgc07-mixto-completo.xsd" },
        { PgcModelType.Eirl, "https://www.icac.gob.es/sites/default/files/pgc2007/v170/pgc07-eirl-completo.xsd" }
    };

    // Constructor que recibe HttpClient
    public PgcTaxonomyValidator(ILogger<PgcTaxonomyValidator> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
        _schemaBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schemas", "PGC2007");
        Directory.CreateDirectory(_schemaBasePath);
    }

    public async Task<ValidationResult> ValidateXmlAsync(string xmlContent, PgcModelType modelType)
    {
        string localXsdPath = await EnsureXsdDownloadedAsync(modelType);

        var schemas = new XmlSchemaSet();
        schemas.Add(null, localXsdPath);

        var settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            Schemas = schemas,
            Async = true // Habilitar lectura asíncrona para el XmlReader
        };

        var errors = new List<string>();
        settings.ValidationEventHandler += (sender, args) =>
        {
            if (args.Severity == XmlSeverityType.Error || args.Severity == XmlSeverityType.Warning)
                errors.Add(args.Message);
        };

        // Usa StringReader para el contenido XML en memoria, y XmlReader.Create con configuración asíncrona
        using var reader = XmlReader.Create(new StringReader(xmlContent), settings);
        try
        {
            while (await reader.ReadAsync()) { } // Usa ReadAsync
        }
        catch (XmlException ex)
        {
            errors.Add(ex.Message);
        }
        // Se pueden añadir más capturas de excepciones si se detectan otros problemas específicos durante la validación

        return new ValidationResult
        {
            IsValid = !errors.Any(),
            Errors = errors
        };
    }

    private async Task<string> EnsureXsdDownloadedAsync(PgcModelType modelType)
    {
        var xsdUrl = SchemaUrls[modelType];
        // Uri.AbsolutePath puede incluir '/' al principio, Path.GetFileName lo maneja bien.
        var fileName = Path.GetFileName(new Uri(xsdUrl).AbsolutePath); 
        var localDir = Path.Combine(_schemaBasePath, modelType.ToString());
        var localFilePath = Path.Combine(localDir, fileName);

        if (!File.Exists(localFilePath))
        {
            Directory.CreateDirectory(localDir); // Asegurar que el directorio específico de modalidad existe
            
            _logger.LogInformation("Descargando XSD '{FileName}' desde {Url} a {Path}", fileName, xsdUrl, localFilePath);
            try
            {
                // Usar el HttpClient inyectado
                using var stream = await _httpClient.GetStreamAsync(xsdUrl);
                // *** CORRECCIÓN CLAVE: await using para File.Create y CopyToAsync ***
                await using var fileStream = File.Create(localFilePath);
                await stream.CopyToAsync(fileStream);
                _logger.LogInformation("XSD '{FileName}' descargado correctamente.", fileName);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error HTTP al descargar el XSD '{FileName}' desde {Url}.", fileName, xsdUrl);
                throw new InvalidOperationException($"No se pudo descargar el esquema XSD desde {xsdUrl}.", ex);
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Error de E/S al guardar el XSD '{FileName}' en {Path}.", fileName, localFilePath);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al descargar o guardar el XSD '{FileName}'.", fileName);
                throw;
            }
        }
        else
        {
            _logger.LogInformation("XSD '{FileName}' ya existe localmente en {Path}. No se requiere descarga.", fileName, localFilePath);
        }

        return localFilePath;
    }
}