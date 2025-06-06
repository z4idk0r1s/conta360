namespace Conta360.Core.Interfaces
{
    /// <summary>
    /// Taxonomía PGC desde ICAC y persistirla.
    /// </summary>
    public interface IPgcImporter
    {
        /// <summary>
        /// Descarga (si procede), parsea y persiste toda la taxonomía PGC (en SQLite/postgres).
        /// </summary>
        Task ImportPgcAsync(CancellationToken cancellationToken = default);
    }
}
