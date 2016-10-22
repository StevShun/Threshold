using System.ComponentModel;
using System.Threading;
using threshold.Tools;

namespace threshold.Producers.Connections
{
    public class ConnectionProducer : BaseProducer<IConnection>
    {
        public ConnectionProducer()
        {

        }

        public override string Name
        {
            get
            {
                return "Connection Producer";
            }
        }

        public override int ProduceInterval
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
                lock (Lock)
                {
                    foreach (Connection connection in netstat.GetConnections())
                    {
                        Data[connection.OwnerPid.ToString()] = connection;
                    }
                }
                System.Diagnostics.Debug.WriteLine("Added stuff for: " + Name);
                Thread.Sleep(ProduceInterval);
            }
            e.Cancel = true;
        }
    }
}
