namespace BankApi.Api.Domain.Results
{
    
    public sealed class Result<T>
    {
        private Result(T? value, Error? error)
        {
            Value = value;
            Error = error;
        }

        public T? Value { get; }

        public Error? Error { get; }

        public bool IsSuccess => Error is null;

        public bool IsFailure => !IsSuccess;

        public static Result<T> Success(T value) => new(value, error: null);

        public static Result<T> Failure(Error error) => new(value: default, error);

        public static implicit operator Result<T>(T value) => Success(value);

        public static implicit operator Result<T>(Error error) => Failure(error);
    }
}
