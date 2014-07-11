using System;
using System.Collections.Generic;

namespace PatcherLib.Datatypes
{
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> innerDict;

        public bool ThrowOnWrite
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="innerDict">The dictionary to wrap as a read-only dictionary</param>
        public ReadOnlyDictionary( IDictionary<TKey, TValue> innerDict )
            : this( innerDict, false )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyDictionary&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="innerDict">The dictionary to wrap as a read-only dictionary</param>
        /// <param name="throwOnWrite">Whether or not to throw <see cref="NotSupportedException"/> when adding or modifying items in the dictionary.</param>
        public ReadOnlyDictionary( IDictionary<TKey, TValue> innerDict, bool throwOnWrite )
        {
            this.innerDict = innerDict;
            ThrowOnWrite = throwOnWrite;
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.</exception>
        /// <exception cref="T:System.NotSupportedException"><see cref="ThrowOnWrite"/> is [b]true[/b]</exception>
        public void Add( TKey key, TValue value )
        {
            if( ThrowOnWrite )
            {
                throw new NotSupportedException( "Attempt to modify a read-only collection" );
            }
        }

        public bool ContainsKey( TKey key )
        {
            return innerDict.ContainsKey( key );
        }

        public ICollection<TKey> Keys
        {
            get { return innerDict.Keys; }
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="key"/> is null.</exception>
        /// <exception cref="T:System.NotSupportedException"><see cref="ThrowOnWrite"/> is [b]true[/b]</exception>
        public bool Remove( TKey key )
        {
            if( ThrowOnWrite )
            {
                throw new NotSupportedException( "Attempt to modify a read-only collection" );
            }
            return false;
        }

        public bool TryGetValue( TKey key, out TValue value )
        {
            return innerDict.TryGetValue( key, out value );
        }

        public ICollection<TValue> Values
        {
            get { return innerDict.Values; }
        }

        /// <summary>
        /// Gets or sets the <see cref="TValue"/> with the specified key.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException"><see cref="ThrowOnWrite"/> is [b]true[/b]</exception>
        public TValue this[TKey key]
        {
            get
            {
                return innerDict[key];
            }
            set
            {
                if( ThrowOnWrite )
                {
                    throw new NotSupportedException( "Attempt to modify a read-only collection" );
                }
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException"><see cref="ThrowOnWrite"/> is [b]true[/b]</exception>
        public void Add( KeyValuePair<TKey, TValue> item )
        {
            if( ThrowOnWrite )
            {
                throw new NotSupportedException( "Attempt to modify a read-only collection" );
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException"><see cref="ThrowOnWrite"/> is [b]true[/b]</exception>
        public void Clear()
        {
            if( ThrowOnWrite )
            {
                throw new NotSupportedException( "Attempt to modify a read-only collection" );
            }
        }

        public bool Contains( KeyValuePair<TKey, TValue> item )
        {
            return innerDict.Contains( item );
        }

        public void CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
        {
            innerDict.CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return innerDict.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"><see cref="ThrowOnWrite"/> is [b]true[/b]</exception>
        public bool Remove( KeyValuePair<TKey, TValue> item )
        {
            if( ThrowOnWrite )
            {
                throw new NotSupportedException( "Attempt to modify a read-only collection" );
            }
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return innerDict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)innerDict).GetEnumerator();
        }
    }
}
