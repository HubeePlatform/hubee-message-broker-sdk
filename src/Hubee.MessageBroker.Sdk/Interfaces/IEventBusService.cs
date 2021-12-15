using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hubee.MessageBroker.Sdk.Interfaces
{
    public interface IEventBusService
    {
        Task Publish<T>(object message) where T : class;
        Task Publish<T>(object message, Type t) where T : class;
        Task Publish<T>(object message, CancellationToken cancellationToken) where T : class;
    }
}