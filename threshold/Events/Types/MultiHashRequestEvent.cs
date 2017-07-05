using threshold.Apis.VirusTotal.Requests;

namespace threshold.Events.Types
{
    public class MultiHashRequestEvent : IEvent
    {
        public IRequest MultiHashReportRequest { get; }

        public MultiHashRequestEvent(IRequest multiHashReportRequest)
        {
            MultiHashReportRequest = multiHashReportRequest;
        }

        public EventType GetEventType()
        {
            return EventType.MultiHashRequest;
        }
    }
}
