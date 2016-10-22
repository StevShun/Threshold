using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using threshold.Producers.Connections;

namespace threshold.Producers.Applications
{
    public class WindowsApplicationProducer : BaseProducer<IApplication>
    {
        private IProducer<IConnection> ConnectionProducer;

        public WindowsApplicationProducer(IProducer<IConnection> connectionProducer)
        {
            ConnectionProducer = connectionProducer;
        }

        public override string Name
        {
            get
            {
                return "Windows Application Producer";
            }
        }

        public override int ProduceInterval
        {
            get
            {
                return 2000;
            }
        }

        protected override void Produce(object sender, DoWorkEventArgs e)
        {
            ConnectionProducer.Start();
            while (!BackgroundThread.CancellationPending)
            {
                ConcurrentDictionary<string, IApplication> applications = new ConcurrentDictionary<string, IApplication>();
                Dictionary<string, IConnection> connections = new Dictionary<string, IConnection>(ConnectionProducer.GetData());

                Parallel.ForEach(connections, (connection) =>
                {
                    IApplication windowsApp = new WindowsApplication(connection.Value.OwnerPid);
                    applications.GetOrAdd(windowsApp.Md5Hash, windowsApp);
                });

                lock (Lock)
                {
                    foreach (IApplication application in applications.Values)
                    {
                        Data[application.Md5Hash] = application;
                        System.Diagnostics.Debug.WriteLine("Added " + application.Md5Hash);
                    }
                }

                System.Diagnostics.Debug.WriteLine("Added stuff for: " + Name);
                Thread.Sleep(ConnectionProducer.IdealClientPollingInterval);
            }
            ConnectionProducer.Stop();
            e.Cancel = true;
        }
    }
}
