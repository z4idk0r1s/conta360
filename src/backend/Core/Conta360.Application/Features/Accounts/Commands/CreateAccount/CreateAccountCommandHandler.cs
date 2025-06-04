using MediatR;
using Conta360.Core.Common;
using Conta360.Domain.Entities;
using Conta360.Domain.Interfaces;
using Conta360.Application.Interfaces;
using AutoMapper;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, OperationResult<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public CreateAccountCommandHandler(IUnitOfWork unitOfWork, IAccountRepository accountRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = _mapper.Map<Account>(request);
            await _accountRepository.AddAsync(account);
            await _unitOfWork.CommitAsync();

            return OperationResult<Guid>.Success(account.Id);
        }
    }
}