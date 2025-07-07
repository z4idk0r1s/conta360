using AutoMapper;
using Conta360.Application.DTOs;
using Conta360.Domain.Interfaces;
using MediatR;
using AutoMapper.Extensions.Microsoft.DependencyInjection;
using MediatR.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries
{
    public class GetAllPgcAccountsQueryHandler : IRequestHandler<GetAllPgcAccountsQuery, List<PgcAccountDto>>
    {
        private readonly IPgcAccountRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPgcAccountsQueryHandler(IPgcAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PgcAccountDto>> Handle(GetAllPgcAccountsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<PgcAccountDto>>(entities);
        }
    }
}
