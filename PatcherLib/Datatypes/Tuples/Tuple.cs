#region (c)2009 Lokad - New BSD license

// Copyright (c) Lokad 2009 
// Company: http://www.lokad.com
// This code is released under the terms of the new BSD licence

#endregion

namespace Lokad
{
    /// <summary>
    /// Helper class that simplifies tuple inline generation
    /// </summary>
    ///  <example>
    /// Tuple.From("Mike",1,true)
    /// </example>
    public static class Tuple
    {
        /// <summary>
        /// Creates <see cref="Pair{T1,T2}"/> out of two arguments
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <returns>New tuple instance</returns>
        public static Pair<T1, T2> From<T1, T2>( T1 first, T2 second )
        {
            return new Pair<T1, T2>( first, second );
        }

        /// <summary>
        /// Creates <see cref="Triple{T1,T2,T3}"/> out of the three arguments
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="third">The third item.</param>
        /// <returns>New tuple instance</returns>
        public static Triple<T1, T2, T3> From<T1, T2, T3>( T1 first, T2 second, T3 third )
        {
            return new Triple<T1, T2, T3>( first, second, third );
        }

        /// <summary>
        /// Creates <see cref="Tuple{T1,T2,T3,T4}"/> out of four arguments
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <typeparam name="T4">The type of the fourth.</typeparam>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="third">The third item.</param>
        /// <param name="fourth">The fourth item.</param>
        /// <returns>New instance</returns>
        public static Quad<T1, T2, T3, T4> From<T1, T2, T3, T4>( T1 first, T2 second, T3 third, T4 fourth )
        {
            return new Quad<T1, T2, T3, T4>( first, second, third, fourth );
        }

        /// <summary>
        /// Creates <see cref="Tuple{T1,T2,T3,T4}"/> out of four arguments
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <typeparam name="T4">The type of the fourth item.</typeparam>
        /// <typeparam name="T5">The type of the fifth item.</typeparam>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="third">The third item.</param>
        /// <param name="fourth">The fourth item.</param>
        /// <param name="fifth">The fifth item.</param>
        /// <returns>New instance</returns>
        public static Tuple<T1, T2, T3, T4, T5> From<T1, T2, T3, T4, T5>( T1 first, T2 second, T3 third, T4 fourth, T5 fifth )
        {
            return new Tuple<T1, T2, T3, T4, T5>( first, second, third, fourth, fifth );
        }


        /// <summary>
        /// Creates <see cref="Tuple{T1,T2,T3,T4}"/> out of four arguments
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <typeparam name="T4">The type of the fourth item.</typeparam>
        /// <typeparam name="T5">The type of the fifth item.</typeparam>
        /// <typeparam name="T6">The type of the sixth item.</typeparam>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="third">The third item.</param>
        /// <param name="fourth">The fourth item.</param>
        /// <param name="fifth">The fifth item.</param>
        /// <param name="sixth">The sixth item.</param>
        /// <returns>New instance</returns>
        public static Tuple<T1, T2, T3, T4, T5, T6> From<T1, T2, T3, T4, T5, T6>( T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth )
        {
            return new Tuple<T1, T2, T3, T4, T5, T6>( first, second, third, fourth, fifth, sixth );
        }

        /// <summary>
        /// Creates <see cref="Tuple{T1,T2,T3,T4}"/> out of four arguments
        /// </summary>
        /// <typeparam name="T1">The type of the first item.</typeparam>
        /// <typeparam name="T2">The type of the second item.</typeparam>
        /// <typeparam name="T3">The type of the third item.</typeparam>
        /// <typeparam name="T4">The type of the fourth item.</typeparam>
        /// <typeparam name="T5">The type of the fifth item.</typeparam>
        /// <typeparam name="T6">The type of the sixth item.</typeparam>
        /// <typeparam name="T7">The type of the seventh item.</typeparam>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        /// <param name="third">The third item.</param>
        /// <param name="fourth">The fourth item.</param>
        /// <param name="fifth">The fifth item.</param>
        /// <param name="sixth">The sixth item.</param>
        /// <param name="seventh">The seventh item.</param>
        /// <returns>New instance</returns>
        public static Tuple<T1, T2, T3, T4, T5, T6, T7> From<T1, T2, T3, T4, T5, T6, T7>( T1 first, T2 second, T3 third, T4 fourth, T5 fifth, T6 sixth, T7 seventh )
        {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7>( first, second, third, fourth, fifth, sixth, seventh );
        }
    }
}