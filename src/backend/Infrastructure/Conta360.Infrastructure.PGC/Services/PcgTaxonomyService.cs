using Conta360.Infrastructure.PGC.Processing;
using Conta360.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Conta360.Core.Common;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Conta360.Infrastructure.PGC.Services
{
    public class PgcTaxonomyService : IPgcTaxonomyService
    {
        private readonly PgcTaxonomyDownloader _downloader;
        private readonly PgcTaxonomyBuilder _builder;
        private readonly string _extractDirectory;

        // Modalidades soportadas
        private readonly string[] _modalidades = { "normal", "abreviado", "pymes" };

        public PgcTaxonomyService(
            PgcTaxonomyDownloader downloader,
            PgcTaxonomyBuilder builder,
            IOptions<PgcExtractorOptions> options)
        {
            _downloader = downloader;
            _builder = builder;
            // Validación robusta: nunca null ni vacío
            if (string.IsNullOrWhiteSpace(options.Value.ExtractDirectory))
                throw new ArgumentException("ExtractDirectory no puede ser null o vacío en la configuración.");
            _extractDirectory = options.Value.ExtractDirectory;
        }

        public async Task RunAsync()
        {
            await _downloader.DownloadAndExtractAsync();

            foreach (var modalidad in _modalidades)
            {
                var modalidadDir = Path.Combine(_extractDirectory, "EstadosCuentasAnuales", modalidad);
                if (!Directory.Exists(modalidadDir)) continue;

                var xsdFiles = Directory.GetFiles(modalidadDir, "*.xsd", SearchOption.TopDirectoryOnly);
                foreach (var xsd in xsdFiles)
                {
                    var baseName = Path.GetFileNameWithoutExtension(xsd);

                    // Busca los XMLs asociados
                    var label = Directory.GetFiles(modalidadDir, $"{baseName}-label-es.xml").FirstOrDefault();
                    if (label == null) continue;

                    var presentation = Directory.GetFiles(modalidadDir, $"{baseName}-presentation.xml").FirstOrDefault();
                    if (presentation == null) continue;

                    await _builder.ParseAndPersistAccountsFromXsdLabelPresentationAsync(xsd, label, presentation);
                }
            }
        }
    }
}