using MediatR;
using Conta360.Core.Common;
using Conta360.Application.DTOs;
using Conta360.Domain.Interfaces;
using AutoMapper;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries
{
    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, OperationResult<AccountDto>>
    {
        private readonly IPgcAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public GetAccountByIdQueryHandler(IPgcAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<AccountDto>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.Id);
            if (account == null)
            {
                return OperationResult.Failure<AccountDto>(new Error("Account.NotFound", "Account not found."));
            }
            var accountDto = _mapper.Map<AccountDto>(account);
            return OperationResult<AccountDto>.Success(accountDto);
        }
    }
}