using BankApi.Api.Domain.DTOs;
using BankApi.Api.Domain.Models;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class DepositAccountCommand : IRequest<AccountDestinationDto>
    {
        public long Destination { get; set; }
        public decimal Amount { get; set; }
    }
}
