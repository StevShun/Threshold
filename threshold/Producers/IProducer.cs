using System.Collections.Generic;

namespace threshold.Producers
{
    public interface IProducer<T>
    {
        string Name { get; }

        int ProduceInterval { get; }

        int MaintenanceInterval { get; }

        int IdealClientPollingInterval { get; }

        Dictionary<string, T> GetData();

        void Start();

        void Stop();
    }
}
