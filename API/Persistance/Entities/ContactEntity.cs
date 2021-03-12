using System.Collections.Generic;

namespace Persistance.Entities
{
    public class ContactEntity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<MessageEntity> MessagesSent { get; set; }
        public virtual ICollection<MessageEntity> MessagesReceived { get; set; }
    }
}
