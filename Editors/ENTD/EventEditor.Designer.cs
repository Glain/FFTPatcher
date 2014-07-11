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
    partial class EventEditor
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
            this.unitSelectorListBox = new FFTPatcher.Controls.ModifiedListBox();
            this.eventUnitEditor = new FFTPatcher.Editors.EventUnitEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // unitSelectorListBox
            // 
            this.unitSelectorListBox.DisplayMember = "Description";
            this.unitSelectorListBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.unitSelectorListBox.FormattingEnabled = true;
            this.unitSelectorListBox.IncludePrefix = false;
            this.unitSelectorListBox.Location = new System.Drawing.Point(0, 0);
            this.unitSelectorListBox.Name = "unitSelectorListBox";
            this.unitSelectorListBox.Size = new System.Drawing.Size(659, 212);
            this.unitSelectorListBox.TabIndex = 1;
            this.unitSelectorListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.unitSelectorListBox_KeyDown);
            // 
            // eventUnitEditor
            // 
            this.eventUnitEditor.AutoSize = true;
            this.eventUnitEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.eventUnitEditor.EventUnit = null;
            this.eventUnitEditor.Location = new System.Drawing.Point(0, 0);
            this.eventUnitEditor.Name = "eventUnitEditor";
            this.eventUnitEditor.Size = new System.Drawing.Size(581, 511);
            this.eventUnitEditor.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.eventUnitEditor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 212);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(659, 397);
            this.panel1.TabIndex = 2;
            // 
            // EventEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.unitSelectorListBox);
            this.Name = "EventEditor";
            this.Size = new System.Drawing.Size(659, 609);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private EventUnitEditor eventUnitEditor;
        private FFTPatcher.Controls.ModifiedListBox unitSelectorListBox;
        private System.Windows.Forms.Panel panel1;
    }
}
