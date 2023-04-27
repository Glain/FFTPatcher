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
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// Represents the <see cref="Ability"/>s a monster can use.
    /// </summary>
    public class MonsterSkill : IChangeable, ISupportDigest, ISupportDefault<MonsterSkill>
    {
		#region Instance Variables (1) 

        private static readonly string[] digestableProperties = new string[4] {
            "Ability1", "Ability2", "Ability3", "Beastmaster" };

		#endregion Instance Variables 

		#region Public Properties (9) 

        public Ability Ability1 { get; set; }

        public Ability Ability2 { get; set; }

        public Ability Ability3 { get; set; }

        public Ability Beastmaster { get; set; }

        public Ability OldAbility1 { get; set; }

        public Ability OldAbility2 { get; set; }

        public Ability OldAbility3 { get; set; }

        public Ability OldBeastmaster { get; set; }

        public MonsterSkill Default { get; private set; }

        public IList<string> DigestableProperties
        {
            get { return digestableProperties; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public bool HasChanged
        {
            get
            {
                return ( Default != null ) &&
                    Ability1.Offset != Default.Ability1.Offset ||
                    Ability2.Offset != Default.Ability2.Offset ||
                    Ability3.Offset != Default.Ability3.Offset ||
                    Beastmaster.Offset != Default.Beastmaster.Offset;
            }
        }

        public string Name { get; private set; }

        public byte Value { get; private set; }

		#endregion Public Properties 

		#region Constructors (3) 

        public MonsterSkill(IList<byte> bytes, Context context)
            : this( 0, "", bytes, null, context )
        {
        }

        public MonsterSkill(byte value, string name, IList<byte> bytes, Context context)
            : this( value, name, bytes, null, context )
        {
        }

        public MonsterSkill( byte value, string name, IList<byte> bytes, MonsterSkill defaults, Context context )
        {
            Ability[] dummyAbilities = AllAbilities.GetDummyAbilities(context);

            Default = defaults;
            Name = name;
            Value = value;
            bool[] flags = PatcherLib.Utilities.Utilities.BooleansFromByteMSB( bytes[0] );
            Ability1 = dummyAbilities[flags[0] ? ( bytes[1] + 0x100 ) : bytes[1]];
            Ability2 = dummyAbilities[flags[1] ? ( bytes[2] + 0x100 ) : bytes[2]];
            Ability3 = dummyAbilities[flags[2] ? ( bytes[3] + 0x100 ) : bytes[3]];
            Beastmaster = dummyAbilities[flags[3] ? ( bytes[4] + 0x100 ) : bytes[4]];

            OldAbility1 = Ability1;
            OldAbility2 = Ability2;
            OldAbility3 = Ability3;
            OldBeastmaster = Beastmaster;
        }

		#endregion Constructors 

		#region Public Methods (4) 

        public byte[] ToByteArray()
        {
            byte[] result = new byte[5];
            result[0] = PatcherLib.Utilities.Utilities.ByteFromBooleans(
                Ability1.Offset > 0xFF,
                Ability2.Offset > 0xFF,
                Ability3.Offset > 0xFF,
                Beastmaster.Offset > 0xFF,
                false, false, false, false );
            result[1] = (byte)( Ability1.Offset & 0xFF );
            result[2] = (byte)( Ability2.Offset & 0xFF );
            result[3] = (byte)( Ability3.Offset & 0xFF );
            result[4] = (byte)( Beastmaster.Offset & 0xFF );

            return result;
        }

        public byte[] ToByteArray( Context context )
        {
            return ToByteArray();
        }

        public static void CopyAll(MonsterSkill source, MonsterSkill destination)
        {
            destination.Ability1 = source.Ability1;
            destination.Ability2 = source.Ability2;
            destination.Ability3 = source.Ability3;
            destination.Beastmaster = source.Beastmaster;

            destination.OldAbility1 = source.OldAbility1;
            destination.OldAbility2 = source.OldAbility2;
            destination.OldAbility3 = source.OldAbility3;
            destination.OldBeastmaster = source.OldBeastmaster;
        }

        public void CopyAllTo(MonsterSkill destination)
        {
            CopyAll(this, destination);
        }

		#endregion Public Methods 
    }

    public class AllMonsterSkills : PatchableFile, IXmlDigest, IGenerateCodes
    {

        #region Static Properties


        //public static IList<string> Names { get { return FFTPatch.Context == Context.US_PSP ? PSPNames : PSXNames; } }

        public static readonly IList<string> PSPNames = PSPResources.Lists.MonsterNames;

        public static readonly IList<string> PSXNames = PSXResources.Lists.MonsterNames;


        #endregion Static Properties

        #region Properties (2)


        public MonsterSkill[] MonsterSkills { get; private set; }


        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public override bool HasChanged
        {
            get
            {
                foreach ( MonsterSkill m in MonsterSkills )
                {
                    if ( m.HasChanged )
                        return true;
                }

                return false;
            }
        }

        #endregion Properties

        #region Constructors

        public AllMonsterSkills(IList<byte> bytes, Context context) : this(bytes, null, context) { }

        public AllMonsterSkills( IList<byte> bytes, IList<byte> defaultBytes, Context context )
        {
            defaultBytes = defaultBytes ?? (context == Context.US_PSP ? PSPResources.Binaries.MonsterSkills : PSXResources.Binaries.MonsterSkills);

            MonsterSkills = new MonsterSkill[48];
            for ( int i = 0; i < 48; i++ )
            {
                MonsterSkills[i] = new MonsterSkill( (byte)( i + 0xB0 ), GetNames(context)[i], bytes.Sub( 5 * i, 5 * i + 4 ),
                    new MonsterSkill( (byte)( i + 0xB0 ), GetNames(context)[i], defaultBytes.Sub( 5 * i, 5 * i + 4 ), context ), context );
            }
        }

        #endregion Constructors

        public static IList<string> GetNames(Context context) 
        { 
            return (context == Context.US_PSP) ? PSPNames : PSXNames; 
        }

        #region Methods


        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>( 5 * MonsterSkills.Length );
            foreach ( MonsterSkill s in MonsterSkills )
            {
                result.AddRange( s.ToByteArray() );
            }

            return result.ToArray();
        }

        public byte[] ToByteArray( Context context )
        {
            return ToByteArray();
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            if ( HasChanged )
            {
                writer.WriteStartElement( this.GetType().Name );
                writer.WriteAttributeString( "changed", HasChanged.ToString() );
                foreach ( MonsterSkill m in MonsterSkills )
                {
                    if ( m.HasChanged )
                    {
                        writer.WriteStartElement( m.GetType().Name );
                        writer.WriteAttributeString( "value", m.Value.ToString( "X2" ) );
                        writer.WriteAttributeString( "name", m.Name );
                        DigestGenerator.WriteXmlDigest( m, writer, false, true );
                    }
                }
                writer.WriteEndElement();
            }
        }

        #endregion Methods


        public override IList<PatchedByteArray> GetPatches( Context context )
        {
            var result = new List<PatchedByteArray>( 2 );

            var bytes = ToByteArray( context );
            if ( context == Context.US_PSX )
            {
                result.Add(PatcherLib.Iso.PsxIso.MonsterSkills.GetPatchedByteArray(bytes));
            }
            else if ( context == Context.US_PSP )
            {
                PatcherLib.Iso.PspIso.MonsterSkills.ForEach(kl => result.Add(kl.GetPatchedByteArray(bytes)));
            }

            return result;
        }

        #region IGenerateCodes Members

        string IGenerateCodes.GetCodeHeader(Context context)
        {
            return context == Context.US_PSP ? "_C0 Monster Skill Sets" : "\"Monster Skill Sets";
        }

        IList<string> IGenerateCodes.GenerateCodes(Context context, FFTPatch fftPatch)
        {
            if (context == Context.US_PSP)
            {
                return Codes.GenerateCodes( Context.US_PSP, fftPatch.Defaults[FFTPatch.ElementName.MonsterSkills], this.ToByteArray(), 0x27AB60 );
            }
            else
            {
                return Codes.GenerateCodes(Context.US_PSX, fftPatch.Defaults[FFTPatch.ElementName.MonsterSkills], this.ToByteArray(), 0x065BC4);
            }
        }

        #endregion
    }
}