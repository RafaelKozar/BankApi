using BankApi.Api.Domain.Repositories;
using MediatR;

namespace BankApi.Api.Domain.Query
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, decimal>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<decimal> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.Get(request.AccountId);
            return account?.Balance ?? 0m;
        }
    }
}
