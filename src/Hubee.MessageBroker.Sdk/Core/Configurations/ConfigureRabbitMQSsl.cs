using Hubee.MessageBroker.Sdk.Core.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Security.Authentication;

namespace Hubee.MessageBroker.Sdk.Core.Configurations
{
    internal class ConfigureRabbitMQSsl : IConfigureMessageBroker
    {
        public void Configure(IServiceCollection services, HubeeMessageBrokerConfig config)
        {
            services.AddMassTransit(x => x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.UseHealthCheck(context);
                cfg.Host(config.HostName, config.Port, config.VirtualHost,
                     h =>
                     {
                         h.Username(config.UserName);
                         h.Password(config.Password);

                         h.UseSsl(s =>
                         {
                             s.Protocol = SslProtocols.Tls12;
                         });
                     });

                cfg.ExchangeType = ExchangeType.Fanout;
            })));
        }
    }
}