using System.ComponentModel;

namespace threshold.Consumers
{
    public abstract class BaseConsumer<T> : IConsumer<T>
    {
        protected BackgroundWorker BackgroundThread = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };
        protected object Lock = new object();

        public abstract int ConsumeInterval { get; set; }

        public abstract string Name { get; }

        public void Start()
        {
            BackgroundThread.DoWork += Consume;
            BackgroundThread.RunWorkerAsync();
            System.Diagnostics.Debug.WriteLine("Start called for: " + Name);
        }

        public void Stop()
        {
            BackgroundThread.CancelAsync();
            System.Diagnostics.Debug.WriteLine("Stop called for: " + Name);
        }

        protected abstract void Consume(object sender, DoWorkEventArgs e);
    }
}
