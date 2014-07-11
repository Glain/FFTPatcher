using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PatcherLib.Iso
{
    [System.Diagnostics.DebuggerDisplay( "{Filename} - {Sector} - {Size} - {Timestamp}" )]
    public class DirectoryEntry
    {
        public static IList<DirectoryEntry> GetPspDirectoryEntries( Stream iso, PatcherLib.Iso.PspIso.PspIsoInfo info, PspIso.Sectors sectorOfParentEntry, int numSectors )
        {
            int length = numSectors;
            List<DirectoryEntry> result = new List<DirectoryEntry>();
            var bytes = PspIso.GetBlock( iso, info,
                new PspIso.KnownPosition( sectorOfParentEntry, 0, numSectors * 2048 ) );
            return BuildDirectoryEntriesFromBytes( bytes );
        }

        private static IList<DirectoryEntry> BuildDirectoryEntriesFromBytes( IList<byte> bytes )
        {
            List<DirectoryEntry> result = new List<DirectoryEntry>();
            for (int i = 0; i < bytes.Count; i++)
            {
                if (bytes[i] == 0) continue;

                IList<byte> entry = bytes.Sub( i, i + bytes[i] - 1 );
                result.Add( new DirectoryEntry( entry ) );
                i += bytes[i];
                i--;
            }
            return result;
        }

        public static IList<DirectoryEntry> GetPsxDirectoryEntries( Stream iso, int sectorOfParentEntry, int numSectors )
        {
            int sector = sectorOfParentEntry;
            int length = numSectors;
            byte[] bytes = PsxIso.GetBlock( iso, new PsxIso.KnownPosition( (PatcherLib.Iso.PsxIso.Sectors)sector, 0, length * 2048 ) );
            return BuildDirectoryEntriesFromBytes( bytes );
        }

        public static IList<DirectoryEntry> GetPsxBattleEntries( Stream iso )
        {
            const int sector = PsxIso.BattleDirectoryEntrySector;
            const int length = PsxIso.BattleDirectoryEntryLength;
            return GetPsxDirectoryEntries( iso, sector, length );
        }

        public static IList<DirectoryEntry> GetPsxDummyEntries( Stream iso )
        {
            const int sector = PsxIso.DummyDirectoryEntrySector;
            const int length = PsxIso.DummyDirectoryEntryLength;
            return GetPsxDirectoryEntries( iso, sector, length );
        }

        public static IList<PatchedByteArray> GetPspDirectoryEntryPatches( int sector, int numSectors, IList<DirectoryEntry> entries )
        {
            var sectors = GetDirectoryEntrySectors( numSectors, entries );
            List<PatchedByteArray> result = new List<PatchedByteArray>();
            for (int i = 0; i < numSectors; i++)
            {
                result.Add(
                    new PatchedByteArray(
                        (PatcherLib.Iso.PspIso.Sectors)(sector + i),
                        0,
                        sectors[i].ToArray() ) );

            }
            return result;
        }

        private static IList<IList<byte>> GetDirectoryEntrySectors( int numSectors, IList<DirectoryEntry> entries )
        {
            byte[][] dirEntryBytes = new byte[entries.Count][];
            for (int i = 0; i < entries.Count; i++)
            {
                dirEntryBytes[i] = entries[i].ToByteArray();
            }

            List<byte>[] sectors = new List<byte>[numSectors];
            for (int i = 0; i < sectors.Length; i++)
                sectors[i] = new List<byte>( 2048 );
            int currentSector = 0;
            foreach (byte[] entry in dirEntryBytes)
            {
                if (sectors[currentSector].Count + entry.Length > 2048)
                {
                    currentSector++;
                }
                if (currentSector >= numSectors)
                    throw new InvalidOperationException( "not enough sectors for all directory entries" );
                sectors[currentSector].AddRange( entry );
            }
            foreach (List<byte> sec in sectors)
            {
                sec.AddRange( new byte[2048 - sec.Count] );
            }

            return sectors;
        }

        public static IList<PatchedByteArray> GetPsxDirectoryEntryPatches( int sector, int numSectors, IList<DirectoryEntry> entries )
        {
            var sectors = GetDirectoryEntrySectors( numSectors, entries );
            List<PatchedByteArray> result = new List<PatchedByteArray>();
            for (int i = 0; i < numSectors; i++)
            {
                result.Add(
                    new PatchedByteArray(
                        (PatcherLib.Iso.PsxIso.Sectors)(sector + i),
                        0,
                        sectors[i].ToArray() ) );

            }
            return result;
        }

        public static void WritePsxDirectoryEntries( Stream iso, int sector, int numSectors, IList<DirectoryEntry> entries )
        {
            var patches = GetPsxDirectoryEntryPatches( sector, numSectors, entries );
            patches.ForEach( p => PsxIso.PatchPsxIso( iso, patches ) );
        }

        public string Filename { get; set; }
        public UInt32 Size { get; set; }
        public UInt32 Sector { get; set; }
        public DateTime Timestamp { get; set; }
        public byte[] MiddleBytes { get; private set; }
        public byte[] ExtendedBytes { get; private set; }
        public byte GMTOffset { get; set; }

        public DirectoryEntry( UInt32 sector, UInt32 size, DateTime timestamp, byte gmtOffset, IList<byte> middleBytes, string filename, IList<byte> extendedBytes )
        {
            if (middleBytes.Count != 7)
                throw new ArgumentException( "middleBytes" );
            if (extendedBytes.Count != 0x0e)
                throw new ArgumentException( "extendedBytes" );
            Sector = sector;
            Size = size;
            Timestamp = timestamp;
            GMTOffset = gmtOffset;
            MiddleBytes = middleBytes.ToArray();
            ExtendedBytes = extendedBytes.ToArray();
            Filename = filename;
        }

        public DirectoryEntry( IList<byte> bytes )
        {
            System.Diagnostics.Debug.Assert( bytes[0] == bytes.Count );
            Sector = bytes.Sub( 2, 2 + 4 - 1 ).ToUInt32();
            Size = bytes.Sub( 10, 10 + 4 - 1 ).ToUInt32();
            Timestamp = new DateTime( bytes[18] + 1900, bytes[19], bytes[20], bytes[21], bytes[22], bytes[23] );
            GMTOffset = bytes[24];
            MiddleBytes = bytes.Sub( 25, 25 + 7 - 1 ).ToArray();
            byte nameLength = bytes[32];
            Filename = System.Text.Encoding.ASCII.GetString( bytes.Sub( 33, 33 + nameLength - 1 ).ToArray() );
            byte padding = (byte)(((nameLength % 2) == 0) ? 1 : 0);
            ExtendedBytes = bytes.Sub( 33 + nameLength + padding, bytes[0] - 1 ).ToArray();
        }

        public byte[] ToByteArray()
        {
            List<byte> result = new List<byte>();
            result.Add( 0x00 );
            byte[] sectorBytes = Sector.ToBytes();
            result.AddRange( sectorBytes );
            result.AddRange( new byte[] { sectorBytes[3], sectorBytes[2], sectorBytes[1], sectorBytes[0] } );
            byte[] sizeBytes = Size.ToBytes();
            result.AddRange( sizeBytes );
            result.AddRange( new byte[] { sizeBytes[3], sizeBytes[2], sizeBytes[1], sizeBytes[0] } );
            result.Add( (byte)(Timestamp.Year - 1900) );
            result.Add( (byte)Timestamp.Month );
            result.Add( (byte)Timestamp.Day );
            result.Add( (byte)Timestamp.Hour );
            result.Add( (byte)Timestamp.Minute );
            result.Add( (byte)Timestamp.Second );
            result.Add( GMTOffset );
            result.AddRange( MiddleBytes );
            byte[] nameBytes = Filename.ToByteArray();
            result.Add( (byte)nameBytes.Length );
            result.AddRange( nameBytes );
            if (((byte)nameBytes.Length) % 2 == 0)
            {
                result.Add( 0x00 );
            }
            result.AddRange( ExtendedBytes );
            byte length = (byte)(result.Count + 1);
            result.Insert( 0, length );

            return result.ToArray();
        }

        //38 // length of record
        //00 // nothing
        //D6 E9 00 00 00 00 E9 D6 // sector
        //01 92 00 00 00 00 92 01 // size
        //61 // year
        //05 // month
        //10 // day
        //12 // hour
        //15 // minutes
        //1E // seconds
        //24 // GMT offset
        //01 // hidden file
        //00 00 
        //01 00 00 01 
        //09 // name length
        //31 30 4D 2E 53 50 52 3B 31 // name 10M.SPR;1

        //2A 00 2A 00 // owner id
        //08 01 // attributes
        //58 41  // X A
        //00  // file number
        //00  00 00  00 00 // reserved         }

    }
}