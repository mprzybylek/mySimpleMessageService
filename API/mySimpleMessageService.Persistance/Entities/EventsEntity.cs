using Persistance.Entities;
using System;

namespace mySimpleMessageService.Persistance.Entities
{
    public class EventsEntity : IEntity
    {
        public int Id { set; get; }
        public string Event { get; set; }
        public DateTime Date { get; set; }
    }
}
