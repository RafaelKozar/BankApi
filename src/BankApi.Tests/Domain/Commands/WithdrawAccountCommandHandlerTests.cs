using BankApi.Api.Domain.Commands;
using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Repositories;
using BankApi.Api.Domain.Results;
using Moq;

namespace BankApi.Tests.Domain.Commands
{
    public class WithdrawAccountCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly WithdrawAccountCommandHandler _handler;

        public WithdrawAccountCommandHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new WithdrawAccountCommandHandler(_accountRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WithUpdatedAccountBalance_WhenAccountExists()
        {
            // Arrange
            var command = new WithdrawAccountCommand { Origin = 1, Amount = 30m };
            var updatedAccount = new Account { Id = 1, Balance = 70m };

            _accountRepositoryMock
                .Setup(r => r.Withdraw(command.Origin, command.Amount))
                .ReturnsAsync(updatedAccount);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(updatedAccount.Id, result.Value!.Origin.Id);
            Assert.Equal(updatedAccount.Balance, result.Value!.Origin.Balance);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WithNotFoundError_WhenAccountDoesNotExist()
        {
            // Arrange
            var command = new WithdrawAccountCommand { Origin = 99, Amount = 30m };

            _accountRepositoryMock
                .Setup(r => r.Withdraw(command.Origin, command.Amount))
                .ReturnsAsync((Account?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorType.NotFound, result.Error!.Type);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryWithdraw_WithOriginAndAmountFromCommand()
        {
            // Arrange
            var command = new WithdrawAccountCommand { Origin = 7, Amount = 200m };

            _accountRepositoryMock
                .Setup(r => r.Withdraw(command.Origin, command.Amount))
                .ReturnsAsync(new Account { Id = 7, Balance = 0m });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _accountRepositoryMock.Verify(r => r.Withdraw(7, 200m), Times.Once);
        }
    }
}
