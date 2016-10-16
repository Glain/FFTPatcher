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

            StupidTM2Image image = new StupidTM2Image( info.Name, info.Width, info.Height, palPos, firstPosition, positions );
            image.OriginalFilename = info.OriginalFilename;
            image.Filesize = info.Filesize;
            image.Sector = info.Sector;

            return image;
        }

        public StupidTM2Image(string name, int width, int height,
            PatcherLib.Iso.KnownPosition palettePosition,
            PatcherLib.Iso.KnownPosition firstPixelsPosition,
            params PatcherLib.Iso.KnownPosition[] otherPixelsPositions)
            : base(name, width, height)
        {
            Build(name, width, height, palettePosition, firstPixelsPosition, otherPixelsPositions);
        }

        public StupidTM2Image(string name, int width, int height, PatcherLib.Iso.KnownPosition palettePosition, PatcherLib.Iso.KnownPosition[] pixelPositions)
            : base(name, width, height)
        {
            List<PatcherLib.Iso.KnownPosition> otherKnownPositionList = new List<PatcherLib.Iso.KnownPosition>();
            for (int index = 1; index < pixelPositions.Length; index++)
            {
                otherKnownPositionList.Add(pixelPositions[index]);
            }

            Build(name, width, height, palettePosition, pixelPositions[0], otherKnownPositionList.ToArray());
        }

        private void Build(string name, int width, int height,
            PatcherLib.Iso.KnownPosition palettePosition,
            PatcherLib.Iso.KnownPosition firstPixelsPosition,
            PatcherLib.Iso.KnownPosition[] otherPixelsPositions)
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
                saveFileName = string.Format( "{0}_{1}_{2}.bmp", pos.Sector, pos.StartLocation, pos.Length );
            }
            else if (position is PatcherLib.Iso.PspIso.KnownPosition)
            {
                var pos = position as PatcherLib.Iso.PspIso.KnownPosition;
                saveFileName = string.Format( "{0}_{1}_{2}.bmp", pos.SectorEnum, pos.StartLocation, pos.Length );
            }
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
                return "8bpp paletted bitmap (*.bmp)|*.bmp";
            }
        }

        protected List<byte> GetAllPixelBytes(System.IO.Stream iso)
        {
            List<byte> pixels = new List<byte>();
            PixelPositions.ForEach( pp => pixels.AddRange( pp.ReadIso( iso ) ) );
            return pixels;
        }

        protected Palette GetPalette( System.IO.Stream iso )
        {
            return new Palette( PalettePosition.ReadIso( iso ));
        }

        /*
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
                result.AddRange(Palette.ColorToBytes(c));
            }

            return result.AsReadOnly();
        }
        */

        protected override System.Drawing.Bitmap GetImageFromIsoInner( System.IO.Stream iso )
        {
            List<byte> pixels = GetAllPixelBytes( iso );
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

        public override void SaveImage(System.IO.Stream iso, System.IO.Stream output)
        {
            SaveImageSpecific(iso, output, false);
        }

        public void SaveImageSpecific(System.IO.Stream iso, System.IO.Stream output, bool isSource4bpp = false)
        {
            IList<byte> imageBytes = GetAllPixelBytes(iso);
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
            Palette p = new Palette(PalettePosition.ReadIso(iso), FFTPatcher.SpriteEditor.Palette.ColorDepth._16bit, true);

            // Convert colors to indices
            using (Bitmap bmp = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
            {
                System.Drawing.Imaging.ColorPalette pal = bmp.Palette;
                for (int i = 0; i < p.Colors.Length; i++)
                {
                    pal.Entries[i] = p.Colors[i];
                }
                bmp.Palette = pal;

                var bmd = bmp.LockBits(new Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        bmd.SetPixel8bpp(x, y, imageBytes[(y * Width) + x]);
                    }
                }
                bmp.UnlockBits(bmd);

                // Write that shit
                bmp.Save(output, System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }

        protected override void WriteImageToIsoInner( System.IO.Stream iso, System.Drawing.Image image )
        {
            WriteImageToIsoInnerSpecific(iso, image, false);
        }

        public void WriteImageToIsoInnerSpecific(System.IO.Stream iso, System.Drawing.Image image, bool isDest4bpp = false)
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
                    byte[] imageBytes = GetImageBytes(sourceBitmap, isDest4bpp);
                    IList<byte> bytes = PixelsToBytes(imageBytes);

                    byte[] originalPaletteBytes = PalettePosition.ReadIso(iso);
                    PalettePosition.PatchIso(iso, GetPaletteBytes(image.Palette.Entries, originalPaletteBytes));

                    int currentIndex = 0;
                    foreach ( var kp in PixelPositions )
                    {
                        var bb = bytes.Sub( currentIndex, currentIndex + kp.Length - 1 );
                        kp.PatchIso( iso, bb );
                        currentIndex += kp.Length;
                    }
                }
            }
        }

        protected byte[] GetImageBytes(Bitmap image, bool is4bpp = false)
        {
            List<byte> result = new List<byte>(Width * Height);

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

        protected List<byte> GetPaletteBytes(IEnumerable<Color> colors, IList<byte> originalPaletteBytes)
        {
            List<byte> result = new List<byte>();
            int index = 0;

            foreach (Color c in colors)
            {
                byte alphaByte = originalPaletteBytes[index * 2 + 1];
                result.AddRange(Palette.ColorToBytes(c, alphaByte, FFTPatcher.SpriteEditor.Palette.ColorDepth._16bit));
                index++;
            }

            return result;
        }
    }
}
