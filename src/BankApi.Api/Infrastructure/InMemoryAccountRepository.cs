using System.Collections.Concurrent;
using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Repositories;

namespace BankApi.Api.Infrastructure
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly ConcurrentDictionary<long, Account> _accounts = new();
        private readonly ConcurrentDictionary<long, SemaphoreSlim> _locks = new();
        
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

        public async Task<Dictionary<int, Account>?> Transfer(long origin, long destination, decimal amount)
        {
            var lesserId = Math.Min(origin, destination);
            var greaterId = Math.Max(origin, destination);
            var lockFirst = GetLock(lesserId);
            var lockSecond = GetLock(greaterId);
            await lockFirst.WaitAsync();
            await lockSecond.WaitAsync();
            try
            {
                if (!_accounts.TryGetValue(origin, out var existingOrigin))
                {
                    return null;
                }

                if (!_accounts.TryGetValue(destination, out var existingDestination))
                {
                    return null;
                }

                var updatedOrigin = new Account { Id = existingOrigin.Id, Balance = existingOrigin.Balance - amount };
                if (_accounts.TryUpdate(origin, updatedOrigin, existingOrigin))
                {
                    var updatedDestination = new Account { Id = existingDestination.Id, Balance = existingDestination.Balance + amount };
                    if (_accounts.TryUpdate(destination, updatedDestination, existingDestination))
                    {
                        return new Dictionary<int, Account>
                        {
                            { (int)origin, updatedOrigin },
                            { (int)destination, updatedDestination }
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                lockSecond.Release();
                lockFirst.Release();
            }
        }

        private SemaphoreSlim GetLock(long id) =>
                _locks.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));

        public Task Reset()
        {
            _accounts.Clear();
            _locks.Clear();
            return Task.CompletedTask;
        }
    }
}
