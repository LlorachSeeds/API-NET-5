using System;
using System.Collections.Generic;

namespace Domain.Events
{
    public class EventType
    {
        public Guid Id { get; set; }

        public string EventTypeName { get; set; }

        public bool IsDeleted { get; set; }

        public List<Event> Events { get; set; }

        public EventType()
        {
            Events = new List<Event>();
        }
    }
}