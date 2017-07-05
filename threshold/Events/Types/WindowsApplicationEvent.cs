using threshold.Applications;

namespace threshold.Events.Types
{
    public class WindowsApplicationEvent : IEvent
    {
        public IApplication Application { get; }

        public WindowsApplicationEvent(IApplication application)
        {
            Application = application;
        }

        public EventType GetEventType()
        {
            return EventType.WindowsApplication;
        }
    }
}
