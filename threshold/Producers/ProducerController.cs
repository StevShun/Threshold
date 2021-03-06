﻿using threshold.Applications;
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
            ApplicationProducer = new WindowsApplicationProducer(eventConduit);
            RequestProducer = new MultiHashRequestProducer(eventConduit);
            ResultProducer = new ExecutedHashRequestProducer(eventConduit);
        }

        public void Start()
        {
            ConnectionProducer.Start();
            ApplicationProducer.Start();
            RequestProducer.Start();
            ResultProducer.Start();
        }

        public void Stop()
        {
            ConnectionProducer.Stop();
            ApplicationProducer.Stop();
            RequestProducer.Stop();
            ResultProducer.Stop();
        }
    }
}
