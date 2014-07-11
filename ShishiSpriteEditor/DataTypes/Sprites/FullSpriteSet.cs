using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using PatcherLib.Utilities;

namespace FFTPatcher.SpriteEditor
{
    public class FullSpriteSet
    {
        public AbstractSprite this[string key]
        {
            get
            {
                return sprites.Find( s => s.Filenames.Contains( key ) );
            }
        }

        private IList<AbstractSprite> sprites;

        public IList<AbstractSprite> Sprites
        {
            get { return sprites.AsReadOnly(); }
        }

        private FullSpriteSet( IList<AbstractSprite> sprites, System.ComponentModel.BackgroundWorker worker, int tasksComplete, int tasks )
        {
            bool haveWorker = worker != null;
            if ( haveWorker )
                worker.ReportProgress( ( tasksComplete++ * 100 ) / tasks, "Sorting" );
            sprites.Sort( ( a, b ) => a.Name.CompareTo( b.Name ) );
            this.sprites = sprites;
        }

        private static FullSpriteSet DoInitPSX( Stream iso, BackgroundWorker worker )
        {
            const int numberOfPsxSprites = 134;
            int tasks = numberOfPsxSprites * 2 + 1;
            int tasksComplete = 0;

            var sprites = new List<AbstractSprite>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml( Properties.Resources.PSXFiles );
            foreach ( XmlNode node in doc.SelectNodes( string.Format( "/PsxFiles/{0}/Sprite", typeof( MonsterSprite ).FullName ) ) )
            {
                string name = node.SelectSingleNode( "@name" ).InnerText;
                List<string> filenames = new List<string>();
                List<byte[]> bytes = new List<byte[]>();
                foreach ( XmlNode file in node.SelectNodes( "Files/File" ) )
                {
                    string filename = file.SelectSingleNode( "@name" ).InnerText;
                    worker.ReportProgress( tasksComplete++ * 100 / tasks, "Reading " + filename );
                    filenames.Add( filename );
                    bytes.Add( IsoPatch.ReadFile( IsoPatch.IsoType.Mode2Form1, iso,
                        (int)Enum.Parse( typeof( PsxIso.Sectors ), file.SelectSingleNode( "@enum" ).InnerText ),
                        0,
                        Int32.Parse( file.SelectSingleNode( "@original_size" ).InnerText ) ) );
                }
                if ( bytes.Count > 1 )
                {
                    sprites.Add( new MonsterSprite( bytes[0], bytes.Sub( 1 ).ToArray() ) );
                }
                else
                {
                    sprites.Add( new MonsterSprite( bytes[0] ) );
                }
            }
            foreach ( Type t in new Type[] { 
                typeof( TYPE1Sprite ), 
                typeof( TYPE2Sprite ), 
                typeof( ShortSprite ), 
                typeof( KANZEN ), 
                typeof( CYOKO ), 
                typeof( ARUTE ) } )
            {
                ConstructorInfo constructor = t.GetConstructor( new Type[] { typeof( string ), typeof( IList<byte> ) } );
                foreach ( XmlNode node in doc.SelectNodes( string.Format( "/PsxFiles/{0}/Sprite", t.FullName ) ) )
                {
                    worker.ReportProgress( tasksComplete++ * 100 / tasks, "Reading " + node.SelectSingleNode( "Files/File/@name" ).InnerText );
                    sprites.Add( (AbstractSprite)constructor.Invoke( new object[] { node.SelectSingleNode("@name").InnerText, 
                        IsoPatch.ReadFile(
                            IsoPatch.IsoType.Mode2Form1, 
                            iso, 
                            (int)Enum.Parse(typeof(PsxIso.Sectors), node.SelectSingleNode("Files/File/@enum").InnerText), 
                            0, 
                            Int32.Parse(node.SelectSingleNode("Files/File/@original_size").InnerText))} ) );

                }
            }
            return new FullSpriteSet( sprites, worker, tasksComplete, tasks );
        }

        private static FullSpriteSet DoInitPSP( Stream iso, BackgroundWorker worker )
        {
            PatcherLib.Iso.PspIso.PspIsoInfo info = PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo(iso);
            const int numberOfPspSprites = 143;
            int tasks = numberOfPspSprites * 2 + 1;
            int tasksComplete = 0;

            var sprites = new List<AbstractSprite>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml( Properties.Resources.PSPFiles );
            foreach ( XmlNode node in doc.SelectNodes( string.Format( "/PspFiles/{0}/Sprite", typeof( MonsterSprite ).FullName ) ) )
            {
                string name = node.SelectSingleNode( "@name" ).InnerText;
                List<string> filenames = new List<string>();
                List<byte[]> bytes = new List<byte[]>();
                foreach ( XmlNode file in node.SelectNodes( "Files/File" ) )
                {
                    string filename = file.SelectSingleNode( "@name" ).InnerText;
                    worker.ReportProgress( tasksComplete++ * 100 / tasks, "Reading " + filename );
                    filenames.Add( filename );
                    bytes.Add( FFTPack.GetFileFromIso( iso, info, (FFTPack.Files)Enum.Parse( typeof( FFTPack.Files ), file.SelectSingleNode( "@enum" ).InnerText ) ) );
                }
                if ( bytes.Count > 1 )
                {
                    sprites.Add( new MonsterSprite( bytes[0], bytes.Sub( 1 ).ToArray() ) );
                }
                else
                {
                    sprites.Add( new MonsterSprite( bytes[0] ) );
                }
            }

            foreach ( Type t in new Type[] { 
                typeof( TYPE1Sprite ), 
                typeof( TYPE2Sprite ), 
                typeof( ShortSprite ), 
                typeof( KANZEN ), 
                typeof( CYOKO ), 
                typeof( ARUTE ) } )
            {
                ConstructorInfo constructor = t.GetConstructor( new Type[] { typeof( string ), typeof( IList<byte> ) } );
                foreach ( XmlNode node in doc.SelectNodes( string.Format( "/PspFiles/{0}/Sprite", t.FullName ) ) )
                {
                    worker.ReportProgress( tasksComplete++ * 100 / tasks, "Reading " + node.SelectSingleNode( "Files/File/@name" ).InnerText );
                    sprites.Add( (AbstractSprite)constructor.Invoke( new object[] { node.SelectSingleNode("@name").InnerText, 
                        FFTPack.GetFileFromIso( iso, info, (FFTPack.Files)Enum.Parse( typeof( FFTPack.Files ), node.SelectSingleNode( "Files/File/@enum" ).InnerText ))} ) );
                }
            }

            worker.ReportProgress( tasksComplete++ * 100 / tasks, "Sorting..." );
            sprites.Sort( ( a, b ) => a.Name.CompareTo( b.Name ) );

            return new FullSpriteSet( sprites, worker, tasksComplete, tasks );
        }

        public void PatchPsxISO( string filename, BackgroundWorker worker, IList<PatchedByteArray> patches )
        {
            using ( Stream stream = File.Open( filename, FileMode.Open, FileAccess.ReadWrite ) )
            {
                PatchPsxISO( stream, worker, patches );
            }
        }

        public void PatchPsxISO( Stream stream, BackgroundWorker worker, IList<PatchedByteArray> patches )
        {
            int totalTasks = patches.Count;
            int tasksComplete = 0;

            foreach ( PatchedByteArray patch in patches )
            {
                if ( patch != null )
                {
                    worker.ReportProgress( tasksComplete++ * 100 / totalTasks, "Patching " + patch.SectorEnum.ToString() );
                    IsoPatch.PatchFileAtSector(IsoPatch.IsoType.Mode2Form1, stream, true, patch.Sector, patch.Offset, patch.GetBytes(), true);
                }
                else
                {
                    tasksComplete++;
                }
            }
        }

        public void PatchPspISO( Stream stream, BackgroundWorker worker, IList<PatchedByteArray> patches )
        {
            int totalTasks = patches.Count;
            int tasksComplete = 0;
            PatcherLib.Iso.PspIso.PspIsoInfo info = PspIso.PspIsoInfo.GetPspIsoInfo(stream);
            foreach ( var patch in patches )
            {
                worker.ReportProgress( tasksComplete++ * 100 / totalTasks, "Patching " + patch.SectorEnum.ToString() );
                FFTPack.PatchFile( stream, info, (int)( (FFTPack.Files)patch.SectorEnum ), (int)patch.Offset, patch.GetBytes() );
            }

        }

        public void PatchPspISO( string filename, BackgroundWorker worker, IList<PatchedByteArray> patches )
        {
            using ( Stream stream = File.Open( filename, FileMode.Open, FileAccess.ReadWrite ) )
            {
                PatchPspISO( stream, worker, patches );
            }
        }

        public void SaveShishiFile( string filename )
        {
            using( ZipOutputStream stream = new ZipOutputStream( File.Open( filename, FileMode.Create, FileAccess.ReadWrite ) ) )
            {
                Dictionary<string, Dictionary<string, List<string>>> files = new Dictionary<string, Dictionary<string, List<string>>>();

                foreach( var sprite in Sprites )
                {
                    byte[] pixels = sprite.Pixels.ToArray();
                    IList<byte> palettes = new List<byte>();
                    sprite.Palettes.ForEach( p => palettes.AddRange( p.ToByteArray() ) );
                    palettes = palettes.ToArray();

                    string type = sprite.GetType().FullName;
                    if( !files.ContainsKey( type ) )
                    {
                        files[type] = new Dictionary<string, List<string>>();
                    }

                    List<string> fileList = new List<string>( );
                    files[type][sprite.Name] = fileList;

                    WriteFileToZip(
                        stream,
                        string.Format( System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}/Pixels", type, sprite.Name ),
                        pixels );
                    WriteFileToZip(
                        stream,
                        string.Format( System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}/Palettes", type, sprite.Name ),
                        palettes.ToArray() );
                    WriteFileToZip(
                        stream,
                        string.Format( System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}/Size", type, sprite.Name ),
                        sprite.OriginalSize.ToString( System.Globalization.CultureInfo.InvariantCulture ).ToByteArray() );
                    fileList.AddRange( sprite.Filenames );
                }

                BinaryFormatter f = new BinaryFormatter();
                stream.PutNextEntry( new ZipEntry( "manifest" ) );
                f.Serialize( stream, files );

                const string fileVersion = "1.0";
                WriteFileToZip( stream, "version", Encoding.UTF8.GetBytes( fileVersion ) );
            }
        }

        public static FullSpriteSet FromPsxISO( string filename, BackgroundWorker worker )
        {
            using ( FileStream stream = File.OpenRead( filename ) )
            {
                return FromPsxISO( stream, worker );
            }
        }

        public static FullSpriteSet FromPsxISO( Stream stream, BackgroundWorker worker )
        {
            return DoInitPSX( stream, worker );
        }

        public static FullSpriteSet FromPspISO( string filename, BackgroundWorker worker )
        {
            using ( FileStream stream = File.OpenRead( filename ) )
            {
                return FromPspISO( stream, worker );
            }
        }

        public static FullSpriteSet FromPspISO( Stream stream, BackgroundWorker worker )
        {
            return DoInitPSP( stream, worker );
        }

        public static FullSpriteSet FromShishiFile( string filename, System.ComponentModel.BackgroundWorker worker )
        {
            Dictionary<string, Dictionary<string, List<string>>> manifest;

            List<AbstractSprite> sprites = new List<AbstractSprite>();

            int tasks = 0;
            int tasksComplete = 0;
            using ( ZipFile zf = new ZipFile( filename ) )
            {
                BinaryFormatter f = new BinaryFormatter();
                manifest = f.Deserialize( zf.GetInputStream( zf.GetEntry( "manifest" ) ) ) as Dictionary<string, Dictionary<string, List<string>>>;

                foreach ( KeyValuePair<string, Dictionary<string, List<string>>> kvp in manifest )
                {
                    tasks += kvp.Value.Keys.Count * 3;
                }

                tasks += 1;
                foreach( string type in manifest.Keys )
                {
                    Type spriteType = Type.GetType( type );
                    ConstructorInfo constructor = spriteType.GetConstructor( BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof( SerializedSprite ) }, null );

                    foreach( string name in manifest[type].Keys )
                    {
                        List<string> filenames = manifest[type][name];
                        int size = filenames.Count;
                        byte[][] bytes = new byte[size][];

                        worker.ReportProgress( ( tasksComplete++ * 100 ) / tasks, string.Format( "Extracting {0}", name ) );

                        ZipEntry entry = zf.GetEntry( string.Format( System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}/Pixels", type, name ) );
                        byte[] pixels = new byte[entry.Size];
                        StreamUtils.ReadFully( zf.GetInputStream( entry ), pixels );

                        entry = zf.GetEntry( string.Format( System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}/Palettes", type, name ) );
                        byte[] palettes = new byte[entry.Size];
                        StreamUtils.ReadFully( zf.GetInputStream( entry ), palettes );

                        entry = zf.GetEntry( string.Format( System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}/Size", type, name ) );
                        byte[] sizeBytes = new byte[entry.Size];
                        StreamUtils.ReadFully( zf.GetInputStream( entry ), sizeBytes );
                        int origSize = Int32.Parse( new string( Encoding.UTF8.GetChars( sizeBytes ) ), System.Globalization.CultureInfo.InvariantCulture );


                        worker.ReportProgress( ( tasksComplete++ * 100 ) / tasks, string.Format( "Building {0}", name ) );
                        sprites.Add( constructor.Invoke( new object[] { new SerializedSprite( name, origSize, filenames, pixels, palettes ) } ) as AbstractSprite );
                    }
                }
                
            }

            return new FullSpriteSet( sprites, worker, tasksComplete, tasks );
        }

        private static void WriteFileToZip( ZipOutputStream stream, string filename, byte[] bytes )
        {
            stream.PutNextEntry( new ZipEntry( filename ) );
            stream.Write( bytes, 0, bytes.Length );
        }

    }
}
