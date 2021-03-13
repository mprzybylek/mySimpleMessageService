using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using mySimpleMessageService.Domain.Eventbus;
using System.Threading.Tasks;

namespace mySimpleMessageService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class EventBus : ODataController
    {
        private readonly IMediator _mediator;

        public EventBus(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get system events
        /// </summary>
        /// <returns>
        /// List of events
        /// </returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new ReadEventsQuery()));
        }
    }
}
