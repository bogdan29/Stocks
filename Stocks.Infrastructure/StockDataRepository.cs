using MongoDB.Driver;
using Stocks.Core.Common;
using Stocks.Domain;
using System;
using System.Threading.Tasks;

namespace Stocks.Infrastructure
{
    public class StockDataRepository : IStockDataRepository
    {
        private readonly MongoDbContext _mongoContext;
        private readonly IMongoCollection<Stock> _dbCollection;

        public StockDataRepository(MongoDbContext context)
        {
            _mongoContext = context;
            _dbCollection = _mongoContext.GetCollection<Stock>("Stocks");
        }

        public Task Update(Stock stock)
        {
            var filter = Builders<Stock>.Filter.Where(x => x.Symbol == stock.Symbol);
            return _dbCollection.ReplaceOneAsync(filter, stock, new ReplaceOptions { IsUpsert = true });
        }

        public async Task<Stock> Get(string symbol)
        {
            var filter = Builders<Stock>.Filter.Where(x => x.Symbol == symbol);
            var result = await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
            return result;
        }
    }
}