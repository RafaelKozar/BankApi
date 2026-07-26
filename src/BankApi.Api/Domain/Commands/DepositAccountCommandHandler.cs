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

        public async Task<Account> Handle(DepositAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                Id = request.Destination,
                Balance = request.Amount
            };

            var result = await _accountRepository.Add(account);

            return result;
        }
    }
}
