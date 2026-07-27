using BankApi.Api.Domain.Commands;
using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Repositories;
using BankApi.Api.Domain.Results;
using Moq;

namespace BankApi.Tests.Domain.Commands
{
    public class TransferAccountCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly TransferAccountCommandHandler _handler;

        public TransferAccountCommandHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new TransferAccountCommandHandler(_accountRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WithUpdatedOriginAndDestinationBalances_WhenBothAccountsExist()
        {
            // Arrange
            var command = new TransferAccountCommand { Origin = 1, Destination = 2, Amount = 30m };
            var transferResult = new Dictionary<long, Account>
            {
                { 1, new Account { Id = 1, Balance = 70m } },
                { 2, new Account { Id = 2, Balance = 130m } }
            };

            _accountRepositoryMock
                .Setup(r => r.Transfer(command.Origin, command.Destination, command.Amount))
                .ReturnsAsync(transferResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value!.Origin.Id);
            Assert.Equal(70m, result.Value!.Origin.Balance);
            Assert.Equal(2, result.Value!.Destination.Id);
            Assert.Equal(130m, result.Value!.Destination.Balance);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WithNotFoundError_WhenTransferReturnsNull()
        {
            // Arrange
            var command = new TransferAccountCommand { Origin = 1, Destination = 99, Amount = 30m };

            _accountRepositoryMock
                .Setup(r => r.Transfer(command.Origin, command.Destination, command.Amount))
                .ReturnsAsync((Dictionary<long, Account>?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorType.NotFound, result.Error!.Type);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryTransfer_WithOriginDestinationAndAmountFromCommand()
        {
            // Arrange
            var command = new TransferAccountCommand { Origin = 5, Destination = 8, Amount = 100m };

            _accountRepositoryMock
                .Setup(r => r.Transfer(command.Origin, command.Destination, command.Amount))
                .ReturnsAsync(new Dictionary<long, Account>
                {
                    { 5, new Account { Id = 5, Balance = 0m } },
                    { 8, new Account { Id = 8, Balance = 100m } }
                });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _accountRepositoryMock.Verify(r => r.Transfer(5, 8, 100m), Times.Once);
        }
    }
}
