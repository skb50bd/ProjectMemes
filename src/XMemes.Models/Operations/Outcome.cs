using System;

namespace XMemes.Models.Operations
{
    public enum OutcomeType
    {
        Success,
        Error
    }

    public class Outcome<T> where T: class
    {
        public static Outcome<T> FromSuccess(T? value, string? msg = null) =>
            new Outcome<T>
            {
                Result = OutcomeType.Success,
                Message = msg,
                Value = value
            };

        public static Outcome<T> FromError(string? msg, Exception? exp = null) =>
            new Outcome<T>
            {
                Result = OutcomeType.Error,
                Message = msg,
                Exception = exp
            };

        public static Outcome<T> FromError<TU>(Outcome<TU> errorOutcome) where TU : class =>
            new Outcome<T>
            {
                Result = OutcomeType.Error,
                Message = errorOutcome.Message,
                Exception = errorOutcome.Exception
            };

        public OutcomeType Result { get; set; }
        public bool IsSuccess => Result == OutcomeType.Success;
        public bool IsError => Result == OutcomeType.Error;
        public string? Message { get; set; }
        public Exception? Exception { get; set; }
        public T? Value { get; set; }
    }
}