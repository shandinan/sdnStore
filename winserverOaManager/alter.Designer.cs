namespace winserverOaManager
{
    partial class alter
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
            this.msgShow = new System.Windows.Forms.Panel();
            this.lbMsgContent = new System.Windows.Forms.Label();
            this.msgShow.SuspendLayout();
            this.SuspendLayout();
            // 
            // msgShow
            // 
            this.msgShow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.msgShow.Controls.Add(this.lbMsgContent);
            this.msgShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msgShow.Location = new System.Drawing.Point(3, 24);
            this.msgShow.Name = "msgShow";
            this.msgShow.Size = new System.Drawing.Size(383, 210);
            this.msgShow.TabIndex = 0;
            this.msgShow.Tag = "sdn";
            // 
            // lbMsgContent
            // 
            this.lbMsgContent.AutoSize = true;
            this.lbMsgContent.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbMsgContent.ForeColor = System.Drawing.Color.Red;
            this.lbMsgContent.Location = new System.Drawing.Point(7, 27);
            this.lbMsgContent.Name = "lbMsgContent";
            this.lbMsgContent.Size = new System.Drawing.Size(0, 14);
            this.lbMsgContent.TabIndex = 0;
            // 
            // alter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 237);
            this.Controls.Add(this.msgShow);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "alter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "OA系统提醒";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.alter_Load);
            this.msgShow.ResumeLayout(false);
            this.msgShow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel msgShow;
        private System.Windows.Forms.Label lbMsgContent;
    }
}