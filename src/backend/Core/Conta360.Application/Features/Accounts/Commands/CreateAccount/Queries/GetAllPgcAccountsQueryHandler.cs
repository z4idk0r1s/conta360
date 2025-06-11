using AutoMapper;
using Conta360.Application.DTOs;
using Conta360.Domain.Interfaces;
using MediatR;

namespace Conta360.Application.Features.PgcAccounts.Queries
{
    public class GetAllPgcAccountsQueryHandler : IRequestHandler<GetAllPgcAccountsQuery, List<PgcAccountDto>>
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPgcAccountsQueryHandler(IAccountRepository repository, IMapper mapper)
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
