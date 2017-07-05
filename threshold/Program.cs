using System;
using System.Windows.Forms;
using log4net;
using threshold.Apis.VirusTotal.Requests;
using threshold.Applications;
using threshold.Events.Conduit;
using threshold.Connections;
using threshold.Producers;
using System.Threading;

namespace threshold
{
    static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        [STAThread]
        static void Main()
        {
            IEventConduit eventConduit = new DefaultEventConduit();
            eventConduit.Start();
            ProducerController producerController = new ProducerController(eventConduit);
            producerController.StartConnectionProducer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            eventConduit.Stop();
            producerController.StopConnectionProducer();
        }
    }
}
