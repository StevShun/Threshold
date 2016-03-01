using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            consoleOutputTextBox.Clear();

            foreach (Connection conn in connection.GetActiveConnections())
            {
                consoleOutputTextBox.AppendText(
                    " Local Address: " + conn.LocalAddress +
                    " Local Port: " + conn.LocalPort.ToString() +
                    " External Address: " + conn.ExternalAddress +
                    " External Port: " + conn.ExternalPort.ToString() +
                    " Proto: " + conn.Protocol +
                    " Process: " + conn.Owner.Name +
                    " State: " + conn.State +
                    Environment.NewLine +
                    " #########################################################"
                    + Environment.NewLine);
            }
        }
    }
}
