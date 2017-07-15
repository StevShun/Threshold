using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using log4net;
using threshold.Applications;
using threshold.Apis.VirusTotal.Requests;
using threshold.Events.Conduit;
using threshold.Events.Types;

namespace threshold.Producers
{
    public class MultiHashRequestProducer : BaseProducer<IRequest>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MultiHashRequestProducer));
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
                    string info = "'" + windowsApplicationEvent.Application.Name
                        + "', PID '" + windowsApplicationEvent.Application.Pid + "', hash '"
                        + windowsApplicationEvent.Application.Md5Hash + "'";
                    Log.Info("Got application: " + info);
                    if (windowsApplicationEvent.Application.IsSystemOwned)
                    {
                        Log.Info("Application " + info + " will not be scanned because it is system owned");
                    }
                    // Use a HashSet to store submitted applications. This way,
                    // redundant requests will not be produced.
                    else if (SubmittedApplicationHashes.Add(windowsApplicationEvent.Application.Md5Hash))
                    {
                        PendingApplications.Enqueue(windowsApplicationEvent.Application);
                        Log.Info("Queued application: " + info);
                    }
                    else
                    {
                        Log.Info("Application " + info + " has already been enqueued");
                    }
                    break;
            }
        }

        protected override void Produce(object sender, DoWorkEventArgs e)
        {
            while (!BackgroundThread.CancellationPending)
            {
                if (RequestFactory.IsApiKeyValid)
                {
                    if (!PendingApplications.IsEmpty)
                    {
                        MultiHashRequest multiHashRequest = RequestFactory.GetMultiHashRequest();

                        foreach (var item in PendingApplications)
                        {
                            if (BackgroundThread.CancellationPending)
                            {
                                break;
                            }
                            else
                            {
                                IApplication application;
                                if (PendingApplications.TryDequeue(out application))
                                {
                                    if (multiHashRequest.AddApplication(application))
                                    {
                                        Log.Info("Added '" + application.Name + "' to the request");
                                    }
                                    else
                                    {
                                        Log.Debug("Requeued application because the request is full: "
                                            + application.Name);
                                        // Requeue the application if the hash request is full.
                                        PendingApplications.Enqueue(application);
                                        break;
                                    }
                                }
                            }
                        }

                        if (BackgroundThread.CancellationPending)
                        {
                            break;
                        }
                        else
                        {
                            Log.Info("Building multi hash request...");
                            multiHashRequest.Build();
                            IEvent multiHashRequestEvent = new MultiHashRequestEvent(multiHashRequest);
                            EventConduit.SendEvent(multiHashRequestEvent);
                        }
                    }
                }
                else
                {
                    Log.Error("VirusTotal API key is invalid. Please set the API key");
                }

                Thread.Sleep(1000);
            }
            e.Cancel = true;
        }
    }
}
