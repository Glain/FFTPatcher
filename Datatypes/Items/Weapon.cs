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
    /// Represents a weapon.
    /// </summary>
    public class Weapon : Item
    {
		#region Instance Variables (9) 

        public bool Arc;


        public bool Throwable;

        [Obsolete]
        public bool Blank { get { return Throwable; } set { Throwable = value; } }

        public bool Direct;
        public bool Force2Hands;
        public bool Lunging;
        public bool Striking;
        public bool TwoHands;
        public bool TwoSwords;
        private static readonly List<string> weaponDigestableProperties = GetWeaponDigestableProperties();

		#endregion Instance Variables 

		#region Public Properties

        public override IList<string> DigestableProperties
        {
            get { return weaponDigestableProperties; }
        }

        public Elements Elements { get; private set; }

        public byte EvadePercentage { get; set; }

        public AbilityFormula Formula { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public override bool HasChanged
        {
            get
            {
                return base.HasChanged ||
                    (WeaponDefault != null &&
                    (Elements.ToByte() != WeaponDefault.Elements.ToByte() ||
                    !PatcherLib.Utilities.Utilities.CompareArrays( ToWeaponBoolArray(), WeaponDefault.ToWeaponBoolArray() ) ||
                    EvadePercentage != WeaponDefault.EvadePercentage ||
                    Formula.Value != WeaponDefault.Formula.Value ||
                    InflictStatus != WeaponDefault.InflictStatus ||
                    Range != WeaponDefault.Range ||
                    Unknown != WeaponDefault.Unknown ||
                    WeaponPower != WeaponDefault.WeaponPower));
            }
        }

        [Hex]
        public byte InflictStatus { get; set; }

        public byte OldInflictStatus { get; set; }

        public byte Range { get; set; }

        public byte Unknown { get; set; }

        public Weapon WeaponDefault { get; private set; }

        public byte WeaponPower { get; set; }

		#endregion Public Properties 

		#region Constructors

        public Weapon(UInt16 offset, IList<byte> itemBytes, IList<byte> weaponBytes, PatcherLib.Datatypes.Context context)
            : this( offset, itemBytes, weaponBytes, null, context )
        {
        }

        public Weapon( UInt16 offset, IList<byte> itemBytes, IList<byte> weaponBytes, Weapon defaults, PatcherLib.Datatypes.Context context )
            : base( offset, itemBytes, defaults, context )
        {
            Range = weaponBytes[0];
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( weaponBytes[1], ref Striking, ref Lunging, ref Direct, ref Arc, ref TwoSwords, ref TwoHands, ref Throwable, ref Force2Hands );
            Formula = AbilityFormula.PSPAbilityFormulaHash[weaponBytes[2]];
            Unknown = weaponBytes[3];
            WeaponPower = weaponBytes[4];
            EvadePercentage = weaponBytes[5];
            InflictStatus = weaponBytes[7];
            Elements = new Elements( weaponBytes[6] );

            if( defaults != null )
            {
                WeaponDefault = defaults;
                Elements.Default = WeaponDefault.Elements;
            }

            OldInflictStatus = InflictStatus;
        }

		#endregion Constructors

        private static List<string> GetWeaponDigestableProperties()
        {
            List<string> props = new List<string>( Item.digestableProperties );
            props.AddRange(new string[] {
                "Range", "Formula", "WeaponPower", "EvadePercentage", "InflictStatus",
                "Striking", "Lunging", "Direct", "Arc", "TwoSwords", "TwoHands", "Throwable",
                "Force2Hands", "Elements"} );

            return props;
        }

		#region Public Methods (9) 

        public static void CopyAll( Weapon source, Weapon destination )
        {
            CopyWeapon( source, destination );
            CopyCommon( source, destination );
        }

        public void CopyAllTo( Weapon destination )
        {
            CopyAll( this, destination );
        }

        public static void CopyWeapon( Weapon source, Weapon destination )
        {
            destination.Range = source.Range;
            destination.Striking = source.Striking;
            destination.Lunging = source.Lunging;
            destination.Direct = source.Direct;
            destination.Arc = source.Arc;
            destination.TwoSwords = source.TwoSwords;
            destination.TwoHands = source.TwoHands;
            destination.Throwable = source.Throwable;
            destination.Force2Hands = source.Force2Hands;
            destination.Formula = source.Formula;
            destination.Unknown = source.Unknown;
            destination.WeaponPower = source.WeaponPower;
            destination.EvadePercentage = source.EvadePercentage;
            destination.InflictStatus = source.InflictStatus;
            destination.OldInflictStatus = source.OldInflictStatus;

            source.Elements.CopyTo( destination.Elements );
        }

        public void CopyWeaponTo( Weapon destination )
        {
            CopyWeapon( this, destination );
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
            return ToWeaponByteArray();
        }

        public bool[] ToWeaponBoolArray()
        {
            return new bool[8] {
                Striking, Lunging, Direct, Arc, TwoSwords, TwoHands, Throwable, Force2Hands };
        }

        public byte[] ToWeaponByteArray()
        {
            byte[] result = new byte[8];
            result[0] = Range;
            result[1] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Striking, Lunging, Direct, Arc, TwoSwords, TwoHands, Throwable, Force2Hands );
            result[2] = Formula.Value;
            result[3] = Unknown;
            result[4] = WeaponPower;
            result[5] = EvadePercentage;
            result[6] = Elements.ToByte();
            result[7] = InflictStatus;
            return result;
        }

		#endregion Public Methods 
    }
}
