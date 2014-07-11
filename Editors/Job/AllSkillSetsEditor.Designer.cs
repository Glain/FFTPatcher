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
    partial class AllSkillSetsEditor
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
            System.Windows.Forms.Panel panel;
            this.skillSetListBox = new FFTPatcher.Controls.ModifiedListBox();
            this.skillSetEditor = new FFTPatcher.Editors.SkillSetEditor();
            panel = new System.Windows.Forms.Panel();
            panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // skillSetListBox
            // 
            this.skillSetListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.skillSetListBox.FormattingEnabled = true;
            this.skillSetListBox.Location = new System.Drawing.Point( 0, 0 );
            this.skillSetListBox.Name = "skillSetListBox";
            this.skillSetListBox.Size = new System.Drawing.Size( 192, 420 );
            this.skillSetListBox.TabIndex = 0;
            this.skillSetListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.skillSetListBox_KeyDown);
            // 
            // skillSetEditor
            // 
            this.skillSetEditor.AutoSize = true;
            this.skillSetEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.skillSetEditor.Location = new System.Drawing.Point( 0, 0 );
            this.skillSetEditor.Name = "skillSetEditor";
            this.skillSetEditor.Size = new System.Drawing.Size( 348, 380 );
            this.skillSetEditor.SkillSet = null;
            this.skillSetEditor.TabIndex = 1;
            // 
            // panel
            // 
            panel.AutoScroll = true;
            panel.Controls.Add( this.skillSetEditor );
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            panel.Location = new System.Drawing.Point( 192, 0 );
            panel.Name = "panel";
            panel.Size = new System.Drawing.Size( 422, 426 );
            panel.TabIndex = 2;
            // 
            // AllSkillSetsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( panel );
            this.Controls.Add( this.skillSetListBox );
            this.Name = "AllSkillSetsEditor";
            this.Size = new System.Drawing.Size( 614, 426 );
            panel.ResumeLayout( false );
            panel.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private FFTPatcher.Controls.ModifiedListBox skillSetListBox;
        private SkillSetEditor skillSetEditor;
    }
}
