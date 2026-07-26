namespace BankApi.Api.Domain.Models    
{
    public class Account
    {
        public long Id { get; set; }
        public decimal Balance { get; set; } = 0;
    }
}
