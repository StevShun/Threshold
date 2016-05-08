using System;
using System.Collections.Generic;
using System.Windows.Forms;
using threshold.Network;

namespace threshold
{
    public partial class MainForm : Form
    {
        public Connection connection = new Connection();

        public MainForm()
        {
            InitializeComponent();
        }

        private void getActiveConnectionsButton_Click(object sender, EventArgs e)
        {
            List<Connection> conns = connection.GetActiveConnections();

            consoleOutputTextBox.Clear();
            consoleOutputTextBox.AppendText("Number of connections: " 
                + conns.Count
                + Environment.NewLine);

            foreach (Connection conn in conns)
            {
                consoleOutputTextBox.AppendText(
                    " Local Address: " + conn.LocalAddress
                    + " | Local Port: " + conn.LocalPort.ToString()
                    + " | External Address: " + conn.ExternalAddress
                    + " | External Port: " + conn.ExternalPort.ToString()
                    + " | Proto: " + conn.Protocol
                    + " | Process: " + conn.OwnerPid
                    + " | State: " + conn.State
                    + Environment.NewLine
                    + "#########################################################"
                    + Environment.NewLine);
            }
        }
    }
}
