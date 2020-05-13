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
    partial class AllItemsEditor
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
        	System.Windows.Forms.Panel panel;
        	this.itemEditor = new FFTPatcher.Editors.ItemEditor();
            this.itemListBox = new FFTPatcher.Controls.EnhancedListBox();
        	panel = new System.Windows.Forms.Panel();
        	panel.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// panel
        	// 
        	panel.AutoScroll = true;
        	panel.Controls.Add(this.itemEditor);
        	panel.Dock = System.Windows.Forms.DockStyle.Fill;
        	panel.Location = new System.Drawing.Point(134, 0);
        	panel.Name = "panel";
        	panel.Size = new System.Drawing.Size(569, 445);
        	panel.TabIndex = 2;
        	// 
        	// itemEditor
        	// 
        	this.itemEditor.AutoSize = true;
        	this.itemEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.itemEditor.Item = null;
        	this.itemEditor.Location = new System.Drawing.Point(0, 0);
        	this.itemEditor.Name = "itemEditor";
        	this.itemEditor.Size = new System.Drawing.Size(493, 591);
        	this.itemEditor.TabIndex = 1;
        	// 
        	// itemListBox
        	// 
        	this.itemListBox.Dock = System.Windows.Forms.DockStyle.Left;
        	this.itemListBox.FormattingEnabled = true;
        	this.itemListBox.Location = new System.Drawing.Point(0, 0);
        	this.itemListBox.Name = "itemListBox";
        	this.itemListBox.Size = new System.Drawing.Size(134, 445);
        	this.itemListBox.TabIndex = 0;
        	// 
        	// AllItemsEditor
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.Controls.Add(panel);
        	this.Controls.Add(this.itemListBox);
        	this.Name = "AllItemsEditor";
        	this.Size = new System.Drawing.Size(703, 445);
        	panel.ResumeLayout(false);
        	panel.PerformLayout();
        	this.ResumeLayout(false);
        }

        #endregion

        private FFTPatcher.Controls.EnhancedListBox itemListBox;
        private ItemEditor itemEditor;
    }
}
