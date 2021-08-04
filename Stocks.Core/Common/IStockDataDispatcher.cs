using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Core.Common
{
    public interface IStockDataDispatcher
    {
        Task<Dictionary<DateTime, double>> GetStockData(string symbol, int count);
    }
}
