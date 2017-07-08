using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using threshold.Apis.VirusTotal.Requests;
using threshold.Events.Conduit;
using threshold.Events.Types;

namespace threshold.Producers
{
    class ExecutedHashRequestProducer : BaseProducer<IRequest>
    {
        private IEventConduit EventConduit;
        private ConcurrentQueue<IRequest> RequestsToExecute;

        public ExecutedHashRequestProducer(IEventConduit eventConduit)
        {
            EventConduit = eventConduit;
            eventConduit.AddEventListener(this);
            RequestsToExecute = new ConcurrentQueue<IRequest>();
        }

        public override string Name
        {
            get
            {
                return "VirusTotal API Request Result Producer";
            }
        }

        protected override void Produce(object sender, DoWorkEventArgs e)
        {
            while (!BackgroundThread.CancellationPending)
            {
                IRequest request;
                if (RequestsToExecute.TryDequeue(out request))
                {
                    request.ExecuteSynchronously();

                    if (request.ReceivedResponseFromServer)
                    {
                        ExecutedMultiHashRequestEvent requestEvent =
                            new ExecutedMultiHashRequestEvent(request);
                        EventConduit.SendEvent(requestEvent);
                        
                        System.Diagnostics.Debug.WriteLine(
                            "Executed request. Result: " + request.GetServerResponse());
                    }
                    else
                    {
                        RequestsToExecute.Enqueue(request);
                        System.Diagnostics.Debug.WriteLine(
                            "Failed to receive response from server. Request requeued.");
                    }
                }
                // VirusTotal API request limit is 4 requests per minute,
                // which means we must wait at least 15 seconds.
                Thread.Sleep(15000);
            }
            e.Cancel = true;
        }

        public override void OnEvent(IEvent _event)
        {
            EventType eventType = _event.GetEventType();
            switch (eventType)
            {
                case EventType.MultiHashRequest:
                    MultiHashRequestEvent multiHashReportRequestEvent =
                        (MultiHashRequestEvent)_event;
                    IRequest multiHashReportRequest =
                        multiHashReportRequestEvent.MultiHashReportRequest;
                    RequestsToExecute.Enqueue(multiHashReportRequest);
                    break;
            }
        }

        public override List<EventType> GetNotifyTypes()
        {
            List<EventType> eventTypes = new List<EventType>();
            eventTypes.Add(EventType.MultiHashRequest);
            return eventTypes;
        }
    }
}
