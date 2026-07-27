using BankApi.Api.Domain.DTOs;
using BankApi.Api.Domain.Repositories;
using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class TransferCommandHandler : IRequestHandler<TransferCommand, Result<AccountTransferDto>>
    {
        private readonly IAccountRepository repository;

        public TransferCommandHandler(IAccountRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<AccountTransferDto>> Handle(TransferCommand request, CancellationToken cancellationToken)
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
                        Id = transferResult[(int)request.Origin].Id,
                        Balance = transferResult[(int)request.Origin].Balance
                    },
                    Destination = new AccountDto
                    {
                        Id = transferResult[(int)request.Destination].Id,
                        Balance = transferResult[(int)request.Destination].Balance
                    }
                });
            }

        }
    }
}
