using System;

namespace Aec.Cqrs
{
    public class EventHandled : ISystemEvent
    {
        public readonly object Event;

        public EventHandled(object @event)
        {
            Event = @event;
        }

        public override string ToString()
        {
            return "Handled Event: " + Event + " at " + DateTime.UtcNow + " UTC";
        }
    }
}