using BankApi.Api.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Infrastructure.Results
{
    public static class ResultExtensions
    {
        public static ActionResult<T> ToActionResult<T>(this Result<T> result, int successStatusCode = StatusCodes.Status200OK)
        {
            if (result.IsSuccess)
            {
                return new ObjectResult(result.Value) { StatusCode = successStatusCode };
            }

            var statusCode = MapStatusCode(result.Error!.Type);

            return new StatusCodeResult(statusCode);
        }

        private static int MapStatusCode(ErrorType type) => type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
