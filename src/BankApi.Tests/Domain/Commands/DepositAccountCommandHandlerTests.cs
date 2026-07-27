using BankApi.Api.Domain.Commands;
using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Repositories;
using Moq;

namespace BankApi.Tests.Domain.Commands
{
    public class DepositAccountCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly DepositAccountCommandHandler _handler;

        public DepositAccountCommandHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new DepositAccountCommandHandler(_accountRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WithUpdatedAccountBalance()
        {
            // Arrange
            var command = new DepositAccountCommand { Destination = 1, Amount = 50m };
            var updatedAccount = new Account { Id = 1, Balance = 150m };

            _accountRepositoryMock
                .Setup(r => r.Deposit(command.Destination, command.Amount))
                .ReturnsAsync(updatedAccount);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(updatedAccount.Id, result.Value!.Destination.Id);
            Assert.Equal(updatedAccount.Balance, result.Value!.Destination.Balance);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryDeposit_WithDestinationAndAmountFromCommand()
        {
            // Arrange
            var command = new DepositAccountCommand { Destination = 7, Amount = 200m };

            _accountRepositoryMock
                .Setup(r => r.Deposit(command.Destination, command.Amount))
                .ReturnsAsync(new Account { Id = 7, Balance = 200m });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _accountRepositoryMock.Verify(r => r.Deposit(7, 200m), Times.Once);
        }
    }
}
