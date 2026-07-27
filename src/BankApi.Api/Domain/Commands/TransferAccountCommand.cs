using BankApi.Api.Domain.DTOs;
using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class TransferAccountCommand : IRequest<Result<AccountTransferDto>>
    {
        public long Destination { get; set; }
        public decimal Amount { get; set; }
        public long Origin { get; set; }
    }
}
