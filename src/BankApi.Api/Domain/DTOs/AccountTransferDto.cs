namespace BankApi.Api.Domain.DTOs
{
    public class AccountTransferDto
    {
        public AccountDto Origin { get; set; } = new AccountDto();
        public AccountDto Destination { get; set; } = new AccountDto();        
    }
}
