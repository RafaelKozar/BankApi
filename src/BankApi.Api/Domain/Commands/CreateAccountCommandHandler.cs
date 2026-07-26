using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Repositories;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Account>
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
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
