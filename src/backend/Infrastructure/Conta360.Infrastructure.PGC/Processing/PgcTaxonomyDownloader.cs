using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Conta360.Core.Interfaces;
using Conta360.Core.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conta360.Infrastructure.PGC.Processing
{
    public class PgcTaxonomyDownloader : IPgcTaxonomyDownloader
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PgcTaxonomyDownloader> _logger;
        private readonly PgcExtractorOptions _options;

        public PgcTaxonomyDownloader(
            HttpClient httpClient,
            IOptions<PgcExtractorOptions> options,
            ILogger<PgcTaxonomyDownloader> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task DownloadAndExtractAsync(CancellationToken cancellationToken = default)
        {
            var destDir = _options.ExtractDirectory;
            Directory.CreateDirectory(destDir);
            var zipPath = Path.Combine(destDir, _options.ZipFileName);

            _logger.LogInformation(
                "[PGCTaxonomyDownloader] Descargando taxonomía desde {Url}",
                _options.TaxonomyZipUrl);

            var sw = System.Diagnostics.Stopwatch.StartNew();
            using var response = await _httpClient
                .GetAsync(_options.TaxonomyZipUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var fs = new FileStream(
                zipPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fs, cancellationToken);

            sw.Stop();
            _logger.LogInformation(
                "[PGCTaxonomyDownloader] Descarga completada en {Time} ms",
                sw.ElapsedMilliseconds);

            try
            {
                _logger.LogInformation(
                    "[PGCTaxonomyDownloader] Descomprimiendo ZIP en carpeta {Dir}",
                    destDir);
                ZipFile.ExtractToDirectory(zipPath, destDir, overwriteFiles: true);
                _logger.LogInformation("[PGCTaxonomyDownloader] Descompresión finalizada.");

                File.Delete(zipPath);
                _logger.LogInformation(
                    "[PGCTaxonomyDownloader] ZIP eliminado: {ZipPath}",
                    zipPath);
            }
            catch (InvalidDataException ex)
            {
                _logger.LogError(
                    ex,
                    "[PGCTaxonomyDownloader] Error al descomprimir ZIP; puede estar corrupto.");
                throw new InvalidOperationException(
                    "La taxonomía descargada está corrupta.", ex);
            }
        }
    }
}
