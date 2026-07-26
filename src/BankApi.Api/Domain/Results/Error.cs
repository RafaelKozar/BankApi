namespace BankApi.Api.Domain.Results
{
    public sealed record Error
    {
        public ErrorType Type { get; }

        public string Message { get; }

        private Error(ErrorType type, string message)
        {
            Type = type;
            Message = message;
        }

        public static Error NotFound(string message) => new(ErrorType.NotFound, message);

    }
}
