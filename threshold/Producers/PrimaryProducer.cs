using System;
using System.Collections.Generic;
using threshold.Producers.Connections;

namespace threshold.Producers
{
    public class PrimaryProducer<IProducer> : IProducer<IProducer>
    {
        ConnectionProducer _ConnectionProducer;

        public PrimaryProducer()
        {
            _ConnectionProducer = new ConnectionProducer();
        }

        public int IdealClientPollingInterval
        {
            get
            {
                return ProduceInterval * 1;
            }
        }

        public int MaintenanceInterval
        {
            get
            {
                return IdealClientPollingInterval * 2;
            }
        }

        public string Name
        {
            get
            {
                return "Blah";
            }
        }

        public int ProduceInterval
        {
            get
            {
                return 1000;
            }
        }

        public Dictionary<string, IProducer> GetData()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            _ConnectionProducer.Start();
        }

        public void Stop()
        {
            _ConnectionProducer.Stop();
        }
    }
}
