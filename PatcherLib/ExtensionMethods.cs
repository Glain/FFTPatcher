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
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Text;
using PatcherLib.Datatypes;

namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute
    {

    }
}

namespace PatcherLib.Utilities
{
    /// <summary>
    /// Extension methods for various types.
    /// </summary>
    public static partial class ExtensionMethods
    {

        #region Methods (14)
        #region Xmlwriter
        public static void WriteValueElement( this XmlWriter writer, string name, UInt16 value )
        {
            writer.WriteStartElement( name );
            writer.WriteValue( value );
            writer.WriteEndElement();
        }
        public static void WriteValueElement( this XmlWriter writer, string name, byte value )
        {
            writer.WriteStartElement( name );
            writer.WriteValue( value );
            writer.WriteEndElement();
        }
        public static void WriteValueElement( this XmlWriter writer, string name, Enum value )
        {
            writer.WriteStartElement( name );
            writer.WriteValue( value.ToString() );
            writer.WriteEndElement();
        }
        public static void WriteValueElement( this XmlWriter writer, string name, bool value )
        {
            writer.WriteStartElement( name );
            writer.WriteValue( value );
            writer.WriteEndElement();
        }
        #endregion

        public static XmlDocument ToXmlDocument( this string s )
        {
            XmlDocument result = new XmlDocument();
            result.LoadXml( s );
            return result;
        }

        #region Ilist / IEnumerable
        public static int IndexOf<T>( this IList<T> list, T item )
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals( item )) return i;
            }
            return -1;
        }

        public static T[] ToArray<T>( this IEnumerable<T> collection )
        {
            return collection.ToList().ToArray();
        }

        public static IList<T> ToList<T>( this IEnumerable<T> collection )
        {
            if (collection is IList<T>)
                return new ReadOnlyCollection<T>( collection as IList<T> );

            return new List<T>( collection ).AsReadOnly();
        }
        public static IList<T> SetAll<T>(this IList<T> list, T value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = value;
            }
            return list;
        }

        
        public static bool TrueForAll<T>( this IList<T> list, Predicate<T> condition )
        {
            if (list == null) throw new ArgumentNullException( "list" );
            if (condition == null) throw new ArgumentNullException( "condition" );
            for (int i = 0; i < list.Count; i++)
            {
                if (!condition( list[i] )) return false;
            }
            return true;
        }
        public static bool TrueForAll<T>( this IEnumerable<T> list, Predicate<T> condition )
        {
            if (list == null) throw new ArgumentNullException( "list" );
            if (condition == null) throw new ArgumentNullException( "condition" );
            foreach (T item in list)
            {
                if (!condition( item )) return false;
            }
            return true;
        }
        #endregion

        public static void Copy<T>( this IList<T> sourceList, int sourceIndex, IList<T> destinationList, int destinationIndex, int length )
        {
            if (sourceList == null) throw new ArgumentNullException( "sourceList" );
            if (sourceList.Count <= sourceIndex) throw new ArgumentOutOfRangeException( "sourceIndex" );
            if (sourceList.Count <= sourceIndex + length) throw new ArgumentOutOfRangeException( "length" );
            if (destinationList == null) throw new ArgumentNullException( "destinationList" );
            if (destinationList.Count <= destinationIndex) throw new ArgumentOutOfRangeException( "destinationIndex" );
            if (destinationList.Count <= destinationIndex + length) throw new ArgumentOutOfRangeException( "length" );
            if (destinationList.IsReadOnly) throw new InvalidOperationException( "destinationList is readonly" );
            for (int i = 0; i < length; i++)
            {
                destinationList[i + destinationIndex] = sourceList[i + sourceIndex];
            }
        }

        
        [System.Diagnostics.DebuggerStepThrough]
        public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>( this IDictionary<TKey, TValue> dict )
        {
            if (dict.IsReadOnly) return dict;
            else return new ReadOnlyDictionary<TKey, TValue>( dict, true );
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static TupleDictionary<TKey1, TKey2, TValue> AsReadOnly<TKey1, TKey2, TValue>( this IDictionary<Lokad.Tuple<TKey1, TKey2>, TValue> dict )
        {
            return new TupleDictionary<TKey1, TKey2, TValue>( dict, true, true );
        }


        [System.Diagnostics.DebuggerStepThrough]
        public static ReadOnlyCollection<T> AsReadOnly<T>( this IList<T> list )
        {
            if (list is ReadOnlyCollection<T>) return list as ReadOnlyCollection<T>;
            else return new ReadOnlyCollection<T>( list );
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void RemoveAll<T, U>( this IDictionary<T, U> dict, Predicate<T> criteria )
        {
            Set<T> toRemove = new Set<T>();
            foreach (T key in dict.Keys)
            {
                if (criteria( key ))
                {
                    toRemove.Add( key );
                }
            }
            foreach (T key in toRemove.GetElements())
            {
                dict.Remove( key );
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void Sort<T>( this IList<T> list ) where T : IComparable<T>
        {
            Utilities.SortList( list );
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void Sort<T>( this IList<T> list, Comparison<T> comparer )
        {
            Utilities.SortList( list, comparer );
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static IList<T> Join<T>( this IEnumerable<IEnumerable<T>> lists )
        {
            List<T> result = new List<T>();
            lists.ForEach( l => result.AddRange( l ) );
            return result;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static IList<T> Join<T>( this IList<IList<T>> lists )
        {
            List<T> result = new List<T>();
            lists.ForEach( l => result.AddRange( l ) );
            return result;
        }

        /// <summary>
        /// Sums the items in the list.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static int Sum( this IList<int> items )
        {
            int sum = 0;
            foreach (int i in items)
            {
                sum += i;
            }
            return sum;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static bool Exists<T>( this IList<T> list, Predicate<T> match )
        {
            foreach (T t in list)
            {
                if (match( t ))
                {
                    return true;
                }
            }

            return false;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void CopyTo<T>( this IList<T> list, IList<T> destination, int destinationIndex )
        {
            if (destination.Count - destinationIndex < list.Count)
            {
                throw new InvalidOperationException( "source list is larger than destination" );
            }

            for (int i = 0; i < list.Count; i++)
            {
                destination[i + destinationIndex] = list[i];
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void CopyTo<TKey, TValue>( this Dictionary<TKey, TValue>.ValueCollection list, IList<TValue> destination, int destinationIndex )
        {
            if (destination.Count - destinationIndex < list.Count)
            {
                throw new InvalidOperationException( "source list is larger than destination" );
            }

            int count = 0;
            foreach (TValue v in list)
            {
                destination[destinationIndex + count++] = v;
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static IList<T> FindAll<T>( this IList<T> list, Predicate<T> match )
        {
            List<T> result = new List<T>( list.Count );
            for (int i = 0; i < list.Count; i++)
            {
                if (match( list[i] ))
                    result.Add( list[i] );
            }
            return result.AsReadOnly();
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static IList<T> FindAll<T>( this ICollection<T> list, Predicate<T> match )
        {
            List<T> result = new List<T>( list.Count );
            foreach (var item in list)
            {
                if (match( item ))
                    result.Add( item );
            }

            return result.AsReadOnly();
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void InitializeElements<T>( this IList<T> list )
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = default( T );
            }
        }

        /// <summary>
        /// Returns the location of every item in the list that is equal to a given item.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="item">The item to match.</param>
        [System.Diagnostics.DebuggerStepThrough]
        public static IList<int> IndexOfEvery<T>( this IList<T> list, T item ) where T : IEquatable<T>
        {
            List<int> result = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals( item ))
                {
                    result.Add( i );
                }
            }

            return result.AsReadOnly();
        }

        /// <summary>
        /// Performs the specified action on each element of the <see cref="System.Collections.Generic.IList&lt;T&gt;"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">
        /// The <see cref="System.Action&lt;T&gt;"/> delegate to perform on each element of the <see cref="System.Collections.Generic.List&lt;T&gt;"/>.
        /// </param>
        /// <exception cref="System.ArgumentNullException"><paramref name="action"/> is null</exception>
        [System.Diagnostics.DebuggerStepThrough]
        public static void ForEach<T>( this IList<T> list, Action<T> action )
        {
            if (action == null)
            {
                throw new ArgumentNullException( "action" );
            }

            int count = list.Count;
            for (int i = 0; i < count; i++) action( list[i] );
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void ForEach<T>( this IEnumerable<T> list, Action<T> action )
        {
            if (action == null)
            {
                throw new ArgumentNullException( "action" );
            }

            foreach (T item in list)
            {
                action( item );
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static string Join( this IList<string> strings, string joiner )
        {
            StringBuilder result = new StringBuilder();
            strings.ForEach( s => result.Append( s + joiner ) );
            return result.ToString();
        }

        /// <summary>
        /// Finds the specified item in the list.
        /// </summary>
        /// <param name="match">The <see cref="System.Predicate&lt;T&gt;"/> delegate that defines the conditions of the element to search for.</param>
        /// <exception cref="System.ArgumentNullException"/><paramref name="match"/> is null</exception>.
        [System.Diagnostics.DebuggerStepThrough]
        public static T Find<T>( this IList<T> list, Predicate<T> match ) where T : class
        {
            foreach (T item in list)
            {
                if (match( item ))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds a collection of values to the list.
        /// </summary>
        /// <param name="items">The items to add</param>
        [System.Diagnostics.DebuggerStepThrough]
        public static void AddRange<T>( this IList<T> list, IEnumerable<T> items )
        {
            foreach (T item in items)
            {
                list.Add( item );
            }
        }

        /// <summary>
        /// Adds <paramref name="lines"/> to the StringBuilder.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static void AppendLines( this StringBuilder sb, IEnumerable<string> lines )
        {
            foreach (string line in lines)
            {
                sb.Append( line + "\n" );
            }
        }

        /// <summary>
        /// Gets the lower nibble of this byte.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static byte GetLowerNibble( this byte b )
        {
            return (byte)(b & 0x0F);
        }

        /// <summary>
        /// Gets the upper nibble of this byte.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static byte GetUpperNibble( this byte b )
        {
            return (byte)((b & 0xF0) >> 4);
        }

        /// <summary>
        /// Finds the last index of the specified value in this list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The index of the item found. -1 if not found.</returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static int LastIndexOf<T>( this IList<T> list, T value ) where T : IEquatable<T>
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Equals( value ))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Converts this list into an array.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static T[] ToArray<T>( this IList<T> list )
        {
            T[] result = new T[list.Count];
            if (list is T[])
            {
                T[] arr = list as T[];
                arr.CopyTo( result, 0 );
            }
            else
            {
                list.CopyTo( result, 0 );
            }

            return result;
        }

        /// <summary>
        /// Converts this string to an array of bytes.
        /// Each character in the string should be a single byte ASCII character.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static byte[] ToByteArray( this string s )
        {
            byte[] result = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                result[i] = (byte)s[i];
            }

            return result;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static byte[] ToBytes( this UInt16 value )
        {
            byte[] result = new byte[2];
            result[0] = (byte)(value & 0xFF);
            result[1] = (byte)((value >> 8) & 0xFF);
            return result;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static byte[] ToBytes( this UInt32 value )
        {
            byte[] result = new byte[4];
            result[0] = (byte)(value & 0xFF);
            result[1] = (byte)((value >> 8) & 0xFF);
            result[2] = (byte)((value >> 16) & 0xFF);
            result[3] = (byte)((value >> 24) & 0xFF);
            return result;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static byte[] ToBytes( this long value )
        {
            byte[] result = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                result[i] = (byte)((value >> (i * 8)) & 0xFF);
            }
            return result;
        }

        /// <summary>
        /// Converts this array of bytes into a UInt32.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static UInt32 ToUInt32( this IList<byte> bytes )
        {
            UInt32 result = 0;
            result += bytes[0];
            result += (UInt32)(bytes[1] << 8);
            result += (UInt32)(bytes[2] << 16);
            result += (UInt32)(bytes[3] << 24);

            return result;
        }

        /// <summary>
        /// Converts this to a string.
        /// </summary>
        [System.Diagnostics.DebuggerStepThrough]
        public static string ToUTF8String( this IList<byte> bytes )
        {
            if ((bytes[0] == 0xef) && (bytes[1] == 0xbb) && (bytes[2] == 0xbf))
            {
                return Encoding.UTF8.GetString( bytes.ToArray(), 3, bytes.Count - 3 );
            }
            else
            {
                return Encoding.UTF8.GetString( bytes.ToArray() );
            }
        }

        /// <summary>
        /// Writes an array to the specified position in the stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="array">The array to write.</param>
        /// <param name="position">The position to start writing.</param>
        [System.Diagnostics.DebuggerStepThrough]
        public static void WriteArrayToPosition( this Stream stream, byte[] array, long position )
        {
            stream.Seek( position, SeekOrigin.Begin );
            stream.Write( array, 0, array.Length );
        }

        /// <summary>
        /// Writes an array to the specified positions in the stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="array">The array to write.</param>
        /// <param name="positions">The positions to start writing.</param>
        [System.Diagnostics.DebuggerStepThrough]
        public static void WriteArrayToPositions( this Stream stream, byte[] array, params long[] positions )
        {
            foreach (long position in positions)
            {
                stream.WriteArrayToPosition( array, position );
            }
        }

        public static string[] ToLines(this string wholestring)
        {
            string[] result = wholestring.Replace("\r", "").Split('\n');
            return result;
        }
  
        #endregion Methods
    }
}
