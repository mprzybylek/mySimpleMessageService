using MediatR;
using mySimpleMessageService.Common.Models;
using System.Linq;

namespace mySimpleMessageService.Domain.Message.Queries
{
    public class ReadMessagesQuery : IRequest<IQueryable<MessageReadModel>>
    {
    }
}
