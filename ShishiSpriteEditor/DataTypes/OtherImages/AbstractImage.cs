using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Datatypes;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using FFTPatcher.SpriteEditor.Helpers;
using System.Drawing.Imaging;

namespace FFTPatcher.SpriteEditor
{
    public abstract class AbstractImage : IDisposable
    {
        protected struct ImageInfo
        {
            public string Name;
            public int Width;
            public int Height;
            public Enum Sector;
            public string OriginalFilename;
            public int Filesize;

            public int PaletteCount;
            public int CurrentPalette;
            public int DefaultPalette;
        }

        public abstract string DescribeXml();

        protected static PatcherLib.Iso.KnownPosition ParsePositionNode(Enum sectorType, XmlNode node)
        {
            Int32 offset = Int32.Parse(node.SelectSingleNode("Offset").InnerText);
            Int32 length = Int32.Parse(node.SelectSingleNode("Length").InnerText);
            return PatcherLib.Iso.KnownPosition.ConstructKnownPosition(sectorType, offset, length);
        }

        protected static PatcherLib.Iso.KnownPosition GetPositionFromImageNode(Enum sectorType, XmlNode node)
        {
            return ParsePositionNode(sectorType, node.SelectSingleNode("Position"));
        }

        protected static FFTPatcher.SpriteEditor.Palette.ColorDepth GetColorDepth(XmlNode node)
        {
            return (FFTPatcher.SpriteEditor.Palette.ColorDepth)Enum.Parse(typeof(FFTPatcher.SpriteEditor.Palette.ColorDepth), node.SelectSingleNode("ColorDepth").InnerText);
        }

        protected static PatcherLib.Iso.KnownPosition GetPalettePositionFromImageNode(Enum sectorType, XmlNode node)
        {
            return ParsePositionNode(sectorType, node.SelectSingleNode("PalettePosition"));
        }

        protected static ImageInfo GetImageInfo(XmlNode node)
        {
            XmlNode nameNode = node.SelectSingleNode("Name");
            int width = Int32.Parse(node.SelectSingleNode("Width").InnerText);
            int height = Int32.Parse(node.SelectSingleNode("Height").InnerText);
            XmlNode sectorNode = node.SelectSingleNode("Sector");
            XmlNode fftpackNode = node.SelectSingleNode("FFTPack");
            XmlNode pspSector = node.SelectSingleNode("PSPSector");
            string name = nameNode.InnerText;
            XmlNode filenameNode = node.SelectSingleNode("Filename");
            XmlNode filesizeNode = node.SelectSingleNode("Filesize");
            string filename = (filenameNode == null) ? "" : filenameNode.InnerText;
            int filesize = (filesizeNode == null) ? 0 : int.Parse(filesizeNode.InnerText);

            XmlNode nameResourceNode = nameNode.SelectSingleNode("Resource");
            if (nameResourceNode != null)
            {
                string resourceName = nameResourceNode.Attributes["file"].InnerText;
                string context = nameResourceNode.Attributes["context"].InnerText;

                Type resourcesClass = context == "PSP" ? typeof(PatcherLib.PSPResources.Lists) : typeof(PatcherLib.PSXResources.Lists);
                object strings = resourcesClass.GetProperty(resourceName).GetValue(null, null);
                if (resourceName == "TownNames")
                {
                    var names = strings as IDictionary<Town, string>;
                    name = names[(Town)Enum.Parse(typeof(Town), nameResourceNode.InnerText)];
                }
                else if (resourceName == "ShopNames")
                {
                    var names = strings as IDictionary<ShopsFlags, string>;
                    name = names[(ShopsFlags)Enum.Parse(typeof(ShopsFlags), nameResourceNode.InnerText)];
                }
                else
                {
                    var names = strings as IList<string>;
                    name = names[Int32.Parse(nameResourceNode.InnerText)];
                }
            }

            XmlNode paletteCountNode = node["PaletteCount"];
            XmlNode defaultPaletteNode = node["DefaultPalette"];

            int paletteCount = ((paletteCountNode != null) && (paletteCountNode.InnerText != null)) ? int.Parse(paletteCountNode.InnerText) : 0;
            int defaultPalette = ((defaultPaletteNode != null) && (defaultPaletteNode.InnerText != null)) ? int.Parse(defaultPaletteNode.InnerText) : 0;

            if (sectorNode != null)
            {
                PatcherLib.Iso.PsxIso.Sectors sector = (PatcherLib.Iso.PsxIso.Sectors)Enum.Parse(typeof(PatcherLib.Iso.PsxIso.Sectors), sectorNode.InnerText);
                if (string.IsNullOrEmpty(filename))
                    filename = GetFilenameFromSectorName(Enum.GetName(typeof(PatcherLib.Iso.PsxIso.Sectors), sector));

                return new ImageInfo { Name = name, Width = width, Height = height,
                    Sector = sector, OriginalFilename = filename, Filesize = filesize,
                    PaletteCount = paletteCount, DefaultPalette = defaultPalette, CurrentPalette = defaultPalette
                };
            }
            else if (fftpackNode != null)
            {
                PatcherLib.Iso.FFTPack.Files sector = (PatcherLib.Iso.FFTPack.Files)Enum.Parse(typeof(PatcherLib.Iso.FFTPack.Files), fftpackNode.InnerText);
                if (string.IsNullOrEmpty(filename))
                    filename = GetFilenameFromSectorName(Enum.GetName(typeof(PatcherLib.Iso.FFTPack.Files), sector));

                return new ImageInfo { Name = name, Width = width, Height = height,
                    Sector = sector, OriginalFilename = filename, Filesize = filesize,
                    PaletteCount = paletteCount, DefaultPalette = defaultPalette, CurrentPalette = defaultPalette
                };
            }
            else if (pspSector != null)
            {
                PatcherLib.Iso.PspIso.Sectors sector = (PatcherLib.Iso.PspIso.Sectors)Enum.Parse(typeof(PatcherLib.Iso.PspIso.Sectors), pspSector.InnerText);
                if (string.IsNullOrEmpty(filename))
                    filename = GetFilenameFromSectorName(Enum.GetName(typeof(PatcherLib.Iso.PspIso.Sectors), sector));

                return new ImageInfo { Name = name, Width = width, Height = height,
                    Sector = sector, OriginalFilename = filename, Filesize = filesize,
                    PaletteCount = paletteCount, DefaultPalette = defaultPalette, CurrentPalette = defaultPalette
                };
            }
            else
            {
                throw new Exception("No valid Sector element found");
            }
        }

        public static string GetFilenameFromSectorName(string sectorName)
        {
            if (string.IsNullOrEmpty(sectorName))
                return "";

            int startIndex = sectorName.Substring(0, sectorName.LastIndexOf('_')).LastIndexOf('_') + 1;
            string filePart = sectorName.Substring(startIndex, sectorName.Length - startIndex);
            return filePart.Replace('_', '.');
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        protected abstract Bitmap GetImageFromIsoInner(System.IO.Stream iso);

        public virtual string InputFilenameFilter { get { return "BMP/PNG image (*.bmp, *.png)|*.bmp;*.png"; } }
        public virtual string FilenameFilter { get { return "BMP image (*.bmp)|*.bmp|PNG image (*.png)|*.png"; } }

        public string ImportFilename { get; set; }
        public string OriginalFilename { get; set; }
        public int Filesize { get; set; }
        public Enum Sector { get; set; }

        // Old property... possibly for loading/saving multiple palettes at once
        public int NumPalettes { get; private set; }

        // For selecting one palette to load/save
        public int PaletteCount { get; set; }
        public int DefaultPalette { get; set; }
        public int CurrentPalette { get; set; }

        public bool IsEffect { get; set; }
        public int EffectIndex { get; set; }

        System.Drawing.Bitmap cachedImage;
        bool cachedImageDirty = true;

        public int CalculateStride(int bpp)
        {
            return (((Width * bpp) + 31) / 32) * 4;
        }

        public System.Drawing.Bitmap GetImageFromIso(System.IO.Stream iso, bool forceReload = false)
        {
            try
            {
                if ((cachedImageDirty || forceReload) && cachedImage != null)
                {
                    cachedImage.Dispose();
                    cachedImage = null;
                }

                if (cachedImage == null)
                {
                    cachedImage = GetImageFromIsoInner(iso);
                    cachedImageDirty = false;
                }
                return cachedImage;
            }
            catch (Exception ex)
            {
                return cachedImage;
            }

        }

        /*
        protected IList<byte> GetPaletteBytes( Set<Color> colors, Palette.ColorDepth depth )
        {
            List<byte> result = new List<byte>();
            if ( depth == Palette.ColorDepth._16bit )
            {
                foreach ( Color c in colors )
                {
                    result.AddRange(Palette.ColorToBytes(c));
                }
            }
            else if ( depth == Palette.ColorDepth._32bit )
            {
                foreach ( Color c in colors )
                {
                    result.Add( c.R );
                    result.Add( c.G );
                    result.Add( c.B );
                    result.Add( c.A );
                }
            }

            result.AddRange( new byte[Math.Max( 0, (int)depth * 256 - result.Count )] );
            return result;
        }
        */

        protected abstract string SaveFileName { get; }

        public string GetSaveFileName(ImageFormat format = null)
        {
            if (format == null)
                format = ImageFormat.Bmp;

            return SaveFileName.Replace(".bmp", ImageFormatHelper.GetExtension(format));
        }

        protected abstract void WriteImageToIsoInner(System.IO.Stream iso, Bitmap image, ImageFormat format);

        protected virtual byte[] GetImageBytesByFormat(Bitmap image, ImageFormat format, bool isSource4bpp, bool isDest4bpp)
        {
            if (format == ImageFormat.Bmp)
            {
                if (isSource4bpp)
                    return GetImageBytesBMP_4bpp(image);
                else
                    return GetImageBytesBMP_8bpp(image, isDest4bpp);
            }
            else
            {
                return GetImageBytesFromObject(image, isSource4bpp, isDest4bpp);
            }
        }

        protected virtual byte[] GetImageBytesBMP_8bpp(Bitmap image, bool isDest4bpp)
        {
            List<byte> result = new List<byte>(Width * Height);

            byte[] fileBytes = System.IO.File.ReadAllBytes(ImportFilename);
            int stride = CalculateStride(8);
            int resultStride = isDest4bpp ? (image.Width / 2) : image.Width;
            byte[] resultData = new byte[image.Height * resultStride];
            int imageDataOffset = fileBytes[10] | (fileBytes[11] << 8) | (fileBytes[12] << 16) | (fileBytes[13] << 24);

            for (int rowIndex = 0; rowIndex < image.Height; rowIndex++)
            {
                for (int colIndex = 0; colIndex < image.Width; colIndex++)
                {
                    int currentByteIndex = imageDataOffset + (rowIndex * stride) + colIndex;
                    int resultByteIndex = ((image.Height - rowIndex - 1) * resultStride) + (isDest4bpp ? (colIndex / 2) : colIndex);
                    byte currentByte = fileBytes[currentByteIndex];

                    if (isDest4bpp)
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

        // The standard Windows Image method doesn't work to load in the indeces from a 4bpp paletted bitmap, 
        // so just load in the data directly by opening the bitmap as a binary file.
        protected virtual byte[] GetImageBytesBMP_4bpp(Bitmap image)
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

        protected virtual byte[] GetImageBytesFromObject(Bitmap image, bool isSource4bpp, bool isDest4bpp)
        {
            int imageSize = Width * Height;
            int resultSize = (isDest4bpp) ? (imageSize / 2) : imageSize;
            byte[] resultBytes = new byte[resultSize];
            //List<byte> resultBytes = new List<byte>(resultSize);

            BitmapData bmd = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            int row = 0;
            int col = 0;
            for (int i = 0; (i < imageSize) && (row < image.Height); i++)
            {
                byte value;
                if (isSource4bpp)
                {
                    value = (byte)bmd.GetPixel4bpp(col, row);
                }
                else
                {
                    value = (byte)bmd.GetPixel(col, row);
                }

                if (isDest4bpp)
                {
                    resultBytes[i / 2] |= (((i & 0x01) == 0) ? ((byte)(value & 0x0F)) : ((byte)((value & 0x0F) << 4)));
                }
                else
                {
                    resultBytes[i] = value;
                }

                col++;
                if (col == image.Width)
                {
                    col = 0;
                    row++;
                }
            }

            image.UnlockBits(bmd);
            return resultBytes;
        }

        public virtual void SaveImage( System.IO.Stream iso, System.IO.Stream output, ImageFormat format )
        {
            if (format == null)
                format = ImageFormat.Bmp;

            Bitmap bmp = GetImageFromIso( iso );
            bmp.Save(output, format);
        }

        /*
        public void WriteImageToIso( System.IO.Stream iso, System.IO.Stream imageStream, ImageFormat format )
        {   
            //using ( Image image = Image.FromStream( imageStream ) )
            //{
            //    WriteImageToIso( iso, image, format );
            //}
            
            Bitmap image = new Bitmap(imageStream);
            WriteImageToIso(iso, image, format);
        }

        public void WriteImageToIso( System.IO.Stream iso, string filename, ImageFormat format )
        {
            ImportFilename = filename;
            using ( System.IO.Stream s = System.IO.File.OpenRead( filename ) )
            {
                WriteImageToIso( iso, s, format );
            }
        }
        */
        public void WriteImageToIso(System.IO.Stream iso, string filename, ImageFormat format)
        {
            ImportFilename = filename;
            Bitmap image = ImageFormatHelper.GetImageFileBitmap(filename);
            WriteImageToIso(iso, image, format);
        }

        public void WriteImageToIso( System.IO.Stream iso, Bitmap image, ImageFormat format )
        {
            if ( image.Width != Width || image.Height != Height )
            {
                throw new FormatException( "Image has wrong dimensions" );
            }

            WriteImageToIsoInner( iso, image, format );
            cachedImageDirty = true;
        }

        public bool ImportEntireFile(System.IO.Stream iso, string path)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return ImportEntireFile(iso, bytes);
        }

        public bool ExportEntireFile(System.IO.Stream iso, string path, bool checkFilesize)
        {
            if ((checkFilesize) && (Filesize <= 0))
                return false;
            else
                return ExportEntireFile(iso, path);
        }

        public bool ImportEntireFile(System.IO.Stream iso, byte[] bytes)
        {
            if (Sector == null)
                return false;

            PatcherLib.Iso.KnownPosition importPosition = PatcherLib.Iso.KnownPosition.ConstructKnownPosition(Sector, 0, bytes.Length);
            importPosition.PatchIso(iso, bytes);
            return true;
        }

        public bool ExportEntireFile(System.IO.Stream iso, string path)
        {
            if (Sector == null)
                return false;

            PatcherLib.Iso.KnownPosition exportPosition = PatcherLib.Iso.KnownPosition.ConstructKnownPosition(Sector, 0, Filesize);
            byte[] bytes = exportPosition.ReadIso(iso);
            System.IO.File.WriteAllBytes(path, bytes);
            return true;
        }

        public bool CanSelectPalette(AbstractImage img = null)
        {
            img = img ?? this;
            //return ((img is PalettedImage4bpp) || (img is PalettedImage4bppSectioned) || (img is StupidTM2Image4bpp));
            return (img is ISelectablePalette4bppImage);
        }

        public string Name { get; private set; }

        protected static Set<Color> GetColors( Bitmap bmp )
        {
            Set<Color> result = new Set<Color>( ( a, b ) => a.ToArgb() == b.ToArgb() ? 0 : 1 );
            for ( int x = 0; x < bmp.Width; x++ )
            {
                for ( int y = 0; y < bmp.Height; y++ )
                {
                    result.Add( bmp.GetPixel( x, y ) );
                }
            }

            return result.AsReadOnly();
        }

        protected Set<Color> GetColors( System.IO.Stream iso )
        {
            Bitmap bmp = GetImageFromIso( iso );
            return GetColors( bmp );
        }

        protected AbstractImage( string name, int width, int height )
            : this( name, width, height, 0 )
        {
        }

        protected AbstractImage( string name, int width, int height, int numPalettes )
        {
            Name = name;
            Width = width;
            Height = height;
            NumPalettes = numPalettes;
        }

        public override string ToString()
        {
            return Name;
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if ( cachedImage != null )
            {
                cachedImage.Dispose();
                cachedImage = null;
            }
        }

        #endregion
    }
}
