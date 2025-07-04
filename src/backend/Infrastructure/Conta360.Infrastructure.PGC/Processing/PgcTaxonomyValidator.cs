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

namespace Conta360.Infrastructure.PGC.Processing;

public class PgcTaxonomyValidator : IPgcTaxonomyValidator
{
    private readonly ILogger<PgcTaxonomyValidator> _logger;
    private readonly string _schemaBasePath;

    private static readonly Dictionary<PgcModelType, string> SchemaUrls = new()
    {
        { PgcModelType.Normal, "https://www.icac.gob.es/sites/default/files/pgc2007/v170/pgc07-normal-completo.xsd" },
        { PgcModelType.Abreviado, "https://www.icac.gob.es/sites/default/files/pgc2007/v170/pgc07-abreviado-completo.xsd" },
        { PgcModelType.Pyme, "https://www.icac.gob.es/sites/default/files/pgc2007/v170/pgc07-pymes-completo.xsd" },
        { PgcModelType.Mixto, "https://www.icac.gob.es/sites/default/files/pgc2007/v170/pgc07-mixto-completo.xsd" },
        { PgcModelType.Eirl, "https://www.icac.gob.es/sites/default/files/pgc2007/v170/pgc07-eirl-completo.xsd" }
    };

    public PgcTaxonomyValidator(ILogger<PgcTaxonomyValidator> logger)
    {
        _logger = logger;
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
            Schemas = schemas
        };

        var errors = new List<string>();
        settings.ValidationEventHandler += (sender, args) =>
        {
            if (args.Severity == XmlSeverityType.Error || args.Severity == XmlSeverityType.Warning)
                errors.Add(args.Message);
        };

        using var reader = XmlReader.Create(new StringReader(xmlContent), settings);
        try
        {
            while (reader.Read()) { }
        }
        catch (XmlException ex)
        {
            errors.Add(ex.Message);
        }

        return new ValidationResult
        {
            IsValid = !errors.Any(),
            Errors = errors
        };
    }

    private async Task<string> EnsureXsdDownloadedAsync(PgcModelType modelType)
    {
        var xsdUrl = SchemaUrls[modelType];
        var fileName = Path.GetFileName(new Uri(xsdUrl).AbsolutePath);
        var localFilePath = Path.Combine(_schemaBasePath, modelType.ToString(), fileName);

        if (!File.Exists(localFilePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(localFilePath)!);
            using var client = new HttpClient();
            var stream = await client.GetStreamAsync(xsdUrl);
            await using var fileStream = File.Create(localFilePath);
            await stream.CopyToAsync(fileStream);
            _logger.LogInformation("XSD descargado: {Path}", localFilePath);
        }

        return localFilePath;
    }
}
