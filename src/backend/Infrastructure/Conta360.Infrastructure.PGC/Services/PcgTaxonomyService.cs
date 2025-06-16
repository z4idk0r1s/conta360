using Conta360.Infrastructure.PGC.Extraction;
using Conta360.Infrastructure.PGC.Processing;

namespace Conta360.Infrastructure.PGC.Services
{
    public class PgcTaxonomyService : IPgcTaxonomyService
    {
        private readonly PgcTaxonomyDownloader _downloader;
        private readonly PgcTaxonomyBuilder _builder;

        public PgcTaxonomyService(PgcTaxonomyDownloader downloader, PgcTaxonomyBuilder builder)
        {
            _downloader = downloader;
            _builder = builder;
        }

        public async Task RunAsync()
        {
            var directory = await _downloader.ExtractTaxonomyZipAsync();
            var xsdFiles = Directory.GetFiles(directory, "*.xsd", SearchOption.AllDirectories);

            foreach (var xsd in xsdFiles)
            {
                await _builder.ParseAndPersistAccountsFromXsdAsync(xsd);
            }
        }
    }
}
