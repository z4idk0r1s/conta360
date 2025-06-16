using AutoMapper;
using Conta360.Application.DTOs;
using Conta360.Application.Interfaces;
using Conta360.Application.Features.PgcAccounts.Queries;
using Conta360.Application.Services;
using Conta360.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries
{
    public class GetPgcAccountTreeQueryHandler : IRequestHandler<GetPgcAccountTreeQuery, List<PgcAccountTreeDto>>
    {
        private readonly IAccountRepository _repository;

        public GetPgcAccountTreeQueryHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PgcAccountTreeDto>> Handle(GetPgcAccountTreeQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _repository.GetAllAsync();
            return PgcAccountTreeBuilder.BuildTree(accounts);
        }
    }
}
