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
    /// Represents a Chemist's item.
    /// </summary>
    public class ChemistItem : Item
    {
		#region Instance Variables (1) 

        private static readonly List<string> chemistItemDigestableProperties;

		#endregion Instance Variables 

		#region Public Properties (6) 

        public ChemistItem ChemistItemDefault { get; private set; }

        public override IList<string> DigestableProperties
        {
            get { return chemistItemDigestableProperties; }
        }

        [Hex]
        public byte Formula { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public override bool HasChanged
        {
            get
            {
                return base.HasChanged ||
                    (ChemistItemDefault != null &&
                    (Formula != ChemistItemDefault.Formula ||
                    InflictStatus != ChemistItemDefault.InflictStatus) ||
                    X != ChemistItemDefault.X);
            }
        }

        [Hex]
        public byte InflictStatus { get; set; }

        public byte OldInflictStatus { get; set; }

        public byte X { get; set; }

		#endregion Public Properties 

		#region Constructors (3) 

        static ChemistItem()
        {
            chemistItemDigestableProperties = new List<string>( Item.digestableProperties );
            chemistItemDigestableProperties.Add( "Formula" );
            chemistItemDigestableProperties.Add( "X" );
            chemistItemDigestableProperties.Add( "InflictStatus" );
        }

        public ChemistItem(UInt16 offset, IList<byte> itemBytes, IList<byte> chemistBytes, PatcherLib.Datatypes.Context context) :
            this( offset, itemBytes, chemistBytes, null, context )
        {
        }

        public ChemistItem(UInt16 offset, IList<byte> itemBytes, IList<byte> chemistBytes, ChemistItem defaults, PatcherLib.Datatypes.Context context)
            : base( offset, itemBytes, defaults, context )
        {
            ChemistItemDefault = defaults;
            Formula = chemistBytes[0];
            X = chemistBytes[1];
            InflictStatus = chemistBytes[2];

            OldInflictStatus = InflictStatus;
        }

		#endregion Constructors 

		#region Public Methods (8) 

        public static void CopyAll( ChemistItem source, ChemistItem destination )
        {
            CopyChemistItem( source, destination );
            CopyCommon( source, destination );
        }

        public void CopyAllTo( ChemistItem destination )
        {
            CopyAll( this, destination );
        }

        public static void CopyChemistItem( ChemistItem source, ChemistItem destination )
        {
            destination.Formula = source.Formula;
            destination.X = source.X;
            destination.InflictStatus = source.InflictStatus;
            destination.OldInflictStatus = source.OldInflictStatus;
        }

        public void CopyChemistItemTo( ChemistItem destination )
        {
            CopyChemistItem( this, destination );
        }

        public byte[] ToChemistItemByteArray()
        {
            return new byte[3] { Formula, X, InflictStatus };
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
            return ToChemistItemByteArray();
        }

		#endregion Public Methods 
    }
}
