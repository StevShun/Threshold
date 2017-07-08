using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using log4net;
using threshold.Applications;
using threshold.Events.Conduit;
using threshold.Events.Types;

namespace threshold.Producers
{
    public class WindowsApplicationProducer : BaseProducer<IApplication>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WindowsApplicationProducer));
        private IEventConduit EventConduit;

        public WindowsApplicationProducer(IEventConduit eventConduit)
        {
            EventConduit = eventConduit;
            eventConduit.AddEventListener(this);
        }

        public override string Name
        {
            get
            {
                return "Windows Application Producer";
            }
        }

        protected override void Produce(object sender, DoWorkEventArgs e)
        {
            while (!BackgroundThread.CancellationPending)
            {
                Thread.Sleep(1000);
            }
            e.Cancel = true;
        }

        public override void OnEvent(IEvent _event)
        {
            EventType eventType = _event.GetEventType();
            switch(eventType)
            {
                case EventType.Connection:
                    ConnectionEvent connectionEvent = (ConnectionEvent)_event;
                    IApplication application = new WindowsApplication(connectionEvent.Connection);
                    string info = "App: " + application.Name + " Hash: " + application.Md5Hash;
                    Log.Info(info);
                    EventConduit.SendEvent(new WindowsApplicationEvent(application));
                    break;
            }
        }

        public override List<EventType> GetNotifyTypes()
        {
            List<EventType> eventTypes = new List<EventType>();
            eventTypes.Add(EventType.Connection);
            return eventTypes;
        }
    }
}
