namespace FFTPatcher.SpriteEditor
{
    partial class AllOtherImagesEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.imageSizeLabel = new System.Windows.Forms.Label();
            this.pnl_Palette = new System.Windows.Forms.Panel();
            this.ddl_Palette = new System.Windows.Forms.ComboBox();
            this.lbl_Palette = new System.Windows.Forms.Label();
            this.comboBox1 = new FFTPatcher.SpriteEditor.SeparatorComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnl_Palette.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(265, 288);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(3, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(412, 342);
            this.panel1.TabIndex = 2;
            this.panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel1_DragDrop);
            this.panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel1_DragEnter);
            // 
            // imageSizeLabel
            // 
            this.imageSizeLabel.AutoSize = true;
            this.imageSizeLabel.Location = new System.Drawing.Point(3, 57);
            this.imageSizeLabel.Name = "imageSizeLabel";
            this.imageSizeLabel.Size = new System.Drawing.Size(35, 13);
            this.imageSizeLabel.TabIndex = 3;
            this.imageSizeLabel.Text = "label1";
            // 
            // pnl_Palette
            // 
            this.pnl_Palette.Controls.Add(this.ddl_Palette);
            this.pnl_Palette.Controls.Add(this.lbl_Palette);
            this.pnl_Palette.Location = new System.Drawing.Point(5, 29);
            this.pnl_Palette.Name = "pnl_Palette";
            this.pnl_Palette.Size = new System.Drawing.Size(409, 28);
            this.pnl_Palette.TabIndex = 4;
            this.pnl_Palette.Visible = false;
            // 
            // ddl_Palette
            // 
            this.ddl_Palette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_Palette.FormattingEnabled = true;
            this.ddl_Palette.Location = new System.Drawing.Point(49, 3);
            this.ddl_Palette.Name = "ddl_Palette";
            this.ddl_Palette.Size = new System.Drawing.Size(69, 21);
            this.ddl_Palette.TabIndex = 1;
            this.ddl_Palette.SelectedIndexChanged += new System.EventHandler(this.ddl_Palette_SelectedIndexChanged);
            // 
            // lbl_Palette
            // 
            this.lbl_Palette.AutoSize = true;
            this.lbl_Palette.Location = new System.Drawing.Point(0, 5);
            this.lbl_Palette.Name = "lbl_Palette";
            this.lbl_Palette.Size = new System.Drawing.Size(43, 13);
            this.lbl_Palette.TabIndex = 0;
            this.lbl_Palette.Text = "Palette:";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox1.DropDownHeight = 212;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IntegralHeight = false;
            this.comboBox1.Location = new System.Drawing.Point(3, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(412, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // AllOtherImagesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_Palette);
            this.Controls.Add(this.imageSizeLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.comboBox1);
            this.Enabled = false;
            this.Name = "AllOtherImagesEditor";
            this.Size = new System.Drawing.Size(418, 418);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnl_Palette.ResumeLayout(false);
            this.pnl_Palette.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FFTPatcher.SpriteEditor.SeparatorComboBox comboBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label imageSizeLabel;
        private System.Windows.Forms.Panel pnl_Palette;
        private System.Windows.Forms.ComboBox ddl_Palette;
        private System.Windows.Forms.Label lbl_Palette;
    }
}
