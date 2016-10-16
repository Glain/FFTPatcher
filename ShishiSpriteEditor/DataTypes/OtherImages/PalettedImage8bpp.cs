using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using PatcherLib.Datatypes;
using System.Xml;
using System.Drawing.Imaging;

namespace FFTPatcher.SpriteEditor
{
    public class PalettedImage8bpp : AbstractImage
    {
        protected PatcherLib.Iso.KnownPosition position;

        public static PalettedImage8bpp ConstructFromXml( XmlNode node )
        {
            ImageInfo info = GetImageInfo(node);

            var palPos = GetPalettePositionFromImageNode(info.Sector, node);
            var pos = GetPositionFromImageNode(info.Sector, node);
            var depth = GetColorDepth(node);

            PalettedImage8bpp image = new PalettedImage8bpp(info.Name, info.Width, info.Height, 1, depth, pos, palPos);
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

        public PalettedImage8bpp( 
            string name, 
            int width, int height, 
            int numPalettes, Palette.ColorDepth depth,
            PatcherLib.Iso.KnownPosition imagePosition, 
            PatcherLib.Iso.KnownPosition palettePosition,
            bool ignoreAssert = false)
            : base( name, width, height )
        {
            this.position = imagePosition;
            this.palettePosition = palettePosition;
            this.depth = depth;

            if (!ignoreAssert)
            {
                System.Diagnostics.Debug.Assert(palettePosition.Length == 256 * (int)depth * numPalettes);
            }

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

        private FFTPatcher.SpriteEditor.Palette.ColorDepth depth;
        private PatcherLib.Iso.KnownPosition palettePosition;

        protected override System.Drawing.Bitmap GetImageFromIsoInner( System.IO.Stream iso )
        {
            Palette p = new Palette( palettePosition.ReadIso( iso ), depth );
            //IList<byte> bytes = position.ReadIso( iso );
            IList<byte> bytes = GetIsoBytes(iso);

            Bitmap result = new Bitmap( Width, Height );

            for (int i = 0; i < Width * Height; i++)
            {
                result.SetPixel( i % Width, i / Width, p[bytes[i]] );
            }

            return result;
        }

        public override string InputFilenameFilter
        {
            get
            {
                return FilenameFilter;
            }
        }

        public override string FilenameFilter
        {
            get
            {
                //return "GIF file (*.gif)|*.gif";
                return "8bpp paletted bitmap (*.bmp)|*.bmp";
            }
        }

        public override void SaveImage(System.IO.Stream iso, System.IO.Stream output)
        {
            SaveImageSpecific(iso, output, false);
        }

        //public void SaveImageSpecific( System.IO.Stream iso, System.IO.Stream output, Bitmap originalImage = null, bool isSource4bpp = false )
        public void SaveImageSpecific(System.IO.Stream iso, System.IO.Stream output, bool isSource4bpp = false, IList<byte> imageBytes = null)
        {
            //imageBytes = imageBytes ?? position.ReadIso(iso);
            imageBytes = imageBytes ?? GetIsoBytes(iso);
            
            if (isSource4bpp)
            {
                List<byte> newImageBytes = new List<byte>();
                foreach (byte imageByte in imageBytes)
                {
                    newImageBytes.Add((byte)(imageByte & 0x0F));
                    newImageBytes.Add((byte)((imageByte & 0xF0) >> 4));
                }
                imageBytes = newImageBytes;
            }

            // Get colors
            //Set<Color> colors = GetColors( iso );
            PatcherLib.Iso.KnownPosition newPalettePosition = palettePosition.AddOffset(CurrentPalette * palettePosition.Length, 0);
            Palette p = new Palette(newPalettePosition.ReadIso(iso), depth, true);

            // Convert colors to indices
            //if (originalImage == null)
            //    originalImage = GetImageFromIso(iso);

            using ( Bitmap bmp = new Bitmap( Width, Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed ) )
            {
                ColorPalette pal = bmp.Palette;
                for (int i = 0; i < p.Colors.Length; i++)
                {
                    pal.Entries[i] = p.Colors[i];
                }
                bmp.Palette = pal;

                var bmd = bmp.LockBits( new Rectangle( 0, 0, Width, Height ), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        bmd.SetPixel8bpp(x, y, imageBytes[(y * Width) + x]);
                    }
                }
                bmp.UnlockBits( bmd );

                // Write that shit
                //bmp.Save( output, System.Drawing.Imaging.ImageFormat.Gif );
                bmp.Save(output, System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }

        protected override void WriteImageToIsoInner(System.IO.Stream iso, System.Drawing.Image image)
        {
            WriteImageToIsoInnerSpecific(iso, image, false);
        }

        public void WriteImageToIsoInnerSpecific(System.IO.Stream iso, System.Drawing.Image image, bool isDest4bpp = false)
        {
            using ( Bitmap bmp = new Bitmap( image ) )
            {
                Set<Color> colors = GetColors( bmp );
                if ( colors.Count > 256 )
                {
                    ImageQuantization.OctreeQuantizer q = new ImageQuantization.OctreeQuantizer( 256,  8 );
                    using ( var newBmp = q.Quantize( bmp ) )
                    {
                        WriteImageToIsoInner( iso, newBmp );
                    }
                }
                else
                {
                    PatcherLib.Iso.KnownPosition newPalettePosition = palettePosition.AddOffset(CurrentPalette * palettePosition.Length, 0);
                    byte[] originalPaletteBytes = newPalettePosition.ReadIso(iso);
                    byte[] imageBytes = GetImageBytes(bmp, isDest4bpp);
                    List<Byte> paletteBytes = GetPaletteBytes(image.Palette.Entries, originalPaletteBytes);

                    newPalettePosition.PatchIso(iso, paletteBytes);
                    //position.PatchIso(iso, imageBytes);
                    PatchIsoBytes(iso, imageBytes, isDest4bpp);
                }
            }
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

        protected byte[] GetImageBytes(Bitmap image, bool is4bpp = false)
        {
            List<byte> result = new List<byte>(Width * Height);
            
            /*
             * This gets back wrong color values - don't know why - just load the data directly from the bitmap file instead
             * 
            System.Drawing.Imaging.BitmapData bmd = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, 
                System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = bmd.GetPixel(x, y);
                    result.Add((byte)index);
                }
            }
            
            image.UnlockBits(bmd);
            */

            byte[] fileBytes = System.IO.File.ReadAllBytes(ImportFilename);
            int stride = CalculateStride(8);
            int resultStride = CalculateStride(is4bpp ? 4 : 8);
            byte[] resultData = new byte[image.Height * resultStride];
            int imageDataOffset = fileBytes[10] | (fileBytes[11] << 8) | (fileBytes[12] << 16) | (fileBytes[13] << 24);

            for (int rowIndex = 0; rowIndex < image.Height; rowIndex++)
            {
                for (int colIndex = 0; colIndex < image.Width; colIndex++)
                {
                    int currentByteIndex = imageDataOffset + (rowIndex * stride) + colIndex;
                    int resultByteIndex = ((image.Height - rowIndex - 1) * resultStride) + (is4bpp ? (colIndex / 2) : colIndex);
                    byte currentByte = fileBytes[currentByteIndex];

                    if (is4bpp)
                    {
                        resultData[resultByteIndex] |= (((colIndex & 0x01) == 0) ? ((byte)(currentByte & 0x0F)) : ((byte)((currentByte & 0x0F) << 4)));
                    }
                    else
                    {
                        resultData[resultByteIndex] = currentByte;
                    }
                }
            }

            return resultData;
        }

        protected virtual byte[] GetIsoBytes(System.IO.Stream iso)
        {
            return position.ReadIso(iso);
        }

        protected virtual void PatchIsoBytes(System.IO.Stream iso, byte[] bytes, bool is4bpp)
        {
            position.PatchIso(iso, bytes);
        }
    }
}
