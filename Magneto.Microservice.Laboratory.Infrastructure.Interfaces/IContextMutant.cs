using Magneto.Microservice.Laboratory.Domain.Entity;

namespace Magneto.Microservice.Laboratory.Infrastructure.Interfaces
{
    public interface IContextMutant
    {
        public void InsertMutant(Mutant mutant);
        public void InsertHuman(Human mutant);
    }
}