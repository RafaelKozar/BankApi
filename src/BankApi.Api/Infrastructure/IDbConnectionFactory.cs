using System.Data;

namespace BankApi.Api.Infrastructure
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
