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

namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// Represents all Events in the game.
    /// </summary>
    public class ENTD : IChangeable, ISupportDefault<ENTD>
    {
		#region Public Properties (3) 

        public ENTD Default { get; private set; }

        public Event[] Events { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public bool HasChanged
        {
            get
            {
                foreach( Event e in Events )
                {
                    if( e.HasChanged )
                    {
                        return true;
                    }
                }

                return false;
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public ENTD( int start, IList<byte> bytes, ENTD defaults, Context context )
        {
            Default = defaults;
            Events = new Event[0x80];
            for( int i = 0; i < 0x80; i++ )
            {
                Events[i] = new Event(
                    i + start,
                    bytes.Sub( i * 16 * 40, (i + 1) * 16 * 40 - 1 ),
                    defaults == null ? null : defaults.Events[i],
                    context);
            }
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>( 16 * 40 * 0x80 );
            foreach( Event e in Events )
            {
                result.AddRange( e.ToByteArray() );
            }

            return result.ToArray();
        }

		#endregion Public Methods 
    }

    public class AllENTDs : PatchableFile, IXmlDigest
    {
		#region Public Properties (4) 

        public ENTD[] ENTDs { get; private set; }

        public List<Event> Events { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has changed.
        /// </summary>
        /// <value></value>
        public override bool HasChanged
        {
            get
            {
                foreach( Event e in Events )
                {
                    if( e.HasChanged )
                    {
                        return true;
                    }
                }

                if( PSPEvent != null && PSPEvent.Count > 0 )
                {
                    foreach( Event e in PSPEvent )
                    {
                        if( e.HasChanged )
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public List<Event> PSPEvent { get; private set; }

		#endregion Public Properties 

		#region Constructors

        public AllENTDs(IList<byte> entd1, IList<byte> entd2, IList<byte> entd3, IList<byte> entd4, Context context)
            : this(entd1, entd2, entd3, entd4, null, null, null, null, context) { }

        public AllENTDs( IList<byte> entd1, IList<byte> entd2, IList<byte> entd3, IList<byte> entd4, 
            IList<byte> defaultEntd1, IList<byte> defaultEntd2, IList<byte> defaultEntd3, IList<byte> defaultEntd4, Context context )
        {
            ENTDs = new ENTD[4];
            ENTDs[0] = new ENTD(
                0,
                entd1,
                new ENTD( 0, defaultEntd1 ?? PSPResources.Binaries.ENTD1, null, context ),
                context);
            ENTDs[1] = new ENTD(
                0x80,
                entd2,
                new ENTD( 0x80, defaultEntd2 ?? PSPResources.Binaries.ENTD2, null, context ),
                context);
            ENTDs[2] = new ENTD(
                0x100,
                entd3,
                new ENTD( 0x100, defaultEntd3 ?? PSPResources.Binaries.ENTD3, null, context ),
                context);
            ENTDs[3] = new ENTD(
                0x180,
                entd4,
                new ENTD( 0x180, defaultEntd4 ?? PSPResources.Binaries.ENTD4, null, context ),
                context);

            Events = new List<Event>( 0x200 );
            foreach( ENTD e in ENTDs )
            {
                Events.AddRange( e.Events );
            }
        }

        public AllENTDs(IList<byte> entd1, IList<byte> entd2, IList<byte> entd3, IList<byte> entd4, IList<byte> entd5, Context context)
            : this(entd1, entd2, entd3, entd4, entd5, null, null, null, null, null, context) { }

        public AllENTDs( IList<byte> entd1, IList<byte> entd2, IList<byte> entd3, IList<byte> entd4, IList<byte> entd5,
            IList<byte> defaultEntd1, IList<byte> defaultEntd2, IList<byte> defaultEntd3, IList<byte> defaultEntd4, IList<byte> defaultEntd5, Context context)
            : this( entd1, entd2, entd3, entd4, defaultEntd1, defaultEntd2, defaultEntd3, defaultEntd4, context )
        {
            if( context == Context.US_PSP )
            {
                PSPEvent = new List<Event>( 77 );
                for( int i = 0; i < 77; i++ )
                {
                    PSPEvent.Add( new Event( 0x200 + i, entd5.Sub( i * 16 * 40, (i + 1) * 16 * 40 - 1 ),
                                  new Event( 0x200 + i, (defaultEntd5 ?? PSPResources.Binaries.ENTD5).Sub( i * 16 * 40, ( i + 1 ) * 16 * 40 - 1 ), null, context ),
                                  context) );
                }

                Events.AddRange( PSPEvent );
            }
        }

		#endregion Constructors 

		#region Public Methods (3) 

        public override IList<PatchedByteArray> GetPatches( Context context )
        {
            var result = new List<PatchedByteArray>( 5 );

            var bytes1 = ENTDs[0].ToByteArray();
            var bytes2 = ENTDs[1].ToByteArray();
            var bytes3 = ENTDs[2].ToByteArray();
            var bytes4 = ENTDs[3].ToByteArray();

            if ( context == Context.US_PSX )
            {
                result.Add(PatcherLib.Iso.PsxIso.ENTD1.GetPatchedByteArray(bytes1));
                result.Add(PatcherLib.Iso.PsxIso.ENTD2.GetPatchedByteArray(bytes2));
                result.Add(PatcherLib.Iso.PsxIso.ENTD3.GetPatchedByteArray(bytes3));
                result.Add(PatcherLib.Iso.PsxIso.ENTD4.GetPatchedByteArray(bytes4));
            }
            else if ( context == Context.US_PSP )
            {
                result.Add(PatcherLib.Iso.PspIso.ENTD1.GetPatchedByteArray(bytes1));
                result.Add(PatcherLib.Iso.PspIso.ENTD2.GetPatchedByteArray(bytes2));
                result.Add(PatcherLib.Iso.PspIso.ENTD3.GetPatchedByteArray(bytes3));
                result.Add(PatcherLib.Iso.PspIso.ENTD4.GetPatchedByteArray(bytes4));
                result.Add(PatcherLib.Iso.PspIso.ENTD5.GetPatchedByteArray(PSPEventsToByteArray()));
            }

            return result;
        }

        public byte[] PSPEventsToByteArray()
        {
            List<byte> result = new List<byte>();
            if( PSPEvent != null )
            {
                foreach( Event e in PSPEvent )
                {
                    result.AddRange( e.ToByteArray() );
                }
                result.AddRange( new byte[1920] );
            }

            return result.ToArray();
        }

        public void WriteXmlDigest(System.Xml.XmlWriter writer, FFTPatch FFTPatch)
        {
            if( HasChanged )
            {
                writer.WriteStartElement( GetType().Name );
                writer.WriteAttributeString( "changed", HasChanged.ToString() );
                foreach( Event e in Events )
                {
                    e.WriteXmlDigest( writer, FFTPatch );
                }
                writer.WriteEndElement();
            }
        }

		#endregion Public Methods 
    }
}
