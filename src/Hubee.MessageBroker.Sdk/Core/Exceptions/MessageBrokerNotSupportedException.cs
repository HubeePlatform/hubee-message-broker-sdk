using System;

namespace Hubee.MessageBroker.Sdk.Core.Exceptions
{
    public class MessageBrokerNotSupportedException : Exception
    {
        public MessageBrokerNotSupportedException(string typeName) : base($"Message Broker {typeName} not supported")
        {

        }
    }
}