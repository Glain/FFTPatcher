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

namespace FFTPatcher.TextEditor
{
    partial class GlyphEditor
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
            if (disposing && (components != null))
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
            this.widthLabel = new System.Windows.Forms.Label();
            this.smallerThumbnailPanelPanel = new System.Windows.Forms.Panel();
            this.smallerThumbnailPanel = new System.Windows.Forms.Panel();
            this.thumbnailPanelPanel = new System.Windows.Forms.Panel();
            this.thumbnailPanel = new System.Windows.Forms.Panel();
            this.glyphPanelPanel = new System.Windows.Forms.Panel();
            this.glyphPanel = new System.Windows.Forms.Panel();
            this.smallerThumbnailPanelPanel.SuspendLayout();
            this.thumbnailPanelPanel.SuspendLayout();
            this.glyphPanelPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point( 165, 132 );
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size( 52, 13 );
            this.widthLabel.TabIndex = 5;
            this.widthLabel.Text = "Width {0}";
            // 
            // smallerThumbnailPanelPanel
            // 
            this.smallerThumbnailPanelPanel.AutoSize = true;
            this.smallerThumbnailPanelPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.smallerThumbnailPanelPanel.BackgroundImage = global::FFTPatcher.TextEditor.Properties.Resources.bg;
            this.smallerThumbnailPanelPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.smallerThumbnailPanelPanel.Controls.Add( this.smallerThumbnailPanel );
            this.smallerThumbnailPanelPanel.Location = new System.Drawing.Point( 164, 45 );
            this.smallerThumbnailPanelPanel.Name = "smallerThumbnailPanelPanel";
            this.smallerThumbnailPanelPanel.Size = new System.Drawing.Size( 18, 22 );
            this.smallerThumbnailPanelPanel.TabIndex = 10;
            // 
            // smallerThumbnailPanel
            // 
            this.smallerThumbnailPanel.BackColor = System.Drawing.Color.Transparent;
            this.smallerThumbnailPanel.Location = new System.Drawing.Point( 3, 3 );
            this.smallerThumbnailPanel.Name = "smallerThumbnailPanel";
            this.smallerThumbnailPanel.Size = new System.Drawing.Size( 10, 14 );
            this.smallerThumbnailPanel.TabIndex = 1;
            // 
            // thumbnailPanelPanel
            // 
            this.thumbnailPanelPanel.AutoSize = true;
            this.thumbnailPanelPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.thumbnailPanelPanel.BackgroundImage = global::FFTPatcher.TextEditor.Properties.Resources.bg;
            this.thumbnailPanelPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.thumbnailPanelPanel.Controls.Add( this.thumbnailPanel );
            this.thumbnailPanelPanel.Location = new System.Drawing.Point( 164, 3 );
            this.thumbnailPanelPanel.Name = "thumbnailPanelPanel";
            this.thumbnailPanelPanel.Size = new System.Drawing.Size( 28, 36 );
            this.thumbnailPanelPanel.TabIndex = 2;
            // 
            // thumbnailPanel
            // 
            this.thumbnailPanel.BackColor = System.Drawing.Color.Transparent;
            this.thumbnailPanel.Location = new System.Drawing.Point( 3, 3 );
            this.thumbnailPanel.Name = "thumbnailPanel";
            this.thumbnailPanel.Size = new System.Drawing.Size( 20, 28 );
            this.thumbnailPanel.TabIndex = 1;
            // 
            // glyphPanelPanel
            // 
            this.glyphPanelPanel.AutoSize = true;
            this.glyphPanelPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.glyphPanelPanel.BackgroundImage = global::FFTPatcher.TextEditor.Properties.Resources.bg;
            this.glyphPanelPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.glyphPanelPanel.Controls.Add( this.glyphPanel );
            this.glyphPanelPanel.Location = new System.Drawing.Point( 3, 3 );
            this.glyphPanelPanel.Name = "glyphPanelPanel";
            this.glyphPanelPanel.Size = new System.Drawing.Size( 160, 222 );
            this.glyphPanelPanel.TabIndex = 3;
            // 
            // glyphPanel
            // 
            this.glyphPanel.BackColor = System.Drawing.Color.Transparent;
            this.glyphPanel.Location = new System.Drawing.Point( 3, 5 );
            this.glyphPanel.MaximumSize = new System.Drawing.Size( 150, 210 );
            this.glyphPanel.MinimumSize = new System.Drawing.Size( 150, 210 );
            this.glyphPanel.Name = "glyphPanel";
            this.glyphPanel.Size = new System.Drawing.Size( 150, 210 );
            this.glyphPanel.TabIndex = 0;
            // 
            // GlyphEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add( this.smallerThumbnailPanelPanel );
            this.Controls.Add( this.widthLabel );
            this.Controls.Add( this.glyphPanelPanel );
            this.Controls.Add( this.thumbnailPanelPanel );
            this.Name = "GlyphEditor";
            this.Size = new System.Drawing.Size( 220, 228 );
            this.smallerThumbnailPanelPanel.ResumeLayout( false );
            this.thumbnailPanelPanel.ResumeLayout( false );
            this.glyphPanelPanel.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel glyphPanel;
        private System.Windows.Forms.Panel thumbnailPanel;
        private System.Windows.Forms.Panel glyphPanelPanel;
        private System.Windows.Forms.Panel smallerThumbnailPanel;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Panel smallerThumbnailPanelPanel;
        private System.Windows.Forms.Panel thumbnailPanelPanel;

    }
}
