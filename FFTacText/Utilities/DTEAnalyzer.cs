using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Datatypes;
using PatcherLib;
using PatcherLib.Utilities;
using System.IO;
using PatcherLib.Iso;

namespace FFTPatcher.TextEditor
{
    static partial class DTE
    {
        public static class DTEAnalyzer
        {
            public static class PSX
            {
                static FFTFont defaultFont = TextUtilities.PSXFont;
                static GenericCharMap defaultMap = TextUtilities.PSXMap;

                private static void BuildCharMapFromIso( Stream iso, out GenericCharMap outCharmap, out IList<Glyph> customGlyphs )
                {
                    var rootDirEnt = DirectoryEntry.GetPsxDirectoryEntries( iso, FFTText.PsxRootDirEntSector, 1 );
                    var charMapEntry = rootDirEnt.Find( d => d.Filename == FFTText.PsxCharmapFileName );

                    System.Diagnostics.Debug.Assert( charMapEntry.Sector == FFTText.PsxCharmapSector );
                    var charmapBytes = PsxIso.GetBlock( iso, new PsxIso.KnownPosition( (PsxIso.Sectors)FFTText.PsxCharmapSector, 0,
                        (int)charMapEntry.Size ) ); 
                    Dictionary<int, string> myCharmap = new Dictionary<int, string>();
                    using (MemoryStream memStream = new MemoryStream( charmapBytes ))
                    using (TextReader reader = new StreamReader( memStream, Encoding.UTF8 ))
                    {
                        // Get header line
                        reader.ReadLine();
                        string currentLine = string.Empty;
                        while ((currentLine = reader.ReadLine()) != null)
                        {
                            string[] cols = currentLine.Split( '\t' );
                            int index = int.Parse( cols[0], System.Globalization.NumberStyles.HexNumber );
                            myCharmap[index] = cols[1];
                        }
                    }

                    outCharmap = new NonDefaultCharMap( myCharmap );
                    customGlyphs = null;
                }

                public static void GetCharMap( Stream iso, out GenericCharMap outCharmap, out IList<Glyph> customGlyphs )
                {
                    var matchBytes = Encoding.UTF8.GetBytes( FFTText.CharmapHeader );
                    var isoBytes = PsxIso.GetBlock( iso, new PsxIso.KnownPosition( (PsxIso.Sectors)FFTText.PsxCharmapSector, 0,
                        matchBytes.Length ) );

                    if (Utilities.CompareArrays( matchBytes, isoBytes ))
                    {
                        BuildCharMapFromIso( iso, out outCharmap, out customGlyphs);
                    }
                    else
                    {
                        IList<byte> dteBytes = PsxIso.ReadFile( iso, DTE.PsxDteTable );
                        outCharmap = GetCharMap( dteBytes );
                        customGlyphs = null;
                    }
                }

                private static GenericCharMap GetCharMap( IList<byte> dteTable )
                {
                    Dictionary<int, string> myCharMap = new Dictionary<int, string>( defaultMap );

                    IList<string> dtePairs = GetDtePairs( dteTable );
                    for (int i = 0; i < dtePairs.Count; i++)
                    {
                        if (string.IsNullOrEmpty( dtePairs[i] )) continue;

                        myCharMap[i + DTE.MinDteByte] = dtePairs[i];
                    }

                    return new NonDefaultCharMap( myCharMap );
                }

                static IList<string> GetDtePairs( IList<byte> dteTable )
                {
                    string[] result = new string[dteTable.Count / 2];
                    for (int i = 0; i < dteTable.Count; i += 2)
                    {
                        if (dteTable[i] == 0 && dteTable[i + 1] == 0) continue;

                        string firstChar = defaultMap[(int)dteTable[i + 0]];
                        string secondChar = defaultMap[(int)dteTable[i + 1]];
                        result[i / 2] = firstChar + secondChar;
                    }

                    return result.AsReadOnly();
                }
            }

            public static class PSP
            {
                static FFTFont defaultFont = TextUtilities.PSPFont;
                static GenericCharMap defaultMap = TextUtilities.PSPMap;

                public static void GetCharMap( Stream iso, out GenericCharMap outCharmap, out IList<Glyph> customGlyphs )
                {
                    PspIso.PspIsoInfo info = PspIso.PspIsoInfo.GetPspIsoInfo( iso );

                    GetCharMap( iso, info, out outCharmap, out customGlyphs );
                }

                public static void GetCharMap( Stream iso, PatcherLib.Iso.PspIso.PspIsoInfo info, out GenericCharMap outCharmap, out IList<Glyph> customGlyphs )
                {
                    var matchBytes = Encoding.UTF8.GetBytes( FFTText.CharmapHeader );
                    if (info.ContainsKey( PspIso.Sectors.PSP_GAME_USRDIR_CHARMAP ))
                    {
                        var isoBytes = PspIso.GetBlock( iso, info,
                            new PspIso.KnownPosition( PspIso.Sectors.PSP_GAME_USRDIR_CHARMAP, 0,
                                matchBytes.Length ) );
                        if (Utilities.CompareArrays( matchBytes, isoBytes ))
                        {
                            BuildCharMapFromIso( iso, info , out outCharmap, out customGlyphs );
                            return;
                        }
                    }

                    IList<byte> fontBytes = PspIso.GetBlock( iso, info, DTE.PspFontSection[0] );
                    IList<byte> widthBytes = PspIso.GetBlock( iso, info, DTE.PspFontWidths[0] );
                    outCharmap = GetCharMap( fontBytes, widthBytes, info );
                    customGlyphs = null;
                }

                private static void BuildCharMapFromIso( Stream iso, PspIso.PspIsoInfo info, out GenericCharMap outCharmap, out IList<Glyph> customGlyphs )
                {
                    var usrDirEnt = DirectoryEntry.GetPspDirectoryEntries( iso, info, PspIso.Sectors.PSP_GAME_USRDIR, 1 );
                    var charMapEntry = usrDirEnt.Find( d => d.Filename == FFTText.PspCharmapFileName );
                    System.Diagnostics.Debug.Assert( charMapEntry.Sector == info[PspIso.Sectors.PSP_GAME_USRDIR_CHARMAP] );
                    var charmapBytes = PspIso.GetBlock( iso, info, new PspIso.KnownPosition(
                         PspIso.Sectors.PSP_GAME_USRDIR_CHARMAP, 0, (int)charMapEntry.Size ) ).ToArray();

                    Dictionary<int, string> myCharMap = new Dictionary<int, string>();
                    using(MemoryStream memStream = new MemoryStream(charmapBytes))
                    using (TextReader reader = new StreamReader( memStream, Encoding.UTF8 ))
                    {
                        reader.ReadLine();

                        string currentLine = string.Empty;
                        while ((currentLine = reader.ReadLine()) != null)
                        {
                            string[] cols = currentLine.Split( '\t' );
                            int index = int.Parse( cols[0], System.Globalization.NumberStyles.HexNumber );
                            myCharMap[index] = cols[1];
                        }
                    }

                    outCharmap = new NonDefaultCharMap( myCharMap );
                    customGlyphs = null;
                }

                private static Glyph DetermineSecondCharacter( IList<Glyph> glyphs, IList<byte> matchBytes, int totalWidth, int firstWidth )
                {
                    Glyph newGlyph = new Glyph( 0, (byte)totalWidth, matchBytes );
                    foreach (Glyph g in glyphs)
                    {
                        if (g.Width > (totalWidth - firstWidth)) continue;
                        for (int x = 0; x < g.Width; x++)
                        {
                            for (int y = 0; y < FFTFont.CharacterHeight; y++)
                            {
                                if (g.Pixels[y * FFTFont.CharacterWidth + x] != newGlyph.Pixels[y * FFTFont.CharacterWidth + x + firstWidth])
                                {
                                    goto mainloop;
                                }
                            }
                        }

                        // All pixels matched
                        return g;

mainloop: continue;
                    }

                    return null;
                }

                private static Glyph DetermineFirstCharacter( IList<Glyph> glyphs, IList<byte> matchBytes, int width )
                {
                    Glyph newGlyph = new Glyph( 0, (byte)width, matchBytes );
                    foreach (Glyph g in glyphs)
                    {
                        if (g.Width > width) continue;

                        for (int x = 0; x < g.Width; x++)
                        {
                            for (int y = 0; y < FFTFont.CharacterHeight; y++)
                            {
                                if (g.Pixels[y * FFTFont.CharacterWidth + x] != newGlyph.Pixels[y * FFTFont.CharacterWidth + x])
                                {
                                    goto mainloop;
                                }
                            }
                        }

                        // All pixels matched
                        return g;

mainloop: continue;
                    }

                    return null;
                }

                private static GenericCharMap GetCharMap( IList<byte> fontBytes, IList<byte> widthBytes, PspIso.PspIsoInfo info )
                {
                    Dictionary<int, string> myCharMap = new Dictionary<int, string>( defaultMap );

                    List<Glyph> glyphs = new List<Glyph>( defaultFont.Glyphs );
                    glyphs.Sort( ( a, b ) => b.Width.CompareTo( a.Width ) );

                    for (int i = MinDteByte; i < (DTE.MaxDteByte - DTE.MinDteByte + 1); i++)
                    {
                        IList<byte> bytes = fontBytes.Sub( i * DTE.characterSize, (i + 1) * DTE.characterSize - 1 );
                        Glyph first = DetermineFirstCharacter( glyphs, bytes, widthBytes[i] );
                        if (first != null)
                        {
                            Glyph second = DetermineSecondCharacter( glyphs, bytes, widthBytes[i], first.Width );
                            if (second != null)
                            {

                                int firstIndex = defaultFont.Glyphs.IndexOf( first );
                                int secondIndex = defaultFont.Glyphs.IndexOf( second );

                                firstIndex = firstIndex < 0xD0 ? firstIndex :
                                    (firstIndex - 0xD0) % 0xD0 + 0xD100 + 0x100 * ((firstIndex - 0xD0) / 0xD0);
                                secondIndex = secondIndex < 0xD0 ? secondIndex :
                                    (secondIndex - 0xD0) % 0xD0 + 0xD100 + 0x100 * ((secondIndex - 0xD0) / 0xD0);

                                myCharMap[i] = defaultMap[firstIndex] + defaultMap[secondIndex];
                            }
                        }
                    }

                    return new NonDefaultCharMap( myCharMap );
                    // For each glyph in new font:
                    //   Determine if it's different from default
                    //   Determine the two characters that make it up 
                    //     Sort original font characters DECREASING by WIDTH
                    //     For each character in the original font
                    //       Compare pixels to left side of new font
                    //         look for match -> first character
                    //     After finding first character, subtract character width from current width
                    //     For each character in original font
                    //       Compare pixels to right side of new font
                    //         look for match -> second character
                }
            }
        }
    }
}