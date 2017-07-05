using System.Collections.Generic;
using System.ComponentModel;
using threshold.Events.Conduit;
using threshold.Events.Types;

namespace threshold.Producers
{
    public abstract class BaseProducer<T> : IProducer<T>, IEventListener
    {
        protected BackgroundWorker BackgroundThread = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        public abstract string Name { get; }

        public abstract int ProduceIntervalMillis { get; }

        public void Start()
        {
            BackgroundThread.DoWork += Produce;
            BackgroundThread.RunWorkerAsync();
            System.Diagnostics.Debug.WriteLine("Start called for: " + Name);
        }

        public void Stop()
        {
            BackgroundThread.CancelAsync();
            System.Diagnostics.Debug.WriteLine("Stop called for: " + Name);
        }

        protected abstract void Produce(object sender, DoWorkEventArgs e);

        public abstract void OnEvent(IEvent _event);

        public abstract List<EventType> GetNotifyTypes();
    }
}
