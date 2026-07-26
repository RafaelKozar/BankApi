using BankApi.Api.Domain.Commands;
using BankApi.Api.Domain.Models;
using BankApi.Api.Domain.Query;
using BankApi.Api.Infrastructure.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<Account>> GetAccount([FromQuery] GetAccountQuery query)
        {
            var result = await mediator.Send(query);
            return result.ToActionResult();
        }

    }
}
