using System;
using System.Collections.Generic;
using System.Windows.Forms;
using threshold.Views.Forms;
using threshold.Layers;
using threshold.Apis.VirusTotal;

namespace threshold
{
    public partial class MainForm : Form
    {
        public Connection connection = new Connection();

        public MainForm()
        {
            InitializeComponent();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Show();
        }

        private void getActiveConnectionsButton_Click(object sender, EventArgs e)
        {
            consoleOutputTextBox.Clear();
            List<Connection> connections = connection.GetActiveConnections();
            consoleOutputTextBox.AppendText(getActiveConnectionsText(connections));
        }

        private string getActiveConnectionsText(List<Connection> connections)
        {
            string activeConnectionsContent = "";

            foreach (Connection conn in connections)
            {
                SystemApplication application = new SystemApplication(conn.OwnerPid);
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
                    + " | Hash: " + application.Hash
                    + Environment.NewLine
                    + "#########################################################"
                    + Environment.NewLine;
            }

            return activeConnectionsContent;
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
