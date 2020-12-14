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
using System.Xml;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using System.Xml.Serialization;

namespace FFTPatcher.Datatypes
{
    public enum AbilityType
    {
        Blank,
        Normal,
        Item,
        Throwing,
        Jumping,
        Charging,

        [PSXDescription( "Mathematics" )]
        [PSPDescription( "Arithmetick" )]
        Arithmetick,
        Reaction,
        Support,
        Movement,
        Unknown1,
        Unknown2,
        Unknown3,
        Unknown4,
        Unknown5,
        Unknown6
    }

    /// <summary>
    /// Represents an ability and its attributes.
    /// </summary>
    public class Ability : BaseDataType, IChangeable, IXmlDigest, ISupportDigest, ISupportDefault<Ability>
    {
        public static ushort ItemEffectPrefixValue = 0x0800;

		#region Instance Variables

        private Context ourContext = Context.Default;

        private bool displayAbilityName;
        private bool blank1;
        private bool blank2;
        private bool blank3;
        private bool blank4;
        private bool blank5;

        private static class Strings
        {
            public const string JPCost = "JPCost";
            public const string LearnRate = "LearnRate";
            public const string AbilityType = "AbilityType";
            public const string LearnWithJP = "LearnWithJP";
            public const string Action = "Action";

            public const string LearnOnHit = "LearnOnHit";
            public const string Blank1 = "Blank1";
            public const string Unknown1 = "Unknown1";
            public const string Unknown2 = "Unknown2";
            public const string Unknown3 = "Unknown3";

            public const string Blank2 = "Blank2";
            public const string Blank3 = "Blank3";
            public const string Blank4 = "Blank4";
            public const string Blank5 = "Blank5";
            public const string Unknown4 = "Unknown4";
        }
        private static readonly string[] digestableProperties = new string[] {
            Strings.JPCost, Strings.LearnRate, Strings.AbilityType, Strings.LearnWithJP, Strings.Action,
            Strings.LearnOnHit, Strings.Blank1, Strings.Unknown1, Strings.Unknown2, Strings.Unknown3,
            Strings.Blank2, Strings.Blank3, Strings.Blank4, Strings.Blank5, Strings.Unknown4};
        private bool learnOnHit;
        private bool learnWithJP;
        private bool usedByEnemies;
        private bool unknown2;
        private bool unknown3;
        private bool unknown4;

		#endregion Instance Variables 

		#region Public Properties (63) 

        public AbilityType AbilityType { get; set; }

        public bool DisplayAbilityName { get { return displayAbilityName; } set { displayAbilityName = value; } }

        [Obsolete]
        public bool Action { get { return displayAbilityName; } set { displayAbilityName = value; } }

        public bool AIAddStatus { get { return AIFlags.AddStatus; } set { AIFlags.AddStatus = value; } }

        public bool AIBlank { get { return AIFlags.Blank; } set { AIFlags.Blank = value; } }

        public bool AICancelStatus { get { return AIFlags.CancelStatus; } set { AIFlags.CancelStatus = value; } }

        public bool AIUnknown10 { get { return AIFlags.Unknown10; } set { AIFlags.Unknown10 = value; } }

        public bool AIUnknown4 { get { return AIFlags.Unknown4; } set { AIFlags.Unknown4 = value; } }

        public AIFlags AIFlags { get; private set; }

        public bool AIHP { get { return AIFlags.HP; } set { AIFlags.HP = value; } }

        public bool AILineOfSight { get { return AIFlags.LineOfSight; } set { AIFlags.LineOfSight = value; } }

        public bool AIUnknown5 { get { return AIFlags.Unknown5; } set { AIFlags.Unknown5 = value; } }

        public bool AIUnknown9 { get { return AIFlags.Unknown9; } set { AIFlags.Unknown9 = value; } }

        public bool AIMP { get { return AIFlags.MP; } set { AIFlags.MP = value; } }

        public bool AIAllowRandomly { get { return AIFlags.AllowRandomly; } set { AIFlags.AllowRandomly = value; } }

        public bool AIReflectable { get { return AIFlags.Reflectable; } set { AIFlags.Reflectable = value; } }

        public bool AISilence { get { return AIFlags.Silence; } set { AIFlags.Silence = value; } }

        public bool AIStats { get { return AIFlags.Stats; } set { AIFlags.Stats = value; } }

        public bool AITargetAllies { get { return AIFlags.TargetAllies; } set { AIFlags.TargetAllies = value; } }

        public bool AITargetEnemies { get { return AIFlags.TargetEnemies; } set { AIFlags.TargetEnemies = value; } }

        public bool AIUnknown7 { get { return AIFlags.Unknown7; } set { AIFlags.Unknown7 = value; } }

        public bool AIUnknown8 { get { return AIFlags.Unknown8; } set { AIFlags.Unknown8 = value; } }

        public bool AIUndeadReverse { get { return AIFlags.UndeadReverse; } set { AIFlags.UndeadReverse = value; } }

        public bool AIUnequip { get { return AIFlags.Unequip; } set { AIFlags.Unequip = value; } }

        public bool AIUnknown1 { get { return AIFlags.Unknown1; } set { AIFlags.Unknown1 = value; } }

        public bool AIUnknown2 { get { return AIFlags.Unknown2; } set { AIFlags.Unknown2 = value; } }

        public bool AIUnknown3 { get { return AIFlags.Unknown3; } set { AIFlags.Unknown3 = value; } }

        public bool AIUnknown6 { get { return AIFlags.Unknown6; } set { AIFlags.Unknown6 = value; } }

        [Hex]
        public byte ArithmetickSkill { get; set; }

        public AbilityAttributes Attributes { get; private set; }

        public bool Blank1 { get { return blank1; } set { blank1 = value; } }

        public bool Blank2 { get { return blank2; } set { blank2 = value; } }

        public bool Blank3 { get { return blank3; } set { blank3 = value; } }

        public bool Blank4 { get { return blank4; } set { blank4 = value; } }

        public bool Blank5 { get { return blank5; } set { blank5 = value; } }

        public byte ChargeBonus { get; set; }

        public byte ChargeCT { get; set; }

        public Ability Default { get; private set; }

        public IList<string> DigestableProperties
        {
            get { return digestableProperties; }
        }

        public Effect Effect { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        public bool HasChanged
        {
            get
            {
                return
                    (Default != null) &&
                    (AbilityType != Default.AbilityType ||
                    Action != Default.Action ||
                    AIAddStatus != Default.AIAddStatus ||
                    AIBlank != Default.AIBlank ||
                    AICancelStatus != Default.AICancelStatus ||
                    AIUnknown10 != Default.AIUnknown10 ||
                    AIUnknown4 != Default.AIUnknown4 ||
                    AIHP != Default.AIHP ||
                    AILineOfSight != Default.AILineOfSight ||
                    AIUnknown5 != Default.AIUnknown5 ||
                    AIUnknown9 != Default.AIUnknown9 ||
                    AIMP != Default.AIMP ||
                    AIAllowRandomly != Default.AIAllowRandomly ||
                    AIReflectable != Default.AIReflectable ||
                    AISilence != Default.AISilence ||
                    AIStats != Default.AIStats ||
                    AITargetAllies != Default.AITargetAllies ||
                    AITargetEnemies != Default.AITargetEnemies ||
                    AIUnknown7 != Default.AIUnknown7 ||
                    AIUnknown8 != Default.AIUnknown8 ||
                    AIUndeadReverse != Default.AIUndeadReverse ||
                    AIUnequip != Default.AIUnequip ||
                    AIUnknown1 != Default.AIUnknown1 ||
                    AIUnknown2 != Default.AIUnknown2 ||
                    AIUnknown3 != Default.AIUnknown3 ||
                    AIUnknown6 != Default.AIUnknown6 ||
                    Blank1 != Default.Blank1 ||
                    Blank2 != Default.Blank2 ||
                    Blank3 != Default.Blank3 ||
                    Blank4 != Default.Blank4 ||
                    Blank5 != Default.Blank5 ||
                    JPCost != Default.JPCost ||
                    LearnOnHit != Default.LearnOnHit ||
                    LearnRate != Default.LearnRate ||
                    LearnWithJP != Default.LearnWithJP ||
                    Unknown1 != Default.Unknown1 ||
                    Unknown2 != Default.Unknown2 ||
                    Unknown3 != Default.Unknown3 ||
                    Unknown4 != Default.Unknown4 ||
                    (IsArithmetick && ArithmetickSkill != Default.ArithmetickSkill) ||
                    (IsCharging && (ChargeBonus != Default.ChargeBonus || ChargeCT != Default.ChargeCT)) ||
                    (IsItem && ItemOffset != Default.ItemOffset) ||
                    (IsJumping && (JumpHorizontal != Default.JumpHorizontal || JumpVertical != Default.JumpVertical)) ||
                    (IsNormal && Attributes.HasChanged) ||
                    (Effect != null && Default.Effect != null && Effect.Value != Default.Effect.Value) ||
                    (IsOther && OtherID != Default.OtherID) ||
                    (IsThrowing && Throwing != Default.Throwing));
            }
        }

        public bool IsArithmetick { get; private set; }

        public bool IsCharging { get; private set; }

        public bool IsItem { get; private set; }

        public bool IsJumping { get; private set; }

        public bool IsNormal { get; private set; }

        public bool IsOther { get; private set; }

        public bool IsThrowing { get; private set; }

        public Item Item { get; set; }

        public UInt16 ItemOffset
        {
            get
            {
                return (UInt16)(IsItem ? Item.Offset : 0);
            }
        }
        public void SetItemOffset(UInt16 value, Context context)
        {
            Item = Item.GetItemAtOffset(value, context);
        }

        public UInt16 JPCost { get; set; }

        public byte JumpHorizontal { get; set; }

        public byte JumpVertical { get; set; }

        public bool LearnOnHit { get { return learnOnHit; } set { learnOnHit = value; } }

        public byte LearnRate { get; set; }

        public bool LearnWithJP { get { return learnWithJP; } set { learnWithJP = value; } }

        public string Name { get; private set; }

        public UInt16 Offset { get; private set; }

        [Hex]
        public byte OtherID { get; set; }

        public ItemSubType Throwing { get; set; }

        public bool UsedByEnemies { get { return usedByEnemies; } set { usedByEnemies = value; } }

        [Obsolete]
        public bool Unknown1 { get { return usedByEnemies; } set { usedByEnemies = value; } }

        public bool Unknown2 { get { return unknown2; } set { unknown2 = value; } }

        public bool Unknown3 { get { return unknown3; } set { unknown3 = value; } }

        public bool Unknown4 { get { return unknown4; } set { unknown4 = value; } }

		#endregion Public Properties 

		#region Constructors (4) 

        public Ability( string name, UInt16 offset, Context context )
        {
            Name = name;
            Offset = offset;
            ourContext = context;
        }

        private Ability( string name, UInt16 offset, IList<byte> first, Context context )
        {
            Name = name;
            Offset = offset;
            JPCost = PatcherLib.Utilities.Utilities.BytesToUShort( first[0], first[1] );
            LearnRate = first[2];

            bool dummy = false;
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( first[3],
                ref learnWithJP, ref displayAbilityName, ref learnOnHit, ref blank1, ref dummy, ref dummy, ref dummy, ref dummy );
            learnWithJP = !learnWithJP;

            AbilityType = (AbilityType)(first[3] & 0x0F);

            AIFlags = new AIFlags( first.Sub( 4, 6 ) );

            PatcherLib.Utilities.Utilities.CopyByteToBooleans( first[7],
                ref usedByEnemies, ref unknown2, ref unknown3, ref blank2, ref blank3, ref blank4, ref blank5, ref unknown4 );

            ourContext = context;
        }

        public Ability( string name, UInt16 offset, IList<byte> first, IList<byte> second, Context context )
            : this( name, offset, first, context )
        {
            IsNormal = ((offset >= 0x000) && (offset <= 0x16F));
            IsItem = ((offset >= 0x170) && (offset <= 0x17D));
            IsThrowing = ((offset >= 0x17E) && (offset <= 0x189));
            IsJumping = ((offset >= 0x18A) && (offset <= 0x195));
            IsCharging = ((offset >= 0x196) && (offset <= 0x19D));
            IsArithmetick = ((offset >= 0x19E) && (offset <= 0x1A5));
            IsOther = (offset >= 0x1A6);

            if( IsNormal )
            {
                Attributes = new AbilityAttributes( name, offset, second, context);
            }
            if( IsItem )
            {
                //ItemOffset = second[0];
                SetItemOffset(second[0], context);
            }
            if( IsThrowing )
            {
                Throwing = (ItemSubType)second[0];
            }
            if( IsJumping )
            {
                JumpHorizontal = second[0];
                JumpVertical = second[1];
            }
            if( IsCharging )
            {
                ChargeCT = second[0];
                ChargeBonus = second[1];
            }
            if( IsArithmetick )
            {
                ArithmetickSkill = second[0];
            }
            if( IsOther )
            {
                OtherID = second[0];
            }
        }

        public Ability( string name, UInt16 offset, IList<byte> first, IList<byte> second, Ability defaults, Context context )
            : this( name, offset, first, second, context )
        {
            Default = defaults;
            if( IsNormal )
            {
                Attributes.Default = Default.Attributes;
            }
            AIFlags.Default = Default.AIFlags;
        }

		#endregion Constructors 

		#region Public Methods (13) 

        public static void CopyAll( Ability source, Ability destination, Context context )
        {
            CopySpecific( source, destination, context );
            CopyCommon( source, destination );
        }

        public void CopyAllTo( Ability destination, Context context )
        {
            CopyAll( this, destination, context );
        }

        public static void CopyCommon( Ability source, Ability destination )
        {
            destination.JPCost = source.JPCost;
            destination.LearnRate = source.LearnRate;
            destination.LearnWithJP = source.LearnWithJP;
            destination.Action = source.Action;
            destination.LearnOnHit = source.LearnOnHit;
            destination.Blank1 = source.Blank1;
            destination.AbilityType = source.AbilityType;
            destination.Unknown1 = source.Unknown1;
            destination.Unknown2 = source.Unknown2;
            destination.Unknown3 = source.Unknown3;
            destination.Unknown4 = source.Unknown4;
            destination.Blank2 = source.Blank2;
            destination.Blank3 = source.Blank3;
            destination.Blank4 = source.Blank4;
            destination.Blank5 = source.Blank5;
            source.AIFlags.CopyTo( destination.AIFlags );
        }

        public void CopyCommonTo( Ability destination )
        {
            CopyCommon( this, destination );
        }

        public static void CopySpecific( Ability source, Ability destination, Context context )
        {
            if( (source.IsNormal ^ destination.IsNormal) ||
                (source.IsItem ^ destination.IsItem) ||
                (source.IsThrowing ^ destination.IsThrowing) ||
                (source.IsJumping ^ destination.IsJumping) ||
                (source.IsCharging ^ destination.IsCharging) ||
                (source.IsArithmetick ^ destination.IsArithmetick) ||
                (source.IsOther ^ destination.IsOther) )
            {
                throw new InvalidOperationException( "Cannot convert between ability types" );
            }
            if( destination.IsNormal )
            {
                source.Attributes.CopyTo( destination.Attributes );
            }
            if( destination.IsItem )
            {
                //destination.ItemOffset = source.ItemOffset;
                destination.SetItemOffset(source.ItemOffset, context);
            }
            if( destination.IsThrowing )
            {
                destination.Throwing = source.Throwing;
            }
            if( destination.IsJumping )
            {
                destination.JumpHorizontal = source.JumpHorizontal;
                destination.JumpVertical = source.JumpVertical;
            }
            if( destination.IsCharging )
            {
                destination.ChargeCT = source.ChargeCT;
                destination.ChargeBonus = source.ChargeBonus;
            }
            if( destination.IsArithmetick )
            {
                destination.ArithmetickSkill = source.ArithmetickSkill;
            }
            if( destination.IsOther )
            {
                destination.OtherID = source.OtherID;
            }

            if (source.Effect != null && destination.Effect != null)
            {
                destination.Effect = source.Effect;
            }
        }

        public void CopySpecificTo( Ability destination, Context context )
        {
            CopySpecific( this, destination, context );
        }

        public bool[] PropertiesToBoolArray()
        {
            return new bool[12] {
                LearnWithJP, Action, LearnOnHit, Blank1,
                Unknown1, Unknown2, Unknown3, Blank2, Blank3, Blank4, Blank5, Unknown4 };
        }

        public byte[] ToByteArray()
        {
            byte[] result = new byte[8];
            result[0] = (byte)(JPCost & 0xFF);
            result[1] = (byte)(JPCost >> 8);
            result[2] = LearnRate;
            result[3] = PatcherLib.Utilities.Utilities.ByteFromBooleans( !learnWithJP, displayAbilityName, learnOnHit, blank1, false, false, false, false );
            result[3] |= (byte)AbilityType;
            Array.Copy( AIFlags.ToByteArray(), 0, result, 4, 3 );
            result[7] = PatcherLib.Utilities.Utilities.ByteFromBooleans( usedByEnemies, unknown2, unknown3, blank2, blank3, blank4, blank5, unknown4 );

            return result;
        }

        public byte[] ToByteArray( Context context )
        {
            switch( context )
            {
                case Context.US_PSX:
                    return ToByteArray();
                default:
                    return ToByteArray();
            }
        }

        public byte[] ToSecondByteArray( Context context )
        {
            switch( context )
            {
                case Context.US_PSX:
                    return ToSecondByteArray();
                default:
                    return ToSecondByteArray();
            }
        }

        public byte[] ToSecondByteArray()
        {
            if( IsNormal )
                return Attributes.ToByteArray();
            if( IsItem )
                return new byte[] { (byte)(Item.Offset & 0xFF) };
            if( IsThrowing )
                return new byte[] { (byte)Throwing };
            if( IsJumping )
                return new byte[] { JumpHorizontal, JumpVertical };
            if( IsCharging )
                return new byte[] { ChargeCT, ChargeBonus };
            if( IsArithmetick )
                return new byte[] { ArithmetickSkill };
            if( IsOther )
                return new byte[] { OtherID };
            return null;
        }

        public override string ToString()
        {
        	return (HasChanged ? "*" : "") + Offset.ToString( "X4" ) + " " + Name;
        }

        public void WriteXmlDigest(XmlWriter writer, FFTPatch FFTPatch)
        {
            if( HasChanged )
            {
                writer.WriteStartElement( GetType().Name );
                writer.WriteAttributeString( "value", Offset.ToString( "X4" ) );
                writer.WriteAttributeString( "name", Name );
                DigestGenerator.WriteXmlDigest( this, writer, false, false );
                DigestGenerator.WriteXmlDigest( AIFlags, writer, true, true );

                if (Effect != null && Default.Effect != null)
                {
                    DigestGenerator.WriteDigestEntry( writer, "Effect", Default.Effect, Effect );
                }

                if( IsNormal )
                {
                    DigestGenerator.WriteXmlDigest( Attributes, writer, true, true );
                    if( Attributes.Formula.Value == 0x02 &&
                        (Attributes.Formula.Value != Attributes.Default.Formula.Value || Attributes.InflictStatus != Attributes.Default.InflictStatus) )
                    {
                        writer.WriteStartElement( "CastSpell" );
                        writer.WriteAttributeString( "default", AllAbilities.GetNames(FFTPatch.Context)[Attributes.Default.InflictStatus] );
                        writer.WriteAttributeString( "value", AllAbilities.GetNames(FFTPatch.Context)[Attributes.InflictStatus] );
                        writer.WriteEndElement();
                    }
                    else if( Attributes.InflictStatus != Attributes.Default.InflictStatus )
                    {
                        writer.WriteStartElement( "InflictStatusDescription" );
                        writer.WriteAttributeString( "default", FFTPatch.InflictStatuses.InflictStatuses[Attributes.Default.InflictStatus].Statuses.ToString() );
                        writer.WriteAttributeString( "value", FFTPatch.InflictStatuses.InflictStatuses[Attributes.InflictStatus].Statuses.ToString() );
                        writer.WriteEndElement();
                    }
                }
                else if( IsItem )
                {
                    DigestGenerator.WriteDigestEntry( writer, "ItemOffset", Default.ItemOffset, ItemOffset, "0x{0:X2}" );
                }
                else if( IsThrowing )
                {
                    DigestGenerator.WriteDigestEntry( writer, "Throwing", Default.Throwing, Throwing );
                }
                else if( IsJumping )
                {
                    DigestGenerator.WriteDigestEntry( writer, "JumpHorizontal", Default.JumpHorizontal, JumpHorizontal );
                    DigestGenerator.WriteDigestEntry( writer, "JumpVertical", Default.JumpVertical, JumpVertical );
                }
                else if( IsCharging )
                {
                    DigestGenerator.WriteDigestEntry( writer, "ChargeCT", Default.ChargeCT, ChargeCT );
                    DigestGenerator.WriteDigestEntry( writer, "ChargeBonus", Default.ChargeBonus, ChargeBonus );
                }
                else if( IsArithmetick )
                {
                    DigestGenerator.WriteDigestEntry( writer, "ArithmetickSkill", Default.ArithmetickSkill, ArithmetickSkill, "0x{0:X2}" );
                }
                else if( IsOther )
                {
                    DigestGenerator.WriteDigestEntry( writer, "OtherID", Default.OtherID, OtherID, "0x{0:X2}" );
                }

                writer.WriteEndElement();
            }
        }

		#endregion Public Methods 

        protected override void WriteXml( XmlWriter writer )
        {
            writer.WriteAttributeString( "Name", Name );
            writer.WriteStartAttribute( "Offset" );
            writer.WriteValue( Offset );
            writer.WriteEndAttribute();

            writer.WriteValueElement( Strings.JPCost, JPCost );
            writer.WriteValueElement( Strings.LearnRate, LearnRate );
            writer.WriteValueElement( Strings.AbilityType, AbilityType );
            writer.WriteValueElement( Strings.LearnWithJP, LearnWithJP );
            writer.WriteValueElement( Strings.Action, Action );
            writer.WriteValueElement( Strings.LearnOnHit, LearnOnHit );
            writer.WriteValueElement( Strings.Blank1, Blank1 );
            writer.WriteValueElement( Strings.Unknown1, Unknown1 );
            writer.WriteValueElement( Strings.Unknown2, Unknown2 );
            writer.WriteValueElement( Strings.Unknown3, Unknown3 );
            writer.WriteValueElement( Strings.Blank2, Blank2 );
            writer.WriteValueElement( Strings.Blank3, Blank3 );
            writer.WriteValueElement( Strings.Blank4, Blank4 );
            writer.WriteValueElement( Strings.Blank5, Blank5 );
            writer.WriteValueElement( Strings.Unknown4, Unknown4 );

            writer.WriteStartElement( "AI" );
            ((IXmlSerializable)AIFlags).WriteXml( writer );
            writer.WriteEndElement();

            if (Effect != null)
            {
                writer.WriteStartElement( "Effect" );
                writer.WriteStartAttribute( "value" );
                writer.WriteValue( Effect.Value );
                writer.WriteEndAttribute();
                writer.WriteAttributeString( "name", Effect.Name );
            }

            if ( IsNormal )
            {
                writer.WriteStartElement( "Attributes" );
                ((IXmlSerializable)Attributes).WriteXml( writer );
                writer.WriteEndElement();
            }
            else if ( IsItem )
            {
                writer.WriteStartElement( "ItemOffset" );
                writer.WriteValue( ItemOffset );
            }
            else if ( IsThrowing )
            {
                writer.WriteStartElement( "Throwing" );
                writer.WriteValue( Throwing.ToString() );
            }
            else if ( IsJumping )
            {
                writer.WriteStartElement( "Jumping" );
                writer.WriteValueElement( "JumpHorizontal", JumpHorizontal );
                writer.WriteValueElement( "JumpVertical", JumpVertical );
            }
            else if ( IsCharging )
            {
                writer.WriteStartElement( "Charging" );
                writer.WriteValueElement( "ChargeCT", ChargeCT );
                writer.WriteValueElement( "ChargeBonus", ChargeBonus );
            }
            else if ( IsArithmetick )
            {
                writer.WriteStartElement( "ArithmetickSkill" );
                writer.WriteValue( ArithmetickSkill );
            }
            else if ( IsOther )
            {
                writer.WriteStartElement( "OtherID" );
                writer.WriteValue( OtherID );
            }
            else
            {
                throw new InvalidOperationException( "Invalid item type" );
            }
            writer.WriteEndElement();

        }

        protected override void ReadXml( XmlReader reader )
        {
            reader.MoveToAttribute( "Name" );
            Name = reader.ReadContentAsString();
            reader.MoveToAttribute( "Offset" );
            Offset = (UInt16)reader.ReadContentAsInt();
            reader.MoveToElement();
            reader.ReadStartElement();

            JPCost = (ushort)reader.ReadElementContentAsInt();
            LearnRate = (byte)reader.ReadElementContentAsInt();
            AbilityType = (AbilityType)Enum.Parse( typeof( AbilityType ), reader.ReadElementContentAsString(), true );
            LearnWithJP = reader.ReadElementContentAsBoolean();
            Action = reader.ReadElementContentAsBoolean();
            LearnOnHit= reader.ReadElementContentAsBoolean();
            Blank1 = reader.ReadElementContentAsBoolean();
            Unknown1 = reader.ReadElementContentAsBoolean();
            Unknown2 = reader.ReadElementContentAsBoolean();
            Unknown3 = reader.ReadElementContentAsBoolean();
            Blank2 = reader.ReadElementContentAsBoolean();
            Blank3 = reader.ReadElementContentAsBoolean();
            Blank4 = reader.ReadElementContentAsBoolean();
            Blank5 = reader.ReadElementContentAsBoolean();
            Unknown4 = reader.ReadElementContentAsBoolean();

            AIFlags = new AIFlags();
            ( (IXmlSerializable)AIFlags ).ReadXml( reader );

            IsNormal = ( ( Offset >= 0x000 ) && ( Offset <= 0x16F ) );
            IsItem = ( ( Offset >= 0x170 ) && ( Offset <= 0x17D ) );
            IsThrowing = ( ( Offset >= 0x17E ) && ( Offset <= 0x189 ) );
            IsJumping = ( ( Offset >= 0x18A ) && ( Offset <= 0x195 ) );
            IsCharging = ( ( Offset >= 0x196 ) && ( Offset <= 0x19D ) );
            IsArithmetick = ( ( Offset >= 0x19E ) && ( Offset <= 0x1A5 ) );
            IsOther = ( Offset >= 0x1A6 );
            bool IsReaction = Offset >= 422 && Offset <= 453;

            //if (IsNormal || IsItem || IsReaction)
            if ((Offset >= 0x000) && (Offset <= 0x1C5))
            {
                reader.MoveToAttribute( "value" );
                ushort effectIndex = (ushort)reader.ReadElementContentAsInt();
                reader.MoveToElement();
                reader.ReadStartElement();
                reader.ReadEndElement();

                if ( IsItem )
                    effectIndex &= (ushort)~ItemEffectPrefixValue;

                Effect = ourContext == Context.US_PSP ? Effect.PSPEffects[effectIndex] : Effect.PSXEffects[effectIndex];
            }

            if ( IsNormal )
            {
                Attributes = new AbilityAttributes(ourContext);
                ( (IXmlSerializable)Attributes ).ReadXml( reader );

            }
            else if ( IsItem )
            {
                //ItemOffset = (ushort)reader.ReadElementContentAsInt();
                SetItemOffset((ushort)reader.ReadElementContentAsInt(), ourContext);
            }
            else if ( IsThrowing )
            {
                Throwing = (ItemSubType)Enum.Parse( typeof( ItemSubType ),
                    reader.ReadElementContentAsString(), true );
            }
            else if ( IsJumping )
            {
                reader.ReadStartElement();
                JumpHorizontal = (byte)reader.ReadElementContentAsInt();
                JumpVertical = (byte)reader.ReadElementContentAsInt();
                reader.ReadEndElement();
            }
            else if ( IsCharging )
            {
                reader.ReadStartElement();
                ChargeCT = (byte)reader.ReadElementContentAsInt();
                ChargeBonus = (byte)reader.ReadElementContentAsInt();
                reader.ReadEndElement();
            }
            else if ( IsArithmetick )
            {
                ArithmetickSkill = (byte)reader.ReadElementContentAsInt();
            }
            else if ( IsOther )
            {
                OtherID = (byte)reader.ReadElementContentAsInt();
            }
            else
            {
                throw new InvalidOperationException( "Invalid item type" );
            }


            reader.ReadEndElement();
        }

    }
}