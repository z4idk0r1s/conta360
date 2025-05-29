using Conta360.Core.Common;

namespace Conta360.Core.Interfaces
{
    public interface IValidator<T>
    {
        OperationResult Validate(T entity);
    }
}