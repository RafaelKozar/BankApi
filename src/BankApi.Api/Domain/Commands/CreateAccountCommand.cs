using BankApi.Api.Domain.Models;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class CreateAccountCommand : IRequest<Account>
    {
        public long Destination { get; set; } = 0;
        public decimal Amount { get; set; }
    }
}
