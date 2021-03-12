using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using mySimpleMessageService.Domain.Contact.Commands;
using mySimpleMessageService.Domain.Contact.Queries;
using System.Threading.Tasks;

namespace mySimpleMessageService.API.Controllers
{
    /// <summary>
    /// Contact operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class ContactsController : ODataController
    {

        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all contacts saved in system + possible odata features ( searching, sorting, paingation )
        /// </summary>
        /// <returns>
        /// List of contacts
        /// </returns>
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult>  Get()
        {
            return Ok(await _mediator.Send(new ReadContactsQuery()));
        }

        /// <summary>
        /// Create new contact in db
        /// </summary>
        /// <param name="command">
        /// Contact - all params are required
        /// </param>
        /// <returns>
        /// OK
        /// </returns>
        [HttpPost]
        public async Task Post([FromBody] CreateContactCommand command)
        {
            await _mediator.Send(command);
        }
        /// <summary>
        /// Update/create contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <param name="command">
        /// Contact - all params are required
        /// </param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] UpdateContactCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <returns>
        /// OK
        /// </returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _mediator.Send(new DeleteContactCommand { Id = id });
        }
    }
}
