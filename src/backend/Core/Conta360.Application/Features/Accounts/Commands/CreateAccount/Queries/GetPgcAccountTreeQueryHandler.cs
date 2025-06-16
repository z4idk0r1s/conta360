using AutoMapper;
using Conta360.Application.DTOs;
using Conta360.Application.Interfaces;
using Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries;
using Conta360.Application.Services;
using Conta360.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries
{
    public class GetPgcAccountTreeQueryHandler : IRequestHandler<GetPgcAccountTreeQuery, List<PgcAccountTreeDto>>
    {
        private readonly IPgcAccountRepository _repository;
        private readonly IMapper _mapper;

        public GetPgcAccountTreeQueryHandler(IPgcAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PgcAccountTreeDto>> Handle(GetPgcAccountTreeQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _repository.GetAllAsync();
            var accountDtos = _mapper.Map<List<PgcAccountDto>>(accounts); 
            return PgcAccountTreeBuilder.BuildTree(accountDtos);
        }
    }
}
