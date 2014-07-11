#region (c)2009 Lokad - New BSD license

// Copyright (c) Lokad 2009 
// Company: http://www.lokad.com
// This code is released under the terms of the new BSD licence

#endregion

using System.Collections.Generic;

namespace Lokad
{
    /// <summary>
    /// Helper extensions for tuples
    /// </summary>
    public static class ExtendTuple
    {
        /// <summary>
        /// Appends the specified <paramref name="item"/> to the <paramref name="tuple"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <param name="tuple">The tuple to append to.</param>
        /// <param name="item">The item to append.</param>
        /// <returns>New tuple instance</returns>
        public static Triple<T1, T2, T3> Append<T1, T2, T3>( this Tuple<T1, T2> tuple, T3 item )
        {
            return Tuple.From( tuple.Item1, tuple.Item2, item );
        }

        /// <summary>
        /// Appends the specified <paramref name="item"/> to the <paramref name="tuple"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <typeparam name="T4">The type of the fourth item.</typeparam>
        /// <param name="tuple">The tuple to append to.</param>
        /// <param name="item">The item to append.</param>
        /// <returns>New tuple instance</returns>
        public static Quad<T1, T2, T3, T4> Append<T1, T2, T3, T4>( this Tuple<T1, T2, T3> tuple, T4 item )
        {
            return Tuple.From( tuple.Item1, tuple.Item2, tuple.Item3, item );
        }

        /// <summary>
        /// Appends the specified <paramref name="item"/> to the <paramref name="tuple"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <typeparam name="T4">The type of the fourth item.</typeparam>
        /// <typeparam name="T5">The type of the fifth item.</typeparam>
        /// <param name="tuple">The tuple to append to.</param>
        /// <param name="item">The item to append.</param>
        /// <returns>New tuple instance</returns>
        public static Tuple<T1, T2, T3, T4, T5> Append<T1, T2, T3, T4, T5>( this Tuple<T1, T2, T3, T4> tuple, T5 item )
        {
            return Tuple.From( tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, item );
        }

        /// <summary>
        /// Appends the specified <paramref name="item"/> to the <paramref name="tuple"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <typeparam name="T4">The type of the fourth item.</typeparam>
        /// <typeparam name="T5">The type of the fifth item.</typeparam>
        /// <typeparam name="T6">The type of the sixth item.</typeparam>
        /// <param name="tuple">The tuple to append to.</param>
        /// <param name="item">The item to append.</param>
        /// <returns>New tuple instance</returns>
        public static Tuple<T1, T2, T3, T4, T5, T6> Append<T1, T2, T3, T4, T5, T6>( this Tuple<T1, T2, T3, T4, T5> tuple, T6 item )
        {
            return Tuple.From( tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, item );
        }

        /// <summary>
        /// Appends the specified <paramref name="item"/> to the <paramref name="tuple"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <typeparam name="T4">The type of the fourth item.</typeparam>
        /// <typeparam name="T5">The type of the fifth item.</typeparam>
        /// <typeparam name="T6">The type of the sixth item.</typeparam>
        /// <typeparam name="T7">The type of the seventh item.</typeparam>
        /// <param name="tuple">The tuple to append to.</param>
        /// <param name="item">The item to append.</param>
        /// <returns>New tuple instance</returns>
        public static Tuple<T1, T2, T3, T4, T5, T6, T7> Append<T1, T2, T3, T4, T5, T6, T7>( this Tuple<T1, T2, T3, T4, T5, T6> tuple, T7 item )
        {
            return Tuple.From( tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, item );
        }

        /// <summary> Shortcut to create and add tuple to the collection </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <param name="collection">The collection to add to.</param>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        public static void AddTuple<T1, T2>( this ICollection<Tuple<T1, T2>> collection, T1 first, T2 second )
        {
            collection.Add( Tuple.From( first, second ) );
        }

        /// <summary> Shortcut to create and add tuple to the collection </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <param name="collection">The collection to add to.</param>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        public static void AddTuple<T1, T2>( this ICollection<Pair<T1, T2>> collection, T1 first, T2 second )
        {
            collection.Add( Tuple.From( first, second ) );
        }

        /// <summary> Shortcut to create and add tuple to the collection </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <param name="collection">The collection to add to.</param>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="third">The third item.</param>
        public static void AddTuple<T1, T2, T3>( this ICollection<Tuple<T1, T2, T3>> collection, T1 first, T2 second, T3 third )
        {
            collection.Add( Tuple.From( first, second, third ) );
        }

        /// <summary> Shortcut to create and add tuple to the collection </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <typeparam name="T4">The type of the fourth item.</typeparam>
        /// <param name="collection">The collection to add to.</param>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="third">The third item.</param>
        /// <param name="fourth">The fourth item.</param>
        public static void AddTuple<T1, T2, T3, T4>( this ICollection<Tuple<T1, T2, T3, T4>> collection, T1 first, T2 second,
            T3 third, T4 fourth )
        {
            collection.Add( Tuple.From( first, second, third, fourth ) );
        }

        /// <summary> Shortcut to create and add tuple to the collection </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <typeparam name="T4">The type of the fourth item.</typeparam>
        /// <typeparam name="T5">The type of the fifth item.</typeparam>
        /// <param name="collection">The collection to add to.</param>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="third">The third item.</param>
        /// <param name="fourth">The fourth item.</param>
        /// <param name="fifth">The fifth item.</param>
        public static void AddTuple<T1, T2, T3, T4, T5>( this ICollection<Tuple<T1, T2, T3, T4, T5>> collection, T1 first, T2 second,
            T3 third, T4 fourth, T5 fifth )
        {
            collection.Add( Tuple.From( first, second, third, fourth, fifth ) );
        }
    }
}