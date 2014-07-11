namespace FFTPatcher.Editors
{
    partial class AllPropositionDetailsEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propositionEditor1 = new FFTPatcher.Editors.PropositionEditor();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point( 3, 0 );
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size( 225, 394 );
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler( this.listBox1_SelectedIndexChanged );
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add( this.propositionEditor1 );
            this.panel1.Location = new System.Drawing.Point( 234, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 632, 394 );
            this.panel1.TabIndex = 2;
            // 
            // propositionEditor1
            // 
            this.propositionEditor1.AutoSize = true;
            this.propositionEditor1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.propositionEditor1.Location = new System.Drawing.Point( 3, 3 );
            this.propositionEditor1.Name = "propositionEditor1";
            this.propositionEditor1.Size = new System.Drawing.Size( 539, 254 );
            this.propositionEditor1.TabIndex = 1;
            // 
            // AllPropositionDetailsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.panel1 );
            this.Controls.Add( this.listBox1 );
            this.Name = "AllPropositionDetailsEditor";
            this.Size = new System.Drawing.Size( 866, 399 );
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private PropositionEditor propositionEditor1;
        private System.Windows.Forms.Panel panel1;
    }
}
