using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace threshold
{
    public partial class MainForm : Form
    {
        public Layers.Transport.Ports ports = new Layers.Transport.Ports();

        public MainForm()
        {
            InitializeComponent();
        }

        private void getListeningPortsButton_Click(object sender, EventArgs e)
        {
            int[] allListeningPorts = ports.AllListeningPorts;
            string[] safeText =
                Array.ConvertAll(allListeningPorts, element => element.ToString());

            consoleOutputTextBox.Clear();

            foreach (string s in safeText)
            {
                consoleOutputTextBox.AppendText(s + Environment.NewLine);
            }
        }
    }
}
