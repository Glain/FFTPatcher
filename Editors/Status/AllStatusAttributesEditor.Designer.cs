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
    partial class AllStatusAttributesEditor
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
            this.listBox = new FFTPatcher.Controls.EnhancedListBox();
        	this.statusAttributeEditor = new FFTPatcher.Editors.StatusAttributeEditor();
        	this.SuspendLayout();
        	// 
        	// listBox
        	// 
        	this.listBox.Dock = System.Windows.Forms.DockStyle.Left;
        	this.listBox.FormattingEnabled = true;
        	this.listBox.Location = new System.Drawing.Point(0, 0);
        	this.listBox.Name = "listBox";
        	this.listBox.Size = new System.Drawing.Size(120, 513);
        	this.listBox.TabIndex = 0;
        	// 
        	// statusAttributeEditor
        	// 
        	this.statusAttributeEditor.AutoScroll = true;
        	this.statusAttributeEditor.AutoSize = true;
        	this.statusAttributeEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.statusAttributeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.statusAttributeEditor.Location = new System.Drawing.Point(120, 0);
        	this.statusAttributeEditor.Name = "statusAttributeEditor";
        	this.statusAttributeEditor.Size = new System.Drawing.Size(523, 513);
        	this.statusAttributeEditor.StatusAttribute = null;
        	this.statusAttributeEditor.TabIndex = 1;
        	// 
        	// AllStatusAttributesEditor
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.Controls.Add(this.statusAttributeEditor);
        	this.Controls.Add(this.listBox);
        	this.Name = "AllStatusAttributesEditor";
        	this.Size = new System.Drawing.Size(643, 513);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private FFTPatcher.Controls.EnhancedListBox listBox;
        private StatusAttributeEditor statusAttributeEditor;
    }
}
