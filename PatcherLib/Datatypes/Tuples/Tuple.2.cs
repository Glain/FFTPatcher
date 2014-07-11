#region (c)2009 Lokad - New BSD license

// Copyright (c) Lokad 2009 
// Company: http://www.lokad.com
// This code is released under the terms of the new BSD licence

#endregion

using System;
using System.Diagnostics;

namespace Lokad
{
    /// <summary>
    /// Tuple class with 2 items
    /// </summary>
    /// <typeparam name="T1">The type of the first item.</typeparam>
    /// <typeparam name="T2">The type of the second item.</typeparam>
    [Serializable]
    [DebuggerDisplay( "({Item1},{Item2})" )]
    public class Tuple<T1, T2> : IEquatable<Tuple<T1, T2>>
    {
        readonly T1 _item1;

        /// <summary>
        /// Gets Item1.
        /// </summary>
        /// <value>The item1.</value>
        public T1 Item1
        {
            get { return _item1; }
        }

        readonly T2 _item2;

        /// <summary>
        /// Gets Item2.
        /// </summary>
        /// <value>The item2.</value>
        public T2 Item2
        {
            get { return _item2; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pair{T1,T2}"/> class.
        /// </summary>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        public Tuple( T1 first, T2 second )
        {
            _item1 = first;
            _item2 = second;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="Pair{T1,T2}"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="Pair{T1,T2}"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="Pair{T1,T2}"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals( object obj )
        {
            if (ReferenceEquals( null, obj ))
                throw new NullReferenceException( "obj is null" );
            if (ReferenceEquals( this, obj )) return true;
            if (!(obj is Pair<T1, T2>)) return false;
            return Equals( (Tuple<T1, T2>)obj );
        }


        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="Pair{T1,T2}"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="Pair{T1,T2}"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format( "({0},{1})", Item1, Item2 );
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="obj" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="obj">
        /// An object to compare with this object.
        /// </param>
        public bool Equals( Tuple<T1, T2> obj )
        {
            if (ReferenceEquals( null, obj )) return false;
            if (ReferenceEquals( this, obj )) return true;
            return Equals( obj.Item1, Item1 ) && Equals( obj.Item2, Item2 );
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return SystemUtil.GetHashCode( Item1, Item2 );
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==( Tuple<T1, T2> left, Tuple<T1, T2> right )
        {
            return Equals( left, right );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=( Tuple<T1, T2> left, Tuple<T1, T2> right )
        {
            return !Equals( left, right );
        }
    }
}