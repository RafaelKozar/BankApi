using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Query;
using BankApi.Api.Domain.Repositories;
using BankApi.Api.Domain.Results;
using Moq;

namespace BankApi.Tests.Domain.Query
{
    public class GetAccountQueryHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly GetAccountQueryHandler _handler;

        public GetAccountQueryHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _handler = new GetAccountQueryHandler(_accountRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WithAccountBalance_WhenAccountExists()
        {
            // Arrange
            var query = new GetAccountQuery { AccountId = 1 };

            _accountRepositoryMock
                .Setup(r => r.Get(query.AccountId))
                .ReturnsAsync(new Account { Id = 1, Balance = 250m });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(250m, result.Value);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WithNotFoundError_WhenAccountDoesNotExist()
        {
            // Arrange
            var query = new GetAccountQuery { AccountId = 99 };

            _accountRepositoryMock
                .Setup(r => r.Get(query.AccountId))
                .ReturnsAsync((Account?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorType.NotFound, result.Error!.Type);
            Assert.Contains("99", result.Error.Message);
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryGet_WithAccountIdFromQuery()
        {
            // Arrange
            var query = new GetAccountQuery { AccountId = 42 };

            _accountRepositoryMock
                .Setup(r => r.Get(query.AccountId))
                .ReturnsAsync(new Account { Id = 42, Balance = 0m });

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _accountRepositoryMock.Verify(r => r.Get(42), Times.Once);
        }
    }
}
