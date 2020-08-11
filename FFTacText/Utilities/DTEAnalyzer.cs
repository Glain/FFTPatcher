using System;
using System.Collections.Generic;
using System.Text;
using PatcherLib.Datatypes;
using PatcherLib;
using PatcherLib.Utilities;
using System.IO;
using PatcherLib.Iso;
using PatcherLib.TextUtilities;

namespace FFTPatcher.TextEditor
{
    static partial class DTE
    {
        public static class DTEAnalyzer
        {
            public static class PSX
            {
                static FFTFont defaultFont = PSXResources.PSXFont;
                static GenericCharMap defaultMap = TextUtilities.PSXMap;

                public static GenericCharMap GetCharMap(Stream iso)
                {
                    IList<byte> dteBytes = PsxIso.ReadFile(iso, DTE.PsxDteTable);
                    return GetCharMap(dteBytes);
                }

                public static GenericCharMap GetCharMap(IList<byte> dteTable)
                {
                    Dictionary<int, string> myCharMap = new Dictionary<int, string>(defaultMap);

                    IList<string> dtePairs = GetDtePairs(dteTable);
                    for (int i = 0; i < dtePairs.Count; i++)
                    {
                        if (string.IsNullOrEmpty(dtePairs[i])) continue;

                        myCharMap[i + DTE.MinDteByte] = dtePairs[i];
                    }

                    return new NonDefaultCharMap(myCharMap);
                }

                static IList<string> GetDtePairs(IList<byte> dteTable)
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
                static FFTFont defaultFont = PSPResources.PSPFont;
                static GenericCharMap defaultMap = TextUtilities.PSPMap;

                public static GenericCharMap GetCharMap(Stream iso)
                {
                    PspIso.PspIsoInfo info = PspIso.PspIsoInfo.GetPspIsoInfo(iso);

                    return GetCharMap(iso, info);
                }

                public static GenericCharMap GetCharMap(Stream iso, PatcherLib.Iso.PspIso.PspIsoInfo info)
                {
                    IList<byte> fontBytes = PspIso.GetBlock(iso, info, DTE.PspFontSection[0]);
                    IList<byte> widthBytes = PspIso.GetBlock(iso, info, DTE.PspFontWidths[0]);

                    return GetCharMap(fontBytes, widthBytes, info);
                }

                private static Glyph DetermineSecondCharacter(IList<Glyph> glyphs, IList<byte> matchBytes, int totalWidth, int firstWidth)
                {
                    //Glyph newGlyph = new Glyph((byte)totalWidth, matchBytes);
                    Glyph newGlyph = new Glyph(0, (byte)totalWidth, matchBytes);
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

                private static Glyph DetermineFirstCharacter(IList<Glyph> glyphs, IList<byte> matchBytes, int width)
                {
                    Glyph newGlyph = new Glyph(0, (byte)width, matchBytes);
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

                public static GenericCharMap GetCharMap(IList<byte> fontBytes, IList<byte> widthBytes, PspIso.PspIsoInfo info)
                {
                    Dictionary<int, string> myCharMap = new Dictionary<int, string>(defaultMap);

                    List<Glyph> glyphs = new List<Glyph>(defaultFont.Glyphs);
                    glyphs.Sort((a, b) => b.Width.CompareTo(a.Width));

                    for (int i = MinDteByte; i < (DTE.MaxDteByte - DTE.MinDteByte + 1); i++)
                    {
                        IList<byte> bytes = fontBytes.Sub(i * DTE.characterSize, (i + 1) * DTE.characterSize - 1);
                        Glyph first = DetermineFirstCharacter(glyphs, bytes, widthBytes[i]);
                        if (first != null)
                        {
                            Glyph second = DetermineSecondCharacter(glyphs, bytes, widthBytes[i], first.Width);
                            if (second != null)
                            {
                                int firstIndex = defaultFont.Glyphs.IndexOf(first);
                                int secondIndex = defaultFont.Glyphs.IndexOf(second);
                                myCharMap[i] = defaultMap[firstIndex] + defaultMap[secondIndex];
                            }
                        }
                    }

                    return new NonDefaultCharMap(myCharMap);
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