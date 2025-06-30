using Conta360.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries
{
    public class GetAllPgcAccountsQuery : IRequest<List<PgcAccountDto>> { }
}