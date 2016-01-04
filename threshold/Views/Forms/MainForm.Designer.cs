namespace threshold
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.consoleOutputTextBox = new System.Windows.Forms.RichTextBox();
            this.getListeningPortsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // consoleOutputTextBox
            // 
            this.consoleOutputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.consoleOutputTextBox.Location = new System.Drawing.Point(12, 12);
            this.consoleOutputTextBox.Name = "consoleOutputTextBox";
            this.consoleOutputTextBox.ReadOnly = true;
            this.consoleOutputTextBox.Size = new System.Drawing.Size(260, 212);
            this.consoleOutputTextBox.TabIndex = 0;
            this.consoleOutputTextBox.Text = "";
            // 
            // getListeningPortsButton
            // 
            this.getListeningPortsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.getListeningPortsButton.Location = new System.Drawing.Point(12, 230);
            this.getListeningPortsButton.Name = "getListeningPortsButton";
            this.getListeningPortsButton.Size = new System.Drawing.Size(260, 28);
            this.getListeningPortsButton.TabIndex = 1;
            this.getListeningPortsButton.Text = "Get Listening Ports";
            this.getListeningPortsButton.UseVisualStyleBackColor = true;
            this.getListeningPortsButton.Click += new System.EventHandler(this.getListeningPortsButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.getListeningPortsButton);
            this.Controls.Add(this.consoleOutputTextBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox consoleOutputTextBox;
        private System.Windows.Forms.Button getListeningPortsButton;
    }
}

