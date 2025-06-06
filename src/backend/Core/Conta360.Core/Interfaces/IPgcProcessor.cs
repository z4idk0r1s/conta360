namespace Conta360.Core.Interfaces
{
    /// <summary>
    /// Contrato para procesar la taxonomía PGC: parsear XBRL → entidades de dominio.
    /// </summary>
    public interface IPgcProcessor
    {
        /// <summary>
        /// Ejecuta el flujo completo: descarga, parseo y persistencia de entidades PgcAccount.
        /// </summary>
        Task SystemProcessAsync(CancellationToken cancellationToken = default);
    }
}
