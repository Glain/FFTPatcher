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
    public class PalettedImage4bpp : AbstractImage, ISelectablePalette4bppImage
    {
        public bool ImportExport8bpp { get; set; }

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

            PalettedImage4bpp image = new PalettedImage4bpp( info.Name, info.Width, info.Height, 1, depth, pos, palPos );
            image.PaletteCount = info.PaletteCount;
            image.DefaultPalette = info.DefaultPalette;
            image.CurrentPalette = info.CurrentPalette;
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
                saveFileName = string.Format( "{0}_{1}_{2}.bmp", pos.Sector, pos.StartLocation, pos.Length );
            }
            else if ( position is PatcherLib.Iso.PspIso.KnownPosition )
            {
                var pos = position as PatcherLib.Iso.PspIso.KnownPosition;
                saveFileName = string.Format( "{0}_{1}_{2}.bmp", pos.SectorEnum, pos.StartLocation, pos.Length );
            }
        }

        private string saveFileName;
        protected override string SaveFileName
        {
            get { return saveFileName; }
        }

        private Palette.ColorDepth depth;
        protected Palette.ColorDepth Depth
        {
            get { return depth; }
        }

        private PatcherLib.Iso.KnownPosition palettePosition;
        protected PatcherLib.Iso.KnownPosition PalettePosition
        {
            get { return palettePosition; }
        }

        protected override System.Drawing.Bitmap GetImageFromIsoInner( System.IO.Stream iso )
        {
            PatcherLib.Iso.KnownPosition newPalettePosition = palettePosition.AddOffset(CurrentPalette * palettePosition.Length, 0);
            Palette p = new Palette(newPalettePosition.ReadIso(iso), depth);

            byte[] bytes = GetIsoBytes( iso );
            List<byte> splitBytes = new List<byte>( bytes.Length * 2 );
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

        public override string InputFilenameFilter
        {
            get
            {
                return (ImportExport8bpp ? "8bpp" : "4bpp") + " paletted BMP/PNG (*.bmp, *.png)|*.bmp;*.png";
            }
        }

        public override string FilenameFilter
        {
            get
            {
                string strBpp = (ImportExport8bpp ? "8bpp" : "4bpp");
                return strBpp + " paletted BMP (*.bmp)|*.bmp|" + strBpp + " paletted PNG (*.png)|*.png";
            }
        }

        public override void SaveImage( System.IO.Stream iso, System.IO.Stream output, ImageFormat format )
        {
            if (ImportExport8bpp)
            {
                SaveImage8bpp(iso, output, format);
                return;
            }

            byte[] imageBytes = GetIsoBytes(iso);
            int imageByteIndex = 0;
            bool useHighNibble = false;

            PatcherLib.Iso.KnownPosition newPalettePosition = palettePosition.AddOffset(CurrentPalette * palettePosition.Length, 0);
            Palette p = new Palette(newPalettePosition.ReadIso(iso), depth, true);

            using ( Bitmap bmp = new Bitmap( Width, Height, PixelFormat.Format4bppIndexed ) )
            {
                ColorPalette pal = bmp.Palette;
                for ( int i = 0; i < p.Colors.Length; i++ )
                {
                    pal.Entries[i] = p.Colors[i];
                }
                bmp.Palette = pal;

                BitmapData bmd = bmp.LockBits( new Rectangle( 0, 0, Width, Height ), ImageLockMode.WriteOnly, PixelFormat.Format4bppIndexed );
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        byte currentByte = imageBytes[imageByteIndex];
                        int index = useHighNibble ? currentByte.GetUpperNibble() : currentByte.GetLowerNibble();
                        
                        bmd.SetPixel4bpp(x, y, index);

                        useHighNibble = !useHighNibble;
                        imageByteIndex = useHighNibble ? imageByteIndex : (imageByteIndex + 1);
                    }
                }
                bmp.UnlockBits( bmd );

                // Write that shit
                bmp.Save(output, format);
            }
        }

        protected override void WriteImageToIsoInner( System.IO.Stream iso, Bitmap image, ImageFormat format )
        {
            if (ImportExport8bpp)
            {
                WriteImageToIsoInner8bpp(iso, image, format);
                return;
            }

            //using ( Bitmap bmp = new Bitmap( image ) )
            //{
            Bitmap bmp = image;

                Set<Color> colors = GetColors( bmp );
                IList<Color> myColors = new List<Color>( colors );
                if ( myColors.Count > 16 )
                {
                    ImageQuantization.OctreeQuantizer q = new ImageQuantization.OctreeQuantizer( 16, 8 );
                    using ( var newBmp = q.Quantize( bmp ) )
                    {
                        WriteImageToIsoInner( iso, newBmp, format );
                    }
                }
                else
                {
                    PatcherLib.Iso.KnownPosition newPalettePosition = palettePosition.AddOffset(CurrentPalette * palettePosition.Length, 0);
                    IList<Byte> originalPaletteBytes = newPalettePosition.ReadIso(iso);
                    byte[] imageBytes = GetImageBytesByFormat(bmp, format, true, true);
                    List<Byte> paletteBytes = GetPaletteBytes(image.Palette.Entries, originalPaletteBytes);

                    newPalettePosition.PatchIso(iso, paletteBytes);
                    PatchIsoBytes(iso, imageBytes);
                }
            //}
        }

        protected List<byte> GetPaletteBytes(IEnumerable<Color> colors, IList<byte> originalPaletteBytes)
        {
            List<byte> result = new List<byte>();
            int index = 0;

            foreach (Color c in colors)
            {
                //byte alphaByte = originalPaletteBytes[index * 2 + 1];
                byte alphaByte = (depth == FFTPatcher.SpriteEditor.Palette.ColorDepth._32bit ? originalPaletteBytes[index * 4 + 3] : originalPaletteBytes[index * 2 + 1]);
                result.AddRange(Palette.ColorToBytes(c, alphaByte, depth));
                index++;
            }

            return result;
        }

        /*
        // The standard Windows Image method doesn't work to load in the indeces from a 4bpp paletted bitmap, 
        // so just load in the data directly by opening the bitmap as a binary file.
        protected byte[] GetImageBytes(Bitmap image)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(ImportFilename);
            int combinedWidth = (image.Width + 1) / 2;
            int stride = CalculateStride(4);
            int resultStride = image.Width / 2;

            byte[] resultData = new byte[image.Height * resultStride];
            int imageDataOffset = fileBytes[10] | (fileBytes[11] << 8) | (fileBytes[12] << 16) | (fileBytes[13] << 24);

            for (int rowIndex = 0; rowIndex < image.Height; rowIndex++)
            {
                for (int colIndex = 0; colIndex < combinedWidth; colIndex++)
                {
                    int currentByteIndex = imageDataOffset + (rowIndex * stride) + colIndex;
                    int resultByteIndex = ((image.Height - rowIndex - 1) * resultStride) + colIndex;
                    byte currentByte = fileBytes[currentByteIndex];
                    resultData[resultByteIndex] = (byte)(((currentByte & 0x0F) << 4) | ((currentByte & 0xF0) >> 4));
                }
            }

            return resultData;
        }
        */

        protected virtual PalettedImage8bpp Get8BitPalettedBitmap()
        {
            int numRemainingPalettes = PaletteCount - CurrentPalette;
            PatcherLib.Iso.KnownPosition newPalettePosition = palettePosition.AddOffset(CurrentPalette * palettePosition.Length, palettePosition.Length * Math.Min((numRemainingPalettes - 1), 15));
            PalettedImage8bpp image = new PalettedImage8bpp(Name, Width, Height, NumPalettes, depth, position, newPalettePosition, true);
            image.ImportFilename = ImportFilename;
            return image;
        }

        protected void SaveImage8bpp(System.IO.Stream iso, System.IO.Stream output, ImageFormat format, PalettedImage8bpp image8bpp = null)
        {
            if (image8bpp == null)
                image8bpp = Get8BitPalettedBitmap();

            //image8bpp.SaveImageSpecific(iso, output, GetImageFromIso(iso), true);
            //image8bpp.SaveImageSpecific(iso, output, true);
            image8bpp.SaveImageSpecific(iso, output, format, true, GetIsoBytes(iso));
        }

        protected void WriteImageToIsoInner8bpp(System.IO.Stream iso, Bitmap image, ImageFormat format, PalettedImage8bpp image8bpp = null)
        {
            if (image8bpp == null)
                image8bpp = Get8BitPalettedBitmap();

            image8bpp.WriteImageToIsoInnerSpecific(iso, image, format, true);
        }

        protected virtual byte[] GetIsoBytes(System.IO.Stream iso)
        {
            return position.ReadIso(iso);
        }

        protected virtual void PatchIsoBytes(System.IO.Stream iso, byte[] bytes)
        {
            position.PatchIso(iso, bytes);
        }
    }
}
