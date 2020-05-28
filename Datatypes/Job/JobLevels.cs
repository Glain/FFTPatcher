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
using PatcherLib;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// Represents the JP needed to grow in a job level as well as the prerequisites for unlocking jobs.
    /// </summary>
    public class JobLevels : PatchableFile, IXmlDigest, ISupportDefault<JobLevels>, IGenerateCodes
    {
		#region Instance Variables (4) 

        private static readonly string[] digestableProperties = new string[] {
            "Chemist", "Knight", "Archer", "Monk", "WhiteMage", "BlackMage", "TimeMage", "Summoner", "Thief", "Orator", 
            "Mystic", "Geomancer", "Dragoon", "Samurai", "Ninja", "Arithmetician", "Bard", "Dancer", "Mime", "DarkKnight", "OnionKnight", "Unknown",
            "Level1", "Level2", "Level3", "Level4", "Level5", "Level6", "Level7", "Level8" };
        private string[] levelFields = new string[] {
            "Level1", "Level2", "Level3", "Level4", "Level5", "Level6", "Level7", "Level8" };
        private ushort[] levels = new ushort[8];
        private string[] reqs = new string[] {
            "Chemist", "Knight", "Archer", "Monk", "WhiteMage", "BlackMage", "TimeMage", "Summoner", "Thief", "Orator", 
            "Mystic", "Geomancer", "Dragoon", "Samurai", "Ninja", "Arithmetician", "Bard", "Dancer", "Mime", "DarkKnight", "OnionKnight", "Unknown" };

		#endregion Instance Variables 

		#region Public Properties (32) 

        public Requirements Archer { get; private set; }

        public Requirements Arithmetician { get; private set; }

        public Requirements Bard { get; private set; }

        public Requirements BlackMage { get; private set; }

        public Requirements Chemist { get; private set; }

        public Requirements Dancer { get; private set; }

        public Requirements DarkKnight { get; private set; }

        public JobLevels Default { get; private set; }

        public Requirements Dragoon { get; private set; }

        public Requirements Geomancer { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public override bool HasChanged
        {
            get
            {
                return (Default != null) &&
                    Level1 != Default.Level1 ||
                    Level2 != Default.Level2 ||
                    Level3 != Default.Level3 ||
                    Level4 != Default.Level4 ||
                    Level5 != Default.Level5 ||
                    Level6 != Default.Level6 ||
                    Level7 != Default.Level7 ||
                    Level8 != Default.Level8 ||
                    Archer.HasChanged ||
                    Arithmetician.HasChanged ||
                    Bard.HasChanged ||
                    BlackMage.HasChanged ||
                    Chemist.HasChanged ||
                    Dancer.HasChanged ||
                    Dragoon.HasChanged ||
                    Geomancer.HasChanged ||
                    Knight.HasChanged ||
                    Mime.HasChanged ||
                    Monk.HasChanged ||
                    Mystic.HasChanged ||
                    Ninja.HasChanged ||
                    Orator.HasChanged ||
                    Samurai.HasChanged ||
                    Summoner.HasChanged ||
                    Thief.HasChanged ||
                    TimeMage.HasChanged ||
                    WhiteMage.HasChanged ||
                    (OnionKnight != null && OnionKnight.HasChanged) ||
                    (DarkKnight != null && DarkKnight.HasChanged) ||
                    (Unknown != null && Unknown.HasChanged);
            }
        }

        public Requirements Knight { get; private set; }

        public ushort Level1
        {
            get { return levels[0]; }
            set { levels[0] = value; }
        }

        public ushort Level2
        {
            get { return levels[1]; }
            set { levels[1] = value; }
        }

        public ushort Level3
        {
            get { return levels[2]; }
            set { levels[2] = value; }
        }

        public ushort Level4
        {
            get { return levels[3]; }
            set { levels[3] = value; }
        }

        public ushort Level5
        {
            get { return levels[4]; }
            set { levels[4] = value; }
        }

        public ushort Level6
        {
            get { return levels[5]; }
            set { levels[5] = value; }
        }

        public ushort Level7
        {
            get { return levels[6]; }
            set { levels[6] = value; }
        }

        public ushort Level8
        {
            get { return levels[7]; }
            set { levels[7] = value; }
        }

        public Requirements Mime { get; private set; }

        public Requirements Monk { get; private set; }

        public Requirements Mystic { get; private set; }

        public Requirements Ninja { get; private set; }

        public Requirements OnionKnight { get; private set; }

        public Requirements Orator { get; private set; }

        public Requirements Samurai { get; private set; }

        public Requirements Summoner { get; private set; }

        public Requirements Thief { get; private set; }

        public Requirements TimeMage { get; private set; }

        public Requirements Unknown { get; private set; }

        public Requirements WhiteMage { get; private set; }

		#endregion Public Properties 

		#region Constructors (3) 

        public JobLevels( IList<byte> bytes )
            : this( Context.US_PSP, bytes )
        {
        }

        public JobLevels( Context context, IList<byte> bytes )
            : this( context, bytes, null )
        {
        }

        public JobLevels( Context context, IList<byte> bytes, JobLevels defaults )
        {
            Default = defaults;
            int jobCount = context == Context.US_PSP ? 22 : 19;
            int requirementsLength = context == Context.US_PSP ? 12 : 10;

            for( int i = 0; i < jobCount; i++ )
            {
                ReflectionHelpers.SetFieldOrProperty( this, reqs[i],
                    new Requirements( context,
                        bytes.Sub( i * requirementsLength, (i + 1) * requirementsLength - 1 ),
                        defaults == null ? null : ReflectionHelpers.GetFieldOrProperty<Requirements>( defaults, reqs[i] ) ) );
            }

            int start = requirementsLength * jobCount;
            if( context == Context.US_PSX )
                start += 2;

            for( int i = 0; i < levels.Length; i++ )
            {
                levels[i] = PatcherLib.Utilities.Utilities.BytesToUShort( bytes[start + i * 2], bytes[start + i * 2 + 1] );
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
                result.Add(PatcherLib.Iso.PsxIso.JobLevels.GetPatchedByteArray(bytes));
            }
            else if ( context == Context.US_PSP )
            {
                PatcherLib.Iso.PspIso.JobLevels.ForEach(kl => result.Add(kl.GetPatchedByteArray(bytes)));
            }

            return result;
        }

        public byte[] ToByteArray()
        {
            return ToByteArray( Context.US_PSP );
        }

        public byte[] ToByteArray( Context context )
        {
            int jobCount = context == Context.US_PSP ? 22 : 19;
            List<byte> result = new List<byte>( 0x118 );
            for( int i = 0; i < jobCount; i++ )
            {
                result.AddRange( ReflectionHelpers.GetFieldOrProperty<Requirements>( this, reqs[i] ).ToByteArray( context ) );
            }
            if( context == Context.US_PSX )
            {
                result.Add( 0x00 );
                result.Add( 0x00 );
            }

            foreach( ushort level in levels )
            {
                result.AddRange( level.ToBytes() );
            }

            return result.ToArray();
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            writer.WriteStartElement( GetType().Name );
            writer.WriteAttributeString( "changed", HasChanged.ToString() );

            foreach( string req in reqs )
            {
                Requirements r = ReflectionHelpers.GetFieldOrProperty<Requirements>( this, req, false );
                if( r != null )
                {
                    writer.WriteStartElement( req );
                    r.WriteXml( writer );
                    writer.WriteEndElement();
                }
            }

            for( int i = 0; i < levels.Length; i++ )
            {
                if( levels[i] != Default.levels[i] )
                {
                    writer.WriteStartElement( string.Format( "Level{0}", i ) );
                    writer.WriteAttributeString( "changed", (levels[i] != Default.levels[i]).ToString() );
                    writer.WriteAttributeString( "default", Default.levels[i].ToString() );
                    writer.WriteAttributeString( "value", levels[i].ToString() );
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }

		#endregion Public Methods 
    
        #region IGenerateCodes Members

        string IGenerateCodes.GetCodeHeader(Context context)
        {
            return context == Context.US_PSP ? "_C0 Job Levels" : "\"Job Levels";
        }

        IList<string> IGenerateCodes.GenerateCodes(Context context, FFTPatch fftPatch)
        {
            if (context == Context.US_PSP)
            {
                return Codes.GenerateCodes( Context.US_PSP, fftPatch.Defaults[FFTPatch.ElementName.JobLevels], this.ToByteArray(), 0x27B030 );
            }
            else
            {
                return Codes.GenerateCodes(Context.US_PSX, fftPatch.Defaults[FFTPatch.ElementName.JobLevels], this.ToByteArray(Context.US_PSX), 0x0660C4);
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents the prerequisites to unlock a job.
    /// </summary>
    public class Requirements : IChangeable, IEquatable<Requirements>, ISupportDefault<Requirements>
    {
        private static readonly string[] fields = new string[24] {
            "Squire", "Chemist", "Knight", "Archer", "Monk", "WhiteMage", "BlackMage", "TimeMage", "Summoner",
            "Thief", "Orator", "Mystic", "Geomancer", "Dragoon", "Samurai", "Ninja", "Arithmetician", "Bard",
            "Dancer", "Mime", "DarkKnight", "OnionKnight", "Unknown1", "Unknown2" };
        private Context ourContext = Context.Default;

        #region Properties (26)


        public int Archer { get; set; }

        public int Arithmetician { get; set; }

        public int Bard { get; set; }

        public int BlackMage { get; set; }

        public int Chemist { get; set; }

        public int Dancer { get; set; }

        public int DarkKnight { get; set; }

        public Requirements Default { get; private set; }

        public int Dragoon { get; set; }

        public int Geomancer { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        public bool HasChanged
        {
            get
            {
                return
                    Default != null &&
                    !PatcherLib.Utilities.Utilities.CompareArrays(ToByteArray(ourContext), Default.ToByteArray(ourContext));
            }
        }

        public int Knight { get; set; }

        public int Mime { get; set; }

        public int Monk { get; set; }

        public int Mystic { get; set; }

        public int Ninja { get; set; }

        public int OnionKnight { get; set; }

        public int Orator { get; set; }

        public int Samurai { get; set; }

        public int Squire { get; set; }

        public int Summoner { get; set; }

        public int Thief { get; set; }

        public int TimeMage { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public int WhiteMage { get; set; }


        #endregion Properties

        #region Constructors (3)

        public Requirements( IList<byte> bytes )
            : this( Context.US_PSP, bytes )
        {
        }

        public Requirements( Context context, IList<byte> bytes )
            : this( context, bytes, null )
        {
        }

        public Requirements( Context context, IList<byte> bytes, Requirements defaults )
        {
            ourContext = context;

            Default = defaults;
            Squire = bytes[0].GetUpperNibble();
            Chemist = bytes[0].GetLowerNibble();
            Knight = bytes[1].GetUpperNibble();
            Archer = bytes[1].GetLowerNibble();
            Monk = bytes[2].GetUpperNibble();
            WhiteMage = bytes[2].GetLowerNibble();
            BlackMage = bytes[3].GetUpperNibble();
            TimeMage = bytes[3].GetLowerNibble();
            Summoner = bytes[4].GetUpperNibble();
            Thief = bytes[4].GetLowerNibble();
            Orator = bytes[5].GetUpperNibble();
            Mystic = bytes[5].GetLowerNibble();
            Geomancer = bytes[6].GetUpperNibble();
            Dragoon = bytes[6].GetLowerNibble();
            Samurai = bytes[7].GetUpperNibble();
            Ninja = bytes[7].GetLowerNibble();
            Arithmetician = bytes[8].GetUpperNibble();
            Bard = bytes[8].GetLowerNibble();
            Dancer = bytes[9].GetUpperNibble();
            Mime = bytes[9].GetLowerNibble();
            if( context == Context.US_PSP )
            {
                DarkKnight = bytes[10].GetUpperNibble();
                OnionKnight = bytes[10].GetLowerNibble();
                Unknown1 = bytes[11].GetUpperNibble();
                Unknown2 = bytes[11].GetLowerNibble();
            }
        }

        #endregion Constructors

        #region Methods (7)


        public byte[] ToByteArray( Context context )
        {
            List<byte> result = new List<byte>( 12 );
            result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( Squire, Chemist ) );
            result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( Knight, Archer ) );
            result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( Monk, WhiteMage ) );
            result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( BlackMage, TimeMage ) );
            result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( Summoner, Thief ) );
            result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( Orator, Mystic ) );
            result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( Geomancer, Dragoon ) );
            result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( Samurai, Ninja ) );
            result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( Arithmetician, Bard ) );
            result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( Dancer, Mime ) );
            if( context == Context.US_PSP )
            {
                result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( DarkKnight, OnionKnight ) );
                result.Add( PatcherLib.Utilities.Utilities.MoveToUpperAndLowerNibbles( Unknown1, Unknown2 ) );
            }

            return result.ToArray();
        }

        public byte[] ToByteArray()
        {
            return ToByteArray( Context.US_PSP );
        }

        public override string ToString()
        {
            List<string> strings = new List<string>( 24 );
            foreach( string field in fields )
            {
                int v = ReflectionHelpers.GetFieldOrProperty<int>( this, field );
                if( v > 0 )
                {
                    strings.Add( string.Format( "{0}={1}", field, v ) );
                }
            }

            return string.Join( " | ", strings.ToArray() );
        }

        public override bool Equals( object obj )
        {
            if( obj is Requirements )
            {
                return Equals( obj as Requirements );
            }
            else
            {
                return base.Equals( obj );
            }
        }

        public bool Equals( Requirements other )
        {
            return PatcherLib.Utilities.Utilities.CompareArrays( ToByteArray(), other.ToByteArray() );
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal void WriteXml( System.Xml.XmlWriter writer )
        {
            writer.WriteAttributeString( "changed", HasChanged.ToString() );
            foreach( string field in fields )
            {
                int current = ReflectionHelpers.GetFieldOrProperty<int>( this, field );
                int def = ReflectionHelpers.GetFieldOrProperty<int>( Default, field );
                if( current > 0 || def > 0 )
                {
                    writer.WriteStartElement( field );
                    writer.WriteAttributeString( "changed", (current != def).ToString() );
                    writer.WriteAttributeString( "default", def.ToString() );
                    writer.WriteAttributeString( "value", current.ToString() );
                    writer.WriteEndElement();
                }
            }
        }

        #endregion Methods

    }
}
