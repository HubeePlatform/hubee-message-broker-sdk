using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hubee.MessageBroker.Sdk.Core.Models.Headers
{
    public class FaultHeader
    {
        public FaultHeader(Guid faultId, Guid? faultedMessageId, DateTime timestamp, string[] faultMessageTypes, IEnumerable<ExceptionInfoHeader> exceptions)
        {
            FaultId = faultId;
            FaultedMessageId = faultedMessageId;
            Timestamp = timestamp;
            FaultMessageTypes = faultMessageTypes;
            Exceptions = exceptions;
        }

        public Guid FaultId { get; set; }
        public Guid? FaultedMessageId { get; set; }
        public DateTime Timestamp { get; set; }
        public IEnumerable<ExceptionInfoHeader> Exceptions { get; set; }
        public string[] FaultMessageTypes { get; }

        public static FaultHeader Generate<TCustomEvent>(Fault<TCustomEvent> context) where TCustomEvent : class
        {
            return new FaultHeader(
                context.FaultId,
                context?.FaultedMessageId,
                context.Timestamp,
                context?.FaultMessageTypes,
                ExtractException(context.Exceptions)
                );
        }

        private static IEnumerable<ExceptionInfoHeader> ExtractException(ExceptionInfo[] exceptions)
        {
            var list = new List<ExceptionInfoHeader>();

            exceptions.ToList().ForEach(x => list.Add(ExceptionInfoHeader.Generate(x)));
            return list;
        }
    }
}