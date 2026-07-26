using MediatR;
using System.Text.Json.Serialization;

namespace BankApi.Api.Domain.Query
{
    public class GetAccountQuery : IRequest<decimal>
    {
        [JsonPropertyName("account_id")]
        public long AccountId { get; set; }
    }
}
