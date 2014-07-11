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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PatcherLib.Datatypes;

namespace FFTPatcher.TextEditor
{
    public partial class GlyphEditor : UserControl
    {
        #region Instance Variables (5)

        private static Dictionary<FontColor, Brush> colors = new Dictionary<FontColor, Brush>();
        private FontColor currentColor;
        private Glyph glyph;
        private bool ignoreChanges = false;

        #endregion Instance Variables

        #region Public Properties (1)

        public Glyph Glyph
        {
            get { return glyph; }
            set
            {
                if (glyph != value)
                {
                    glyph = value;
                    ignoreChanges = true;
                    widthLabel.Text = string.Format( "Width: {0}", glyph.Width );
                    ignoreChanges = false;
                    Invalidate( true );
                    glyphPanel.Invalidate();
                    thumbnailPanel.Invalidate();
                }
            }
        }

        #endregion Public Properties

        #region Constructors (2)

        public GlyphEditor()
        {
            InitializeComponent();

            glyphPanel.Paint += glyphPanel_Paint;
            thumbnailPanel.Paint += thumbnailPanel_Paint;
            smallerThumbnailPanel.Paint += smallerThumbnailPanel_Paint;
        }

        static GlyphEditor()
        {
            colors[FontColor.Black] = new SolidBrush( Color.FromArgb( 70, 63, 51 ) );
            colors[FontColor.Dark] = new SolidBrush( Color.FromArgb( 107, 104, 85 ) );
            colors[FontColor.Light] = new SolidBrush( Color.FromArgb( 120, 112, 96 ) );
            colors[FontColor.Transparent] = new SolidBrush( Color.Transparent );
        }

        #endregion Constructors

        #region Private Methods (7)
        private void glyphPanel_Paint( object sender, PaintEventArgs e )
        {
            if (Glyph != null)
            {
                if (e.ClipRectangle == e.Graphics.VisibleClipBounds)
                {
                    for (int i = 0; i < Glyph.Pixels.Length; i++)
                    {
                        int col = i % 10;
                        int row = i / 10;
                        RedrawPixel( col, row, Glyph.Pixels[i], e.Graphics );
                    }
                }
                else
                {
                    int col = e.ClipRectangle.Location.X / 15;
                    int row = e.ClipRectangle.Location.Y / 15;
                    RedrawPixel( col, row, Glyph.Pixels[row * 10 + col], e.Graphics );
                }

                using (Pen p = new Pen( Color.White ))
                {
                    e.Graphics.DrawLine( p,
                        new Point( (int)glyph.Width * 15, 0 ),
                        new Point( (int)glyph.Width * 15, (int)e.Graphics.VisibleClipBounds.Bottom ) );
                }
            }
        }

        private void RedrawPixel( int col, int row, FontColor color, Graphics g )
        {
            g.FillRectangle( colors[color], new Rectangle( col * 15, row * 15, 15, 15 ) );
        }

        private void smallerThumbnailPanel_Paint( object sender, PaintEventArgs e )
        {
            if (Glyph != null)
            {
                for (int i = 0; i < Glyph.Pixels.Length; i++)
                {
                    int col = i % 10;
                    int row = i / 10;
                    e.Graphics.FillRectangle( colors[Glyph.Pixels[row * 10 + col]], new Rectangle( col, row, 1, 1 ) );
                }
            }
        }

        private void thumbnailPanel_Paint( object sender, PaintEventArgs e )
        {
            if (Glyph != null)
            {
                if (e.ClipRectangle == e.Graphics.VisibleClipBounds)
                {
                    for (int i = 0; i < Glyph.Pixels.Length; i++)
                    {
                        int col = i % 10;
                        int row = i / 10;
                        e.Graphics.FillRectangle( colors[Glyph.Pixels[row * 10 + col]], new Rectangle( col * 2, row * 2, 2, 2 ) );
                    }
                }
                else
                {
                    int col = e.ClipRectangle.Location.X / 2;
                    int row = e.ClipRectangle.Location.Y / 2;
                    e.Graphics.FillRectangle( colors[Glyph.Pixels[row * 10 + col]], new Rectangle( col * 2, row * 2, 2, 2 ) );
                }
            }
        }

        #endregion Private Methods
    }
}
