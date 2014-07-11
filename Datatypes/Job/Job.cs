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
    /// Represents all <see cref="Job"/>s in memory.
    /// </summary>
    public class AllJobs : PatchableFile, IXmlDigest, IGenerateCodes
    {
		#region Instance Variables (2) 

        private static Job[] pspJobs;
        private static Job[] psxJobs;

		#endregion Instance Variables 

		#region Public Properties (6) 

        public static Job[] DummyJobs
        {
            get { return FFTPatch.Context == Context.US_PSP ? pspJobs : psxJobs; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public override bool HasChanged
        {
            get
            {
                return Jobs.Exists( j => j.HasChanged );
            }
        }

        public Job[] Jobs { get; private set; }

        public static IList<string> Names
        {
            get { return FFTPatch.Context == Context.US_PSP ? PSPNames : PSXNames; }
        }

        public static IList<string> PSPNames { get; private set; }

        public static IList<string> PSXNames { get; private set; }

		#endregion Public Properties 

		#region Constructors (3) 

        static AllJobs()
        {
            pspJobs = new Job[0xAA];
            psxJobs = new Job[0xA0];

            PSPNames = PSPResources.Lists.JobNames;
            List<string> psxNames = new List<string>( PSXResources.Lists.JobNames );
            psxNames.AddRange( new string[9] { "", "", "", "", "", "", "", "", "" } );
            PSXNames = psxNames.AsReadOnly(); ;

            for( int i = 0; i < 0xAA; i++ )
            {
                pspJobs[i] = new Job( (byte)i, PSPNames[i] );
            }

            for( int i = 0; i < 0xA0; i++ )
            {
                psxJobs[i] = new Job( (byte)i, PSXNames[i] );
            }
        }

        public AllJobs( IList<byte> bytes )
            : this( Context.US_PSP, bytes )
        {
        }

        public AllJobs( Context context, IList<byte> bytes )
        {
            int numJobs = context == Context.US_PSP ? 0xA9 : 0xA0;
            int jobLength = context == Context.US_PSP ? 49 : 48;
            IList<byte> defaultBytes = context == Context.US_PSP ? PSPResources.Binaries.Jobs : PSXResources.Binaries.Jobs;
            Jobs = new Job[numJobs];
            for( int i = 0; i < numJobs; i++ )
            {
                Jobs[i] = new Job( context, (byte)i, Names[i], bytes.Sub( i * jobLength, (i + 1) * jobLength - 1 ),
                    new Job( context, (byte)i, Names[i], defaultBytes.Sub( i * jobLength, (i + 1) * jobLength - 1 ) ) );
            }
        }

		#endregion Constructors 

		#region Public Methods (5) 

        public override IList<PatchedByteArray> GetPatches( Context context )
        {
            var result = new List<PatchedByteArray>( 2 );

            var bytes = ToByteArray( context );
            if ( context == Context.US_PSX )
            {
                result.Add(PatcherLib.Iso.PsxIso.Jobs.GetPatchedByteArray(bytes));
            }
            else if ( context == Context.US_PSP )
            {
                PatcherLib.Iso.PspIso.Jobs.ForEach(kl => result.Add(kl.GetPatchedByteArray(bytes)));
            }

            return result;
        }

        public byte[] ToByteArray()
        {
            return ToByteArray( Context.US_PSP );
        }

        public byte[] ToByteArray( Context context )
        {
            List<byte> result = new List<byte>( 0x205C );
            foreach( Job j in Jobs )
            {
                result.AddRange( j.ToByteArray( context ) );
            }

            return result.ToArray();
        }

        public void WriteXmlDigest( System.Xml.XmlWriter writer )
        {
            if( HasChanged )
            {
                writer.WriteStartElement( this.GetType().Name );
                writer.WriteAttributeString( "changed", HasChanged.ToString() );
                foreach( Job j in Jobs )
                {
                    if( j.HasChanged )
                    {
                        writer.WriteStartElement( j.GetType().Name );
                        writer.WriteAttributeString( "value", j.Value.ToString( "X2" ) );
                        writer.WriteAttributeString( "name", j.Name );
                        DigestGenerator.WriteXmlDigest( j, writer, false, true );
                    }
                }
                writer.WriteEndElement();
            }
        }

		#endregion Public Methods 
    
        #region IGenerateCodes Members

        string IGenerateCodes.GetCodeHeader(Context context)
        {
            return context == Context.US_PSP ? "_C0 Jobs" : "\"Jobs";
        }

        IList<string> IGenerateCodes.GenerateCodes(Context context)
        {
            if (context == Context.US_PSP)
            {
                return Codes.GenerateCodes( Context.US_PSP, PSPResources.Binaries.Jobs, this.ToByteArray(), 0x277988 );
            }
            else
            {
                return Codes.GenerateCodes( Context.US_PSX, PSXResources.Binaries.Jobs, this.ToByteArray( Context.US_PSX ), 0x0610B8 );
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a character's Job and its abilities and attributes.
    /// </summary>
    public class Job : IChangeable, ISupportDigest, ISupportDefault<Job>
    {
		#region Instance Variables (1) 

        private static readonly string[] digestableAttributes = new string[] {
            "SkillSet", "HPConstant", "HPMultiplier", "MPConstant", "MPMultiplier", "SpeedConstant", "SpeedMultiplier",
            "PAConstant", "PAMultiplier", "MAConstant", "MAMultiplier", "Move", "Jump", "CEvade", "MPortrait",
            "MPalette", "MGraphic", "InnateA", "InnateB", "InnateC", "InnateD", "AbsorbElement", "CancelElement",
            "HalfElement", "WeakElement", "Equipment", "PermanentStatus", "StartingStatus", "StatusImmunity" };

		#endregion Instance Variables 

		#region Public Properties (34) 

        public Elements AbsorbElement { get; private set; }

        public Elements CancelElement { get; private set; }

        public byte CEvade { get; set; }

        public Job Default { get; private set; }

        public IList<string> DigestableProperties
        {
            get { return digestableAttributes; }
        }

        public Equipment Equipment { get; private set; }

        public Elements HalfElement { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public bool HasChanged
        {
            get
            {
                return Default != null &&
                    (CEvade != Default.CEvade ||
                    HPConstant != Default.HPConstant ||
                    HPMultiplier != Default.HPMultiplier ||
                    InnateA.Offset != Default.InnateA.Offset ||
                    InnateB.Offset != Default.InnateB.Offset ||
                    InnateC.Offset != Default.InnateC.Offset ||
                    InnateD.Offset != Default.InnateD.Offset ||
                    Jump != Default.Jump ||
                    MAConstant != Default.MAConstant ||
                    MAMultiplier != Default.MAMultiplier ||
                    MGraphic != Default.MGraphic ||
                    Move != Default.Move ||
                    MPalette != Default.MPalette ||
                    MPConstant != Default.MPConstant ||
                    MPMultiplier != Default.MPMultiplier ||
                    MPortrait != Default.MPortrait ||
                    PAConstant != Default.PAConstant ||
                    PAMultiplier != Default.PAMultiplier ||
                    SkillSet.Value != Default.SkillSet.Value ||
                    SpeedConstant != Default.SpeedConstant ||
                    SpeedMultiplier != Default.SpeedMultiplier ||
                    AbsorbElement.ToByte() != Default.AbsorbElement.ToByte() ||
                    CancelElement.ToByte() != Default.CancelElement.ToByte() ||
                    HalfElement.ToByte() != Default.HalfElement.ToByte() ||
                    WeakElement.ToByte() != Default.WeakElement.ToByte() ||
                    !PatcherLib.Utilities.Utilities.CompareArrays( PermanentStatus.ToByteArray(), Default.PermanentStatus.ToByteArray() ) ||
                    !PatcherLib.Utilities.Utilities.CompareArrays( Equipment.ToByteArray( FFTPatch.Context ), Default.Equipment.ToByteArray( FFTPatch.Context ) ) ||
                    !PatcherLib.Utilities.Utilities.CompareArrays( StartingStatus.ToByteArray(), Default.StartingStatus.ToByteArray() ) ||
                    !PatcherLib.Utilities.Utilities.CompareArrays( StatusImmunity.ToByteArray(), Default.StatusImmunity.ToByteArray() )
                    );
            }
        }

        public byte HPConstant { get; set; }

        public byte HPMultiplier { get; set; }

        public Ability InnateA { get; set; }

        public Ability InnateB { get; set; }

        public Ability InnateC { get; set; }

        public Ability InnateD { get; set; }

        public byte Jump { get; set; }

        public byte MAConstant { get; set; }

        public byte MAMultiplier { get; set; }

        [Hex]
        public byte MGraphic { get; set; }

        public byte Move { get; set; }

        [Hex]
        public byte MPalette { get; set; }

        public byte MPConstant { get; set; }

        public byte MPMultiplier { get; set; }

        [Hex]
        public byte MPortrait { get; set; }

        public string Name { get; private set; }

        public byte PAConstant { get; set; }

        public byte PAMultiplier { get; set; }

        public Statuses PermanentStatus { get; private set; }

        public SkillSet SkillSet { get; set; }

        public byte SpeedConstant { get; set; }

        public byte SpeedMultiplier { get; set; }

        public Statuses StartingStatus { get; private set; }

        public Statuses StatusImmunity { get; private set; }

        public byte Value { get; private set; }

        public Elements WeakElement { get; private set; }

		#endregion Public Properties 

		#region Constructors (5) 

        public Job( IList<byte> bytes )
            : this( Context.US_PSP, bytes )
        {
        }

        public Job( Context context, IList<byte> bytes )
            : this( context, 0, "", bytes )
        {
        }

        public Job( byte value, string name )
        {
            Value = value;
            Name = name;
        }

        public Job( Context context, byte value, string name, IList<byte> bytes )
            : this( context, value, name, bytes, null )
        {
        }

        public Job( Context context, byte value, string name, IList<byte> bytes, Job defaults )
        {
            Value = value;
            Name = name;
            int equipEnd = context == Context.US_PSP ? 13 : 12;

            SkillSet = context == Context.US_PSP ? SkillSet.PSPSkills[bytes[0]] : SkillSet.PSXSkills[bytes[0]];
            InnateA = AllAbilities.DummyAbilities[PatcherLib.Utilities.Utilities.BytesToUShort( bytes[1], bytes[2] )];
            InnateB = AllAbilities.DummyAbilities[PatcherLib.Utilities.Utilities.BytesToUShort( bytes[3], bytes[4] )];
            InnateC = AllAbilities.DummyAbilities[PatcherLib.Utilities.Utilities.BytesToUShort( bytes[5], bytes[6] )];
            InnateD = AllAbilities.DummyAbilities[PatcherLib.Utilities.Utilities.BytesToUShort( bytes[7], bytes[8] )];
            Equipment = new Equipment( bytes.Sub( 9, equipEnd ), defaults == null ? null : defaults.Equipment );
            HPConstant = bytes[equipEnd + 1];
            HPMultiplier = bytes[equipEnd + 2];
            MPConstant = bytes[equipEnd + 3];
            MPMultiplier = bytes[equipEnd + 4];
            SpeedConstant = bytes[equipEnd + 5];
            SpeedMultiplier = bytes[equipEnd + 6];
            PAConstant = bytes[equipEnd + 7];
            PAMultiplier = bytes[equipEnd + 8];
            MAConstant = bytes[equipEnd + 9];
            MAMultiplier = bytes[equipEnd + 10];
            Move = bytes[equipEnd + 11];
            Jump = bytes[equipEnd + 12];
            CEvade = bytes[equipEnd + 13];
            PermanentStatus = new Statuses( bytes.Sub( equipEnd + 14, equipEnd + 18 ), defaults == null ? null : defaults.PermanentStatus );
            StatusImmunity = new Statuses( bytes.Sub( equipEnd + 19, equipEnd + 23 ), defaults == null ? null : defaults.StatusImmunity );
            StartingStatus = new Statuses( bytes.Sub( equipEnd + 24, equipEnd + 28 ), defaults == null ? null : defaults.StartingStatus );
            AbsorbElement = new Elements( bytes[equipEnd + 29] );
            CancelElement = new Elements( bytes[equipEnd + 30] );
            HalfElement = new Elements( bytes[equipEnd + 31] );
            WeakElement = new Elements( bytes[equipEnd + 32] );

            MPortrait = bytes[equipEnd + 33];
            MPalette = bytes[equipEnd + 34];
            MGraphic = bytes[equipEnd + 35];

            if( defaults != null )
            {
                Default = defaults;
                AbsorbElement.Default = defaults.AbsorbElement;
                CancelElement.Default = defaults.CancelElement;
                HalfElement.Default = defaults.HalfElement;
                WeakElement.Default = defaults.WeakElement;
            }
        }

		#endregion Constructors 

		#region Public Methods (5) 

        public static void Copy( Job source, Job destination )
        {
            source.PermanentStatus.CopyTo( destination.PermanentStatus );
            source.StatusImmunity.CopyTo( destination.StatusImmunity );
            source.StartingStatus.CopyTo( destination.StartingStatus );
            source.AbsorbElement.CopyTo( destination.AbsorbElement );
            source.CancelElement.CopyTo( destination.CancelElement );
            source.HalfElement.CopyTo( destination.HalfElement );
            source.WeakElement.CopyTo( destination.WeakElement );
            source.Equipment.CopyTo( destination.Equipment );
            destination.SkillSet = source.SkillSet;
            destination.InnateA = source.InnateA;
            destination.InnateB = source.InnateB;
            destination.InnateC = source.InnateC;
            destination.InnateD = source.InnateD;
            destination.HPConstant = source.HPConstant;
            destination.HPMultiplier = source.HPMultiplier;
            destination.MPConstant = source.MPConstant;
            destination.MPMultiplier = source.MPMultiplier;
            destination.PAConstant = source.PAConstant;
            destination.PAMultiplier = source.PAMultiplier;
            destination.MAConstant = source.MAConstant;
            destination.MAMultiplier = source.MAMultiplier;
            destination.SpeedConstant = source.SpeedConstant;
            destination.SpeedMultiplier = source.SpeedMultiplier;
            destination.Move = source.Move;
            destination.Jump = source.Jump;
            destination.CEvade = source.CEvade;
            destination.MPortrait = source.MPortrait;
            destination.MPalette = source.MPalette;
            destination.MGraphic = source.MGraphic;
        }

        public void CopyTo( Job destination )
        {
            Copy( this, destination );
        }

        public byte[] ToByteArray()
        {
            return ToByteArray( Context.US_PSP );
        }

        public byte[] ToByteArray( Context context )
        {
            List<byte> result = new List<byte>( 49 );
            result.Add( SkillSet.Value );
            result.AddRange( InnateA.Offset.ToBytes() );
            result.AddRange( InnateB.Offset.ToBytes() );
            result.AddRange( InnateC.Offset.ToBytes() );
            result.AddRange( InnateD.Offset.ToBytes() );
            result.AddRange( Equipment.ToByteArray( context ) );
            result.Add( HPConstant );
            result.Add( HPMultiplier );
            result.Add( MPConstant );
            result.Add( MPMultiplier );
            result.Add( SpeedConstant );
            result.Add( SpeedMultiplier );
            result.Add( PAConstant );
            result.Add( PAMultiplier );
            result.Add( MAConstant );
            result.Add( MAMultiplier );
            result.Add( Move );
            result.Add( Jump );
            result.Add( CEvade );
            result.AddRange( PermanentStatus.ToByteArray() );
            result.AddRange( StatusImmunity.ToByteArray() );
            result.AddRange( StartingStatus.ToByteArray() );
            result.Add( AbsorbElement.ToByte() );
            result.Add( CancelElement.ToByte() );
            result.Add( HalfElement.ToByte() );
            result.Add( WeakElement.ToByte() );
            result.Add( MPortrait );
            result.Add( MPalette );
            result.Add( MGraphic );

            return result.ToArray();
        }

        public override string ToString()
        {
            return (HasChanged ? "*" : "") + Value.ToString( "X2" ) + " " + Name;
        }

		#endregion Public Methods 
    }
}
