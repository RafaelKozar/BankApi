namespace BankApi.Api.Domain.DTOs
{
    public class AccountTransferDto
    {
        public AccountOriginDto Origin { get; set; } = new AccountOriginDto();
        public AccountDestinationDto Destination { get; set; } = new AccountDestinationDto();        
    }
}
