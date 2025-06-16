using Conta360.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Conta360.Application.Features.PgcAccounts.Queries
{
    public class GetAllPgcAccountsQuery : IRequest<List<PgcAccountDto>> { }
}