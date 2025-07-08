using System;
using System.Globalization;

namespace Conta360.Infrastructure.A3Cash.Models
{
    /// <summary>
    /// Representa una única línea (apunte) en un asiento contable para el fichero A3.
    /// Esta clase es un modelo de datos y contiene métodos para formatear sus propios
    /// atributos según la especificación del fichero.
    /// </summary>
    public class AccountingEntry
    {
        public string Account { get; }
        public string AccountDescription { get; }
        public char ImportType { get; } // 'D' para Debe, 'H' para Haber
        public char EntryLineType { get; } // 'I' (Inicio), 'M' (Medio), 'U' (Último)
        public string EntryDescription { get; }
        public decimal Amount { get; }
        public DateTime EntryDate { get; }
        public string DocumentReference { get; }
        public bool IsPayrollEntry { get; }
        public bool HasAnalyticalRecord { get; }

        public AccountingEntry(
            string account,
            string accountDescription,
            char importType,
            char entryLineType,
            string entryDescription,
            decimal amount,
            DateTime entryDate,
            string documentReference = "",
            bool isPayrollEntry = false,
            bool hasAnalyticalRecord = false)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(amount, nameof(amount));
            if (importType is not ('D' or 'H'))
                throw new ArgumentException("El tipo de importe debe ser 'D' (Debe) o 'H' (Haber).", nameof(importType));
            if (entryLineType is not ('I' or 'M' or 'U'))
                throw new ArgumentException("El tipo de línea de apunte debe ser 'I' (Inicio), 'M' (Medio) o 'U' (Último).", nameof(entryLineType));

            Account = account;
            AccountDescription = accountDescription;
            ImportType = importType;
            EntryLineType = entryLineType;
            EntryDescription = entryDescription;
            Amount = amount;
            EntryDate = entryDate;
            DocumentReference = documentReference;
            IsPayrollEntry = isPayrollEntry;
            HasAnalyticalRecord = hasAnalyticalRecord;
        }

        /// <summary>Formatea la cuenta a 12 caracteres, justificada a la izquierda (Pos 16-27).</summary>
        public string GetFormattedAccount() => Account.PadRight(12);

        /// <summary>Formatea la descripción de la cuenta a 30 caracteres, truncando y justificando (Pos 28-57).</summary>
        public string GetFormattedAccountDescription() => AccountDescription.Length > 30 ? AccountDescription[..30].PadRight(30) : AccountDescription.PadRight(30);

        /// <summary>Formatea la referencia del documento a 10 caracteres, truncando y justificando (Pos 59-68).</summary>
        public string GetFormattedDocumentReference() => DocumentReference.Length > 10 ? DocumentReference[..10].PadRight(10) : DocumentReference.PadRight(10);

        /// <summary>Formatea la descripción del apunte a 30 caracteres, truncando y justificando (Pos 70-99).</summary>
        public string GetFormattedEntryDescription() => EntryDescription.Length > 30 ? EntryDescription[..30].PadRight(30) : EntryDescription.PadRight(30);

        /// <summary>
        /// Formatea el importe a 14 caracteres: Signo (+) + 10 enteros con ceros
        /// a la izquierda + punto + 2 decimales (Pos 100-113).
        /// Ejemplo: 123.45 -> +0000000123.45
        /// </summary>
        public string GetFormattedAmount()
        {
            // Usar CultureInfo.InvariantCulture para asegurar el punto decimal y no la coma
            var parts = Amount.ToString("0.00", CultureInfo.InvariantCulture).Split('.');
            return $"+{parts[0].PadLeft(10, '0')}.{parts[1]}";
        }

        /// <summary>Formatea la fecha del apunte al formato AAAAmmdd (Pos 7-14).</summary>
        public string GetFormattedDate() => EntryDate.ToString("yyyyMMdd");
    }
}