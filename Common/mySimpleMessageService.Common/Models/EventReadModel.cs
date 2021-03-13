using System;

namespace mySimpleMessageService.Common.Models
{
    public class EventReadModel
    {
        public int Id { get; set; }
        public string Event { get; set; }
        public DateTime Date { get; set; }
    }
}
