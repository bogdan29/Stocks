using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Stocks.Domain
{
    public class Stock
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string Symbol { get; set; }
        public Dictionary<string, double> Values { get; set; }

        public Stock()
        {
            _id = ObjectId.GenerateNewId();
        }
    }
}
