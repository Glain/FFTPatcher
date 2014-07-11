using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;

namespace FFTPatcher.SpriteEditor
{
    public class PalettedImage4bpp : AbstractImage
    {
        public static PalettedImage4bpp ConstructFromXml( XmlNode node )
        {
            ImageInfo info = GetImageInfo( node );
            var palPos = GetPalettePositionFromImageNode( info.Sector, node );
            var pos = GetPositionFromImageNode( info.Sector, node );
            FFTPatcher.SpriteEditor.Palette.ColorDepth depth = Palette.ColorDepth._16bit;

            var cdNode = node.SelectSingleNode( "ColorDepth" );
            if (cdNode != null)
            {
                depth = (Palette.ColorDepth)Enum.Parse( typeof( Palette.ColorDepth ), cdNode.InnerText );
            }

            return new PalettedImage4bpp( info.Name, info.Width, info.Height, 1, depth, pos, palPos );
        }

        public override string DescribeXml()
        {
            string sectorType = this.position is PatcherLib.Iso.PsxIso.KnownPosition ? "Sector" :
                                ((PatcherLib.Iso.PspIso.KnownPosition)position).FFTPack.HasValue ?
                                "FFTPack" : "Sector";
            string sectorValue = this.position is PatcherLib.Iso.PsxIso.KnownPosition ?
                ((PatcherLib.Iso.PsxIso.KnownPosition)position).Sector.ToString() :
                ((PatcherLib.Iso.PspIso.KnownPosition)position).FFTPack.HasValue ?
                ((PatcherLib.Iso.PspIso.KnownPosition)position).FFTPack.Value.ToString() :
                ((PatcherLib.Iso.PspIso.KnownPosition)position).Sector.Value.ToString();
            int offset = this.position is PatcherLib.Iso.PsxIso.KnownPosition ?
                ((PatcherLib.Iso.PsxIso.KnownPosition)position).StartLocation :
                ((PatcherLib.Iso.PspIso.KnownPosition)position).StartLocation;
            int paletteOffset = palettePosition is PatcherLib.Iso.PsxIso.KnownPosition ?
                ((PatcherLib.Iso.PsxIso.KnownPosition)palettePosition).StartLocation :
                ((PatcherLib.Iso.PspIso.KnownPosition)palettePosition).StartLocation;

            return string.Format(
@"<{0}>
  <Name>{1}</Name>
  <Width>{2}</Width>
  <Height>{3}</Height>
  <ColorDepth>{10}</ColorDepth>
  <{4}>{5}</{4}>
  <PalettePosition>
    <Offset>{8}</Offset>
    <Length>{9}</Length>
  </PalettePosition>
  <Position>
    <Offset>{6}</Offset>
    <Length>{7}</Length>
  </Position>
</{0}>", this.GetType().Name, this.Name, this.Width, this.Height, sectorType, sectorValue, offset, position.Length, paletteOffset, palettePosition.Length, depth );
        }


        public PalettedImage4bpp( 
            string name, 
            int width, int height, 
            int numPalettes,
            PatcherLib.Iso.KnownPosition imagePosition, 
            PatcherLib.Iso.KnownPosition palettePosition )
            : this( name, width, height, numPalettes, Palette.ColorDepth._16bit, imagePosition, palettePosition )
        {
        }

        protected PatcherLib.Iso.KnownPosition position;

        public PalettedImage4bpp(
            string name,
            int width, int height,
            int numPalettes,
            FFTPatcher.SpriteEditor.Palette.ColorDepth depth,
            PatcherLib.Iso.KnownPosition imagePosition,
            PatcherLib.Iso.KnownPosition palettePosition )
            : base( name, width, height )
        {
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

            Bitmap result = new Bitmap( Width, Height );

            for (int i = 0; i < Width * Height; i++)
            {
                result.SetPixel( i % Width, i / Width, p[splitBytes[i]] );
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

        protected override void WriteImageToIsoInner( System.IO.Stream iso, System.Drawing.Image image )
        {
            using ( Bitmap bmp = new Bitmap( image ) )
            {
                Set<Color> colors = GetColors( bmp );
                IList<Color> myColors = new List<Color>( colors );
                if ( myColors.Count > 16 )
                {
                    ImageQuantization.OctreeQuantizer q = new ImageQuantization.OctreeQuantizer( 16, 8 );
                    using ( var newBmp = q.Quantize( bmp ) )
                    {
                        WriteImageToIsoInner( iso, newBmp );
                    }
                }
                else
                {
                    IEnumerable<Color> enumColors = myColors;
                    var paletteBytes = GetPaletteBytes( ref enumColors );
                    var imageBytes = GetImageBytes( bmp, new Set<Color>(enumColors) );
                    position.PatchIso( iso, imageBytes );
                    palettePosition.PatchIso( iso, paletteBytes );
                }
            }
        }

        protected IList<byte> GetPaletteBytes( ref IEnumerable<Color> colors )
        {
            List<byte> result = new List<byte>( );
            List<Color> realColors = new List<Color>( colors );

            // This is a bit silly
            // Sort the list by ARGB, and put the transparent one at the top
            // Return the new collection by side effect
            realColors.Sort( ( a, b ) => a.ToArgb().CompareTo( b.ToArgb() ) );

            if (realColors.Exists( c => c.A == 0 ))
            {
                int transIndex = realColors.FindIndex( c => c.A == 0 );
                Color trans = realColors[transIndex];
                realColors.RemoveAt( transIndex );

                realColors.Insert( 0, trans );

                colors = realColors;
            }

            if (depth == Palette.ColorDepth._16bit)
            {
                foreach (Color c in realColors)
                {
                    result.AddRange( Palette.ColorToBytes( c ) );
                }
            }
            else if (depth == Palette.ColorDepth._32bit)
            {
                foreach (Color c in realColors)
                {
                    result.AddRange( new byte[] { c.R, c.G, c.B, c.A } );
                }
            }
            result.AddRange( new byte[Math.Max( 0, 16*(int)depth - result.Count )] );
            return result;
        }

        protected IList<byte> GetImageBytes( Bitmap image, Set<Color> colors )
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
