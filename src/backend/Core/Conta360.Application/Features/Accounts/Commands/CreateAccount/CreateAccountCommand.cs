using Conta360.Core.Common;
using MediatR;
using MediatR.Extensions.Microsoft.DependencyInjection;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<OperationResult<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
    }
}