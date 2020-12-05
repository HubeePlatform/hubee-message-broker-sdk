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
    }
}