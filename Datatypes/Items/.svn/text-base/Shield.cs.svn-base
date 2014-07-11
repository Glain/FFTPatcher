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
    /// Represents a shield.
    /// </summary>
    public class Shield : Item
    {
		#region Instance Variables (1) 

        private static readonly List<string> shieldDigestableProperties;

		#endregion Instance Variables 

		#region Public Properties (5) 

        public override IList<string> DigestableProperties
        {
            get { return shieldDigestableProperties; }
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
                    (ShieldDefault != null &&
                    (MagicBlockRate != ShieldDefault.MagicBlockRate ||
                    PhysicalBlockRate != ShieldDefault.PhysicalBlockRate));
            }
        }

        public byte MagicBlockRate { get; set; }

        public byte PhysicalBlockRate { get; set; }

        public Shield ShieldDefault { get; private set; }

		#endregion Public Properties 

		#region Constructors (3) 

        static Shield()
        {
            shieldDigestableProperties = new List<string>( Item.digestableProperties );
            shieldDigestableProperties.Add( "PhysicalBlockRate" );
            shieldDigestableProperties.Add( "MagicBlockRate" );
        }

        public Shield( UInt16 offset, IList<byte> itemBytes, IList<byte> shieldBytes )
            : this( offset, itemBytes, shieldBytes, null )
        {
        }

        public Shield( UInt16 offset, IList<byte> itemBytes, IList<byte> shieldBytes, Shield defaults )
            : base( offset, itemBytes, defaults )
        {
            ShieldDefault = defaults;
            PhysicalBlockRate = shieldBytes[0];
            MagicBlockRate = shieldBytes[1];
        }

		#endregion Constructors 

		#region Public Methods (8) 

        public static void CopyAll( Shield source, Shield destination )
        {
            CopyShield( source, destination );
            CopyCommon( source, destination );
        }

        public void CopyAllTo( Shield destination )
        {
            CopyAll( this, destination );
        }

        public static void CopyShield( Shield source, Shield destination )
        {
            destination.PhysicalBlockRate = source.PhysicalBlockRate;
            destination.MagicBlockRate = source.MagicBlockRate;
        }

        public void CopyShieldTo( Shield destination )
        {
            CopyShield( this, destination );
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
            return ToShieldByteArray();
        }

        public byte[] ToShieldByteArray()
        {
            return new byte[2] { PhysicalBlockRate, MagicBlockRate };
        }

		#endregion Public Methods 
    }
}
