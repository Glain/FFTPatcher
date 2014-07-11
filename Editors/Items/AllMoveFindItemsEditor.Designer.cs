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
    partial class AllMoveFindItemsEditor
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
            if ( disposing && ( components != null ) )
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
            this.mapMoveFindItemEditor1 = new FFTPatcher.Editors.MapMoveFindItemEditor();
            this.mapListBox = new FFTPatcher.Controls.ModifiedListBox();
            panel = new System.Windows.Forms.Panel();
            panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            panel.AutoScroll = true;
            panel.Controls.Add( this.mapMoveFindItemEditor1 );
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            panel.Location = new System.Drawing.Point( 183, 0 );
            panel.Name = "panel";
            panel.Size = new System.Drawing.Size( 386, 689 );
            panel.TabIndex = 4;
            // 
            // mapMoveFindItemEditor1
            // 
            this.mapMoveFindItemEditor1.AutoSize = true;
            this.mapMoveFindItemEditor1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mapMoveFindItemEditor1.Location = new System.Drawing.Point( 6, 3 );
            this.mapMoveFindItemEditor1.MapMoveFindItems = null;
            this.mapMoveFindItemEditor1.Name = "mapMoveFindItemEditor1";
            this.mapMoveFindItemEditor1.Size = new System.Drawing.Size( 373, 680 );
            this.mapMoveFindItemEditor1.TabIndex = 0;
            // 
            // mapListBox
            // 
            this.mapListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.mapListBox.FormattingEnabled = true;
            this.mapListBox.Location = new System.Drawing.Point( 0, 0 );
            this.mapListBox.Name = "mapListBox";
            this.mapListBox.Size = new System.Drawing.Size( 183, 680 );
            this.mapListBox.TabIndex = 3;
            // 
            // AllMoveFindItemsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( panel );
            this.Controls.Add( this.mapListBox );
            this.Name = "AllMoveFindItemsEditor";
            this.Size = new System.Drawing.Size( 569, 689 );
            panel.ResumeLayout( false );
            panel.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private FFTPatcher.Controls.ModifiedListBox mapListBox;
        private MapMoveFindItemEditor mapMoveFindItemEditor1;
    }
}
