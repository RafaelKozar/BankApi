using BankApi.Api.Domain.DTOs;
using BankApi.Api.Domain.Repositories;
using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class TransferAccountCommandHandler : IRequestHandler<TransferAccountCommand, Result<AccountTransferDto>>
    {
        private readonly IAccountRepository repository;

        public TransferAccountCommandHandler(IAccountRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<AccountTransferDto>> Handle(TransferAccountCommand request, CancellationToken cancellationToken)
        {
            var transferResult = await repository.Transfer(request.Origin, request.Destination, request.Amount);
            if (transferResult == null)
            {
                return Result<AccountTransferDto>.Failure(Error.NotFound("Not Found count"));
            }
            else
            {
                return Result<AccountTransferDto>.Success(new AccountTransferDto
                {
                    Origin = new AccountDto
                    {
                        Id = transferResult[request.Origin].Id,
                        Balance = transferResult[request.Origin].Balance
                    },
                    Destination = new AccountDto
                    {
                        Id = transferResult[request.Destination].Id,
                        Balance = transferResult[request.Destination].Balance
                    }
                });
            }

        }
    }
}
