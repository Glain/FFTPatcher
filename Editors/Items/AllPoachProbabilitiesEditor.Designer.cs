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
    partial class AllPoachProbabilitiesEditor
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
        	this.Monster = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.CommonItem = new System.Windows.Forms.DataGridViewComboBoxColumn();
        	this.UncommonItem = new System.Windows.Forms.DataGridViewComboBoxColumn();
        	((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// dataGridView
        	// 
        	this.dataGridView.AllowUserToAddRows = false;
        	this.dataGridView.AllowUserToDeleteRows = false;
        	this.dataGridView.AllowUserToResizeColumns = false;
        	this.dataGridView.AllowUserToResizeRows = false;
        	this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
        	this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
        	this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        	this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
        	        	        	this.Monster,
        	        	        	this.CommonItem,
        	        	        	this.UncommonItem});
        	this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
        	this.dataGridView.Location = new System.Drawing.Point(0, 0);
        	this.dataGridView.Name = "dataGridView";
        	this.dataGridView.RowHeadersVisible = false;
        	this.dataGridView.Size = new System.Drawing.Size(567, 327);
        	this.dataGridView.TabIndex = 0;
        	// 
        	// Monster
        	// 
        	this.Monster.DataPropertyName = "MonsterName";
        	this.Monster.Frozen = true;
        	this.Monster.HeaderText = "Monster";
        	this.Monster.Name = "Monster";
        	this.Monster.ReadOnly = true;
        	this.Monster.Width = 70;
        	// 
        	// CommonItem
        	// 
        	this.CommonItem.DataPropertyName = "Common";
        	this.CommonItem.HeaderText = "Common Item (~88%)";
        	this.CommonItem.Name = "CommonItem";
        	this.CommonItem.Width = 72;
        	// 
        	// UncommonItem
        	// 
        	this.UncommonItem.DataPropertyName = "Uncommon";
        	this.UncommonItem.HeaderText = "Uncommon Item (~12%)";
        	this.UncommonItem.Name = "UncommonItem";
        	this.UncommonItem.Width = 84;
        	// 
        	// AllPoachProbabilitiesEditor
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.Controls.Add(this.dataGridView);
        	this.Name = "AllPoachProbabilitiesEditor";
        	this.Size = new System.Drawing.Size(567, 327);
        	((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Monster;
        private System.Windows.Forms.DataGridViewComboBoxColumn CommonItem;
        private System.Windows.Forms.DataGridViewComboBoxColumn UncommonItem;
    }
}
