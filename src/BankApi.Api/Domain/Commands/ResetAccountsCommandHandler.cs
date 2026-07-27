using BankApi.Api.Domain.Repositories;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class ResetAccountsCommandHandler : IRequestHandler<ResetAccountsCommand>
    {
        private readonly IAccountRepository _accountRepository;

        public ResetAccountsCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Handle(ResetAccountsCommand request, CancellationToken cancellationToken)
        {
            await _accountRepository.Reset();
        }
    }
}
