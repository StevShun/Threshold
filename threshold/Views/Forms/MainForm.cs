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
        public Network.Ports ports = new Network.Ports();

        public MainForm()
        {
            InitializeComponent();
        }

        private void getListeningPortsButton_Click(object sender, EventArgs e)
        {
            List<int> allListeningPorts = ports.GetAllListeningPorts();

            List<string> safeText = 
                allListeningPorts.ConvertAll<string>
                (delegate(int i) {return i.ToString(); });

            consoleOutputTextBox.Clear();

            foreach (string s in safeText)
            {
                consoleOutputTextBox.AppendText(s + Environment.NewLine);
            }
        }
    }
}
