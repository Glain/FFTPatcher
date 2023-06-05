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
using System.Xml.Serialization;

namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// Represents an <see cref="Ability"/>'s attributes.
    /// </summary>
    public class AbilityAttributes : BaseDataType, PatcherLib.Datatypes.IChangeable, ISupportDigest, ISupportDefault<AbilityAttributes>
    {
		#region Instance Variables

        private PatcherLib.Datatypes.Context ourContext = PatcherLib.Datatypes.Context.Default;

        public bool AnimateMiss;
        public bool Arithmetick;
        public bool Auto;
        public bool ForceSelfTarget;
        public bool Blank7;
        public bool TopDownTarget;
        public bool CounterFlood;
        public bool CounterMagic;
        
        private AbilityAttributes defaults;
        public bool Direct;
        public byte Effect;
        public bool Evadeable;
        public bool FollowTarget;
        public bool HitAllies;
        public bool HitCaster;
        public bool HitEnemies;
        [Hex]
        public byte InflictStatus;
        public bool LinearAttack;
        public bool Mimic;
        public byte MPCost;
        public bool NormalAttack;
        public bool Perservere;
        public bool RandomFire;
        public byte Range;
        public bool Reflect;
        public bool RequiresMateriaBlade;
        public bool RequiresSword;
        public bool Shirahadori;
        public bool ShowQuote;
        public bool Silence;
        public bool Targeting;
        public bool TargetSelf;
        public byte Ticks;
        public bool ThreeDirections;

        public byte OldInflictStatus;

        private static class Strings
        {
            public const string AnimateMiss = "AnimateMiss";
            public const string Arithmetick = "Arithmetick";
            public const string Auto = "Auto";
            public const string ForceSelfTarget = "Blank6";
            public const string Blank7 = "Blank7";
            public const string TopDownTarget = "TopDownTarget";
            public const string CounterFlood = "CounterFlood";
            public const string CounterMagic = "CounterMagic";
            public const string Ticks = "Ticks";
            public const string Direct = "Direct";
            public const string Effect = "Effect";
            public const string Evadeable = "Evadeable";
            public const string FollowTarget = "FollowTarget";
            public const string HitAllies = "HitAllies";
            public const string HitCaster = "HitCaster";
            public const string HitEnemies = "HitEnemies";
            public const string InflictStatus = "InflictStatus";
            public const string LinearAttack = "LinearAttack";
            public const string Mimic = "Mimic";
            public const string MPCost = "MPCost";
            public const string NormalAttack = "NormalAttack";
            public const string Perservere = "Perservere";
            public const string RandomFire = "RandomFire";
            public const string Range = "Range";
            public const string Reflect = "Reflect";
            public const string RequiresMateriaBlade = "RequiresMateriaBlade";
            public const string RequiresSword = "RequiresSword";
            public const string Shirahadori = "Shirahadori";
            public const string ShowQuote = "ShowQuote";
            public const string Silence = "Silence";
            public const string Targeting = "Targeting";
            public const string TargetSelf = "TargetSelf";
            public const string ThreeDirections = "ThreeDirections";
            public const string Vertical = "Vertical";
            public const string VerticalFixed = "VerticalFixed";
            public const string VerticalTolerance = "VerticalTolerance";
            public const string WeaponRange = "WeaponRange";
            public const string WeaponStrike = "WeaponStrike";
            public const string X = "X";
            public const string Y = "Y";
            public const string Elements = "Elements";
            public const string Formula = "Formula";
        }
        private static readonly string[] valuesToSerialize = new string[] {
            Strings.AnimateMiss,Strings.Arithmetick,Strings.Auto,Strings.ForceSelfTarget,Strings.Blank7,Strings.TopDownTarget,Strings.CounterFlood,Strings.CounterMagic,
            Strings.Ticks,Strings.Direct,Strings.Effect,Strings.Evadeable,Strings.FollowTarget,Strings.HitAllies,Strings.HitCaster,Strings.HitEnemies,
            Strings.InflictStatus,Strings.LinearAttack,Strings.Mimic,Strings.MPCost,Strings.NormalAttack,Strings.Perservere,Strings.RandomFire,
            Strings.Range,Strings.Reflect,Strings.RequiresMateriaBlade,Strings.RequiresSword,Strings.Shirahadori,Strings.ShowQuote,Strings.Silence,
            Strings.Targeting,Strings.TargetSelf,Strings.ThreeDirections,Strings.Vertical,Strings.VerticalFixed,Strings.VerticalTolerance,
            Strings.WeaponRange,Strings.WeaponStrike,Strings.X,Strings.Y, Strings.Elements, Strings.Formula };
        public byte Vertical;
        public bool VerticalFixed;
        public bool VerticalTolerance;
        public bool WeaponRange;
        public bool WeaponStrike;
        public byte X;
        public byte Y;

		#endregion Instance Variables 

		#region Public Properties (7) 

        public AbilityAttributes Default
        {
            get { return defaults; }
            set
            {
                defaults = value;
                if( defaults != null )
                {
                    Elements.Default = defaults.Elements;
                }
            }
        }

        public IList<string> DigestableProperties
        {
            get { return valuesToSerialize; }
        }

        public Elements Elements { get; private set; }

        public AbilityFormula Formula { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        public bool HasChanged
        {
            get
            {
                return Default != null && !PatcherLib.Utilities.Utilities.CompareArrays( ToByteArray(), Default.ToByteArray() );
            }
        }

        public string Name { get; private set; }

        public UInt16 Offset { get; private set; }

		#endregion Public Properties 

		#region Constructors

        public AbilityAttributes( string name, UInt16 offset, IList<byte> second, PatcherLib.Datatypes.Context context )
        {
            Name = name;
            Offset = offset;
            ourContext = context;

            Range = second[0];
            Effect = second[1];
            Vertical = second[2];
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( second[3],
                ref ForceSelfTarget, ref Blank7, ref WeaponRange, ref VerticalFixed, ref VerticalTolerance, ref WeaponStrike, ref Auto, ref TargetSelf);
            TargetSelf = !TargetSelf;
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( second[4],
                ref HitEnemies, ref HitAllies, ref TopDownTarget, ref FollowTarget, ref RandomFire, ref LinearAttack, ref ThreeDirections, ref HitCaster);
            HitEnemies = !HitEnemies;
            FollowTarget = !FollowTarget;
            HitAllies = !HitAllies;
            HitCaster = !HitCaster;
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( second[5],
                ref Reflect, ref Arithmetick, ref Silence, ref Mimic, ref NormalAttack, ref Perservere, ref ShowQuote, ref AnimateMiss );
            Silence = !Silence;
            Mimic = !Mimic;
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( second[6],
                ref CounterFlood, ref CounterMagic, ref Direct, ref Shirahadori, ref RequiresSword, ref RequiresMateriaBlade, ref Evadeable, ref Targeting );
            Targeting = !Targeting;
            Elements = new Elements( second[7] );

            byte b = second[8];
            Formula = AbilityFormula.GetAbilityFormulaHash(context)[second[8]];
            X = second[9];
            Y = second[10];
            InflictStatus = second[11];
            Ticks = second[12];
            MPCost = second[13];

            OldInflictStatus = InflictStatus;
        }

        public AbilityAttributes( string name, UInt16 offset, IList<byte> second, AbilityAttributes defaults, PatcherLib.Datatypes.Context context )
            : this( name, offset, second, context )
        {
            if( defaults != null )
            {
                Default = defaults;
                Elements.Default = defaults.Elements;
            }
        }

        internal AbilityAttributes(PatcherLib.Datatypes.Context context)
        {
            ourContext = context;
        }

		#endregion Constructors 

		#region Public Methods (4) 

        public static void Copy( AbilityAttributes source, AbilityAttributes destination )
        {
            destination.Range = source.Range;
            destination.Effect = source.Effect;
            destination.Vertical = source.Vertical;
            source.Elements.CopyTo( destination.Elements );
            destination.Formula = source.Formula;
            destination.X = source.X;
            destination.Y = source.Y;
            destination.InflictStatus = source.InflictStatus;
            destination.Ticks = source.Ticks;
            destination.MPCost = source.MPCost;
            destination.ForceSelfTarget = source.ForceSelfTarget;
            destination.Blank7 = source.Blank7;
            destination.WeaponRange = source.WeaponRange;
            destination.VerticalFixed = source.VerticalFixed;
            destination.VerticalTolerance = source.VerticalTolerance;
            destination.WeaponStrike = source.WeaponStrike;
            destination.Auto = source.Auto;
            destination.TargetSelf = source.TargetSelf;

            destination.HitEnemies = source.HitEnemies;
            destination.HitAllies = source.HitAllies;
            destination.TopDownTarget = source.TopDownTarget;
            destination.FollowTarget = source.FollowTarget;
            destination.RandomFire = source.RandomFire;
            destination.LinearAttack = source.LinearAttack;
            destination.ThreeDirections = source.ThreeDirections;
            destination.HitCaster = source.HitCaster;

            destination.Reflect = source.Reflect;
            destination.Arithmetick = source.Arithmetick;
            destination.Silence = source.Silence;
            destination.Mimic = source.Mimic;
            destination.NormalAttack = source.NormalAttack;
            destination.Perservere = source.Perservere;
            destination.ShowQuote = source.ShowQuote;
            destination.AnimateMiss = source.AnimateMiss;

            destination.CounterFlood = source.CounterFlood;
            destination.CounterMagic = source.CounterMagic;
            destination.Direct = source.Direct;
            destination.Shirahadori = source.Shirahadori;
            destination.RequiresSword = source.RequiresSword;
            destination.RequiresMateriaBlade = source.RequiresMateriaBlade;
            destination.Evadeable = source.Evadeable;
            destination.Targeting = source.Targeting;

            destination.OldInflictStatus = source.OldInflictStatus;
        }

        public void CopyTo( AbilityAttributes destination )
        {
            Copy( this, destination );
        }

        public bool[] ToBoolArray()
        {
            return new bool[32] { 
                ForceSelfTarget, Blank7, WeaponRange, VerticalFixed, VerticalTolerance, WeaponStrike, Auto, TargetSelf,
                HitEnemies, HitAllies, TopDownTarget, FollowTarget, RandomFire, LinearAttack, ThreeDirections, HitCaster,
                Reflect, Arithmetick, Silence, Mimic, NormalAttack, Perservere, ShowQuote, AnimateMiss,
                CounterFlood, CounterMagic, Direct, Shirahadori, RequiresSword, RequiresMateriaBlade,Evadeable, Targeting };
        }

        public byte[] ToByteArray()
        {
            byte[] result = new byte[14];
            result[0] = Range;
            result[1] = Effect;
            result[2] = Vertical;
            result[3] = PatcherLib.Utilities.Utilities.ByteFromBooleans(ForceSelfTarget, Blank7, WeaponRange, VerticalFixed, VerticalTolerance, WeaponStrike, Auto, !TargetSelf);
            result[4] = PatcherLib.Utilities.Utilities.ByteFromBooleans(!HitEnemies, !HitAllies, TopDownTarget, !FollowTarget, RandomFire, LinearAttack, ThreeDirections, !HitCaster);
            result[5] = PatcherLib.Utilities.Utilities.ByteFromBooleans( Reflect, Arithmetick, !Silence, !Mimic, NormalAttack, Perservere, ShowQuote, AnimateMiss );
            result[6] = PatcherLib.Utilities.Utilities.ByteFromBooleans( CounterFlood, CounterMagic, Direct, Shirahadori, RequiresSword, RequiresMateriaBlade, Evadeable, !Targeting );
            result[7] = Elements.ToByte();
            result[8] = Formula.Value;
            result[9] = X;
            result[10] = Y;
            result[11] = InflictStatus;
            result[12] = Ticks;
            result[13] = MPCost;

            return result;
        }

		#endregion Public Methods 
    
        protected override void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();

            Range = (byte)reader.ReadElementContentAsInt();
            Effect = (byte)reader.ReadElementContentAsInt();
            Vertical = (byte)reader.ReadElementContentAsInt();
            InflictStatus = (byte)reader.ReadElementContentAsInt();
            Ticks = (byte)reader.ReadElementContentAsInt();
            MPCost = (byte)reader.ReadElementContentAsInt();

            Elements = new Elements();
            ( (IXmlSerializable)Elements ).ReadXml( reader );

            reader.MoveToAttribute( "value" );
            byte formulaValue = (byte)reader.ReadContentAsInt();
            Formula = (ourContext == PatcherLib.Datatypes.Context.US_PSP) ? 
                AbilityFormula.PSPAbilityFormulaHash[formulaValue] : 
                AbilityFormula.PSXAbilityFormulaHash[formulaValue];
            reader.MoveToElement();
            reader.ReadStartElement();
            reader.ReadEndElement();

            ForceSelfTarget = reader.ReadElementContentAsBoolean();
            Blank7 = reader.ReadElementContentAsBoolean();
            WeaponRange = reader.ReadElementContentAsBoolean();
            VerticalFixed = reader.ReadElementContentAsBoolean();
            VerticalTolerance = reader.ReadElementContentAsBoolean();
            WeaponStrike = reader.ReadElementContentAsBoolean();
            Auto = reader.ReadElementContentAsBoolean();
            TargetSelf = reader.ReadElementContentAsBoolean();

            HitEnemies = reader.ReadElementContentAsBoolean();
            HitAllies = reader.ReadElementContentAsBoolean();
            TopDownTarget = reader.ReadElementContentAsBoolean();
            FollowTarget = reader.ReadElementContentAsBoolean();
            RandomFire = reader.ReadElementContentAsBoolean();
            LinearAttack = reader.ReadElementContentAsBoolean();
            ThreeDirections = reader.ReadElementContentAsBoolean();
            HitCaster = reader.ReadElementContentAsBoolean();

            Reflect = reader.ReadElementContentAsBoolean();
            Arithmetick = reader.ReadElementContentAsBoolean();
            Silence = reader.ReadElementContentAsBoolean();
            Mimic = reader.ReadElementContentAsBoolean();
            NormalAttack = reader.ReadElementContentAsBoolean();
            Perservere = reader.ReadElementContentAsBoolean();
            ShowQuote = reader.ReadElementContentAsBoolean();
            AnimateMiss = reader.ReadElementContentAsBoolean();

            CounterFlood = reader.ReadElementContentAsBoolean();
            CounterMagic = reader.ReadElementContentAsBoolean();
            Direct = reader.ReadElementContentAsBoolean();
            Shirahadori = reader.ReadElementContentAsBoolean();
            RequiresSword = reader.ReadElementContentAsBoolean();
            RequiresMateriaBlade = reader.ReadElementContentAsBoolean();
            Evadeable = reader.ReadElementContentAsBoolean();
            Targeting = reader.ReadElementContentAsBoolean();

            reader.ReadEndElement();
        }

        protected override void WriteXml( System.Xml.XmlWriter writer )
        {
            writer.WriteValueElement( Strings.Range, Range );
            writer.WriteValueElement( Strings.Effect, Effect );
            writer.WriteValueElement( Strings.Vertical, Vertical );
            writer.WriteValueElement( Strings.InflictStatus, InflictStatus );
            writer.WriteValueElement( Strings.Ticks, Ticks );
            writer.WriteValueElement( Strings.MPCost, MPCost );

            writer.WriteStartElement( Strings.Elements );
            ((IXmlSerializable)Elements).WriteXml( writer );
            writer.WriteEndElement();

            writer.WriteStartElement( Strings.Formula );
            writer.WriteStartAttribute( "value" );
            writer.WriteValue( Formula.Value );
            writer.WriteEndAttribute();

            writer.WriteStartAttribute( "name" );
            writer.WriteString( Formula.Formula );
            writer.WriteEndAttribute();
            writer.WriteEndElement();

            writer.WriteValueElement( Strings.ForceSelfTarget, ForceSelfTarget );
            writer.WriteValueElement( Strings.Blank7, Blank7 );
            writer.WriteValueElement( Strings.WeaponRange, WeaponRange );
            writer.WriteValueElement( Strings.VerticalFixed, VerticalFixed );
            writer.WriteValueElement( Strings.VerticalTolerance, VerticalTolerance );
            writer.WriteValueElement( Strings.WeaponStrike, WeaponStrike );
            writer.WriteValueElement( Strings.Auto, Auto );
            writer.WriteValueElement( Strings.TargetSelf, TargetSelf );

            writer.WriteValueElement( Strings.HitEnemies, HitEnemies );
            writer.WriteValueElement( Strings.HitAllies, HitAllies );
            writer.WriteValueElement( Strings.TopDownTarget, TopDownTarget );
            writer.WriteValueElement( Strings.FollowTarget, FollowTarget );
            writer.WriteValueElement( Strings.RandomFire, RandomFire );
            writer.WriteValueElement( Strings.LinearAttack, LinearAttack );
            writer.WriteValueElement( Strings.ThreeDirections, ThreeDirections );
            writer.WriteValueElement( Strings.HitCaster, HitCaster );

            writer.WriteValueElement( Strings.Reflect, Reflect );
            writer.WriteValueElement( Strings.Arithmetick, Arithmetick );
            writer.WriteValueElement( Strings.Silence, Silence );
            writer.WriteValueElement( Strings.Mimic, Mimic );
            writer.WriteValueElement( Strings.NormalAttack, NormalAttack );
            writer.WriteValueElement( Strings.Perservere, Perservere );
            writer.WriteValueElement( Strings.ShowQuote, ShowQuote );
            writer.WriteValueElement( Strings.AnimateMiss, AnimateMiss );

            writer.WriteValueElement( Strings.CounterFlood, CounterFlood );
            writer.WriteValueElement( Strings.CounterMagic, CounterMagic );
            writer.WriteValueElement( Strings.Direct, Direct );
            writer.WriteValueElement( Strings.Shirahadori, Shirahadori );
            writer.WriteValueElement( Strings.RequiresSword, RequiresSword );
            writer.WriteValueElement( Strings.RequiresMateriaBlade, RequiresMateriaBlade );
            writer.WriteValueElement( Strings.Evadeable, Evadeable );
            writer.WriteValueElement( Strings.Targeting, Targeting );


        }
    }
}
