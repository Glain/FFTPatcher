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
using PatcherLib;

namespace FFTPatcher.Datatypes
{
    public class Statuses : ISupportDigest, ISupportDefault<Statuses>
    {
		#region Instance Variables (41) 

        public bool Berserk;
        public bool BloodSuck;
        public bool Charging;
        public bool Charm;
        public bool Chicken;
        public bool Confusion;
        public bool Critical;
        public bool Crystal;
        public bool DarkEvilLooking;
        public bool Darkness;
        public bool Dead;
        public bool DeathSentence;
        public bool Defending;
        public bool DontAct;
        public bool DontMove;
        public bool Faith;
        public static string[] FieldNames = new string[] {
            "NoEffect","Crystal","Dead","Undead","Charging","Jump","Defending","Performing",
            "Petrify","Invite","Darkness","Confusion","Silence","BloodSuck","DarkEvilLooking","Treasure",
            "Oil","Float","Reraise","Transparent","Berserk","Chicken","Frog","Critical",
            "Poison","Regen","Protect","Shell","Haste","Slow","Stop","Wall",
            "Faith","Innocent","Charm","Sleep","DontMove","DontAct","Reflect","DeathSentence" };
        public bool Float;
        public bool Frog;
        public bool Haste;
        public bool Innocent;
        public bool Invite;
        public bool Jump;
        public bool NoEffect;
        public bool Oil;
        public bool Performing;
        public bool Petrify;
        public bool Poison;
        public bool Protect;
        public bool Reflect;
        public bool Regen;
        public bool Reraise;
        public bool Shell;
        public bool Silence;
        public bool Sleep;
        public bool Slow;
        public bool Stop;
        public bool Transparent;
        public bool Treasure;
        public bool Undead;
        public bool Wall;

		#endregion Instance Variables 

		#region Public Properties (3) 

        public Statuses Default { get; private set; }

        public IList<string> DigestableProperties
        {
            get { return FieldNames; }
        }

        public bool HasChanged
        {
            get { return Default != null && !PatcherLib.Utilities.Utilities.CompareArrays( ToByteArray(), Default.ToByteArray() ); }
        }

		#endregion Public Properties 

		#region Constructors (2) 

        public Statuses( IList<byte> bytes )
        {
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[0], ref NoEffect, ref Crystal, ref Dead, ref Undead, ref Charging, ref Jump, ref Defending, ref Performing );
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[1], ref Petrify, ref Invite, ref Darkness, ref Confusion, ref Silence, ref BloodSuck, ref DarkEvilLooking, ref Treasure );
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[2], ref Oil, ref Float, ref Reraise, ref Transparent, ref Berserk, ref Chicken, ref Frog, ref Critical );
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[3], ref Poison, ref Regen, ref Protect, ref Shell, ref Haste, ref Slow, ref Stop, ref Wall );
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[4], ref Faith, ref Innocent, ref Charm, ref Sleep, ref DontMove, ref DontAct, ref Reflect, ref DeathSentence );
        }

        public Statuses( IList<byte> bytes, Statuses defaults )
            : this( bytes )
        {
            Default = defaults;
        }

		#endregion Constructors 

		#region Public Methods (7) 

        public static void Copy( Statuses source, Statuses destination )
        {
            destination.NoEffect = source.NoEffect;
            destination.Crystal = source.Crystal;
            destination.Dead = source.Dead;
            destination.Undead = source.Undead;
            destination.Charging = source.Charging;
            destination.Jump = source.Jump;
            destination.Defending = source.Defending;
            destination.Performing = source.Performing;
            destination.Petrify = source.Petrify;
            destination.Invite = source.Invite;
            destination.Darkness = source.Darkness;
            destination.Confusion = source.Confusion;
            destination.Silence = source.Silence;
            destination.BloodSuck = source.BloodSuck;
            destination.DarkEvilLooking = source.DarkEvilLooking;
            destination.Treasure = source.Treasure;
            destination.Oil = source.Oil;
            destination.Float = source.Float;
            destination.Reraise = source.Reraise;
            destination.Transparent = source.Transparent;
            destination.Berserk = source.Berserk;
            destination.Chicken = source.Chicken;
            destination.Frog = source.Frog;
            destination.Critical = source.Critical;
            destination.Poison = source.Poison;
            destination.Regen = source.Regen;
            destination.Protect = source.Protect;
            destination.Shell = source.Shell;
            destination.Haste = source.Haste;
            destination.Slow = source.Slow;
            destination.Stop = source.Stop;
            destination.Wall = source.Wall;
            destination.Faith = source.Faith;
            destination.Innocent = source.Innocent;
            destination.Charm = source.Charm;
            destination.Sleep = source.Sleep;
            destination.DontMove = source.DontMove;
            destination.DontAct = source.DontAct;
            destination.Reflect = source.Reflect;
            destination.DeathSentence = source.DeathSentence;
       }

        public void CopyTo( Statuses destination )
        {
            Copy( this, destination );
        }

        public override bool Equals( object obj )
        {
            return obj is Statuses && PatcherLib.Utilities.Utilities.CompareArrays( ToByteArray(), ( obj as Statuses ).ToByteArray() );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool[] ToBoolArray()
        {
            return new bool[40] { 
                NoEffect, Crystal, Dead, Undead, Charging, Jump, Defending, Performing,
                Petrify, Invite, Darkness, Confusion, Silence, BloodSuck, DarkEvilLooking, Treasure,
                Oil, Float, Reraise, Transparent, Berserk, Chicken, Frog, Critical,
                Poison, Regen, Protect, Shell, Haste, Slow, Stop, Wall,
                Faith, Innocent, Charm, Sleep, DontMove, DontAct, Reflect, DeathSentence };
        }

        public byte[] ToByteArray()
        {
            byte[] result = new byte[5];
            result[0] = PatcherLib.Utilities.Utilities.ByteFromBooleans( NoEffect, Crystal, Dead, Undead, Charging, Jump, Defending, Performing );
            result[1] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Petrify, Invite, Darkness, Confusion, Silence, BloodSuck, DarkEvilLooking, Treasure );
            result[2] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Oil, Float, Reraise, Transparent, Berserk, Chicken, Frog, Critical );
            result[3] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Poison, Regen, Protect, Shell, Haste, Slow, Stop, Wall );
            result[4] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Faith, Innocent, Charm, Sleep, DontMove, DontAct, Reflect, DeathSentence );
            return result;
        }

        public override string ToString()
        {
            List<string> strings = new List<string>( 40 );
            foreach( string name in FieldNames )
            {
                if( ReflectionHelpers.GetFieldOrProperty<bool>( this, name ) )
                {
                    strings.Add( name );
                }
            }

            return string.Join( " | ", strings.ToArray() );
        }

		#endregion Public Methods 
    }
}
