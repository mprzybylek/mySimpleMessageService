using MediatR;
using mySimpleMessageService.Common.Models;
using System.Linq;

namespace mySimpleMessageService.Domain.Eventbus
{
    public class ReadEventsQuery : IRequest<IQueryable<EventReadModel>>
    {
    }
}
