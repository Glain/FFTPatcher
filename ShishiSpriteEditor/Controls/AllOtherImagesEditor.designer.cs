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
            this.pnl_Parameters = new System.Windows.Forms.Panel();
            this.ddl_Entry = new System.Windows.Forms.ComboBox();
            this.lbl_Entry = new System.Windows.Forms.Label();
            this.pnl_Palette = new System.Windows.Forms.Panel();
            this.chk_8bpp = new System.Windows.Forms.CheckBox();
            this.lbl_Palette = new System.Windows.Forms.Label();
            this.ddl_Palette = new System.Windows.Forms.ComboBox();
            this.cmbZoom = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new FFTPatcher.SpriteEditor.SeparatorComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnl_Parameters.SuspendLayout();
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
            this.panel1.Location = new System.Drawing.Point(3, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(472, 342);
            this.panel1.TabIndex = 2;
            this.panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel1_DragDrop);
            this.panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel1_DragEnter);
            // 
            // imageSizeLabel
            // 
            this.imageSizeLabel.AutoSize = true;
            this.imageSizeLabel.Location = new System.Drawing.Point(3, 97);
            this.imageSizeLabel.Name = "imageSizeLabel";
            this.imageSizeLabel.Size = new System.Drawing.Size(94, 13);
            this.imageSizeLabel.TabIndex = 3;
            this.imageSizeLabel.Text = "[Image Size Label]";
            // 
            // pnl_Parameters
            // 
            this.pnl_Parameters.Controls.Add(this.ddl_Entry);
            this.pnl_Parameters.Controls.Add(this.lbl_Entry);
            this.pnl_Parameters.Controls.Add(this.pnl_Palette);
            this.pnl_Parameters.Location = new System.Drawing.Point(5, 29);
            this.pnl_Parameters.Name = "pnl_Parameters";
            this.pnl_Parameters.Size = new System.Drawing.Size(469, 68);
            this.pnl_Parameters.TabIndex = 4;
            // 
            // ddl_Entry
            // 
            this.ddl_Entry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_Entry.FormattingEnabled = true;
            this.ddl_Entry.Location = new System.Drawing.Point(52, 3);
            this.ddl_Entry.Name = "ddl_Entry";
            this.ddl_Entry.Size = new System.Drawing.Size(251, 21);
            this.ddl_Entry.TabIndex = 3;
            this.ddl_Entry.SelectedIndexChanged += new System.EventHandler(this.ddl_Entry_SelectedIndexChanged);
            // 
            // lbl_Entry
            // 
            this.lbl_Entry.AutoSize = true;
            this.lbl_Entry.Location = new System.Drawing.Point(3, 8);
            this.lbl_Entry.Name = "lbl_Entry";
            this.lbl_Entry.Size = new System.Drawing.Size(34, 13);
            this.lbl_Entry.TabIndex = 2;
            this.lbl_Entry.Text = "Entry:";
            // 
            // pnl_Palette
            // 
            this.pnl_Palette.Controls.Add(this.cmbZoom);
            this.pnl_Palette.Controls.Add(this.chk_8bpp);
            this.pnl_Palette.Controls.Add(this.lbl_Palette);
            this.pnl_Palette.Controls.Add(this.ddl_Palette);
            this.pnl_Palette.Location = new System.Drawing.Point(-2, 25);
            this.pnl_Palette.Name = "pnl_Palette";
            this.pnl_Palette.Size = new System.Drawing.Size(470, 42);
            this.pnl_Palette.TabIndex = 5;
            // 
            // chk_8bpp
            // 
            this.chk_8bpp.AutoSize = true;
            this.chk_8bpp.Location = new System.Drawing.Point(148, 9);
            this.chk_8bpp.Name = "chk_8bpp";
            this.chk_8bpp.Size = new System.Drawing.Size(186, 17);
            this.chk_8bpp.TabIndex = 4;
            this.chk_8bpp.Text = "Import/Export as 256-color palette";
            this.chk_8bpp.UseVisualStyleBackColor = true;
            this.chk_8bpp.CheckedChanged += new System.EventHandler(this.chk_8bpp_CheckedChanged);
            // 
            // lbl_Palette
            // 
            this.lbl_Palette.AutoSize = true;
            this.lbl_Palette.Location = new System.Drawing.Point(3, 10);
            this.lbl_Palette.Name = "lbl_Palette";
            this.lbl_Palette.Size = new System.Drawing.Size(43, 13);
            this.lbl_Palette.TabIndex = 0;
            this.lbl_Palette.Text = "Palette:";
            // 
            // ddl_Palette
            // 
            this.ddl_Palette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddl_Palette.FormattingEnabled = true;
            this.ddl_Palette.Location = new System.Drawing.Point(54, 7);
            this.ddl_Palette.Name = "ddl_Palette";
            this.ddl_Palette.Size = new System.Drawing.Size(69, 21);
            this.ddl_Palette.TabIndex = 1;
            this.ddl_Palette.SelectedIndexChanged += new System.EventHandler(this.ddl_Palette_SelectedIndexChanged);
            // 
            // cmbZoom
            // 
            this.cmbZoom.FormattingEnabled = true;
            this.cmbZoom.Location = new System.Drawing.Point(389, 5);
            this.cmbZoom.Name = "cmbZoom";
            this.cmbZoom.Size = new System.Drawing.Size(78, 21);
            this.cmbZoom.TabIndex = 5;
            this.cmbZoom.SelectedIndexChanged += cmbZoom_SelectedIndexChanged;
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
            this.comboBox1.Size = new System.Drawing.Size(472, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // AllOtherImagesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_Parameters);
            this.Controls.Add(this.imageSizeLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.comboBox1);
            this.Enabled = false;
            this.Name = "AllOtherImagesEditor";
            this.Size = new System.Drawing.Size(478, 458);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnl_Parameters.ResumeLayout(false);
            this.pnl_Parameters.PerformLayout();
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
        private System.Windows.Forms.Panel pnl_Parameters;
        private System.Windows.Forms.ComboBox ddl_Palette;
        private System.Windows.Forms.Label lbl_Palette;
        private System.Windows.Forms.CheckBox chk_8bpp;
        private System.Windows.Forms.ComboBox ddl_Entry;
        private System.Windows.Forms.Label lbl_Entry;
        private System.Windows.Forms.Panel pnl_Palette;
        private System.Windows.Forms.ComboBox cmbZoom;
    }
}
