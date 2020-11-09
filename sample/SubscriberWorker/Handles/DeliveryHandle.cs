using Events.Payment;
using Hubee.MessageBroker.Sdk.Core.Handles;
using Hubee.MessageBroker.Sdk.Core.Models.Headers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SubscriberWorker.Handles
{
    public class DeliveryHandle : GenericMessageHandle<PaymentAuthorizedEvent>
    {
        private readonly ILogger<DeliveryHandle> _logger;

        public DeliveryHandle(ILogger<DeliveryHandle> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(PaymentAuthorizedEvent message, EventHeader header)
        {
            await Task.Run(() =>
                _logger.LogInformation($"DeliveryHandle: (PaymentAuthorizedEvent): EventId: {message.EventId} Status: {message.Status}"));
        }
    }
}
