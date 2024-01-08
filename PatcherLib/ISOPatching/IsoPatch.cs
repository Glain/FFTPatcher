/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.
    ISO patching/ECC/EDC generation lifted from Agemo's isopatcherv05

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
using System.IO;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace PatcherLib.Iso
{
    public static class IsoPatch
    {
		#region Instance Variables (7) 

        private static readonly int[] dataSizes = new int[3] { 2048, 2048, 2324 };
        private static readonly int[] dataStarts = new int[3] { 0, 0x18, 0x18 };
        private static byte[] eccBLUT = new byte[256];
        private static byte[] eccFLUT = new byte[256];
        private static ulong[] edcLUT = new ulong[256];
        private const int fullSectorSize = 2352;    
        private static readonly int[] sectorSizes = new int[3] { 2048, fullSectorSize, fullSectorSize };

        private static IDictionary<IsoType, int> sectorSizeDict;
        public static IDictionary<IsoType, int> SectorSizes
        {
            get
            {
                if (sectorSizeDict == null)
                {
                    sectorSizeDict = new ReadOnlyDictionary<IsoType, int>(
                        new Dictionary<IsoType, int>{
                            { IsoType.Mode1, 2048 },
                            { IsoType.Mode2Form1, 2352 },
                            { IsoType.Mode2Form2, 2352 } });
                }
                return sectorSizeDict;
            }
        }

		#endregion Instance Variables 

		#region Constructors

        static IsoPatch()
        {
            InitIsoPatch();
        }

		#endregion Constructors 

        private static void InitIsoPatch()
        {
            uint j = 0;
            ulong edc = 0;
            for (uint i = 0; i < 256; i++)
            {
                j = (uint)((i << 1) ^ ((i & 0x80) == 0x80 ? 0x11D : 0));
                eccFLUT[i] = (byte)j;
                eccBLUT[i ^ j] = (byte)i;
                edc = i;
                for (j = 0; j < 8; j++)
                {
                    edc = (edc >> 1) ^ ((edc & 1) == 1 ? 0xD8018001 : 0);
                }
                edcLUT[i] = edc;
            }
        }

		#region Public Methods (8) 

        public static void FixupECC( IsoType isoType, Stream iso )
        {
            int type = (int)isoType;
            int sectorSize = sectorSizes[type];
            byte[] sector = new byte[sectorSize];

            if ( iso.Length % sectorSize != 0 )
            {
                throw new ArgumentException( "ISO does not have correct length for its type", "isoType" );
            }
            if ( isoType == IsoType.Mode1 )
            {
                throw new ArgumentException( "Mode1 does not support ECC/EDC", "isoType" );
            }

            Int64 numberOfSectors = iso.Length / sectorSize;
            iso.Seek( 0, SeekOrigin.Begin );
            for ( Int64 i = 0; i < numberOfSectors; i++ )
            {
                iso.Read( sector, 0, sectorSize );

                if ( isoType != IsoType.Mode1 && ( sector[0x12] & 8 ) == 0 )
                {
                    sector[0x12] = 8;
                    sector[0x16] = 8;
                }

                GenerateEccEdc( sector, isoType );
                iso.Seek( -sectorSize, SeekOrigin.Current );
                iso.Write( sector, 0, sectorSize );
            }
        }

        /// <summary>
        /// Patches the bytes at a given offset.
        /// </summary>
        /// <param name="isoType">The type of ISO</param>
        /// <param name="isoFile">Path to the ISO image</param>
        /// <param name="patchEccEdc">Whether or not ECC/EDC blocks should be updated</param>
        /// <param name="offset">Where in the ISO to start writing</param>
        /// <param name="input">Bytes to write</param>
        public static void PatchFile( IsoType isoType, string isoFile, bool patchEccEdc, long offset, IList<byte> input, bool patchIso, byte maskWrite)
        {
            using( FileStream stream = new FileStream( isoFile, FileMode.Open ) )
            {
                PatchFile( isoType, stream, patchEccEdc, offset, input, patchIso, maskWrite);
            }
        }

        /// <summary>
        /// Patches the bytes at a given offset.
        /// </summary>
        /// <param name="isoType">The type of ISO</param>
        /// <param name="iso">Stream that contains the ISO</param>
        /// <param name="patchEccEdc">Whether or not ECC/EDC blocks should be updated</param>
        /// <param name="offset">Where in the ISO to start writing</param>
        /// <param name="input">Bytes to write</param>
        public static void PatchFile( IsoType isoType, Stream iso, bool patchEccEdc, long offset, IList<byte> input, bool patchIso, byte maskWrite )
        {
            using ( MemoryStream inputStream = new MemoryStream( input.ToArray() ) )
            {
                PatchFile( isoType, iso, patchEccEdc, offset, inputStream, patchIso, maskWrite );
            }
        }

        /// <summary>
        /// Patches the bytes at a given offset.
        /// </summary>
        /// <param name="isoType">The type of ISO</param>
        /// <param name="iso">Stream that contains the ISO</param>
        /// <param name="patchEccEdc">Whether or not ECC/EDC blocks should be updated</param>
        /// <param name="offset">Where in the ISO to start writing</param>
        /// <param name="input">Stream that contains the bytes to write</param>
        public static void PatchFile( IsoType isoType, Stream iso, bool patchEccEdc, long offset, Stream input, bool patchIso, byte maskWrite )
        {
            int type = (int)isoType;
            int sectorSize = sectorSizes[type];
            byte[] sector = new byte[sectorSize];

            long sectorStart = offset % sectorSize;
            if( sectorStart < dataStarts[type] ||
                sectorStart >= (dataStarts[type] + dataSizes[type]) )
            {
                throw new ArgumentException( "start offset is incorrect", "offset" );
            }
            if( patchEccEdc && isoType == IsoType.Mode1 )
            {
                throw new ArgumentException( "Mode1 does not support ECC/EDC", "patchEccEdc" );
            }

            long sectorLength = dataSizes[type] + dataStarts[type] - sectorStart;
            int totalPatchedBytes = 0;
            int sizeRead = 0;
            long temp = offset - (offset % sectorSize);
            iso.Seek( temp, SeekOrigin.Begin );

            input.Seek( 0, SeekOrigin.Begin );
            while( input.Position < input.Length )
            {
                iso.Read( sector, 0, sectorSize );
                byte[] originalSector = sector.ToArray();

                sizeRead = input.Read( sector, (int)sectorStart, (int)sectorLength );

                if (maskWrite > 0)
                {
                    for (int index = 0; index < sectorSize; index++)
                    {
                        sector[index] = (byte)((sector[index] & maskWrite) | (originalSector[index] & ~maskWrite));
                    }
                }

                if( isoType != IsoType.Mode1 && (sector[0x12] & 8) == 0 )
                {
                    sector[0x12] = 8;
                    sector[0x16] = 8;
                }

                if( patchEccEdc )
                {
                    GenerateEccEdc( sector, isoType );
                }

                iso.Seek( -sectorSize, SeekOrigin.Current );

                if( patchIso )
                {
                    iso.Write( sector, 0, sectorSize );
                }
                else
                {
                    iso.Seek( sectorSize, SeekOrigin.Current );
                }

                totalPatchedBytes += sizeRead;
                sectorStart = dataStarts[type];
                sectorLength = dataSizes[type];
            }
        }

        /// <summary>
        /// Patches the file at a given sector in an ISO.
        /// </summary>
        /// <param name="isoType">The type of ISO</param>
        /// <param name="isoFile">Path to the ISO image</param>
        /// <param name="patchEccEdc">Whether or not ECC/EDC blocks should be updated</param>
        /// <param name="sectorNumber">The sector number where the file begins</param>
        /// <param name="input">Bytes to write</param>
        public static void PatchFileAtSector( IsoType isoType, string isoFile, bool patchEccEdc, int sectorNumber, IList<byte> input, 
            bool patchIso, byte maskWrite )
        {
            PatchFileAtSector( isoType, isoFile, patchEccEdc, sectorNumber, 0, input, patchIso, maskWrite );
        }

        /// <summary>
        /// Patches the file at a given sector in an ISO.
        /// </summary>
        /// <param name="isoType">The type of ISO</param>
        /// <param name="isoFile">Path to the ISO image</param>
        /// <param name="patchEccEdc">Whether or not ECC/EDC blocks should be updated</param>
        /// <param name="sectorNumber">The sector number where the file begins</param>
        /// <param name="offset">Where in the file to start writing</param>
        /// <param name="input">Bytes to write</param>
        public static void PatchFileAtSector( IsoType isoType, string isoFile, bool patchEccEdc, int sectorNumber, long offset, 
            IList<byte> input, bool patchIso, byte maskWrite)
        {
            using ( FileStream stream = new FileStream( isoFile, FileMode.Open ) )
            {
                PatchFileAtSector( isoType, stream, patchEccEdc, sectorNumber, offset, input, patchIso, maskWrite );
            }
        }

        /// <summary>
        /// Patches the file at a given sector in an ISO.
        /// </summary>
        /// <param name="isoType">The type of ISO</param>
        /// <param name="iso">Stream that contains the ISO</param>
        /// <param name="patchEccEdc">Whether or not ECC/EDC blocks should be updated</param>
        /// <param name="sectorNumber">The sector number where the file begins</param>
        /// <param name="offset">Where in the file to start writing</param>
        /// <param name="input">Bytes to write</param>
        public static void PatchFileAtSector( IsoType isoType, Stream iso, bool patchEccEdc, int sectorNumber, long offset, 
            IList<byte> input, bool patchIso, byte maskWrite)
        {
            int dataSize = dataSizes[(int)isoType];
            int dataStart = dataStarts[(int)isoType];
            int sectorSize = sectorSizes[(int)isoType];

            long sectorsToAdvance = offset / dataSize;
            long newOffset = offset % dataSize;

            long realOffset = (sectorNumber + sectorsToAdvance) * sectorSize + dataStart + newOffset;

            PatchFile( isoType, iso, patchEccEdc, realOffset, input, patchIso, maskWrite );
        }

        public static byte[] ReadFile( IsoType isoType, Stream iso, int fileSector, int offset, int length )
        {
            int dataSize = dataSizes[(int)isoType];
            int dataStart = dataStarts[(int)isoType];
            int sectorSize = sectorSizes[(int)isoType];

            int desiredStartSector = fileSector + offset / dataSize;
            int startOffset = offset % dataSize;

            int bytesLeftInFirstSector = dataSize - startOffset;

            iso.Seek( desiredStartSector * sectorSize + dataStart + startOffset, SeekOrigin.Begin );

            byte[] result = new byte[length];

            int bytesRead = iso.Read( result, 0, Math.Min( bytesLeftInFirstSector, length ) );
            desiredStartSector++;

            while ( bytesRead < length )
            {
               iso.Seek( desiredStartSector * sectorSize + dataStart, SeekOrigin.Begin );
               bytesRead += iso.Read( result, bytesRead, Math.Min( length - bytesRead, dataSize) );
               desiredStartSector++;
            }

            return result;
        }

		#endregion Public Methods 

		#region Private Methods (4) 

        private static void ComputeEccBlock( IList<byte> source, uint majorCount, uint minorCount, uint majorMult, uint minorIncrement, IList<byte> destination )
        {
            ulong size = majorCount * minorCount;
            uint major = 0;
            uint minor = 0;
            for( major = 0; major < majorCount; major++ )
            {
                ulong i = (major >> 1) * majorMult + (major & 1);
                byte eccA = 0;
                byte eccB = 0;
                for( minor = 0; minor < minorCount; minor++ )
                {
                    byte t = source[(int)i];
                    i += minorIncrement;
                    if( i >= size ) i -= size;
                    eccA ^= t;
                    eccB ^= t;
                    eccA = eccFLUT[eccA];
                }

                eccA = eccBLUT[eccFLUT[eccA] ^ eccB];
                destination[(int)major] = eccA;
                destination[(int)(major + majorCount)] = (byte)(eccA ^ eccB);
            }
        }

        private static void ComputeEdcBlock( IList<byte> source, int size, IList<byte> destination )
        {
            ulong edc = 0;
            for( int i = 0; i < size; i++ )
            {
                edc = (edc >> 8) ^ edcLUT[(edc ^ source[i]) & 0xFF];
            }
            destination[0] = (byte)((edc >> 0) & 0xFF);
            destination[1] = (byte)((edc >> 8) & 0xFF);
            destination[2] = (byte)((edc >> 16) & 0xFF);
            destination[3] = (byte)((edc >> 24) & 0xFF);
        }

        private static void GenerateEcc( IList<byte> sector, bool zeroAddress )
        {
            byte[] address = new byte[4];
            byte i = 0;
            if( zeroAddress )
            {
                for( i = 0; i < 4; i++ )
                {
                    address[i] = sector[12 + i];
                    sector[12 + i] = 0;
                }
            }

            ComputeEccBlock( sector.Sub( 0x0C ), 86, 24, 2, 86, sector.Sub( 0x81C ) );
            ComputeEccBlock( sector.Sub( 0x0C ), 52, 43, 86, 88, sector.Sub( 0x8C8 ) );
            if( zeroAddress )
            {
                for( i = 0; i < 4; i++ )
                {
                    sector[12 + i] = address[i];
                }
            }
        }

        public static void GenerateEccEdc( IList<byte> sector, IsoType isoType )
        {
            switch( isoType )
            {
                case IsoType.Mode2Form1:
                    ComputeEdcBlock( sector.Sub( 0x10 ), 0x808, sector.Sub( 0x818 ) );
                    GenerateEcc( sector, true );
                    break;
                case IsoType.Mode2Form2:
                    ComputeEdcBlock( sector.Sub( 0x10 ), 0x91C, sector.Sub( 0x92C ) );
                    break;
                case IsoType.Mode1:
                default:
                    throw new ArgumentException( "isotype" );
            }
        }

		#endregion Private Methods 


        public struct NewOldValue
        {
            public byte OldValue;
            public byte NewValue;
            public NewOldValue( byte newValue, byte oldValue )
            {
                OldValue = oldValue;
                NewValue = newValue;
            }
        }        public enum IsoType
        {
            Mode1 = 0,
            Mode2Form1 = 1,
            Mode2Form2 = 2
        }
    }
}
