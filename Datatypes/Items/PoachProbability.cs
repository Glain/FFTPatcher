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
    /// Represent's the common and uncommon items that can be poached from a monster.
    /// </summary>
    public class PoachProbability : IChangeable, ISupportDigest, ISupportDefault<PoachProbability>
    {
		#region Instance Variables (1) 

        private static readonly string[] digestableProperties = new string[] {
            "Common", "Uncommon" };

		#endregion Instance Variables 

		#region Public Properties (6) 

        public Item Common { get; set; }

        public PoachProbability Default { get; private set; }

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
                return
                    Default != null &&
                    (Common.Offset != Default.Common.Offset ||
                    Uncommon.Offset != Default.Uncommon.Offset);
            }
        }

        public string MonsterName { get; private set; }

        public Item Uncommon { get; set; }

		#endregion Public Properties 

		#region Constructors (2) 

        public PoachProbability( string name, IList<byte> bytes, Context context )
            : this( name, bytes, null, context )
        {
        }

        public PoachProbability( string name, IList<byte> bytes, PoachProbability defaults, Context context )
        {
            Default = defaults;
            MonsterName = name;
            Common = Item.GetItemAtOffset( bytes[0], context );
            Uncommon = Item.GetItemAtOffset( bytes[1], context );
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public byte[] ToByteArray()
        {
            byte[] result = new byte[2];
            result[0] = (byte)(Common.Offset & 0xFF);
            result[1] = (byte)(Uncommon.Offset & 0xFF);
            return result;
        }

        public static void CopyAll(PoachProbability source, PoachProbability destination)
        {
            destination.Common = source.Common;
            destination.Uncommon = source.Uncommon;
        }

        public void CopyAllTo(PoachProbability destination)
        {
            CopyAll(this, destination);
        }

		#endregion Public Methods 
    }

    public class AllPoachProbabilities : PatchableFile, IXmlDigest, IGenerateCodes
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
                foreach( PoachProbability p in PoachProbabilities )
                {
                    if( p.HasChanged )
                        return true;
                }

                return false;
            }
        }

        public PoachProbability[] PoachProbabilities { get; private set; }

		#endregion Public Properties 

		#region Constructors (1) 

        public AllPoachProbabilities( IList<byte> bytes, Context context )
        {
            IList<byte> defaultBytes = context == Context.US_PSP ? PSPResources.Binaries.PoachProbabilities : PSXResources.Binaries.PoachProbabilities;

            PoachProbabilities = new PoachProbability[48];
            for( int i = 0; i < 48; i++ )
            {
                PoachProbabilities[i] = new PoachProbability( AllJobs.GetNames(context)[i + 0x5E], bytes.Sub( i * 2, i * 2 + 1 ),
                    new PoachProbability( AllJobs.GetNames(context)[i + 0x5E], defaultBytes.Sub( i * 2, i * 2 + 1 ), context ), context );
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
                result.Add(PatcherLib.Iso.PsxIso.PoachProbabilities.GetPatchedByteArray(bytes));
            }
            else if ( context == Context.US_PSP )
            {
                PatcherLib.Iso.PspIso.PoachProbabilities.ForEach(kl => result.Add(kl.GetPatchedByteArray(bytes)));
            }

            return result;
        }

        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>( 96 );
            foreach( PoachProbability p in PoachProbabilities )
            {
                result.AddRange( p.ToByteArray() );
            }

            return result.ToArray();
        }

        public byte[] ToByteArray( Context context )
        {
            return ToByteArray();
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            if( HasChanged )
            {
                writer.WriteStartElement( this.GetType().Name );
                writer.WriteAttributeString( "changed", HasChanged.ToString() );
                foreach( PoachProbability p in PoachProbabilities )
                {
                    if( p.HasChanged )
                    {
                        writer.WriteStartElement( p.GetType().Name );
                        writer.WriteAttributeString( "name", p.MonsterName );
                        DigestGenerator.WriteXmlDigest( p, writer, false, true );
                    }
                }
                writer.WriteEndElement();
            }
        }

		#endregion Public Methods 
    
        #region IGenerateCodes Members

        string IGenerateCodes.GetCodeHeader(Context context)
        {
            return context == Context.US_PSP ? "_C0 Poaching" : "\"Poaching";
        }

        IList<string> IGenerateCodes.GenerateCodes(Context context)
        {
            if (context == Context.US_PSP)
            {
                return Codes.GenerateCodes( Context.US_PSP, PSPResources.Binaries.PoachProbabilities, this.ToByteArray(), 0x27AFD0 );
            }
            else
            {
                return Codes.GenerateCodes( Context.US_PSX, PSXResources.Binaries.PoachProbabilities, this.ToByteArray(), 0x066064 );
            }
        }

        #endregion
    }
}
