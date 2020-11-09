using MassTransit;

namespace Hubee.MessageBroker.Sdk.Core.Models.Headers
{
    public class ExceptionInfoHeader
    {
        public ExceptionInfoHeader(string exceptionType, InnerException innerException, string stackTrace, string message, string source)
        {
            ExceptionType = exceptionType;
            InnerException = innerException;
            StackTrace = stackTrace;
            Message = message;
            Source = source;
        }

        public string ExceptionType { get; set; }
        public InnerException InnerException { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }

        public static ExceptionInfoHeader Generate(ExceptionInfo exception)
        {
            var innerException = exception?.InnerException;

            return new ExceptionInfoHeader(
                exception?.ExceptionType,
                new InnerException(innerException?.ExceptionType, null, innerException?.StackTrace, innerException?.Message, innerException?.Source),
                exception?.StackTrace,
                exception?.Message,
                exception?.Source
                );
        }
    }
}