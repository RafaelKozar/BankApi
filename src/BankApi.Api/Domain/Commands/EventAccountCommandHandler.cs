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
                    var depositResult = await mediator.Send(new DepositAccountCommand { Destination = request.Destination!.Value, Amount = request.Amount }, cancellationToken);
                    return depositResult.IsSuccess ? Result<object>.Success(depositResult.Value!) : Result<object>.Failure(depositResult.Error!);

                case "withdraw":
                    var withdrawResult = await mediator.Send(new WithdrawAccountCommand { Origin = request.Origin!.Value, Amount = request.Amount }, cancellationToken);
                    return withdrawResult.IsSuccess ? Result<object>.Success(withdrawResult.Value!) : Result<object>.Failure(withdrawResult.Error!);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
