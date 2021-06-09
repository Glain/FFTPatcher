using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using PatcherLib.Utilities;
using PatcherLib.Datatypes;

namespace FFTPatcher.SpriteEditor
{
    public class StupidTM2Image4bpp : StupidTM2Image, ISelectablePalette4bppImage
    {
        public bool ImportExport8bpp { get; set; }

        protected override int NumColors
        {
            get
            {
                return 16;
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
                return (ImportExport8bpp ? "8bpp" : "4bpp") + " paletted bitmap (*.bmp)|*.bmp";
            }
        }

        public StupidTM2Image4bpp( string name, int width, int height,
            PatcherLib.Iso.KnownPosition palettePosition,
            PatcherLib.Iso.KnownPosition firstPixelsPosition,
            params PatcherLib.Iso.KnownPosition[] otherPixelsPositions )
            : base( name, width, height, palettePosition, firstPixelsPosition, otherPixelsPositions)
        {
        }

        public static StupidTM2Image4bpp ConstructFromXml(System.Xml.XmlNode node)
        {
            ImageInfo info = GetImageInfo(node);
            var palPos = GetPalettePositionFromImageNode(info.Sector, node);

            var posNodes = node.SelectNodes("Position");
            PatcherLib.Iso.KnownPosition firstPosition = ParsePositionNode(info.Sector, posNodes[0]);
            PatcherLib.Iso.KnownPosition[] positions = new PatcherLib.Iso.KnownPosition[posNodes.Count - 1];
            for (int i = 1; i < posNodes.Count; i++)
            {
                positions[i - 1] = ParsePositionNode(info.Sector, posNodes[i]);
            }

            StupidTM2Image4bpp image = new StupidTM2Image4bpp(info.Name, info.Width, info.Height, palPos, firstPosition, positions);
            image.PaletteCount = info.PaletteCount;
            image.DefaultPalette = info.DefaultPalette;
            image.CurrentPalette = info.CurrentPalette;
            image.OriginalFilename = info.OriginalFilename;
            image.Filesize = info.Filesize;
            image.Sector = info.Sector;

            return image;
        }

        protected override IList<byte> PixelsToBytes( IList<byte> pixels )
        {
            List<byte> result = new List<byte>( pixels.Count / 2 );

            for ( int i = 0; i < pixels.Count; i += 2 )
            {
                result.Add( (byte)( pixels[i] & 0x0F | ( ( pixels[i + 1] & 0x0F ) << 4 ) ) );
            }

            return result.AsReadOnly();
        }

        protected override System.Drawing.Bitmap GetImageFromIsoInner( System.IO.Stream iso )
        {
            List<byte> pixels = GetAllPixelBytes( iso );
            //var palette = GetPalette( iso );

            PatcherLib.Iso.KnownPosition newPalettePosition = PalettePosition.AddOffset(CurrentPalette * PalettePosition.Length, 0);
            Palette palette = new Palette(newPalettePosition.ReadIso(iso), FFTPatcher.SpriteEditor.Palette.ColorDepth._16bit, true);

            Bitmap result = new Bitmap( Width, Height );

            var mypixels = new List<byte>();
            foreach ( byte p in pixels )
            {
                mypixels.Add( p.GetLowerNibble() );
                mypixels.Add( p.GetUpperNibble() );
            }
            pixels = mypixels;

            for ( int i = 0; i < Width * Height; i++ )
            {
                result.SetPixel( i % Width, i / Width, palette[pixels[i]] );
            }

            return result;
        }

        public override void SaveImage(System.IO.Stream iso, System.IO.Stream output)
        {
            if (ImportExport8bpp)
            {
                SaveImage8bpp(iso, output);
                return;
            }

            List<byte> imageBytes = GetAllPixelBytes(iso);
            int imageByteIndex = 0;
            bool useHighNibble = false;

            PatcherLib.Iso.KnownPosition newPalettePosition = PalettePosition.AddOffset(CurrentPalette * PalettePosition.Length, 0);
            Palette p = new Palette(newPalettePosition.ReadIso(iso), FFTPatcher.SpriteEditor.Palette.ColorDepth._16bit, true);

            using (Bitmap bmp = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format4bppIndexed))
            {
                System.Drawing.Imaging.ColorPalette pal = bmp.Palette;
                for (int i = 0; i < p.Colors.Length; i++)
                {
                    pal.Entries[i] = p.Colors[i];
                }
                bmp.Palette = pal;

                System.Drawing.Imaging.BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format4bppIndexed);
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
                bmp.UnlockBits(bmd);

                // Write that shit
                bmp.Save(output, System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }

        protected override void WriteImageToIsoInner(System.IO.Stream iso, System.Drawing.Image image)
        {
            if (ImportExport8bpp)
            {
                WriteImageToIsoInner8bpp(iso, image);
                return;
            }

            using (Bitmap bmp = new Bitmap(image))
            {
                Set<Color> colors = GetColors(bmp);
                IList<Color> myColors = new List<Color>(colors);
                if (myColors.Count > 16)
                {
                    ImageQuantization.OctreeQuantizer q = new ImageQuantization.OctreeQuantizer(16, 8);
                    using (var newBmp = q.Quantize(bmp))
                    {
                        WriteImageToIsoInner(iso, newBmp);
                    }
                }
                else
                {
                    PatcherLib.Iso.KnownPosition newPalettePosition = PalettePosition.AddOffset(CurrentPalette * PalettePosition.Length, 0);
                    IList<Byte> originalPaletteBytes = newPalettePosition.ReadIso(iso);
                    byte[] imageBytes = GetImageBytes(bmp);
                    List<Byte> paletteBytes = GetPaletteBytes(image.Palette.Entries, originalPaletteBytes);

                    newPalettePosition.PatchIso(iso, paletteBytes);

                    int currentIndex = 0;
                    foreach (PatcherLib.Iso.KnownPosition kp in PixelPositions)
                    {
                        IList<byte> bb = imageBytes.Sub(currentIndex, currentIndex + kp.Length - 1);
                        kp.PatchIso(iso, bb);
                        currentIndex += kp.Length;
                    }

                    //PatchIsoBytes(iso, imageBytes);
                }
            }
        }

        protected new List<byte> GetPaletteBytes(IEnumerable<Color> colors, IList<Byte> originalPaletteBytes)
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

        // The standard Windows Image method doesn't work to load in the indeces from a 4bpp paletted bitmap, 
        // so just load in the data directly by opening the bitmap as a binary file.
        protected new byte[] GetImageBytes(Bitmap image)
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

        protected StupidTM2Image Get8BitPalettedBitmap()
        {
            PatcherLib.Iso.KnownPosition newPalettePosition = PalettePosition.AddOffset(CurrentPalette * PalettePosition.Length, PalettePosition.Length * 15);
            StupidTM2Image image = new StupidTM2Image(Name, Width, Height, newPalettePosition, PixelPositions.ToArray());
            image.ImportFilename = ImportFilename;
            return image;
        }

        protected void SaveImage8bpp(System.IO.Stream iso, System.IO.Stream output, StupidTM2Image image8bpp = null)
        {
            if (image8bpp == null)
                image8bpp = Get8BitPalettedBitmap();

            image8bpp.SaveImageSpecific(iso, output, true);
        }

        protected void WriteImageToIsoInner8bpp(System.IO.Stream iso, System.Drawing.Image image, StupidTM2Image image8bpp = null)
        {
            if (image8bpp == null)
                image8bpp = Get8BitPalettedBitmap();

            image8bpp.WriteImageToIsoInnerSpecific(iso, image, true);
        }
    }
}
