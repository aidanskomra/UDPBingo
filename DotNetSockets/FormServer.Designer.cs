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
            this.panelMoveCharacter = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxServer
            // 
            this.listBoxServer.FormattingEnabled = true;
            this.listBoxServer.Location = new System.Drawing.Point(13, 13);
            this.listBoxServer.Name = "listBoxServer";
            this.listBoxServer.Size = new System.Drawing.Size(228, 433);
            this.listBoxServer.TabIndex = 0;
            // 
            // panelMoveCharacter
            // 
            this.panelMoveCharacter.Location = new System.Drawing.Point(256, 13);
            this.panelMoveCharacter.Name = "panelMoveCharacter";
            this.panelMoveCharacter.Size = new System.Drawing.Size(532, 425);
            this.panelMoveCharacter.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(256, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(532, 425);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelMoveCharacter);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.listBoxServer);
            this.Name = "FormServer";
            this.Text = "FormServer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxServer;
        private System.Windows.Forms.Panel panelMoveCharacter;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}