using threshold.Connections;

namespace threshold.Events.Types
{
    public class ConnectionEvent : IEvent
    {
        public IConnection Connection { get; }

        public ConnectionEvent(IConnection connection)
        {
            Connection = connection;
        }

        public EventType GetEventType()
        {
            return EventType.Connection;
        }
    }
}
