namespace FFTorgASM
{
    partial class ConflictChecker
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
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Filelabel = new System.Windows.Forms.Label();
            this.Filelistbox = new System.Windows.Forms.ListBox();
            this.Patcheslistview = new System.Windows.Forms.ListView();
            this.ConflictListview = new System.Windows.Forms.ListView();
            this.WriteLocations = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PatchNumbers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ConflictWriteLocations = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbl_ViewType = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(262, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Patches";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(873, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Enabled = false;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Visible = false;
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Files";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(296, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "This patch is in";
            // 
            // Filelabel
            // 
            this.Filelabel.AutoSize = true;
            this.Filelabel.Location = new System.Drawing.Point(380, 11);
            this.Filelabel.Name = "Filelabel";
            this.Filelabel.Size = new System.Drawing.Size(16, 13);
            this.Filelabel.TabIndex = 5;
            this.Filelabel.Text = "...";
            // 
            // Filelistbox
            // 
            this.Filelistbox.FormattingEnabled = true;
            this.Filelistbox.Location = new System.Drawing.Point(12, 67);
            this.Filelistbox.Name = "Filelistbox";
            this.Filelistbox.Size = new System.Drawing.Size(86, 108);
            this.Filelistbox.TabIndex = 8;
            this.Filelistbox.SelectedIndexChanged += new System.EventHandler(this.Filelistbox_SelectedIndexChanged_1);
            // 
            // Patcheslistview
            // 
            this.Patcheslistview.LabelEdit = true;
            this.Patcheslistview.LabelWrap = false;
            this.Patcheslistview.Location = new System.Drawing.Point(104, 67);
            this.Patcheslistview.MultiSelect = false;
            this.Patcheslistview.Name = "Patcheslistview";
            this.Patcheslistview.Size = new System.Drawing.Size(388, 471);
            this.Patcheslistview.TabIndex = 10;
            this.Patcheslistview.UseCompatibleStateImageBehavior = false;
            this.Patcheslistview.View = System.Windows.Forms.View.List;
            this.Patcheslistview.SelectedIndexChanged += new System.EventHandler(this.Patcheslistview_SelectedIndexChanged);
            // 
            // ConflictListview
            // 
            this.ConflictListview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.WriteLocations,
            this.PatchNumbers,
            this.ConflictWriteLocations});
            this.ConflictListview.LabelEdit = true;
            this.ConflictListview.Location = new System.Drawing.Point(498, 67);
            this.ConflictListview.MultiSelect = false;
            this.ConflictListview.Name = "ConflictListview";
            this.ConflictListview.Size = new System.Drawing.Size(341, 471);
            this.ConflictListview.TabIndex = 11;
            this.ConflictListview.UseCompatibleStateImageBehavior = false;
            this.ConflictListview.View = System.Windows.Forms.View.List;
            this.ConflictListview.SelectedIndexChanged += new System.EventHandler(this.ConflictListview_SelectedIndexChanged);
            // 
            // WriteLocations
            // 
            this.WriteLocations.Text = "Write Location";
            this.WriteLocations.Width = 130;
            // 
            // PatchNumbers
            // 
            this.PatchNumbers.Text = "Patch #";
            this.PatchNumbers.Width = 80;
            // 
            // ConflictWriteLocations
            // 
            this.ConflictWriteLocations.Text = "Conflict write locations";
            this.ConflictWriteLocations.Width = 200;
            // 
            // lbl_ViewType
            // 
            this.lbl_ViewType.AutoSize = true;
            this.lbl_ViewType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ViewType.Location = new System.Drawing.Point(636, 40);
            this.lbl_ViewType.Name = "lbl_ViewType";
            this.lbl_ViewType.Size = new System.Drawing.Size(70, 20);
            this.lbl_ViewType.TabIndex = 13;
            this.lbl_ViewType.Text = "Conflicts";
            // 
            // ConflictChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 548);
            this.Controls.Add(this.lbl_ViewType);
            this.Controls.Add(this.ConflictListview);
            this.Controls.Add(this.Patcheslistview);
            this.Controls.Add(this.Filelistbox);
            this.Controls.Add(this.Filelabel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ConflictChecker";
            this.Text = "Conflict Checker v1.0";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label Filelabel;
        private System.Windows.Forms.ListBox Filelistbox;
        private System.Windows.Forms.ListView Patcheslistview;
        private System.Windows.Forms.ListView ConflictListview;
        private System.Windows.Forms.ColumnHeader WriteLocations;
        private System.Windows.Forms.ColumnHeader PatchNumbers;
        private System.Windows.Forms.ColumnHeader ConflictWriteLocations;
        private System.Windows.Forms.Label lbl_ViewType;
    }
}

