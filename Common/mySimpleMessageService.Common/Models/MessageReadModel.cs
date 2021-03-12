namespace mySimpleMessageService.Common.Models
{
    public class MessageReadModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public MessageType MessageType { get; set; }

    }
}
