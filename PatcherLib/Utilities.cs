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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace PatcherLib.Utilities
{
    public class PSXDescriptionAttribute : DescriptionAttribute
    {
		#region Constructors (1) 

        public PSXDescriptionAttribute( string description )
            : base( description )
        {
        }

		#endregion Constructors 
    }

    public class PSPDescriptionAttribute : DescriptionAttribute
    {
		#region Constructors (1) 

        public PSPDescriptionAttribute( string description )
            : base( description )
        {
        }

		#endregion Constructors 
    }
    
    /// <summary>
    /// Utilities
    /// </summary>
    public static class Utilities
    {
        public static readonly System.Text.RegularExpressions.Regex stripRegex = new System.Text.RegularExpressions.Regex(@"\s");
        public static readonly System.Text.RegularExpressions.Regex hexRegex = new System.Text.RegularExpressions.Regex(@"[^0-9a-fA-F]");
        public const string hexAlphabet = "0123456789ABCDEF";

		#region Methods


        public static IDictionary<TKey, TValue> DictionaryFromKVPs<TKey, TValue>( IEnumerable<KeyValuePair<TKey, TValue>> kvps )
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            foreach ( var kvp in kvps )
            {
                result[kvp.Key] = kvp.Value;
            }
            return result;
        }

        public static void SortList(IList list, Comparison<object> comparer)
        {
            QuickSort(list, 0, list.Count - 1, comparer);
        }

        public static void SortList<T>( IList<T> list ) where T : IComparable<T>
        {
            QuickSort( list, 0, list.Count - 1, ( a, b ) => a.CompareTo( b ) );
        }

        public static void SortList<T>( IList<T> list, Comparison<T> comparer )
        {
            QuickSort( list, 0, list.Count - 1, comparer );
        }

        public static void Swap<T>(IList<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        public static bool SafeSwap<T>(IList<T> list, int index1, int index2)
        {
            bool isSwapValid = (list != null) && (index1 >= 0) && (index1 < list.Count) && (index2 >= 0) && (index2 < list.Count);

            if (isSwapValid)
                Swap(list, index1, index2);
            
            return isSwapValid;
        }

        private static int QuickSortPivot<T>(IList<T> list, int beginning, int end, Comparison<T> comparer)
        {
            T pivot = list[beginning];
            int m = beginning;
            int n = end + 1;

            do
            {
                m += 1;
            } while (comparer(list[m], pivot) <= 0 && m < end);
            do
            {
                n -= 1;
            } while (comparer(list[n], pivot) > 0);

            while (m < n)
            {
                T temp = list[m];
                list[m] = list[n];
                list[n] = temp;

                do
                {
                    m += 1;
                } while (comparer(list[m], pivot) <= 0);
                do
                {
                    n -= 1;
                } while (comparer(list[n], pivot) > 0);
            }

            T temp2 = list[n];
            list[n] = list[beginning];
            list[beginning] = temp2;
            return n;
        }

        private static int QuickSortPivot(IList list, int beginning, int end, Comparison<object> comparer)
        {
            object pivot = list[beginning];
            int m = beginning;
            int n = end + 1;

            do
            {
                m += 1;
            } while (comparer(list[m], pivot) <= 0 && m < end);
            do
            {
                n -= 1;
            } while (comparer(list[n], pivot) > 0);

            while (m < n)
            {
                object temp = list[m];
                list[m] = list[n];
                list[n] = temp;

                do
                {
                    m += 1;
                } while (comparer(list[m], pivot) <= 0);
                do
                {
                    n -= 1;
                } while (comparer(list[n], pivot) > 0);
            }

            object temp2 = list[n];
            list[n] = list[beginning];
            list[beginning] = temp2;
            return n;
        }

        private static void SelectionSort<T>(IList<T> list, int beginning, int end, Comparison<T> comparer)
        {
            for (int i = beginning; i < end; i++)
            {
                int minj = i;
                T minx = list[i];
                for (int j = i + 1; j <= end; j++)
                {
                    if (comparer(list[j], minx) < 0)
                    {
                        minj = j;
                        minx = list[j];
                    }
                }

                list[minj] = list[i];
                list[i] = minx;
            }
        }

        private static void SelectionSort(IList list, int beginning, int end, Comparison<object> comparer)
        {
            for (int i = beginning; i < end; i++)
            {
                int minj = i;
                object minx = list[i];
                for (int j = i + 1; j <= end; j++)
                {
                    if (comparer(list[j], minx) < 0)
                    {
                        minj = j;
                        minx = list[j];
                    }
                }

                list[minj] = list[i];
                list[i] = minx;
            }
        }

        private static void QuickSort(IList list, int beginning, int end, Comparison<object> comparer)
        {
            if (end == beginning) return;
            if ((end - beginning) < 9)
            {
                SelectionSort(list, beginning, end, comparer);
            }
            else
            {
                int l = QuickSortPivot(list, beginning, end, comparer);
                QuickSort(list, beginning, l - 1, comparer);
                QuickSort(list, l + 1, end, comparer);
            }
        }

        private static void QuickSort<T>( IList<T> list, int beginning, int end, Comparison<T> comparer )
        {
            if( end == beginning ) return;
            if( (end - beginning) < 9 )
            {
                SelectionSort( list, beginning, end, comparer );
            }
            else
            {
                int l = QuickSortPivot( list, beginning, end, comparer );
                QuickSort( list, beginning, l - 1, comparer );
                QuickSort( list, l + 1, end, comparer );
            }
        }


        /// <summary>
        /// Builds a dictionary given a list of Key/Value pairs.
        /// </summary>
        /// <typeparam name="TKey">
        /// The type of Key. Every other value in <paramref name="keyValuePairs"/> must be of this type.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// The type of Value. Every other value in <paramref name="keyValuePairs"/> must be of this type.
        /// </typeparam>
        /// <param name="keyValuePairs">The key/value pairs to put in the dictionary. Must have an even length.</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> BuildDictionary<TKey, TValue>( IList keyValuePairs )
        {
            if( keyValuePairs.Count % 2 != 0 )
            {
                throw new ArgumentException( "List must have an even number of elements", "keyValuePairs" );
            }

            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>( keyValuePairs.Count / 2 );

            for( int i = 0; i < keyValuePairs.Count; i += 2 )
            {
                if( !(keyValuePairs[i] is TKey) )
                {
                    throw new ArgumentException( string.Format( "Element {0} was not of type {1}", i, typeof( TKey ).Name ), "keyValuePairs" );
                }
                if( !(keyValuePairs[i + 1] is TValue) )
                {
                    throw new ArgumentException( string.Format( "Element {0} was not of type {1}", i + 1, typeof( TValue ).Name ), "keyValuePairs" );
                }

                result.Add( (TKey)keyValuePairs[i], (TValue)keyValuePairs[i + 1] );
            }

            return result;
        }

        /// <summary>
        /// Determines if the code is currently running on Mono (vs. MS .NET)
        /// </summary>
        public static bool IsRunningOnMono()
        {
            return Type.GetType( "Mono.Runtime" ) != null;
        }

        /// <summary>
        /// Creates an array of booleans from a byte. Index 0 in the array is the least significant bit.
        /// </summary>
        public static bool[] BooleansFromByte( byte b )
        {
            bool[] result = new bool[8];
            for( int i = 0; i < 8; i++ )
            {
                result[i] = ((b >> i) & 0x01) > 0;
            }

            return result;
        }

        /// <summary>
        /// Creates an array of booleans from a byte. Index 0 in the array is the most significant bit.
        /// </summary>
        public static bool[] BooleansFromByteMSB( byte b )
        {
            bool[] result = new bool[8];
            for( int i = 0; i < 8; i++ )
            {
                result[i] = ((b >> (7 - i)) & 0x01) > 0;
            }

            return result;
        }

        /// <summary>
        /// Builds a byte from the passed booleans.
        /// </summary>
        public static byte ByteFromBooleans( bool msb, bool six, bool five, bool four, bool three, bool two, bool one, bool lsb )
        {
            bool[] flags = new bool[] { lsb, one, two, three, four, five, six, msb };
            byte result = 0;

            for( int i = 0; i < 8; i++ )
            {
                if( flags[i] )
                {
                    result |= (byte)(1 << i);
                }
            }

            return result;
        }

        /// <summary>
        /// Joins four bytes into a uint.
        /// </summary>
        public static UInt32 BytesToUInt32( IList<byte> bytes )
        {
            UInt32 result = 0;
            result += bytes[0];
            result += (UInt32)(bytes[1] << 8);
            result += (UInt32)(bytes[2] << 16);
            result += (UInt32)(bytes[3] << 24);

            return result;
        }

        /// <summary>
        /// Joins the two bytes into a ushort.
        /// </summary>
        public static UInt16 BytesToUShort( byte lsb, byte msb )
        {
            UInt16 result = 0;
            result += lsb;
            result += (UInt16)(msb << 8);
            return result;
        }

        /// <summary>
        /// Compares two arrays of the same type.
        /// </summary>
        public static bool CompareArrays<T>(IList<T> one, IList<T> two) where T : IComparable, IEquatable<T>
        {
            if (one.Count != two.Count)
                return false;
            for (int i = 0; i < one.Count; i++)
            {
                if( !one[i].Equals( two[i] ) )
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Copies an array of booleans into actual boolean instances.
        /// </summary>
        public static void CopyBoolArrayToBooleans( bool[] bools,
            ref bool msb,
            ref bool six,
            ref bool five,
            ref bool four,
            ref bool three,
            ref bool two,
            ref bool one,
            ref bool lsb )
        {
            lsb = bools[0];
            one = bools[1];
            two = bools[2];
            three = bools[3];
            four = bools[4];
            five = bools[5];
            six = bools[6];
            msb = bools[7];
        }

        /// <summary>
        /// Copies the bits of a byte into boolean instances.
        /// </summary>
        public static void CopyByteToBooleans( byte b,
            ref bool msb,
            ref bool six,
            ref bool five,
            ref bool four,
            ref bool three,
            ref bool two,
            ref bool one,
            ref bool lsb )
        {
            CopyBoolArrayToBooleans( BooleansFromByte( b ), ref msb, ref six, ref five, ref four, ref three, ref two, ref one, ref lsb );
        }

        /// <summary>
        /// Gets a nicely formatted Base64 representation of a list of bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public static string GetPrettyBase64( IList<byte> bytes )
        {
            StringBuilder sb = new StringBuilder( Convert.ToBase64String( bytes.ToArray(), Base64FormattingOptions.InsertLineBreaks ) );
            sb.Insert( 0, "\r\n" );
            sb.Replace( "\r\n", "\r\n    " );
            sb.Append( "\r\n  " );

            return sb.ToString();
        }

        /// <summary>
        /// Copies the numbers to the upper and lower nibbles of a byte.
        /// </summary>
        public static byte MoveToUpperAndLowerNibbles( int upper, int lower )
        {
            return (byte)(((upper & 0x0F) << 4) | (lower & 0x0F));
        }

        public static bool TryParseEnum<T>( string value, out T t ) where T : struct
        {
            t = default( T );
            if ( Enum.IsDefined( typeof( T ), value ) )
            {
                t = (T)Enum.Parse( typeof( T ), value, false );
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryParseInt(string str, out int value)
        {
            if (string.IsNullOrEmpty(str))
            {
                value = default( int );
                return false;
            }
            else
            {
                str = str.Trim();
                if (str.StartsWith("-0x"))
                {
                    bool isValid = int.TryParse(str.Substring(3), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out value);
                    if (isValid)
                        value = -value;
                    return isValid;
                }
                else if (str.StartsWith("0x"))
                    return int.TryParse(str.Substring(2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out value);
                else
                    return int.TryParse(str, out value);
            }
        }

        public static bool TryParseUint(string str, out uint value)
        {
            if (string.IsNullOrEmpty(str))
            {
                value = default( uint );
                return false;
            }
            else
            {
                str = str.Trim();
                if (str.StartsWith("0x"))
                    return uint.TryParse(str.Substring(2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out value);
                else
                    return uint.TryParse(str, out value);
            }
        }

        public static int ParseInt(string str)
        {
            int parseResult = default(int);
            TryParseInt(str, out parseResult);
            return parseResult;
        }

        public static string RemoveWhitespace(string text)
        {
            return stripRegex.Replace(text, string.Empty);
        }

        public static string[] SplitIntoLines(string text)
        {
            return System.Text.RegularExpressions.Regex.Split(text, "\r\n|\r|\n");
        }

        public static byte[] GetBytesFromHexString(string byteText, bool forceHex = false)
        {
            string strippedText = stripRegex.Replace(byteText, string.Empty);

            if (forceHex)
                strippedText = hexRegex.Replace(strippedText, string.Empty);

            int bytes = strippedText.Length / 2;
            byte[] result = new byte[bytes];

            for (int i = 0; i < bytes; i++)
            {
                result[i] = Byte.Parse(strippedText.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return result;
        }

        public static string ByteArrayToHexString(ICollection<byte> bytes)
        {
            StringBuilder result = new StringBuilder(bytes.Count * 2);

            foreach (byte b in bytes)
            {
                result.Append(hexAlphabet[(int)(b >> 4)]);
                result.Append(hexAlphabet[(int)(b & 0x0F)]);
            }

            return result.ToString();
        }

        public static uint[] GetUintArrayFromBytes(IEnumerable<byte> bytes, bool littleEndian = false)
        {
            List<uint> uintList = new List<uint>();

            uint uintValue = 0;
            int offset = 0;
            foreach (byte b in bytes)
            {
                int shiftAmount = littleEndian ? (offset * 8) : (24 - (offset * 8));
                uintValue |= (((uint)b) << shiftAmount);

                offset = (offset + 1) % 4;
                if (offset == 0)
                {
                    uintList.Add(uintValue);
                    uintValue = 0;
                }
            }

            return uintList.ToArray();
        }

        public static string GetByteString(IList<byte> bytes)
        {
            if (bytes == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            
            List<uint> fourByteSets = new List<uint>(GetUintArrayFromBytes(bytes, false));
            foreach (uint fourByteSet in fourByteSets)
            {
                sb.AppendFormat("{0}{1}", fourByteSet.ToString("X8"), Environment.NewLine);
            }

            int byteCount = bytes.Count;
            int remainingBytes = byteCount % 4;
            if (remainingBytes > 0)
            {
                int remainingBytesIndex = byteCount - remainingBytes;
                System.Text.StringBuilder sbRemainingBytes = new System.Text.StringBuilder(remainingBytes * 2);
                for (int index = remainingBytesIndex; index < byteCount; index++)
                {
                    sbRemainingBytes.Append(bytes[index].ToString("X2"));
                }
                sb.AppendFormat("{0}{1}", sbRemainingBytes.ToString(), Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion Methods 

    }
}
