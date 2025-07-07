using Conta360.Application.DTOs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries
{
    public class GetPgcAccountTreeQuery : IRequest<List<PgcAccountTreeDto>> { }
}
