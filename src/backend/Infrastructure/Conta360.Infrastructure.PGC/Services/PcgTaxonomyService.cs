/*
using Conta360.Infrastructure.PGC.Processing;
using Conta360.Application.Interfaces;
using Microsoft.Extensions.Options;
using Conta360.Core.Common;

namespace Conta360.Infrastructure.PGC.Services
{
    public class PgcTaxonomyService : IPgcTaxonomyService
    {
        private readonly PgcTaxonomyDownloader _downloader;
        private readonly PgcTaxonomyBuilder _builder;
        private readonly string _extractDirectory;

        public PgcTaxonomyService(
            PgcTaxonomyDownloader downloader,
            PgcTaxonomyBuilder builder,
            IOptions<PgcExtractorOptions> options)
        {
            _downloader = downloader;
            _builder = builder;
            _extractDirectory = options.Value.ExtractDirectory;
        }

        public async Task RunAsync()
        {
            await _downloader.DownloadAndExtractAsync();

            var xsdFiles = Directory.GetFiles(_extractDirectory, "*.xsd", SearchOption.AllDirectories);

            foreach (var xsd in xsdFiles)
            {
                await _builder.ParseAndPersistAccountsFromXsdAsync(xsd);
            }
        }
    }
}
*/