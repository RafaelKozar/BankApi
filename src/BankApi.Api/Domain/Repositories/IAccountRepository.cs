using BankApi.Api.Domain.Models;

namespace BankApi.Api.Domain.Repositories
{
    public interface IAccountRepository
    {
        public Task<Account> Add(Account account);

        public Task<Account> Get(long id);
    }
}
