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
    partial class ENTDEditor
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
            this.eventEditor1 = new FFTPatcher.Editors.EventEditor();
            this.eventListBox = new FFTPatcher.Controls.EnhancedListBox();
            this.SuspendLayout();
            // 
            // eventEditor1
            // 
            this.eventEditor1.AutoScroll = true;
            this.eventEditor1.AutoSize = true;
            this.eventEditor1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.eventEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventEditor1.Event = null;
            this.eventEditor1.Location = new System.Drawing.Point(266, 0);
            this.eventEditor1.Name = "eventEditor1";
            this.eventEditor1.Size = new System.Drawing.Size(601, 541);
            this.eventEditor1.TabIndex = 1;
            // 
            // eventListBox
            // 
            this.eventListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.eventListBox.FormattingEnabled = true;
            this.eventListBox.IncludePrefix = false;
            this.eventListBox.Location = new System.Drawing.Point(0, 0);
            this.eventListBox.Name = "eventListBox";
            this.eventListBox.Size = new System.Drawing.Size(266, 541);
            this.eventListBox.TabIndex = 0;
            this.eventListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.eventListBox_KeyDown);
            // 
            // ENTDEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventEditor1);
            this.Controls.Add(this.eventListBox);
            this.Name = "ENTDEditor";
            this.Size = new System.Drawing.Size(867, 541);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FFTPatcher.Controls.EnhancedListBox eventListBox;
        private EventEditor eventEditor1;
    }
}
