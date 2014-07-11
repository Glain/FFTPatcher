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
using System.Drawing.Imaging;


namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute
    {

    }

}

namespace FFTPatcher.SpriteEditor
{
    /// <summary>
    /// Extension methods for various types.
    /// </summary>
    public static class ExtensionMethods
    {

        #region Static Fields (1)

        private static readonly Rectangle portraitRectangle = new Rectangle( 80, 256, 48, 32 );

        #endregion Static Fields

        #region Methods (5)

        public static void CopyRectangleToPointNonIndexed( this Bitmap source, Rectangle sourceRectangle, Bitmap destination, Point destinationPoint, Palette sourcePalette, bool flip )
        {
            BitmapData bmdSource = source.LockBits( new Rectangle( 0, 0, source.Width, source.Height ), ImageLockMode.ReadOnly, source.PixelFormat );
            BitmapData bmdDest = destination.LockBits( new Rectangle( 0, 0, destination.Width, destination.Height ), ImageLockMode.WriteOnly, destination.PixelFormat );
            if (flip)
            {
                for (int col = 0; col < sourceRectangle.Width; col++)
                {
                    for (int row = 0; row < sourceRectangle.Height; row++)
                    {
                        int index = bmdSource.GetPixel( col + sourceRectangle.X, row + sourceRectangle.Y );
                        Color c = sourcePalette.Colors[index % 16];
                        if (c.A != 0)
                        {
                            bmdDest.SetPixel32bpp( destinationPoint.X + (sourceRectangle.Width - col - 1), destinationPoint.Y + row, c );
                        }
                    }
                }
            }
            else
            {
                for (int col = 0; col < sourceRectangle.Width; col++)
                {
                    for (int row = 0; row < sourceRectangle.Height; row++)
                    {
                        int index = bmdSource.GetPixel( col + sourceRectangle.X, row + sourceRectangle.Y );
                        Color c = sourcePalette.Colors[index % 16];

                        if (c.A != 0)
                        {
                            bmdDest.SetPixel32bpp( destinationPoint.X + col, destinationPoint.Y + row, c );
                        }
                    }
                }
            }
            source.UnlockBits( bmdSource );
            destination.UnlockBits( bmdDest );
        }

        private delegate int CalcOffset( int rowOrColumn );

        /// <summary>
        /// Copies the rectangle to point.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="destinationPoint">The destination point.</param>
        /// <param name="flip">if set to <c>true</c> [flip].</param>
        public static void CopyRectangleToPoint( this Bitmap source, Rectangle sourceRectangle, Bitmap destination, Point destinationPoint, Palette palette, bool reverseX, bool reverseY )
        {
            BitmapData bmdSource = source.LockBits( new Rectangle( 0, 0, source.Width, source.Height ), ImageLockMode.ReadOnly, source.PixelFormat );
            BitmapData bmdDest = destination.LockBits( new Rectangle( 0, 0, destination.Width, destination.Height ), ImageLockMode.WriteOnly, destination.PixelFormat );

            int width = sourceRectangle.Width;
            int height = sourceRectangle.Height;
            int x = destinationPoint.X;
            int y = destinationPoint.Y;
            CalcOffset calcX = reverseX ?
                (CalcOffset)(col => (width - col - 1)) :
                (CalcOffset)(col => col);
            CalcOffset calcY = reverseY ?
                (CalcOffset)(row => (height - row - 1)) :
                (CalcOffset)(row => row);

            for (int col = 0; col < sourceRectangle.Width; col++)
            {
                for (int row = 0; row < sourceRectangle.Height; row++)
                {
                    int index = bmdSource.GetPixel( col + sourceRectangle.X, row + sourceRectangle.Y );
                    if (palette.Colors[index % 16].A != 0)
                    {
                        bmdDest.SetPixel8bpp(
                            x + calcX( col ),
                            y + calcY( row ),
                            index );
                    }
                }
            }

            source.UnlockBits( bmdSource );
            destination.UnlockBits( bmdDest );
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object to draw on.</param>
        public static void DrawSprite( this Graphics g, AbstractSprite s, int palette, int portrait )
        {
            using (Bitmap b = new Bitmap( s.Width, s.Height ))
            {
                b.DrawSprite( s, palette, portrait );
                g.DrawImage( b, 0, 0 );
            }
        }

        public static Bitmap CropImage( this Bitmap b, Rectangle cropRectangle )
        {
            Bitmap result = new Bitmap( b.Width, b.Height, b.PixelFormat );

            int xOffset = cropRectangle.X;
            int yOffset = cropRectangle.Y;
            int width = cropRectangle.Width;
            int height = cropRectangle.Height;

            if (xOffset + width > b.Width)
                throw new System.ArgumentException( "cropRectangle too wide", "cropRectangle" );
            if (yOffset + height > b.Height)
                throw new System.ArgumentException( "cropRectangle too tall", "cropRectangle" );


            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result.SetPixel( x, y, b.GetPixel( x + xOffset, y + yOffset ) );
                }
            }

            return result;
        }

        /// <summary>
        /// Draws a sprite.
        /// </summary>
        /// <param name="b">The <see cref="Bitmap"/> object to draw on.</param>
        /// <param name="s">The <see cref="Sprite"/> to draw.</param>
        /// <param name="p">The <see cref="Palette"/> to use to draw the sprite.</param>
        public static void DrawSprite( this Bitmap b, AbstractSprite s, int palette, int portrait )
        {
            s.DrawSprite( b, palette, portrait );
        }

        /// <summary>
        /// Sets a pixel in this bitmap to a specified value.
        /// </summary>
        /// <param name="bmd">The bitmap data.</param>
        /// <param name="x">The x position of the pixel.</param>
        /// <param name="y">The y position of the pixel.</param>
        /// <param name="index">The index in the palette to use to set the pixel to.</param>
        public static unsafe void SetPixel8bpp( this BitmapData bmd, int x, int y, int index )
        {
            byte* p = (byte*)bmd.Scan0.ToPointer();
            int offset = y * bmd.Stride + x;
            p[offset] = (byte)index;
        }

        public static unsafe void SetPixel32bpp( this BitmapData bmd, int x, int y, Color color )
        {
            byte* p = (byte*)bmd.Scan0.ToPointer();
            int offset = y * bmd.Stride + x * 4;
            p[offset] = color.B;
            p[offset + 1] = color.G;
            p[offset + 2] = color.R;
            p[offset + 3] = color.A;
        }

        public static unsafe void SetPixel4bpp( this BitmapData bmd, int x, int y, int index )
        {
            System.Diagnostics.Debug.Assert( bmd.PixelFormat == PixelFormat.Format4bppIndexed );
            int offset = y * bmd.Stride + x / 2;
            byte* p = (byte*)bmd.Scan0.ToPointer();
            byte currentByte = p[offset];
            if ((x & 1) == 1)
            {
                currentByte &= 0xF0;
                currentByte |= (byte)(index & 0x0F);
            }
            else
            {
                currentByte &= 0x0F;
                currentByte |= (byte)((index & 0x0F) << 4);
            }

            p[offset] = currentByte;
        }

        /// <summary>
        /// Gets a pixel in this bitmap.
        /// </summary>
        /// <param name="bmd">The bitmap data.</param>
        /// <param name="x">The x position of the pixel.</param>
        /// <param name="y">The y position of the pixel.</param>
        /// <returns>The palette index of the pixel.</returns>
        public static unsafe int GetPixel( this BitmapData bmd, int x, int y )
        {
            byte* p = (byte*)bmd.Scan0.ToPointer();
            int offset = y * bmd.Stride + x;
            return p[offset];
        }

        public static unsafe Color GetPixel32bpp( this BitmapData bmd, int x, int y )
        {
            byte* p = (byte*)bmd.Scan0.ToPointer();
            int offset = y * bmd.Stride + x * 4;
            return Color.FromArgb( p[offset + 3], p[offset + 2], p[offset + 1], p[offset] );
        }


        public static Image ToImage( this IList<IList<Color>> colors )
        {
            Bitmap b = new Bitmap( colors.Count, colors[0].Count );
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    b.SetPixel( x, y, colors[x][y] );
                }
            }

            return b;
        }

        #endregion Methods

    }
}
