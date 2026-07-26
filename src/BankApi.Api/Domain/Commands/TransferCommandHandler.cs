using BankApi.Api.Domain.DTOs;
using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class TransferCommandHandler : IRequestHandler<TransferCommand, Result<AccountTransferDto>>
    {
        private readonly IMediator repository;

        public TransferCommandHandler(IMediator mediator)
        {
            repository = mediator;
        }

        public Task<Result<AccountTransferDto>> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var transferResult = 
        }
    }
}
