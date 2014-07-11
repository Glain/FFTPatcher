#region (c)2009 Lokad - New BSD license

// Copyright (c) Lokad 2009 
// Company: http://www.lokad.com
// This code is released under the terms of the new BSD licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Tuple class with 2 items
    /// </summary>
    /// <typeparam name="TKey">The type of the first item.</typeparam>
    /// <typeparam name="TValue">The type of the second item.</typeparam>
    [Serializable]
    public sealed class Pair<TKey, TValue> : Tuple<TKey, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pair{T1, T2}"/> class.
        /// </summary>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        public Pair( TKey first, TValue second )
            : base( first, second )
        {
        }

        /// <summary>
        /// Gets the key (or Item1).
        /// </summary>
        /// <value>The key.</value>
        public TKey Key
        {
            get { return Item1; }
        }

        /// <summary>
        /// Gets the value (or Item2).
        /// </summary>
        /// <value>The value.</value>
        public TValue Value
        {
            get { return Item2; }
        }
    }
}