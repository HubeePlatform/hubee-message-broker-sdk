using Hubee.MessageBroker.Sdk.Core.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Hubee.MessageBroker.Sdk.Core.Configurations
{
    internal class ConfigureInMemory : IConfigureMessageBroker
    {
        public void Configure(IServiceCollection services, HubeeMessageBrokerConfig config)
        {
            services.AddMassTransit(x =>
            {
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.TransportConcurrencyLimit = 100;

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}