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
            switch (request.Type)
            {
                case "deposit":
                    return await mediator.Send(new DepositAccountCommand { Destination = request.Destination!.Value, Amount = request.Amount }, cancellationToken);
                case "withdraw":
                    var result = await mediator.Send(new WithdrawAccountCommand { Origin = request.Origin!.Value, Amount = request.Amount }, cancellationToken);
                    return result.IsSuccess ? Result<object>.Success(result.Value!) : Result<object>.Failure(result.Error!);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
