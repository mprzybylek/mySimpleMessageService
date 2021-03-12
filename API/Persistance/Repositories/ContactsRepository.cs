using Persistance;
using Persistance.Entities;
using Persistance.Repositories;

namespace mySimpleMessageService.Persistance.Repositories
{
    public class ContactsRepository : GenericRepository<ContactEntity>
    {
        public ContactsRepository(MessageServiceContext context) : base(context) { }

    }
}
