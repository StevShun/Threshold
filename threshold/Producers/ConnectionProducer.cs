using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using log4net;
using threshold.Connections;
using threshold.Events.Conduit;
using threshold.Events.Types;
using threshold.Tools;

namespace threshold.Producers
{
    public class ConnectionProducer : BaseProducer<IConnection>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConnectionProducer));
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

        public override int ProduceIntervalMillis
        {
            get
            {
                return 1000;
            }
        }

        protected override void Produce(object sender, DoWorkEventArgs e)
        {
            Netstat netstat = new Netstat();
            while (!BackgroundThread.CancellationPending)
            {
                foreach (IConnection connection in netstat.GetConnections())
                {
                    string info = connection.OwnerExecutablePath + " PID: " + connection.OwnerPid;
                    Log.Debug("New connection: " + info);
                    ConnectionEvent connectionEvent = new ConnectionEvent(connection);
                    EventConduit.SendEvent(connectionEvent);
                }
                Thread.Sleep(ProduceIntervalMillis);
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
