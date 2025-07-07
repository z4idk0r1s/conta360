using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Conta360.Core.Common;

namespace Conta360.Application.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : OperationResult // Assuming all MediatR responses are OperationResult or OperationResult<T>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Any())
            {
                var errors = failures.Select(f => new Error(f.ErrorCode, f.ErrorMessage)).ToList();
                // This cast might need specific handling if TResponse is not directly OperationResult<T>
                // For simplicity, assuming TResponse can be cast to OperationResult for errors
                return (TResponse)OperationResult.Failure(errors.AsReadOnly());
            }

            return await next();
        }
    }
}