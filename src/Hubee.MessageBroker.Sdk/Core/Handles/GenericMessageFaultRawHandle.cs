using Hubee.MessageBroker.Sdk.Core.Models.Headers;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hubee.MessageBroker.Sdk.Core.Handles
{
    public abstract class GenericMessageFaultRawHandle<TCustomEvent> : IConsumer<TCustomEvent> where TCustomEvent : class
    {
        public async Task Consume(ConsumeContext<TCustomEvent> context)
        {
            await Handle(context.Message, EventHeader.Generate(context).AddFault(context));
        }

        public abstract Task Handle(TCustomEvent message, EventHeader header);
    }
}
