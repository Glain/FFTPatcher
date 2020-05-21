namespace FFTorgASM
{
    partial class FreeSpaceForm
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
            this.Filelistbox = new System.Windows.Forms.ListBox();
            this.dgv_FreeSpace = new System.Windows.Forms.DataGridView();
            this.lbl_NumberOfPatchesText = new System.Windows.Forms.Label();
            this.lbl_NumberOfWritesText = new System.Windows.Forms.Label();
            this.lbl_NumberOfPatches = new System.Windows.Forms.Label();
            this.lbl_NumberOfWrites = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.btnMove = new System.Windows.Forms.Button();
            this.dgv_Column_Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Column_Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Column_Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Column_NextAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Column_SpaceToNext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Column_FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Column_PatchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FreeSpace)).BeginInit();
            this.SuspendLayout();
            // 
            // Filelistbox
            // 
            this.Filelistbox.FormattingEnabled = true;
            this.Filelistbox.Items.AddRange(new object[] {
            "BATTLE.BIN",
            "WORLD.BIN",
            "SCUS 1",
            "SCUS 2"});
            this.Filelistbox.Location = new System.Drawing.Point(12, 50);
            this.Filelistbox.Name = "Filelistbox";
            this.Filelistbox.Size = new System.Drawing.Size(86, 108);
            this.Filelistbox.TabIndex = 11;
            this.Filelistbox.SelectedIndexChanged += new System.EventHandler(this.Filelistbox_SelectedIndexChanged);
            // 
            // dgv_FreeSpace
            // 
            this.dgv_FreeSpace.AllowUserToAddRows = false;
            this.dgv_FreeSpace.AllowUserToDeleteRows = false;
            this.dgv_FreeSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_FreeSpace.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_FreeSpace.BackgroundColor = System.Drawing.Color.White;
            this.dgv_FreeSpace.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_FreeSpace.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_Column_Number,
            this.dgv_Column_Address,
            this.dgv_Column_Length,
            this.dgv_Column_NextAddress,
            this.dgv_Column_SpaceToNext,
            this.dgv_Column_FileName,
            this.dgv_Column_PatchName});
            this.dgv_FreeSpace.Location = new System.Drawing.Point(104, 50);
            this.dgv_FreeSpace.Name = "dgv_FreeSpace";
            this.dgv_FreeSpace.RowHeadersVisible = false;
            this.dgv_FreeSpace.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_FreeSpace.Size = new System.Drawing.Size(878, 489);
            this.dgv_FreeSpace.TabIndex = 12;
            this.dgv_FreeSpace.CellFormatting += dgv_FreeSpace_CellFormatting;
            this.dgv_FreeSpace.SelectionChanged += dgv_FreeSpace_SelectionChanged;
            // 
            // lbl_NumberOfPatchesText
            // 
            this.lbl_NumberOfPatchesText.AutoSize = true;
            this.lbl_NumberOfPatchesText.Location = new System.Drawing.Point(174, 9);
            this.lbl_NumberOfPatchesText.Name = "lbl_NumberOfPatchesText";
            this.lbl_NumberOfPatchesText.Size = new System.Drawing.Size(101, 13);
            this.lbl_NumberOfPatchesText.TabIndex = 13;
            this.lbl_NumberOfPatchesText.Text = "Number of Patches:";
            // 
            // lbl_NumberOfWritesText
            // 
            this.lbl_NumberOfWritesText.AutoSize = true;
            this.lbl_NumberOfWritesText.Location = new System.Drawing.Point(119, 29);
            this.lbl_NumberOfWritesText.Name = "lbl_NumberOfWritesText";
            this.lbl_NumberOfWritesText.Size = new System.Drawing.Size(156, 13);
            this.lbl_NumberOfWritesText.TabIndex = 13;
            this.lbl_NumberOfWritesText.Text = "Number of writes to current file: ";
            // 
            // lbl_NumberOfPatches
            // 
            this.lbl_NumberOfPatches.AutoSize = true;
            this.lbl_NumberOfPatches.Location = new System.Drawing.Point(289, 9);
            this.lbl_NumberOfPatches.Name = "lbl_NumberOfPatches";
            this.lbl_NumberOfPatches.Size = new System.Drawing.Size(16, 13);
            this.lbl_NumberOfPatches.TabIndex = 13;
            this.lbl_NumberOfPatches.Text = "...";
            // 
            // lbl_NumberOfWrites
            // 
            this.lbl_NumberOfWrites.AutoSize = true;
            this.lbl_NumberOfWrites.Location = new System.Drawing.Point(289, 29);
            this.lbl_NumberOfWrites.Name = "lbl_NumberOfWrites";
            this.lbl_NumberOfWrites.Size = new System.Drawing.Size(16, 13);
            this.lbl_NumberOfWrites.TabIndex = 13;
            this.lbl_NumberOfWrites.Text = "...";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(14, 169);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(83, 20);
            this.txtAddress.TabIndex = 14;
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(14, 198);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(82, 21);
            this.btnMove.TabIndex = 15;
            this.btnMove.Text = "Move";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += btnMove_Click;
            // 
            // dgv_Column_Number
            // 
            this.dgv_Column_Number.HeaderText = "Number";
            this.dgv_Column_Number.Name = "dgv_Column_Number";
            this.dgv_Column_Number.Width = 69;
            // 
            // dgv_Column_Address
            // 
            this.dgv_Column_Address.HeaderText = "Address";
            this.dgv_Column_Address.Name = "dgv_Column_Address";
            this.dgv_Column_Address.ReadOnly = true;
            this.dgv_Column_Address.Width = 70;
            // 
            // dgv_Column_Length
            // 
            this.dgv_Column_Length.HeaderText = "Length";
            this.dgv_Column_Length.Name = "dgv_Column_Length";
            this.dgv_Column_Length.ReadOnly = true;
            this.dgv_Column_Length.Width = 74;
            // 
            // dgv_Column_NextAddress
            // 
            this.dgv_Column_NextAddress.HeaderText = "Next Address";
            this.dgv_Column_NextAddress.Name = "dgv_Column_NextAddress";
            this.dgv_Column_NextAddress.Width = 120;
            // 
            // dgv_Column_SpaceToNext
            // 
            this.dgv_Column_SpaceToNext.HeaderText = "Space To Next Patch";
            this.dgv_Column_SpaceToNext.Name = "dgv_Column_SpaceToNext";
            this.dgv_Column_SpaceToNext.Width = 98;
            // 
            // dgv_Column_FileName
            // 
            this.dgv_Column_FileName.HeaderText = "File";
            this.dgv_Column_FileName.Name = "dgv_Column_FileName";
            this.dgv_Column_FileName.Width = 84;
            // 
            // dgv_Column_PatchName
            // 
            this.dgv_Column_PatchName.HeaderText = "Name";
            this.dgv_Column_PatchName.Name = "dgv_Column_PatchName";
            this.dgv_Column_PatchName.Width = 84;
            // 
            // FreeSpaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 554);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lbl_NumberOfWrites);
            this.Controls.Add(this.lbl_NumberOfWritesText);
            this.Controls.Add(this.lbl_NumberOfPatches);
            this.Controls.Add(this.lbl_NumberOfPatchesText);
            this.Controls.Add(this.dgv_FreeSpace);
            this.Controls.Add(this.Filelistbox);
            this.Name = "FreeSpaceForm";
            this.Text = "Free Space";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FreeSpace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox Filelistbox;
        private System.Windows.Forms.DataGridView dgv_FreeSpace;
        private System.Windows.Forms.Label lbl_NumberOfPatchesText;
        private System.Windows.Forms.Label lbl_NumberOfWritesText;
        private System.Windows.Forms.Label lbl_NumberOfPatches;
        private System.Windows.Forms.Label lbl_NumberOfWrites;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_Length;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_NextAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_SpaceToNext;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_PatchName;
    }
}