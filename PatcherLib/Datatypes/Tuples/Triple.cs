#region (c)2009 Lokad - New BSD license

// Copyright (c) Lokad 2009 
// Company: http://www.lokad.com
// This code is released under the terms of the new BSD licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Tuple class with 3 items
    /// </summary>
    /// <typeparam name="T1">The type of the first item.</typeparam>
    /// <typeparam name="T2">The type of the second item.</typeparam>
    /// <typeparam name="T3">The type of the third item.</typeparam>
    [Serializable]
    public sealed class Triple<T1, T2, T3> : Tuple<T1, T2, T3>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Triple{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="third">The third item.</param>
        public Triple( T1 first, T2 second, T3 third )
            : base( first, second, third )
        {
        }
    }
}