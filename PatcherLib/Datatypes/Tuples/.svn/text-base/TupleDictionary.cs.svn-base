using System;
using System.Collections.Generic;
using System.Text;
using Lokad;

namespace PatcherLib.Datatypes
{
    public class TupleDictionary<TKey1, TKey2, TValue> : IDictionary<Tuple<TKey1, TKey2>, TValue>
    {
        Dictionary<Tuple<TKey1, TKey2>, TValue> _innerDict;

        public TupleDictionary()
        {
            _innerDict = new Dictionary<Tuple<TKey1, TKey2>, TValue>();
            IsReadOnly = false;
        }

        public TupleDictionary( IDictionary<Tuple<TKey1, TKey2>, TValue> baseDict )
            : this( baseDict, false, false )
        {
        }

        public TupleDictionary(IDictionary<Tuple<TKey1, TKey2>, TValue> baseDict, bool readOnly, bool getSetOnly )
        {
            _innerDict = new Dictionary<Tuple<TKey1, TKey2>, TValue>( baseDict );
            IsReadOnly = readOnly;
            IsGetSetOnly = getSetOnly;
        }

        public TValue this[TKey1 key1, TKey2 key2]
        {
            get { return this[Tuple.From( key1, key2 )]; }
            set 
            {
                this[Tuple.From( key1, key2 )] = value; 
            }
        }

        public bool ContainsKey( TKey1 key1, TKey2 key2 )
        {
            return ContainsKey( Tuple.From( key1, key2 ) );
        }

        public void Add( TKey1 key1, TKey2 key2, TValue value )
        {
            Add( Tuple.From( key1, key2 ), value );
        }

        public bool Remove( TKey1 key1, TKey2 key2 )
        {
            return Remove( Tuple.From( key1, key2 ) );
        }

        public bool TryGetValue( TKey1 key1, TKey2 key2, out TValue value )
        {
            return TryGetValue( Tuple.From( key1, key2 ), out value );
        }



        public void Add( Tuple<TKey1, TKey2> key, TValue value )
        {
            if (IsReadOnly || IsGetSetOnly)
                throw new InvalidOperationException( "Dictionary is readonly" );
            _innerDict[key] = value;
        }

        public bool ContainsKey( Tuple<TKey1, TKey2> key )
        {
            return _innerDict.ContainsKey( key );
        }

        public ICollection<Tuple<TKey1, TKey2>> Keys
        {
            get { return _innerDict.Keys; }
        }

        public bool Remove( Tuple<TKey1, TKey2> key )
        {
            if (IsReadOnly || IsGetSetOnly)
                throw new InvalidOperationException( "Dictionary is readonly" );
            return _innerDict.Remove( key );
        }

        public bool TryGetValue( Tuple<TKey1, TKey2> key, out TValue value )
        {
            return _innerDict.TryGetValue( key, out value );
        }

        public ICollection<TValue> Values
        {
            get { return _innerDict.Values; }
        }

        public TValue this[Tuple<TKey1, TKey2> key]
        {
            get
            {
                return _innerDict[key];
            }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException( "Dictionary is readonly" );
                _innerDict[key] = value;
            }
        }

        public void Add( KeyValuePair<Tuple<TKey1, TKey2>, TValue> item )
        {
            if (IsReadOnly || IsGetSetOnly)
                throw new InvalidOperationException( "Dictionary is readonly" );
            _innerDict.Add( item.Key, item.Value );
        }

        public void Clear()
        {
            if (IsReadOnly || IsGetSetOnly)
                throw new InvalidOperationException( "Dictionary is readonly" );
            _innerDict.Clear();
        }

        public bool Contains( KeyValuePair<Tuple<TKey1, TKey2>, TValue> item )
        {
            return ((IDictionary<Tuple<TKey1,TKey2>,TValue>)_innerDict).Contains( item );
        }

        public void CopyTo( KeyValuePair<Tuple<TKey1, TKey2>, TValue>[] array, int arrayIndex )
        {
            ((IDictionary<Tuple<TKey1, TKey2>, TValue>)_innerDict).CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return _innerDict.Count; }
        }

        public bool IsReadOnly
        {
            get;
            private set;
        }

        public bool IsGetSetOnly { get; private set; }

        public bool Remove( KeyValuePair<Tuple<TKey1, TKey2>, TValue> item )
        {
            if (IsReadOnly || IsGetSetOnly)
                throw new InvalidOperationException( "Dictionary is readonly" );
            return ((IDictionary<Tuple<TKey1, TKey2>, TValue>)_innerDict).Remove( item );
        }

        public IEnumerator<KeyValuePair<Tuple<TKey1, TKey2>, TValue>> GetEnumerator()
        {
            return ((IDictionary<Tuple<TKey1, TKey2>, TValue>)_innerDict).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_innerDict).GetEnumerator();
        }
    }
}
