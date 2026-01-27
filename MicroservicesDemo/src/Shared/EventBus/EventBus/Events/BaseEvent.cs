using System;

namespace EventBus.Events
{
    public abstract class BaseEvent
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected BaseEvent()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}