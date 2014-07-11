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
using System.Reflection;

namespace PatcherLib
{
    /// <summary>
    /// Utilities to help with Reflection.
    /// </summary>
    public static class ReflectionHelpers
    {
		#region Public Methods (7) 

        /// <summary>
        /// Determines if a field or property in an object has a particular attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="target">The object that owns the field or property.</param>
        /// <param name="propertyName">Name of the field or property.</param>
        /// <returns></returns>
        public static bool FieldOrPropertyHasAttribute<TAttribute>( object target, string propertyName ) where TAttribute : Attribute
        {
            MemberInfo mi = target.GetType().GetField( propertyName );
            if( mi == null )
            {
                mi = target.GetType().GetProperty( propertyName );
            }

            if( mi != null )
            {
                object[] attrs = mi.GetCustomAttributes( typeof( TAttribute ), true );
                return attrs != null && attrs.Length > 0;
            }
            else
            {
                throw new ArgumentException( "propertyName" );
            }
        }

        /// <summary>
        /// Gets a field or property from an object.
        /// </summary>
        /// <typeparam name="T">Thet type of the field or property.</typeparam>
        public static T GetFieldOrProperty<T>( object target, string name, bool throwOnError )
        {
            PropertyInfo pi = target.GetType().GetProperty( name );
            FieldInfo fi = target.GetType().GetField( name );

            if( pi != null )
            {
                return (T)pi.GetValue( target, null );
            }
            else if( fi != null )
            {
                return (T)fi.GetValue( target );
            }
            else if( throwOnError )
            {
                throw new ArgumentException();
            }
            else
            {
                return default( T );
            }
        }

        public static T GetPublicStaticFieldOrProperty<T>( Type sourceType, string name, bool throwOnError )
        {
            PropertyInfo pi = sourceType.GetProperty( name, BindingFlags.Static | BindingFlags.Public );
            FieldInfo fi = sourceType.GetField( name, BindingFlags.Static | BindingFlags.Public );
            if ( pi != null )
            {
                return (T)pi.GetValue( null, null );
            }
            else if ( fi != null )
            {
                return (T)fi.GetValue( null );
            }
            else if ( throwOnError )
            {
                throw new ArgumentException();
            }
            else
            {
                return default( T );
            }

        }

        /// <summary>
        /// Gets a field or property from an object.
        /// </summary>
        /// <typeparam name="T">Thet type of the field or property.</typeparam>
        public static T GetFieldOrProperty<T>( object target, string name )
        {
            return GetFieldOrProperty<T>( target, name, true );
        }

        /// <summary>
        /// Gets an array of fields or properties from an object.
        /// </summary>
        /// <typeparam name="T">The type of the fields or properties.</typeparam>
        public static T[] GetFieldsOrProperties<T>( object target, string[] names )
        {
            T[] result = new T[names.Length];
            for( int i = 0; i < names.Length; i++ )
            {
                result[i] = GetFieldOrProperty<T>( target, names[i] );
            }

            return result;
        }

        /// <summary>
        /// Gets a boolean from an object.
        /// </summary>
        public static bool GetFlag( object o, string name )
        {
            return GetFieldOrProperty<bool>( o, name );
        }

        /// <summary>
        /// Sets a field or property on an object.
        /// </summary>
        public static void SetFieldOrProperty( object target, string name, object newValue )
        {
            PropertyInfo pi = target.GetType().GetProperty( name );
            FieldInfo fi = target.GetType().GetField( name );
            if( pi != null )
            {
                pi.SetValue( target, newValue, null );
            }
            else if( fi != null )
            {
                fi.SetValue( target, newValue );
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Sets a boolean on an object.
        /// </summary>
        public static void SetFlag( object o, string name, bool newValue )
        {
            SetFieldOrProperty( o, name, newValue );
        }

		#endregion Public Methods 
    }
}
