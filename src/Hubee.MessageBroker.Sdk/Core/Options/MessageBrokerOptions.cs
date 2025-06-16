using GreenPipes;
using Hubee.MessageBroker.Sdk.Core.Models.Constants;
using MassTransit;
using MassTransit.RabbitMqTransport;
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

        /// <summary>
        /// Subscribes a consumer to handle specific event messages with retry policies and queue configuration.
        /// </summary>
        /// <typeparam name="TEvent">The type of event message to consume</typeparam>
        /// <typeparam name="THandle">The type of consumer that will handle the event</typeparam>
        /// <param name="retryCount">Number of retry attempts for failed messages</param>
        /// <param name="interval">Time interval in seconds between retries</param>
        /// <param name="prefetchCount">Maximum number of unacknowledged messages that can be processed simultaneously</param>
        /// <param name="concurrentMessageLimit">Maximum number of messages that can be processed concurrently</param>
        /// <param name="priorityQueue">Priority queue level (recommended values between 1-10)</param>
        /// <returns>The current MessageBrokerOptions instance for method chaining</returns>
        public MessageBrokerOptions Subscribe<TEvent, THandle>(
            int retryCount = SubscribeDefault.RETRY_COUNT,
            int interval = SubscribeDefault.RETRY_INTERVAL,
            ushort? prefetchCount = null,
            int? concurrentMessageLimit = null,
            int? priorityQueue = null
            ) where THandle : class, IConsumer
        {
            var eventBus = _serviceProvider.GetService<IBusControl>();

            var eventHandler = eventBus.ConnectReceiveEndpoint(GenerateQueueName(typeof(TEvent).Name, typeof(THandle).Name), x =>
            {
                if (x is IRabbitMqReceiveEndpointConfigurator rabbitMqConfig)
                {
                    if (prefetchCount.HasValue)
                        rabbitMqConfig.PrefetchCount = prefetchCount.Value;

                    if (priorityQueue.HasValue)
                    {
                        if (priorityQueue < 1 || priorityQueue > 255)
                            throw new ArgumentOutOfRangeException(nameof(priorityQueue), "Priority must be between 1 and 255.");

                        rabbitMqConfig.SetQueueArgument("x-max-priority", priorityQueue);
                    }
                }

                x.Consumer<THandle>(_serviceProvider,
                    c =>
                    {
                        c.UseMessageRetry(r => { r.Interval(retryCount, TimeSpan.FromSeconds(interval)); });

                        if (concurrentMessageLimit.HasValue)
                            c.UseConcurrentMessageLimit(concurrentMessageLimit.Value);
                    }
                );
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