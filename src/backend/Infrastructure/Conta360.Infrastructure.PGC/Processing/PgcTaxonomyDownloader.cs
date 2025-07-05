using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Conta360.Core.Interfaces;
using Conta360.Application.Services;
using Conta360.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

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
            _logger.LogInformation(
                "[PGCTaxonomyDownloader][DIAGNOSTICO] Opciones cargadas: TaxonomyZipUrl={TaxonomyZipUrl}, ZipFileName={ZipFileName}, ExtractDirectory={ExtractDirectory}, EnableStartupDownload={EnableStartupDownload}",
                _options.TaxonomyZipUrl, _options.ZipFileName, _options.ExtractDirectory, _options.EnableStartupDownload);

            // Validación robusta de configuración
            if (string.IsNullOrWhiteSpace(_options.ExtractDirectory))
                throw new ArgumentException("ExtractDirectory no puede ser null o vacío en la configuración.");
            if (string.IsNullOrWhiteSpace(_options.ZipFileName))
                throw new ArgumentException("ZipFileName no puede ser null o vacío en la configuración.");
            if (string.IsNullOrWhiteSpace(_options.TaxonomyZipUrl))
                throw new ArgumentException("TaxonomyZipUrl no puede ser null o vacío en la configuración.");

            var destDir = _options.ExtractDirectory;
            Directory.CreateDirectory(destDir!); // Ya está validado

            var zipPath = Path.Combine(destDir!, _options.ZipFileName!);

            _logger.LogInformation(
                "[PGCTaxonomyDownloader] Descargando taxonomía desde {Url}",
                _options.TaxonomyZipUrl);

            var sw = System.Diagnostics.Stopwatch.StartNew();

            // CORRECCIÓN CLAVE: Usar await using para asegurar que el FileStream se cierra asíncronamente y libera el archivo
            using (var response = await _httpClient.GetAsync(_options.TaxonomyZipUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                await using (var fs = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                {
                    await response.Content.CopyToAsync(fs, cancellationToken);
                } // El 'await using' asegura que 'fs' se desecha y el archivo se libera aquí.
            } // El 'using' asegura que 'response' se desecha aquí.

            sw.Stop();
            _logger.LogInformation(
                "[PGCTaxonomyDownloader] Descarga completada en {Time} ms",
                sw.ElapsedMilliseconds);

            try
            {
                _logger.LogInformation(
                    "[PGCTaxonomyDownloader] Descomprimiendo ZIP en carpeta {Dir}",
                    destDir);
                // Ahora el zipPath debería estar libre y accesible para la descompresión
                ZipFile.ExtractToDirectory(zipPath, destDir, overwriteFiles: true);
                _logger.LogInformation("[PGCTaxonomyDownloader] Descompresión finalizada.");
            }
            catch (InvalidDataException ex)
            {
                _logger.LogError(
                    ex,
                    "[PGCTaxonomyDownloader] Error al descomprimir ZIP; puede estar corrupto.");
                throw new InvalidOperationException(
                    "La taxonomía descargada está corrupta.", ex);
            }
            // Capturar IOException específicamente para dar un mensaje más claro si persiste el problema
            catch (IOException ex)
            {
                _logger.LogError(ex, "[PGCTaxonomyDownloader] Error de E/S al descomprimir o eliminar el ZIP. Archivo: {ZipPath}", zipPath);
                throw; // Re-lanza la excepción para que sea manejada por el PgcTaxonomyService
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PGCTaxonomyDownloader] Error inesperado durante la descompresión de la taxonomía.");
                throw;
            }
        }
    }
}