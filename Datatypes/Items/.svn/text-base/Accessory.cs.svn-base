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

namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// Represents an accessory.
    /// </summary>
    public class Accessory : Item
    {
		#region Instance Variables (1) 

        private static readonly List<string> accessoryDigestableProperties;

		#endregion Instance Variables 

		#region Public Properties (5) 

        public Accessory AccessoryDefault { get; private set; }

        public override IList<string> DigestableProperties
        {
            get { return accessoryDigestableProperties; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public override bool HasChanged
        {
            get
            {
                return base.HasChanged ||
                    (AccessoryDefault != null &&
                    (MagicEvade != AccessoryDefault.MagicEvade ||
                    PhysicalEvade != AccessoryDefault.PhysicalEvade));
            }
        }

        public byte MagicEvade { get; set; }

        public byte PhysicalEvade { get; set; }

		#endregion Public Properties 

		#region Constructors (3) 

        static Accessory()
        {
            accessoryDigestableProperties = new List<string>( Item.digestableProperties );
            accessoryDigestableProperties.AddRange( new string[] {
                "PhysicalEvade", "MagicEvade" } );
        }

        public Accessory( UInt16 offset, IList<byte> itemBytes, IList<byte> accessoryBytes )
            : this( offset, itemBytes, accessoryBytes, null )
        {
        }

        public Accessory( UInt16 offset, IList<byte> itemBytes, IList<byte> accessoryBytes, Accessory defaults )
            : base( offset, itemBytes, defaults )
        {
            AccessoryDefault = defaults;
            PhysicalEvade = accessoryBytes[0];
            MagicEvade = accessoryBytes[1];
        }

		#endregion Constructors 

		#region Public Methods (8) 

        public static void CopyAccessory( Accessory source, Accessory destination )
        {
            destination.PhysicalEvade = source.PhysicalEvade;
            destination.MagicEvade = source.MagicEvade;
        }

        public void CopyAccessoryTo( Accessory destination )
        {
            CopyAccessory( this, destination );
        }

        public static void CopyAll( Accessory source, Accessory destination )
        {
            CopyAccessory( source, destination );
            CopyCommon( source, destination );
        }

        public void CopyAllTo( Accessory destination )
        {
            CopyAll( this, destination );
        }

        public byte[] ToAccessoryByteArray()
        {
            return new byte[2] { PhysicalEvade, MagicEvade };
        }

        public override byte[] ToFirstByteArray()
        {
            return ToItemByteArray();
        }

        public byte[] ToItemByteArray()
        {
            return base.ToByteArray().ToArray();
        }

        public override byte[] ToSecondByteArray()
        {
            return ToAccessoryByteArray();
        }

		#endregion Public Methods 
    }
}
