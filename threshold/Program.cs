using System;
using System.Windows.Forms;
using log4net;
using threshold.Events.Conduit;
using threshold.Producers;

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
            producerController.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            eventConduit.Stop();
            producerController.Stop();
        }
    }
}
