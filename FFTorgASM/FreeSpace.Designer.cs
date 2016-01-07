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
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FreeSpace)).BeginInit();
            this.SuspendLayout();
            // 
            // Filelistbox
            // 
            this.Filelistbox.FormattingEnabled = true;
            this.Filelistbox.Items.AddRange(new object[] {
            "Battle.Bin",
            "World.Bin"});
            this.Filelistbox.Location = new System.Drawing.Point(113, 50);
            this.Filelistbox.Name = "Filelistbox";
            this.Filelistbox.Size = new System.Drawing.Size(86, 108);
            this.Filelistbox.TabIndex = 11;
            this.Filelistbox.SelectedIndexChanged += new System.EventHandler(this.Filelistbox_SelectedIndexChanged);
            // 
            // dgv_FreeSpace
            // 
            this.dgv_FreeSpace.BackgroundColor = System.Drawing.Color.White;
            this.dgv_FreeSpace.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_FreeSpace.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Address,
            this.Length});
            this.dgv_FreeSpace.Location = new System.Drawing.Point(205, 50);
            this.dgv_FreeSpace.Name = "dgv_FreeSpace";
            this.dgv_FreeSpace.RowHeadersVisible = false;
            this.dgv_FreeSpace.Size = new System.Drawing.Size(453, 489);
            this.dgv_FreeSpace.TabIndex = 12;
            // 
            // Address
            // 
            this.Address.HeaderText = "Address";
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            // 
            // Length
            // 
            this.Length.HeaderText = "Free Space (length)";
            this.Length.Name = "Length";
            this.Length.ReadOnly = true;
            // 
            // FreeSpace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 551);
            this.Controls.Add(this.dgv_FreeSpace);
            this.Controls.Add(this.Filelistbox);
            this.Name = "FreeSpace";
            this.Text = "FreeSpace";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FreeSpace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Filelistbox;
        private System.Windows.Forms.DataGridView dgv_FreeSpace;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Length;
    }
}