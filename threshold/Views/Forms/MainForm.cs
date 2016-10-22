using System;
using System.Collections.Generic;
using System.Windows.Forms;
using threshold.Views.Forms;
using threshold.Apis.VirusTotal;
using threshold.Producers;
using threshold.Producers.Applications;
using threshold.Producers.Connections;

namespace threshold
{
    public partial class MainForm : Form
    {
        private IProducer<IConnection> ConnectionProducer { get; set; }
        private IProducer<IApplication> ApplicationProducer { get; set; }

        public MainForm()
        {
            InitializeComponent();
            ConnectionProducer = new ConnectionProducer();
            ApplicationProducer = new WindowsApplicationProducer(ConnectionProducer);
            ApplicationProducer.Start();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Show();
        }

        private void getActiveConnectionsButton_Click(object sender, EventArgs e)
        {
            consoleOutputTextBox.Clear();
            /*
            List<IConnection> connections = new List<IConnection>();
            connections.AddRange(ConnectionProducer.GetData());
            consoleOutputTextBox.AppendText(getActiveConnectionsText(connections));
            */
            Dictionary<string, IApplication> applications = ApplicationProducer.GetData();
            consoleOutputTextBox.AppendText(getActiveWindowsApplicationsText(applications));
        }

        private string getActiveConnectionsText(Dictionary<string, IConnection> connections)
        {
            string activeConnectionsContent = "";

            foreach (Connection conn in connections.Values)
            {
                IApplication application = new WindowsApplication(conn.OwnerPid);
                activeConnectionsContent = activeConnectionsContent
                    + "Connection info:"
                    + Environment.NewLine
                    + "Local Address: " + conn.LocalAddress
                    + " | Local Port: " + conn.LocalPort.ToString()
                    + " | External Address: " + conn.ExternalAddress
                    + " | External Port: " + conn.ExternalPort.ToString()
                    + " | Proto: " + conn.Protocol
                    + " | Process: " + conn.OwnerPid
                    + " | State: " + conn.State
                    + Environment.NewLine
                    + "Application info:"
                    + Environment.NewLine
                    + "Name: " + application.Name
                    + " | Location: " + application.ExecutablePath
                    + " | Hash: " + application.Md5Hash
                    + Environment.NewLine
                    + "#########################################################"
                    + Environment.NewLine;
            }

            return activeConnectionsContent;
        }

        private string getActiveWindowsApplicationsText(Dictionary<string, IApplication> applications)
        {
            string applicationsText = "";

            foreach (IApplication app in applications.Values)
            {
                applicationsText = applicationsText
                    + "Aoo info:"
                    + Environment.NewLine
                    + "PID: " + app.Pid
                    + " | Name: " + app.Name
                    + " | Executable path: " + app.ExecutablePath
                    + " | MD5 Hash: " + app.Md5Hash
                    + Environment.NewLine
                    + "#########################################################"
                    + Environment.NewLine;
            }

            return applicationsText;
        }

        private void checkHashButton_Click(object sender, EventArgs e)
        {
            VirusTotalApi virusTotalApi = new VirusTotalApi();
            string info = virusTotalApi.RequestHashInfo("35b3e3e8ab090db701c1766704dd624d");
            consoleOutputTextBox.Clear();
            consoleOutputTextBox.AppendText(info);
        }
    }
}
