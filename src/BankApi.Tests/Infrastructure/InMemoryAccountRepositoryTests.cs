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

        [Fact]
        public async Task Withdraw_ShouldDecreaseBalance_WhenAccountExists()
        {
            // Arrange
            await _repository.Add(new Account { Id = 1, Balance = 100m });

            // Act
            var account = await _repository.Withdraw(1, 30m);

            // Assert
            Assert.NotNull(account);
            Assert.Equal(70m, account!.Balance);
        }

        [Fact]
        public async Task Withdraw_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            // Act
            var account = await _repository.Withdraw(999, 30m);

            // Assert
            Assert.Null(account);
        }

        [Fact]
        public async Task Withdraw_ShouldPersistUpdatedBalance()
        {
            // Arrange
            await _repository.Add(new Account { Id = 1, Balance = 100m });
            await _repository.Withdraw(1, 30m);

            // Act
            var account = await _repository.Get(1);

            // Assert
            Assert.NotNull(account);
            Assert.Equal(70m, account!.Balance);
        }

        [Fact]
        public async Task Withdraw_ShouldBeThreadSafe_WhenCalledConcurrentlyForSameAccount()
        {
            // Arrange
            const int concurrentWithdrawals = 100;
            const decimal amount = 1m;
            await _repository.Add(new Account { Id = 1, Balance = 1000m });

            // Act
            var tasks = Enumerable.Range(0, concurrentWithdrawals)
                .Select(_ => _repository.Withdraw(1, amount));
            await Task.WhenAll(tasks);

            // Assert
            var account = await _repository.Get(1);
            Assert.NotNull(account);
            Assert.Equal(1000m - (concurrentWithdrawals * amount), account!.Balance);
        }

        [Fact]
        public async Task Transfer_ShouldMoveAmount_BetweenTwoExistingAccounts()
        {
            // Arrange
            await _repository.Add(new Account { Id = 1, Balance = 100m });
            await _repository.Add(new Account { Id = 2, Balance = 50m });

            // Act
            var result = await _repository.Transfer(1, 2, 30m);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(70m, result![1].Balance);
            Assert.Equal(80m, result[2].Balance);
        }

        [Fact]
        public async Task Transfer_ShouldReturnNull_WhenOriginDoesNotExist()
        {
            // Arrange
            await _repository.Add(new Account { Id = 2, Balance = 50m });

            // Act
            var result = await _repository.Transfer(999, 2, 30m);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Transfer_ShouldReturnNull_WhenDestinationDoesNotExist()
        {
            // Arrange
            await _repository.Add(new Account { Id = 1, Balance = 100m });

            // Act
            var result = await _repository.Transfer(1, 999, 30m);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Transfer_ShouldPersistUpdatedBalances_ForBothAccounts()
        {
            // Arrange
            await _repository.Add(new Account { Id = 1, Balance = 100m });
            await _repository.Add(new Account { Id = 2, Balance = 50m });
            await _repository.Transfer(1, 2, 30m);

            // Act
            var origin = await _repository.Get(1);
            var destination = await _repository.Get(2);

            // Assert
            Assert.Equal(70m, origin!.Balance);
            Assert.Equal(80m, destination!.Balance);
        }

        [Fact]
        public async Task Transfer_ShouldNotChangeBalances_WhenOriginDoesNotExist()
        {
            // Arrange
            await _repository.Add(new Account { Id = 2, Balance = 50m });

            // Act
            await _repository.Transfer(999, 2, 30m);
            var destination = await _repository.Get(2);

            // Assert
            Assert.Equal(50m, destination!.Balance);
        }

        [Fact]
        public async Task Transfer_ShouldBeThreadSafe_WhenCalledConcurrentlyInOppositeDirections()
        {
            // Arrange
            const int concurrentTransfers = 100;
            const decimal amount = 1m;
            await _repository.Add(new Account { Id = 1, Balance = 1000m });
            await _repository.Add(new Account { Id = 2, Balance = 1000m });

            // Act
            var tasksAtoB = Enumerable.Range(0, concurrentTransfers)
                .Select(_ => _repository.Transfer(1, 2, amount));
            var tasksBtoA = Enumerable.Range(0, concurrentTransfers)
                .Select(_ => _repository.Transfer(2, 1, amount));
            await Task.WhenAll(tasksAtoB.Concat(tasksBtoA));

            // Assert
            var accountOne = await _repository.Get(1);
            var accountTwo = await _repository.Get(2);
            Assert.Equal(1000m, accountOne!.Balance);
            Assert.Equal(1000m, accountTwo!.Balance);
        }
    }
}
