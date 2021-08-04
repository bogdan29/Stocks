using MediatR;
using Stocks.Core.Common;
using Stocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stocks.Core.Features.StockQuery
{
    internal class StockQueryHandler : IRequestHandler<StockQuery, Dictionary<string, double>>
    {
        private readonly IStockDataDispatcher _stockQueryDispatcher;
        private readonly IStockDataRepository _stockDataRepository;
        private readonly string _dateFormat = "yyyy-MM-dd";

        public StockQueryHandler(IStockDataDispatcher stockQueryDispatcher, IStockDataRepository stockDataRepository) 
            => (_stockQueryDispatcher, _stockDataRepository) = (stockQueryDispatcher, stockDataRepository);

        public async Task<Dictionary<string, double>> Handle(StockQuery request, CancellationToken cancellationToken)
        {
            var stockData = await _stockQueryDispatcher.GetStockData(request.Symbol, 7);
            if (stockData.Values.Count > 0)
            {
                await SaveToDb(request.Symbol, stockData);
                var spyStockData = await _stockQueryDispatcher.GetStockData("SPY", 7);

                return ComparePerformance(stockData, spyStockData);
            }

            return null;
        }

        private async Task SaveToDb(string symbol, Dictionary<DateTime, double> data)
        {
            var existingStock = await _stockDataRepository.Get(symbol.ToUpper());
            if (existingStock is not null)
            {
                foreach(var key in data.Keys)
                {
                    var stringKey = key.ToString(_dateFormat);
                    if (existingStock.Values.ContainsKey(stringKey))
                    {
                        existingStock.Values[stringKey] = data[key];
                    }
                    else
                    {
                        existingStock.Values.Add(stringKey, data[key]);
                    }
                }
                await _stockDataRepository.Update(existingStock);
            }
            else
            {
                var newStock = new Stock { Symbol = symbol.ToUpper(), Values = data.ToDictionary(p => p.Key.ToString(_dateFormat), p => p.Value) };
                await _stockDataRepository.Update(newStock);
            }

        }

        private Dictionary<string, double> ComparePerformance(Dictionary<DateTime, double> stockData, Dictionary<DateTime, double> spyStockData)
        {
            var result = new Dictionary<string, double>();
            foreach (var key in spyStockData.Keys)
            {
                double stockValue = stockData.ContainsKey(key) ? stockData[key] : 0;
                result.Add(key.ToString(_dateFormat), stockValue - spyStockData[key]);
            }
            return result;
        }
    }
}
