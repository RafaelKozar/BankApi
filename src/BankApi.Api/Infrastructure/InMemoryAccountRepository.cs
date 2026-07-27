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

        public async Task<Account> Deposit(long id, decimal amount)
        {
            var @lock = GetLock(id);
            await @lock.WaitAsync();
            try
            {
                return _accounts.AddOrUpdate(
                    id,
                    addValueFactory: _ => new Account { Id = id, Balance = amount },
                    updateValueFactory: (_, existing) => existing.Deposit(amount));
            }
            finally
            {
                @lock.Release();
            }
        }

        public async Task<Account?> Withdraw(long id, decimal amount)
        {
            var @lock = GetLock(id);
            await @lock.WaitAsync();
            try
            {
                if (!_accounts.TryGetValue(id, out var existing))
                {
                    return null;
                }

                var updated = existing.Withdraw(amount);
                _accounts.TryUpdate(id, updated, existing);
                return updated;
            }
            finally
            {
                @lock.Release();
            }
        }

        public async Task<Dictionary<long, Account>?> Transfer(long origin, long destination, decimal amount)
        {
            var lesserId = Math.Min(origin, destination);
            var greaterId = Math.Max(origin, destination);
            var @lockFirst = GetLock(lesserId);
            var @lockSecond = GetLock(greaterId);
            await @lockFirst.WaitAsync();
            await @lockSecond.WaitAsync();
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

                var updatedOrigin = existingOrigin.Withdraw(amount);
                _accounts.TryUpdate(origin, updatedOrigin, existingOrigin);

                var updatedDestination = existingDestination.Deposit(amount);
                _accounts.TryUpdate(destination, updatedDestination, existingDestination);

                return new Dictionary<long, Account>
                {
                    { origin, updatedOrigin },
                    { destination, updatedDestination }
                };
            }
            finally
            {
                @lockSecond.Release();
                @lockFirst.Release();
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
