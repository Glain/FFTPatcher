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
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using System.IO;
using System.Windows.Forms;

namespace FFTPatcher.TextEditor
{
    /// <summary>
    /// Utilities for manipulating FFT text.
    /// </summary>
    public static class TextUtilities
    {

        /// <summary>
        /// Types of character maps.
        /// </summary>
        public enum CharMapType
        {
            /// <summary>
            /// Playstation
            /// </summary>
            PSX,
            /// <summary>
            /// PSP
            /// </summary>
            PSP
        }


        #region Static Properties (3)


        /// <summary>
        /// Gets the PSP character map.
        /// </summary>
        public static PSPCharMap PSPMap { get; private set; }

        /// <summary>
        /// Gets the PSX character map.
        /// </summary>
        public static PSXCharMap PSXMap { get; private set; }

        public static FFTFont PSXFont
        {
            get
            {
                return new FFTFont( PatcherLib.PSXResources.Binaries.Font, PatcherLib.PSXResources.Binaries.FontWidths );
            }
        }

        public static FFTFont PSPFont
        {
            get
            {
                return new FFTFont( PatcherLib.PSPResources.Binaries.Font, PatcherLib.PSPResources.Binaries.FontWidths );
            }
        }

        #endregion Static Properties

        #region Constructors (1)

        //[DllImport( "kernel32" )]
        //private static extern IntPtr LoadLibrary( string lpFileName );

        //[DllImport( "kernel32.dll", SetLastError = true )]
        //private static extern bool FreeLibrary( IntPtr hModule );

        static TextUtilities()
        {
            //PSXMap = new PSXCharMap();
            //PSPMap = new PSPCharMap();
            var psx = new Dictionary<int, string>();
            var psp = new Dictionary<int, string>();
            BuildVersion1Charmap( psx, psp );
            BuildVersion2Charmap( psx, psp );
            BuildVersion3Charmap( psx, psp );

            PSXMap = new PSXCharMap( psx );
            PSPMap = new PSPCharMap( psp );

            /*
            // Extract the DLL to the temp folder
            string dir = Path.Combine( Path.GetTempPath(), Path.GetRandomFileName() );
            Directory.CreateDirectory( dir );

            string path = Path.Combine( dir, "FFTTextCompression.dll" );
            File.WriteAllBytes( path, Resources.FFTTextCompression );

            IntPtr h = LoadLibrary( path );
            System.Diagnostics.Debug.Assert( h != IntPtr.Zero, "Unable to load library " + path );

            Application.ApplicationExit +=
                delegate( object sender, EventArgs args )
                {
                    if (FreeLibrary( h ) && File.Exists( path ) && Directory.Exists( dir ))
                    {
                        try
                        {
                            File.Delete( path );
                            Directory.Delete( dir );
                        }
                        catch { }
                    }
                };
            */
        }

        #endregion Constructors

        #region Delegates (1)

        /// <summary>
        /// A delegate to call when the progress of an operation has changed.
        /// </summary>
        public delegate void ProgressCallback( int progress );

        #endregion Delegates

        #region Methods (12)


        private static void BuildVersion1Charmap(IDictionary<int, string> psx, IDictionary<int, string> psp)
        {
            for( int i = (int)'a'; i <= (int)'z'; i++ )
            {
                psx.Add( i - (ushort)'a' + 0x24, ((char)i).ToString() );
                psx.Add( i - (ushort)'a' + 0x24 + 0xD000, ((char)i).ToString() );
            }
            psx.Add( 0x40, "?" );
            psx.Add( 0xD040, "?" );
            psx.Add( 0xD9C9, "?" );
            psx.Add( 0xB2, "\u266A" );
            psx.Add( 0xD0B2, "\u266A" );
            psx.Add( 0xD117, "\u2014" );
            psx.Add( 0xD118, "\u300C" );
            psx.Add( 0xD11B, "\u22EF" );
            psx.Add( 0xD11F, "\xD7" );
            psx.Add( 0xD120, "\xF7" );
            psx.Add( 0xD121, "\u2229" );
            psx.Add( 0xD122, "\u222A" );
            psx.Add( 0xD123, "=" );
            psx.Add( 0xDA70, "=" );
            psx.Add( 0xD124, "\u2260" );
            psx.Add( 0xD9B5, "\u221E" );
            psx.Add( 0xD9B7, "&" );
            psx.Add( 0xD9B8, "%" );
            psx.Add( 0xD9B9, "\u25CB" );
            psx.Add( 0xD9BA, "\u2190" );
            psx.Add( 0xD9BB, "\u2192" );
            psx.Add( 0xD9C2, "\u300E" );
            psx.Add( 0xD9C3, "\u300F" );
            psx.Add( 0xD9C4, "\u300D" );
            psx.Add( 0xD9C5, "\uFF5E" );
            psx.Add( 0xD9C7, "\u25B3" );
            psx.Add( 0xD9C8, "\u25A1" );
            psx.Add( 0xD9CA, "\u2665" );
            psx.Add( 0xD9CB, "\u2160" );
            psx.Add( 0xD9CC, "\u2161" );
            psx.Add( 0xD9CD, "\u2162" );
            psx.Add( 0xD9CE, "\u2163" );
            psx.Add( 0xD9CF, "\u2164" );
            psx.Add( 0xDA00, "\u2648" );
            psx.Add( 0xDA01, "\u2649" );
            psx.Add( 0xDA02, "\u264A" );
            psx.Add( 0xDA03, "\u264B" );
            psx.Add( 0xDA04, "\u264C" );
            psx.Add( 0xDA05, "\u264D" );
            psx.Add( 0xDA06, "\u264E" );
            psx.Add( 0xDA07, "\u264F" );
            psx.Add( 0xDA08, "\u2650" );
            psx.Add( 0xDA09, "\u2651" );
            psx.Add( 0xDA0A, "\u2652" );
            psx.Add( 0xDA0B, "\u2653" );
            psx.Add( 0xDA0C, "{Serpentarius}" );
            psx.Add( 0xDA71, "$" );
            psx.Add( 0xDA72, "\xA5" );
            psx.Add( 0xDA74, "," );
            psx.Add( 0xDA75, ";" );

            psx.Add( 0xD11D, "-" );

            psx.Add( 0x42, "+" );
            psx.Add( 0xD042, "+" );
            psx.Add( 0xD11E, "+" );

            psx.Add( 0x46, ":" );
            psx.Add( 0xD046, ":" );
            psx.Add( 0xD9BD, ":" );

            psx.Add( 0x8D, "(" );
            psx.Add( 0xD08D, "(" );
            psx.Add( 0xD9BE, "(" );

            psx.Add( 0x8E, ")" );
            psx.Add( 0xD08E, ")" );
            psx.Add( 0xD9BF, ")" );

            psx.Add( 0x91, "\"" );
            psx.Add( 0xD091, "\"" );
            psx.Add( 0xD9C0, "\"" );
            psx.Add( 0xDA77, "\"" );

            psx.Add( 0x93, "'" );
            psx.Add( 0xD093, "'" );
            psx.Add( 0xD9C1, "'" );
            psx.Add( 0xDA76, "'" );

            psx.Add( 0x8B, "\xB7" );
            psx.Add( 0xD08B, "\xB7" );
            psx.Add( 0xD9BC, "\xB7" );

            psx.Add( 0x44, "/" );
            psx.Add( 0xD044, "/" );
            psx.Add( 0xD9C6, "/" );

            psx.Add( 0xD125, ">" );

            psx.Add( 0xD126, "<" );

            psx.Add( 0xD127, "\u2267" );

            psx.Add( 0xD128, "\u2266" );

            psx.Add( 0xFA, " " );
            psx.Add( 0xD12A, " " );
            psx.Add( 0xDA73, " " );

            psx.Add( 0x5F, "." );
            psx.Add( 0xD05F, "." );
            psx.Add( 0xD119, "." );
            psx.Add( 0xD11C, "." );
            psx.Add( 0xD9B6, "." );

            psx.Add( 0x3E, "!" );
            psx.Add( 0xD03E, "!" );
            psx.Add( 0xD11A, "!" );

            psx.Add( 0xB5, "*" );
            psx.Add( 0xD0B5, "*" );
            psx.Add( 0xD111, "*" );
            psx.Add( 0xD129, "*" );
            psx.Add( 0xD12B, "*" );
            psx.Add( 0xD12C, "*" );
            psx.Add( 0xD12D, "*" );
            psx.Add( 0xD12E, "*" );
            psx.Add( 0xD12F, "*" );
            psx.Add( 0xD130, "*" );
            psx.Add( 0xD131, "*" );
            psx.Add( 0xD132, "*" );
            psx.Add( 0xE0, "{Ramza}" );
            psx.Add( 0xF8, "{Newline}" );
            psx.Add( 0xFB, "{Begin List}" );
            psx.Add( 0xFC, "{End List}" );
            psx.Add( 0xFF, "{Close}" );

            for( int i = 0; i < 10; i++ )
            {
                psx.Add( i, i.ToString() );
                psx.Add( i + 0xD000, i.ToString() );
            }
            for( int i = (int)'A'; i <= (int)'Z'; i++ )
            {
                psx.Add( i - (ushort)'A' + 0x0A, ((char)i).ToString() );
                psx.Add( i - (ushort)'A' + 0x0A + 0xD000, ((char)i).ToString() );
            }

            for( int i = 0; i < 256; i++ )
            {
                // HACK
                psx.Add( 0xE200 + i, string.Format( "{{Delay {0:X2}", i ) + @"}" );
                psx.Add( 0xE300 + i, string.Format( "{{Color {0:X2}", i ) + @"}" );
            }

            psx.Add( 0x3F, "\u3042" );
            psx.Add( 0x41, "\u3044" );
            psx.Add( 0x43, "\u3046" );
            psx.Add( 0x45, "\u3048" );
            psx.Add( 0xD03F, "\u3042" );
            psx.Add( 0xD041, "\u3044" );
            psx.Add( 0xD043, "\u3046" );
            psx.Add( 0xD045, "\u3048" );

            for( int i = 0x47; i <= 0x5E; i++ )
            {
                psx.Add( i, ((char)(i - 0x47 + 0x304A)).ToString() );
                psx.Add( i + 0xD000, ((char)(i - 0x47 + 0x304A)).ToString() );
            }
            for( int i = 0x60; i <= 0x8A; i++ )
            {
                psx.Add( i, ((char)(i - 0x60 + 0x3063)).ToString() );
                psx.Add( i + 0xD000, ((char)(i - 0x60 + 0x3063)).ToString() );
            }

            psx.Add( 0x8C, "\u308F" );
            psx.Add( 0xD08C, "\u308F" );
            psx.Add( 0x8F, "\u3092" );
            psx.Add( 0xD08F, "\u3092" );
            psx.Add( 0x90, "\u3093" );
            psx.Add( 0xD090, "\u3093" );
            psx.Add( 0x92, "\u30A2" );
            psx.Add( 0xD092, "\u30A2" );

            for( int i = 0x94; i <= 0xB1; i++ )
            {
                psx.Add( i, ((char)(i - 0x94 + 0x30A4)).ToString() );
                psx.Add( i + 0xD000, ((char)(i - 0x94 + 0x30A4)).ToString() );
            }

            psx.Add( 0xB3, "\u30C3" );
            psx.Add( 0xD0B3, "\u30C3" );
            psx.Add( 0xB4, "\u30C4" );
            psx.Add( 0xD0B4, "\u30C4" );

            for( int i = 0xB6; i <= 0xCF; i++ )
            {
                psx.Add( i, ((char)(i - 0xB6 + 0x30C6)).ToString() );
                psx.Add( i + 0xD000, ((char)(i - 0xB6 + 0x30C6)).ToString() );
            }

            for( int i = 0xD0; i <= 0xDB; i++ )
            {
                psx.Add( i - 0xD0 + 0xD100, ((char)(i - 0xD0 + 0x30E0)).ToString() );
            }

            psx.Add( 0xD10C, "\u30EC" );
            psx.Add( 0xD10D, "\u30ED" );
            psx.Add( 0xD10E, "\u30EE" );
            psx.Add( 0xD10F, "\u30EF" );

            for( int i = 0xE2; i <= 0xE6; i++ )
            {
                psx.Add( i - 0xE2 + 0xD112, ((char)(i - 0xE2 + 0x30F2)).ToString() );
            }

            foreach( KeyValuePair<int, string> kvp in psx )
            {
                psp.Add( kvp.Key, kvp.Value );
            }

            for (int i = 0; i < 256; i++)
            {
                psp.Add( 0xEE00 + i, string.Format( "{{Tab {0:X2}", i ) + @"}" );
            }


            psp[0x95] = " ";
            psp[0xfa] = "{SP2}";
            psp.Add( 0xDA60, "\xE1" );
            psp.Add( 0xDA61, "\xE0" );
            psp.Add( 0xDA62, "\xE9" );
            psp.Add( 0xDA63, "\xE8" );
            psp.Add( 0xDA64, "\xED" );
            psp.Add( 0xDA65, "\xFA" );
            psp.Add( 0xDA66, "\xF9" );
        }

        private static void BuildVersion2Charmap(IDictionary<int, string> psx, IDictionary<int, string> psp)
        {
            foreach (IDictionary<int, string> map in new IDictionary<int, string>[] { psx, psp })
            {
                map.Add( 0xD133, "\u5263" );
                map.Add( 0xD134, "\u4E00" );
                map.Add( 0xD135, "\u4E59" );
                map.Add( 0xD136, "\u4E03" );
                map.Add( 0xD137, "\u4E01" );
                map.Add( 0xD138, "\u4E5D" );
                map.Add( 0xD139, "\u4E86" );
                map.Add( 0xD13A, "\u61E8" );
                map.Add( 0xD13B, "\u4EBA" );
                map.Add( 0xD13C, "\u5165" );
                map.Add( 0xD13D, "\u516B" );
                map.Add( 0xD13E, "\u5200" );
                map.Add( 0xD13F, "\u529B" );
                map.Add( 0xD140, "\u5341" );
                map.Add( 0xD141, "\u4E0B" );
                map.Add( 0xD142, "\u4E09" );
                map.Add( 0xD143, "\u4E0A" );
                map.Add( 0xD144, "\u4E08" );
                map.Add( 0xD145, "\u4E07" );
                map.Add( 0xD146, "\u4E0E" );
                map.Add( 0xD147, "\u4E45" );
                map.Add( 0xD148, "\u4E38" );
                map.Add( 0xD149, "\u4E5E" );
                map.Add( 0xD14A, "\u4E5F" );
                map.Add( 0xD14B, "\u4EA1" );
                map.Add( 0xD14C, "\u51E1" );
                map.Add( 0xD14D, "\u5203" );
                map.Add( 0xD14E, "\u5343" );
                map.Add( 0xD14F, "\u98EF" );
                map.Add( 0xD150, "\u571F" );
                map.Add( 0xD151, "\u58EB" );
                map.Add( 0xD152, "\u5915" );
                map.Add( 0xD153, "\u5927" );
                map.Add( 0xD154, "\u5973" );
                map.Add( 0xD155, "\u5B50" );
                map.Add( 0xD156, "\u5BF8" );
                map.Add( 0xD157, "\u5C0F" );
                map.Add( 0xD158, "\u5C71" );
                map.Add( 0xD159, "\u5DDD" );
                map.Add( 0xD15A, "\u5DE5" );
                map.Add( 0xD15B, "\u5DF1" );
                map.Add( 0xD15C, "\u5E72" );
                map.Add( 0xD15D, "\u5F13" );
                map.Add( 0xD15E, "\u3005" );
                map.Add( 0xD15F, "\u6CB9" );
                map.Add( 0xD160, "\u796D" );
                map.Add( 0xD161, "\u5947" );
                map.Add( 0xD162, "\u8DE1" );
                map.Add( 0xD164, "\u4E0D" );
                map.Add( 0xD165, "\u4E2D" );
                map.Add( 0xD166, "\u4E88" );
                map.Add( 0xD167, "\u4E94" );
                map.Add( 0xD168, "\u4E92" );
                map.Add( 0xD169, "\u4E95" );
                map.Add( 0xD16A, "\u4ECB" );
                map.Add( 0xD16B, "\u4EC7" );
                map.Add( 0xD16C, "\u4ECA" );
                map.Add( 0xD16D, "\u4EC1" );
                map.Add( 0xD16E, "\u5185" );
                map.Add( 0xD16F, "\u5143" );
                map.Add( 0xD170, "\u516C" );
                map.Add( 0xD171, "\u516D" );
                map.Add( 0xD172, "\u5186" );
                map.Add( 0xD173, "\u5197" );
                map.Add( 0xD174, "\u51F6" );
                map.Add( 0xD175, "\u5207" );
                map.Add( 0xD176, "\u5206" );
                map.Add( 0xD177, "\u5302" );
                map.Add( 0xD178, "\u5316" );
                map.Add( 0xD179, "\u5DE8" );
                map.Add( 0xD17A, "\u5339" );
                map.Add( 0xD17B, "\u725B" );
                map.Add( 0xD17C, "\u5384" );
                map.Add( 0xD17D, "\u53CC" );
                map.Add( 0xD17E, "\u53CD" );
                map.Add( 0xD17F, "\u53CB" );
                map.Add( 0xD180, "\u592A" );
                map.Add( 0xD181, "\u5929" );
                map.Add( 0xD182, "\u5C11" );
                map.Add( 0xD183, "\u5E7B" );
                map.Add( 0xD184, "\u5F15" );
                map.Add( 0xD185, "\u5FC3" );
                map.Add( 0xD186, "\u6238" );
                map.Add( 0xD187, "\u624B" );
                map.Add( 0xD188, "\u652F" );
                map.Add( 0xD189, "\u6587" );
                map.Add( 0xD18A, "\u6597" );
                map.Add( 0xD18B, "\u65B9" );
                map.Add( 0xD18C, "\u65E5" );
                map.Add( 0xD18D, "\u6708" );
                map.Add( 0xD18E, "\u6728" );
                map.Add( 0xD18F, "\u6B20" );
                map.Add( 0xD190, "\u6B62" );
                map.Add( 0xD191, "\u6BD4" );
                map.Add( 0xD192, "\u6BDB" );
                map.Add( 0xD193, "\u6C34" );
                map.Add( 0xD194, "\u706B" );
                map.Add( 0xD195, "\u722A" );
                map.Add( 0xD196, "\u7236" );
                map.Add( 0xD197, "\u7247" );
                map.Add( 0xD198, "\u725B" );
                map.Add( 0xD199, "\u72AC" );
                map.Add( 0xD19A, "\u738B" );
                map.Add( 0xD19C, "\u5EB7" );
                map.Add( 0xD19D, "\u808C" );
                map.Add( 0xD19E, "\u72AF" );
                map.Add( 0xD19F, "\u5C4D" );
                map.Add( 0xD1A0, "\u6557" );
                map.Add( 0xD1A1, "\u6211" );
                map.Add( 0xD1A2, "\u767B" );
                map.Add( 0xD1A3, "\u9332" );
                map.Add( 0xD1A4, "\u4E18" );
                map.Add( 0xD1A5, "\u4E16" );
                map.Add( 0xD1A6, "\u4E3B" );
                map.Add( 0xD1A7, "\u4EE5" );
                map.Add( 0xD1A8, "\u4ED5" );
                map.Add( 0xD1A9, "\u4ED9" );
                map.Add( 0xD1AA, "\u4ED6" );
                map.Add( 0xD1AB, "\u4EE3" );
                map.Add( 0xD1AC, "\u4ED8" );
                map.Add( 0xD1AD, "\u4EE4" );
                map.Add( 0xD1AE, "\u5144" );
                map.Add( 0xD1AF, "\u5199" );
                map.Add( 0xD1B1, "\u51FA" );
                map.Add( 0xD1B2, "\u52A0" );
                map.Add( 0xD1B3, "\u5305" );
                map.Add( 0xD1B4, "\u5317" );
                map.Add( 0xD1B5, "\u534A" );
                map.Add( 0xD1B6, "\u5360" );
                map.Add( 0xD1B7, "\u53BB" );
                map.Add( 0xD1B8, "\u53CE" );
                map.Add( 0xD1B9, "\u53EF" );
                map.Add( 0xD1BA, "\u529F" );
                map.Add( 0xD1BB, "\u53E5" );
                map.Add( 0xD1BC, "\u53E4" );
                map.Add( 0xD1BD, "\u53F7" );
                map.Add( 0xD1BE, "\u53F2" );
                map.Add( 0xD1BF, "\u53F8" );
                map.Add( 0xD1C0, "\u53EC" );
                map.Add( 0xD1C1, "\u53F0" );
                map.Add( 0xD1C2, "\u53F3" );
                map.Add( 0xD1C3, "\u56DB" );
                map.Add( 0xD1C4, "\u56DA" );
                map.Add( 0xD1C5, "\u5727" );
                map.Add( 0xD1C6, "\u51AC" );
                map.Add( 0xD1C7, "\u5916" );
                map.Add( 0xD1C8, "\u592E" );
                map.Add( 0xD1C9, "\u5931" );
                map.Add( 0xD1CA, "\u5974" );
                map.Add( 0xD1CB, "\u5C3B" );
                map.Add( 0xD1CC, "\u5DE6" );
                map.Add( 0xD1CD, "\u5E02" );
                map.Add( 0xD1CE, "\u5E03" );
                map.Add( 0xD1CF, "\u5E73" );
                map.Add( 0xD200, "\u5E7C" );
                map.Add( 0xD201, "\u5E83" );
            }
        }

        private static void BuildVersion3Charmap(IDictionary<int, string> PSXMap, IDictionary<int, string> PSPMap)
        {
            IList<string> psxChars = PatcherLib.PSXResources.CharacterSet;
            IList<string> pspChars = PatcherLib.PSPResources.CharacterSet;

            for ( int i = 0; i < 0xD0; i++ )
            {
                PSXMap[i] = psxChars[i];
                PSPMap[i] = pspChars[i];
                PSXMap[i + 0xD000] = psxChars[i];
                PSPMap[i + 0xD000] = pspChars[i];
            }
            for ( int i = 0xD0; i < pspChars.Count; i++ )
            {
                PSXMap[( i - 0xD0 ) % 0xD0 + 0xD100 + 0x100 * ( ( i - 0xD0 ) / 0xD0 )] = psxChars[i];
                PSPMap[( i - 0xD0 ) % 0xD0 + 0xD100 + 0x100 * ( ( i - 0xD0 ) / 0xD0 )] = pspChars[i];
            }
        }

        /*
        [DllImport( "FFTTextCompression.dll" )]
        static extern void CompressSection( byte[] input, int inputLength, byte[] output, ref int outputPosition );

        private static void CompressSection( IList<byte> bytes, byte[] output, ref int outputPosition )
        {
            CompressSection( bytes.ToArray(), bytes.Count, output, ref outputPosition );
        }
        */

        private static int CompressSection(IList<byte> bytes, byte[] output, int outputPosition)
        {
            return FFTPatcher.TextEditor.TextCompression.CompressSection(bytes.ToArray(), bytes.Count, output, outputPosition);
        }

        private static void ProcessPointer( IList<byte> bytes, out int length, out int jump )
        {
            length = ((bytes[0] & 0x03) << 3) + ((bytes[1] & 0xE0) >> 5) + 4;
            int j = ((bytes[1] & 0x1F) << 8) + bytes[2];
            jump = j - (j / 256) * 2;
        }

        /// <summary>
        /// Compresses the specified file.
        /// </summary>
        /// <typeparam name="T">Must be <see cref="IStringSectioned"/> and <see cref="ICompressed"/></typeparam>
        /// <param name="file">The file to compress.</param>
        /// <param name="ignoreSections">A dictionary indicating which entries to not compress, with each key being the section that contains the ignored
        /// entries and each item in the value being an entry to ignore</param>
        /// <param name="callback">The progress callback.</param>
        public static CompressionResult Compress( IList<IList<string>> sections, byte terminator, GenericCharMap charmap, IList<bool> allowedSections )
        {
            int length = 0;
            //sections.ForEach( s => length += charmap.StringsToByteArray( s, terminator ).Length );

            int sectionCount = sections.Count;
            byte[][] sectionBytes = new byte[sectionCount][];
            for (int section = 0; section < sectionCount; section++)
            {
                byte[] bytes = charmap.StringsToByteArray(sections[section], terminator);
                length += bytes.Length;
                sectionBytes[section] = bytes;
            }

            byte[] result = new byte[length];
            int[] lengths = new int[sectionCount];

            int pos = 0;
            for (int section = 0; section < sectionCount; section++)
            {
                int oldPos = pos;

                if ( allowedSections == null || allowedSections[section] )
                {
                    //CompressSection( charmap.StringsToByteArray( sections[section], terminator ), result, ref pos );
                    //pos = CompressSection(charmap.StringsToByteArray(sections[section], terminator), result, pos);
                    pos = CompressSection(sectionBytes[section], result, pos);
                }
                else
                {
                    //byte[] secResult = charmap.StringsToByteArray( sections[section], terminator );
                    byte[] secResult = sectionBytes[section];
                    secResult.CopyTo( result, pos );
                    pos += secResult.Length;
                }

                lengths[section] = pos - oldPos;

            }

            return new CompressionResult( result.Sub( 0, pos - 1 ), lengths );
        }

        /// <summary>
        /// Decompresses the specified section.
        /// </summary>
        /// <param name="allBytes">A collection containing the bytes to decompress.</param>
        /// <param name="sectionBytes">A collection consisting ONLY of the bytes to decompress</param>
        /// <param name="sectionStart">The relative position of <paramref name="sectionBytes"/> in <paramref name="allBytes"/></param>
        /// <returns></returns>
        public static IList<byte> Decompress( IList<byte> allBytes, IList<byte> sectionBytes, int sectionStart )
        {
            IList<byte> result = new List<byte>();

            for( int i = 0; i < sectionBytes.Count; i++ )
            {
                if( sectionBytes[i] >= 0xF0 && sectionBytes[i] <= 0xF3 )
                {
                    int length;
                    int jump;
                    ProcessPointer( new byte[] { sectionBytes[i], sectionBytes[i + 1], sectionBytes[i + 2] }, out length, out jump );
                    if( (i + sectionStart - jump) < 0 || (i + sectionStart - jump + length) >= allBytes.Count )
                    {
                        result.AddRange( new byte[] { sectionBytes[i], sectionBytes[i + 1], sectionBytes[i + 2] } );
                    }
                    else
                    {
                        result.AddRange( allBytes.Sub( i + sectionStart - jump, i + sectionStart - jump + length - 1 ) );
                    }
                    i += 2;
                }
                else
                {
                    result.Add( sectionBytes[i] );
                }
            }

            return result;
        }

        public static IList<string> ProcessList(IList<byte> bytes, Set<byte> terminators, GenericCharMap charmap)
        {
            if (terminators.Count == 1)
                return ProcessList(bytes, terminators[0], charmap);

            IList<IList<byte>> words = bytes.Split(terminators);

            List<string> result = new List<string>(words.Count);

            foreach (IList<byte> word in words)
            {
                StringBuilder sb = new StringBuilder();
                int pos = 0;
                while (pos < (word.Count - 1) || (pos == (word.Count - 1) && !terminators.Contains(word[pos])))
                {
                    sb.Append(charmap.GetNextChar(word, ref pos));
                }

                sb.Replace(@"{Newline}", @"{Newline}" + Environment.NewLine);

                result.Add(sb.ToString());
            }

            return result;
        }

        /// <summary>
        /// Processes a list of FFT text bytes into a list of FFTacText strings.
        /// </summary>
        /// <param name="bytes">The bytes to process</param>
        /// <param name="charmap">The charmap to use</param>
        public static IList<string> ProcessList( IList<byte> bytes, byte terminator, GenericCharMap charmap )
        {
            IList<IList<byte>> words = bytes.Split( terminator );

            List<string> result = new List<string>( words.Count );

            foreach ( IList<byte> word in words )
            {
                StringBuilder sb = new StringBuilder();
                int pos = 0;
                while ( pos < ( word.Count - 1 ) || ( pos == ( word.Count - 1 ) && word[pos] != terminator ) )
                {
                    sb.Append( charmap.GetNextChar( word, ref pos ) );
                }

                sb.Replace(@"{Newline}", @"{Newline}" + Environment.NewLine);

                result.Add( sb.ToString() );
            }

            return result;
        }

        /// <summary>
        /// Replaces bracketed hex values (e.g. {0xD2E3}) with Unicode characters, if known.
        /// </summary>
        /// <param name="s">The string to update.</param>
        /// <param name="charMap">The character map to use.</param>
        /// <returns>The updated string</returns>
        public static string UpgradeString( string s, GenericCharMap charMap )
        {
            string pattern = @"{0x[A-Fa-f0-9][A-Fa-f0-9][A-Fa-f0-9][A-Fa-f0-9]}";
            MatchCollection matches = Regex.Matches( s, pattern );
            if( matches.Count > 0 )
            {
                StringBuilder sb = new StringBuilder( s );
                foreach( Match m in matches )
                {
                    int val = Convert.ToInt32( m.Value.Substring( 3, 4 ), 16 );
                    if( charMap.ContainsKey( val ) )
                    {
                        sb.Replace( m.Value, charMap[val] );
                    }
                }

                return sb.ToString();
            }

            return s;
        }


        #endregion Methods

        public class GroupableSet
        {
            public string Group { get; private set; }
            public IList<int> Indices { get; private set; }
            public int OriginalCost { get; private set; }

            private int hashCode = 0;

            public GroupableSet( string group, IList<int> indices, int originalCost )
            {
                Group = group;
                Indices = indices.AsReadOnly();
                OriginalCost = originalCost;
                hashCode = group.GetHashCode();
            }


            public override int GetHashCode()
            {
                return hashCode;
            }
        }

        private static IList<byte> IntToBytes( int val )
        {
            List<byte> result = new List<byte>( 4 );
            do
            {
                result.Add( (byte)( val & 0xFF ) );
                val /= 256;
            } while ( val > 0 );
            result.Reverse();
            return result.ToArray();
        }

        public static Set<string> GetGroups( GenericCharMap charmap, IList<string> characterSet, IList<int> characterWidths )
        {
            const bool alphaNumOnly = true;
            const string allowedAlphaNum = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?:\"' ./";
            System.Diagnostics.Debug.Assert( allowedAlphaNum.Length == 10 + 26 + 26 + 8 );
            Dictionary<string,string> allowedPairs = new Dictionary<string,string> {
              { "0", "0123456789 ./" }, // 0
              { "1", "0123456789!? ./" }, // 1
              { "2", "0123456789!? ./" }, // 2
              { "3", "0123456789!? ./" },
              { "4", "0123456789!? ./" },
              { "5", "0123456789 ./" },
              { "6", "0123456789 ./" },
              { "7", "0123456789 ./" },
              { "8", "0123456789 ./" },
              { "9", "0123456789 ./" }, // 9
              { "A", "bcdfghijklmnpqrstuvwxyz!?\"' ./" }, // A
              { "B", "aeilmorsuy!?\"' ./" },
              { "C", "aehiklorstu!?\"' ./" },
              { "D", "aeghilnorstuwy!?\"' ./" }, // D
              { "E", "abcdefghijklmnopqrstuvwxyz!?\"' ./" },
              { "F", "aeiorstuy!?\"' ./" },
              { "G", "adehilnorsu!?\"' ./" },
              { "H", "aeiou!?\"' ./" },
              { "I", "cdefghjklmnopqrstuvwxyz!?\"' ./" },
              { "J", "aeious!?\"' ./" }, // J
              { "K", "aeilostuy!?\"' ./" },
              { "L", "aehiou!?\"' ./" },
              { "M", "aeiou!?\"' ./" },
              { "N", "aeiou!?\"' ./" },
              { "O", "abcdefghijklmnprstuvwxyz!?\"' ./" },
              { "P", "aehioruy!?\"' ./" }, // P
              { "Q", "u!?\"' ./" },
              { "R", "aehiouwy!?\"' ./" },
              { "S", "acdefghiklmnopqrtuwyz!?\"' ./" },
              { "T", "aehiouwy!?\"' ./" },
              { "U", "ghlmnoprst!?\"' ./" },
              { "V", "aeiouy!?\"' ./" }, // V
              { "W", "aehiouy!?\"' ./" },
              { "X", "aeiou!?\"' ./" },
              { "Y", "aeiou!?\"' ./" },
              { "Z", "aeiou!?\"' ./" }, // Z
              { "a", "bcdefghijklmnopqrstuvwxyz!?:\"' ./" }, // a
              { "b", "abeilmorsuy!?:\"' ./" },
              { "c", "acehiklorstu!?:\"' ./" },
              { "d", "adeghilnorstuwy!?:\"' ./" }, // d
              { "e", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "f", "aefiorstuy!?:\"' ./" },
              { "g", "adeghilnorsu!?:\"' ./" },
              { "h", "acdegilmnoprstuw!?:\"' ./" },
              { "i", "abcdefghjklmnopqrstuvwxyz!?:\"' ./" },
              { "j", "aeiosuy!?:\"' ./" }, // j
              { "k", "aeiklorsuwy!?:\"' ./" },
              { "l", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "m", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "n", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "o", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "p", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" }, // p
              { "q", "u!?:\"'./" },
              { "r", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "s", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "t", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "u", "abcdefghijklmnopqrstvwxyz!?:\"' ./" },
              { "v", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" }, // v
              { "w", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "x", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "y", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" },
              { "z", "abcdefghijklmnopqrstuvwxyz!?:\"' ./" }, // z

              { "!", "!?\" /" }, // !
              { "?", "!?\" /" }, // ?
              { ":", "!?\" /" }, // :
              { "\"", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?: .,/" }, // "
              { "'", "dlrstv /" }, // '
              { " ", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz /" }, //  
              { ".", "!?\"' ./" }, // .
              { "/", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?:\"' .,/" }
            };
            
            List<string> allowed = new List<string>( allowedAlphaNum.Length );
            for ( int i = 0; i < allowedAlphaNum.Length; i++ )
                allowed.Add( allowedAlphaNum[i].ToString( System.Globalization.CultureInfo.InvariantCulture ) );
            
            Set<string> allowedChars = new Set<string>( allowed );

            const int maxWidth = 10;
            Set<string> result = new Set<string>();

            System.Diagnostics.Debug.Assert( characterSet.Count == characterWidths.Count );
            for ( int i = 0; i < characterSet.Count; i++ )
            {
                if ( !alphaNumOnly || allowedChars.Contains( characterSet[i] ) )
                {
                    for ( int j = i; j < characterSet.Count; j++ )
                    {
                        bool goodWidth = characterWidths[i] + characterWidths[j] <= maxWidth;
                        if ( goodWidth &&
                             ( !alphaNumOnly || allowedPairs[characterSet[i]].Contains( characterSet[j] ) ) ) 
                        {
                            result.Add( characterSet[i] + characterSet[j] );
                        }
                        if ( goodWidth &&
                             ( !alphaNumOnly || 
                               (allowedPairs.ContainsKey(characterSet[j]) &&
                                allowedPairs[characterSet[j]].Contains( characterSet[i] ) ) ))
                        {
                            result.Add( characterSet[j] + characterSet[i] );
                        }

                    }
                }
            }

            return result;
        }

        private static void IncrementPairCount( string pair, IDictionary<string, int> dict )
        {
            if( dict.ContainsKey( pair ) )
            {
                dict[pair] += 1;
            }
            else
            {
                dict[pair] = 1;
            }
        }

        public static IDictionary<string, int> GetPairAndTripleCounts( string text, Set<string> allowedGroups )
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();
            text = text.Replace( "\r\n", "{}" );
            int length = text.Length;
            
            Action<int> meth =
                delegate( int groupSize )
                {
                    for ( int pos = 0; pos + groupSize - 1 < length; )
                    {
                        for ( int j = 1; j < groupSize; j++ )
                        {
                            if ( text[pos + j] == '{' )
                            {
                                pos++;
                            }
                        }
                        while ( pos < length && text[pos] == '{' )
                        {
                            while ( text[++pos] != '}' )
                                ;
                            pos++;
                        }

                        if ( pos + groupSize - 1 < length )
                        {
                            IncrementPairCount( text.Substring( pos, groupSize ), counts );
                        }
                        pos++;
                    }
                };

            meth( 2 );

            counts.RemoveAll( p => !allowedGroups.Contains( p ) );
            return counts;
        }



        /// <summary>
        /// The result of a compression operation.
        /// </summary>
        public class CompressionResult
        {

            #region Properties (2)


            /// <summary>
            /// Gets the compressed bytes.
            /// </summary>
            public IList<byte> Bytes { get; private set; }

            /// <summary>
            /// Gets the lenght of each section.
            /// </summary>
            public IList<int> SectionLengths { get; private set; }


            #endregion Properties

            #region Constructors (2)

            private CompressionResult()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CompressionResult"/> class.
            /// </summary>
            /// <param name="bytes">The bytes.</param>
            /// <param name="sectionLengths">The section lengths.</param>
            public CompressionResult( IList<byte> bytes, IList<int> sectionLengths )
            {
                Bytes = bytes;
                SectionLengths = sectionLengths;
            }

            #endregion Constructors

        }

        public static void DoDTEEncoding( IList<IList<string>> strings, IList<bool> allowedSections, IDictionary<string, byte> dteTable )
        {
            for( int i = 0; i < strings.Count; i++ )
            {
                if ( allowedSections[i] )
                {
                    DoDTEEncoding( strings[i], dteTable );
                }
            }
        }

        public static void DoDTEEncoding( IList<string> strings, IDictionary<string, byte> dteTable )
        {
            for ( int i = 0; i < strings.Count; i++ )
            {
                strings[i] = DoDTEEncoding( strings[i], dteTable );
            }
        }

        private static string DoDTEEncoding( string text, IDictionary<string, byte> dteTable )
        {
            text = text.Replace( "\r\n", "\n" );
            int length = text.Length;
            StringBuilder sb = new StringBuilder( text.Length );
            Action<int> meth =
                delegate( int groupSize )
                {
                    int pos = 0;
                    for ( pos = 0; pos + groupSize - 1 < length; )
                    {
                        for ( int j = 1; j < groupSize; j++ )
                        {
                            if ( text[pos + j] == '{' )
                            {
                                sb.Append( text[pos] );
                                pos++;
                            }
                        }
                        while ( pos < length && text[pos] == '{' )
                        {
                            sb.Append( text[pos] );
                            while ( text[++pos] != '}' )
                            {
                                sb.Append( text[pos] );
                            }
                            sb.Append( text[pos] );
                            pos++;
                        }

                        if ( pos + groupSize - 1 < length )
                        {
                            string sub = text.Substring( pos, groupSize );
                            if( dteTable.ContainsKey( sub ) )
                            {
                                sb.Append( @"{0x" );
                                sb.AppendFormat( "{0:X2}", dteTable[sub] );
                                sb.Append( @"}" );
                                pos += groupSize;
                            }
                            else
                            {
                                sb.Append( sub[0] );
                                pos += 1;
                            }
                        }
                    }
                    for ( ; pos < length; pos++ )
                    {
                        sb.Append( text[pos] );
                    }
                };
            meth( 2 );
            sb.Replace( "\n", "\r\n" );
            return sb.ToString();
        }
    }
}
