using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using threshold.Applications;
using threshold.Connections;
using threshold.Events.Conduit;
using threshold.Events.Types;

namespace threshold.Producers
{
    public class WindowsApplicationProducer : BaseProducer<IApplication>
    {
        private IEventConduit EventConduit;
        private ConcurrentBag<IConnection> Connections;

        public WindowsApplicationProducer(IEventConduit eventConduit)
        {
            EventConduit = eventConduit;
            eventConduit.AddEventListener(this);
            Connections = new ConcurrentBag<IConnection>();
        }

        public override string Name
        {
            get
            {
                return "Windows Application Producer";
            }
        }

        public override int ProduceIntervalMillis
        {
            get
            {
                return 1000;
            }
        }

        protected override void Produce(object sender, DoWorkEventArgs e)
        {
            while (!BackgroundThread.CancellationPending)
            {
                foreach(IConnection connection in Connections)
                {
                    WindowsApplication windowsApplication = new WindowsApplication(connection);
                    WindowsApplicationEvent windowsApplicationEvent = 
                        new WindowsApplicationEvent(windowsApplication);
                    EventConduit.SendEvent(windowsApplicationEvent);
                }
                Thread.Sleep(ProduceIntervalMillis);
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
                    Connections.Add(connectionEvent.Connection);
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
