namespace FFTPatcher.Editors
{
    partial class AllAnimationsEditor
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.IndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AbilityNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Byte1Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Byte2Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Byte3Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IndexColumn,
            this.AbilityNameColumn,
            this.Byte1Column,
            this.Byte2Column,
            this.Byte3Column});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(967, 507);
            this.dataGridView1.TabIndex = 0;
            // 
            // IndexColumn
            // 
            this.IndexColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.IndexColumn.DataPropertyName = "Index";
            this.IndexColumn.Frozen = true;
            this.IndexColumn.HeaderText = "";
            this.IndexColumn.Name = "IndexColumn";
            this.IndexColumn.ReadOnly = true;
            this.IndexColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.IndexColumn.Width = 19;
            // 
            // AbilityNameColumn
            // 
            this.AbilityNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AbilityNameColumn.DataPropertyName = "Name";
            this.AbilityNameColumn.Frozen = true;
            this.AbilityNameColumn.HeaderText = "";
            this.AbilityNameColumn.Name = "AbilityNameColumn";
            this.AbilityNameColumn.Width = 19;
            // 
            // Byte1Column
            // 
            this.Byte1Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Byte1Column.DataPropertyName = "Byte1";
            this.Byte1Column.Frozen = true;
            this.Byte1Column.HeaderText = "";
            this.Byte1Column.MaxInputLength = 2;
            this.Byte1Column.Name = "Byte1Column";
            this.Byte1Column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Byte1Column.Width = 19;
            // 
            // Byte2Column
            // 
            this.Byte2Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Byte2Column.DataPropertyName = "Byte2";
            this.Byte2Column.Frozen = true;
            this.Byte2Column.HeaderText = "";
            this.Byte2Column.MaxInputLength = 2;
            this.Byte2Column.Name = "Byte2Column";
            this.Byte2Column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Byte2Column.Width = 19;
            // 
            // Byte3Column
            // 
            this.Byte3Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Byte3Column.DataPropertyName = "Byte3";
            this.Byte3Column.Frozen = true;
            this.Byte3Column.HeaderText = "";
            this.Byte3Column.MaxInputLength = 2;
            this.Byte3Column.Name = "Byte3Column";
            this.Byte3Column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Byte3Column.Width = 19;
            // 
            // AllAnimationsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "AllAnimationsEditor";
            this.Size = new System.Drawing.Size(967, 507);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AbilityNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Byte1Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn Byte2Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn Byte3Column;
    }
}
