using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
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
            while (!BackgroundThread.CancellationPending)
            {
                foreach (IConnection connection in Netstat.SingleExecution.GetCurrentConnections())
                {
                    if (BackgroundThread.CancellationPending)
                    {
                        break;
                    }
                    else
                    {
                        ConnectionEvent connectionEvent = new ConnectionEvent(connection);
                        EventConduit.SendEvent(connectionEvent);
                    }
                }
                if (BackgroundThread.CancellationPending)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
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
