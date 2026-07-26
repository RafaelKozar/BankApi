using BankApi.Api.Domain.DTOs;
using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class DepositAccountCommand : IRequest<Result<AccountDestinationDto>>
    {
        public long Destination { get; set; }
        public decimal Amount { get; set; }
    }
}
