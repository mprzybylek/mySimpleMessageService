using Persistance;
using Persistance.Entities;
using Persistance.Repositories;

namespace mySimpleMessageService.Persistance.Repositories
{
    public class MessagesRepository : GenericRepository<MessageEntity>
    {
        public MessagesRepository(MessageServiceContext context) : base(context) { }
    }
}
