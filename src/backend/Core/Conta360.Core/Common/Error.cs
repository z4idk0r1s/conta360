namespace Conta360.Core.Common
{
    public class Error
    {
        public string Code { get; }
        public string Description { get; }

        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public static readonly Error None = new Error(string.Empty, string.Empty);
        public static readonly Error NullValue = new Error("Error.NullValue", "Null value was provided.");
        public static readonly Error Validation = new Error("Error.Validation", "Validation failed.");
        // Add more common errors
    }
}