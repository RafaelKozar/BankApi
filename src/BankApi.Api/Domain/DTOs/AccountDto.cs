namespace BankApi.Api.Domain.DTOs
{
    public class AccountDto
    {
        public long Id { get; set; }
        public decimal Balance { get; set; } = 0;
    }
}
