using Hubee.MessageBroker.Sdk.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hubee.MessageBroker.Sdk.Services
{
    internal class EventBusService : IEventBusService
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly ILogger<EventBusService> _logger;

        public EventBusService(IPublishEndpoint endpoint, ILogger<EventBusService> logger)
        {
            _endpoint = endpoint;
            _logger = logger;
        }

        public async Task Publish<T>(object message, CancellationToken cancellationToken) where T : class
        {
            try
            {
                await _endpoint.Publish<T>(message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public async Task Publish<T>(object message) where T : class
        {
            try
            {
                await _endpoint.Publish<T>(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public async Task Publish<T>(object message, Guid? messageId, Guid? correlationId) where T : class
        {
            try
            {
                await _endpoint.Publish<T>(message, context =>
                {
                    if (messageId.HasValue)
                        context.MessageId = messageId;

                    if (correlationId.HasValue)
                        context.CorrelationId = correlationId;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public async Task Publish<T>(object message, Guid? messageId, Guid? correlationId, CancellationToken cancellationToken) where T : class
        {
            try
            {
                await _endpoint.Publish<T>(message, context =>
                {
                    if (messageId.HasValue)
                        context.MessageId = messageId;

                    if (correlationId.HasValue)
                        context.CorrelationId = correlationId;
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public async Task Publish(object message, Type type)
        {
            try
            {
                await _endpoint.Publish(message, type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public async Task Publish(object message, Type type, Guid? messageId, Guid? correlationId)
        {
            try
            {
                await _endpoint.Publish(message, type, context =>
                {
                    if (messageId.HasValue)
                        context.MessageId = messageId;

                    if (correlationId.HasValue)
                        context.CorrelationId = correlationId;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public async Task Publish(object message, Type type, Guid? messageId, Guid? correlationId, CancellationToken cancellationToken)
        {
            try
            {
                await _endpoint.Publish(message, type, context =>
                {
                    if (messageId.HasValue)
                        context.MessageId = messageId;

                    if (correlationId.HasValue)
                        context.CorrelationId = correlationId;
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
}