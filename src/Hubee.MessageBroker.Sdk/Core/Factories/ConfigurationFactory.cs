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
                MessageBrokerType.RabbitMQ => GetRabbitmqByProtocol(config.ProtocolEnum),
                _ => throw new MessageBrokerNotSupportedException(config.MessageBroker)
            };

        }

        private IConfigureMessageBroker GetRabbitmqByProtocol(ProtocolType protocolType)
        {
            return protocolType switch
            {
                ProtocolType.Ssl => new ConfigureRabbitMQSsl(),
                _ => new ConfigureRabbitMQ()
            };
        }
    }
}