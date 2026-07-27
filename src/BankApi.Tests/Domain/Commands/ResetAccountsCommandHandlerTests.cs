using BankApi.Api.Domain.Commands;
using BankApi.Api.Domain.Repositories;
using Moq;

namespace BankApi.Tests.Domain.Commands
{
    public class ResetAccountsCommandHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly ResetAccountsCommandHandler _handler;

        public ResetAccountsCommandHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new ResetAccountsCommandHandler(_accountRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryReset_Once()
        {
            // Arrange
            var command = new ResetAccountsCommand();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _accountRepositoryMock.Verify(r => r.Reset(), Times.Once);
        }
    }
}
