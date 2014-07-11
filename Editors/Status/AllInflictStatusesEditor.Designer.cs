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
    partial class AllInflictStatusesEditor
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
            this.offsetListBox = new System.Windows.Forms.ListBox();
            this.inflictStatusEditor = new FFTPatcher.Editors.InflictStatusEditor();
            panel = new System.Windows.Forms.Panel();
            panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // offsetListBox
            // 
            this.offsetListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.offsetListBox.FormattingEnabled = true;
            this.offsetListBox.Location = new System.Drawing.Point( 0, 0 );
            this.offsetListBox.Name = "offsetListBox";
            this.offsetListBox.Size = new System.Drawing.Size( 46, 433 );
            this.offsetListBox.TabIndex = 1;
            // 
            // inflictStatusEditor
            // 
            this.inflictStatusEditor.AutoSize = true;
            this.inflictStatusEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.inflictStatusEditor.InflictStatus = null;
            this.inflictStatusEditor.Location = new System.Drawing.Point( 0, 0 );
            this.inflictStatusEditor.Name = "inflictStatusEditor";
            this.inflictStatusEditor.Size = new System.Drawing.Size( 601, 188 );
            this.inflictStatusEditor.TabIndex = 0;
            // 
            // panel
            // 
            panel.AutoScroll = true;
            panel.Controls.Add( this.inflictStatusEditor );
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            panel.Location = new System.Drawing.Point( 46, 0 );
            panel.Name = "panel";
            panel.Size = new System.Drawing.Size( 692, 437 );
            panel.TabIndex = 2;
            // 
            // AllInflictStatusesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( panel );
            this.Controls.Add( this.offsetListBox );
            this.Name = "AllInflictStatusesEditor";
            this.Size = new System.Drawing.Size( 738, 437 );
            panel.ResumeLayout( false );
            panel.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private InflictStatusEditor inflictStatusEditor;
        private System.Windows.Forms.ListBox offsetListBox;
    }
}
