using mySimpleMessageService.Persistance.Entities;
using Persistance;
using Persistance.Repositories;

namespace mySimpleMessageService.Persistance.Repositories
{
    public class EventsRepository : GenericRepository<EventsEntity>
    {
        public EventsRepository(MessageServiceContext context) : base(context) { }

    }
}
