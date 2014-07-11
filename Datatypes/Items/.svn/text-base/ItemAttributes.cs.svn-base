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
    /// Represents an item's attributes.
    /// </summary>
    public class ItemAttributes : IChangeable, ISupportDigest, ISupportDefault<ItemAttributes>
    {
		#region Instance Variables (1) 

        private static readonly string[] digestableProperties = new string[] {
            "PA", "MA", "Speed", "Move", "Jump", "Absorb", "Cancel", "Half", "Weak", "Strong",
            "PermanentStatuses", "StatusImmunity", "StartingStatuses" };

		#endregion Instance Variables 

		#region Public Properties (18) 

        public Elements Absorb { get; private set; }

        public Elements Cancel { get; private set; }

        public string CorrespondingItems
        {
            get
            {
                List<string> result = new List<string>();
                foreach( Item i in FFTPatch.Items.Items )
                {
                    if( i.SIA == this.Value )
                    {
                        result.Add( i.ToString() );
                    }
                }

                return string.Join( ", ", result.ToArray() );
            }
        }

        public ItemAttributes Default { get; private set; }

        public IList<string> DigestableProperties
        {
            get { return digestableProperties; }
        }

        public Elements Half { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public bool HasChanged
        {
            get
            {
                return Default != null && !PatcherLib.Utilities.Utilities.CompareArrays( ToByteArray(), Default.ToByteArray() );
            }
        }

        public byte Jump { get; set; }

        public byte MA { get; set; }

        public byte Move { get; set; }

        public byte PA { get; set; }

        public Statuses PermanentStatuses { get; private set; }

        public byte Speed { get; set; }

        public Statuses StartingStatuses { get; private set; }

        public Statuses StatusImmunity { get; private set; }

        public Elements Strong { get; private set; }

        public byte Value { get; private set; }

        public Elements Weak { get; private set; }

		#endregion Public Properties 

		#region Constructors (2) 

        public ItemAttributes( byte value, IList<byte> bytes )
            : this( value, bytes, null )
        {
        }

        public ItemAttributes( byte value, IList<byte> bytes, ItemAttributes defaults )
        {
            Value = value;
            PA = bytes[0];
            MA = bytes[1];
            Speed = bytes[2];
            Move = bytes[3];
            Jump = bytes[4];

            PermanentStatuses = new Statuses( bytes.Sub( 5, 9 ), defaults == null ? null : defaults.PermanentStatuses );
            StatusImmunity = new Statuses( bytes.Sub( 10, 14 ), defaults == null ? null : defaults.StatusImmunity );
            StartingStatuses = new Statuses( bytes.Sub( 15, 19 ), defaults == null ? null : defaults.StartingStatuses );

            Absorb = new Elements( bytes[20] );
            Cancel = new Elements( bytes[21] );
            Half = new Elements( bytes[22] );
            Weak = new Elements( bytes[23] );
            Strong = new Elements( bytes[24] );

            if( defaults != null )
            {
                Default = defaults;
                Absorb.Default = Default.Absorb;
                Cancel.Default = Default.Cancel;
                Half.Default = Default.Half;
                Weak.Default = Default.Weak;
                Strong.Default = Default.Strong;
            }
        }

		#endregion Constructors 

		#region Public Methods (4) 

        public static void Copy( ItemAttributes source, ItemAttributes destination )
        {
            destination.PA = source.PA;
            destination.MA = source.MA;
            destination.Speed = source.Speed;
            destination.Move = source.Move;
            destination.Jump = source.Jump;
            source.PermanentStatuses.CopyTo( destination.PermanentStatuses );
            source.StatusImmunity.CopyTo( destination.StatusImmunity );
            source.StartingStatuses.CopyTo( destination.StartingStatuses );
            source.Absorb.CopyTo( destination.Absorb );
            source.Cancel.CopyTo( destination.Cancel );
            source.Half.CopyTo( destination.Half );
            source.Weak.CopyTo( destination.Weak );
            source.Strong.CopyTo( destination.Strong );
        }

        public void CopyTo( ItemAttributes destination )
        {
            Copy( this, destination );
        }

        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>( 25 );
            result.Add( PA );
            result.Add( MA );
            result.Add( Speed );
            result.Add( Move );
            result.Add( Jump );
            result.AddRange( PermanentStatuses.ToByteArray() );
            result.AddRange( StatusImmunity.ToByteArray() );
            result.AddRange( StartingStatuses.ToByteArray() );
            result.Add( Absorb.ToByte() );
            result.Add( Cancel.ToByte() );
            result.Add( Half.ToByte() );
            result.Add( Weak.ToByte() );
            result.Add( Strong.ToByte() );

            return result.ToArray();
        }

        public override string ToString()
        {
            return (HasChanged ? "*" : "") + Value.ToString( "X2" );
        }

		#endregion Public Methods 
    }

    public class AllItemAttributes : PatchableFile, IXmlDigest, IGenerateCodes
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
                foreach( ItemAttributes a in ItemAttributes )
                {
                    if( a.HasChanged )
                        return true;
                }
                return false;
            }
        }

        public ItemAttributes[] ItemAttributes { get; private set; }

		#endregion Public Properties 

		#region Constructors (1) 

        public AllItemAttributes( IList<byte> first, IList<byte> second )
        {
            List<ItemAttributes> temp = new List<ItemAttributes>( 0x65 );
            IList<byte> defaultFirst = second == null ? PSXResources.Binaries.OldItemAttributes : PSPResources.Binaries.OldItemAttributes;
            IList<byte> defaultSecond = second == null ? null : PSPResources.Binaries.NewItemAttributes;

            for( byte i = 0; i < 0x50; i++ )
            {
                temp.Add( new ItemAttributes( i, first.Sub( i * 25, (i + 1) * 25 - 1 ),
                    new ItemAttributes( i, defaultFirst.Sub( i * 25, (i + 1) * 25 - 1 ) ) ) );
            }
            if( second != null )
            {
                for( byte i = 0x50; i < 0x65; i++ )
                {
                    temp.Add( new ItemAttributes( i, second.Sub( (i - 0x50) * 25, ((i - 0x50) + 1) * 25 - 1 ),
                        new ItemAttributes( i, defaultSecond.Sub( (i - 0x50) * 25, ((i - 0x50) + 1) * 25 - 1 ) ) ) );
                }
            }

            ItemAttributes = temp.ToArray();
        }

		#endregion Constructors 

		#region Public Methods (5) 

        public override IList<PatchedByteArray> GetPatches( Context context )
        {
            var result = new List<PatchedByteArray>( 4 );

            var first = ToFirstByteArray();
            if ( context == Context.US_PSX )
            {
                result.Add(PatcherLib.Iso.PsxIso.OldItemAttributes.GetPatchedByteArray(first));
            }
            else if ( context == Context.US_PSP )
            {
                var second = ToSecondByteArray();
                PatcherLib.Iso.PspIso.OldItemAttributes.ForEach(kl => result.Add(kl.GetPatchedByteArray(first)));
                PatcherLib.Iso.PspIso.NewItemAttributes.ForEach(kl => result.Add(kl.GetPatchedByteArray(second)));
            }

            return result;
        }

        public byte[] ToFirstByteArray()
        {
            List<byte> result = new List<byte>( 0x50 * 25 );
            for( int i = 0; i < 0x50; i++ )
            {
                result.AddRange( ItemAttributes[i].ToByteArray() );
            }
            return result.ToArray();
        }

        public byte[] ToSecondByteArray()
        {
            List<byte> result = new List<byte>( 0x15 * 25 );
            for( int i = 0x50; i < 0x65; i++ )
            {
                result.AddRange( ItemAttributes[i].ToByteArray() );
            }
            return result.ToArray();
        }

        public void WriteXmlDigest( System.Xml.XmlWriter writer )
        {
            if( HasChanged )
            {
                writer.WriteStartElement( this.GetType().Name );
                writer.WriteAttributeString( "changed", HasChanged.ToString() );
                foreach( ItemAttributes attr in ItemAttributes )
                {
                    if( attr.HasChanged )
                    {
                        writer.WriteStartElement( attr.GetType().Name );
                        writer.WriteAttributeString( "value", attr.Value.ToString( "X2" ) );
                        DigestGenerator.WriteXmlDigest( attr, writer, false, false );
                        writer.WriteElementString( "CorrespondingItems", attr.CorrespondingItems );
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
            return context == Context.US_PSP ? "_C0 Item Attributes" : "\"Item Attributes";
        }

        IList<string> IGenerateCodes.GenerateCodes(Context context)
        {
            if (context == Context.US_PSP)
            {
                List<string> strings = new List<string>();
                strings.AddRange( Codes.GenerateCodes( Context.US_PSP, PSPResources.Binaries.NewItemAttributes, this.ToSecondByteArray(), 0x25B1B8 ) );
                strings.AddRange( Codes.GenerateCodes( Context.US_PSP, PSPResources.Binaries.OldItemAttributes, this.ToFirstByteArray(), 0x32A694 ) );
                return strings;
            }
            else
            {
                return Codes.GenerateCodes( Context.US_PSX, PSXResources.Binaries.OldItemAttributes, this.ToFirstByteArray(), 0x0642C4 );
            }
        }

        #endregion
    }
}
