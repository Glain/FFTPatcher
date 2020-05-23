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
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    public enum PreRequisiteJob
    {
        Base = 0x00,
        Chemist = 0x01,
        Knight = 0x02,
        Archer = 0x03,
        Monk = 0x04,

        [PSXDescription( "Priest" )]
        [PSPDescription( "White Mage" )]
        WhiteMage = 0x05,

        [PSXDescription( "Wizard" )]
        [PSPDescription( "Black Mage" )]
        BlackMage = 0x06,

        [PSXDescription( "Time Mage" )]
        [PSPDescription( "Time Mage" )]
        TimeMage = 0x07,
        Summoner = 0x08,
        Thief = 0x09,

        [PSXDescription( "Mediator" )]
        [PSPDescription( "Orator" )]
        Mediator = 0x0A,

        [PSXDescription( "Oracle" )]
        [PSPDescription( "Mystic" )]
        Oracle = 0x0B,
        Geomancer = 0x0C,

        [PSXDescription( "Lancer" )]
        [PSPDescription( "Dragoon" )]
        Lancer = 0x0D,
        Samurai = 0x0E,
        Ninja = 0x0F,

        [PSXDescription( "Calculator" )]
        [PSPDescription( "Arithmetician" )]
        Calculator = 0x10,
        Bard = 0x11,
        Dancer = 0x12,
        Mime = 0x13,

        [PSXDescription( "" )]
        [PSPDescription( "Dark Knight" )]
        DarkKnight = 0x14,

        [PSXDescription( "" )]
        [PSPDescription( "Onion Knight" )]
        OnionKnight = 0x15,
        Unknown = 0xA9,
    }

    public enum Facing
    {
        South = 0,
        West = 1,
        North = 2,
        East = 3,
        //Unknown0x80 = 128,
        //Unknown0x81 = 129,
        //Unknown0x82 = 130,
        //Unknown0x83 = 131,
        Unknown0x30 = 48,
        Unknown0x33 = 51,
    }

    public enum TeamColor
    {
        Blue = 0,
        Red = 1,
        LightBlue = 2,
        Green = 3
    }

    /// <summary>
    /// Represents a unit that participates in an <see cref="Event"/>.
    /// </summary>
    public class EventUnit : IEquatable<EventUnit>, PatcherLib.Datatypes.IChangeable, ISupportDigest, ISupportDefault<EventUnit>
    {
		#region Instance Variables (33) 

        public bool AlwaysPresent;
        public bool Blank2;
        public bool Blank6;
        public bool Blank7;
        public bool Control;
        private static readonly string[] digestableProperties = new string[] {
            "SpriteSet", "SpecialName", "Month", "Day", "Job", "Level", "Faith", "Bravery", "Palette", "UnitID",
            "X", "Y", "PrerequisiteJob", "PrerequisiteJobLevel", "FacingDirection", "UpperLevel", "TeamColor", "Target", 
            "SkillSet", "SecondaryAction", "Reaction", "Support", "Movement", "RightHand", "LeftHand", "Head",
            "Body", "Accessory", "BonusMoney", "WarTrophy", "Male", "Female", "Monster", "JoinAfterEvent",
            "LoadFormation", "ZodiacMonster", "Blank2", "SaveFormation", "AlwaysPresent", "RandomlyPresent",
            "Control", "Immortal", "Blank6", "Blank7", "Experience", "TargetX", "TargetY", "Unknown8", 
            "Unknown10", "Unknown11", "Unknown12" };
        public bool Female;
        public static readonly string[] Flags1FieldNames = new string[] { 
            "Male", "Female", "Monster", "JoinAfterEvent", "LoadFormation", "ZodiacMonster", "Blank2", "SaveFormation" };
        public static readonly string[] Flags2FieldNames = new string[] { 
            "AlwaysPresent", "RandomlyPresent", "Control", "Immortal", "Blank6", "Blank7" };

        public static readonly string[] AIFlags1FieldNames = new string[] {
            "Blank8", "FocusUnit", "StayNearXY", "Aggressive", "Defensive", "Blank9", "Blank10", "Blank11"
        };
        public static readonly string[] AIFlags2FieldNames = new string[] {
            "Blank12", "Blank13", "Blank14", "Blank15", "Blank16", "SaveCT", "Blank17", "Blank18"
        };

        public bool Immortal;
        public bool JoinAfterEvent;
        public bool LoadFormation;
        public bool Male;
        public bool Monster;
        public bool RandomlyPresent;
        public bool SaveFormation;
        public bool ZodiacMonster;

        public bool Blank8;
        public bool FocusUnit;
        public bool StayNearXY;
        public bool Aggressive;
        public bool Defensive;
        public bool Blank9;
        public bool Blank10;
        public bool Blank11;

        public bool Blank12;
        public bool Blank13;
        public bool Blank14;
        public bool Blank15;
        public bool Blank16;
        public bool SaveCT;
        public bool Blank17;
        public bool Blank18;

		#endregion Instance Variables 

		#region Public Properties (41) 

        public Item Accessory { get; set; }

        public Item Body { get; set; }

        public byte BonusMoney { get; set; }

        public byte Bravery { get; set; }

        public byte Day { get; set; }

        public EventUnit Default { get; private set; }

        public string Description
        {
            get { return string.Format( "Sprite: {0} | Name: {1} | Job: {2}", SpriteSet.Name, SpecialName.Name, Job.Name ); }
        }

        public IList<string> DigestableProperties
        {
            get { return digestableProperties; }
        }

        public Facing FacingDirection { get; set; }

        public byte Faith { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public bool HasChanged
        {
            get { return Default != null && !Default.Equals( this ); }
        }

        public Item Head { get; set; }

        public Job Job { get; set; }

        public Item LeftHand { get; set; }

        public byte Level { get; set; }

        public Month Month { get; set; }

        public Ability Movement { get; set; }

        [Hex]
        public byte Palette { get; set; }

        public PreRequisiteJob PrerequisiteJob { get; set; }

        public byte PrerequisiteJobLevel { get; set; }

        public Ability Reaction { get; set; }

        public Item RightHand { get; set; }

        public SkillSet SecondaryAction { get; set; }

        public SkillSet SkillSet { get; set; }

        public SpecialName SpecialName { get; set; }

        public SpriteSet SpriteSet { get; set; }

        public Ability Support { get; set; }

        [Hex]
        public byte Target { get; set; }

        public TeamColor TeamColor { get; set; }

        [Hex]
        public byte UnitID { get; set; }

        [Hex]
        public byte Unknown10 { get; set; }

        //[Hex]
        //public byte Unknown11 { get; set; }

        [Hex]
        public byte Unknown12 { get; set; }

        public byte Experience { get; set; }

        [Hex]
        public byte TargetX { get; set; }

        [Hex]
        public byte TargetY { get; set; }

        //[Hex]
        //public byte Unknown8 { get; set; }

        public bool UpperLevel { get; set; }

        public Item WarTrophy { get; set; }

        public byte X { get; set; }

        public byte Y { get; set; }

		#endregion Public Properties 

		#region Constructors (2) 

        public EventUnit(IList<byte> bytes, PatcherLib.Datatypes.Context context)
            : this( bytes, null, context )
        {
        }

        public EventUnit( IList<byte> bytes, EventUnit defaults, PatcherLib.Datatypes.Context context )
        {
            List<Item> eventItems = Item.GetEventItems(context);
            Ability[] eventAbilities = AllAbilities.GetEventAbilities(context);

            SpriteSet = SpriteSet.GetSpriteSets(context)[bytes[0]];
            Default = defaults;
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[1], ref Male, ref Female, ref Monster, ref JoinAfterEvent, ref LoadFormation, ref ZodiacMonster, ref Blank2, ref SaveFormation );
            SpecialName = SpecialName.GetSpecialNames(context)[bytes[2]];
            Level = bytes[3];
            Month = (Month)bytes[4];
            Day = bytes[5];
            Bravery = bytes[6];
            Faith = bytes[7];
            PrerequisiteJob = (PreRequisiteJob)bytes[8];
            PrerequisiteJobLevel = bytes[9];
            Job = AllJobs.GetDummyJobs(context)[bytes[10]];
            SecondaryAction = SkillSet.GetEventSkillSets(context)[bytes[11]];
            Reaction = eventAbilities[PatcherLib.Utilities.Utilities.BytesToUShort( bytes[12], bytes[13] )];
            Support = eventAbilities[PatcherLib.Utilities.Utilities.BytesToUShort( bytes[14], bytes[15] )];
            Movement = eventAbilities[PatcherLib.Utilities.Utilities.BytesToUShort( bytes[16], bytes[17] )];
            Head = eventItems[bytes[18]];
            Body = eventItems[bytes[19]];
            Accessory = eventItems[bytes[20]];
            RightHand = eventItems[bytes[21]];
            LeftHand = eventItems[bytes[22]];
            Palette = bytes[23];
            bool dummy = false;
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[24], ref AlwaysPresent, ref RandomlyPresent, ref dummy, ref dummy, ref Control, ref Immortal, ref Blank6, ref Blank7 );
            TeamColor = (TeamColor)((bytes[24] & 0x30) >> 4);
            X = bytes[25];
            Y = bytes[26];
            FacingDirection = (Facing)(bytes[27] & 0x7F);
            UpperLevel = (bytes[27] & 0x80) == 0x80;
            Experience = bytes[28];
            SkillSet = SkillSet.GetEventSkillSets(context)[bytes[29]];
            WarTrophy = eventItems[bytes[30]];
            BonusMoney = bytes[31];
            UnitID = bytes[32];
            TargetX = bytes[33];
            TargetY = bytes[34];
            
            //Unknown8 = bytes[35];
            PatcherLib.Utilities.Utilities.CopyByteToBooleans(bytes[35], ref Blank8, ref FocusUnit, ref StayNearXY, ref Aggressive, ref Defensive, ref Blank9, ref Blank10, ref Blank11);

            Target = bytes[36];
            Unknown10 = bytes[37];
            
            //Unknown11 = bytes[38];
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[38], ref Blank12, ref Blank13, ref Blank14, ref Blank15, ref Blank16, ref SaveCT, ref Blank17, ref Blank18 );
            
            Unknown12 = bytes[39];
        }

		#endregion Constructors 

		#region Public Methods (5) 

        public static void Copy( EventUnit source, EventUnit destination )
        {
            destination.SpriteSet = source.SpriteSet;
            destination.Male = source.Male;
            destination.Female = source.Female;
            destination.Monster = source.Monster;
            destination.JoinAfterEvent = source.JoinAfterEvent;
            destination.LoadFormation = source.LoadFormation;
            destination.ZodiacMonster = source.ZodiacMonster;
            destination.Blank2 = source.Blank2;
            destination.SaveFormation = source.SaveFormation;
            destination.SpecialName = source.SpecialName;
            destination.Level = source.Level;
            destination.Month = source.Month;
            destination.Day = source.Day;
            destination.Bravery = source.Bravery;
            destination.Faith = source.Faith;
            destination.PrerequisiteJob = source.PrerequisiteJob;
            destination.PrerequisiteJobLevel = source.PrerequisiteJobLevel;
            destination.Job = source.Job;
            destination.SecondaryAction = source.SecondaryAction;
            destination.Reaction = source.Reaction;
            destination.Support = source.Support;
            destination.Movement = source.Movement;
            destination.Head = source.Head;
            destination.Body = source.Body;
            destination.Accessory = source.Accessory;
            destination.RightHand = source.RightHand;
            destination.LeftHand = source.LeftHand;
            destination.Palette = source.Palette;
            destination.AlwaysPresent = source.AlwaysPresent;
            destination.RandomlyPresent = source.RandomlyPresent;
            destination.Control = source.Control;
            destination.Immortal = source.Immortal;
            destination.Blank6 = source.Blank6;
            destination.Blank7 = source.Blank7;
            destination.TeamColor = source.TeamColor;
            destination.X = source.X;
            destination.Y = source.Y;
            destination.FacingDirection = source.FacingDirection;
            destination.UpperLevel = source.UpperLevel;
            destination.Experience = source.Experience;
            destination.SkillSet = source.SkillSet;
            destination.WarTrophy = source.WarTrophy;
            destination.BonusMoney = source.BonusMoney;
            destination.UnitID = source.UnitID;
            destination.TargetX = source.TargetX;
            destination.TargetY = source.TargetY;
            //destination.Unknown8 = source.Unknown8;
            destination.Target = source.Target;
            destination.Unknown10 = source.Unknown10;
            //destination.Unknown11 = source.Unknown11;
            destination.Unknown12 = source.Unknown12;

            destination.Blank8 = source.Blank8;
            destination.FocusUnit = source.FocusUnit;
            destination.StayNearXY = source.StayNearXY;
            destination.Aggressive = source.Aggressive;
            destination.Defensive = source.Defensive;
            destination.Blank9 = source.Blank9;
            destination.Blank10 = source.Blank10;
            destination.Blank11 = source.Blank11;

            destination.Blank12 = source.Blank12;
            destination.Blank13 = source.Blank13;
            destination.Blank14 = source.Blank14;
            destination.Blank15 = source.Blank15;
            destination.Blank16 = source.Blank16;
            destination.SaveCT = source.SaveCT;
            destination.Blank17 = source.Blank17;
            destination.Blank18 = source.Blank18;
        }

        public void CopyTo( EventUnit destination )
        {
            Copy( this, destination );
        }

        public bool Equals( EventUnit other )
        {
            return PatcherLib.Utilities.Utilities.CompareArrays( other.ToByteArray(), this.ToByteArray() );
        }

        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>( 40 );
            result.Add( SpriteSet.ToByte() );
            result.Add( PatcherLib.Utilities.Utilities.ByteFromBooleans( Male, Female, Monster, JoinAfterEvent, LoadFormation, ZodiacMonster, Blank2, SaveFormation ) );
            result.Add( SpecialName.ToByte() );
            result.Add( Level );
            result.Add( (byte)Month );
            result.Add( Day );
            result.Add( Bravery );
            result.Add( Faith );
            result.Add( (byte)PrerequisiteJob );
            result.Add( PrerequisiteJobLevel );
            result.Add( Job.Value );
            result.Add( SecondaryAction.Value );
            result.AddRange( Reaction.Offset.ToBytes() );
            result.AddRange( Support.Offset.ToBytes() );
            result.AddRange( Movement.Offset.ToBytes() );
            result.Add( (byte)(Head.Offset & 0xFF) );
            result.Add( (byte)(Body.Offset & 0xFF) );
            result.Add( (byte)(Accessory.Offset & 0xFF) );
            result.Add( (byte)(RightHand.Offset & 0xFF) );
            result.Add( (byte)(LeftHand.Offset & 0xFF) );
            result.Add( Palette );
            result.Add( PatcherLib.Utilities.Utilities.ByteFromBooleans( AlwaysPresent, RandomlyPresent, ( ( (int)TeamColor ) & 0x02 ) == 2, ( ( (int)TeamColor ) & 0x01 ) == 1, Control, Immortal, Blank6, Blank7 ) );
            result.Add( X );
            result.Add( Y );
            result.Add( (byte)(((byte)FacingDirection & 0x7F) | (UpperLevel ? 0x80 : 0x00)) );
            result.Add( Experience );
            result.Add( SkillSet.Value );
            result.Add( (byte)(WarTrophy.Offset & 0xFF) );
            result.Add( BonusMoney );
            result.Add( UnitID );
            result.Add( TargetX );
            result.Add( TargetY );
            
            //result.Add( Unknown8 );
            result.Add(PatcherLib.Utilities.Utilities.ByteFromBooleans(Blank8, FocusUnit, StayNearXY, Aggressive, Defensive, Blank9, Blank10, Blank11));
            
            result.Add( Target );
            result.Add( Unknown10 );
            
            //result.Add( Unknown11 );
            result.Add(PatcherLib.Utilities.Utilities.ByteFromBooleans(Blank12, Blank13, Blank14, Blank15, Blank16, SaveCT, Blank17, Blank18));
            
            result.Add( Unknown12 );

            return result.ToArray();
        }

        public override string ToString()
        {
            return Description;
        }

		#endregion Public Methods 
    }
}
