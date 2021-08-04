using Stocks.Core.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Stocks.Infrastructure
{
    public class StocksDataDispatcher : IStockDataDispatcher
    {
        private readonly string _apiKey;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        public StocksDataDispatcher(string apiKey)
            => _apiKey = apiKey;

        public async Task<Dictionary<DateTime, double>> GetStockData(string symbol, int count)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=compact&apikey={_apiKey}");

            if (response.IsSuccessStatusCode)
            {
                return ParseStockDate(await response.Content.ReadAsStringAsync(), count);
            }
            return null;         
        }

        private Dictionary<DateTime, double> ParseStockDate(string json, int count)
        {
            var doc = JsonDocument.Parse(json);
            var result = new Dictionary<DateTime, double>();
            if (doc.RootElement.TryGetProperty("Time Series (Daily)", out JsonElement timeSeries))
            {
                var iterator = timeSeries.EnumerateObject();
                int counter = 0;

                while (++counter <= count && iterator.MoveNext())
                {
                    var key = iterator.Current.Name;
                    var date = DateTime.ParseExact(key, "yyyy-MM-dd", null);
                    var value = iterator.Current.Value.GetProperty("1. open").GetString();
                    result.Add(date, double.Parse(value));
                }
            }
            return result;
        }
    }
}
