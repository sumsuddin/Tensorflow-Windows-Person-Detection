namespace PersonDetectorClient
{
    partial class Main
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
            this.pbMain = new System.Windows.Forms.PictureBox();
            this.btAutoPlay = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.cbCamList = new System.Windows.Forms.ComboBox();
            this.chkbUseStream = new System.Windows.Forms.CheckBox();
            this.tbRtmp = new System.Windows.Forms.TextBox();
            this.tbThreshold = new System.Windows.Forms.TextBox();
            this.lblThreshold = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pbMain
            // 
            this.pbMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbMain.Location = new System.Drawing.Point(12, 39);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(640, 640);
            this.pbMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMain.TabIndex = 2;
            this.pbMain.TabStop = false;
            // 
            // btAutoPlay
            // 
            this.btAutoPlay.Location = new System.Drawing.Point(751, 39);
            this.btAutoPlay.Name = "btAutoPlay";
            this.btAutoPlay.Size = new System.Drawing.Size(75, 28);
            this.btAutoPlay.TabIndex = 4;
            this.btAutoPlay.Text = "Play";
            this.btAutoPlay.UseVisualStyleBackColor = true;
            this.btAutoPlay.Click += new System.EventHandler(this.btAutoPlay_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(12, 682);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 17);
            this.lblMessage.TabIndex = 6;
            // 
            // cbCamList
            // 
            this.cbCamList.FormattingEnabled = true;
            this.cbCamList.Location = new System.Drawing.Point(531, 5);
            this.cbCamList.Name = "cbCamList";
            this.cbCamList.Size = new System.Drawing.Size(121, 24);
            this.cbCamList.TabIndex = 7;
            this.cbCamList.Text = "Camera devices";
            this.cbCamList.SelectedIndexChanged += new System.EventHandler(this.cbCamList_SelectedIndexChanged);
            // 
            // chkbUseStream
            // 
            this.chkbUseStream.AutoSize = true;
            this.chkbUseStream.Location = new System.Drawing.Point(12, 9);
            this.chkbUseStream.Name = "chkbUseStream";
            this.chkbUseStream.Size = new System.Drawing.Size(104, 21);
            this.chkbUseStream.TabIndex = 8;
            this.chkbUseStream.Text = "Use Stream";
            this.chkbUseStream.UseVisualStyleBackColor = true;
            this.chkbUseStream.CheckedChanged += new System.EventHandler(this.chkbUseRtmp_CheckedChanged);
            // 
            // tbRtmp
            // 
            this.tbRtmp.Enabled = false;
            this.tbRtmp.Location = new System.Drawing.Point(122, 7);
            this.tbRtmp.Name = "tbRtmp";
            this.tbRtmp.Size = new System.Drawing.Size(323, 22);
            this.tbRtmp.TabIndex = 9;
            this.tbRtmp.Text = "rtsp://184.72.239.149/vod/mp4:BigBuckBunny_115k.mov";
            this.tbRtmp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbRtmp_KeyDown);
            // 
            // tbThreshold
            // 
            this.tbThreshold.Location = new System.Drawing.Point(726, 657);
            this.tbThreshold.Name = "tbThreshold";
            this.tbThreshold.Size = new System.Drawing.Size(100, 22);
            this.tbThreshold.TabIndex = 10;
            this.tbThreshold.Text = "0.5";
            this.tbThreshold.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbThreshold_KeyDown);
            // 
            // lblThreshold
            // 
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(723, 637);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(107, 17);
            this.lblThreshold.TabIndex = 11;
            this.lblThreshold.Text = "Threshold (0-1)";
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(838, 710);
            this.Controls.Add(this.lblThreshold);
            this.Controls.Add(this.tbThreshold);
            this.Controls.Add(this.tbRtmp);
            this.Controls.Add(this.chkbUseStream);
            this.Controls.Add(this.cbCamList);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btAutoPlay);
            this.Controls.Add(this.pbMain);
            this.Name = "Main";
            this.Text = "Person Detector";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbMain;
        private System.Windows.Forms.Button btAutoPlay;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ComboBox cbCamList;
        private System.Windows.Forms.CheckBox chkbUseStream;
        private System.Windows.Forms.TextBox tbRtmp;
        private System.Windows.Forms.TextBox tbThreshold;
        private System.Windows.Forms.Label lblThreshold;
    }
}

