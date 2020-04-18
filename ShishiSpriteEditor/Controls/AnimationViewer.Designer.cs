namespace FFTPatcher.SpriteEditor
{
    partial class AnimationViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.control1 = new System.Windows.Forms.PictureBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.playButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.forwardButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.zoomPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.control1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.zoomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // control1
            // 
            this.control1.Location = new System.Drawing.Point(0, 0);
            this.control1.Name = "control1";
            //this.control1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.control1.Size = new System.Drawing.Size(242, 257);
            //this.control1.Size = new System.Drawing.Size(242, 232);
            //this.control1.Size = new System.Drawing.Size(242, 275);
            //this.control1.Size = new System.Drawing.Size(242, 300);
            this.control1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.control1.TabIndex = 0;
            this.control1.TabStop = false;
            // 
            // trackBar1
            // 
            //this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            //| System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.trackBar1.AutoSize = false;
            //this.trackBar1.Location = new System.Drawing.Point(128, 336);
            this.trackBar1.Location = new System.Drawing.Point(128, 295);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 27);
            this.trackBar1.TabIndex = 3;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            //this.playButton.AutoSize = true;
            //this.playButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            //this.playButton.Location = new System.Drawing.Point(3, 336);
            this.playButton.Location = new System.Drawing.Point(3, 295);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(37, 23);
            this.playButton.TabIndex = 0;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            //this.backButton.AutoSize = true;
            //this.backButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.backButton.Enabled = false;
            //this.backButton.Location = new System.Drawing.Point(99, 336);
            this.backButton.Location = new System.Drawing.Point(99, 295);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(23, 23);
            this.backButton.TabIndex = 2;
            this.backButton.Text = "<";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // forwardButton
            // 
            this.forwardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.forwardButton.AutoSize = true;
            //this.forwardButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            //this.forwardButton.Enabled = false;
            //this.forwardButton.Location = new System.Drawing.Point(238, 336);
            this.forwardButton.Location = new System.Drawing.Point(238, 295);
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(23, 23);
            this.forwardButton.TabIndex = 4;
            this.forwardButton.Text = ">";
            this.forwardButton.UseVisualStyleBackColor = true;
            this.forwardButton.Click += new System.EventHandler(this.forwardButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            //this.pauseButton.AutoSize = true;
            //this.pauseButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(46, 295);
            //this.pauseButton.Location = new System.Drawing.Point(46, 336);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(47, 23);
            this.pauseButton.TabIndex = 1;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // zoomPanel
            // 
            this.zoomPanel.AutoScroll = true;
            this.zoomPanel.BackColor = System.Drawing.Color.Black;
            this.zoomPanel.Location = new System.Drawing.Point(0, 0);
            this.zoomPanel.Name = "zoomPanel";
            //this.zoomPanel.Size = new System.Drawing.Size(262, 330);
            this.zoomPanel.Bounds = new System.Drawing.Rectangle(0, 0, this.control1.Width + 10 + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth,
                this.control1.Height + 10 + System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight);
            //this.zoomPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.zoomPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            //this.zoomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zoomPanel.Controls.Add(this.control1);
            this.zoomPanel.TabIndex = 5;
            // 
            // AnimationViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.forwardButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.zoomPanel);
            this.Name = "AnimationViewer";
            //this.Size = new System.Drawing.Size(264, 366);
            //this.Size = new System.Drawing.Size(264, 225);
            //this.Size = new System.Drawing.Size(268, 275);
            this.Size = new System.Drawing.Size(268, 325);
            //this.Size = new System.Drawing.Size(268, 305);
            ((System.ComponentModel.ISupportInitialize)(this.control1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.zoomPanel.ResumeLayout(false);
            this.zoomPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel zoomPanel;
        private System.Windows.Forms.PictureBox control1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button forwardButton;
        private System.Windows.Forms.Button pauseButton;
    }
}
