using RabbitMQ.Bus.Events;
using System.Threading.Tasks;

namespace RabbitMQ.Bus.RabbitBus
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        Task Handle(TEvent @event);
    }
    public interface IEventHandler
    {

    }
}
