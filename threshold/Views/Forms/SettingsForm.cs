using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace threshold.Views.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            apiKeyTextBox.Text = Properties.Settings.Default.VirusTotalApiKey;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (apiKeyTextBox.IsHandleCreated && apiKeyTextBox.Text != null)
            {
                Properties.Settings.Default.VirusTotalApiKey = apiKeyTextBox.Text;
                Properties.Settings.Default.Save();
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
