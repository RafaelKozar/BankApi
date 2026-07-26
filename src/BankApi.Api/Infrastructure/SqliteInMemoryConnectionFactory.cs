using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace BankApi.Api.Infrastructure
{
    public class SqliteInMemoryConnectionFactory : IDbConnectionFactory, IDisposable
    {
        public SqliteConnection Connection { get; }

        public SqliteInMemoryConnectionFactory()
        {
            Connection = new SqliteConnection("Data Source=:memory:");
            Connection.Open();

            Connection.Execute(@"
            CREATE TABLE Contas (
                Id TEXT PRIMARY KEY,
                BALANCE DECIMAL NOT NULL
            );");
        }

        public IDbConnection CreateConnection() => Connection;

        public void Dispose() => Connection.Dispose();
    }
}
