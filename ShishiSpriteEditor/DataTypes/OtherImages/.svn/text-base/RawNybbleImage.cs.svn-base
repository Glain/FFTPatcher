using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Utilities;
using System.Drawing;
namespace FFTPatcher.SpriteEditor
{
    class RawNybbleImage : AbstractImage
    {
        private PatcherLib.Iso.KnownPosition position;

        public static RawNybbleImage ConstructFromXml( System.Xml.XmlNode node )
        {
            ImageInfo info = GetImageInfo( node );
            var pos = GetPositionFromImageNode( info.Sector, node );
            return new RawNybbleImage( info.Name, info.Width, info.Height, pos );
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

            return string.Format(
@"<{0}>
  <Name>{1}</Name>
  <Width>{2}</Width>
  <Height>{3}</Height>
  <{4}>{5}</{4}>
  <Position>
    <Offset>{6}</Offset>
    <Length>{7}</Length>
  </Position>
</{0}>", this.GetType().Name, this.Name, this.Width, this.Height, sectorType, sectorValue, offset, position.Length);
        }

        public RawNybbleImage( string name, int width, int height, PatcherLib.Iso.KnownPosition position )
            : base( name, width, height )
        {
            this.position = position;
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

        protected override System.Drawing.Bitmap GetImageFromIsoInner( System.IO.Stream iso )
        {
            var bytes = position.ReadIso( iso );
            var pixels = new List<byte>( bytes.Count * 2 );
            foreach (byte b in bytes)
            {
                pixels.Add( b.GetLowerNibble() );
                pixels.Add( b.GetUpperNibble() );
            }
            System.Drawing.Bitmap result = new System.Drawing.Bitmap( Width,Height );
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    byte b = pixels[y * Width + x];
                    result.SetPixel( x, y, System.Drawing.Color.FromArgb( b << 4, b << 4, b << 4 ) );
                }
            }
            return result;
        }

        public override void SaveImage( System.IO.Stream iso, System.IO.Stream output )
        {
            base.SaveImage( iso, output );
        }

        private static IList<System.Drawing.Color> greyscalePalette = new Color[16] {
            Color.FromArgb(0,0,0),
            Color.FromArgb(0x10,0x10,0x10),
            Color.FromArgb(0x20,0x20,0x20),
            Color.FromArgb(0x30,0x30,0x30),
            Color.FromArgb(0x40,0x40,0x40),
            Color.FromArgb(0x50,0x50,0x50),
            Color.FromArgb(0x60,0x60,0x60),
            Color.FromArgb(0x70,0x70,0x70),
            Color.FromArgb(0x80,0x80,0x80),
            Color.FromArgb(0x90,0x90,0x90),
            Color.FromArgb(0xA0,0xA0,0xA0),
            Color.FromArgb(0xB0,0xB0,0xB0),
            Color.FromArgb(0xC0,0xC0,0xC0),
            Color.FromArgb(0xD0,0xD0,0xD0),
            Color.FromArgb(0xE0,0xE0,0xE0),
            Color.FromArgb(0xF0,0xF0,0xF0)
        }.AsReadOnly();

        protected override void WriteImageToIsoInner( System.IO.Stream iso, System.Drawing.Image image )
        {
            ImageQuantization.PaletteQuantizer q = new ImageQuantization.PaletteQuantizer( greyscalePalette );
            
            using (System.Drawing.Bitmap b = q.Quantize( image ))
            {
                List<byte> pixels = new List<byte>();
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        pixels.Add( (byte)((b.GetPixel( x, y ).R & 0xF0) >> 4) );
                    }
                }

                List<byte> bytes = new List<byte>( pixels.Count / 2 );
                for (int i = 0; i < pixels.Count; i += 2)
                {
                    bytes.Add( (byte)(((pixels[i + 1] & 0x0F) << 4) | (pixels[i] & 0x0F)) );
                }
                position.PatchIso( iso, bytes );
            }
        }
    }
}
