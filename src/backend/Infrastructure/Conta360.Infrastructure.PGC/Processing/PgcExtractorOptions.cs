namespace Conta360.Infrastructure.PGC.Processing
{
    /// <summary>
    /// Opciones configurables para descargar y procesar la taxonomía PGC.
    /// </summary>
    public class PgcExtractorOptions
    {
        /// <summary>
        /// URL del ZIP oficial de la taxonomía en ICAC.
        /// </summary>
        public string TaxonomyZipUrl { get; set; } =
            "https://www.icac.gob.es/sites/default/files/pgc2007/v170/taxonomiaPGC2007_v1.7.0_20240101_r1-EIRL.zip";

        /// <summary>
        /// Nombre con que se guardará el ZIP localmente.
        /// </summary>
        public string ZipFileName { get; set; } = "taxonomiaPGC2007.zip";

        /// <summary>
        /// Carpeta donde se descargará y descomprimirá el ZIP.
        /// </summary>
        public string ExtractDirectory { get; set; } = "./Data/PGC";

        /// <summary>
        /// Si es true, al arrancar la aplicación descargará y procesará la taxonomía automáticamente.
        /// </summary>
        public bool EnableStartupDownload { get; set; } = true;
    }
}
