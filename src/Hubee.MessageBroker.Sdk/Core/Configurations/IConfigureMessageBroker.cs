using Hubee.MessageBroker.Sdk.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Hubee.MessageBroker.Sdk.Core.Configurations
{
    internal interface IConfigureMessageBroker
    {
        public void Configure(IServiceCollection services, HubeeMessageBrokerConfig config);
    }
}