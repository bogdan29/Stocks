using Stocks.Domain;
using System.Threading.Tasks;

namespace Stocks.Core.Common
{
    public interface IStockDataRepository
    {
        Task<Stock> Get(string symbol);
        Task Update(Stock stock);
    }
}
