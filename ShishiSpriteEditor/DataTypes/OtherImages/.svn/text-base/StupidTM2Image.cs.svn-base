using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using PatcherLib.Utilities;
using PatcherLib.Datatypes;
namespace FFTPatcher.SpriteEditor
{
    public class StupidTM2Image : AbstractImage
    {
        protected PatcherLib.Iso.KnownPosition PalettePosition { get; private set; }
        protected IList<PatcherLib.Iso.KnownPosition> PixelPositions { get; private set; }

        protected virtual int NumColors { get { return 256; } }
        public override string DescribeXml()
        {

            string sectorType = this.PalettePosition is PatcherLib.Iso.PsxIso.KnownPosition ? "Sector" :
                                ((PatcherLib.Iso.PspIso.KnownPosition)PalettePosition).FFTPack.HasValue ?
                                "FFTPack" : "Sector";
            string sectorValue = this.PalettePosition is PatcherLib.Iso.PsxIso.KnownPosition ?
                ((PatcherLib.Iso.PsxIso.KnownPosition)PalettePosition).Sector.ToString() :
                ((PatcherLib.Iso.PspIso.KnownPosition)PalettePosition).FFTPack.HasValue ?
                ((PatcherLib.Iso.PspIso.KnownPosition)PalettePosition).FFTPack.Value.ToString() :
                ((PatcherLib.Iso.PspIso.KnownPosition)PalettePosition).Sector.Value.ToString();
            int paletteOffset = PalettePosition is PatcherLib.Iso.PsxIso.KnownPosition ?
                ((PatcherLib.Iso.PsxIso.KnownPosition)PalettePosition).StartLocation :
                ((PatcherLib.Iso.PspIso.KnownPosition)PalettePosition).StartLocation;

            string result = string.Format(
@"<{0}>
  <Name>{1}</Name>
  <Width>{2}</Width>
  <Height>{3}</Height>
  <{4}>{5}</{4}>
  <PalettePosition>
    <Offset>{6}</Offset>
    <Length>{7}</Length>
  </PalettePosition>
", this.GetType().Name, this.Name, this.Width, this.Height, sectorType, sectorValue, paletteOffset, PalettePosition.Length );

            foreach (var pos in PixelPositions)
            {
                int offset = pos is PatcherLib.Iso.PsxIso.KnownPosition ?
                    ((PatcherLib.Iso.PsxIso.KnownPosition)pos).StartLocation :
                    ((PatcherLib.Iso.PspIso.KnownPosition)pos).StartLocation;
                result += string.Format(
@"  <Position>
    <Offset>{0}</Offset>
    <Length>{1}</Length>
  </Position>
", offset, pos.Length );
            }
            result += string.Format( "</{0}>", GetType().Name );
            return result;
        }

        public static StupidTM2Image ConstructFromXml( System.Xml.XmlNode node )
        {
            ImageInfo info = GetImageInfo( node );
            var palPos = GetPalettePositionFromImageNode( info.Sector, node );

            var posNodes = node.SelectNodes( "Position" );
            PatcherLib.Iso.KnownPosition firstPosition = ParsePositionNode(info.Sector, posNodes[0]);
            PatcherLib.Iso.KnownPosition[] positions = new PatcherLib.Iso.KnownPosition[posNodes.Count-1];
            for ( int i = 1; i < posNodes.Count; i++ )
            {
                positions[i-1] = ParsePositionNode(info.Sector, posNodes[i]);
            }

            return new StupidTM2Image( info.Name, info.Width, info.Height, palPos, firstPosition, positions );
        }

        public StupidTM2Image( string name, int width, int height,
            PatcherLib.Iso.KnownPosition palettePosition,
            PatcherLib.Iso.KnownPosition firstPixelsPosition,
            params PatcherLib.Iso.KnownPosition[] otherPixelsPositions )
            : base( name, width, height )
        {
            PalettePosition = palettePosition;
            var pixelPositions = new List<PatcherLib.Iso.KnownPosition>( 1 + otherPixelsPositions.Length );
            pixelPositions.Add( firstPixelsPosition );
            pixelPositions.AddRange( otherPixelsPositions );

            int sum = 0;
            pixelPositions.ForEach( kp => sum += kp.Length );

            PixelPositions = pixelPositions.AsReadOnly();

            var position = firstPixelsPosition;
            if (position is PatcherLib.Iso.PsxIso.KnownPosition)
            {
                var pos = position as PatcherLib.Iso.PsxIso.KnownPosition;
                saveFileName = string.Format( "{0}_{1}.png", pos.Sector, pos.StartLocation );
            }
            else if (position is PatcherLib.Iso.PspIso.KnownPosition)
            {
                var pos = position as PatcherLib.Iso.PspIso.KnownPosition;
                saveFileName = string.Format( "{0}_{1}.png", pos.SectorEnum, pos.StartLocation );
            }
        }

        protected IList<byte> GetAllPixelBytes(System.IO.Stream iso)
        {
            List<byte> pixels = new List<byte>();
            PixelPositions.ForEach( pp => pixels.AddRange( pp.ReadIso( iso ) ) );
            return pixels.AsReadOnly();
        }

        protected Palette GetPalette( System.IO.Stream iso )
        {
            return new Palette( PalettePosition.ReadIso( iso ) );
        }

        protected virtual IList<byte> GetPaletteBytes( Set<Color> colors )
        {
            List<byte> result = new List<byte>( colors.Count * 2 );
            List<Color> colorList = new List<Color>( colors );
            int transparentIndex = colorList.FindIndex( c => c.A == 0 );
            if (transparentIndex != -1)
            {
                Color trans = colorList[transparentIndex];
                trans = Color.FromArgb( 255, trans.R, trans.G, trans.B );
                colorList.RemoveAt( transparentIndex );
                colorList.Insert( 0, trans );
            }

            foreach (Color c in colorList)
            {
                result.AddRange( Palette.ColorToBytes( c ) );
            }

            return result.AsReadOnly();
        }

        protected override System.Drawing.Bitmap GetImageFromIsoInner( System.IO.Stream iso )
        {
            var pixels = GetAllPixelBytes( iso );

            Palette palette = GetPalette( iso );

            Bitmap result = new Bitmap( Width, Height );

            for ( int i = 0; i < Width * Height; i++ )
            {
                result.SetPixel( i % Width, i / Width, palette[pixels[i]] );
            }

            return result;
        }
        private string saveFileName;

        protected override string SaveFileName
        {
            get { return saveFileName; }
        }

        protected virtual IList<byte> PixelsToBytes( IList<byte> pixels )
        {
            return pixels;
        }

        protected override void WriteImageToIsoInner( System.IO.Stream iso, System.Drawing.Image image )
        {
            using ( System.Drawing.Bitmap sourceBitmap = new System.Drawing.Bitmap( image ) )
            {
                Set<System.Drawing.Color> colors = GetColors( sourceBitmap );
                if ( colors.Count > NumColors )
                {
                    ImageQuantization.OctreeQuantizer q = new ImageQuantization.OctreeQuantizer( NumColors, 8 );
                    using ( var newBmp = q.Quantize( sourceBitmap ) )
                    {
                        WriteImageToIsoInner( iso, newBmp );
                    }
                }
                else
                {
                    IList<byte> bytes = new List<byte>( Width * Height );
                    for ( int y = 0; y < Height; y++ )
                    {
                        for ( int x = 0; x < Width; x++ )
                        {
                            bytes.Add( (byte)colors.IndexOf( sourceBitmap.GetPixel( x, y ) ) );
                        }
                    }

                    bytes = PixelsToBytes( bytes );

                    int currentIndex = 0;
                    foreach ( var kp in PixelPositions )
                    {
                        var bb = bytes.Sub( currentIndex, currentIndex + kp.Length - 1 );
                        kp.PatchIso( iso, bb );
                        currentIndex += kp.Length;
#if DEBUG
                        Console.Out.WriteLine( "<Location file=\"WORLD_WLDTEX_TM2\" offset=\"{0:X}\">", (kp as PatcherLib.Iso.PsxIso.KnownPosition).StartLocation );
                        foreach (byte b in bb)
                        {
                            Console.Out.Write( "{0:X2}", b );
                        }
                        Console.Out.WriteLine();
                        Console.Out.WriteLine( "</Location>" );
#endif
                    }

                    PalettePosition.PatchIso( iso, GetPaletteBytes( colors ) );
                }
            }
        }
    }
}
