using System;

namespace Domain.Events
{
    public class Event
    {
        public Guid Id { get; set; }

        public string EventName { get; set; }

        public bool IsDeleted { get; set; }

        public EventType EventType { get; set; }
    }
}