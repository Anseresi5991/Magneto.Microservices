using Magneto.Microservice.Orchestrator.Application.Dto;
using Magneto.Microservice.Orchestrator.Domain.Entity;
using Newtonsoft.Json;

namespace Magneto.Microservice.Orchestrator.Application.Map
{
    public class Map
    {
        public static Mutant MapToMutant(DemoQueue mutantDto)
        {
            Mutant mutant = new Mutant();
            mutant.Dna = mutantDto.Dna;
            return mutant;
        }
        public static Human MapToHuman(DemoQueue humantDto)
        {
            Human humant = new Human();
            humant.Dna = humantDto.Dna;
            return humant;
        }
    }
}