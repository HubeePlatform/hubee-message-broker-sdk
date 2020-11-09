using System;

namespace Events.Order
{
    public interface OrderCreatedEvent
    {
        public Guid EventId { get; }
        public string OrderId { get; }
        public string Description { get; }
        public string Status { get; }
    }
}