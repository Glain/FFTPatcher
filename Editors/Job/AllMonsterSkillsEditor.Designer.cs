/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace FFTPatcher.Editors
{
    partial class AllMonsterSkillsEditor
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
            if( disposing && (components != null) )
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
        	this.dataGridView = new System.Windows.Forms.DataGridView();
        	this.Offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.Ability1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
        	this.Ability2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
        	this.Ability3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
        	this.Beastmaster = new System.Windows.Forms.DataGridViewComboBoxColumn();
        	((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// dataGridView
        	// 
        	this.dataGridView.AllowUserToAddRows = false;
        	this.dataGridView.AllowUserToDeleteRows = false;
        	this.dataGridView.AllowUserToResizeRows = false;
        	this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
        	this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
        	this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        	this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
        	        	        	this.Offset,
        	        	        	this.NameColumn,
        	        	        	this.Ability1,
        	        	        	this.Ability2,
        	        	        	this.Ability3,
        	        	        	this.Beastmaster});
        	this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
        	this.dataGridView.Location = new System.Drawing.Point(0, 0);
        	this.dataGridView.MultiSelect = false;
        	this.dataGridView.Name = "dataGridView";
        	this.dataGridView.RowHeadersVisible = false;
        	this.dataGridView.Size = new System.Drawing.Size(292, 251);
        	this.dataGridView.TabIndex = 0;
        	// 
        	// Offset
        	// 
        	this.Offset.DataPropertyName = "Value";
        	this.Offset.Frozen = true;
        	this.Offset.HeaderText = "";
        	this.Offset.Name = "Offset";
        	this.Offset.ReadOnly = true;
        	this.Offset.Width = 19;
        	// 
        	// NameColumn
        	// 
        	this.NameColumn.DataPropertyName = "Name";
        	this.NameColumn.Frozen = true;
        	this.NameColumn.HeaderText = "";
        	this.NameColumn.Name = "NameColumn";
        	this.NameColumn.ReadOnly = true;
        	this.NameColumn.Width = 19;
        	// 
        	// Ability1
        	// 
        	this.Ability1.DataPropertyName = "Ability1";
        	this.Ability1.HeaderText = "Ability 1";
        	this.Ability1.MinimumWidth = 165;
        	this.Ability1.Name = "Ability1";
        	this.Ability1.Width = 165;
        	// 
        	// Ability2
        	// 
        	this.Ability2.DataPropertyName = "Ability2";
        	this.Ability2.HeaderText = "Ability 2";
        	this.Ability2.MinimumWidth = 165;
        	this.Ability2.Name = "Ability2";
        	this.Ability2.Width = 165;
        	// 
        	// Ability3
        	// 
        	this.Ability3.DataPropertyName = "Ability3";
        	this.Ability3.HeaderText = "Ability 3";
        	this.Ability3.MinimumWidth = 165;
        	this.Ability3.Name = "Ability3";
        	this.Ability3.Width = 165;
        	// 
        	// Beastmaster
        	// 
        	this.Beastmaster.DataPropertyName = "Beastmaster";
        	this.Beastmaster.HeaderText = "Beastmaster";
        	this.Beastmaster.MinimumWidth = 165;
        	this.Beastmaster.Name = "Beastmaster";
        	this.Beastmaster.Width = 165;
        	// 
        	// AllMonsterSkillsEditor
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.AutoSize = true;
        	this.Controls.Add(this.dataGridView);
        	this.Name = "AllMonsterSkillsEditor";
        	this.Size = new System.Drawing.Size(292, 251);
        	((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offset;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn Ability1;
        private System.Windows.Forms.DataGridViewComboBoxColumn Ability2;
        private System.Windows.Forms.DataGridViewComboBoxColumn Ability3;
        private System.Windows.Forms.DataGridViewComboBoxColumn Beastmaster;
    }
}
