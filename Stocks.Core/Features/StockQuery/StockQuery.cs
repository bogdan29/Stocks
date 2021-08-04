using MediatR;
using System;
using System.Collections.Generic;

namespace Stocks.Core.Features.StockQuery
{
    public class StockQuery : IRequest<Dictionary<string, double>>
    {
        public string Symbol { get; set; }
    }
}
