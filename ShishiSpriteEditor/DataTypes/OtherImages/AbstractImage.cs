using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Datatypes;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

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
        }

        public abstract string DescribeXml();

        protected static PatcherLib.Iso.KnownPosition ParsePositionNode( Enum sectorType, XmlNode node )
        {
            Int32 offset = Int32.Parse( node.SelectSingleNode( "Offset" ).InnerText );
            Int32 length = Int32.Parse( node.SelectSingleNode( "Length" ).InnerText );
            return PatcherLib.Iso.KnownPosition.ConstructKnownPosition( sectorType, offset, length );
        }

        protected static PatcherLib.Iso.KnownPosition GetPositionFromImageNode( Enum sectorType, XmlNode node )
        {
            return ParsePositionNode( sectorType, node.SelectSingleNode( "Position" ) );
        }

        protected static FFTPatcher.SpriteEditor.Palette.ColorDepth GetColorDepth( XmlNode node )
        {
            return (FFTPatcher.SpriteEditor.Palette.ColorDepth)Enum.Parse( typeof( FFTPatcher.SpriteEditor.Palette.ColorDepth ), node.SelectSingleNode( "ColorDepth" ).InnerText );
        }

        protected static PatcherLib.Iso.KnownPosition GetPalettePositionFromImageNode( Enum sectorType, XmlNode node )
        {
            return ParsePositionNode( sectorType, node.SelectSingleNode( "PalettePosition" ) );
        }

        protected static ImageInfo GetImageInfo( XmlNode node )
        {
            XmlNode nameNode = node.SelectSingleNode( "Name" );
            int width = Int32.Parse( node.SelectSingleNode( "Width" ).InnerText );
            int height = Int32.Parse( node.SelectSingleNode( "Height" ).InnerText );
            XmlNode sectorNode = node.SelectSingleNode( "Sector" );
            XmlNode fftpackNode = node.SelectSingleNode( "FFTPack" );
            XmlNode pspSector = node.SelectSingleNode( "PSPSector" );
            string name = nameNode.InnerText;

            XmlNode nameResourceNode = nameNode.SelectSingleNode( "Resource" );
            if (nameResourceNode != null)
            {
                string resourceName = nameResourceNode.Attributes["file"].InnerText;
                string context = nameResourceNode.Attributes["context"].InnerText;

                Type resourcesClass = context == "PSP" ? typeof( PatcherLib.PSPResources.Lists ) : typeof( PatcherLib.PSXResources.Lists );
                object strings = resourcesClass.GetProperty( resourceName ).GetValue( null, null ) ;
                if (resourceName == "TownNames")
                {
                    var names = strings as IDictionary<Town, string>;
                    name = names[(Town)Enum.Parse( typeof( Town ), nameResourceNode.InnerText )];
                }
                else if (resourceName == "ShopNames")
                {
                    var names = strings as IDictionary<ShopsFlags, string>;
                    name = names[(ShopsFlags)Enum.Parse( typeof( ShopsFlags ), nameResourceNode.InnerText )];
                }
                else
                {
                    var names = strings as IList<string>;
                    name = names[Int32.Parse( nameResourceNode.InnerText )];
                }
            }

            if ( sectorNode != null )
            {
                return new ImageInfo { Name = name, Width = width, Height = height, 
                    Sector = (PatcherLib.Iso.PsxIso.Sectors)Enum.Parse(typeof(PatcherLib.Iso.PsxIso.Sectors), sectorNode.InnerText) };
            }
            else if ( fftpackNode != null )
            {
                return new ImageInfo { Name = name, Width = width, Height = height, 
                    Sector = (PatcherLib.Iso.FFTPack.Files)Enum.Parse(typeof(PatcherLib.Iso.FFTPack.Files), fftpackNode.InnerText) };
            }
            else if ( pspSector != null )
            {
                return new ImageInfo { Name = name, Width = width, Height = height, 
                    Sector = (PatcherLib.Iso.PspIso.Sectors)Enum.Parse(typeof(PatcherLib.Iso.PspIso.Sectors), pspSector.InnerText) };
            }
            else
            {
                throw new Exception( "No valid Sector element found" );
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        protected abstract System.Drawing.Bitmap GetImageFromIsoInner( System.IO.Stream iso );
        public virtual string FilenameFilter { get { return "PNG image (*.png)|*.png"; } }

        public int NumPalettes { get; private set; }

        System.Drawing.Bitmap cachedImage;
        bool cachedImageDirty = true;

        public System.Drawing.Bitmap GetImageFromIso( System.IO.Stream iso )
        {
            try
            {
                if (cachedImageDirty && cachedImage != null)
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
            catch
            {
                return cachedImage;
            }
         
        }

        protected IList<byte> GetPaletteBytes( Set<Color> colors, Palette.ColorDepth depth )
        {
            List<byte> result = new List<byte>();
            if ( depth == Palette.ColorDepth._16bit )
            {
                foreach ( Color c in colors )
                {
                    result.AddRange( Palette.ColorToBytes( c ) );
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

        protected abstract string SaveFileName { get; }

        public string GetSaveFileName()
        {
            return SaveFileName;
        }

        protected abstract void WriteImageToIsoInner( System.IO.Stream iso, System.Drawing.Image image );

        public virtual void SaveImage( System.IO.Stream iso, System.IO.Stream output )
        {
            System.Drawing.Bitmap bmp = GetImageFromIso( iso );
            bmp.Save( output, System.Drawing.Imaging.ImageFormat.Png );
        }

        public void WriteImageToIso( System.IO.Stream iso, System.IO.Stream image )
        {
            using ( System.Drawing.Image i = System.Drawing.Image.FromStream( image ) )
            {
                WriteImageToIso( iso, i );
            }
        }

        public void WriteImageToIso( System.IO.Stream iso, string filename )
        {
            using ( System.IO.Stream s = System.IO.File.OpenRead( filename ) )
            {
                WriteImageToIso( iso, s );
            }
        }

        public void WriteImageToIso( System.IO.Stream iso, System.Drawing.Image image )
        {
            if ( image.Width != Width || image.Height != Height )
            {
                throw new FormatException( "image has wrong dimensions" );
            }

            WriteImageToIsoInner( iso, image );
            cachedImageDirty = true;
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
