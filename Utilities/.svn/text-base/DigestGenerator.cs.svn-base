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

using System.Xml;
using FFTPatcher.Datatypes;
using PatcherLib;

namespace FFTPatcher
{
    public static class DigestGenerator
    {
		#region Public Methods (3) 

        public static void WriteDigestEntry( XmlWriter writer, string name, object def, object cur )
        {
            WriteDigestEntry( writer, name, def, cur, "{0}" );
        }

        public static void WriteDigestEntry( XmlWriter writer, string name, object def, object cur, string formatString )
        {
            bool changed = !def.Equals( cur );
            if( changed )
            {
                writer.WriteStartElement( name );
                writer.WriteAttributeString( "changed", changed.ToString() );
                writer.WriteAttributeString( "default", string.Format( formatString, def ) );
                writer.WriteAttributeString( "value", string.Format( formatString, cur ) );
                writer.WriteEndElement();
            }
        }

        public static void WriteXmlDigest( ISupportDigest digest, XmlWriter writer, bool writeStartElement, bool writeEndElement )
        {
            bool changed = digest.HasChanged;
            if( changed )
            {
                if( writeStartElement )
                {
                    writer.WriteStartElement( digest.GetType().Name );
                }

                object defaultObject = ReflectionHelpers.GetFieldOrProperty<object>( digest, "Default" );
                writer.WriteAttributeString( "changed", changed.ToString() );
                foreach( string value in digest.DigestableProperties )
                {
                    object cur = ReflectionHelpers.GetFieldOrProperty<object>( digest, value );
                    if( cur != null )
                    {
                        if( (cur is ISupportDigest) && ReflectionHelpers.GetFieldOrProperty<object>( cur, "Default" ) != null )
                        {
                            ISupportDigest curDigest = cur as ISupportDigest;
                            if( curDigest.HasChanged )
                            {
                                writer.WriteStartElement( value );
                                WriteXmlDigest( cur as ISupportDigest, writer, false, true );
                            }
                        }
                        else
                        {
                            object def = ReflectionHelpers.GetFieldOrProperty<object>( defaultObject, value );
                            if( def != null )
                            {
                                string formatString = "{0}";
                                if( ReflectionHelpers.FieldOrPropertyHasAttribute<HexAttribute>( digest, value ) )
                                {
                                    formatString = "0x{0:X2}";
                                }
                                WriteDigestEntry( writer, value, def, cur, formatString );
                            }
                        }
                    }
                }

                if( writeEndElement )
                {
                    writer.WriteEndElement();
                }
            }
        }

		#endregion Public Methods 
    }
}
