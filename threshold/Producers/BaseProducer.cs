using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;

namespace threshold.Producers
{
    public abstract class BaseProducer<T> : IProducer<T>
    {
        protected BackgroundWorker BackgroundThread = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };
        protected object Lock = new object();
        protected Timer MaintenanceTimer = new Timer
        {
            AutoReset = true
        };

        protected Dictionary<string, T> Data { get; set; } = new Dictionary<string, T>();

        public abstract string Name { get; }

        public abstract int ProduceInterval { get; }

        public int MaintenanceInterval
        {
            get
            {
                return ProduceInterval * 3;
            }
        }

        public int IdealClientPollingInterval
        {
            get
            {
                return MaintenanceInterval / 2;
            }
        }

        public Dictionary<string, T> GetData()
        {
            Dictionary<string, T> data = new Dictionary<string, T>();
            MaintenanceTimer.Stop();
            lock (Lock)
            {
                data = new Dictionary<string, T>(Data);
                Data.Clear();
            }
            MaintenanceTimer.Start();
            System.Diagnostics.Debug.WriteLine("Get data called for: " + Name);
            return data;
        }

        public void Start()
        {
            MaintenanceTimer.Elapsed += new ElapsedEventHandler(PerformMaintenance);
            MaintenanceTimer.Interval = MaintenanceInterval;
            MaintenanceTimer.Start();
            BackgroundThread.DoWork += Produce;
            BackgroundThread.RunWorkerAsync();
            System.Diagnostics.Debug.WriteLine("Start called for: " + Name);
        }

        public void Stop()
        {
            MaintenanceTimer.Stop();
            BackgroundThread.CancelAsync();
            System.Diagnostics.Debug.WriteLine("Stop called for: " + Name);
        }

        protected void PerformMaintenance(object source, ElapsedEventArgs e)
        {
            lock (Lock)
            {
                System.Diagnostics.Debug.WriteLine("Performing maintenance on: " 
                    + Name + ". Size before maintenance is: " + Data.Count);
                Data.Clear();
                System.Diagnostics.Debug.WriteLine("Size after maintenance on " 
                    + Name + " is: " + Data.Count);
            }
        }

        protected abstract void Produce(object sender, DoWorkEventArgs e);
    }
}
