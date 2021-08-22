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
using System.Diagnostics;
using PatcherLib.Utilities;
using System.Linq;

namespace PatcherLib.Datatypes
{
    public static partial class ExtensionMethods
    {
		#region Public Methods (4) 

        public static int IndexOf<T>( this IList<T> list, IList<T> find ) where T : IEquatable<T>
        {
            if( find.Count > list.Count )
            {
                return -1;
            }

            for( int i = 0; i + find.Count < list.Count; i++ )
            {
                if( list.Sub( i, i + find.Count - 1 ).Equals( find ) )
                {
                    return i;
                }
            }

            return -1;
        }

        public static IList<IList<T>> Split<T>(this IList<T> members, Set<T> values) where T : IEquatable<T>
        {
            if (values.Count == 1)
                return Split(members, values[0]);

            List<IList<T>> result = new List<IList<T>>();
            uint start = 0;
            uint stop = 0;
            for (int i = 0; i < members.Count; i++)
            {
                if (values.Contains(members[i]))
                {
                    stop = (uint)i;
                    result.Add(members.Sub(start, stop).AsReadOnly());
                    start = (uint)i + 1;
                }
            }

            if (!values.Contains(members[members.Count - 1]))
            {
                result.Add(members.Sub(start, (uint)(members.Count - 1)).AsReadOnly());
            }

            return result.AsReadOnly();
        }

        public static IList<IList<T>> Split<T>(this IList<T> members, T value) where T : IEquatable<T>
        {
            List<IList<T>> result = new List<IList<T>>();

            uint start = 0;
            uint stop = 0;

            for( int i = 0; i < members.Count; i++ )
            {
                if( members[i].Equals( value ) )
                {
                    stop = (uint)i;
                    result.Add( members.Sub( start, stop ).AsReadOnly() );
                    start = (uint)i + 1;
                }
            }

            if( !members[members.Count - 1].Equals( value ) )
            {
                result.Add( members.Sub( start, (uint)(members.Count - 1) ).AsReadOnly() );
            }

            return result.AsReadOnly();
        }

        [DebuggerStepThrough]
        public static IList<T> Sub<T>( this IList<T> list, int start )
        {
            return new SubArray<T>( list, (uint)start );
        }

        [DebuggerStepThrough]
        public static IList<T> Sub<T>( this IList<T> list, int start, int stop )
        {
            return new SubArray<T>( list, (uint)start, (uint)stop );
        }

        [DebuggerStepThrough]
        public static IList<T> SubLength<T>(this IList<T> list, int start, int length)
        {
            if (length > 0)
                return new SubArray<T>(list, (uint)start, (uint)(start + length - 1));
            else
                return new T[0];
        }

        [DebuggerStepThrough]
        public static IList<T> Sub<T>( this IList<T> list, uint start, uint stop )
        {
            return new SubArray<T>( list, start, stop );
        }

        [DebuggerStepThrough]
        public static IList<T> Sub<T>( this IList<T> list, uint start )
        {
            return new SubArray<T>( list, start );
        }

        [DebuggerStepThrough]
        public static IList<T> SubLength<T>(this IList<T> list, uint start, uint length)
        {
            if (length > 0)
                return new SubArray<T>(list, start, (start + length - 1));
            else
                return new T[0];
        }

		#endregion Public Methods 
    }
}

internal class CollectionDebugView<T>
{
    private IEnumerable<T> collection;

    [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
    public T[] Items
    {
        get { return collection.ToArray(); }
    }

    public CollectionDebugView( IEnumerable<T> collection )
    {
        if ( collection == null )
        {
            throw new ArgumentNullException( "collection" );
        }

        this.collection = collection;
    }
}

namespace PatcherLib.Datatypes
{
    using PatcherLib.Utilities;

    [DebuggerTypeProxy( typeof( CollectionDebugView<> ) )]
    [DebuggerDisplay( "Count = {Count}" )]
    [DebuggerStepThrough]
    public class SubArray<T> : ICollection<T>, IList<T>, IDisposable
    {
		#region Instance Variables (3) 

        private IList<T> baseArray;
        private uint start;
        private uint stop;

		#endregion Instance Variables 

		#region Public Properties (3) 

        public int Count
        {
            get { return (int)( stop - start + 1 ); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public T this[int index]
        {
            get
            {
                CheckIndex( index );
                return baseArray[index + (int)start];
            }
            set
            {
                CheckIndex( index );
                baseArray[index + (int)start] = value;
            }
        }

        public T this[uint index]
        {
            get
            {
                CheckIndex( (int)index );
                return baseArray[(int)(index + start)];
            }
            set
            {
                CheckIndex( (int)index );
                baseArray[(int)(index + start)] = value;
            }
        }

		#endregion Public Properties 

		#region Constructors (4) 

        public SubArray( IList<T> baseArray )
            : this( baseArray, 0 )
        {
        }

        public SubArray( IList<T> baseArray, uint start )
            : this( baseArray, start, (uint)(baseArray.Count - 1 ) )
        {
        }

        public SubArray( SubArray<T> baseArray, uint start, uint stop )
            : this( baseArray.baseArray, (uint)(start + baseArray.start), (uint)(stop + baseArray.start) )
        {
        }

        public SubArray( IList<T> baseArray, uint start, uint stop )
        {
            if( stop < start )
            {
                throw new ArgumentException();
            }
            if( stop > baseArray.Count - 1 )
            {
                throw new ArgumentException();
            }
            if( start > baseArray.Count - 1 )
            {
                throw new ArgumentException();
            }

            this.start = start;
            this.stop = stop;
            this.baseArray = baseArray;
        }

		#endregion Constructors 

		#region Public Methods (10) 

        public static bool operator !=( SubArray<T> left, SubArray<T> right )
        {
            return !(left == right);
        }

        public static bool operator ==( SubArray<T> left, SubArray<T> right )
        {
            return
                (object.ReferenceEquals( left, null ) && object.ReferenceEquals( right, null )) ||
                (!object.ReferenceEquals( left, null ) && !object.ReferenceEquals( right, null ) && left.Equals( right ));
        }

        public bool Contains( T item )
        {
            int index = baseArray.IndexOf( item );
            return ((index != -1) && (index >= start) && (index <= stop));
        }

        public void CopyTo( T[] array, int arrayIndex )
        {
            for( int i = 0; i < Count; i++ )
            {
                array[i + arrayIndex] = baseArray[(int)start + i];
            }
        }

        public void Dispose()
        {
            baseArray = null;
        }

        public override bool Equals( object obj )
        {
            IList<T> other = obj as IList<T>;

            if( (other == null) || (other.Count != Count) || !(this[0] is IEquatable<T>) )
            {
                return false;
            }

            for( int i = 0; i < Count; i++ )
            {
                IEquatable<T> mine = this[i] as IEquatable<T>;
                IEquatable<T> theirs = other[i] as IEquatable<T>;
                if( !mine.Equals( theirs ) )
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SubArrayEnumerator<T>( this );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int IndexOf( T item )
        {
            for( int i = 0; i < Count; i++ )
            {
                if( this[i].Equals( item ) )
                {
                    return i;
                }
            }

            return -1;
        }

        public T[] ToArray()
        {
            T[] result = new T[this.Count];
            for( int i = 0; i < this.Count; i++ )
            {
                result[i] = this[i];
            }
            return result;
        }

		#endregion Public Methods 

		#region Private Methods (2) 

        private void CheckIndex( int index )
        {
            if( index > (stop - start) )
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SubArrayEnumerator<T>( this );
        }

		#endregion Private Methods 

        void ICollection<T>.Add( T item )
        {
        }
        void ICollection<T>.Clear()
        {
        }
        void IList<T>.Insert( int index, T item )
        {
        }
        bool ICollection<T>.Remove( T item )
        {
            return false;
        }
        void IList<T>.RemoveAt( int index )
        {
            throw new NotImplementedException();
        }
    }

    [DebuggerStepThrough]
    public class SubArrayEnumerator<T> : IEnumerator<T>
    {
		#region Instance Variables (2) 

        private SubArray<T> array;
        private int currentIndex = -1;

		#endregion Instance Variables 

		#region Public Properties (1) 

        public T Current
        {
            get
            {
                if( (currentIndex != -1) && (currentIndex < array.Count) )
                {
                    return array[currentIndex];
                }

                return default( T );
            }
        }

		#endregion Public Properties 

		#region Private Properties (1) 

        object System.Collections.IEnumerator.Current
        {
            get
            {
                if( (currentIndex != -1) && (currentIndex < array.Count) )
                {
                    return array[currentIndex];
                }

                return null;
            }
        }

		#endregion Private Properties 

		#region Constructors (1) 

        public SubArrayEnumerator( SubArray<T> array )
        {
            this.array = array;
        }

		#endregion Constructors 

		#region Public Methods (3) 

        public void Dispose()
        {
            array = null;
        }

        public bool MoveNext()
        {
            currentIndex++;
            if( currentIndex >= array.Count )
            {
                currentIndex = array.Count;
                return false;
            }

            return true;
        }

        public void Reset()
        {
            currentIndex = 0;
        }

		#endregion Public Methods 
    }
}