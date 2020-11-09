using MassTransit;
using System;
using System.Collections.Generic;

namespace Hubee.MessageBroker.Sdk.Core.Models.Headers
{
    public class EventHeader
    {
        public EventHeader(Guid? messageId, string correlationId, Guid? requestId, Guid? initiatorId, Guid? conversationId, Uri sourceAddress, Uri destinationAddress, Uri responseAddress, Uri faultAddress, DateTime? expirationTime, DateTime? sentTime, IEnumerable<KeyValuePair<string, object>> custom)
        {
            MessageId = messageId;
            CorrelationId = correlationId;
            RequestId = requestId;
            InitiatorId = initiatorId;
            ConversationId = conversationId;
            SourceAddress = sourceAddress;
            DestinationAddress = destinationAddress;
            ResponseAddress = responseAddress;
            FaultAddress = faultAddress;
            ExpirationTime = expirationTime;
            SentTime = sentTime;
            CustomHeader = custom;
        }

        public Guid? MessageId { get; set; }
        public string CorrelationId { get; set; }
        public Guid? RequestId { get; set; }
        public Guid? InitiatorId { get; set; }
        public Guid? ConversationId { get; set; }
        public Uri SourceAddress { get; set; }
        public Uri DestinationAddress { get; set; }
        public Uri ResponseAddress { get; set; }
        public Uri FaultAddress { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public DateTime? SentTime { get; set; }
        public IEnumerable<KeyValuePair<string, object>> CustomHeader { get; set; }
        public FaultHeader Fault { get; set; }

        public static EventHeader Generate<TCustomEvent>(ConsumeContext<TCustomEvent> context) where TCustomEvent : class
        {
            return new EventHeader(
                context?.MessageId,
                context?.CorrelationId.ToString(),
                context?.RequestId,
                context?.InitiatorId,
                context?.ConversationId,
                context?.SourceAddress,
                context?.DestinationAddress,
                context?.ResponseAddress,
                context?.FaultAddress,
                context?.ExpirationTime,
                context?.SentTime,
                context?.Headers?.GetAll());
        }
        public EventHeader AddFault<TCustomEvent>(Fault<TCustomEvent> context) where TCustomEvent : class
        {
            this.Fault = FaultHeader.Generate(context);
            return this;
        }
    }
}