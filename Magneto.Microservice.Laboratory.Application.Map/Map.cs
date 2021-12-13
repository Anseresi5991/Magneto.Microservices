using Magneto.Microservice.Laboratory.Application.Dto;
using Magneto.Microservice.Laboratory.Domain.Entity;
using Newtonsoft.Json;

namespace Magneto.Microservice.Laboratory.Application.Map
{
    public class Map
    {
        public static Mutant MapToMutant(DemoQueue mutantDto)
        {
            System.Guid guid = System.Guid.NewGuid();
            Mutant mutant = new Mutant();
            mutant.Dna = mutantDto.Dna;
            mutant.pk = guid.ToString();
            return mutant;
        }
        public static Human MapToHuman(DemoQueue humantDto)
        {
            System.Guid guid = System.Guid.NewGuid();
            Human humant = new Human();
            humant.Dna = humantDto.Dna;
            humant.pk = guid.ToString();
            return humant;
        }
    }
}