using BankApi.Api.Domain.DTOs;
using BankApi.Api.Domain.Repositories;
using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class WithdrawAccountCommandHandler : IRequestHandler<WithdrawAccountCommand, Result<AccountOriginDto>>
    {
        private readonly IAccountRepository _accountRepository;

        public WithdrawAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result<AccountOriginDto>> Handle(WithdrawAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.Withdraw(request.Origin, request.Amount);

            if (account is null)
            {
                return Result<AccountOriginDto>.Failure(Error.NotFound($"Account '{request.Origin}' was not found."));
            }

            return Result<AccountOriginDto>.Success(new AccountOriginDto
            {
                Origin = new AccountDto { Id = account.Id, Balance = account.Balance }
            });
        }
    }
}
