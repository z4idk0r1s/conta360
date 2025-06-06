namespace Conta360.Core.Interfaces
{
    /// <summary>
    /// Contrato para descargar y descomprimir la taxonomía PGC de ICAC.
    /// </summary>
    public interface IPgcTaxonomyDownloader
    {
        /// <summary>
        /// Descarga (si procede) y descomprime el ZIP en la carpeta configurada.
        /// </summary>
        Task DownloadAndExtractAsync(CancellationToken cancellationToken = default);
    }
}
