using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using mySimpleMessageService.Domain.Message.Commands;
using mySimpleMessageService.Domain.Message.Queries;
using System.Threading.Tasks;

namespace mySimpleMessageService.API.Controllers
{

    /// <summary>
    /// Messages operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class MessagesController : ODataController
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all messages saved in system + possible odata features ( searching, sorting, paingation )
        /// </summary>
        /// <returns>
        /// List of messages
        /// </returns>
        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new ReadMessagesQuery()));
        }
        /// <summary>
        /// Create new contact in db
        /// </summary>
        /// <param name="command">
        /// Message - all params are required
        /// </param>
        /// <returns>
        /// OK
        /// </returns>
        [HttpPost]
        public async Task Post([FromBody] SendMessageCommand command)
        {
            await _mediator.Send(command);
        }

        /// <summary>
        /// Delete message
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <returns>
        /// OK
        /// </returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
           await _mediator.Send(new DeleteMessageCommand { Id = id });
        }

    }
}
