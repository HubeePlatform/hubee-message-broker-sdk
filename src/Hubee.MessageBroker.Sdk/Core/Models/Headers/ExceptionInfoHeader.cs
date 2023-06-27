using MassTransit;
using System;

namespace Hubee.MessageBroker.Sdk.Core.Models.Headers
{
    public class ExceptionInfoHeader : Exception
    {
        public ExceptionInfoHeader(string exceptionType, InnerException innerException, string stackTrace, string message, string source)
        {
            ExceptionType = exceptionType;
            InnerException = innerException;
            StackTrace = stackTrace;
            Message = message;
            Source = source;
        }

        public ExceptionInfoHeader(string message, string stackTrace)
        {
            Message = message;
            StackTrace = stackTrace;
        }

        public string ExceptionType { get; set; }
        public new InnerException InnerException { get; set; }
        public new string StackTrace { get; set; }
        public new string Message { get; set; }
        public new string Source { get; set; }

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