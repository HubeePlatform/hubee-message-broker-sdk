using Hubee.MessageBroker.Sdk.Core.Helpers;

namespace Hubee.MessageBroker.Sdk.Core.Models
{
    public class HubeeMessageBrokerConfig
    {
        public string MessageBroker { get; set; }
        public string Protocol { get; set; }
        public ProtocolType ProtocolEnum => EnumHelper.Parse<ProtocolType>(this.Protocol);
        public MessageBrokerType MessageBrokerTypeEnum => EnumHelper.Parse<MessageBrokerType>(this.MessageBroker);
        public string ApplicationName { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; set; } = "/";
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsInvalid()
        {
            return !IsInMemoryValid() &&
                (IsRabbitMQInvalid() ||
                 string.IsNullOrEmpty(ApplicationName) ||
                 string.IsNullOrEmpty(HostName) ||
                 string.IsNullOrEmpty(UserName) ||
                 string.IsNullOrEmpty(Password)
                );
        }

        private bool IsRabbitMQInvalid()
        {
            if (this.MessageBrokerTypeEnum != MessageBrokerType.RabbitMQ)
                return false;

            return this.ProtocolEnum switch
            {
                ProtocolType.Ssl => string.IsNullOrEmpty(VirtualHost) || string.IsNullOrEmpty(Port),
                _ => false,
            };
        }

        private bool IsInMemoryValid()
        {
            return MessageBrokerTypeEnum.Equals(MessageBrokerType.InMemory) && !string.IsNullOrEmpty(ApplicationName);
        }
    }
}