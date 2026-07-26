using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class EventAcountCommand : IRequest<Result<object>>
    {
        public string Type { get; set; } = string.Empty;
        public long? Origin { get; set; }
        public long Amount { get; set; } = 0;
        public long? Destination { get; set; }


    }
}
