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
using PatcherLib;
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    public enum Element
    {
        Fire = 0,
        Lightning = 1,
        Ice = 2,
        Wind = 3,
        Earth = 4,
        Water = 5,
        Holy = 6,
        Dark = 7,
    }

    public class Elements : BaseDataType, IEquatable<Elements>, ISupportDigest, ISupportDefault<Elements>
    {
        /*
        private static class ElementFlags
        { 
            internal const byte Fire = 0x80;
            internal const byte Lightning = 0x40;
            internal const byte Ice = 0x20;
            internal const byte Wind = 0x10;
            internal const byte Earth = 0x08;
            internal const byte Water = 0x04;
            internal const byte Holy = 0x02;
            internal const byte Dark = 0x01;
        }
        */

        [Flags]
        public enum ElementFlags : byte
        {
            Fire = 0x80,
            Lightning = 0x40,
            Ice = 0x20,
            Wind = 0x10,
            Earth = 0x08,
            Water = 0x04,
            Holy = 0x02,
            Dark = 0x01
        };

		#region Instance Variables

        /*
        private static class Strings
        {
            public const string Fire = "Fire";
            public const string Lightning = "Lightning";
            public const string Ice = "Ice";
            public const string Wind = "Wind";
            public const string Earth = "Earth";
            public const string Water = "Water";
            public const string Holy = "Holy";
            public const string Dark = "Dark";
        }
        private static readonly string[] elementNames = new string[8] {
            Strings.Fire, Strings.Lightning, Strings.Ice, Strings.Wind,
            Strings.Earth, Strings.Water, Strings.Holy, Strings.Dark };
        */

		#endregion Instance Variables 

		#region Public Properties

        //public bool Dark { get; set; }

        public Elements Default { get; set; }
        public ElementFlags Value { get; set; }

        public IList<string> DigestableProperties
        {
            get 
            { 
                //return elementNames; 
                return Settings.ElementNames;
            }
        }

        //public bool Earth { get; set; }

        //public bool Fire { get; set; }

        public bool HasChanged
        {
            get { return !Equals( Default ); }
        }

        //public bool Holy { get; set; }

        //public bool Ice { get; set; }

        //public bool Lightning { get; set; }

        //public bool Water { get; set; }

        //public bool Wind { get; set; }

		#endregion Public Properties 

		#region Constructors (1) 

        public Elements( byte b )
        {
            //PopulateFromBools( PatcherLib.Utilities.Utilities.BooleansFromByte( b ) );
            Value = (ElementFlags)b;
        }

        internal Elements()
        {
        }

        /*
        private void PopulateFromBools( IList<bool> bools )
        {
            System.Diagnostics.Debug.Assert( bools.Count == 8 );
            Fire = bools[7];
            Lightning = bools[6];
            Ice = bools[5];
            Wind = bools[4];
            Earth = bools[3];
            Water = bools[2];
            Holy = bools[1];
            Dark = bools[0];
        }
        */

		#endregion Constructors 

		#region Public Methods 

        public static void Copy( Elements source, Elements destination )
        {
            destination.Value = source.Value;
            //destination.Fire = source.Fire;
            //destination.Lightning = source.Lightning;
            //destination.Ice = source.Ice;
            //destination.Wind = source.Wind;
            //destination.Earth = source.Earth;
            //destination.Water = source.Water;
            //destination.Holy = source.Holy;
            //destination.Dark = source.Dark;
        }

        public void CopyTo( Elements destination )
        {
            Copy( this, destination );
        }

        /*
        public bool Equals( Elements other )
        {
            return
                other != null &&
                other.Fire == Fire &&
                other.Lightning == Lightning &&
                other.Ice == Ice &&
                other.Wind == Wind &&
                other.Earth == Earth &&
                other.Water == Water &&
                other.Holy == Holy &&
                other.Dark == Dark;
        }
        */

        public bool Equals(Elements other)
        {
            return (other != null) && (Value == other.Value);
        }

        /*
        public override bool Equals( object obj )
        {
            if( obj is Elements )
            {
                return Equals( obj as Elements );
            }
            else
            {
                return base.Equals( obj );
            }
        }
        */

        /*
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        */

        public bool[] ToBoolArray()
        {
            return new bool[8] {
                ((Value & ElementFlags.Fire) == ElementFlags.Fire), 
                ((Value & ElementFlags.Lightning) == ElementFlags.Lightning), 
                ((Value & ElementFlags.Ice) == ElementFlags.Ice), 
                ((Value & ElementFlags.Wind) == ElementFlags.Wind), 
                ((Value & ElementFlags.Earth) == ElementFlags.Earth), 
                ((Value & ElementFlags.Water) == ElementFlags.Water), 
                ((Value & ElementFlags.Holy) == ElementFlags.Holy), 
                ((Value & ElementFlags.Dark) == ElementFlags.Dark)
            };
        }

        public byte ToByte()
        {
            return (byte)Value;
            //return PatcherLib.Utilities.Utilities.ByteFromBooleans( Fire, Lightning, Ice, Wind, Earth, Water, Holy, Dark );
        }

        /*
        public override string ToString()
        {
            List<string> strings = new List<string>( 8 );
            foreach( string name in elementNames )
            {
                if( ReflectionHelpers.GetFieldOrProperty<bool>( this, name ) )
                {
                    strings.Add( name );
                }
            }
            return string.Join( " | ", strings.ToArray() );
        }
        */

        public void SetElementFlagState(ElementFlags flags, bool isSet)
        {
            if (isSet)
                Value |= flags;
            else
                Value &= ~flags;
        }

		#endregion Public Methods 
    
        /*
        protected override void WriteXml(System.Xml.XmlWriter writer)
        {
            bool[] bools = ToXmlBoolArray();
            System.Diagnostics.Debug.Assert( bools.Length == DigestableProperties.Count );
            for ( int i = 0; i < bools.Length; i++ )
            {
                writer.WriteValueElement( DigestableProperties[i], bools[i] );
            }
        }
        */

        protected override void WriteXml(System.Xml.XmlWriter writer)
        {
            byte flag = 0x80;
            for (int i = 0; i < DigestableProperties.Count; i++)
            {
                writer.WriteValueElement(DigestableProperties[i], (((byte)Value & flag) > 0));
                flag /= 2;
            }
        }

        /*
        protected override void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();
            bool[] bools = new bool[8];
            for ( int i = 0; i < bools.Length; i++ )
            {
                bools[i] = reader.ReadElementContentAsBoolean();
            }
            PopulateFromBools( bools );
            reader.ReadEndElement();
        }
        */

        protected override void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();
            byte value = 0;
            for (byte flag = 0x80; flag > 0; flag >>= 1)
            {
                value |= (reader.ReadElementContentAsBoolean() ? flag : (byte)0);
            }
            reader.ReadEndElement();
            Value = (ElementFlags)value;
        }
    }
}
