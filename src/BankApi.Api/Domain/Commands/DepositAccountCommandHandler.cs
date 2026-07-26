using BankApi.Api.Domain.DTOs;
using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Repositories;
using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class DepositAccountCommandHandler : IRequestHandler<DepositAccountCommand, Result<AccountDestinationDto>>
    {
        private readonly IAccountRepository _accountRepository;

        public DepositAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result<AccountDestinationDto>> Handle(DepositAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.Deposit(request.Destination, request.Amount);
            return Result<AccountDestinationDto>.Success(new AccountDestinationDto
            {
                Destination = new AccountDto
                {
                    Id = account.Id,
                    Balance = account.Balance
                }
            });
        }
    }
}
