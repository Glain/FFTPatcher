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
using System.Linq;

namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// 
    /// Note: This order is reversed from the in-game order.
    /// </summary>
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
    //Try to find a way to combine this with `Elements`.
    /// <summary>
    /// 
    /// Note: Order should be matched with FFTPatcher.Datatypes.Element.
    /// </summary>
    [Flags]
    public enum ElementFlags : byte
    {
        Fire = 1,
        Lightning = 2,
        Ice = 4,
        Wind = 8,
        Earth = 16,
        Water = 32,
        Holy = 64,
        Dark = 128,
    }

    public class Elements : BaseDataType, IEquatable<Elements>, ISupportDigest, ISupportDefault<Elements>
    {
        #region Instance Variables (1) 

        //TODO: Read this from XML file?
        public static IDictionary<Element, string> ElementNames = new Dictionary<Element, string>
        {
            [Element.Fire] = "Fire",
            [Element.Lightning] = "Lightning",
            [Element.Ice] = "Ice",
            [Element.Wind] = "Wind",
            [Element.Earth] = "Earth",
            [Element.Water] = "Water",
            [Element.Holy] = "Holy",
            [Element.Dark] = "Dark",
        };
        /// <summary>
        /// Properties of the class for digest purposes.
        /// Order matters.
        /// </summary>
        private static readonly string[] propertyNames = {
            "Fire",
            "Lightning",
            "Ice",
            "Wind",
            "Earth",
            "Water",
            "Holy",
            "Dark",
        };

        #endregion Instance Variables 

        #region Public Properties (11) 

        public ElementFlags Flags { get; set; }
        public bool this[Element e]
        {
            get => ((int)Flags & (1 << (int)e)) != 0;
            set
            {
                if (this[e] != value)  //Just toggle if off.
                    Flags = (ElementFlags)((int)Flags ^ (1 << (int)e));
            }
        }

        public bool Fire
        {
            get => this[Element.Fire];
            set => this[Element.Fire] = value;
        }

        public bool Lightning
        {
            get => this[Element.Lightning];
            set => this[Element.Lightning] = value;
        }

        public bool Ice
        {
            get => this[Element.Ice];
            set => this[Element.Ice] = value;
        }

        public bool Wind
        {
            get => this[Element.Wind];
            set => this[Element.Wind] = value;
        }

        public bool Earth
        {
            get => this[Element.Earth];
            set => this[Element.Earth] = value;
        }

        public bool Water
        {
            get => this[Element.Water];
            set => this[Element.Water] = value;
        }

        public bool Holy
        {
            get => this[Element.Holy];
            set => this[Element.Holy] = value;
        }

        public bool Dark
        {
            get => this[Element.Dark];
            set => this[Element.Dark] = value;
        }

        public Elements Default { get; set; }

        public IList<string> DigestableProperties
        {
            get { return propertyNames; }
        }


        public bool HasChanged
        {
            get { return !Equals(Default); }
        }

        #endregion Public Properties 

        #region Constructors (1) 

        public Elements(byte b)
        {
            PopulateFromBoolsMSB(PatcherLib.Utilities.Utilities.BooleansFromByte(b));
        }

        internal Elements()
        {
        }

        private void PopulateFromBoolsMSB(IList<bool> bools)
        {
            System.Diagnostics.Debug.Assert(bools.Count == 8);
            Fire = bools[7];
            Lightning = bools[6];
            Ice = bools[5];
            Wind = bools[4];
            Earth = bools[3];
            Water = bools[2];
            Holy = bools[1];
            Dark = bools[0];
        }

        #endregion Constructors 

        #region Public Methods (8) 

        public static void Copy(Elements source, Elements destination)
        {
            destination.Flags = source.Flags;
            //Doesn't copy Defaults because that'd be recursive?
        }

        public void CopyTo(Elements destination)
        {
            Copy(this, destination);
        }

        public bool Equals(Elements other)
        {
            return other?.Flags == Flags;
        }

        public override bool Equals(object obj)
        {
            if (obj is Elements)
            {
                return Equals(obj as Elements);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool[] ToBoolArray()
        {
            return new bool[8] {
                Fire, Lightning, Ice, Wind, Earth, Water, Holy, Dark };
        }

        public byte ToByte()
        {
            return PatcherLib.Utilities.Utilities.ByteFromBooleans(Fire, Lightning, Ice, Wind, Earth, Water, Holy, Dark);
        }

        public override string ToString()
        {
            List<string> strings = new List<string>(8);
            foreach (string name in propertyNames)
            {
                if (ReflectionHelpers.GetFieldOrProperty<bool>(this, name))
                {
                    strings.Add(name);
                }
            }
            return string.Join(" | ", strings.ToArray());
        }

        #endregion Public Methods 

        protected override void WriteXml(System.Xml.XmlWriter writer)
        {
            bool[] bools = ToBoolArray();
            System.Diagnostics.Debug.Assert(bools.Length == DigestableProperties.Count);
            for (int i = 0; i < bools.Length; i++)
            {
                writer.WriteValueElement(DigestableProperties[i], bools[i]);
            }
        }

        protected override void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();
            bool[] bools = new bool[8];
            for (int i = 0; i < bools.Length; i++)
            {
                bools[i] = reader.ReadElementContentAsBoolean();
            }
            Array.Reverse(bools);
            PopulateFromBoolsMSB(bools);
            reader.ReadEndElement();
        }
    }
}
