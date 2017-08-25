using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Concurrent;
using threshold.Views.Forms;
using threshold.Applications;
using threshold.Apis.VirusTotal.Requests;
using threshold.Connections;
using threshold.Events.Conduit;
using threshold.Events.Types;

namespace threshold
{
    public partial class MainForm : Form, IEventListener
    {
        private IEventConduit EventConduit;
        private BackgroundWorker _BackgroundWorker;
        private BlockingCollection<IRequest> ExecutedRequests;
        private TreeNode Parent;

        public MainForm(IEventConduit eventConduit)
        {
            InitializeComponent();
            ExecutedRequests = new BlockingCollection<IRequest>();
            applicationsTreeView.BeginUpdate();
            Parent = applicationsTreeView.Nodes.Add("Applications");
            applicationsTreeView.EndUpdate();
            EventConduit = eventConduit;
            EventConduit.AddEventListener(this);
            _BackgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _BackgroundWorker.DoWork += AddEventsToUserInterface;
            _BackgroundWorker.RunWorkerAsync();
        }

        private void AddEventsToUserInterface(object sender, DoWorkEventArgs e)
        {
            while (!_BackgroundWorker.CancellationPending)
            {
                IRequest request = null;
                if (ExecutedRequests.TryTake(out request, 1000))
                {
                    foreach (KeyValuePair<IApplication, Dictionary<string, string>> keyValuePair in request.GetResults())
                    {
                        IApplication application = keyValuePair.Key;
                        String value = application.Name + " - " + application.ExecutablePath;
                        if (IsHandleCreated)
                        {
                            //applicationsTreeView.BeginUpdate();
                            applicationsTreeView.Invoke((MethodInvoker)(() => Parent.Nodes.Add(value)));
                            //applicationsTreeView.EndUpdate();
                        }
                    }
                }
            }
        }

        public void OnEvent(IEvent _event)
        {
            EventType eventType = _event.GetEventType();
            switch (eventType)
            {
                case EventType.ExecutedMultiHashRequest:
                    ExecutedMultiHashRequestEvent executedMultiHashRequestEvent = (ExecutedMultiHashRequestEvent)_event;
                    ExecutedRequests.Add(executedMultiHashRequestEvent.Request);
                    break;
            }
        }

        public List<EventType> GetNotifyTypes()
        {
            List<EventType> eventTypes = new List<EventType>();
            eventTypes.Add(EventType.ExecutedMultiHashRequest);
            return eventTypes;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Show();
        }
    }
}
