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
using System.Text.RegularExpressions;
using PatcherLib.Utilities;
using PatcherLib.Datatypes;

namespace PatcherLib.TextUtilities
{
    /// <summary>
    /// A map between FFT text and UTF8.
    /// </summary>
    public abstract class GenericCharMap : PatcherLib.Datatypes.ReadOnlyDictionary<int, string>
    {
        #region Static Fields

        private static Regex regex = new Regex( @"{0x([0-9A-Fa-f]+)}" );
        private static readonly HashSet<byte> readTerminators = new HashSet<byte>() { 0xFE, 0xFF };

        #endregion Static Fields

        #region Fields

        private Dictionary<string, int> reverse = null;

        private IDictionary<int, PatcherLib.Datatypes.Glyph> customGlyphPatches =
            new Dictionary<int, PatcherLib.Datatypes.Glyph>();

        #endregion Fields

        #region Properties


        /// <summary>
        /// Gets a dictionary mapping FFTacText strings into FFT text bytes.
        /// </summary>
        public Dictionary<string, int> Reverse
        {
            get
            {
                if( reverse == null )
                {
                    reverse = BuildValueToKeyMapping();
                }

                return reverse;
            }
        }


        #endregion Properties

        #region Methods (6)


        private Dictionary<string, int> BuildValueToKeyMapping()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach( KeyValuePair<int, string> kvp in this )
            {
                if( !result.ContainsKey( kvp.Value ) )
                {
                    result.Add( kvp.Value, kvp.Key );
                }
                else if( kvp.Key < result[kvp.Value] )
                {
                    result[kvp.Value] = kvp.Key;
                }
            }

            return result;
        }

        private byte[] IntToOneOrTwoOrThreeBytes( int i )
        {
            if( i < 256 )
            {
                return new byte[] { (byte)i };
            }
            else if( i < 65536 )
            {
                return new byte[] { (byte)((i & 0xFF00) >> 8), (byte)(i & 0xFF) };
            }
            else
            {
                return new byte[] { (byte)((i & 0xFF000) >> 16), (byte)((i & 0xFF00) >> 8), (byte)(i & 0xFF) };
            }
        }

        /// <summary>
        /// Gets the character or characters at a particular position in the list.
        /// </summary>
        /// <param name="bytes">The list whose next value is needed</param>
        /// <param name="pos">The current position of the list, this value is updated based on how many bytes were read.</param>
        /// <returns>A FFTText string</returns>
        public string GetNextChar( IList<byte> bytes, ref int pos )
        {
            int resultPos = pos + 1;
            byte val = bytes[pos];
            int key = val;
            if( (val >= 0xD0 && val <= 0xDA) ||
                (val == 0xE2) ||
                (val == 0xE3) ||
                (val == 0xE7) ||
                (val == 0xE8) ||
                (val == 0xEC) ||
                (val == 0xEE) ||
                (val == 0xF5) ||
                (val == 0xF6) )
            {
                byte nextVal = bytes[pos + 1];
                resultPos++;
                key = val * 256 + nextVal;
            }
            else if( val >= 0xF0 && val <= 0xF3 && (pos + 2) < bytes.Count )
            {
                resultPos += 2;
                key = val * 256 * 256 + bytes[pos + 1] * 256 + bytes[pos + 2];
            }

            string result = string.Format( "{{0x{0:X2}", key ) + @"}";
            if( this.ContainsKey( key ) )
            {
                result = this[key];
            }

            pos = resultPos;

            return result;
        }

        /// <summary>
        /// Converts a collection of FFTacText strings into a FFT text byte array.
        /// </summary>
        public byte[] StringsToByteArray( IList<string> strings, byte terminator )
        {
            List<byte> result = new List<byte>();
            foreach( string s in strings )
            {
                result.AddRange( StringToByteArray( s, terminator ) );
            }
            return result.ToArray();
        }

        public string LastError { get; private set; }

        public IList<UInt32> GetEachEncodedCharacter( string s )
        {
            List<UInt32> result = new List<uint>();
            for ( int i = 0; i < s.Length; i++ )
            {
                int val = 0;
                if ( s[i] == '{' )
                {
                    int j = s.IndexOf( '}', i );
                    if ( j == -1 )
                        return null;

                    string key = s.Substring( i, j - i + 1 );
                    if ( Reverse.ContainsKey( key ) )
                    {
                        val = Reverse[key];
                    }
                    else
                    {
                        Match match = regex.Match( key );
                        if ( match.Success )
                        {
                            result.Add( Convert.ToUInt32( match.Groups[1].Value, 16 ) );
                            val = -1;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    i = j;
                }
                else if ( s[i] == '\r' || s[i] == '\n' )
                {
                    // ignore
                    val = -1;
                }
                else
                {
                    string t = s[i].ToString();
                    if ( Reverse.ContainsKey( t ) )
                    {
                        val = Reverse[t];
                    }
                    else
                        return null;
                }

                if ( val >= 0 )
                {
                    result.Add( (UInt32)val );
                }
            }

            return result;
        }

        public GenericCharMap( IDictionary<int, string> dict )
            : base( dict, true )
        {
        }

        public GenericCharMap( IDictionary<int, string> dict, IDictionary<int, PatcherLib.Datatypes.Glyph> customGlyphPatches )
            : base( dict )
        {
            this.customGlyphPatches = new Dictionary<int, PatcherLib.Datatypes.Glyph>( customGlyphPatches );
        }

        public void ModifyFontWithCustomPatches(FFTFont font)
        {
            foreach (var kvp in customGlyphPatches)
            {
                font.Glyphs[kvp.Key] = kvp.Value;
            }
        }

        public bool TryStringToByteArray( string s, byte terminator, out byte[] bytes )
        {
            List<byte> result = new List<byte>( s.Length );
            for ( int i = 0; i < s.Length; i++ )
            {
                int val = 0;
                if ( s[i] == '{' )
                {
                    int j = s.IndexOf( '}', i );
                    if( j == -1 )
                    {
                        LastError = s.Substring( i );
                        bytes = null;
                        return false;
                    }

                    string key = s.Substring( i, j - i + 1 );
                    if ( Reverse.ContainsKey( key ) )
                    {
                        val = Reverse[key];
                    }
                    else
                    {
                        Match match = regex.Match( key );
                        if ( match.Success )
                        {
                            result.AddRange( IntToOneOrTwoOrThreeBytes( Convert.ToInt32( match.Groups[1].Value, 16 ) ) );
                            val = -1;
                        }
                        else
                        {
                            LastError = key;
                            bytes = null;
                            return false;
                        }
                    }
                    i = j;
                }
                else if ( s[i] == '\r' || s[i]=='\n' )
                {
                    // ignore
                    val = -1;
                }
                else 
                {
                    string t = s[i].ToString();
                    if ( Reverse.ContainsKey( t ) )
                    {
                        val = Reverse[t];
                    }
                    else
                    {
                        LastError = t;
                        bytes = null;
                        return false;
                    }
                }

                // {Close} only encoded at end of entry
                if ((val == 0xFF) && (i < (s.Length - 1)))
                    val = -1;

                if ( val >= 0 )
                {
                    result.AddRange( IntToOneOrTwoOrThreeBytes( val ) );
                }
            }

            if ((result.Count == 0) || (!readTerminators.Contains(result[result.Count - 1])))
                result.Add( terminator );

            bytes = result.ToArray();
            return true;
        }

        /// <summary>
        /// Converts a FFTacText string into a FFT text byte array.
        /// </summary>
        public byte[] StringToByteArray( string s, byte terminator )
        {
            byte[] result;
            if ( TryStringToByteArray( s ?? string.Empty, terminator, out result ) )
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Validates the string with this charmap;
        /// </summary>
        public bool ValidateString( string s, byte terminator )
        {
            byte[] dummy;
            return TryStringToByteArray( s, terminator, out dummy );
        }


        #endregion Methods
    }

    /// <summary>
    /// A PSP character map.
    /// </summary>
    public class PSPCharMap : GenericCharMap
    {
        public PSPCharMap(IDictionary<int, string> source)
            : base(source)
        {
        }
    }

    /// <summary>
    /// A PSX character map.
    /// </summary>
    public class PSXCharMap : GenericCharMap
    {
        public PSXCharMap(IDictionary<int, string> source)
            : base(source)
        {
        }
    }

    public class NonDefaultCharMap : GenericCharMap
    {
        public NonDefaultCharMap(IDictionary<int, string> source)
            : base(source)
        {
        }
    }
}
