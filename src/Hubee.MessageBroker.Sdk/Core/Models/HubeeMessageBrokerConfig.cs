using Hubee.MessageBroker.Sdk.Core.Helpers;
using System;

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
            TryGetConfigFromEnvironment();

            return !IsInMemoryValid() &&
                (IsRabbitMQInvalid() ||
                 string.IsNullOrEmpty(ApplicationName) ||
                 string.IsNullOrEmpty(HostName) ||
                 string.IsNullOrEmpty(UserName) ||
                 string.IsNullOrEmpty(Password)
                );
        }

        private void TryGetConfigFromEnvironment()
        {
            var host = Environment.GetEnvironmentVariable("MESSAGEBROKER_HOSTNAME");
            this.HostName = host ?? this.HostName;

            var port = Environment.GetEnvironmentVariable("MESSAGEBROKER_PORT");
            this.Port = port ?? this.Port;

            var username = Environment.GetEnvironmentVariable("MESSAGEBROKER_USERNAME");
            this.UserName = username ?? this.UserName;

            var password = Environment.GetEnvironmentVariable("MESSAGEBROKER_PASSWORD");
            this.Password = password ?? this.Password;
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