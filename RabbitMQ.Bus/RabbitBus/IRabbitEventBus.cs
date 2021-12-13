using RabbitMQ.Bus.Commands;
using RabbitMQ.Bus.Events;
using System.Threading.Tasks;

namespace RabbitMQ.Bus.RabbitBus
{
    public interface IRabbitEventBus
    {
        Task SendCommands<T>(T command) where T : Command;
        void Subscribe<T, TH>() where T : Event
                                where TH : IEventHandler<T>;
    }
}
