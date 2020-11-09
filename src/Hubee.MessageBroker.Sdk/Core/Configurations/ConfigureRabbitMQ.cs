using Hubee.MessageBroker.Sdk.Core.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Hubee.MessageBroker.Sdk.Core.Configurations
{
    internal class ConfigureRabbitMQ : IConfigureMessageBroker
    {
        public void Configure(IServiceCollection services, HubeeMessageBrokerConfig config)
        {
            services.AddMassTransit(x => x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
             {
                 cfg.UseHealthCheck(context);
                 cfg.Host(config.HostName, config.VirtualHost,
                      h =>
                      {
                          h.Username(config.UserName);
                          h.Password(config.Password);
                      });

                 cfg.ExchangeType = ExchangeType.Fanout;
             })));
        }
    }
}