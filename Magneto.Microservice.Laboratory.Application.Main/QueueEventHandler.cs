using Magneto.Microservice.Laboratory.Application.Dto;
using Magneto.Microservice.Laboratory.Domain.Entity;
using Magneto.Microservice.Laboratory.Infrastructure.Interfaces;
using RabbitMQ.Bus.RabbitBus;
using System.Threading.Tasks;

namespace Magneto.Microservice.Laboratory.Application.Main
{
    public class QueueEventHandler : IEventHandler<DemoQueue>
    {
        private readonly IContextMutant _contextMutant;
        public QueueEventHandler(IContextMutant contextMutant)
        {
            _contextMutant = contextMutant;
        }
        public Task Handle(DemoQueue @event)
        {
            if (@event.IsMutant)
            {
                Mutant mutant = Map.Map.MapToMutant(@event);
                InsertMutant(mutant);
            }
            else
            {
                Human human = Map.Map.MapToHuman(@event);
                InsertHuman(human);
            }
            return Task.CompletedTask;
        }
        public void InsertMutant(Mutant mutant)
        {
            _contextMutant.InsertMutant(mutant);
        }
        public void InsertHuman(Human human)
        {
            _contextMutant.InsertHuman(human);
        }
    }
}