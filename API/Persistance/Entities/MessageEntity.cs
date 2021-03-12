using mySimpleMessageService.Common.Models;

namespace Persistance.Entities
{
    public class MessageEntity : IEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public MessageType MessageType { get; set; }
        public int? ContactSentId { get; set; }
        public virtual ContactEntity ContactSent { get; set; }
        public int? ContactReceivedId { get; set; }
        public virtual ContactEntity ContactReceived { get; set; }
    }
}
