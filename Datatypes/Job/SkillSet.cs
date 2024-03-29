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
    /// Represents the set of <see cref="Ability"/> a <see cref="Job"/> can use.
    /// </summary>
    public class SkillSet : IChangeable, ISupportDigest, ISupportDefault<SkillSet>
    {
		#region Instance Variables

        private static readonly string[] digestableProperties = new string[22] {
            "Action1", "Action2", "Action3", "Action4", "Action5", "Action6", "Action7", "Action8", 
            "Action9", "Action10", "Action11", "Action12", "Action13", "Action14", "Action15", "Action16", 
            "TheRest1", "TheRest2", "TheRest3", "TheRest4", "TheRest5", "TheRest6" };

        private static readonly SkillSet _skillsetRandom = new SkillSet( "<Random>", 0xFE );
        private static readonly SkillSet _skillsetEqual = new SkillSet( "<Job's>", 0xFF );

		#endregion Instance Variables 

		#region Public Properties 

        public Ability Action1 { get { return Actions[0]; } }

        public Ability Action10 { get { return Actions[9]; } }

        public Ability Action11 { get { return Actions[10]; } }

        public Ability Action12 { get { return Actions[11]; } }

        public Ability Action13 { get { return Actions[12]; } }

        public Ability Action14 { get { return Actions[13]; } }

        public Ability Action15 { get { return Actions[14]; } }

        public Ability Action16 { get { return Actions[15]; } }

        public Ability Action2 { get { return Actions[1]; } }

        public Ability Action3 { get { return Actions[2]; } }

        public Ability Action4 { get { return Actions[3]; } }

        public Ability Action5 { get { return Actions[4]; } }

        public Ability Action6 { get { return Actions[5]; } }

        public Ability Action7 { get { return Actions[6]; } }

        public Ability Action8 { get { return Actions[7]; } }

        public Ability Action9 { get { return Actions[8]; } }

        public Ability[] Actions { get; private set; }

        public SkillSet Default { get; private set; }

        public IList<string> DigestableProperties
        {
            get { return digestableProperties; }
        }

        private HashSet<int> referencingJobIDs;
        public HashSet<int> ReferencingJobIDs
        {
            get
            {
                if (referencingJobIDs == null)
                    referencingJobIDs = new HashSet<int>();

                return referencingJobIDs;
            }
        }

        private HashSet<int> referencingENTDs;
        public HashSet<int> ReferencingENTDs
        {
            get
            {
                if (referencingENTDs == null)
                    referencingENTDs = new HashSet<int>();

                return referencingENTDs;
            }
        }

        /*
        public static SkillSet[] DummySkillSets
        {
            get
            {
                return FFTPatch.Context == Context.US_PSP ? PSPSkills : PSXSkills;
            }
        }

        public static SortedDictionary<byte, SkillSet> EventSkillSets
        {
            get { return FFTPatch.Context == Context.US_PSP ? pspEventSkills : psxEventSkills; }
        }
        */

        public static SkillSet[] GetDummySkillSets(Context context)
        {
            return (context == Context.US_PSP) ? PSPSkills : PSXSkills;
        }

        public static SortedDictionary<byte, SkillSet> GetEventSkillSets(Context context)
        {
            return (context == Context.US_PSP) ? PspEventSkills : PsxEventSkills;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public bool HasChanged
        {
            get
            {
                if( Default != null )
                {
                    for( int i = 0; i < Actions.Length; i++ )
                    {
                        if( Actions[i].Offset != Default.Actions[i].Offset )
                            return true;
                    }
                    for( int i = 0; i < TheRest.Length; i++ )
                    {
                        if( TheRest[i].Offset != Default.TheRest[i].Offset )
                            return true;
                    }
                }

                return false;
            }
        }

        public string Name { get; private set; }

        private static SortedDictionary<byte, SkillSet> _pspEventSkills = null;
        private static SortedDictionary<byte, SkillSet> PspEventSkills
        {
            get
            {
                if (_pspEventSkills == null)
                {
                    SkillSet[] pspSkills = PSPSkills;
                    _pspEventSkills = new SortedDictionary<byte, SkillSet>();
                    for (int i = 0; i < 0xE3; i++)
                    {
                        _pspEventSkills.Add((byte)i, pspSkills[i]);
                    }
                    _pspEventSkills.Add(0xFE, _skillsetRandom);
                    _pspEventSkills.Add(0xFF, _skillsetEqual);
                }

                return _pspEventSkills;
            }
        }

        public static readonly IList<string> PSPNames = PSPResources.Lists.SkillSets;

        private static SkillSet[] _pspSkills = null;
        public static SkillSet[] PSPSkills 
        {
            get
            {
                if (_pspSkills == null)
                {
                    _pspSkills = new SkillSet[0xE3];
                    for (int i = 0; i < 0xE3; i++)
                    {
                        _pspSkills[i] = new SkillSet(PSPNames[i], (byte)(i & 0xFF));
                    }
                }

                return _pspSkills;
            }
        }

        private static SortedDictionary<byte, SkillSet> _psxEventSkills = null;
        private static SortedDictionary<byte, SkillSet> PsxEventSkills
        {
            get
            {
                if (_psxEventSkills == null)
                {
                    SkillSet[] psxSkills = PSXSkills;
                    _psxEventSkills = new SortedDictionary<byte, SkillSet>();
                    for (int i = 0; i < 0xE0; i++)
                    {
                        _psxEventSkills.Add((byte)i, psxSkills[i]);
                    }
                    _psxEventSkills.Add(0xFE, _skillsetRandom);
                    _psxEventSkills.Add(0xFF, _skillsetEqual);
                }

                return _psxEventSkills;
            }
        }

        public static readonly IList<string> PSXNames = PSXResources.Lists.SkillSets;

        private static SkillSet[] _psxSkills = null;
        public static SkillSet[] PSXSkills
        {
            get
            {
                if (_psxSkills == null)
                {
                    _psxSkills = new SkillSet[0xE0];
                    for (int i = 0; i < 0xE0; i++)
                    {
                        _psxSkills[i] = new SkillSet(PSXNames[i], (byte)(i & 0xFF));
                    }
                }

                return _psxSkills;
            }
        }

        public static IList<string> GetNames(Context context)
        {
            return (context == Context.US_PSP) ? PSPNames : PSXNames;
        }

        public Ability[] TheRest { get; private set; }

        public Ability TheRest1 { get { return TheRest[0]; } }

        public Ability TheRest2 { get { return TheRest[1]; } }

        public Ability TheRest3 { get { return TheRest[2]; } }

        public Ability TheRest4 { get { return TheRest[3]; } }

        public Ability TheRest5 { get { return TheRest[4]; } }

        public Ability TheRest6 { get { return TheRest[5]; } }

        public byte Value { get; private set; }

        public Ability[] OldActions { get; set; }

        public Ability[] OldTheRest { get; set; }

		#endregion Public Properties 

		#region Constructors

        public SkillSet( byte value, IList<byte> bytes, Context context )
            : this( GetDummySkillSets(context)[value].Name, value )
        {
            List<bool> actions = new List<bool>( 16 );
            actions.AddRange( PatcherLib.Utilities.Utilities.BooleansFromByteMSB( bytes[0] ) );
            actions.AddRange( PatcherLib.Utilities.Utilities.BooleansFromByteMSB( bytes[1] ) );
            List<bool> theRest = new List<bool>( PatcherLib.Utilities.Utilities.BooleansFromByteMSB( bytes[2] ) );

            Actions = new Ability[16];
            TheRest = new Ability[6];

            for( int i = 0; i < 16; i++ )
            {
                Actions[i] = AllAbilities.GetDummyAbilities(context)[(actions[i] ? (bytes[3 + i] + 0x100) : (bytes[3 + i]))];
            }
            for( int i = 0; i < 6; i++ )
            {
                TheRest[i] = AllAbilities.GetDummyAbilities(context)[(theRest[i] ? (bytes[19 + i] + 0x100) : (bytes[19 + i]))];
            }

            OldActions = (Ability[])Actions.Clone();
            OldTheRest = (Ability[])TheRest.Clone();
        }

        private SkillSet( string name, byte value )
        {
            Name = name;
            Value = value;
        }

        public SkillSet( byte value, IList<byte> bytes, SkillSet defaults, Context context )
            : this( value, bytes, context )
        {
            Default = defaults;
        }

		#endregion Constructors 

		#region Public Methods (4) 

        public static void Copy( SkillSet source, SkillSet destination )
        {
            for( int i = 0; i < 16; i++ )
            {
                destination.Actions[i] = source.Actions[i];
                destination.OldActions[i] = source.OldActions[i];
            }
            for( int i = 0; i < 6; i++ )
            {
                destination.TheRest[i] = source.TheRest[i];
                destination.OldTheRest[i] = source.OldTheRest[i];
            }
        }

        public void CopyTo( SkillSet destination )
        {
            Copy( this, destination );
        }

        public byte[] ToByteArray()
        {
            byte[] result = new byte[25];
            result[0] = PatcherLib.Utilities.Utilities.ByteFromBooleans(
                Actions[0].Offset > 0xFF,
                Actions[1].Offset > 0xFF,
                Actions[2].Offset > 0xFF,
                Actions[3].Offset > 0xFF,
                Actions[4].Offset > 0xFF,
                Actions[5].Offset > 0xFF,
                Actions[6].Offset > 0xFF,
                Actions[7].Offset > 0xFF );
            result[1] = PatcherLib.Utilities.Utilities.ByteFromBooleans(
                Actions[8].Offset > 0xFF,
                Actions[9].Offset > 0xFF,
                Actions[10].Offset > 0xFF,
                Actions[11].Offset > 0xFF,
                Actions[12].Offset > 0xFF,
                Actions[13].Offset > 0xFF,
                Actions[14].Offset > 0xFF,
                Actions[15].Offset > 0xFF );
            result[2] = PatcherLib.Utilities.Utilities.ByteFromBooleans(
                TheRest[0].Offset > 0xFF,
                TheRest[1].Offset > 0xFF,
                TheRest[2].Offset > 0xFF,
                TheRest[3].Offset > 0xFF,
                TheRest[4].Offset > 0xFF,
                TheRest[5].Offset > 0xFF,
                false,
                false );
            for( int i = 0; i < 16; i++ )
            {
                result[3 + i] = (byte)(Actions[i].Offset & 0xFF);
            }
            for( int i = 0; i < 6; i++ )
            {
                result[19 + i] = (byte)(TheRest[i].Offset & 0xFF);
            }

            return result;
        }

        public override string ToString()
        {
            return (HasChanged ? "*" : "") + Value.ToString( "X2" ) + " " + Name;
        }

        public string GetCorrespondingJobs(FFTPatch FFTPatch)
        {
            List<string> result = new List<string>();
            foreach (Job j in FFTPatch.Jobs.Jobs)
            {
                if (j.SkillSet.Value == Value)
                {
                    result.Add(j.ToString());
                }
            }

            return string.Join(", ", result.ToArray());
        }

		#endregion Public Methods 
    }

    public class AllSkillSets : PatchableFile, IXmlDigest, IGenerateCodes
    {
		#region Public Properties (2) 

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public override bool HasChanged
        {
            get
            {
                foreach( SkillSet s in SkillSets )
                {
                    if( s.HasChanged )
                        return true;
                }

                return false;
            }
        }

        public SkillSet[] SkillSets { get; private set; }

		#endregion Public Properties 

		#region Constructors (3) 

        public AllSkillSets( IList<byte> bytes )
            : this( Context.US_PSP, bytes, PSPResources.Binaries.SkillSets )
        {
        }

        public AllSkillSets( Context context, IList<byte> bytes )
            : this( context, bytes, null )
        {
        }

        public AllSkillSets( Context context, IList<byte> bytes, IList<byte> defaultBytes )
        {
            List<SkillSet> tempSkills = new List<SkillSet>( 179 );
            for( int i = 0; i < 176; i++ )
            {
                tempSkills.Add( new SkillSet( (byte)i, bytes.Sub( 25 * i, 25 * i + 24 ),
                    new SkillSet( (byte)i, defaultBytes.Sub( 25 * i, 25 * i + 24 ), context ), context ) );
            }

            if( context == Context.US_PSP )
            {
                for( int i = 0xE0; i <= 0xE2; i++ )
                {
                    tempSkills.Add( new SkillSet( (byte)i, bytes.Sub( 25 * (i - 0xE0 + 176), 25 * (i - 0xE0 + 176) + 24 ),
                        new SkillSet( (byte)i, defaultBytes.Sub( 25 * (i - 0xE0 + 176), 25 * (i - 0xE0 + 176) + 24 ), context ), context ) );
                }
            }

            SkillSets = tempSkills.ToArray();
        }

		#endregion Constructors 

		#region Public Methods (5) 

        public override IList<PatchedByteArray> GetPatches( Context context )
        {
            var result = new List<PatchedByteArray>( 2 );

            var bytes = ToByteArray( context );
            if ( context == Context.US_PSX )
            {
                result.Add(PatcherLib.Iso.PsxIso.SkillSets.GetPatchedByteArray(bytes));
            }
            else if ( context == Context.US_PSP )
            {
                PatcherLib.Iso.PspIso.SkillSets.ForEach(kl => result.Add(kl.GetPatchedByteArray(bytes)));
            }

            return result;
        }

        public byte[] ToByteArray()
        {
            return ToByteArray( Context.US_PSP );
        }

        public byte[] ToByteArray( Context context )
        {
            List<byte> result = new List<byte>( 0x117B );
            foreach( SkillSet s in SkillSets )
            {
                result.AddRange( s.ToByteArray() );
            }

            while( (context == Context.US_PSP) && (result.Count < 0x117B) )
            {
                result.Add( 0x00 );
            }

            return result.ToArray();
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            if( HasChanged )
            {
                writer.WriteStartElement( this.GetType().Name );
                writer.WriteAttributeString( "changed", HasChanged.ToString() );
                foreach( SkillSet s in SkillSets )
                {
                    if( s.HasChanged )
                    {
                        writer.WriteStartElement( s.GetType().Name );
                        writer.WriteAttributeString( "value", s.Value.ToString( "X2" ) );
                        writer.WriteAttributeString( "name", s.Name );
                        DigestGenerator.WriteXmlDigest( s, writer, false, false );
                        writer.WriteElementString( "CorrespondingJobs", s.GetCorrespondingJobs(FFTPatch) );
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }
        }

		#endregion Public Methods 
    
        #region IGenerateCodes Members

        string IGenerateCodes.GetCodeHeader(Context context)
        {
            return context == Context.US_PSP ? "_C0 Skill Sets" : "\"Skill Sets";
        }

        IList<string> IGenerateCodes.GenerateCodes(Context context, FFTPatch fftPatch)
        {
            if (context == Context.US_PSP)
            {
                return Codes.GenerateCodes( Context.US_PSP, fftPatch.Defaults[FFTPatch.ElementName.SkillSets], this.ToByteArray(), 0x2799E4 );
            }
            else
            {
                return Codes.GenerateCodes(Context.US_PSX, fftPatch.Defaults[FFTPatch.ElementName.SkillSets], this.ToByteArray(Context.US_PSX), 0x064A94);
            }
        }

        #endregion
    }
}
