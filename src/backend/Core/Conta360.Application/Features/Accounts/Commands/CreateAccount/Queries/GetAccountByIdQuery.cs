using MediatR;
using Microsoft.Extensions.DependencyInjection;

using Conta360.Core.Common;
using Conta360.Application.DTOs;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries
{
    public class GetAccountByIdQuery : IRequest<OperationResult<AccountDto>>
    {
        public Guid Id { get; set; }
    }
}