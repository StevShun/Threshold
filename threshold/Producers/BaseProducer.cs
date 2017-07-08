using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using log4net;
using threshold.Events.Conduit;
using threshold.Events.Types;

namespace threshold.Producers
{
    public abstract class BaseProducer<T> : IProducer<T>, IEventListener
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BaseProducer<T>));

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
            Log.Debug("Start called for: " + Name);
        }

        public void Stop()
        {
            Log.Info("Stopping " + Name + "...");
            BackgroundThread.CancelAsync();
            bool isCleanStop = false;
            for (int i = 0; i < 10; i++)
            {
                if (BackgroundThread.IsBusy)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    isCleanStop = true;
                    break;
                }
            }
            if (isCleanStop)
            {
                Log.Debug("Stopped: " + Name);
            }
            else
            {
                Log.Error("Failed to stop: " + Name);
            }
        }

        protected abstract void Produce(object sender, DoWorkEventArgs e);

        public abstract void OnEvent(IEvent _event);

        public abstract List<EventType> GetNotifyTypes();
    }
}
