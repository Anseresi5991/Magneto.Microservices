using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magneto.Microservice.Laboratory.Domain.Entity
{
    [BsonIgnoreExtraElements]
    public class Human
    {
        public string pk { get; set; }
        public string[]? Dna { get; set; }
    }
}
