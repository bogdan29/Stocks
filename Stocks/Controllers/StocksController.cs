using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stocks.Core.Common;
using Stocks.Core.Features.StockQuery;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StocksController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet("{symbol}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<string, double>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> ComparePerformance([FromRoute]string symbol)
        {
            var result = await _mediator.Send(new StockQuery { Symbol = symbol });
            if (result is null)
            {
                return NotFound(symbol);
            }
            return Ok(result);
        }
    }
}
