namespace Conta360.Application.Services
{
    /// <summary>
    /// Opciones configurables para descargar y procesar la taxonomía PGC.
    /// </summary>
    public class PgcExtractorOptions
    {
        public string? TaxonomyZipUrl { get; set; }
        public string? ZipFileName { get; set; }
        public string? ExtractDirectory { get; set; }
        public bool EnableStartupDownload { get; set; }
    }
}