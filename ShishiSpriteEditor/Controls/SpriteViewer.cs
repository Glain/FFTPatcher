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

using FFTPatcher.SpriteEditor.DataTypes;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace FFTPatcher.SpriteEditor
{
    public class SpriteViewer : UserControl
    {
		#region Fields (5) 

        private int palette = 0;
        private int portraitPalette = 8;
        private AbstractSprite sprite = null;
        private PictureBox pictureBox1;
        private Owf.Controls.Office2007ColorPicker office2007ColorPicker1;
        private ComboBox cmbZoom;
        private IList<Tile> tiles;
        private Zoom zoom;

		#endregion Fields 

		#region Properties (2) 

        public AbstractSprite Sprite
        {
            get { return sprite; }
            set
            {
                if( value != sprite )
                {
                    this.tiles = new Tile[0];
                    sprite = value;
                    UpdateImage();
                }
            }
        }


		#endregion Properties 

		#region Constructors (1) 

        public SpriteViewer()
        {
            pictureBox1 = new PictureBox();
            ( pictureBox1 as System.ComponentModel.ISupportInitialize ).BeginInit();
            SuspendLayout();
            Panel panel1 = new Panel();
            pictureBox1.Location = new Point( 0, 0 );
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size( 256, 50 );
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabStop = false;
            AutoScroll = false;
            Name = "SpriteViewer";
            Size = new Size( 256 + 10 + SystemInformation.VerticalScrollBarWidth, 100 );
            pictureBox1.MouseWheel += pictureBox1_MouseWheel;

            office2007ColorPicker1 = new Owf.Controls.Office2007ColorPicker();
            Controls.Add( office2007ColorPicker1 );
            office2007ColorPicker1.Color = Color.Black;
            office2007ColorPicker1.Location = new Point( 3, Height - office2007ColorPicker1.Height - 3 );
            office2007ColorPicker1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            panel1.AllowDrop = true;
            panel1.Bounds = new Rectangle( 0, 0, Size.Width, office2007ColorPicker1.Location.Y - 3 );
            panel1.AutoScroll = true;
            panel1.BackColor = Color.Black;
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            panel1.Controls.Add( pictureBox1 );
            panel1.DragDrop += new System.Windows.Forms.DragEventHandler(sprite_DragDrop);
            panel1.DragEnter += new System.Windows.Forms.DragEventHandler(sprite_DragEnter);
            Controls.Add( panel1 );

            cmbZoom = new ComboBox();
            Controls.Add(cmbZoom);
            for (int mult = 1; mult < 17; mult++)
            {
                cmbZoom.Items.Add(new Zoom(mult, ((mult * 100).ToString() + "%")));
            }
            cmbZoom.Size = new Size(75, 20); 
            cmbZoom.Location = new Point((Width - cmbZoom.Width - 3), (Height - cmbZoom.Height - 3));
            cmbZoom.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            cmbZoom.SelectedIndex = 0;
            cmbZoom.SelectedIndexChanged += new System.EventHandler(cmbZoom_SelectedIndexChanged);

            pictureBox1.BackColor = Color.Black;
            ( pictureBox1 as System.ComponentModel.ISupportInitialize ).EndInit();
            ResumeLayout( false );
            PerformLayout();
            office2007ColorPicker1.SelectedColorChanged += new System.EventHandler(office2007ColorPicker1_SelectedColorChanged);

            AutoSize = false;

            UpdateZoom();
        }

		#endregion Constructors 

        #region Event Handlers

        public event DragEventHandler SpriteDragEnter;
        protected virtual void OnSpriteDragEnter(DragEventArgs e)
        {
            DragEventHandler handler = SpriteDragEnter;
            if (handler != null)
                handler(this, e);
        }
        private void sprite_DragEnter(object sender, DragEventArgs e)
        {
            OnSpriteDragEnter(e);
        }

        public event DragEventHandler SpriteDragDrop;
        protected virtual void OnSpriteDragDrop(DragEventArgs e)
        {
            DragEventHandler handler = SpriteDragDrop;
            if (handler != null)
                handler(this, e);
        }
        private void sprite_DragDrop(object sender, DragEventArgs e)
        {
            OnSpriteDragDrop(e);
        }

        private void office2007ColorPicker1_SelectedColorChanged(object sender, System.EventArgs e)
        {
            pictureBox1.Parent.BackColor = office2007ColorPicker1.Color;
            pictureBox1.BackColor = office2007ColorPicker1.Color;
        }

        private void cmbZoom_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateZoom();
            UpdateImage();
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.Delta > 0)
                {
                    if (cmbZoom.SelectedIndex > 0)
                        cmbZoom.SelectedIndex--;
                }
                else
                {
                    if (cmbZoom.SelectedIndex < (cmbZoom.Items.Count - 1))
                        cmbZoom.SelectedIndex++;
                }

                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        #endregion

        #region Methods (5)

        public void HighlightTiles( IList<Tile> tiles )
        {
            this.tiles = tiles;
            UpdateImage();
        }

        public void SetPalette( int paletteId )
        {
            SetPalette( paletteId, paletteId );
        }

        public void SetPalette( int paletteId, int portraitPaletteId )
        {
            if( palette != paletteId || portraitPalette != portraitPaletteId )
            {
                palette = paletteId;
                portraitPalette = portraitPaletteId;
                UpdateImage();
            }
        }

        private void UpdateZoom()
        {
            zoom = (Zoom)cmbZoom.SelectedItem;
        }

        private void UpdateImage()
        {
            if ( sprite != null )
            {
                Bitmap b = new Bitmap( sprite.Width, sprite.Height, PixelFormat.Format32bppArgb );
                BitmapData bmd = b.LockBits( new Rectangle( Point.Empty, b.Size ), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb );
                sprite.DrawSprite( bmd, palette, portraitPalette );
                b.UnlockBits( bmd );
                if ( tiles != null )
                {
                    using ( Pen p = new Pen( Color.Yellow ) )
                    using ( Graphics g = Graphics.FromImage( b ) )
                    {
                        foreach ( Tile t in tiles )
                            g.DrawRectangle( p, t.Rectangle );
                    }
                }

                Bitmap zb = zoom.GetZoomedBitmap(b);

                SuspendLayout();
                pictureBox1.SuspendLayout();
                if ( pictureBox1.Image != null )
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }
                pictureBox1.Image = zb;
                //pictureBox1.Image = b;
                pictureBox1.ResumeLayout();
                ResumeLayout( false );
                PerformLayout();
            }           
        }



		#endregion Methods 

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SpriteViewer
            // 
            this.Name = "SpriteViewer";
        }

    }
}
