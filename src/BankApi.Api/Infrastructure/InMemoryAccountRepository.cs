using System.Collections.Concurrent;
using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Repositories;

namespace BankApi.Api.Infrastructure
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly ConcurrentDictionary<long, Account> _accounts = new();

        public Task<Account> Add(Account account)
        {
            _accounts[account.Id] = account;
            return Task.FromResult(account);
        }

        public Task<Account?> Get(long id)
        {
            _accounts.TryGetValue(id, out var account);
            return Task.FromResult(account);
        }

        public Task<Account> Deposit(long id, decimal amount)
        {
            var account = _accounts.AddOrUpdate(
                id,
                addValueFactory: _ => new Account { Id = id, Balance = amount },
                updateValueFactory: (_, existing) => new Account { Id = existing.Id, Balance = existing.Balance + amount });

            return Task.FromResult(account);
        }

        public Task<Account?> Withdraw(long id, decimal amount)
        {
            while (true)
            {
                if (!_accounts.TryGetValue(id, out var existing))
                {
                    return Task.FromResult<Account?>(null);
                }

                var updated = new Account { Id = existing.Id, Balance = existing.Balance - amount };

                if (_accounts.TryUpdate(id, updated, existing))
                {
                    return Task.FromResult<Account?>(updated);
                }
            }
        }
    }
}
