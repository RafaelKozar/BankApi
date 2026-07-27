namespace BankApi.Api.Domain.Models    
{
    public class Account
    {
        public long Id { get; set; }
        public decimal Balance { get; set; } = 0;

        public Account Deposit(decimal amount) => new Account { Id = Id, Balance = Balance + amount };

        public Account Withdraw(decimal amount) => new Account { Id = Id, Balance = Balance - amount };
    }
}
