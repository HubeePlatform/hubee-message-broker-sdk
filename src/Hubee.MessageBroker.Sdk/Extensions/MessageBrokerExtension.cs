using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hubee.MessageBroker.Sdk.Core.Models;
using Hubee.MessageBroker.Sdk.Interfaces;
using Hubee.MessageBroker.Sdk.Services;
using Hubee.MessageBroker.Sdk.Core.Options;
using MassTransit;
using System;
using Hubee.MessageBroker.Sdk.Core.Factories;

namespace Hubee.MessageBroker.Sdk.Extensions
{
    public static class MessageBrokerExtension
    {
        private static readonly ConfigurationFactory _configurationFactory = new ConfigurationFactory();

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration, Action<MessageBrokerOptions> setupAction = null)
        {
            var config = new HubeeMessageBrokerConfig();
            configuration.GetSection("HubeeMessageBrokerConfig").Bind(config);

            if (config.IsInvalid())
                throw new InvalidOperationException($"Please, configure appsettings with a {nameof(HubeeMessageBrokerConfig)} section");

            if (setupAction != null)
            {
                var options = new MessageBrokerOptions(services);
                setupAction.Invoke(options);
            }

            _configurationFactory.GetByConfig(config).Configure(services, config);

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IEventBusService, EventBusService>();

            return services;
        }

        public static IApplicationBuilder UseEventBus(this IApplicationBuilder app, Action<MessageBrokerOptions> setupAction)
        {
            var options = new MessageBrokerOptions(app.ApplicationServices);
            setupAction.Invoke(options);

            return app;
        }
    }
}