/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using PatcherLib.Datatypes;

namespace FFTPatcher.TextEditor
{
    static class Program
    {

		#region Methods (2) 


        private static bool HandleArgs( string[] args )
        {
            if( args == null )
            {
                return false;
            }

            if( args.Length >= 4 && args[0] == @"--load" )
            {
                string filename = args[1];
                string type = args[2];
                string output = args[3];
                int? start = null;
                if( args.Length > 4 )
                {
                    int locStart;
                    string s = args[4].TrimStart( '0', 'x' );
                    if( s == string.Empty )
                        s = "0";
                    if( Int32.TryParse( s, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out locStart ) )
                    {
                        start = locStart;
                    }
                }
                int? length = null;
                if( args.Length > 5 )
                {
                    string s = args[5].TrimStart( '0', 'x' );
                    if( s == string.Empty )
                        s = "0";
                    int loclength;
                    if( Int32.TryParse( s, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out loclength ) )
                    {
                        length = loclength;
                    }
                }

                Type t =
                    Type.GetType( type, false, true ) ??
                    Type.GetType( "FFTPatcher.TextEditor.Files." + type, false, true ) ??
                    Type.GetType( "FFTPatcher.TextEditor.Files.PSX." + type, false, true ) ??
                    Type.GetType( "FFTPatcher.TextEditor.Files.PSP." + type, false, true );
                if( t != null )
                {
                    IList<byte> bytes;
                    using( FileStream stream = new FileStream( filename, FileMode.Open ) )
                    {
                        bytes = new byte[stream.Length];
                        stream.Read( (byte[])bytes, 0, bytes.Count );
                    }

                    ConstructorInfo ci = t.GetConstructor( new Type[] { typeof( IList<byte> ) } );
                    if( start.HasValue && length.HasValue )
                    {
                        bytes = bytes.Sub( start.Value, start.Value + length.Value - 1 );
                    }
                    else if( start.HasValue )
                    {
                        bytes = bytes.Sub( start.Value );
                    }

                    object o = ci.Invoke( new object[] { bytes } );
                    //if( o is IStringSectioned )
                    //{
                    //    using( XmlTextWriter writer = new XmlTextWriter( output, System.Text.Encoding.UTF8 ) )
                    //    {
                    //        writer.WriteStartElement( "dongs" );
                    //        (o as IStringSectioned).WriteXml( writer, true );
                    //        writer.WriteEndElement();
                    //        return true;
                    //    }
                    //}
                    //else if( o is IPartitionedFile )
                    //{
                    //    using( XmlTextWriter writer = new XmlTextWriter( output, System.Text.Encoding.UTF8 ) )
                    //    {
                    //        writer.WriteStartElement( "dongs" );
                    //        (o as IPartitionedFile).WriteXml( writer, true );
                    //        writer.WriteEndElement();
                    //        return true;
                    //    }
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                    return true;
                }
            }

            return false;
        }

        //delegate IFile func( FFTText text, string guid );
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( string[] args )
        {
            if ( !HandleArgs( args ) )
            {
                //using ( Stream psx = File.Open( @"N:\dev\fft\ita\virginimages\FINALFANTASYTACTICS.BIN", FileMode.Open, FileAccess.Read ) )
                //using ( Stream psp = File.Open( @"N:\dev\fft\ita\virginimages\fflw-usa.iso", FileMode.Open, FileAccess.Read ) )
                //{
                //    var psxtext = FFTTextFactory.GetPsxText( psx );
                //    var psptext = FFTTextFactory.GetPspText( psp );
                //    FFTPatcher.TextEditor.Files.QuickEdit pspQuick = psptext.Files[psptext.Files.Count - 1] as FFTPatcher.TextEditor.Files.QuickEdit;
                //    FFTPatcher.TextEditor.Files.QuickEdit psxQuick = psxtext.Files[psxtext.Files.Count - 1] as FFTPatcher.TextEditor.Files.QuickEdit;
                //    int sectionCount = pspQuick.NumberOfSections;
                //    for ( int i = 0; i < sectionCount; i++ )
                //    {
                //        int secLength = pspQuick.SectionLengths[i];
                //        SectionType type = (SectionType)Enum.Parse( typeof( SectionType ), pspQuick.SectionNames[i] );
                //        int psxIndex = psxQuick.SectionNames.IndexOf( pspQuick.SectionNames[i] );
                //        int psxSecLength = psxQuick.SectionLengths[psxIndex];
                //        for ( int j = 0; j < psxSecLength; j++ )
                //        {
                //            psxQuick[psxIndex, j] = pspQuick[i, j];
                //        }

                //    }

                //    var func =
                //        new func(
                //            delegate( FFTText text, string guid )
                //            {
                //                Guid g = new Guid( guid );
                //                return text.Files.Find( f => ( f is AbstractFile ) && ( f as AbstractFile ).Layout.Guid == g );
                //            } );

                //    var file = func( psptext, @"{396EF3EC-D861-4DC9-B4DA-BBCEC443FFC2}" );
                //    var destFile = func( psxtext, @"{A45B2DE6-520A-4484-A208-6F7895798869}" );
                //    for ( int i = 0; i < 172; i++ )
                //    {
                //        destFile[1, i] = file[1, i];
                //        destFile[2, i] = file[2, i];
                //        destFile[3, i] = file[3, i];
                //    }

                //    destFile = func( psxtext, @"{D1CA95EC-FAE1-41DD-AFB1-EF2EF2563025}" );
                //    file = func( psptext, @"{8B291A28-EAEA-42E7-8BB3-150871BBB89F}" );
                //    for ( int i = 0; i < 768; i++ )
                //    {
                //        destFile[20, i] = file[20, i];
                //    }

                //    destFile = func( psxtext, @"{162DC5BF-983B-4E04-972E-C51B51EE1DCA}" );
                //    file = func( psptext, @"{8AB063AC-7E8E-4B8F-9F67-11D16560B874}" );
                //    for ( int i = 0; i < 172; i++ )
                //    {
                //        destFile[0, i] = file[0, i];
                //    }

                //    file = func( psptext, @"{CE4B8E36-0DA8-4A2E-A03E-ABEF21AD3A03}" );
                //    destFile = func( psxtext, @"{4C29FBA2-B49B-4BA7-8880-E785A61743FB}" );
                //    for ( int i = 0; i < 1024; i++ )
                //    {
                //        destFile[1, i] = file[1, i];
                //    }
                //    destFile = func( psxtext, @"{FBFFD898-2E12-4E3C-B73D-CA6FA6997F42}" );
                //    for ( int i = 0; i < 1024; i++ )
                //    {
                //        destFile[8, i] = file[1, i];
                //    }
                //    destFile = func( psxtext, @"{5AD4F95A-0C00-445D-8A29-EA4E09B9553F}" );
                //    for ( int i = 0; i < 1024; i++ )
                //    {
                //        destFile[8, i] = file[1, i];
                //        destFile[9, i] = file[1, i];
                //    }


                //    FFTTextFactory.WriteXml( psxtext, @"N:\dev\fft\ita\virginimages\psxxx.ffttext" );
                //}

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault( false );
                Application.Run( new MainForm() );
            }
        }


		#endregion Methods 

    }
}
