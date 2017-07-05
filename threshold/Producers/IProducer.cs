using System.Collections.Generic;

namespace threshold.Producers
{
    public interface IProducer<T>
    {
        string Name { get; }

        int ProduceIntervalMillis { get; }

        void Start();

        void Stop();
    }
}
