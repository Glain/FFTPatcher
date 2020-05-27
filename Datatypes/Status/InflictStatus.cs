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
    /// Represents statuses an <see cref="Ability"/> inflicts or cancels on its target.
    /// </summary>
    public class InflictStatus : ISupportDigest, ISupportDefault<InflictStatus>, ICheckDuplicate<InflictStatus>
    {
		#region Instance Variables (9) 

        public bool AllOrNothing;
        public bool Blank1;
        public bool Blank2;
        public bool Blank3;
        public bool Blank4;
        public bool Cancel;
        public static readonly string[] digestableProperties = new string[] {
            "AllOrNothing", "Random", "Separate", "Cancel", "Blank1", "Blank2", "Blank3",
            "Blank4", "Statuses" };
        public bool Random;
        public bool Separate;

		#endregion Instance Variables 

		#region Public Properties

        public InflictStatus Default { get; private set; }

        public IList<string> DigestableProperties
        {
            get { return digestableProperties; }
        }

        private HashSet<int> referencingAbilityIDs;
        public HashSet<int> ReferencingAbilityIDs
        {
            get
            {
                if (referencingAbilityIDs == null)
                    referencingAbilityIDs = new HashSet<int>();

                return referencingAbilityIDs;
            }
        }

        private HashSet<int> referencingItemIDs;
        public HashSet<int> ReferencingItemIDs
        {
            get
            {
                if (referencingItemIDs == null)
                    referencingItemIDs = new HashSet<int>();

                return referencingItemIDs;
            }
        }

        public bool IsInUse 
        {
            get
            {
                return ((ReferencingAbilityIDs.Count > 0) || (ReferencingItemIDs.Count > 0));
            }
        }
        public bool IsDuplicate { get; set; }
        public int Index { get; set; }
        public int DuplicateIndex { get; set; }

        public bool HasChanged
        {
            get
            {
                return Default != null &&
                    (AllOrNothing != Default.AllOrNothing ||
                    Random != Default.Random ||
                    Separate != Default.Separate ||
                    Cancel != Default.Cancel ||
                    Blank1 != Default.Blank1 ||
                    Blank2 != Default.Blank2 ||
                    Blank3 != Default.Blank3 ||
                    Blank4 != Default.Blank4 ||
                    Statuses.HasChanged);
            }
        }

        public Statuses Statuses { get; private set; }

        public byte Value { get; private set; }

		#endregion Public Properties 

		#region Constructors (2) 

        public InflictStatus( byte value, IList<byte> bytes )
            : this( value, bytes, null )
        {
        }

        public InflictStatus( byte value, IList<byte> bytes, InflictStatus defaults )
        {
            Default = defaults;
            Value = value;
            PatcherLib.Utilities.Utilities.CopyByteToBooleans( bytes[0], ref AllOrNothing, ref Random, ref Separate, ref Cancel, ref Blank1, ref Blank2, ref Blank3, ref Blank4 );
            Statuses = new Statuses( bytes.Sub( 1, 5 ), defaults == null ? null : defaults.Statuses );
        }

		#endregion Constructors 

		#region Public Methods

        public static void Copy( InflictStatus source, InflictStatus destination )
        {
            source.Statuses.CopyTo( destination.Statuses );
            destination.AllOrNothing = source.AllOrNothing;
            destination.Random = source.Random;
            destination.Separate = source.Separate;
            destination.Cancel = source.Cancel;
            destination.Blank1 = source.Blank1;
            destination.Blank2 = source.Blank2;
            destination.Blank3 = source.Blank3;
            destination.Blank4 = source.Blank4;
        }

        public void CopyTo( InflictStatus destination )
        {
            Copy( this, destination );
        }

        public bool[] ToBoolArray()
        {
            return new bool[8] { 
                AllOrNothing, Random, Separate, Cancel, Blank1, Blank2, Blank3, Blank4 };
        }

        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>( 6 );
            result.Add( PatcherLib.Utilities.Utilities.ByteFromBooleans( AllOrNothing, Random, Separate, Cancel, Blank1, Blank2, Blank3, Blank4 ) );
            result.AddRange( Statuses.ToByteArray() );
            return result.ToArray();
        }

        public bool CheckDuplicate(InflictStatus compareObject)
        {
            return PatcherLib.Utilities.Utilities.CompareArrays<byte>(ToByteArray(), compareObject.ToByteArray());
        }

        public override string ToString()
        {
            return (HasChanged ? "*" : "") + Value.ToString( "X2" );
        }

        public string GetCorrespondingAbilities(FFTPatch FFTPatch)
        {
            List<string> result = new List<string>();
            foreach (Ability a in FFTPatch.Abilities.Abilities)
            {
                if (a.Attributes != null && a.Attributes.Formula.Value != 0x02 && a.Attributes.InflictStatus == Value)
                {
                    result.Add(a.ToString());
                }
            }

            return string.Join(", ", result.ToArray());
        }

        public string GetCorrespondingChemistItems(FFTPatch FFTPatch)
        {
            List<string> result = new List<string>();
            foreach (Item i in FFTPatch.Items.Items)
            {
                if (i is ChemistItem && (i as ChemistItem).Formula != 0x02 && (i as ChemistItem).InflictStatus == Value)
                {
                    result.Add(i.ToString());
                }
            }

            return string.Join(", ", result.ToArray());
        }

        public string GetCorrespondingWeapons(FFTPatch FFTPatch)
        {
            List<string> result = new List<string>();
            foreach (Item i in FFTPatch.Items.Items)
            {
                if (i is Weapon && (i as Weapon).Formula.Value != 0x02 && (i as Weapon).InflictStatus == Value)
                {
                    result.Add(i.ToString());
                }
            }

            return string.Join(", ", result.ToArray());
        }

		#endregion Public Methods 
    }

    public class AllInflictStatuses : PatchableFile, IXmlDigest, IGenerateCodes
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
                foreach( InflictStatus s in InflictStatuses )
                {
                    if ( s.Default != null && !PatcherLib.Utilities.Utilities.CompareArrays( s.ToByteArray(), s.Default.ToByteArray() ) )
                        return true;
                }

                return false;
            }
        }

        public InflictStatus[] InflictStatuses { get; private set; }

		#endregion Public Properties 

		#region Constructors (1) 

        public AllInflictStatuses( IList<byte> bytes, Context context )
        {
            IList<byte> defaultBytes = context == Context.US_PSP ? PSPResources.Binaries.InflictStatuses : PSXResources.Binaries.InflictStatuses;
            InflictStatuses = new InflictStatus[0x80];
            for( int i = 0; i < 0x80; i++ )
            {
                InflictStatuses[i] = new InflictStatus( (byte)i, bytes.Sub( i * 6, (i + 1) * 6 - 1 ),
                    new InflictStatus( (byte)i, defaultBytes.Sub( i * 6, (i + 1) * 6 - 1 ) ) );
            }
        }

		#endregion Constructors 

		#region Public Methods (4) 

        public override IList<PatchedByteArray> GetPatches( Context context )
        {
            var result = new List<PatchedByteArray>( 2 );

            var bytes = ToByteArray();
            if ( context == Context.US_PSX )
            {
                result.Add(PatcherLib.Iso.PsxIso.InflictStatuses.GetPatchedByteArray(bytes));
            }
            else if ( context == Context.US_PSP )
            {
                PatcherLib.Iso.PspIso.InflictStatuses.ForEach(kl => result.Add(kl.GetPatchedByteArray(bytes)));
            }

            return result;
        }

        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>( 0x300 );
            foreach( InflictStatus i in InflictStatuses )
            {
                result.AddRange( i.ToByteArray() );
            }

            return result.ToArray();
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            if( HasChanged )
            {
                writer.WriteStartElement( this.GetType().Name );
                writer.WriteAttributeString( "changed", HasChanged.ToString() );
                foreach( InflictStatus i in InflictStatuses )
                {
                    if( i.HasChanged )
                    {
                        writer.WriteStartElement( i.GetType().Name );
                        writer.WriteAttributeString( "value", i.Value.ToString( "X2" ) );
                        DigestGenerator.WriteXmlDigest( i, writer, false, false );
                        writer.WriteElementString( "CorrespondingAbilities", i.GetCorrespondingAbilities(FFTPatch) );
                        writer.WriteElementString( "CorrespondingChemistItems", i.GetCorrespondingChemistItems(FFTPatch) );
                        writer.WriteElementString( "CorrespondingWeapons", i.GetCorrespondingWeapons(FFTPatch) );
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
            return context == Context.US_PSP ? "_C0 Inflict Statuses" : "\"Inflict Statuses";
        }

        IList<string> IGenerateCodes.GenerateCodes(Context context)
        {
            if (context == Context.US_PSP)
            {
                return Codes.GenerateCodes( Context.US_PSP, PSPResources.Binaries.InflictStatuses, this.ToByteArray(), 0x32A394 );
            }
            else
            {
                return Codes.GenerateCodes( Context.US_PSX, PSXResources.Binaries.InflictStatuses, this.ToByteArray(), 0x063FC4 );
            }
        }

        #endregion
    }
}
