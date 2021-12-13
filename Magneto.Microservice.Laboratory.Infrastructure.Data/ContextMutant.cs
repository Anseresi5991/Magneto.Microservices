using Magneto.Microservice.Laboratory.Domain.Entity;
using Magneto.Microservice.Laboratory.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Security.Authentication;

namespace Magneto.Microservice.Laboratory.Infrastructure.Data
{
    public class ContextMutant : IContextMutant
    {
        private readonly IMongoClient _mongoClient;
        private readonly IConfiguration _configuration;
        private const string CONSTRING = "ConDb";
        public ContextMutant(IConfiguration configuration)
        {
            _configuration = configuration;

            MongoClientSettings settings = MongoClientSettings.FromUrl(
            new MongoUrl(@_configuration.GetConnectionString(CONSTRING))
            );
            settings.SslSettings =
            new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            _mongoClient = new MongoClient(settings);
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