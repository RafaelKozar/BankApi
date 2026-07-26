using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace BankApi.Api.Domain.Query
{
    public class GetAccountQuery : IRequest<Result<decimal>>
    {
        [JsonPropertyName("account_id")]
        public long AccountId { get; set; }
    }
}
