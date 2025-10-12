namespace DotNetSockets
{
    partial class FormServer
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
            this.listBoxServer = new System.Windows.Forms.ListBox();
            this.comboBoxBoardSize = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // listBoxServer
            // 
            this.listBoxServer.FormattingEnabled = true;
            this.listBoxServer.Location = new System.Drawing.Point(13, 39);
            this.listBoxServer.Name = "listBoxServer";
            this.listBoxServer.Size = new System.Drawing.Size(699, 407);
            this.listBoxServer.TabIndex = 0;
            // 
            // comboBoxBoardSize
            // 
            this.comboBoxBoardSize.ForeColor = System.Drawing.SystemColors.WindowText;
            this.comboBoxBoardSize.FormattingEnabled = true;
            this.comboBoxBoardSize.Items.AddRange(new object[] {
            "1x1",
            "2x2",
            "3x3",
            "4x4",
            "5x5"});
            this.comboBoxBoardSize.Location = new System.Drawing.Point(13, 13);
            this.comboBoxBoardSize.Name = "comboBoxBoardSize";
            this.comboBoxBoardSize.Size = new System.Drawing.Size(174, 21);
            this.comboBoxBoardSize.TabIndex = 1;
            this.comboBoxBoardSize.Text = "Board Size";
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboBoxBoardSize);
            this.Controls.Add(this.listBoxServer);
            this.Name = "FormServer";
            this.Text = "FormServer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxServer;
        private System.Windows.Forms.ComboBox comboBoxBoardSize;
    }
}