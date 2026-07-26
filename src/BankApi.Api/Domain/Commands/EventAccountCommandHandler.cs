using BankApi.Api.Domain.Results;
using MediatR;

namespace BankApi.Api.Domain.Commands
{
    public class EventAccountCommandHandler : IRequestHandler<EventAcountCommand, Result<object>>
    {
        private readonly IMediator mediator;

        public EventAccountCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Result<object>> Handle(EventAcountCommand request, CancellationToken cancellationToken)
        {
            return request.Type switch
            {
                "deposit" => await mediator.Send(new DepositAccountCommand { Destination = request.Destination!.Value, Amount = request.Amount }, cancellationToken),
                _ => throw new NotImplementedException()
            };
        }
    }
}
