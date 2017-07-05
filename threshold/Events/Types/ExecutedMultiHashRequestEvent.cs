using threshold.Apis.VirusTotal.Requests;

namespace threshold.Events.Types
{
    public class ExecutedMultiHashRequestEvent : IEvent
    {
        public IRequest Request { get; }

        public ExecutedMultiHashRequestEvent(IRequest executedRequest)
        {
            Request = executedRequest;
        }

        public EventType GetEventType()
        {
            return EventType.ExecutedMultiHashRequest;
        }
    }
}
