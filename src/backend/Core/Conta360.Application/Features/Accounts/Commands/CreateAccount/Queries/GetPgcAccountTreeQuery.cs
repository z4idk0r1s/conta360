using Conta360.Application.DTOs;
using MediatR;

namespace Conta360.Application.Features.PgcAccounts.Queries
{
    public class GetPgcAccountTreeQuery : IRequest<List<PgcAccountTreeDto>> { }
}
