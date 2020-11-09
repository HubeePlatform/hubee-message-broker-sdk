using Hubee.MessageBroker.Sdk.Core.Models.Headers;
using MassTransit;
using System.Threading.Tasks;

namespace Hubee.MessageBroker.Sdk.Core.Handles
{
    public abstract class GenericMessageFaultHandle<TCustomEvent> : IConsumer<Fault<TCustomEvent>> where TCustomEvent : class
    {
        public async Task Consume(ConsumeContext<Fault<TCustomEvent>> context)
        {
            await Handle(context.Message.Message, EventHeader.Generate(context).AddFault(context.Message));
        }

        public abstract Task Handle(TCustomEvent message, EventHeader header);
    }
}