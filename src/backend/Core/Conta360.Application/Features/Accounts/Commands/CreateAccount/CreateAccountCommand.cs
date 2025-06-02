using MediatR;
using Conta360.Core.Common;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<OperationResult<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
    }
}