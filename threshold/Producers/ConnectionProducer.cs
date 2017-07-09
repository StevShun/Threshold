using System.Collections.Generic;
using System.ComponentModel;
using threshold.Connections;
using threshold.Events.Conduit;
using threshold.Events.Types;
using threshold.Tools;

namespace threshold.Producers
{
    public class ConnectionProducer : BaseProducer<IConnection>
    {
        private IEventConduit EventConduit;

        public ConnectionProducer(IEventConduit eventConduit)
        {
            EventConduit = eventConduit;
        }

        public override string Name
        {
            get
            {
                return "Connection Producer";
            }
        }

        protected override void Produce(object sender, DoWorkEventArgs e)
        {
            Netstat.Daemon netstatDaemon = new Netstat.Daemon();
            if (netstatDaemon.Start())
            {
                while (!BackgroundThread.CancellationPending && !netstatDaemon.HasExited)
                {
                    IConnection connection = netstatDaemon.TryGetConnection(1000);
                    if (connection == null)
                    {
                        continue;
                    }
                    else
                    {
                        ConnectionEvent connectionEvent = new ConnectionEvent(connection);
                        EventConduit.SendEvent(connectionEvent);
                    }
                }
            }
            netstatDaemon.Stop();
            e.Cancel = true;
        }

        public override void OnEvent(IEvent _event)
        {
            return;
        }

        public override List<EventType> GetNotifyTypes()
        {
            return new List<EventType>();
        }
    }
}