using PatcherLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using PatcherLib.Datatypes;
using System.Xml;
using System.Drawing.Imaging;

namespace FFTPatcher.SpriteEditor
{
    public class Greyscale4bppImage : PalettedImage4bpp
    {
        public override string DescribeXml()
        {
            string sectorType = this.position is PatcherLib.Iso.PsxIso.KnownPosition ? "Sector" :
                                ((PatcherLib.Iso.PspIso.KnownPosition)position).FFTPack.HasValue ?
                                "FFTPack" : "Sector";
            string sectorValue = this.position is PatcherLib.Iso.PsxIso.KnownPosition?
                ((PatcherLib.Iso.PsxIso.KnownPosition)position).Sector.ToString() :
                ((PatcherLib.Iso.PspIso.KnownPosition)position).FFTPack.HasValue ?
                ((PatcherLib.Iso.PspIso.KnownPosition)position).FFTPack.Value.ToString() :
                ((PatcherLib.Iso.PspIso.KnownPosition)position).Sector.Value.ToString();
            int offset = this.position is PatcherLib.Iso.PsxIso.KnownPosition?
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
</{0}>", this.GetType().Name, this.Name, this.Width, this.Height, sectorType, sectorValue, offset, position.Length );
        }

        static Color[] colors = new Color[] {
            Color.FromArgb(0,0,0), Color.FromArgb(0x10, 0x10, 0x10), Color.FromArgb(0x20,0x20,0x20), Color.FromArgb(0x30,0x30,0x30),
            Color.FromArgb(0x40,0x40,0x40), Color.FromArgb(0x50, 0x50, 0x50), Color.FromArgb(0x60,0x60,0x60), Color.FromArgb(0x70,0x70,0x70),
            Color.FromArgb(0x80,0x80,0x80), Color.FromArgb(0x90, 0x90, 0x90), Color.FromArgb(0xa0,0xa0,0xa0), Color.FromArgb(0xb0,0xb0,0xb0),
            Color.FromArgb(0xC0,0xC0,0xC0), Color.FromArgb(0xD0, 0xd0, 0xd0), Color.FromArgb(0xe0,0xe0,0xe0), Color.FromArgb(0xf0,0xf0,0xf0),
        };

        public Greyscale4bppImage( string name, int width, int height, PatcherLib.Iso.KnownPosition imagePosition )
            : base( name, width, height, 1, imagePosition, new FakeGreyscalePalettePosition() )
        {
        }

        public override string FilenameFilter
        {
            get
            {
                return "4bpp greyscale bitmap (*.bmp)|*.bmp";
            }
        }

        public static new Greyscale4bppImage ConstructFromXml( XmlNode node )
        {
            ImageInfo info = GetImageInfo( node );
            var pos = GetPositionFromImageNode( info.Sector, node );
            Greyscale4bppImage image = new Greyscale4bppImage( info.Name, info.Width, info.Height, pos );
            image.OriginalFilename = info.OriginalFilename;
            image.Filesize = info.Filesize;
            image.Sector = info.Sector;
            return image;
        }

        protected override System.Drawing.Bitmap GetImageFromIsoInner(System.IO.Stream iso)
        {
            byte[] bytes = position.ReadIso(iso);
            List<byte> splitBytes = new List<byte>(bytes.Length * 2);
            foreach (byte b in bytes.Sub(0, Height * Width / 2 - 1))
            {
                splitBytes.Add(b.GetLowerNibble());
                splitBytes.Add(b.GetUpperNibble());
            }

            Bitmap result = new Bitmap(Width, Height);

            for (int i = 0; i < Width * Height; i++)
            {
                result.SetPixel(i % Width, i / Width, colors[splitBytes[i]]);
            }

            return result;
        }

        /*
        protected override void WriteImageToIsoInner( System.IO.Stream iso, Image image )
        {
            var q = new ImageQuantization.PaletteQuantizer( colors );
            using (var newImage = q.Quantize( image ))
            {
                //var imageBytes = GetImageBytes( newImage, new Set<Color>( colors ) );
                var imageBytes = GetImageBytes(newImage, colors);
                position.PatchIso( iso, imageBytes );
            }
        }
        */

        protected override void WriteImageToIsoInner(System.IO.Stream iso, Bitmap image, ImageFormat format)
        {
            Bitmap bmp = image;
            //using (Bitmap bmp = new Bitmap(image))
            //{
                byte[] imageBytes = GetImageBytesByFormat(bmp, format, true, true);
                position.PatchIso(iso, imageBytes);
            //}
        }

        private class FakeGreyscalePalettePosition : PatcherLib.Iso.KnownPosition
        {
            public override void PatchIso( System.IO.Stream iso, IList<byte> bytes )
            {
            }

            public override byte[] ReadIso( System.IO.Stream iso )
            {
                return new Palette( colors ).ToByteArray();
            }

            public override PatcherLib.Datatypes.PatchedByteArray GetPatchedByteArray( byte[] bytes )
            {
                return new PatcherLib.Datatypes.PatchedByteArray( (PatcherLib.Iso.PsxIso.Sectors)0, 0, new byte[0] );
            }

            public override int Length
            {
                get { return 16 * 2; }
            }

            public override PatcherLib.Iso.KnownPosition AddOffset(int offset, int length)
            {
                return this;
            }
        }
    }
}