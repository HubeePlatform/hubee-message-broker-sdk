namespace Hubee.MessageBroker.Sdk.Core.Models.Headers
{
    public class InnerException : ExceptionInfoHeader
    {
        public InnerException(string exceptionType, InnerException innerException, string stackTrace, string message, string source) : base(exceptionType, innerException, stackTrace, message, source)
        {
        }
    }
}