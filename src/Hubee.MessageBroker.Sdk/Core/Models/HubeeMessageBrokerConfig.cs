using Hubee.MessageBroker.Sdk.Core.Helpers;

namespace Hubee.MessageBroker.Sdk.Core.Models
{
    public class HubeeMessageBrokerConfig
    {
        public string MessageBroker { get; set; }
        public MessageBrokerType MessageBrokerTypeEnum => EnumHelper.Parse<MessageBrokerType>(this.MessageBroker);
        public string ApplicationName { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; private set; } = "/";
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsInvalid()
        {
            if (MessageBrokerTypeEnum.Equals(MessageBrokerType.InMemory) && !string.IsNullOrEmpty(ApplicationName))
                return false;

            return
               string.IsNullOrEmpty(ApplicationName) ||
               string.IsNullOrEmpty(HostName) ||
               string.IsNullOrEmpty(UserName) ||
               string.IsNullOrEmpty(Password);
        }
    }
}