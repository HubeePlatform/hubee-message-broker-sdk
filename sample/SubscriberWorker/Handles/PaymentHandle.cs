using Events.Order;
using Events.Payment;
using Hubee.MessageBroker.Sdk.Core.Handles;
using Hubee.MessageBroker.Sdk.Core.Models.Headers;
using Hubee.MessageBroker.Sdk.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SubscriberWorker.Handles
{
    public class PaymentHandle : GenericMessageHandle<OrderCreatedEvent>
    {
        private readonly ILogger<PaymentHandle> _logger;
        private readonly IEventBusService _eventBusService;

        public PaymentHandle(ILogger<PaymentHandle> logger, IEventBusService eventBusService)
        {
            _logger = logger;
            _eventBusService = eventBusService;
        }

        public override async Task Handle(OrderCreatedEvent message, EventHeader header) => await Task.Run(() => ProcessPayments(message));

        private void ProcessPayments(OrderCreatedEvent message)
        {
            _logger.LogInformation($"PaymentHandle: (OrderCreatedEvent): EventId: {message.EventId} Status: {message.Status} Description {message.Description}");

            _eventBusService.Publish<PaymentAuthorizedEvent>(new
            {
                EventId = Guid.NewGuid(),
                OrderId = int.Parse(message.OrderId).ToString(),
                PaymentId = Guid.NewGuid(),
                Status = "PAYMENT_AUTHORIZED"
            });
        }
    }
}