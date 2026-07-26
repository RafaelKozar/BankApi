using System.Data;
using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Repositories;
using Dapper;

namespace BankApi.Api.Infrastructure
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _connection;

        public AccountRepository(IDbConnectionFactory factory) => _connection = factory.CreateConnection();

        public async Task<Account> Add(Account account)
        {
            await _connection.ExecuteAsync(
                "INSERT INTO Contas (Id, BALANCE) VALUES (@Id, @Balance)",
                account);

            return account;
        }

        public Task<Account> Get(long id)
        {
            return _connection.QuerySingleOrDefaultAsync<Account>(
                "SELECT Id, BALANCE as Balance FROM Contas WHERE Id = @Id",
                new { Id = id });
        }
    }
}
