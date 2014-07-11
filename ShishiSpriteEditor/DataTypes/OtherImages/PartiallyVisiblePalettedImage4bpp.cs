#if FALSE
using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace FFTPatcher.SpriteEditor
{
    public class PartiallyVisiblePalettedImage4bpp : PalettedImage4bpp 
    {
        public PartiallyVisiblePalettedImage4bpp( 
            string name, 
            int width, int height, 
            int numPalettes,
            Rectangle visiblePortion,
            PatcherLib.Iso.KnownPosition imagePosition, 
            PatcherLib.Iso.KnownPosition palettePosition )
            : this( name, width, height, numPalettes, visiblePortion, Palette.ColorDepth._16bit, imagePosition, palettePosition )
        {
        }

        private PatcherLib.Iso.KnownPosition position;

        private Size FullSize { get; set; }

        public Rectangle VisiblePortion { get; private set; }

        public PartiallyVisiblePalettedImage4bpp(
            string name,
            int width, int height,
            int numPalettes,
            Rectangle visiblePortion,
            FFTPatcher.SpriteEditor.Palette.ColorDepth depth,
            PatcherLib.Iso.KnownPosition imagePosition,
            PatcherLib.Iso.KnownPosition palettePosition )
            : base( name, visiblePortion.Width, visiblePortion.Height )
        {
            VisiblePortion = visiblePortion;
            FullSize = new Size( width, height );
            this.position = imagePosition;
            this.palettePosition = palettePosition;
            this.depth = depth;

            System.Diagnostics.Debug.Assert( palettePosition.Length == 8 * (int)depth * 2 );
            if ( position is PatcherLib.Iso.PsxIso.KnownPosition )
            {
                var pos = position as PatcherLib.Iso.PsxIso.KnownPosition;
                saveFileName = string.Format( "{0}_{1}.png", pos.Sector, pos.StartLocation );
            }
            else if ( position is PatcherLib.Iso.PspIso.KnownPosition )
            {
                var pos = position as PatcherLib.Iso.PspIso.KnownPosition;
                saveFileName = string.Format( "{0}_{1}.png", pos.SectorEnum, pos.StartLocation );
            }
        }

        private string saveFileName;
        protected override string SaveFileName
        {
            get { return saveFileName; }
        }

        private Palette.ColorDepth depth;

        private PatcherLib.Iso.KnownPosition palettePosition;

        protected override System.Drawing.Bitmap GetImageFromIsoInner( System.IO.Stream iso )
        {
            Palette p = new Palette( palettePosition.ReadIso( iso ), depth );
            IList<byte> bytes = position.ReadIso( iso );
            IList<byte> splitBytes = new List<byte>( bytes.Count * 2 );
            foreach (byte b in bytes.Sub( 0, Height * Width / 2 - 1 ))
            {
                splitBytes.Add( b.GetLowerNibble() );
                splitBytes.Add( b.GetUpperNibble() );
            }

            Bitmap result = new Bitmap( VisiblePortion.Width, VisiblePortion.Height );

            for(int x = 0; x < VisiblePortion.Width; x++)
            {
                for (int y =0 ; y < VisiblePortion.Height; y++)
                {
                    result.SetPixel( x, y, p[splitBytes[(y + VisiblePortion.Y) * FullSize.Width + (x + VisiblePortion.X)]] );
                }
            }

            return result;
        }

        public override string FilenameFilter
        {
            get
            {
                return "GIF image (*.gif)|*.gif";
            }
        }

        public override void SaveImage( System.IO.Stream iso, System.IO.Stream output )
        {
            // Get colors
            Set<Color> colors = GetColors( iso );

            // Convert colors to indices
            Bitmap originalImage = GetImageFromIso( iso );

            using ( Bitmap bmp = new Bitmap( Width, Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed ) )
            {
                var pal = bmp.Palette;
                for ( int i = 0; i < colors.Count; i++ )
                {
                    for ( int j = 0; j < 16; j++ )
                    {
                        pal.Entries[j*16+i] = colors[i];
                    }
                }
                bmp.Palette = pal;

                var bmd = bmp.LockBits( new Rectangle( 0, 0, Width, Height ), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
                for ( int x = 0; x < Width; x++ )
                {
                    for ( int y = 0; y < Height; y++ )
                    {
                        bmd.SetPixel8bpp( x, y, colors.IndexOf( originalImage.GetPixel( x, y ) ) );
                    }
                }
                bmp.UnlockBits( bmd );

                // Write that shit
                bmp.Save( output, System.Drawing.Imaging.ImageFormat.Gif );
            }
        }

        private Bitmap GetFullImageFromIso( System.IO.Stream iso )
        {
            Palette p = new Palette( palettePosition.ReadIso( iso ), depth );
            IList<byte> bytes = position.ReadIso( iso );
            IList<byte> splitBytes = new List<byte>( bytes.Count * 2 );
            foreach (byte b in bytes.Sub( 0, Height * Width / 2 - 1 ))
            {
                splitBytes.Add( b.GetLowerNibble() );
                splitBytes.Add( b.GetUpperNibble() );
            }

            Bitmap bmp = new Bitmap( FullSize.Width, FullSize.Height );
            for (int x = 0; x < FullSize.Width; x++)
            {
                for (int y = 0; y > FullSize.Height; y++)
                {
                    bmp.SetPixel( x, y, p[splitBytes[y * FullSize.Width + x]] );
                }
            }
            return bmp;
        }

        protected override Set<Color> GetColors( System.IO.Stream iso )
        {
            using (Bitmap fullImage = GetFullImageFromIso( iso ))
            {
                Set<Color> result = new Set<Color>( ( a, b ) => a.ToArgb() == b.ToArgb() ? 0 : 1 );

                for (int x = 0; x < fullImage.Width; x++)
                {
                    for (int y = 0; y < fullImage.Height; y++)
                    {
                        result.Add( fullImage.GetPixel( x, y ) );
                    }
                }

                return result.AsReadOnly();
            }
        }

        protected override void WriteImageToIsoInner( System.IO.Stream iso, System.Drawing.Image image )
        {
            using ( Bitmap bmp = new Bitmap( image ) )
            {
                Set<Color> colors = GetColors( bmp );
                if ( colors.Count > 16 )
                {
                    ImageQuantization.OctreeQuantizer q = new ImageQuantization.OctreeQuantizer( 16, 8 );
                    using ( var newBmp = q.Quantize( bmp ) )
                    {
                        WriteImageToIsoInner( iso, newBmp );
                    }
                }
                else
                {
                    var paletteBytes = GetPaletteBytes( colors );
                    var imageBytes = GetImageBytes( bmp, colors );
                    position.PatchIso( iso, imageBytes );
                    palettePosition.PatchIso( iso, paletteBytes );
                }
            }
        }

        private IList<byte> GetPaletteBytes( Set<Color> colors )
        {
            List<byte> result = new List<byte>( colors.Count * 2 );
            if (depth == Palette.ColorDepth._16bit)
            {
                foreach (Color c in colors)
                {
                    result.AddRange( Palette.ColorToBytes( c ) );
                }
            }
            else if (depth == Palette.ColorDepth._32bit)
            {
                foreach (Color c in colors)
                {
                    result.AddRange( new byte[] { c.R, c.G, c.B, c.A } );
                }
            }
            result.AddRange( new byte[Math.Max( 0, 16*(int)depth - result.Count )] );
            return result;
        }

        private IList<byte> GetImageBytes( Bitmap image, Set<Color> colors )
        {
            List<byte> result = new List<byte>( Width * Height );
            for ( int y = 0; y < Height; y++ )
            {
                for ( int x = 0; x < Width; x++ )
                {
                    result.Add( (byte)colors.IndexOf( image.GetPixel( x, y ) ) );
                }
            }

            byte[] realResult = new byte[result.Count / 2];
            for ( int i = 0; i < realResult.Length; i++ )
            {
                realResult[i] = (byte)( ( result[2*i] & 0x0F ) | ( ( result[2*i + 1] & 0x0F ) << 4 ) );
            }

            return realResult;
        }
    }
}
#endif