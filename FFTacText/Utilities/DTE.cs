using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib;
using PatcherLib.Datatypes;
using PatcherLib.Iso;
using PatcherLib.Utilities;
using PatcherLib.TextUtilities;

namespace FFTPatcher.TextEditor
{
    static partial class DTE
    {
        public class DteException : Exception
        {
            private string message;
            public override string Message
            {
                get { return message; }
            }

            public DteException( IFile failedFile )
            {
                message = string.Format("DTE for {0} failed." + Environment.NewLine + "The ISO has NOT been modified.", failedFile.DisplayName);
            }
        }

        // Apply to WORLD.BIN @ 0x6CAC
        private static IList<byte> worldBin1 =
            new byte[] { 
                0x00, 0x00, 0xed, 0x37, // ori $t5,$ra,0x00
                0x12, 0xc8, 0x00, 0x0c  // jal 0x80032048
            }.AsReadOnly();

        // Apply to WORLD.BIN @ 0x4C04
        private static IList<byte> worldBin2 =
            new byte[] {
                0x7c, 0xc8, 0x00, 0x08, // j 0x800321f0
                0xff, 0xff, 0x42, 0x24  // addiu $v0,$v0,0xffffffff
            }.AsReadOnly();

        // Apply to WORLD.BIN @ 0x4CC0
        private static IList<byte> worldBin3 =
            new byte[] {
                0x8c, 0xc8, 0x00, 0x08, // j 0x80032230
                0xff, 0xff, 0x42, 0x24  // addiu $v0,$v0,0xffffffff
            }.AsReadOnly();

        // Apply to SCUS_942.21 @ 0x229F0
        private static IList<byte> scus1 =
            new byte[] {
                0x03, 0x80, 0x0b, 0x3c, // lui     $t3,0x8003
                0x44, 0x20, 0x6b, 0x91, // lbu     $t3,0x2044($t3)
                0x00, 0x00, 0x00, 0x00, // nop
                0x21, 0x10, 0x4b, 0x00, // addu    $v0,$v0,$t3
                0x18, 0x00, 0x82, 0xac, // sw      $v0,0x18($a0)
                0x02, 0x00, 0x40, 0x14, // bne     $v0,$zero,0xffff2210
                0x00, 0x00, 0x00, 0x00, // nop
                0x03, 0x93, 0x03, 0x08, // j       0x800e4c0c
                0x00, 0x00, 0x00, 0x00, // nop
                0x07, 0x93, 0x03, 0x08, // j       0x800e4c1c
                0x00, 0x00, 0x00, 0x00, // nop
            }.AsReadOnly();

        // Apply to SCUS_942.21 @ 0x22a30
        private static IList<byte> scus2 =
            new byte[] {
                0x03, 0x80, 0x0b, 0x3c, // lui     $t3,0x8003
                0x44, 0x20, 0x6b, 0x91, // lbu     $t3,0x2044($t3)
                0x00, 0x00, 0x00, 0x00, // nop
                0x21, 0x10, 0x4b, 0x00, // addu    $v0,$v0,$t3
                0x00, 0x00, 0x82, 0xac, // sw      $v0,0x0($a0)
                0x02, 0x00, 0x40, 0x14, // bne     $v0,$zero,0xffff2250
                0x00, 0x00, 0x00, 0x00, // nop
                0x32, 0x93, 0x03, 0x08, // j       0x800e4cc8
                0x00, 0x00, 0x00, 0x00, // nop
                0x36, 0x93, 0x03, 0x08, // j       0x800e4cd8
                0x00, 0x00, 0x00, 0x00, // nop
            }.AsReadOnly();

        // Apply to SCUS_942.21 @ 0x22848
        private static IList<byte> scus3 =
            new byte[] {
                0x88, 0x00, 0xa8, 0x97, // lhu     $t0,0x88($sp)
                0x30, 0x00, 0xa2, 0x97, // lhu     $v0,0x30($sp)
                0x03, 0x80, 0x0e, 0x3c, // lui     $t6,0x8003
                0xd0, 0x00, 0x03, 0x2a, // slti    $v1,$s0,0xd0
                0x0f, 0x00, 0x60, 0x10, // beq     $v1,$zero,0xffff2098
                0x56, 0x00, 0x0f, 0x2a, // slti    $t7,$s0,0x56
                0x0d, 0x00, 0xe0, 0x15, // bne     $t7,$zero,0xffff2098
                0x03, 0x80, 0x0c, 0x3c, // lui     $t4,0x8003
                0x44, 0x20, 0xcf, 0x91, // lbu     $t7,0x2044($t6)
                0x34, 0x20, 0x8c, 0x35, // ori     $t4,$t4,0x2034
                0x21, 0x60, 0x8f, 0x01, // addu    $t4,$t4,$t7
                0x40, 0x80, 0x10, 0x00, // sll     $s0,$s0,0x1
                0x21, 0x60, 0x90, 0x01, // addu    $t4,$t4,$s0
                0x00, 0x00, 0x90, 0x91, // lbu     $s0,0x0($t4)
                0x00, 0x00, 0x00, 0x00, // nop
                0x07, 0x00, 0x00, 0x12, // beq     $s0,$zero,0xffff20a4
                0x00, 0x00, 0x00, 0x00, // nop
                0x01, 0x00, 0xef, 0x39, // xori    $t7,$t7,0x1
                0x44, 0x20, 0xcf, 0xa1, // sb      $t7,0x2044($t6)
                0x23, 0xa0, 0x8f, 0x02, // subu    $s4,$s4,$t7
                0x2c, 0x00, 0xa3, 0x97, // lhu     $v1,0x2c($sp)
                0x08, 0x00, 0xe0, 0x03, // jr      $ra
                0x00, 0x00, 0xbf, 0x35, // ori     $ra,$t5,0x0
                0x2c, 0x00, 0xa3, 0x97, // lhu     $v1,0x2c($sp)
                0x00, 0x00, 0x90, 0x92, // lbu     $s0,0x0($s4)
                0x08, 0x00, 0xe0, 0x03, // jr      $ra
                0x00, 0x00, 0xbf, 0x35  // ori     $ra,$t5,0x0
            }.AsReadOnly();

        static IList<PatchedByteArray> psxDtePatches = new PatchedByteArray[] {
            new PatchedByteArray( PsxIso.Sectors.WORLD_WORLD_BIN, 0x6CAC, worldBin1.ToArray() ),
            new PatchedByteArray( PsxIso.Sectors.WORLD_WORLD_BIN, 0x4c04, worldBin2.ToArray() ),
            new PatchedByteArray( PsxIso.Sectors.WORLD_WORLD_BIN, 0x4cc0, worldBin3.ToArray() ),
            new PatchedByteArray( PsxIso.Sectors.SCUS_942_21, 0x229F0, scus1.ToArray() ),
            new PatchedByteArray( PsxIso.Sectors.SCUS_942_21, 0x22a30, scus2.ToArray() ),
            new PatchedByteArray( PsxIso.Sectors.SCUS_942_21, 0x22848, scus3.ToArray() ) }.AsReadOnly();

        public static bool DoesPsxIsoHaveDtePatches(System.IO.Stream iso)
        {
            foreach (var pba in psxDtePatches)
            {
                byte[] patchBytes = pba.GetBytes();
                byte[] streambytes = 
                    PsxIso.GetBlock(iso, new PsxIso.KnownPosition((PsxIso.Sectors)pba.Sector, (int)pba.Offset, patchBytes.Length));
                if (!Utilities.CompareArrays(patchBytes, streambytes))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool DoesPspIsoHaveNonDefaultFont( System.IO.Stream iso, PatcherLib.Iso.PspIso.PspIsoInfo info )
        {
            IList<byte> fontBytes = PspIso.GetBlock( iso, info, DTE.PspFontSection[0] );
            IList<byte> widthBytes = PspIso.GetBlock( iso, info, DTE.PspFontWidths[0] );
            IList<byte> defaultFontBytes = PSPResources.PSPFont.ToByteArray();
            IList<byte> defaultWidthBytes = PSPResources.PSPFont.ToWidthsByteArray();

            return 
                !Utilities.CompareArrays( fontBytes, defaultFontBytes ) ||
                !Utilities.CompareArrays( widthBytes, defaultWidthBytes );
        }

        private static Set<string> GetDteGroups( FFTFont font, GenericCharMap charmap, IList<string> charset )
        {
            var f = new FFTFont( font.ToByteArray(), font.ToWidthsByteArray() );
            List<int> widths = new List<int>( 2200 );
            f.Glyphs.ForEach( g => widths.Add( g.Width ) );
            return TextUtilities.GetGroups( charmap, charset, widths );
        }

        /// <summary>
        /// Gets a set of groups that are candidates for DTE encoding
        /// </summary>
        public static Set<string> GetDteGroups( Context context )
        {
            return context == Context.US_PSP ? GetPspDteGroups() : GetPsxDteGroups();
        }

        private static Set<string> GetPsxDteGroups()
        {
            return GetDteGroups( new FFTFont( PSXResources.Binaries.Font, PSXResources.Binaries.FontWidths ), TextUtilities.PSXMap, PSXResources.CharacterSet );
        }

        private static Set<string> GetPspDteGroups()
        {
            return GetDteGroups( new FFTFont( PSPResources.Binaries.Font, PSXResources.Binaries.FontWidths ), TextUtilities.PSPMap, PSPResources.CharacterSet );
        }


        /// <summary>
        /// Generates a list of patches for PSP based on the set of DTE encodings passed.
        /// Generates font patches and ASM patches.
        /// </summary>
        public static IList<PatchedByteArray> GeneratePspDtePatches(IEnumerable<KeyValuePair<string, byte>> dteEncodings)
        {
            var charSet = PSPResources.CharacterSet;
            FFTFont font = new FFTFont( PSPResources.Binaries.Font, PSPResources.Binaries.FontWidths );

            byte[] fontBytes;
            byte[] widthBytes;

            GenerateFontBinPatches( dteEncodings, font, charSet, out fontBytes, out widthBytes );

            fontBytes = fontBytes.Sub( MinDteByte * characterSize, ( MaxDteByte + 1 ) * characterSize - 1 ).ToArray();
            var widths = PspFontWidths;
            var fontDteSection = PspFontDteSection;
            return
                new PatchedByteArray[] {
                    fontDteSection[0].GetPatchedByteArray(fontBytes),
                    fontDteSection[1].GetPatchedByteArray(fontBytes),
                    new PatchedByteArray(PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x27b80c+MinDteByte*characterSize, fontBytes),
                    new PatchedByteArray(PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x2f73b8+MinDteByte*characterSize, fontBytes),
                    widths[0].GetPatchedByteArray(widthBytes),
                    widths[1].GetPatchedByteArray(widthBytes),
                    new PatchedByteArray(PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x293f40, widthBytes),
                    new PatchedByteArray(PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x30fac0, widthBytes)
                };
        }

        public static IList<PspIso.KnownPosition> PspFontSection
        {
            get
            {
                return new PspIso.KnownPosition[] {
                    new PspIso.KnownPosition(PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x27B80C, 2200*characterSize),
                    new PspIso.KnownPosition(PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x2F73B8, 2200*characterSize) };
            }
        }
        
        public static IList<PspIso.KnownPosition> PspFontDteSection
        {
            get
            {
                return new PspIso.KnownPosition[] {
                    new PspIso.KnownPosition(PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x27B80C+MinDteByte*characterSize, (MaxDteByte-MinDteByte+1)*characterSize),
                    new PspIso.KnownPosition(PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x2F73B8+MinDteByte*characterSize, (MaxDteByte-MinDteByte+1)*characterSize) };
            }
        }

        public static IList<PspIso.KnownPosition> PspFontWidths
        {
            get 
            {
                return new PspIso.KnownPosition[] {
                    new PspIso.KnownPosition(PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x293F40, 2200),
                    new PspIso.KnownPosition(PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x30FAC0, 2200) };

            }
        }

        /// <summary>
        /// Generates a list of patches for PSX based on the set of DTE encodings passed.
        /// Generates font patches and ASM patches.
        /// </summary>
        public static IList<PatchedByteArray> GeneratePsxDtePatches(IEnumerable<KeyValuePair<string, byte>> dteEncodings)
        {
            // BATTLE.BIN -> 0xE7614
            // FONT.BIN -> 0
            // WORLD.BIN -> 0x5B8F8

            var charSet = PSXResources.CharacterSet;
            FFTFont font = new FFTFont( PSXResources.Binaries.Font, PSXResources.Binaries.FontWidths );

            byte[] fontBytes;
            byte[] widthBytes;

            GenerateFontBinPatches( dteEncodings, font, charSet, out fontBytes, out widthBytes );

            fontBytes = fontBytes.Sub( MinDteByte * characterSize, ( MaxDteByte + 1 ) * characterSize - 1 ).ToArray();
            // widths:
            // 0x363234 => 1510 = BATTLE.BIN
            // 0xBD84908 => 84497 = WORLD.BIN

            var result = new List<PatchedByteArray>();
            result.AddRange( psxDtePatches );
            result.AddRange( new PatchedByteArray[] {
                    new PatchedByteArray(PsxIso.Sectors.BATTLE_BIN, 0xE7614+MinDteByte*characterSize, fontBytes),
                    new PatchedByteArray(PsxIso.Sectors.EVENT_FONT_BIN, 0x00+MinDteByte*characterSize, fontBytes),
                    new PatchedByteArray(PsxIso.Sectors.WORLD_WORLD_BIN, 0x5B8f8+MinDteByte*characterSize, fontBytes),
                    new PatchedByteArray(PsxIso.Sectors.BATTLE_BIN, 0xFF0FC, widthBytes),
                    new PatchedByteArray(PsxIso.Sectors.WORLD_WORLD_BIN, 0x733E0, widthBytes),
                    PsxDteTable.GetPatchedByteArray(GeneratePsxLookupTable(dteEncodings, charSet).ToArray())
                } );
            
            return result;
        }


        public static PsxIso.KnownPosition PsxDteTable
        {
            get
            {
                return new PsxIso.KnownPosition( PsxIso.Sectors.SCUS_942_21, 0x228E0, ( MaxDteByte - MinDteByte + 1 ) * 2 );
            }
        }

        /// <summary>
        /// Generates a list of patches based on the set of DTE encodings passed.
        /// Generates font patches and ASM patches.
        /// </summary>
        public static IList<PatchedByteArray> GenerateDtePatches(Context context, IEnumerable<KeyValuePair<string, byte>> dteEncodings)
        {
            return context == Context.US_PSP ? GeneratePspDtePatches( dteEncodings ) : GeneratePsxDtePatches( dteEncodings );
        }

        /// <summary>
        /// Gets a collection of all the bytes that are available for DTE.
        /// </summary>
        public static Stack<byte> GetAllowedDteBytes()
        {
            Stack<byte> result = new Stack<byte>();

            for ( byte b = MaxDteByte; b >= 0xB6; b-- )
            {
                result.Push( b );
            }
            result.Push( 0xb4 );
            result.Push( 0xb3 );
            for ( byte b = 0xB1; b >= 0x94; b-- )
            {
                result.Push( b );
            }
            result.Push( 0x92 );
            result.Push( 0x90 );
            result.Push( 0x8F );
            result.Push( 0x8C );
            for ( byte b = 0x8A; b >= 0x60; b-- )
            {
                result.Push( b );
            }
            for ( byte b = 0x5E; b >= MinDteByte; b-- )
            {
                result.Push( b );
            }
            //result.Push( 0x45 );
            //result.Push( 0x43 );
            //result.Push( 0x41 );
            //result.Push( 0x3f );

            return result;
        }

        const int characterSize = ( 14 * 10 ) / 4;
        public const byte MinDteByte = 0x56;
        public const byte MaxDteByte = 0xcf;

        private static IList<byte> GeneratePsxLookupTable(
            IEnumerable<KeyValuePair<string, byte>> dteEncodings,
            IList<string> baseCharSet )
        {
            // 2 bytes per lookup pair
            byte[] result = new byte[( MaxDteByte - MinDteByte + 1 ) * 2];

            foreach ( var kvp in dteEncodings )
            {
                TextUtilities.PSXMap.StringToByteArray( kvp.Key, 0xFE )      // Get the pair to store
                    .Sub( 0, 1 )                                       // Get 2 bytes
                    .CopyTo( result, ( kvp.Value - MinDteByte ) * 2 ); // Store them in the DTE array
            }

            return result;
        }

        private static void GenerateFontBinPatches(
            IEnumerable<KeyValuePair<string, byte>> dteEncodings,
            FFTFont baseFont,
            IList<string> baseCharSet,
            out byte[] fontBytes,
            out byte[] widthBytes )
        {
            // Make a copy of the font
            FFTFont font =
                new FFTFont( baseFont.ToByteArray(), baseFont.ToWidthsByteArray() );

            IList<string> charSet = new List<string>( baseCharSet );

            foreach ( var kvp in dteEncodings )
            {
                int[] chars = 
                    new int[] { 
                        charSet.IndexOf( kvp.Key.Substring( 0, 1 ) ), // Find the index of the first character in the pair
                        charSet.IndexOf( kvp.Key.Substring( 1, 1 ) )  // Second character in the pair
                    };
                int[] widths = 
                    new int[] { 
                        font.Glyphs[chars[0]].Width, // width of first char
                        font.Glyphs[chars[1]].Width  // width of secont char
                    };

                // The width of the concatenated character is the sum...
                int newWidth = widths[0] + widths[1]; 
                font.Glyphs[kvp.Value].Width = (byte)newWidth;

                // Erase all the pixels of the character to replace
                IList<FontColor> newPixels = font.Glyphs[kvp.Value].Pixels;
                for ( int i = 0; i < newPixels.Count; i++ )
                {
                    newPixels[i] = FontColor.Transparent;
                }

                const int fontHeight = 14;
                const int fontWidth = 10;

                int offset = 0;
                // for each character in the pair...
                for ( int c = 0; c < chars.Length; c++ )
                {
                    var pix = font.Glyphs[chars[c]].Pixels;

                    // ... copy the pixels to the concatenated character
                    for ( int x = 0; x < widths[c]; x++ )
                    {
                        for ( int y = 0; y < fontHeight; y++ )
                        {
                            newPixels[y * fontWidth + x + offset] = pix[y * fontWidth + x];
                        }
                    }

                    offset += widths[c];
                }
            }

            // Return the new font and widths arrays
            fontBytes = font.ToByteArray();
            widthBytes = font.ToWidthsByteArray();
        }


        public static IList<byte> GenerateTable( IEnumerable<KeyValuePair<string, byte>> dteEncodings, GenericCharMap charmap )
        {
            Dictionary<int, string> dict = new Dictionary<int, string>( charmap );
            foreach ( var kvp in dteEncodings )
            {
                dict[kvp.Value] = kvp.Key;
            }
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>(dict);
            list.Sort((a, b) => a.Key.CompareTo(b.Key));

            StringBuilder result = new StringBuilder();
            foreach ( var kvp in list )
            {
                result.AppendFormat( "{0:X2}={1}", kvp.Key, kvp.Value );
                result.Append( Environment.NewLine );
            }

            Encoding e = Encoding.GetEncoding( "shift_jis" );
            return e.GetBytes( result.ToString() );
        }

        public static GenericCharMap GenerateCharMap(IEnumerable<KeyValuePair<int, string>> table)
        {
            Dictionary<int, string> b = new Dictionary<int, string>();
            foreach (var kvp in table)
            {
                b[kvp.Key] = kvp.Value;
            }

            GenericCharMap map = new NonDefaultCharMap(b);

            return map;
        }

        public static GenericCharMap GenerateCharMap( System.IO.StreamReader reader )
        {
            Dictionary<int, string> table = new Dictionary<int, string>();
            while ( !reader.EndOfStream )
            {
                string line = reader.ReadLine();
                string[] lineSplit = line.Split( new char[] { '=' }, 2 );
                table[Int32.Parse( lineSplit[0], System.Globalization.NumberStyles.HexNumber )] = lineSplit[1];
            }

            return GenerateCharMap( table );
        }

        public static GenericCharMap GenerateCharMap( string filename )
        {
            using ( System.IO.Stream stream = System.IO.File.Open( filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read ) )
            {
                return GenerateCharMap( stream );
            }
        }

        public static GenericCharMap GenerateCharMap( System.IO.Stream file )
        {
            using ( System.IO.StreamReader reader = new System.IO.StreamReader( file ) )
            {
                return GenerateCharMap( reader );
            }
        }

    }
}