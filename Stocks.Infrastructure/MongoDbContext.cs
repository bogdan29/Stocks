using MongoDB.Driver;

namespace Stocks.Infrastructure
{
    public class MongoDbContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }

        public MongoDbContext(string connection, string databaseName)
        {
            _mongoClient = new MongoClient(connection);
            _db = _mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
