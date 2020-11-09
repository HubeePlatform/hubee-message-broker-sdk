using Events.Payment;
using Hubee.MessageBroker.Sdk.Core.Handles;
using Hubee.MessageBroker.Sdk.Core.Models.Headers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SubscriberWorker.Handles
{
    public class NotificationHandle : GenericMessageHandle<PaymentAuthorizedEvent>
    {
        private readonly ILogger<NotificationHandle> _logger;

        public NotificationHandle(ILogger<NotificationHandle> logger)
        {
            _logger = logger;
        }

        public override async Task Handle(PaymentAuthorizedEvent message, EventHeader header)
        {
   
            await Task.Run(() =>
             _logger.LogInformation($"NotificationHandle: (PaymentAuthorizedEvent): EventId: {message.EventId} Status: {message.Status}"));
        }
    }
}