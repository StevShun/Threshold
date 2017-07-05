using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using threshold.Applications;
using threshold.Apis.VirusTotal.Requests;
using threshold.Events.Conduit;
using threshold.Events.Types;

namespace threshold.Producers
{
    public class MultiHashRequestProducer : BaseProducer<IRequest>
    {
        private IEventConduit EventConduit;
        private HashSet<string> SubmittedApplicationHashes;
        private ConcurrentQueue<IApplication> PendingApplications;

        public MultiHashRequestProducer(IEventConduit eventConduit)
        {
            EventConduit = eventConduit;
            eventConduit.AddEventListener(this);
            SubmittedApplicationHashes = new HashSet<string>();
            PendingApplications = new ConcurrentQueue<IApplication>();
        }

        public override string Name
        {
            get
            {
                return "VirusTotal API Hash Report Request Producer";
            }
        }

        public override int ProduceIntervalMillis
        {
            get
            {
                return 5000;
            }
        }

        public override List<EventType> GetNotifyTypes()
        {
            List<EventType> eventTypes = new List<EventType>();
            eventTypes.Add(EventType.WindowsApplication);
            return eventTypes;
        }

        public override void OnEvent(IEvent _event)
        {
            EventType eventType = _event.GetEventType();
            switch (eventType)
            {
                case EventType.WindowsApplication:
                    WindowsApplicationEvent windowsApplicationEvent = (WindowsApplicationEvent)_event;
                    IApplication application = windowsApplicationEvent.Application;
                    if (SubmittedApplicationHashes.Add(application.Md5Hash))
                    {
                        PendingApplications.Enqueue(windowsApplicationEvent.Application);
                    }
                    break;
            }
        }

        protected override void Produce(object sender, DoWorkEventArgs e)
        {
            while (!BackgroundThread.CancellationPending)
            {
                if (!PendingApplications.IsEmpty)
                {
                    MultiHashRequest multiHashRequest =
                        RequestFactory.GetMultiHashRequest();

                    foreach (var item in PendingApplications)
                    {
                        IApplication application;
                        if (PendingApplications.TryDequeue(out application))
                        {
                            if (!multiHashRequest.AddApplication(application))
                            {
                                PendingApplications.Enqueue(application);
                                break;
                            }
                        }
                    }

                    multiHashRequest.Build();

                    MultiHashRequestEvent multiHashRequestEvent =
                        new MultiHashRequestEvent(multiHashRequest);

                    EventConduit.SendEvent(multiHashRequestEvent);
                }

                Thread.Sleep(ProduceIntervalMillis);
            }
            e.Cancel = true;
        }
    }
}
