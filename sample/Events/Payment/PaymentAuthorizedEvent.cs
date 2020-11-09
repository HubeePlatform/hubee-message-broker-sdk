using System;

namespace Events.Payment
{
    public interface PaymentAuthorizedEvent
    {
        public Guid EventId { get; }
        public string OrderId { get; }
        public Guid PaymentId { get; }
        public string Status { get; }
    }
}