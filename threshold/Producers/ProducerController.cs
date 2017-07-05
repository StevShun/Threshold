using threshold.Applications;
using threshold.Events.Conduit;
using threshold.Connections;
using threshold.Apis.VirusTotal.Requests;

namespace threshold.Producers
{
    public class ProducerController
    {
        private IEventConduit EventConduit;
        private IProducer<IConnection> ConnectionProducer;
        private IProducer<IApplication> ApplicationProducer;
        private IProducer<IRequest> RequestProducer;
        private IProducer<IRequest> ResultProducer;

        public ProducerController(IEventConduit eventConduit)
        {
            EventConduit = eventConduit;
            ConnectionProducer = new ConnectionProducer(eventConduit);
        }

        public void StartConnectionProducer()
        {
            ConnectionProducer.Start();
        }

        public void StopConnectionProducer()
        {
            ConnectionProducer.Stop();
        }
    }
}
