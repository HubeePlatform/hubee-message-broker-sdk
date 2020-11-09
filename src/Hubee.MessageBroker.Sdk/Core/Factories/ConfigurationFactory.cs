using Hubee.MessageBroker.Sdk.Core.Configurations;
using Hubee.MessageBroker.Sdk.Core.Exceptions;
using Hubee.MessageBroker.Sdk.Core.Models;

namespace Hubee.MessageBroker.Sdk.Core.Factories
{
    internal class ConfigurationFactory
    {
        public IConfigureMessageBroker GetByConfig(HubeeMessageBrokerConfig config)
        {
            return config.MessageBrokerTypeEnum switch
            {
                MessageBrokerType.InMemory => new ConfigureInMemory(),
                MessageBrokerType.RabbitMQ => new ConfigureRabbitMQ(),
                _ => throw new MessageBrokerNotSupportedException(config.MessageBroker)
            };
        }
    }
}