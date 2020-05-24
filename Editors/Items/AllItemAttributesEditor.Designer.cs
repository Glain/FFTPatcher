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
    partial class AllItemAttributesEditor
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
            this.offsetListBox = new FFTPatcher.Controls.EnhancedListBox();
        	this.itemAttributeEditor = new FFTPatcher.Editors.ItemAttributeEditor();
        	this.SuspendLayout();
        	// 
        	// offsetListBox
        	// 
        	this.offsetListBox.Dock = System.Windows.Forms.DockStyle.Left;
        	this.offsetListBox.FormattingEnabled = true;
        	this.offsetListBox.Location = new System.Drawing.Point(0, 0);
        	this.offsetListBox.Name = "offsetListBox";
        	this.offsetListBox.Size = new System.Drawing.Size(67, 449);
        	this.offsetListBox.TabIndex = 0;
        	this.offsetListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.offsetListBox_KeyDown);
        	// 
        	// itemAttributeEditor
        	// 
        	this.itemAttributeEditor.AutoScroll = true;
        	this.itemAttributeEditor.AutoSize = true;
        	this.itemAttributeEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.itemAttributeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.itemAttributeEditor.ItemAttributes = null;
        	this.itemAttributeEditor.Location = new System.Drawing.Point(67, 0);
        	this.itemAttributeEditor.Name = "itemAttributeEditor";
        	this.itemAttributeEditor.Size = new System.Drawing.Size(672, 449);
        	this.itemAttributeEditor.TabIndex = 1;
        	// 
        	// AllItemAttributesEditor
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.Controls.Add(this.itemAttributeEditor);
        	this.Controls.Add(this.offsetListBox);
        	this.Name = "AllItemAttributesEditor";
        	this.Size = new System.Drawing.Size(739, 449);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private FFTPatcher.Controls.EnhancedListBox offsetListBox;
        private ItemAttributeEditor itemAttributeEditor;
    }
}
