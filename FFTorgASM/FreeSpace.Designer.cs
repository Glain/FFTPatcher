namespace FFTorgASM
{
    partial class FreeSpace
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_NumberOfPatches = new System.Windows.Forms.Label();
            this.lbl_NumberOfWrites = new System.Windows.Forms.Label();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.freewritelocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpaceToNext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patchname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FreeSpace)).BeginInit();
            this.SuspendLayout();
            // 
            // Filelistbox
            // 
            this.Filelistbox.FormattingEnabled = true;
            this.Filelistbox.Items.AddRange(new object[] {
            "Battle.Bin",
            "World.Bin"});
            this.Filelistbox.Location = new System.Drawing.Point(12, 50);
            this.Filelistbox.Name = "Filelistbox";
            this.Filelistbox.Size = new System.Drawing.Size(86, 108);
            this.Filelistbox.TabIndex = 11;
            this.Filelistbox.SelectedIndexChanged += new System.EventHandler(this.Filelistbox_SelectedIndexChanged);
            // 
            // dgv_FreeSpace
            // 
            this.dgv_FreeSpace.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_FreeSpace.BackgroundColor = System.Drawing.Color.White;
            this.dgv_FreeSpace.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_FreeSpace.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Address,
            this.Length,
            this.freewritelocation,
            this.SpaceToNext,
            this.patchname,
            this.number});
            this.dgv_FreeSpace.Location = new System.Drawing.Point(104, 50);
            this.dgv_FreeSpace.Name = "dgv_FreeSpace";
            this.dgv_FreeSpace.RowHeadersVisible = false;
            this.dgv_FreeSpace.Size = new System.Drawing.Size(836, 489);
            this.dgv_FreeSpace.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(174, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Number of Patches:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(119, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Number of writes to current file: ";
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
            // Address
            // 
            this.Address.HeaderText = "Address";
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            this.Address.Width = 70;
            // 
            // Length
            // 
            this.Length.HeaderText = "Length of Patch";
            this.Length.Name = "Length";
            this.Length.ReadOnly = true;
            this.Length.Width = 74;
            // 
            // freewritelocation
            // 
            this.freewritelocation.HeaderText = "Free Space Start Address";
            this.freewritelocation.Name = "freewritelocation";
            this.freewritelocation.Width = 105;
            // 
            // SpaceToNext
            // 
            this.SpaceToNext.HeaderText = "Space To Next Patch";
            this.SpaceToNext.Name = "SpaceToNext";
            this.SpaceToNext.Width = 98;
            // 
            // patchname
            // 
            this.patchname.HeaderText = "Patch Name";
            this.patchname.Name = "patchname";
            this.patchname.Width = 84;
            // 
            // number
            // 
            this.number.HeaderText = "Number";
            this.number.Name = "number";
            this.number.Width = 69;
            // 
            // FreeSpace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 554);
            this.Controls.Add(this.lbl_NumberOfWrites);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_NumberOfPatches);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_FreeSpace);
            this.Controls.Add(this.Filelistbox);
            this.Name = "FreeSpace";
            this.Text = "FreeSpace";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FreeSpace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox Filelistbox;
        private System.Windows.Forms.DataGridView dgv_FreeSpace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_NumberOfPatches;
        private System.Windows.Forms.Label lbl_NumberOfWrites;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Length;
        private System.Windows.Forms.DataGridViewTextBoxColumn freewritelocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn SpaceToNext;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchname;
        private System.Windows.Forms.DataGridViewTextBoxColumn number;
    }
}