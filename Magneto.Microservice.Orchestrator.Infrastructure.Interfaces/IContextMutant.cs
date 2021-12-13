using Magneto.Microservice.Orchestrator.Domain.Entity;

namespace Magneto.Microservice.Orchestrator.Infrastructure.Interfaces
{
    public interface IContextMutant
    {
        public void InsertMutant(Mutant mutant);
        public void InsertHuman(Human mutant);
    }
}