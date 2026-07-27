using BankApi.Api.Domain.Models;

namespace BankApi.Tests.Domain.Models
{
    public class AccountTests
    {
        [Fact]
        public void Deposit_ShouldIncreaseBalance_ByAmount()
        {
            // Arrange
            var account = new Account { Id = 1, Balance = 100m };

            // Act
            var result = account.Deposit(50m);

            // Assert
            Assert.Equal(150m, result.Balance);
        }

        [Fact]
        public void Deposit_ShouldKeepSameId()
        {
            // Arrange
            var account = new Account { Id = 42, Balance = 0m };

            // Act
            var result = account.Deposit(10m);

            // Assert
            Assert.Equal(42, result.Id);
        }

        [Fact]
        public void Deposit_ShouldNotMutateOriginalInstance()
        {
            // Arrange
            var account = new Account { Id = 1, Balance = 100m };

            // Act
            var result = account.Deposit(50m);

            // Assert
            Assert.Equal(100m, account.Balance);
            Assert.NotSame(account, result);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 100, 100)]
        [InlineData(100, 0, 100)]
        [InlineData(10.5, 4.25, 14.75)]
        public void Deposit_ShouldCalculateBalance_ForVariousAmounts(decimal initialBalance, decimal depositAmount, decimal expectedBalance)
        {
            // Arrange
            var account = new Account { Id = 1, Balance = initialBalance };

            // Act
            var result = account.Deposit(depositAmount);

            // Assert
            Assert.Equal(expectedBalance, result.Balance);
        }

        [Fact]
        public void Withdraw_ShouldDecreaseBalance_ByAmount()
        {
            // Arrange
            var account = new Account { Id = 1, Balance = 100m };

            // Act
            var result = account.Withdraw(30m);

            // Assert
            Assert.Equal(70m, result.Balance);
        }

        [Fact]
        public void Withdraw_ShouldKeepSameId()
        {
            // Arrange
            var account = new Account { Id = 42, Balance = 100m };

            // Act
            var result = account.Withdraw(10m);

            // Assert
            Assert.Equal(42, result.Id);
        }

        [Fact]
        public void Withdraw_ShouldNotMutateOriginalInstance()
        {
            // Arrange
            var account = new Account { Id = 1, Balance = 100m };

            // Act
            var result = account.Withdraw(30m);

            // Assert
            Assert.Equal(100m, account.Balance);
            Assert.NotSame(account, result);
        }

        [Theory]
        [InlineData(100, 0, 100)]
        [InlineData(100, 100, 0)]
        [InlineData(10.5, 4.25, 6.25)]
        [InlineData(50, 100, -50)]
        public void Withdraw_ShouldCalculateBalance_ForVariousAmounts(decimal initialBalance, decimal withdrawAmount, decimal expectedBalance)
        {
            // Arrange
            var account = new Account { Id = 1, Balance = initialBalance };

            // Act
            var result = account.Withdraw(withdrawAmount);

            // Assert
            Assert.Equal(expectedBalance, result.Balance);
        }
    }
}
