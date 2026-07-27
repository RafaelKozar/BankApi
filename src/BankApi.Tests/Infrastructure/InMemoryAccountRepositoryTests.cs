using BankApi.Api.Domain.Models;
using BankApi.Api.Infrastructure;

namespace BankApi.Tests.Infrastructure
{
    public class InMemoryAccountRepositoryTests
    {
        private readonly InMemoryAccountRepository _repository;

        public InMemoryAccountRepositoryTests()
        {
            _repository = new InMemoryAccountRepository();
        }

        [Fact]
        public async Task Deposit_ShouldCreateAccount_WhenAccountDoesNotExist()
        {
            // Act
            var account = await _repository.Deposit(1, 100m);

            // Assert
            Assert.Equal(1, account.Id);
            Assert.Equal(100m, account.Balance);
        }

        [Fact]
        public async Task Deposit_ShouldIncreaseBalance_WhenAccountAlreadyExists()
        {
            // Arrange
            await _repository.Add(new Account { Id = 1, Balance = 100m });

            // Act
            var account = await _repository.Deposit(1, 50m);

            // Assert
            Assert.Equal(150m, account.Balance);
        }

        [Fact]
        public async Task Deposit_ShouldPersistUpdatedBalance()
        {
            // Arrange
            await _repository.Add(new Account { Id = 1, Balance = 100m });
            await _repository.Deposit(1, 50m);

            // Act
            var account = await _repository.Get(1);

            // Assert
            Assert.NotNull(account);
            Assert.Equal(150m, account!.Balance);
        }

        [Fact]
        public async Task Deposit_ShouldBeThreadSafe_WhenCalledConcurrentlyForSameAccount()
        {
            // Arrange
            const int concurrentDeposits = 100;
            const decimal amount = 10m;

            // Act
            var tasks = Enumerable.Range(0, concurrentDeposits)
                .Select(_ => _repository.Deposit(1, amount));
            await Task.WhenAll(tasks);

            // Assert
            var account = await _repository.Get(1);
            Assert.NotNull(account);
            Assert.Equal(concurrentDeposits * amount, account!.Balance);
        }

        [Fact]
        public async Task Get_ShouldReturnAccount_WhenAccountExists()
        {
            // Arrange
            await _repository.Add(new Account { Id = 1, Balance = 100m });

            // Act
            var account = await _repository.Get(1);

            // Assert
            Assert.NotNull(account);
            Assert.Equal(1, account!.Id);
            Assert.Equal(100m, account.Balance);
        }

        [Fact]
        public async Task Get_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            // Act
            var account = await _repository.Get(999);

            // Assert
            Assert.Null(account);
        }
    }
}
