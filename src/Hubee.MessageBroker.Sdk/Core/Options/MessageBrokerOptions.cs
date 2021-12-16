using GreenPipes;
using Hubee.MessageBroker.Sdk.Core.Models.Constants;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hubee.MessageBroker.Sdk.Core.Options
{
    public class MessageBrokerOptions
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly IServiceProvider _serviceProvider;

        public MessageBrokerOptions(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public MessageBrokerOptions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public MessageBrokerOptions AddConsumer<T>() where T : class, IConsumer
        {
            _serviceCollection.AddMassTransit(c =>
                {
                    c.AddConsumer<T>();
                });

            return this;
        }

        public MessageBrokerOptions Subscribe<TEvent, THandle>(int retryCount = SubscribeDefault.RETRY_COUNT, int interval = SubscribeDefault.RETRY_INTERVAL) where THandle : class, IConsumer
        {
            var eventBus = _serviceProvider.GetService<IBusControl>();

            var eventHandler = eventBus.ConnectReceiveEndpoint(GenerateQueueName(typeof(TEvent).Name, typeof(THandle).Name), x =>
            {
                x.Consumer<THandle>(_serviceProvider, c => c.UseMessageRetry(r =>
                {
                    r.Interval(retryCount, TimeSpan.FromSeconds(interval));
                }));
            });

            eventHandler.Ready.Wait();

            return this;
        }

        public MessageBrokerOptions SubscribeFault<TEvent, THandle>() where THandle : class, IConsumer
        {
            var eventBus = _serviceProvider.GetService<IBusControl>();

            var eventHandler = eventBus.ConnectReceiveEndpoint(GenerateQueueName(typeof(TEvent).Name, typeof(THandle).Name), x =>
            {
                x.Consumer<THandle>(_serviceProvider);
            });

            eventHandler.Ready.Wait();
            return this;
        }

        private string GenerateQueueName(string eventName, string handleName)
        {
            var configuration = _serviceProvider.GetService<IConfiguration>();

            var applicationName = configuration["HubeeMessageBrokerConfig:ApplicationName"];
            var queueName = $"{applicationName}.{RemoveGenericsStringFromHandleName(handleName)}.{eventName}";

            return queueName;
        }

        private static string RemoveGenericsStringFromHandleName(string handleName)
        {
            if (handleName.IndexOf('`').Equals(-1))
                return handleName;

            return handleName.Replace(handleName.Substring(handleName.IndexOf('`')), string.Empty);
        }
    }
}