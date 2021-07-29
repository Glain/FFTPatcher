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
using System.IO;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace PatcherLib.Iso
{
    public static class PspIso
    {
        public class PspIsoInfo
        {
            private delegate void MyFunc( string path, Sectors sector, bool throwWhenNotFound );

            private IDictionary<Sectors, long> fileToSectorMap;
            private IDictionary<Sectors, long> fileToSizeMap;

            private PspIsoInfo() { }
            public long this[PspIso.Sectors file] { get { return fileToSectorMap[file]; } }

            [System.Diagnostics.DebuggerDisplay( "Start: {StartSector} - End: {EndSector} - {Size}" )]
            private struct FileSpaceInfo
            {
                public long StartSector { get; set; }
                public long EndSector { get; set; }
                public long Size { get; set; }
                public long NumSectors { get { return EndSector - StartSector + 1; } }
            }

            public bool ContainsKey( Sectors sector )
            {
                return fileToSectorMap.ContainsKey( sector );
            }

            public int GetSectorWithFreeSpace( int freeSpaceNeeded )
            {
                int sectorsNeeded = (freeSpaceNeeded - 1) / 2048 + 1;
                    
                List<FileSpaceInfo> allFiles = new List<FileSpaceInfo>();

                foreach (var key in fileToSectorMap.Keys)
                {
                    allFiles.Add( new FileSpaceInfo
                    {
                        StartSector = fileToSectorMap[key],
                        Size = fileToSizeMap[key],
                        EndSector = fileToSectorMap[key] + (fileToSizeMap[key] - 1) / 2048
                    } );
                }

                allFiles.Sort( ( a, b ) => a.StartSector.CompareTo( b.StartSector ) );

                for (int i = 0; i < allFiles.Count - 1; i++)
                {
                    if (allFiles[i + 1].StartSector - allFiles[i].EndSector - 1 >= sectorsNeeded)
                    {
                        return (int)allFiles[i].EndSector + 1;
                    }
                }

                return -1;
            }

            public void AddFile( PspIso.Sectors file, int sector, int size )
            {
                fileToSectorMap[file] = sector;
                fileToSizeMap[file] = size;
            }

            public void RemoveFile( PspIso.Sectors file )
            {
                if (fileToSectorMap.ContainsKey( file ))
                {
                    fileToSectorMap.Remove( file );
                }
                if (fileToSizeMap.ContainsKey( file ))
                {
                    fileToSizeMap.Remove( file );
                }
            }

            public long GetFileSize( PspIso.Sectors file )
            {
                return fileToSizeMap[file];
            }


            static IDictionary<int, PspIsoInfo> hashCodeToInfoMap = new Dictionary<int, PspIsoInfo>();

            public static PspIsoInfo GetPspIsoInfo( Stream iso )
            {
                int hashCode = iso.GetHashCode();
                if (!hashCodeToInfoMap.ContainsKey( hashCode ))
                {
                    hashCodeToInfoMap[hashCode] = GetPspIsoInfo( ImageMaster.IsoReader.GetRecord( iso ) );
                }
                return hashCodeToInfoMap[hashCode];
            }

            private static PspIsoInfo GetPspIsoInfo( ImageMaster.ImageRecord record )
            {
                ImageMaster.ImageRecord myRecord = null;
                var myDict = new Dictionary<Sectors, long>();
                var myOtherDict = new Dictionary<Sectors, long>();
                MyFunc func =
                    delegate( string path, Sectors sector, bool throwOnError )
                    {
                        myRecord = record.GetItemPath( path );
                        if (myRecord != null)
                        {
                            myDict[sector] = myRecord.Location;
                            myOtherDict[sector] = myRecord.Size;
                        }
                        else if (throwOnError)
                        {
                            throw new FileNotFoundException( "couldn't find file in ISO", path );
                        }
                    };
                myDict[Sectors.Root] = 22;
                myOtherDict[Sectors.Root] = 0x800;
                func( "PSP_GAME", Sectors.PSP_GAME, true );
                func( "PSP_GAME/ICON0.PNG", Sectors.PSP_GAME_ICON0_PNG, true );
                func( "PSP_GAME/PARAM.SFO", Sectors.PSP_GAME_PARAM_SFO, true );
                func( "PSP_GAME/PIC0.PNG", Sectors.PSP_GAME_PIC0_PNG, true );
                func( "PSP_GAME/PIC1.PNG", Sectors.PSP_GAME_PIC1_PNG, true );
                func( "PSP_GAME/SYSDIR", Sectors.PSP_GAME_SYSDIR, true );
                func( "PSP_GAME/SYSDIR/BOOT.BIN", Sectors.PSP_GAME_SYSDIR_BOOT_BIN, true );
                func( "PSP_GAME/SYSDIR/EBOOT.BIN", Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, true );
                func( "PSP_GAME/SYSDIR/UPDATE", Sectors.PSP_GAME_SYSDIR_UPDATE, true );
                func( "PSP_GAME/SYSDIR/UPDATE/DATA.BIN", Sectors.PSP_GAME_SYSDIR_UPDATE_DATA_BIN, true );
                func( "PSP_GAME/SYSDIR/UPDATE/EBOOT.BIN", Sectors.PSP_GAME_SYSDIR_UPDATE_EBOOT_BIN, true );
                func( "PSP_GAME/SYSDIR/UPDATE/PARAM.SFO", Sectors.PSP_GAME_SYSDIR_UPDATE_PARAM_SFO, true );
                func( "PSP_GAME/USRDIR", Sectors.PSP_GAME_USRDIR, true );
                func( "PSP_GAME/USRDIR/fftpack.bin", Sectors.PSP_GAME_USRDIR_fftpack_bin, true );
                func( "PSP_GAME/USRDIR/movie", Sectors.PSP_GAME_USRDIR_movie, true );
                func( "PSP_GAME/USRDIR/movie/001_HolyStone.pmf", Sectors.PSP_GAME_USRDIR_movie_001_HolyStone_pmf, true );
                func( "PSP_GAME/USRDIR/movie/002_Opening.pmf", Sectors.PSP_GAME_USRDIR_movie_002_Opening_pmf, true );
                func( "PSP_GAME/USRDIR/movie/003_Abduction.pmf", Sectors.PSP_GAME_USRDIR_movie_003_Abduction_pmf, true );
                func( "PSP_GAME/USRDIR/movie/004_Kusabue.pmf", Sectors.PSP_GAME_USRDIR_movie_004_Kusabue_pmf, true );
                func( "PSP_GAME/USRDIR/movie/005_Get_away.pmf", Sectors.PSP_GAME_USRDIR_movie_005_Get_away_pmf, true );
                func( "PSP_GAME/USRDIR/movie/006_Reassume_Dilita.pmf", Sectors.PSP_GAME_USRDIR_movie_006_Reassume_Dilita_pmf, true );
                func( "PSP_GAME/USRDIR/movie/007_Dilita_Advice.pmf", Sectors.PSP_GAME_USRDIR_movie_007_Dilita_Advice_pmf, true );
                func( "PSP_GAME/USRDIR/movie/008_Ovelia_and_Dilita.pmf", Sectors.PSP_GAME_USRDIR_movie_008_Ovelia_and_Dilita_pmf, true );
                func( "PSP_GAME/USRDIR/movie/009_Dilita_Musing.pmf", Sectors.PSP_GAME_USRDIR_movie_009_Dilita_Musing_pmf, true );
                func( "PSP_GAME/USRDIR/movie/010_Ending.pmf", Sectors.PSP_GAME_USRDIR_movie_010_Ending_pmf, true );
                func( "PSP_GAME/USRDIR/movie/011_Russo.pmf", Sectors.PSP_GAME_USRDIR_movie_011_Russo_pmf, true );
                func( "PSP_GAME/USRDIR/movie/012_Valuhurea.pmf", Sectors.PSP_GAME_USRDIR_movie_012_Valuhurea_pmf, true );
                func( "PSP_GAME/USRDIR/movie/013_StaffRoll.pmf", Sectors.PSP_GAME_USRDIR_movie_013_StaffRoll_pmf, true );
                func( "UMD_DATA.BIN", Sectors.UMD_DATA_BIN, true );
                func( "PSP_GAME/USRDIR/CHARMAP", Sectors.PSP_GAME_USRDIR_CHARMAP, false );
                PspIsoInfo result = new PspIsoInfo();
                result.fileToSectorMap = myDict;
                result.fileToSizeMap = myOtherDict;
                return result;
            }


            public static PspIsoInfo GetPspIsoInfo( string iso )
            {
                return GetPspIsoInfo( ImageMaster.IsoReader.GetRecord( iso ) );
            }
        }


        #region Instance Variables (6)

        private static readonly long[] bootBinLocations = { 0x10000, 0x0FED8000 };
        private static byte[] buffer = new byte[1024];
        private const int bufferSize = 1024;
        private static byte[] euSizes = new byte[] { 0xA4, 0x84, 0x3A, 0x00, 0x00, 0x3A, 0x84, 0xA4 };
        //public const long FFTPackLocation = 0x02C20000;
        private static byte[] jpSizes = new byte[] { 0xE4, 0xD9, 0x37, 0x00, 0x00, 0x37, 0xD9, 0xE4 };

        #endregion Instance Variables

        #region Public Methods (10)

        /// <summary>
        /// Decrypts the ISO.
        /// </summary>
        /// <param name="filename">The filename of the ISO to decrypt.</param>
        public static void DecryptISO( string filename )
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream( filename, FileMode.Open );
                PspIsoInfo info = PspIsoInfo.GetPspIsoInfo( stream );
                DecryptISO( stream, info );
            }
            catch (NotSupportedException)
            {
                throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Flush();
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Decrypts the ISO.
        /// </summary>
        /// <param name="stream">The stream of the ISO to decrypt.</param>
        public static void DecryptISO( Stream stream, PspIsoInfo info )
        {
            if (IsJP( stream, info ))
            {
                CopyBytes( stream, info[Sectors.PSP_GAME_SYSDIR_BOOT_BIN] * 2048, 0x37D9E4, info[Sectors.PSP_GAME_SYSDIR_EBOOT_BIN] * 2048, 0x37DB40 );
            }
            else if (IsUS( stream, info ) || IsEU( stream, info ))
            {
                CopyBytes( stream, info[Sectors.PSP_GAME_SYSDIR_BOOT_BIN] * 2048, 0x3A84A4, info[Sectors.PSP_GAME_SYSDIR_EBOOT_BIN] * 2048, 0x3A8600 );
            }
            else
            {
                throw new NotSupportedException( "Unrecognized image." );
            }
        }

        /// <summary>
        /// Determines whether the specified stream is EU.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// 	<c>true</c> if the specified stream is EU; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEU( Stream stream, PspIsoInfo info )
        {
            return
                CheckString( stream, info[Sectors.UMD_DATA_BIN] * 2048 + 0, "ULES-00850" ) &&
                CheckString( stream, info[Sectors.PSP_GAME_PARAM_SFO] * 2048 + 0x128, "ULES00850" ) &&
                CheckString( stream, info[Sectors.PSP_GAME_SYSDIR_BOOT_BIN] * 2048 + 0x3143A8, "ULES00850" ) &&
                CheckString( stream, info[Sectors.PSP_GAME_SYSDIR_BOOT_BIN] * 2048 + 0x35A530, "ULES00850" );
        }

        /// <summary>
        /// Determines whether the specified stream is JP.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// 	<c>true</c> if the specified stream is JP; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsJP( Stream stream, PspIsoInfo info )
        {
            //return CheckFile( stream, "ULJM-05194", "ULJM05194", new long[] { 0x8373, 0xE000 }, new long[] { 0x2BF0128, 0xFD619FC, 0xFD97A5C } );
            return
                CheckString( stream, info[Sectors.UMD_DATA_BIN] * 2048 + 0, "ULJM-05194" );
        }

        /// <summary>
        /// Determines whether the specified stream is US.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// 	<c>true</c> if the specified stream is US; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUS( Stream stream, PspIsoInfo info )
        {
            return
                CheckString( stream, info[Sectors.UMD_DATA_BIN] * 2048 + 0, "ULUS-10297" ) &&
                CheckString( stream, info[Sectors.PSP_GAME_PARAM_SFO] * 2048 + 0x128, "ULUS10297" ) &&
                CheckString( stream, info[Sectors.PSP_GAME_SYSDIR_BOOT_BIN] * 2048 + 0x3143A8, "ULUS10297" ) &&
                CheckString( stream, info[Sectors.PSP_GAME_SYSDIR_BOOT_BIN] * 2048 + 0x35A530, "ULUS10297" );
        }

        private static bool CheckString( Stream stream, long loc, string expectedString )
        {
            stream.Seek( loc, SeekOrigin.Begin );
            byte[] buffer = new byte[expectedString.Length];
            stream.Read( buffer, 0, buffer.Length );
            return buffer.ToUTF8String() == expectedString;
        }

        public static void PatchISO( Stream file, IEnumerable<PatcherLib.Datatypes.PatchedByteArray> patches )
        {
            PspIsoInfo info = PspIsoInfo.GetPspIsoInfo( file );
            DecryptISO( file, info );
            patches.ForEach( p => ApplyPatch( file, info, p ) );
        }

        #endregion Public Methods

        #region Private Methods (3)

        public static void ApplyPatch( Stream stream, PspIsoInfo info, PatcherLib.Datatypes.PatchedByteArray patch )
        {
            if (patch.SectorEnum != null)
            {
                if (patch.SectorEnum.GetType() == typeof( PspIso.Sectors ))
                {
                    stream.WriteArrayToPosition( patch.GetBytes(), (int)(info[(PspIso.Sectors)patch.SectorEnum] * 2048) + patch.Offset );
                }
                else if (patch.SectorEnum.GetType() == typeof( FFTPack.Files ))
                {
                    FFTPack.PatchFile( stream, info, (int)((FFTPack.Files)patch.SectorEnum), (int)patch.Offset, patch.GetBytes() );
                }
                else
                {
                    throw new ArgumentException( "invalid type" );
                }
            }
        }

        public static byte[] GetFile( Stream stream, PspIsoInfo info, PspIso.Sectors sector, int start, int length )
        {
            byte[] result = new byte[length];
            stream.Seek( info[sector] * 2048 + start, SeekOrigin.Begin );
            stream.Read( result, 0, length );
            return result;
        }

        public static byte[] GetFile( Stream stream, PspIsoInfo info, FFTPack.Files file, int start, int length )
        {
            return FFTPack.GetFileFromIso( stream, info, file, start, length );
        }

        public static byte[] GetFile( Stream stream, PspIsoInfo info, FFTPack.Files file )
        {
            return FFTPack.GetFileFromIso( stream, info, file );
        }

        public static byte[] GetBlock( Stream iso, PspIsoInfo info, KnownPosition pos )
        {
            if (pos.FFTPack.HasValue)
            {
                return GetFile( iso, info, pos.FFTPack.Value, pos.StartLocation, pos.Length );
            }
            else if (pos.Sector.HasValue)
            {
                return GetFile( iso, info, pos.Sector.Value, pos.StartLocation, pos.Length );
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static void CopyBytes( Stream stream, long src, long srcSize, long dest, long destOldSize )
        {
            long bytesRead = 0;
            while ((bytesRead + bufferSize) < srcSize)
            {
                stream.Seek( src + bytesRead, SeekOrigin.Begin );
                stream.Read( buffer, 0, bufferSize );
                stream.Seek( dest + bytesRead, SeekOrigin.Begin );
                stream.Write( buffer, 0, bufferSize );
                bytesRead += bufferSize;
            }

            stream.Seek( src + bytesRead, SeekOrigin.Begin );
            stream.Read( buffer, 0, (int)(srcSize - bytesRead) );
            stream.Seek( dest + bytesRead, SeekOrigin.Begin );
            stream.Write( buffer, 0, (int)(srcSize - bytesRead) );

            if (destOldSize > srcSize)
            {
                buffer = new byte[bufferSize];
                stream.Seek( dest + srcSize, SeekOrigin.Begin );
                stream.Write( buffer, 0, (int)(destOldSize - srcSize) );
            }
        }

        public class KnownPosition : PatcherLib.Iso.KnownPosition
        {
            public Enum SectorEnum { get; private set; }
            public Sectors? Sector { get; private set; }
            public FFTPack.Files? FFTPack { get; private set; }

            public int StartLocation { get; private set; }
            private int length;
            public override int Length
            {
                get { return length; }
            }

            private KnownPosition( Enum sector, int startLocation, int length )
            {
                SectorEnum = sector;
                StartLocation = startLocation;
                this.length = length;
            }

            public KnownPosition( Sectors sector, int startLocation, int length )
                : this( (Enum)sector, startLocation, length )
            {
                Sector = sector;
            }

            public KnownPosition( FFTPack.Files sector, int startLocation, int length )
                : this( (Enum)sector, startLocation, length )
            {
                FFTPack = sector;
            }

            public override PatchedByteArray GetPatchedByteArray( byte[] bytes )
            {
                if (Sector.HasValue)
                {
                    return new PatchedByteArray( Sector, (uint)StartLocation, bytes );
                }
                else if (FFTPack.HasValue)
                {
                    return new PatchedByteArray( FFTPack, (uint)StartLocation, bytes );
                }
                else
                {
                    throw new Exception();
                }
            }

            public override byte[] ReadIso( Stream iso )
            {
                return ReadIso( iso, PspIsoInfo.GetPspIsoInfo( iso ) );
            }

            public byte[] ReadIso( Stream iso, PspIsoInfo info )
            {
                return PspIso.GetBlock( iso, info, this );
            }

            public override void PatchIso( Stream iso, IList<byte> bytes )
            {
                PspIso.ApplyPatch( iso, PspIsoInfo.GetPspIsoInfo( iso ), GetPatchedByteArray( bytes.ToArray() ) );
            }

            public override PatcherLib.Iso.KnownPosition AddOffset(int offset, int length)
            {
                if (Sector.HasValue)
                    return new PspIso.KnownPosition(Sector.Value, StartLocation + offset, this.length + length);
                else if (FFTPack.HasValue)
                    return new PspIso.KnownPosition(FFTPack.Value, StartLocation + offset, this.length + length);
                else
                    throw new Exception("Either Sector or FFTPack must have a value.");
            }
        }

        public static IList<KnownPosition> Abilities = new KnownPosition[] { 
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x271514, 0x24C6), 
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x271514, 0x24C6) }.AsReadOnly();

        public static IList<KnownPosition> AbilityEffects = new KnownPosition[] { 
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x3177B4, 0x38C), 
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x3177B4, 0x38C)}.AsReadOnly();

        //public static IList<KnownPosition> ItemAbilityEffects = new KnownPosition[] { 
        //    new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x317A94, 0x1C), 
        //    new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x317A94, 0x1C)}.AsReadOnly();

        //public static IList<KnownPosition> ReactionAbilityEffects = new KnownPosition[] { 
        //    new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x317B00, 0x40), 
        //    new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x317B00, 0x40)}.AsReadOnly();

        public static IList<KnownPosition> ActionEvents = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x276CA4, 227),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x276CA4, 227)}.AsReadOnly();

        public static KnownPosition ENTD1 = new KnownPosition(FFTPack.Files.BATTLE_ENTD1_ENT, 0, 81920);

        public static KnownPosition ENTD2 = new KnownPosition(FFTPack.Files.BATTLE_ENTD2_ENT, 0, 81920);

        public static KnownPosition ENTD3 = new KnownPosition(FFTPack.Files.BATTLE_ENTD3_ENT, 0, 81920);

        public static KnownPosition ENTD4 = new KnownPosition(FFTPack.Files.BATTLE_ENTD4_ENT, 0, 81920);

        public static KnownPosition ENTD5 = new KnownPosition(FFTPack.Files.BATTLE_ENTD5_ENT, 0, 51200);

        public static IList<KnownPosition> InflictStatuses = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x3263E8, 0x300),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x3263E8, 0x300)}.AsReadOnly();

        public static IList<KnownPosition> JobLevels = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x277084, 280),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x277084, 280)}.AsReadOnly();

        public static IList<KnownPosition> Jobs = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x2739DC, 8281),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x2739DC, 8281)}.AsReadOnly();

        public static IList<KnownPosition> MonsterSkills = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x276BB4, 0xF0),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x276BB4, 0xF0)}.AsReadOnly();

        public static IList<KnownPosition> MoveFindItems = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x2707A8, 0x800),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x2707A8, 0x800)}.AsReadOnly();

        public static IList<KnownPosition> OldItemAttributes = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x3266E8, 0x7D0),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x3266E8, 0x7D0)}.AsReadOnly();

        public static IList<KnownPosition> OldItems = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x3252DC, 0x110A),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x3252DC, 0x110A)}.AsReadOnly();

        public static IList<KnownPosition> NewItemAttributes = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x25720C, 0x20D),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x25720C, 0x20D)}.AsReadOnly();

        public static IList<KnownPosition> NewItems = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x256E00, 1032),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x256E00, 1032)}.AsReadOnly();

        public static IList<KnownPosition> PoachProbabilities = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x277024, 0x60),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x277024, 0x60)}.AsReadOnly();

        public static IList<KnownPosition> SkillSets = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x275A38, 4475),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x275A38, 4475)}.AsReadOnly();

        public static IList<KnownPosition> StatusAttributes = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x276DA4, 0x280),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x276DA4, 0x280)}.AsReadOnly();

        public static IList<KnownPosition> StoreInventories = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x2DC8D0, 0x200),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x2DC8D0, 0x200)}.AsReadOnly();

        // Ability animations go down until the Support abilities, so down up to and including 0x1C5 = 0x1C6 * 3 = 0x552 bytes
        public static IList<KnownPosition> AbilityAnimations = new KnownPosition[] { new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x32394C, 0x552), 
            new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x32394C, 0x552) }.AsReadOnly();

        public static IList<KnownPosition> Propositions = new KnownPosition[] { new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x2E5688,0xA7C), 
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x2E5688, 0xA7C)}.AsReadOnly();

        public static IList<KnownPosition> JobFormationSpritesJobCheckID = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x18D7D8, 1),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x18D7D8, 1)}.AsReadOnly();

        public static IList<KnownPosition> JobFormationSprites1 = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x2DCF08, 0x4A),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x2DCF08, 0x4A)}.AsReadOnly();

        public static IList<KnownPosition> JobFormationSprites2 = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x29D1F0, 0x94),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x29D1F0, 0x94)}.AsReadOnly();

        public static IList<KnownPosition> JobFormationSprites2A = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x2D7594, 0x94),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x2D7594, 0x94)}.AsReadOnly();

        public static IList<KnownPosition> JobFormationSprites2B = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x2F57CC, 0x94),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x2F57CC, 0x94)}.AsReadOnly();

        public static IList<KnownPosition> JobFormationSprites2C = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x31134C, 0x94),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x31134C, 0x94)}.AsReadOnly();

        public static IList<KnownPosition> JobFormationSprites2D = new KnownPosition[] {
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x312DB0, 0x94),
                new KnownPosition(Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x312DB0, 0x94)}.AsReadOnly();

        #endregion Private Methods

        public enum Sectors
        {
            Root = 22,
            PSP_GAME = 23,
            PSP_GAME_SYSDIR = 24,
            PSP_GAME_SYSDIR_UPDATE = 26,
            PSP_GAME_USRDIR_movie = 27,
            PSP_GAME_ICON0_PNG = 22560,
            PSP_GAME_PARAM_SFO = 22576,
            PSP_GAME_PIC0_PNG = 22416,
            PSP_GAME_PIC1_PNG = 22432,
            PSP_GAME_SYSDIR_BOOT_BIN = 130480,
            PSP_GAME_SYSDIR_EBOOT_BIN = 32,
            PSP_GAME_SYSDIR_UPDATE_DATA_BIN = 6032,
            PSP_GAME_SYSDIR_UPDATE_EBOOT_BIN = 1936,
            PSP_GAME_SYSDIR_UPDATE_PARAM_SFO = 1920,
            PSP_GAME_USRDIR = 25,
            PSP_GAME_USRDIR_fftpack_bin = 22592,
            PSP_GAME_USRDIR_CHARMAP = 14006, 
            PSP_GAME_USRDIR_movie_001_HolyStone_pmf = 132368,
            PSP_GAME_USRDIR_movie_002_Opening_pmf = 190832,
            PSP_GAME_USRDIR_movie_003_Abduction_pmf = 198112,
            PSP_GAME_USRDIR_movie_004_Kusabue_pmf = 135360,
            PSP_GAME_USRDIR_movie_005_Get_away_pmf = 140288,
            PSP_GAME_USRDIR_movie_006_Reassume_Dilita_pmf = 144352,
            PSP_GAME_USRDIR_movie_007_Dilita_Advice_pmf = 150224,
            PSP_GAME_USRDIR_movie_008_Ovelia_and_Dilita_pmf = 156000,
            PSP_GAME_USRDIR_movie_009_Dilita_Musing_pmf = 166192,
            PSP_GAME_USRDIR_movie_010_Ending_pmf = 179264,
            PSP_GAME_USRDIR_movie_011_Russo_pmf = 183360,
            PSP_GAME_USRDIR_movie_012_Valuhurea_pmf = 186304,
            PSP_GAME_USRDIR_movie_013_StaffRoll_pmf = 202128,
            UMD_DATA_BIN = 28,
        }
    }
}