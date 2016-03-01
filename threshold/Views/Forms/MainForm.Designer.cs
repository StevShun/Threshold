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
            this.getActiveConnectionsButton = new System.Windows.Forms.Button();
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
            // getActiveConnectionsButton
            // 
            this.getActiveConnectionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.getActiveConnectionsButton.Location = new System.Drawing.Point(12, 230);
            this.getActiveConnectionsButton.Name = "getActiveConnectionsButton";
            this.getActiveConnectionsButton.Size = new System.Drawing.Size(260, 28);
            this.getActiveConnectionsButton.TabIndex = 1;
            this.getActiveConnectionsButton.Text = "Get Active Connections";
            this.getActiveConnectionsButton.UseVisualStyleBackColor = true;
            this.getActiveConnectionsButton.Click += new System.EventHandler(this.getActiveConnectionsButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.getActiveConnectionsButton);
            this.Controls.Add(this.consoleOutputTextBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox consoleOutputTextBox;
        private System.Windows.Forms.Button getActiveConnectionsButton;
    }
}

