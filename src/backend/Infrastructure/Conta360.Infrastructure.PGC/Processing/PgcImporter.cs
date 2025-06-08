using Conta360.Core.Interfaces;
using Conta360.Core.Common;

namespace Conta360.Infrastructure.PGC.Processing
{
    /// <inheritdoc />
    public class PgcImporter : IPgcImporter
    {
        private readonly IPgcProcessor _processor;

        public PgcImporter(IPgcProcessor processor)
        {
            _processor = processor;
        }

        /// <summary>
        /// Llama a PgcProcessor para descargar, parsear y persistir la taxonomía PGC.
        /// </summary>
        public async Task ImportPgcAsync(CancellationToken cancellationToken = default)
        {
            await _processor.SystemProcessAsync(cancellationToken);
        }
    }
}
