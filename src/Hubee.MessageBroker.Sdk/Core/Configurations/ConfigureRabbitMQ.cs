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

                 if (string.IsNullOrEmpty(config.Port))
                     config.Port = "5672";

                 cfg.Host(config.HostName, ushort.Parse(config.Port), config.VirtualHost,
                      h =>
                      {
                          h.Username(config.UserName);
                          h.Password(config.Password);
                          h.Heartbeat(config.Heartbeat);
                      });

                 cfg.ExchangeType = ExchangeType.Fanout;
             })));
        }
    }
}