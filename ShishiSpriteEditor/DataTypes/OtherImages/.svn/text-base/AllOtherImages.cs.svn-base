using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Datatypes;
using System.IO;
using System.Drawing;
using PatcherLib.Utilities;
using System.Xml;
using FFTPatcher.SpriteEditor.Properties;

namespace FFTPatcher.SpriteEditor
{
    public class AllOtherImages
    {
        private AllImagesDoWorkResult LoadAllImages( Stream iso, string path, Action<int> progressReporter )
        {
            bool progress = progressReporter != null;
            int total = 0;
            int complete = 0;
            int imagesProcessed = 0;

            if (progress)
            {
                images.ForEach( i => total += i.Count );
            }

            foreach (var imgList in images)
            {
                foreach (var img in imgList)
                {
                    string name = string.Empty;
                    name = img.GetSaveFileName();

                    name = Path.Combine( path, name );
                    if (File.Exists( name ))
                    {
                        img.WriteImageToIso( iso, name );
                        imagesProcessed++;
                    }
                    if (progress)
                    {
                        progressReporter( (100 * (complete++)) / total );
                    }
                }
            }

            return new AllImagesDoWorkResult( AllImagesDoWorkResult.Result.Success, imagesProcessed );
        }

        public void LoadAllImages( Stream iso, string path )
        {
            LoadAllImages( iso, path, null );
        }

        public class AllImagesDoWorkData
        {
            public Stream ISO { get; private set; }
            public string Path { get; private set; }
            public AllImagesDoWorkData( Stream iso, string path )
            {
                ISO = iso;
                Path = path;
            }
        }

        public class AllImagesDoWorkResult
        {
            public enum Result
            {
                Success,
                Failure,
            }

            public Result DoWorkResult { get; private set; }
            public int ImagesProcessed { get; private set; }
            public AllImagesDoWorkResult( Result result, int images )
            {
                DoWorkResult = result;
                ImagesProcessed = images;
            }

        }

        internal void LoadAllImages( object sender, System.ComponentModel.DoWorkEventArgs e )
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            AllImagesDoWorkData data = e.Argument as AllImagesDoWorkData;
            if (data == null)
                return;
            e.Result = LoadAllImages( data.ISO, data.Path, worker.WorkerReportsProgress ? (Action<int>)worker.ReportProgress : null );
        }

        internal void DumpAllImages( object sender, System.ComponentModel.DoWorkEventArgs e )
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            AllImagesDoWorkData data = e.Argument as AllImagesDoWorkData;
            if (data == null)
                return;
            var result = DumpAllImages( data.ISO, data.Path, worker.WorkerReportsProgress ? (Action<int>)worker.ReportProgress : null );
            e.Result = result;
        }

        private AllImagesDoWorkResult DumpAllImages( Stream iso, string path, Action<int> progressReporter )
        {
            bool progress = progressReporter != null;
            int total = 0;
            int complete = 0;
            int imagesProcessed = 0;
            if (progress)
                images.ForEach( i => total += i.Count );

            if (!Directory.Exists( path ))
            {
                Directory.CreateDirectory( path );
            }
            foreach (var imgList in images)
            {
                foreach (var img in imgList)
                {
                    string name = string.Empty;
                    name = img.GetSaveFileName();
                    //if (img.Context == Context.US_PSX )
                    //{
                    //    var pos = img.Position as PatcherLib.Iso.PsxIso.KnownPosition;
                    //    name = string.Format( "{0}_{1}.png", pos.Sector, pos.StartLocation );
                    //}
                    //else if (img.Position is PatcherLib.Iso.PspIso.KnownPosition)
                    //{
                    //    var pos = img.Position as PatcherLib.Iso.PspIso.KnownPosition;
                    //    name = string.Format( "{0}_{1}.png", pos.SectorEnum, pos.StartLocation );
                    //}

                    if (!string.IsNullOrEmpty( name ))
                    {
                        Bitmap bmp = img.GetImageFromIso( iso );
                        bmp.Save( Path.Combine( path, name ), System.Drawing.Imaging.ImageFormat.Png );
                        imagesProcessed++;
                    }

                    if (progress)
                    {
                        progressReporter( (100 * (complete++)) / total );
                    }
                }
            }

            return new AllImagesDoWorkResult( AllImagesDoWorkResult.Result.Success, imagesProcessed );
        }

        public void DumpAllImages( Stream iso, string path )
        {
            DumpAllImages( iso, path, null );
        }

        private static IList<IList<AbstractImage>> BuildPspImages()
        {
            List<IList<AbstractImage>> result = new List<IList<AbstractImage>>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml( Resources.PSPFiles );
            foreach (XmlNode sectionNode in doc.SelectNodes( "/PspFiles/Section[@ignore='false' or not(@ignore)]" ))
            {
                result.Add( GetImagesFromNode( sectionNode ) );
            }

            return result.AsReadOnly();
        }
        
        private static IList<Type> ImageTypes
        {
            get
            {
                var assy = System.Reflection.Assembly.GetAssembly( typeof( AbstractImage ) );
                var allTypes = new List<Type>( assy.GetTypes() );
                allTypes.RemoveAll( t => !t.IsSubclassOf( typeof( AbstractImage ) ) );
                return allTypes.AsReadOnly();
            }
        }

        private static IList<AbstractImage> GetImagesFromNode( System.Xml.XmlNode node )
        {
            var types = ImageTypes;
            List<AbstractImage> result = new List<AbstractImage>();
            foreach ( XmlNode imageNode in node.SelectNodes( "*" ) )
            {
                Type type = types.Find( t => t.Name == imageNode.Name );
                System.Reflection.MethodInfo mi = type.GetMethod(
                    "ConstructFromXml");//, System.Reflection.BindingFlags.Static, null, new Type[] { typeof( System.Xml.XmlNode ) }, null );
                result.Add( (AbstractImage)mi.Invoke( null, new object[] { imageNode } ) );
            }
            return result.AsReadOnly();
        }

        private static IList<IList<AbstractImage>> BuildPsxImages()
        {
            // 0, 135
            List<IList<AbstractImage>> result = new List<IList<AbstractImage>>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml( Resources.PSXFiles );
            foreach ( XmlNode sectionNode in doc.SelectNodes( "/PsxFiles/Section[@ignore='false' or not(@ignore)]" ) )
            {
                result.Add( GetImagesFromNode( sectionNode ) );
            }

            return result.AsReadOnly();
        }

        private IList<IList<AbstractImage>> images;

        public AbstractImage this[int i, int j]
        {
            get { return this.images[i][j]; }
        }

        public IList<AbstractImage> this[int i]
        {
            get { return this.images[i]; }
        }

        public int ListCount { get { return images.Count; } }

        public static AllOtherImages GetPsx()
        {
            var aoi = new AllOtherImages( BuildPsxImages() );
            return aoi;
        }

        public static AllOtherImages GetPsp()
        {
            return new AllOtherImages( BuildPspImages() );
        }

        public static AllOtherImages FromIso( Stream iso )
        {
            if ( iso.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode2Form1] == 0 )
            {
                // assume psx
                var aoi = GetPsx();
#if STUPIDBONUSBIN
                var bonus = aoi[1];
                using (Image img = Image.FromFile( @"N:\My Dropbox\FFTC\Tests\bonus.psx.Complete.gif" ))
                using (Bitmap bmp = new Bitmap( img ))
                {
                    System.Drawing.Imaging.ColorPalette pal = img.Palette;
                    for (int i = 0; i < 36; i++)
                    {
                        List<byte> bytes = new List<byte>();
                        int startY = i * 208;
                        const int width = 256;
                        const int height = 200;
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                Color c = bmp.GetPixel( x, y + startY );
                                int cIndex = pal.Entries.IndexOf( c );
                                if (cIndex == -1 || cIndex >= 16) throw new Exception();

                                bytes.Add( (byte)cIndex );
                            }
                        }

                        List<byte> realBytes = new List<byte>();
                        for (int j = 0; j < width * height; j += 2)
                        {
                            realBytes.Add( (byte)(((bytes[j + 1] & 0xF) << 4) | (bytes[j] & 0xF)) );
                        }

                        PatcherLib.Iso.PsxIso.PatchPsxIso(
                            iso, new PatchedByteArray( PatcherLib.Iso.PsxIso.Sectors.EVENT_BONUS_BIN,
                                i * 0x6800, realBytes.ToArray() ) );
                    }
                }
#endif
                return aoi;
            }
            else if ( iso.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode1] == 0 )
            {
                // assume psp
                return GetPsp();
            }
            else
            {
                throw new ArgumentException( "iso" );
            }
        }

        private AllOtherImages( IList<IList<AbstractImage>> images )
        {
            this.images = images.AsReadOnly();
        }

    }
}
