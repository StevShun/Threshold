using System.Collections.Generic;

namespace threshold.Producers
{
    public interface IProducer<T>
    {
        string Name { get; }

        void Start();

        void Stop();
    }
}
