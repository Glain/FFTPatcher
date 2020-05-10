namespace FFTorgASM
{
    partial class ConflictCheckerForm
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
            this.lbl_Patches = new System.Windows.Forms.Label();
            this.lv_Patches = new System.Windows.Forms.ListView();
            this.lv_Patches_columnHeader_PatchNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_Patches_columnHeader_File = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_Patches_columnHeader_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_Conflicts = new System.Windows.Forms.ListView();
            this.lv_Conflicts_columnHeader_PatchNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_Conflicts_columnHeader_Sector = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_Conflicts_columnHeader_WriteLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lv_Conflicts_columnHeader_ConflictWriteLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbl_Conflicts = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_Patches
            // 
            this.lbl_Patches.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Patches.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Patches.Location = new System.Drawing.Point(100, 9);
            this.lbl_Patches.Name = "lbl_Patches";
            this.lbl_Patches.Size = new System.Drawing.Size(392, 20);
            this.lbl_Patches.TabIndex = 2;
            this.lbl_Patches.Text = "Patches";
            this.lbl_Patches.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lv_Patches
            // 
            this.lv_Patches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lv_Patches.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lv_Patches_columnHeader_PatchNumber,
            this.lv_Patches_columnHeader_File,
            this.lv_Patches_columnHeader_Name});
            this.lv_Patches.FullRowSelect = true;
            this.lv_Patches.GridLines = true;
            this.lv_Patches.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_Patches.HideSelection = false;
            this.lv_Patches.LabelWrap = false;
            this.lv_Patches.Location = new System.Drawing.Point(12, 32);
            this.lv_Patches.MultiSelect = false;
            this.lv_Patches.Name = "lv_Patches";
            this.lv_Patches.Size = new System.Drawing.Size(480, 504);
            this.lv_Patches.TabIndex = 10;
            this.lv_Patches.UseCompatibleStateImageBehavior = false;
            this.lv_Patches.View = System.Windows.Forms.View.Details;
            this.lv_Patches.SelectedIndexChanged += new System.EventHandler(this.lv_Patches_SelectedIndexChanged);
            // 
            // lv_Patches_columnHeader_PatchNumber
            // 
            this.lv_Patches_columnHeader_PatchNumber.Text = "Patch #";
            this.lv_Patches_columnHeader_PatchNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lv_Patches_columnHeader_File
            // 
            this.lv_Patches_columnHeader_File.Text = "File";
            this.lv_Patches_columnHeader_File.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lv_Patches_columnHeader_File.Width = 100;
            // 
            // lv_Patches_columnHeader_Name
            // 
            this.lv_Patches_columnHeader_Name.Text = "Name";
            this.lv_Patches_columnHeader_Name.Width = 315;
            // 
            // lv_Conflicts
            // 
            this.lv_Conflicts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lv_Conflicts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lv_Conflicts_columnHeader_PatchNumber,
            this.lv_Conflicts_columnHeader_Sector,
            this.lv_Conflicts_columnHeader_WriteLocation,
            this.lv_Conflicts_columnHeader_ConflictWriteLocation});
            this.lv_Conflicts.FullRowSelect = true;
            this.lv_Conflicts.GridLines = true;
            this.lv_Conflicts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_Conflicts.HideSelection = false;
            this.lv_Conflicts.LabelWrap = false;
            this.lv_Conflicts.Location = new System.Drawing.Point(498, 32);
            this.lv_Conflicts.MultiSelect = false;
            this.lv_Conflicts.Name = "lv_Conflicts";
            this.lv_Conflicts.Size = new System.Drawing.Size(401, 504);
            this.lv_Conflicts.TabIndex = 11;
            this.lv_Conflicts.UseCompatibleStateImageBehavior = false;
            this.lv_Conflicts.View = System.Windows.Forms.View.Details;
            // 
            // lv_Conflicts_columnHeader_PatchNumber
            // 
            this.lv_Conflicts_columnHeader_PatchNumber.Text = "Patch #";
            this.lv_Conflicts_columnHeader_PatchNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lv_Conflicts_columnHeader_Sector
            // 
            this.lv_Conflicts_columnHeader_Sector.Text = "Sector";
            this.lv_Conflicts_columnHeader_Sector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lv_Conflicts_columnHeader_Sector.Width = 145;
            // 
            // lv_Conflicts_columnHeader_WriteLocation
            // 
            this.lv_Conflicts_columnHeader_WriteLocation.Text = "Location";
            this.lv_Conflicts_columnHeader_WriteLocation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lv_Conflicts_columnHeader_WriteLocation.Width = 95;
            // 
            // lv_Conflicts_columnHeader_ConflictWriteLocation
            // 
            this.lv_Conflicts_columnHeader_ConflictWriteLocation.Text = "Conflict Location";
            this.lv_Conflicts_columnHeader_ConflictWriteLocation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lv_Conflicts_columnHeader_ConflictWriteLocation.Width = 95;
            // 
            // lbl_Conflicts
            // 
            this.lbl_Conflicts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Conflicts.AutoSize = true;
            this.lbl_Conflicts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Conflicts.Location = new System.Drawing.Point(663, 9);
            this.lbl_Conflicts.Name = "lbl_Conflicts";
            this.lbl_Conflicts.Size = new System.Drawing.Size(70, 20);
            this.lbl_Conflicts.TabIndex = 13;
            this.lbl_Conflicts.Text = "Conflicts";
            // 
            // ConflictCheckerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 548);
            this.Controls.Add(this.lbl_Conflicts);
            this.Controls.Add(this.lv_Conflicts);
            this.Controls.Add(this.lv_Patches);
            this.Controls.Add(this.lbl_Patches);
            this.MinimumSize = new System.Drawing.Size(927, 587);
            this.Name = "ConflictCheckerForm";
            this.Text = "Conflict Checker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Patches;
        private System.Windows.Forms.ListView lv_Patches;
        private System.Windows.Forms.ListView lv_Conflicts;
        private System.Windows.Forms.ColumnHeader lv_Patches_columnHeader_PatchNumber;
        private System.Windows.Forms.ColumnHeader lv_Patches_columnHeader_File;
        private System.Windows.Forms.ColumnHeader lv_Patches_columnHeader_Name;
        private System.Windows.Forms.ColumnHeader lv_Conflicts_columnHeader_PatchNumber;
        private System.Windows.Forms.ColumnHeader lv_Conflicts_columnHeader_Sector;
        private System.Windows.Forms.ColumnHeader lv_Conflicts_columnHeader_WriteLocation;
        private System.Windows.Forms.ColumnHeader lv_Conflicts_columnHeader_ConflictWriteLocation;
        private System.Windows.Forms.Label lbl_Conflicts;
    }
}

