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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using PatcherLib.Utilities;
using System.Text;
using FFTPatcher.SpriteEditor.Helpers;

namespace FFTPatcher.SpriteEditor
{
    /// <summary>
    /// A FFT sprite.
    /// </summary>
    public abstract class AbstractSprite
    {

        #region Properties (6)

        public int OriginalSize { get; private set; }

        private Palette[] palettes;

        /// <summary>
        /// Gets or sets the palettes used to draw this sprite.
        /// </summary>
        public Palette[] Palettes 
        {
            get { return palettes; }
            set
            {
                palettes = value;
                BitmapDirty = true;
             }
         }
 
         /// <summary>
         /// Gets the pixels used to draw this sprite.
         /// </summary>
        public IList<byte> Pixels { get; private set; }

        public virtual int Width { get { return 256; } }
        public abstract int Height { get; }
        public string Name { get; private set; }

        protected abstract Rectangle PortraitRectangle { get; }
        
        public virtual Shape Shape { get { return null; } }

        protected bool BitmapDirty { get; set; }
        protected Bitmap CachedBitmap { get; set; }

        public void DrawSprite( Bitmap b, int palette, int portraitPalette )
        {
            DrawSpriteInternal( palette, portraitPalette, ( x, y, z ) => b.SetPixel( x, y, z ) );
        }

        public void DrawSprite( BitmapData bmd, int palette, int portraitPalette )
        {
            DrawSpriteInternal( palette, portraitPalette, ( x, y, z ) => bmd.SetPixel32bpp( x, y, z ) );
        }

        protected abstract void DrawSpriteInternal( int palette, int portraitPalette, SetPixel setPixel );

        protected delegate void SetPixel( int x, int y, Color color );

        public IList<string> Filenames { get; private set; }

        #endregion Properties

        #region Constructors (1)

        internal AbstractSprite( SerializedSprite sprite )
            : this()
        {
            OriginalSize = sprite.OriginalSize;
            Palettes = BuildPalettes( sprite.Palettes );
            Pixels = new byte[sprite.Pixels.Count];
            sprite.Pixels.CopyTo( Pixels, 0 );
        }

        private AbstractSprite()
        {
            BitmapDirty = true;
        }

        protected AbstractSprite( IList<byte> bytes, params IList<byte>[] extraBytes )
            : this()
        {
            OriginalSize = bytes.Count;
            Palettes = BuildPalettes( bytes.Sub( 0, 16 * 32 - 1 ) );
            Pixels = BuildPixels( bytes.Sub( 16 * 32 ), extraBytes );
        }

        #endregion Constructors

        #region Methods (11)

        public event EventHandler PixelsChanged;

        protected void FirePixelsChanged()
        {
            if ( PixelsChanged != null )
            {
                PixelsChanged( this, EventArgs.Empty );
            }
        }

        public void ImportSPR( IList<byte> bytes )
        {
            BitmapDirty = true;
            Palettes = BuildPalettes( bytes.Sub( 0, 16 * 32 - 1 ) );
            ImportSPRInner( bytes.Sub( 16 * 32 ) );
            FirePixelsChanged();
        }

        protected abstract IList<byte> BuildPixels(IList<byte> bytes, params IList<byte>[] extraBytes);

        protected abstract void ImportSPRInner( IList<byte> bytes );

        protected static Palette[] BuildPalettes( IList<byte> paletteBytes )
        {
            Palette[] result = new Palette[16];
            for( int i = 0; i < 16; i++ )
            {
                result[i] = new Palette( paletteBytes.Sub( i * 32, (i + 1) * 32 - 1 ) );
            }

            return result;
        }

        /// <summary>
        /// Imports a bitmap and tries to convert it to a FFT sprite.
        /// </summary>
        public virtual void ImportBitmap( Bitmap bmp, out bool foundBadPixels )
        {
            foundBadPixels = false;

            if( bmp.PixelFormat != PixelFormat.Format8bppIndexed )
            {
                throw new BadImageFormatException("Image is not an 8bpp paletted bitmap!");
            }
            if( bmp.Width != 256 )
            {
                throw new BadImageFormatException("Image is not 256 pixels wide!");
            }

            Palettes = new Palette[16];

            for( int i = 0; i < 16; i++ )
            {
                Palettes[i] = Palette.EmptyPalette;
            }

            for ( int i = 0; i < bmp.Palette.Entries.Length; i++ )
            {
                Color c = bmp.Palette.Entries[i];
                Palettes[i / 16][i % 16] = Color.FromArgb( c.R & 0xF8, c.G & 0xF8, c.B & 0xF8 );
                if ( i % 16 == 0 && c.ToArgb() == Color.Black.ToArgb() )
                {
                    Palettes[i / 16][i % 16] = Color.Transparent;
                }
            }

            Pixels.InitializeElements();

            BitmapData bmd = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadWrite, bmp.PixelFormat );
            for( int i = 0; (i < Pixels.Count) && (i / 256 < bmp.Height); i++ )
            {
                Pixels[i] = (byte)bmd.GetPixel( i % 256, i / 256 );
                if( Pixels[i] >= 16 )
                {
                    foundBadPixels = true;
                }
            }

            bmp.UnlockBits( bmd );

            BitmapDirty = true;

            FirePixelsChanged();
        }
        
        protected virtual bool ImportBitmapObject(Bitmap image, IList<byte> originalPaletteBytes, bool is4bpp, int paletteIndex = 0)
        {
            bool foundBadPixels = false;

            if ((is4bpp) && (image.PixelFormat != PixelFormat.Format4bppIndexed))
            {
                throw new BadImageFormatException("Image is not 4bpp paletted!");
            }
            else if ((!is4bpp) && (image.PixelFormat != PixelFormat.Format8bppIndexed))
            {
                throw new BadImageFormatException("Image is not 8bpp paletted!");
            }

            if (image.Width != 256)
            {
                throw new BadImageFormatException("Image is not 256 pixels wide!");
            }

            Palettes = new Palette[16];

            const int singlePaletteBytes = 32;
            Palettes = new Palette[16];

            for (int i = 0; i < 16; i++)
            {
                int startIndex = singlePaletteBytes * i;
                int endIndex = startIndex + singlePaletteBytes - 1;
                IList<byte> bytes = originalPaletteBytes.Sub(startIndex, endIndex);
                Palettes[i] = new Palette(bytes, Palette.ColorDepth._16bit, false, true);
            }
            for (int i = 0; i < image.Palette.Entries.Length; i++)
            {
                if (!is4bpp)
                {
                    paletteIndex = i / 16;
                }
                int colorIndex = i % 16;

                Color c = image.Palette.Entries[i];
                bool wasTransparent = Palettes[paletteIndex][colorIndex] == Color.Transparent;
                Palettes[paletteIndex][colorIndex] = Color.FromArgb(Palettes[paletteIndex][colorIndex].A, c.R & 0xF8, c.G & 0xF8, c.B & 0xF8);
                if (
                    (colorIndex == 0) &&
                    (c.ToArgb() == Color.Black.ToArgb()) &&
                    ((Palettes[paletteIndex][colorIndex].A > 0) || (wasTransparent))
                )
                {
                    Palettes[paletteIndex][colorIndex] = Color.Transparent;
                }
            }

            Pixels.InitializeElements();

            BitmapData bmd = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            int row = 0;
            int col = 0;
            for (int i = 0; (i < Pixels.Count) && (row < image.Height); i++)
            {
                if (is4bpp)
                {
                    Pixels[i] = (byte)bmd.GetPixel4bpp(col, row);
                }
                else
                {
                    Pixels[i] = (byte)bmd.GetPixel(col, row);
                }

                if (Pixels[i] >= 16)
                {
                    foundBadPixels = true;
                }

                col++;
                if (col == image.Width)
                {
                    col = 0;
                    row++;
                }
            }

            image.UnlockBits(bmd);

            BitmapDirty = true;

            FirePixelsChanged();
            return foundBadPixels;
        }

        public virtual bool ImportPNG(IList<byte> importBytes, IList<byte> originalPaletteBytes, bool is4bpp = false, int paletteIndex = 0)
        {
            //System.IO.MemoryStream stream = new System.IO.MemoryStream(importBytes.ToArray());
            //Bitmap pngImage = new Bitmap(stream);
            Bitmap pngImage = PNGHelper.LoadBitmap(importBytes.ToArray());
            return ImportBitmapObject(pngImage, originalPaletteBytes, is4bpp, paletteIndex);
        }

        public virtual void ImportBitmap4bpp(int paletteIndex, IList<byte> importBytes, IList<byte> originalPaletteBytes)
        {
            using (System.IO.Stream stream = new System.IO.MemoryStream(importBytes.ToArray()))
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
                {
                    if (image.PixelFormat != PixelFormat.Format4bppIndexed)
                    {
                        throw new BadImageFormatException("Image is not a 4bpp paletted bitmap!");
                    }
                    if (image.Width != 256)
                    {
                        throw new BadImageFormatException("Image is not 256 pixels wide!");
                    }

                    const int singlePaletteBytes = 32;
                    Palettes = new Palette[16];

                    for (int i = 0; i < 16; i++)
                    {
                        int startIndex = singlePaletteBytes * i;
                        int endIndex = startIndex + singlePaletteBytes - 1;
                        IList<byte> bytes = originalPaletteBytes.Sub(startIndex, endIndex);
                        Palettes[i] = new Palette(bytes, Palette.ColorDepth._16bit, false, true);
                    }
                    for (int i = 0; i < image.Palette.Entries.Length; i++)
                    {
                        Color c = image.Palette.Entries[i];
                        bool wasTransparent = Palettes[paletteIndex][i] == Color.Transparent;
                        Palettes[paletteIndex][i] = Color.FromArgb(Palettes[paletteIndex][i].A, c.R & 0xF8, c.G & 0xF8, c.B & 0xF8);
                        if (
                            (i == 0) && 
                            (c.ToArgb() == Color.Black.ToArgb()) &&
                            ((Palettes[paletteIndex][i].A > 0) || (wasTransparent))
                        )
                        {
                            Palettes[paletteIndex][i] = Color.Transparent;
                        }
                    }

                    Pixels.InitializeElements();

                    int combinedWidth = (image.Width + 1) / 2;
                    int stride = (((Width * 4) + 31) / 32) * 4;

                    //byte[] resultData = new byte[image.Height * stride];
                    int imageDataOffset = importBytes[10] | (importBytes[11] << 8) | (importBytes[12] << 16) | (importBytes[13] << 24);

                    for (int rowIndex = 0; rowIndex < image.Height; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < combinedWidth; colIndex++)
                        {
                            int currentByteIndex = imageDataOffset + (rowIndex * stride) + colIndex;
                            int pixelIndex = ((image.Height - rowIndex - 1) * image.Width) + (colIndex * 2);
                            byte currentByte = importBytes[currentByteIndex];

                            if (pixelIndex < Pixels.Count)
                            {
                                Pixels[pixelIndex] = (byte)((currentByte & 0xF0) >> 4);
                                if ((colIndex < image.Width) && ((pixelIndex + 1) < Pixels.Count))
                                    Pixels[pixelIndex + 1] = (byte)(currentByte & 0x0F);
                            }
                        }
                    }
                }
            }

            //System.IO.File.WriteAllBytes(@"pixels4.bin", Pixels.ToArray());    // DEBUG
            BitmapDirty = true;
            FirePixelsChanged();
        }

        public virtual void ImportBitmap8bpp(IList<byte> importBytes, IList<byte> originalPaletteBytes)
        {
            using (System.IO.Stream stream = new System.IO.MemoryStream(importBytes.ToArray()))
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
                {
                    if (image.PixelFormat != PixelFormat.Format8bppIndexed)
                    {
                        throw new BadImageFormatException("Image is not an 8bpp paletted bitmap!");
                    }
                    if (image.Width != 256)
                    {
                        throw new BadImageFormatException("Image is not 256 pixels wide!");
                    }

                    const int singlePaletteBytes = 32;
                    Palettes = new Palette[16];

                    for (int i = 0; i < 16; i++)
                    {
                        int startIndex = singlePaletteBytes * i;
                        int endIndex = startIndex + singlePaletteBytes - 1;
                        IList<byte> bytes = originalPaletteBytes.Sub(startIndex, endIndex);
                        Palettes[i] = new Palette(bytes, Palette.ColorDepth._16bit, false, true);
                    }
                    for (int i = 0; i < image.Palette.Entries.Length; i++)
                    {
                        int paletteIndex = i / 16;
                        int colorIndex = i % 16;

                        Color c = image.Palette.Entries[i];
                        bool wasTransparent = Palettes[paletteIndex][colorIndex] == Color.Transparent;
                        Palettes[paletteIndex][colorIndex] = Color.FromArgb(Palettes[paletteIndex][colorIndex].A, c.R & 0xF8, c.G & 0xF8, c.B & 0xF8);
                        if (
                            (colorIndex == 0) &&
                            (c.ToArgb() == Color.Black.ToArgb()) &&
                            ((Palettes[paletteIndex][colorIndex].A > 0) || (wasTransparent))
                        )
                        {
                            Palettes[paletteIndex][colorIndex] = Color.Transparent;
                        }
                    }

                    Pixels.InitializeElements();

                    int stride = (((image.Width * 8) + 31) / 32) * 4;
                    //int resultStride = (((image.Width * 4) + 31) / 32) * 4;
                    //byte[] resultData = new byte[image.Height * resultStride];
                    int imageDataOffset = importBytes[10] | (importBytes[11] << 8) | (importBytes[12] << 16) | (importBytes[13] << 24);

                    for (int rowIndex = 0; rowIndex < image.Height; rowIndex++)
                    //for (int rowIndex = 0; rowIndex < 1; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < image.Width; colIndex++)
                        {
                            int currentByteIndex = imageDataOffset + (rowIndex * stride) + colIndex;
                            //int resultByteIndex = ((image.Height - rowIndex - 1) * resultStride) + (colIndex / 2);
                            int pixelIndex = ((image.Height - rowIndex - 1) * image.Width) + (colIndex);
                            byte currentByte = importBytes[currentByteIndex];
                            //resultData[resultByteIndex] |= (((colIndex & 0x01) == 0) ? ((byte)(currentByte & 0x0F)) : ((byte)((currentByte & 0x0F) << 4)));

                            if (pixelIndex < Pixels.Count)
                            {
                                Pixels[pixelIndex] = currentByte;
                            }
                        }
                    }
                }
            }

            //System.IO.File.WriteAllBytes(@"pixels8.bin", Pixels.ToArray());    // DEBUG
            BitmapDirty = true;
            FirePixelsChanged();
        }

        public virtual void Import( Image file )
        {
            if( file is Bitmap )
            {
                bool bad;
                ImportBitmap( file as Bitmap, out bad );
            }
            else
            {
                throw new ArgumentException( "file must be Bitmap", "file" );
            }

            BitmapDirty = true;
        }

        /// <summary>
        /// Converts this sprite to an indexed bitmap.
        /// </summary>
        public Bitmap ToBitmap(int paletteIndex = 0, bool forceUpdate = false)
        {
            if ( BitmapDirty || forceUpdate )
            {

                Bitmap bmp = new Bitmap( Width, Height, PixelFormat.Format8bppIndexed );
                ColorPalette palette = bmp.Palette;

                //Palette.FixupColorPalette( palette, Palettes );
                if (paletteIndex > 0)
                    Palette.FixupColorPalette(palette, Palettes, paletteIndex, 0);
                else
                    Palette.FixupColorPalette(palette, Palettes);

                bmp.Palette = palette;

                BitmapData bmd = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadWrite, bmp.PixelFormat );
                ToBitmapInner( bmp, bmd );
                bmp.UnlockBits( bmd );

                CachedBitmap = bmp;
                BitmapDirty = false;
            }

            return CachedBitmap;
        }

        public virtual Bitmap To4bppBitmapUncached( int whichPalette )
        {
            Bitmap result = new Bitmap( Width, Height, System.Drawing.Imaging.PixelFormat.Format4bppIndexed );
            System.Drawing.Imaging.BitmapData bmd = result.LockBits( new Rectangle( Point.Empty, result.Size ), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format4bppIndexed );
            ColorPalette pal = result.Palette;
            Palette.FixupColorPalette( pal, Palettes, whichPalette, 0 );
            result.Palette = pal;

            for( int i = 0; i < Pixels.Count; i++ )
            {
                bmd.SetPixel4bpp( i % Width, i / Width, Pixels[i] % 16 );
            }

            result.UnlockBits( bmd );

            return result;
        }

        public string GetPaletteForPaintDotNet()
        {
            StringBuilder result = new StringBuilder();
            foreach (Palette p in Palettes)
            {
                foreach (Color c in p.Colors)
                {
                    result.AppendFormat("{0:X2}{1:X2}{2:X2}{3:X2}" + Environment.NewLine, c.A, c.R, c.G, c.B);
                }
            }
            return result.ToString();
        }
    
        protected abstract void ToBitmapInner( Bitmap bmp, BitmapData bmd );

        /*
        public virtual Image Export()
        {
            return ToBitmap();
        }
        */

        public List<byte> GetPaletteBytes(IEnumerable<Palette> palettes, IList<byte> originalPaletteBytes, Palette.ColorDepth depth)
        {
            List<byte> result = new List<byte>();

            int paletteIndex = 0;
            foreach (Palette palette in palettes)
            {
                int colorSize = (depth == Palette.ColorDepth._32bit) ? 4 : 2;
                int paletteSize = colorSize * palette.Colors.Length;
                result.AddRange(GetPaletteBytes(palette.Colors, originalPaletteBytes.SubLength(paletteSize * paletteIndex, paletteSize), depth));
                paletteIndex++;
            }

            return result;
        }

        public List<byte> GetPaletteBytes(IList<Color> colors, IList<byte> originalPaletteBytes, Palette.ColorDepth depth)
        {
            List<byte> result = new List<byte>();
            int index = 0;

            foreach (Color c in colors)
            {
                byte alphaByte = (depth == Palette.ColorDepth._32bit ? originalPaletteBytes[index * 4 + 3] : originalPaletteBytes[index * 2 + 1]);
                result.AddRange(Palette.ColorToBytes(c, alphaByte, depth));
                index++;
            }

            if (colors[0] == Color.Transparent)
            {
                result[0] = 0x00;
                result[1] = 0x00;
            }

            return result;
        }

        /// <summary>
        /// Converts this sprite to an array of bytes.
        /// </summary>
        public IList<byte[]> ToByteArrays()
        {
            byte[][] result = new byte[Filenames.Count][];
            for ( int i = 0; i < Filenames.Count; i++ )
            {
                result[i] = ToByteArray( i );
            }
            return result;
        }

        public abstract byte[] ToByteArray( int index );

        #endregion Methods


    }
}
