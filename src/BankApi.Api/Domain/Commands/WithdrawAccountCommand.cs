using BankApi.Api.Domain.DTOs;
using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class WithdrawAccountCommand : IRequest<Result<AccountOriginDto>>
    {
        public long Origin { get; set; }
        public decimal Amount { get; set; }
    }
}
