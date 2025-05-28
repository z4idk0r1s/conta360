using System.Collections.Generic;
using System.Linq;


namespace Conta360.Shared.Models.Validation
{
    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<ValidationError> Errors { get; } = new List<ValidationError>();
        public string? Source { get; set; }
        public string? EntityId { get; set; }
        public bool HasErrors => Errors.Any(e => e.Severity == ValidationSeverity.Error);


        public void AddError(string code, string message, ValidationSeverity severity = ValidationSeverity.Error)
        {
            Errors.Add(new ValidationError
            {
                Code = code,
                Message = message,
                Severity = severity
            });
        }
        
        public void AddRange(IEnumerable<ValidationError> errors)
        {
            Errors.AddRange(errors);
        }
    }    

    public class ValidationError
    {
        public string? Code { get; set; }
        public string? Message { get; set; }
        public ValidationSeverity Severity { get; set; }
    }

    public enum ValidationSeverity
    {
        Info,
        Warning,
        Error
    }
}
