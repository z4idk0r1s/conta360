using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; 
using Conta360.Infrastructure.A3Cash.Models;
using Conta360.Infrastructure.A3Cash.Configuration; 

namespace Conta360.Infrastructure.A3Cash.Services
{
    /// <summary>
    /// Se encarga de formatear un objeto AccountingEntry en una línea de texto compatible con el formato A3 ASCII 512.
    /// </summary>
    public class A3FileFormatter
    {
        private readonly ILogger<A3FileFormatter> _logger;
        private readonly string _companyCode;
        private const int LineLength = 512; // Longitud fija de la línea en el formato A3
        private static readonly Encoding _encoding = Encoding.GetEncoding("windows-1252"); // Codificación cp1252

        public A3FileFormatter(ILogger<A3FileFormatter> logger, IOptions<A3CashSettings> settings) // Inyección de IOptions<A3CashSettings>
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            var a3CashSettings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrWhiteSpace(a3CashSettings.CompanyCode) || a3CashSettings.CompanyCode.Length != 5 || !int.TryParse(a3CashSettings.CompanyCode, out _))
            {
                throw new ArgumentException("El código de empresa debe ser una cadena de 5 dígitos numéricos.", nameof(settings.Value.CompanyCode));
            }
            _companyCode = a3CashSettings.CompanyCode;
        }

        /// <summary>
        /// Ensambla una única línea de 512 bytes a partir de un objeto AccountingEntry.
        /// </summary>
        /// <param name="entry">Objeto AccountingEntry con los datos a formatear.</param>
        /// <returns>Una cadena de texto con la línea A3 completa (510 caracteres + CR+LF).</returns>
        public string BuildLine(AccountingEntry entry)
        {
            var lineBuilder = new StringBuilder(LineLength - 2); // Pre-asignar tamaño para eficiencia (510 caracteres)

            lineBuilder.Append('5')                                     // Pos 1-1 (Tipo de registro)
                        .Append(_companyCode)                           // Pos 2-6 (Código de empresa)
                        .Append(entry.GetFormattedDate())               // Pos 7-14 (Fecha del apunte AAAAmmdd)
                        .Append('0')                                    // Pos 15-15 (Tipo de asiento - '0' para manual)
                        .Append(entry.GetFormattedAccount())            // Pos 16-27 (Cuenta contable)
                        .Append(entry.GetFormattedAccountDescription()) // Pos 28-57 (Descripción de la cuenta)
                        .Append(entry.ImportType)                       // Pos 58-58 (D/H)
                        .Append(entry.GetFormattedDocumentReference())  // Pos 59-68 (Referencia documento)
                        .Append(entry.EntryLineType)                    // Pos 69-69 (Tipo de línea I/M/U)
                        .Append(entry.GetFormattedEntryDescription())   // Pos 70-99 (Descripción del apunte)
                        .Append(entry.GetFormattedAmount())             // Pos 100-113 (Importe + signo)
                        .Append(' ', 137)                               // Pos 114-250 (Reserva 1) - Longitud 137
                        .Append(entry.IsPayrollEntry ? 'S' : ' ')       // Pos 251-251 (Apunte de nómina)
                        .Append(entry.HasAnalyticalRecord ? 'S' : ' ')  // Pos 252-252 (Apunte con analítica)
                        .Append(' ', 256)                               // Pos 253-508 (Reserva 2) - Longitud 256
                        .Append('E')                                    // Pos 509-509 (Moneda de enlace - 'E' para Euro)
                        .Append('N');                                   // Pos 510-510 (Indicador de generado - 'N' para Normal)

            var lineStr = lineBuilder.ToString();

            // Asegurar que la longitud es exactamente 510 caracteres antes de añadir CR+LF
            // Esto es una salvaguarda, el StringBuilder debería haber construido la longitud correcta.
            if (lineStr.Length != (LineLength - 2))
            {
                _logger.LogWarning($"La longitud de la línea construida ({lineStr.Length}) no coincide con la esperada ({LineLength - 2}). Ajustando.");
                lineStr = lineStr.PadRight(LineLength - 2); // Rellenar con espacios si es más corta
                if (lineStr.Length > (LineLength - 2))
                {
                    lineStr = lineStr[..(LineLength - 2)]; // Truncar si es más larga (no debería ocurrir con PadRight)
                }
            }

            return lineStr + "\r\n"; // Añadir CR+LF (Carriage Return + Line Feed) para formar la línea de 512 bytes
        }

        /// <summary>
        /// Convierte una línea de texto A3 a su representación en bytes usando la codificación cp1252.
        /// (Este método puede ser útil para pruebas o si se requiere la representación en bytes directamente,
        /// pero el StreamWriter ya maneja el encoding de string a bytes).
        /// </summary>
        public byte[] GetLineBytes(string line) => _encoding.GetBytes(line);
    }
}