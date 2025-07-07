using Conta360.Application.DTOs;
using MediatR;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries
{
    public class GetAllPgcAccountsQuery : IRequest<List<PgcAccountDto>> { }
}