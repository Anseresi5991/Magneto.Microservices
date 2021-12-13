using Magneto.Microservice.Orchestrator.Domain.Entity;
using Magneto.Microservice.Orchestrator.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Magneto.Microservice.Orchestrator.Infrastructure.Data
{
    public class ContextMutant : IContextMutant
    {
        private readonly IMongoClient _mongoClient;
        private readonly IConfiguration _configuration;
        private const string CONSTRING = "ConDb";
        public ContextMutant(IConfiguration configuration)
        {
            _configuration = configuration;
            _mongoClient = new MongoClient(_configuration.GetConnectionString(CONSTRING));

        }
        public async void InsertMutant(Mutant mutant)
        {
            var database = _mongoClient.GetDatabase("Magneto");
            var collection = database.GetCollection<Mutant>("Mutant");
            await collection.InsertOneAsync(mutant);
        }
        public async void InsertHuman(Human human)
        {
            var database = _mongoClient.GetDatabase("Magneto");
            var collection = database.GetCollection<Human>("Human");
            await collection.InsertOneAsync(human);
        }
    }
}