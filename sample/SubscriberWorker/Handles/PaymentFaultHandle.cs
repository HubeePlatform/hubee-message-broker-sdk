using Events.Order;
using Hubee.MessageBroker.Sdk.Core.Handles;
using Hubee.MessageBroker.Sdk.Core.Models.Headers;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SubscriberWorker.Handles
{
    public class PaymentFaultHandle : GenericMessageFaultHandle<OrderCreatedEvent>
    {
        private readonly ILogger<PaymentFaultHandle> _logger;
        public PaymentFaultHandle(ILogger<PaymentFaultHandle> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(OrderCreatedEvent message, EventHeader header)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            await Task.Run(() =>
                 _logger.LogInformation($"PaymentFaultHandle: (OrderCreatedEvent): Error {header.Fault.Exceptions.FirstOrDefault().Message}"));
        }
    }
}