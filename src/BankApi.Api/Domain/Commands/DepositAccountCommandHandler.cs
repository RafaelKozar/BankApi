using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Repositories;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class DepositAccountCommandHandler : IRequestHandler<DepositAccountCommand, Account>
    {
        private readonly IAccountRepository _accountRepository;

        public DepositAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<Account> Handle(DepositAccountCommand request, CancellationToken cancellationToken)
        {
            return _accountRepository.Deposit(request.Destination, request.Amount);
        }
    }
}
