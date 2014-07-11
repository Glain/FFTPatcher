using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;

namespace FFTPatcher.SpriteEditor
{
    class Raw16BitImage : AbstractImage
    {
        private PatcherLib.Iso.KnownPosition position;

        public static Raw16BitImage ConstructFromXml( XmlNode node )
        {
            ImageInfo info = GetImageInfo( node );
            var pos = GetPositionFromImageNode( info.Sector, node );
            return new Raw16BitImage( info.Name, info.Width, info.Height, pos );
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

        public Raw16BitImage( string name, int width, int height, PatcherLib.Iso.KnownPosition position )
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
            IList<byte> bytes = position.ReadIso( iso );
            IList<Color> pixels = new Color[bytes.Count / 2];
            for ( int i = 0; i < pixels.Count; i++ )
            {
                pixels[i] = Palette.BytesToColor( bytes[i * 2], bytes[i * 2 + 1] );
            }

            Bitmap result = new Bitmap( Width, Height );
            for ( int x = 0; x < Width; x++ )
            {
                for ( int y = 0; y < Height; y++ )
                {
                    result.SetPixel( x, y, pixels[y * Width + x] );
                }
            }
            return result;
        }

        protected override void WriteImageToIsoInner( System.IO.Stream iso, System.Drawing.Image image )
        {
            IList<Color> pixels = new Color[Width * Height];
            using ( Bitmap bmp = new Bitmap( image ) )
            {
                for ( int x = 0; x < Width; x++ )
                {
                    for ( int y = 0; y < Height; y++ )
                    {
                        pixels[y * Width + x] = bmp.GetPixel( x, y );
                    }
                }
            }

            byte[] result = new byte[pixels.Count * 2];
            for ( int i = 0; i < pixels.Count; i++ )
            {
                byte[] bytes = Palette.ColorToBytes( pixels[i] );
                result[i * 2] = bytes[0];
                result[i * 2 + 1] = bytes[1];
            }

            position.PatchIso( iso, result );
        }
    }
}
