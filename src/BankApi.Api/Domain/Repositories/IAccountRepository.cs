using BankApi.Api.Domain.Models;

namespace BankApi.Api.Domain.Repositories
{
    public interface IAccountRepository
    {
        public Task<Account> Add(Account account);

        public Task<Account?> Get(long id);

        public Task<Account> Deposit(long id, decimal amount);

        public Task<Account?> Withdraw(long id, decimal amount);

        public Task<Dictionary<int, Account>?> Transfer(long origin, long destination, decimal amount);

        public Task Reset();
    }
}
