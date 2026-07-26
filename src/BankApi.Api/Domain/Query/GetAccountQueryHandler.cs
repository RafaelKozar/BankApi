using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Repositories;
using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Query
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, Result<decimal>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result<decimal>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {   
            var account = await _accountRepository.Get(request.AccountId);
            if (account is null)
            {
                return Result<decimal>.Failure(Error.NotFound($"Account '{request.AccountId}' was not found."));
            }

            return  Result<decimal>.Success(account.Balance);
        }
    }
}
