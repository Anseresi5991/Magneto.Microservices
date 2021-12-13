using RabbitMQ.Bus.Events;

namespace Magneto.Microservice.Orchestrator.Application.Dto
{
    public class DemoQueue:Event
    {
        public string[]? Dna { get; set; }
        public bool IsMutant { get; set; }
    }
}