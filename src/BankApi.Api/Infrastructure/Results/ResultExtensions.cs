using BankApi.Api.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Infrastructure.Results
{
    public static class ResultExtensions
    {
        public static ActionResult<T> ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }

            var statusCode = MapStatusCode(result.Error!.Type);

            return new ObjectResult(new ProblemDetails
            {
                Status = statusCode,
                Title = result.Error.Type.ToString(),
                Detail = result.Error.Message
            })
            {
                StatusCode = statusCode
            };
        }

        private static int MapStatusCode(ErrorType type) => type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,          
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
