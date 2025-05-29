using System;

namespace Conta360.Core.Common
{
    public class Error
    {
        public string Code { get; }
        public string Description { get; }

        public Error(string code, string description)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "Null value was provided.");
        public static readonly Error Validation = new("Error.Validation", "Validation failed.");
        // Puedes agregar más errores comunes aquí

        public override string ToString() => $"{Code}: {Description}";
    }
}
