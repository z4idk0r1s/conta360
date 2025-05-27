using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using Conta360.Shared.Models.Validation.Models;


namespace Conta360.Domain.Interfaces
{
    public interface IValidationRule<T>
    {
        string RuleName { get; }
        string ErrorMessage { get; }
        Task<ValidationResult> ValidateAsync(T entity);
    }
}
