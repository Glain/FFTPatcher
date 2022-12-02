using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.SpriteEditor
{
    public abstract class AbstractCompressedSprite : AbstractSprite
    {

        public override void ImportBitmap( Bitmap bmp, out bool foundBadPixels )
        {
            base.ImportBitmap( bmp, out foundBadPixels );
            MovePortraitAndCompressedAreas();
            FirePixelsChanged();
        }

        public override void ImportBitmap4bpp(int paletteIndex, IList<byte> importBytes, IList<byte> originalPaletteBytes)
        {
            base.ImportBitmap4bpp(paletteIndex, importBytes, originalPaletteBytes);
            MovePortraitAndCompressedAreas();
            FirePixelsChanged();
        }

        public override void ImportBitmap8bpp(IList<byte> importBytes, IList<byte> originalPaletteBytes)
        {
            base.ImportBitmap8bpp(importBytes, originalPaletteBytes);
            MovePortraitAndCompressedAreas();
            FirePixelsChanged();
        }

        public override bool ImportPNG(IList<byte> importBytes, IList<byte> originalPaletteBytes, bool is4bpp = false, int paletteIndex = 0)
        {
            bool result = base.ImportPNG(importBytes, originalPaletteBytes, is4bpp, paletteIndex);
            MovePortraitAndCompressedAreas();
            FirePixelsChanged();
            return result;
        }

        public override int Height
        {
            get { return 488; }
        }

        internal AbstractCompressedSprite( SerializedSprite sprite )
            : base( sprite )
        {
        }

        protected AbstractCompressedSprite( IList<byte> bytes, params IList<byte>[] extraBytes )
            : base( bytes, extraBytes )
        {
        }

        protected override Rectangle PortraitRectangle
        {
            get { return new Rectangle( 80, 456, 48, 32 ); }
        }

        private void MovePortraitAndCompressedAreas()
        {
            byte[] portraitArea = Pixels.Sub(Width * (topHeight + compressedHeight), Width * (topHeight + compressedHeight + portraitHeight) - 1).ToArray();
            byte[] compressedArea = Pixels.Sub(Width * topHeight, Width * (topHeight + compressedHeight) - 1).ToArray();
            portraitArea.CopyTo(Pixels, Width * topHeight);
            compressedArea.CopyTo(Pixels, Width * (topHeight + portraitHeight));
            BitmapDirty = true;
        }

        protected override IList<byte> BuildPixels(IList<byte> bytes, params IList<byte>[] extraBytes)
        {
            List<byte> result = new List<byte>( 36864 * 2 );
            foreach( byte b in bytes.Sub( 0, 36864 - 1 ) )
            {
                result.Add( b.GetLowerNibble() );
                result.Add( b.GetUpperNibble() );
            }

            result.AddRange( Decompress( bytes.Sub( 36864 ) ) );

            result.AddRange( new byte[Math.Max( 0, 488 * 256 - result.Count )] );
            return result.ToArray();
        }

        protected override void ImportSPRInner( IList<byte> bytes )
        {
            BuildPixels( bytes, null ).Sub( 0, 488 * 256 - 1 ).CopyTo( Pixels, 0 );
        }

        protected override void DrawSpriteInternal( int palette, int portraitPalette, SetPixel setPixel )
        {
            for ( int i = 0; i < Pixels.Count && ( i / Width ) < topHeight; i++ )
            {
                setPixel( i % Width, i / Width, Palettes[palette].Colors[Pixels[i] % 16] );
            }
            for ( int i = ( topHeight + portraitHeight ) * Width; i < Pixels.Count && i / Width < ( topHeight + portraitHeight + compressedHeight ); i++ )
            {
                setPixel( i % Width, i / Width - portraitHeight, Palettes[palette].Colors[Pixels[i] % 16] );
            }
            for ( int i = Width * topHeight; i < Pixels.Count && i / Width < ( topHeight + portraitHeight ); i++ )
            {
                setPixel( i % Width, i / Width + compressedHeight, Palettes[palette].Colors[Pixels[i] % 16] );
            }

            for ( int i = Width * ( topHeight + portraitHeight + compressedHeight ); i < Pixels.Count && ( i / Width < Height ); i++ )
            {
                setPixel( i % Width, i / Width, Palettes[palette].Colors[Pixels[i] % 16] );
            }

            Rectangle pRect = PortraitRectangle;

            for ( int x = pRect.X; x < pRect.Right; x++ )
            {
                for ( int y = pRect.Y; y < pRect.Bottom && ( x + y * Width < Pixels.Count ); y++ )
                {
                    setPixel( x, y, Palettes[portraitPalette].Colors[Pixels[x + ( y - compressedHeight ) * Width] % 16] );
                }
            }
        }

        private static byte[] Recompress( IList<byte> bytes )
        {
            List<byte> realBytes = new List<byte>( bytes.Count );
            for( int i = 0; (i + 1) < bytes.Count; i += 2 )
            {
                realBytes.Add( bytes[i + 1] );
                realBytes.Add( bytes[i] );
            }

            List<byte> result = new List<byte>();
            int pos = 0;
            while( pos < realBytes.Count )
            {
                int z = NumberOfZeroes( realBytes.Sub( pos ) );
                z = Math.Min( z, 0xFFF );

                if( z == 0 )
                {
                    byte b = realBytes[pos];
                    result.Add( realBytes[pos] );
                    pos += 1;
                }
                else if( z < 16 )
                {
                    if( (z == 8) ||
                        (z == 7) )
                    {
                        result.Add( 0x00 );
                        result.Add( 0x00 );
                        result.Add( (byte)z );
                    }
                    else
                    {
                        result.Add( 0x00 );
                        result.Add( (byte)z );
                    }
                }
                else if( z < 256 )
                {
                    result.Add( 0x00 );
                    result.Add( 0x07 );
                    result.Add( ((byte)z).GetLowerNibble() );
                    result.Add( ((byte)z).GetUpperNibble() );
                }
                else if( z < 4096 )
                {
                    result.Add( 0x00 );
                    result.Add( 0x08 );
                    result.Add( ((byte)z).GetLowerNibble() );
                    result.Add( ((byte)z).GetUpperNibble() );
                    result.Add( (byte)((z & 0xF00) >> 8) );
                }

                pos += z;
            }

            return CompressNibbles( result );
        }

        private static byte[] CompressNibbles( IList<byte> bytes )
        {
            List<byte> result = new List<byte>( bytes.Count / 2 );
            for( int i = 0; i < bytes.Count; i += 2 )
            {
                if( (i + 1) < bytes.Count )
                {
                    result.Add( (byte)(((bytes[i] & 0x0F) << 4) + (bytes[i + 1] & 0x0F)) );
                }
                else
                {
                    result.Add( (byte)((bytes[i] & 0x0F) << 4) );
                }
            }
            return result.ToArray();
        }
        private static int NumberOfZeroes( IList<byte> bytes )
        {
            for( int i = 0; i < bytes.Count; i++ )
            {
                if( bytes[i] != 0 )
                    return i;
            }

            return bytes.Count;
        }

        public override byte[] ToByteArray( int index )
        {
            //System.IO.File.WriteAllBytes(@"pixels_" + DateTime.Now.Ticks + ".bin", Pixels.ToArray());   // DEBUG

            System.Diagnostics.Debug.Assert( index == 0 );
            List<byte> ourResult = new List<byte>( 36864 );
            foreach ( Palette p in Palettes )
            {
                ourResult.AddRange( p.ToByteArray() );
            }
            for ( int i = 0; i < 36864; i++ )
            {
                ourResult.Add( (byte)( ( Pixels[2 * i + 1] << 4 ) | ( Pixels[2 * i] & 0x0F ) ) );
            }

            int pixelIndex = Pixels.Count - 1;
            for (; ((pixelIndex >= 0) && (Pixels[pixelIndex] == 0)); pixelIndex--) ;
            pixelIndex = (pixelIndex < 0) ? 0 : pixelIndex;
            int sectorByteIndex = Math.Min(((pixelIndex / 2048) * 2048), 122880);
            int nextSectorByteIndex = sectorByteIndex + 2048;
            //int endIndex = Math.Min(nextSectorByteIndex, 124928) - 1;
            //int endIndex = Math.Min(nextSectorByteIndex + (2048 * 3), 124928) - 1;

            /*
            // DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int testIndex = 99999; testIndex < 111111; testIndex++)
            {
                byte[] testBytes = Recompress(Pixels.Sub(2 * 36864, testIndex));
                int testStartIndex = testBytes.Length - 2;
                int testEndIndex = testStartIndex + 1;
                if ((testBytes[testStartIndex] == 0xD8) && (testBytes[testEndIndex] == 0x90))
                    sb.AppendLine(testIndex.ToString());
            }
            string testText = sb.ToString();
            if (!string.IsNullOrEmpty(testText))
            {
                System.IO.File.WriteAllText(@"recompress_index.txt", testText);
            }
            */

            //if (OriginalSize > 38912)
            //if (OriginalSize > 40960)
            //if (sectorByteIndex > (40960 * 2))

            if (sectorByteIndex > (55296 * 2))
            {
                ourResult.AddRange(Recompress(Pixels.Sub(2 * 36864, 2 * 36864 + 200 * 256 - 1)));     // (73728, 124927)
            }
            else if (sectorByteIndex > (49152 * 2))
            {
                //int endIndex = Math.Min(nextSectorByteIndex + (2048 * 2) + 1024 + 512, 124928) - 1;
                //int endIndex = Math.Min(nextSectorByteIndex + (2048 * 4), 124928) - 1;
                int endIndex = Math.Min(nextSectorByteIndex + 6568, 124928) - 1;

                byte[] bytes = Recompress(Pixels.Sub(2 * 36864, endIndex));
                //bytes[bytes.Length - 2] = 0xD8;
                //bytes[bytes.Length - 1] = 0x90;

                ourResult.AddRange(bytes);
            }
            else if (sectorByteIndex > (38912 * 2))
            {
                int endIndex = Math.Min(nextSectorByteIndex + 6568, 124928) - 1;
                //int endIndex = Math.Min(nextSectorByteIndex + (2048 * 3), 124928) - 1;

                byte[] bytes = Recompress(Pixels.Sub(2 * 36864, endIndex));
                bytes[bytes.Length - 2] = 0xD8;
                bytes[bytes.Length - 1] = 0x90;

                ourResult.AddRange(bytes);
            }             

            if ( ourResult.Count < OriginalSize )
            {
                ourResult.AddRange( new byte[OriginalSize - ourResult.Count] );
            }

            return ourResult.ToArray();
        }

        public const int topHeight = 256;
        public const int portraitHeight = 32;
        public const int compressedHeight = 200;

        protected override void ToBitmapInner( System.Drawing.Bitmap bmp, System.Drawing.Imaging.BitmapData bmd )
        {
            // Above portrait
            for ( int i = 0; ( i < this.Pixels.Count ) && ( i / Width < topHeight ); i++ )
            {
                bmd.SetPixel8bpp( i % Width, i / Width, Pixels[i] );
            }

            // Compressed part
            for ( int i = ( topHeight + portraitHeight ) * Width; ( i < this.Pixels.Count ) && ( i / Width < Height ); i++ )
            {
                bmd.SetPixel8bpp( i % Width, i / Width - portraitHeight, Pixels[i] );
            }

            // Portrait part
            for ( int i = topHeight * Width; ( i < this.Pixels.Count ) && ( i / Width < ( topHeight + portraitHeight ) ); i++ )
            {
                bmd.SetPixel8bpp( i % Width, i / Width + compressedHeight, Pixels[i] );
            }
        }

        public override Bitmap To4bppBitmapUncached( int whichPalette )
        {
            Bitmap result = base.To4bppBitmapUncached( whichPalette );

            BitmapData bmd = result.LockBits( new Rectangle( Point.Empty, result.Size ), ImageLockMode.WriteOnly, PixelFormat.Format4bppIndexed ); 

            // Compressed part
            for ( int i = ( topHeight + portraitHeight ) * Width; ( i < this.Pixels.Count ) && ( i / Width < Height ); i++ )
            {
                bmd.SetPixel4bpp( i % Width, i / Width - portraitHeight, Pixels[i] );
            }

            // Portrait part
            for ( int i = topHeight * Width; ( i < this.Pixels.Count ) && ( i / Width < ( topHeight + portraitHeight ) ); i++ )
            {
                bmd.SetPixel4bpp( i % Width, i / Width + compressedHeight, Pixels[i] );
            }

            result.UnlockBits( bmd );

            return result;
        }

        protected static IList<byte> Decompress( IList<byte> bytes )
        {
            byte[] compressed = new byte[bytes.Count * 2];
            for( int i = 0; i < bytes.Count; i++ )
            {
                compressed[i * 2] = bytes[i].GetUpperNibble();
                compressed[i * 2 + 1] = bytes[i].GetLowerNibble();
            }

            List<byte> result = new List<byte>();
            int j = 0;
            while( j < compressed.Length )
            {
                byte b = compressed[j];
                if( compressed[j] != 0 )
                {
                    result.Add( compressed[j] );
                }
                else if( (j + 1) < compressed.Length )
                {
                    byte s = compressed[j + 1];
                    int l = s;
                    if( (s == 7) && ((j + 3) < compressed.Length) )
                    {
                        l = compressed[j + 2] + (compressed[j + 3] << 4);
                        j += 2;
                    }
                    else if( (s == 8) && ((j + 4) < compressed.Length) )
                    {
                        l = compressed[j + 2] + (compressed[j + 3] << 4) + (compressed[j + 4] << 8);
                        j += 3;
                    }
                    else if( (s == 0) && ((j + 2) < compressed.Length) )
                    {
                        l = compressed[j + 2];
                        j++;
                    }
                    else
                    {
                        l = s;
                    }

                    j++;
                    result.AddRange(new byte[l]);
                }

                j++;
            }

            j = 0;
            while( (j + 1) < result.Count )
            {
                byte k = result[j];
                result[j] = result[j + 1];
                result[j + 1] = k;
                j += 2;
            }

            return result;
        }
    }
}
