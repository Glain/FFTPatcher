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

using System.Collections.Generic;
using PatcherLib.Datatypes;
using PatcherLib;

namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// The types of equipment a <see cref="Job"/> can use.
    /// </summary>
    public class Equipment : IChangeable, ISupportDigest, ISupportDefault<Equipment>
    {
		#region Instance Variables

        private Context ourContext = Context.Default;

        public bool Armguard;
        public bool Armlet;
        public bool Armor;
        public bool Axe;
        public bool Bag;
        public bool Book;
        public bool Bow;
        public bool Cloak;
        public bool Cloth;
        public bool Clothing;
        public bool Crossbow;
        public bool FellSword;
        public bool Flail;
        public bool Gun;
        public bool HairAdornment;
        public bool Hat;
        public bool Helmet;
        public bool Instrument;
        public bool Katana;
        public bool Knife;
        public bool KnightsSword;
        public bool LipRouge;
        public bool NinjaBlade;
        public bool Perfume;
        public bool Pole;
        public bool Polearm;
        private static readonly string[] pspNames = new string[40] {
            "Unused", "Knife", "NinjaBlade", "Sword", "KnightsSword", "Katana", "Axe", "Rod",
            "Staff", "Flail", "Gun", "Crossbow", "Bow", "Instrument", "Book", "Polearm",
            "Pole","Bag","Cloth","Shield","Helmet","Hat","HairAdornment","Armor",
            "Clothing","Robe","Shoes","Armguard","Ring","Armlet","Cloak","Perfume",
            "Unknown1","Unknown2","Unknown3","FellSword","LipRouge","Unknown6","Unknown7","Unknown8" };
        private static readonly string[] psxNames = new string[32] {
            "Unused", "Knife", "NinjaBlade", "Sword", "KnightsSword", "Katana", "Axe", "Rod",
            "Staff", "Flail", "Gun", "Crossbow", "Bow", "Instrument", "Book", "Polearm",
            "Pole","Bag","Cloth","Shield","Helmet","Hat","HairAdornment","Armor",
            "Clothing","Robe","Shoes","Armguard","Ring","Armlet","Cloak","Perfume" };
        public bool Ring;
        public bool Robe;
        public bool Rod;
        public bool Shield;
        public bool Shoes;
        public bool Staff;
        public bool Sword;
        public bool Unknown1;
        public bool Unknown2;
        public bool Unknown3;
        public bool Unknown6;
        public bool Unknown7;
        public bool Unknown8;
        public bool Unused;

		#endregion Instance Variables 

		#region Public Properties (3) 

        public Equipment Default { get; private set; }

        public IList<string> DigestableProperties
        {
            get { return ourContext == Context.US_PSP ? pspNames : psxNames; }
        }

        public bool HasChanged
        {
            get { return Default != null && !PatcherLib.Utilities.Utilities.CompareArrays(ToByteArray(ourContext), Default.ToByteArray(ourContext)); }
        }

		#endregion Public Properties 

		#region Constructors (2) 

        public Equipment( IList<byte> bytes, Context context )
            : this( bytes, null, context )
        {
        }

        public Equipment( IList<byte> bytes, Equipment defaults, Context context )
        {
            Default = defaults;
            ourContext = context;
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[0], ref Unused, ref Knife, ref NinjaBlade, ref Sword, ref KnightsSword, ref Katana, ref Axe, ref Rod );
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[1], ref Staff, ref Flail, ref Gun, ref Crossbow, ref Bow, ref Instrument, ref Book, ref Polearm );
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[2], ref Pole, ref Bag, ref Cloth, ref Shield, ref Helmet, ref Hat, ref HairAdornment, ref Armor );
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[3], ref Clothing, ref Robe, ref Shoes, ref Armguard, ref Ring, ref Armlet, ref Cloak, ref Perfume );
            if( bytes.Count == 5 )
            {
                PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[4], ref Unknown1, ref Unknown2, ref Unknown3, ref FellSword, ref LipRouge, ref Unknown6, ref Unknown7, ref Unknown8 );
            }
        }

		#endregion Constructors 

		#region Public Methods (8) 

        public static void Copy( Equipment source, Equipment destination )
        {
            destination.Unused = source.Unused;
            destination.Knife = source.Knife;
            destination.NinjaBlade = source.NinjaBlade;
            destination.Sword = source.Sword;
            destination.KnightsSword = source.KnightsSword;
            destination.Katana = source.Katana;
            destination.Axe = source.Axe;
            destination.Rod = source.Rod;

            destination.Staff = source.Staff;
            destination.Flail = source.Flail;
            destination.Gun = source.Gun;
            destination.Crossbow = source.Crossbow;
            destination.Bow = source.Bow;
            destination.Instrument = source.Instrument;
            destination.Book = source.Book;
            destination.Polearm = source.Polearm;

            destination.Pole = source.Pole;
            destination.Bag = source.Bag;
            destination.Cloth = source.Cloth;
            destination.Shield = source.Shield;
            destination.Helmet = source.Helmet;
            destination.Hat = source.Hat;
            destination.HairAdornment = source.HairAdornment;
            destination.Armor = source.Armor;

            destination.Clothing = source.Clothing;
            destination.Robe = source.Robe;
            destination.Shoes = source.Shoes;
            destination.Armguard = source.Armguard;
            destination.Ring = source.Ring;
            destination.Armlet = source.Armlet;
            destination.Cloak = source.Cloak;
            destination.Perfume = source.Perfume;

            destination.Unknown1 = source.Unknown1;
            destination.Unknown2 = source.Unknown2;
            destination.Unknown3 = source.Unknown3;
            destination.FellSword = source.FellSword;
            destination.LipRouge = source.LipRouge;
            destination.Unknown6 = source.Unknown6;
            destination.Unknown7 = source.Unknown7;
            destination.Unknown8 = source.Unknown8;
        }

        public void CopyTo( Equipment destination )
        {
            Copy( this, destination );
        }

        public override bool Equals( object obj )
        {
            return (obj is Equipment) &&
                PatcherLib.Utilities.Utilities.CompareArrays(ToByteArray(ourContext), (obj as Equipment).ToByteArray(ourContext));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool[] ToBoolArray()
        {
            return new bool[40] {
                Unused, Knife, NinjaBlade, Sword, KnightsSword, Katana, Axe, Rod,
                Staff, Flail, Gun, Crossbow, Bow, Instrument, Book, Polearm,
                Pole, Bag, Cloth, Shield, Helmet, Hat, HairAdornment, Armor,
                Clothing, Robe, Shoes, Armguard, Ring, Armlet, Cloak, Perfume,
                Unknown1, Unknown2, Unknown3, FellSword, LipRouge, Unknown6, Unknown7, Unknown8 };
        }

        public byte[] ToByteArray()
        {
            return ToByteArray(ourContext);
        }

        public byte[] ToByteArray( Context context )
        {
            switch( context )
            {
                case Context.US_PSX:
                    return ToByteArrayPSX();
                default:
                    return ToByteArrayPSP();
            }
        }

        public override string ToString()
        {
            List<string> strings = new List<string>( 40 );
            string[] names = ourContext == Context.US_PSP ? pspNames : psxNames;
            foreach( string name in names )
            {
                if( ReflectionHelpers.GetFieldOrProperty<bool>( this, name ) )
                {
                    strings.Add( name );
                }
            }
            return string.Join( " | ", strings.ToArray() );
        }

		#endregion Public Methods 

		#region Private Methods 

        private byte[] ToByteArrayPSX()
        {
            byte[] result = new byte[4];
            result[0] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Unused, Knife, NinjaBlade, Sword, KnightsSword, Katana, Axe, Rod );
            result[1] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Staff, Flail, Gun, Crossbow, Bow, Instrument, Book, Polearm );
            result[2] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Pole, Bag, Cloth, Shield, Helmet, Hat, HairAdornment, Armor );
            result[3] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Clothing, Robe, Shoes, Armguard, Ring, Armlet, Cloak, Perfume );
            return result;
        }

        private byte[] ToByteArrayPSP()
        {
            byte[] result = new byte[5];
            result[0] = PatcherLib.Utilities.Utilities.ByteFromBooleans(Unused, Knife, NinjaBlade, Sword, KnightsSword, Katana, Axe, Rod);
            result[1] = PatcherLib.Utilities.Utilities.ByteFromBooleans(Staff, Flail, Gun, Crossbow, Bow, Instrument, Book, Polearm);
            result[2] = PatcherLib.Utilities.Utilities.ByteFromBooleans(Pole, Bag, Cloth, Shield, Helmet, Hat, HairAdornment, Armor);
            result[3] = PatcherLib.Utilities.Utilities.ByteFromBooleans(Clothing, Robe, Shoes, Armguard, Ring, Armlet, Cloak, Perfume);
            result[4] = PatcherLib.Utilities.Utilities.ByteFromBooleans(Unknown1, Unknown2, Unknown3, FellSword, LipRouge, Unknown6, Unknown7, Unknown8);
            return result;
        }

		#endregion Private Methods 
    }
}
