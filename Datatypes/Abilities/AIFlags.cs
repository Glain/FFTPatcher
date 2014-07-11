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
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// Represents an <see cref="Ability"/>'s AI behavior.
    /// </summary>
    public class AIFlags : BaseDataType, ISupportDigest, ISupportDefault<AIFlags>
    {
		#region Instance Variables (25) 

        public bool AddStatus;
        public bool Blank;
        public bool CancelStatus;
        public bool Unknown10;
        public bool Unknown4;
        public bool HP;
        public bool LineOfSight;
        public bool Unknown5;
        public bool Unknown9;
        public bool MP;
        public bool AllowRandomly;
        public bool Reflectable;
        public bool Silence;
        public bool Stats;
        public bool TargetAllies;
        public bool TargetEnemies;
        public bool Unknown7;
        public bool Unknown8;
        public bool UndeadReverse;
        public bool Unequip;
        public bool Unknown1;
        public bool Unknown2;
        public bool Unknown3;
        public bool Unknown6;

		#endregion Instance Variables 

		#region Public Properties (3) 

        public AIFlags Default { get; set; }

        public IList<string> DigestableProperties
        {
            get { return digestableProperties; }
        }

        public bool HasChanged
        {
            get { return !PatcherLib.Utilities.Utilities.CompareArrays( ToByteArray(), Default.ToByteArray() ); }
        }

		#endregion Public Properties 

		#region Constructors (2) 

        internal AIFlags()
        {
        }

        public AIFlags( IList<byte> bytes )
            : this( bytes, null )
        {
        }

        public AIFlags( IList<byte> bytes, AIFlags defaults )
        {
            Default = defaults;

            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[0],
                ref HP, ref MP, ref CancelStatus, ref AddStatus, ref Stats, ref Unequip, ref TargetEnemies, ref TargetAllies );

            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[1],
                ref LineOfSight, ref Reflectable, ref UndeadReverse, ref Unknown1, ref AllowRandomly, ref Unknown2, ref Unknown3, ref Silence );
            Silence = !Silence;

            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[2],
                ref Blank, ref Unknown4, ref Unknown5, ref Unknown6, ref Unknown7, ref Unknown8, ref Unknown9, ref Unknown10 );
            Unknown6 = !Unknown6;
        }

		#endregion Constructors 

		#region Public Methods (4) 

        public static void Copy( AIFlags source, AIFlags destination )
        {
            destination.AddStatus = source.AddStatus;
            destination.Blank = source.Blank;
            destination.CancelStatus = source.CancelStatus;
            destination.Unknown10 = source.Unknown10;
            destination.Unknown4 = source.Unknown4;
            destination.HP = source.HP;
            destination.LineOfSight = source.LineOfSight;
            destination.Unknown5 = source.Unknown5;
            destination.Unknown9 = source.Unknown9;
            destination.MP = source.MP;
            destination.AllowRandomly = source.AllowRandomly;
            destination.Reflectable = source.Reflectable;
            destination.Silence = source.Silence;
            destination.Stats = source.Stats;
            destination.TargetAllies = source.TargetAllies;
            destination.TargetEnemies = source.TargetEnemies;
            destination.Unknown7 = source.Unknown7;
            destination.Unknown8 = source.Unknown8;
            destination.UndeadReverse = source.UndeadReverse;
            destination.Unequip = source.Unequip;
            destination.Unknown1 = source.Unknown1;
            destination.Unknown2 = source.Unknown2;
            destination.Unknown3 = source.Unknown3;
            destination.Unknown6 = source.Unknown6;
        }

        public void CopyTo( AIFlags destination )
        {
            Copy( this, destination );
        }

        private static readonly string[] digestableProperties = new string[] {
            "HP","MP","CancelStatus","AddStatus","Stats","Unequip","TargetEnemies","TargetAllies",
            "LineOfSight","Reflectable","UndeadReverse","Unknown1","AllowRandomly","Unknown2","Unknown3",
            "Silence","Blank","Unknown4","Unknown5","Unknown6","Unknown7",
            "Unknown8","Unknown9","Unknown10" };

        public bool[] ToBoolArray()
        {
            return new bool[24] { 
                HP, MP, CancelStatus, AddStatus, Stats, Unequip, TargetEnemies, TargetAllies,
                LineOfSight, Reflectable, UndeadReverse, Unknown1, AllowRandomly, Unknown2, Unknown3, Silence,
                Blank, Unknown4, Unknown5, Unknown6, Unknown7, Unknown8, Unknown9, Unknown10 };
        }

        public byte[] ToByteArray()
        {
            byte[] result = new byte[3];
            result[0] = PatcherLib.Utilities.Utilities.ByteFromBooleans( HP, MP, CancelStatus, AddStatus, Stats, Unequip, TargetEnemies, TargetAllies );
            result[1] = PatcherLib.Utilities.Utilities.ByteFromBooleans( LineOfSight, Reflectable, UndeadReverse, Unknown1, AllowRandomly, Unknown2, Unknown3, !Silence );
            result[2] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Blank, Unknown4, Unknown5, !Unknown6, Unknown7, Unknown8, Unknown9, Unknown10 );
            return result;
        }

		#endregion Public Methods 
    
        protected override void ReadXml( System.Xml.XmlReader reader )
        {
            reader.ReadStartElement();
            for ( int i = 0; i < DigestableProperties.Count; i++ )
            {
                PatcherLib.ReflectionHelpers.SetFieldOrProperty(
                    this, DigestableProperties[i], reader.ReadElementContentAsBoolean() );
            }
            reader.ReadEndElement();
        }

        protected override void WriteXml( System.Xml.XmlWriter writer )
        {
            bool[] bools = ToBoolArray();
            System.Diagnostics.Debug.Assert( bools.Length == digestableProperties.Length );
            for ( int i = 0; i < bools.Length; i++ )
            {
                writer.WriteValueElement( digestableProperties[i], bools[i] );
            }
        }
    }
}
