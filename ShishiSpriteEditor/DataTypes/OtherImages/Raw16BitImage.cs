using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Drawing.Imaging;

namespace FFTPatcher.SpriteEditor
{
    class Raw16BitImage : AbstractImage
    {
        private PatcherLib.Iso.KnownPosition position;

        public static Raw16BitImage ConstructFromXml(XmlNode node)
        {
            ImageInfo info = GetImageInfo(node);
            var pos = GetPositionFromImageNode(info.Sector, node);
            Raw16BitImage image = new Raw16BitImage(info.Name, info.Width, info.Height, pos);
            image.OriginalFilename = info.OriginalFilename;
            image.Filesize = info.Filesize;
            image.Sector = info.Sector;
            return image;
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

        public Raw16BitImage(string name, int width, int height, PatcherLib.Iso.KnownPosition position)
            : base(name, width, height)
        {
            this.position = position;
            if (position is PatcherLib.Iso.PsxIso.KnownPosition)
            {
                var pos = position as PatcherLib.Iso.PsxIso.KnownPosition;
                saveFileName = string.Format("{0}_{1}_{2}.bmp", pos.Sector, pos.StartLocation, pos.Length);
            }
            else if (position is PatcherLib.Iso.PspIso.KnownPosition)
            {
                var pos = position as PatcherLib.Iso.PspIso.KnownPosition;
                saveFileName = string.Format("{0}_{1}_{2}.bmp", pos.SectorEnum, pos.StartLocation, pos.Length);
            }
        }

        private string saveFileName;
        protected override string SaveFileName
        {
            get { return saveFileName; }
        }

        public override string InputFilenameFilter
        {
            get
            {
                return "BMP/PNG image (*.bmp, *.png)|*.bmp;*.png";
            }
        }

        public override string FilenameFilter
        {
            get
            {
                return "Bitmap image (*.bmp)|*.bmp|PNG image (*.png)|*.png";
                //return "PNG file (*.png)|*.png";
            }
        }

        protected override Bitmap GetImageFromIsoInner( System.IO.Stream iso )
        {
            IList<byte> bytes = position.ReadIso( iso );
            IList<Color> pixels = new Color[bytes.Count / 2];
            for ( int i = 0; i < pixels.Count; i++ )
            {
                pixels[i] = Palette.BytesToColor(bytes[i * 2], bytes[i * 2 + 1]);
            }

            Bitmap result = new Bitmap( Width, Height, PixelFormat.Format16bppArgb1555 );
            for ( int x = 0; x < Width; x++ )
            {
                for ( int y = 0; y < Height; y++ )
                {
                    result.SetPixel( x, y, pixels[y * Width + x] );
                }
            }
            return result;
        }

        // This seems to save as a 24 bpp bitmap, even though the format was set differently above.
        // That cuts off the alpha value, so we have to preserve the alpha when re-importing.
        // As long as we do that, the 24 bpp save actually seems to work just fine.
        public override void SaveImage(System.IO.Stream iso, System.IO.Stream output, System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Bitmap bmp = GetImageFromIso(iso);
            bmp.Save(output, format);
        }

        protected override void WriteImageToIsoInner( System.IO.Stream iso, Bitmap image, ImageFormat format )
        {
            byte[] originalBytes = position.ReadIso(iso);

            IList<Color> pixels = new Color[Width * Height];
            Bitmap bmp = image;
            //using ( Bitmap bmp = new Bitmap( image ) )
            //{
                for ( int x = 0; x < Width; x++ )
                {
                    for ( int y = 0; y < Height; y++ )
                    {
                        pixels[y * Width + x] = bmp.GetPixel( x, y );
                    }
                }
            //}

            byte[] result = new byte[pixels.Count * 2];
            for ( int i = 0; i < pixels.Count; i++ )
            {
                byte alphaByte = originalBytes[i * 2 + 1];
                byte[] bytes = Palette.ColorToBytes(pixels[i], alphaByte, FFTPatcher.SpriteEditor.Palette.ColorDepth._16bit);
                result[i * 2] = bytes[0];
                result[i * 2 + 1] = bytes[1];
            }

            position.PatchIso( iso, result );
        }
    }
}
