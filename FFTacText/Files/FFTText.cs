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
using System.ComponentModel;
using System.IO;
using FFTPatcher.TextEditor.Files;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;
using PatcherLib.Iso;

namespace FFTPatcher.TextEditor
{
    /// <summary>
    /// Represents a collection of FFT text files.
    /// </summary>
    public class FFTText
    {

        #region Fields (1)

        /// <summary>
        /// Gets the current version of FFTText files.
        /// </summary>
        public const int CurrentVersion = 3;

        #endregion Fields

        #region Properties (5)


        /// <summary>
        /// Gets the character map.
        /// </summary>
        public GenericCharMap CharMap { get; private set; }

        /// <summary>
        /// Gets the filetype.
        /// </summary>
        public Context Filetype { get; private set; }

        #endregion Properties

        #region Constructors (1)

        private FFTText()
        {
        }

        #endregion Constructors

        #region Methods (9)

        private const string charmapBaseFileName = "CHARMAP";
        public const string PspCharmapFileName = charmapBaseFileName;
        public const string PsxCharmapFileName = charmapBaseFileName + ";1";
        public const string CharmapHeader = "FFTacText Custom Charmap";
        public const int PsxRootDirEntSector = 22;
        public const int PsxCharmapSector = (int)PspIso.Sectors.PSP_GAME_USRDIR_CHARMAP;

        private static IList<PatcherLib.Iso.DirectoryEntry> GetDefaultPspRootDirectoryEntry()
        {
            return new List<DirectoryEntry>
            {
                new PatcherLib.Iso.DirectoryEntry( 0x19, 0x0800,
                    new DateTime( 0x6B+1900, 0x07, 0x02, 0x0E, 0x00, 0x01 ), 0x24,
                    new byte[] { 0x02, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 }, 
                    "\0",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x17, 0x0800,
                    new DateTime( 0x6B+1900, 0x07, 0x02, 0x0E, 0x00, 0x01 ), 0x24,
                    new byte[] { 0x02, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 }, 
                    "\x01",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x5840, 0x0D2B2000,
                    new DateTime( 0x6B+1900, 0x07, 0x02, 0x0D, 0x36, 0x10 ), 0x24,
                    new byte[] { 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "fftpack.bin",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x0D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }),
                new PatcherLib.Iso.DirectoryEntry( 0x1B, 0x0800,
                    new DateTime( 0x6B+1900, 0x07, 0x02, 0x0E, 0x00, 0x01 ), 0x24,
                    new byte[] { 0x02, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 }, 
                    "movie",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
            };
        }

        private static IList<PatcherLib.Iso.DirectoryEntry> GetDefaultPsxRootDirectoryEntry()
        {
            return new List<PatcherLib.Iso.DirectoryEntry> {
                new PatcherLib.Iso.DirectoryEntry( 0x16, 0x0800,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x02, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "\0",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x16, 0x0800,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x02, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "\x01",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0xDC74, 0x3000,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "BATTLE",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x03E8, 0x155168,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x04 ), 0x24,
                    new byte[] { 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "BATTLE.BIN;1",
                    new byte[] { 0x2A, 0x00, 0x2A, 0x00, 0x08, 0x01, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x038270, 0x0800,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "DUMMY",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0xEC81, 0x7800,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "EFFECT",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x693, 0x1000,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "EVENT",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x2553, 0x017000,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "MAP",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x0115EF, 0x0800,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "MENU",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x14E0E, 0x0800,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "OPEN",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0xC6, 0x5000,
                    new DateTime( 0x61 + 1900, 0x09, 0x04, 0x0E, 0x36, 0x14 ), 0x24,
                    new byte[] { 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "SCEAP.DAT;1",
                    new byte[] { 0x2A, 0x00, 0x2A, 0x00, 0x08, 0x01, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x18, 0x057000,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x04 ), 0x24,
                    new byte[] { 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "SCUS_942.21;1",
                    new byte[] { 0x2A, 0x00, 0x2A, 0x00, 0x08, 0x01, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x014B01, 0x2000,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "SOUND",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x17, 0x44,
                    new DateTime( 0x61 + 1900, 0x09, 0x0C, 0x10, 0x17, 0x00 ), 0x24,
                    new byte[] { 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "SYSTEM.CNF;1",
                    new byte[] { 0x2A, 0x00, 0x2A, 0x00, 0x08, 0x01, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
                new PatcherLib.Iso.DirectoryEntry( 0x011A83, 0x0800,
                    new DateTime( 0x61 + 1900, 0x0A, 0x11, 0x12, 0x25, 0x15 ), 0x24,
                    new byte[] { 0x03, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01 },
                    "WORLD",
                    new byte[] { 0x00, 0x00, 0x00, 0x00, 0x8D, 0x55, 0x58, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } ),
            };

        }

        private IList<PatchedByteArray> RemoveCharMapPatches( Stream iso, Context context )
        {
            switch (context)
            {
                case Context.US_PSP:
                    return RemovePspCharMapPatches(iso);
                case Context.US_PSX:
                    return RemovePsxCharMapPatches(iso);
                default:
                    return new PatchedByteArray[0];
            }
        }

        private IList<PatchedByteArray> RemovePspCharMapPatches(Stream iso)
        {
            PspIso.PspIsoInfo info = PspIso.PspIsoInfo.GetPspIsoInfo( iso );
            //DirectoryEntry.GetPspDirectoryEntries( iso,
            var usrDir = DirectoryEntry.GetPspDirectoryEntries( 
                iso, 
                info,
                PspIso.Sectors.PSP_GAME_USRDIR,
                1 );
            List<PatchedByteArray> result = new List<PatchedByteArray>();

            var charmapDirEntry = usrDir.Find( de => de.Filename == FFTText.PspCharmapFileName );
            if (charmapDirEntry != null)
            {
                usrDir.Remove( charmapDirEntry );
                result.Add( new PatchedByteArray( (int)PspIso.Sectors.PSP_GAME_USRDIR_CHARMAP, 0,
                    new byte[System.Text.Encoding.UTF8.GetBytes( CharmapHeader ).Length] ) );
                DirectoryEntry.GetPspDirectoryEntryPatches(
                    (int)PspIso.Sectors.PSP_GAME_USRDIR, 1, usrDir );
            }

            return result;
        }

        private IList<PatchedByteArray> RemovePsxCharMapPatches(Stream iso)
        {
            List<PatchedByteArray> result = new List<PatchedByteArray>();
            var rootDirEntries = DirectoryEntry.GetPsxDirectoryEntries( iso, PsxRootDirEntSector, 1 );
            var charmapENtry = rootDirEntries.Find( de => de.Filename == PsxCharmapFileName );
            if (charmapENtry != null)
            {
                rootDirEntries.Remove( charmapENtry );
                result.Add(new PatchedByteArray( PsxCharmapSector, 0,
                    new byte[System.Text.Encoding.UTF8.GetBytes( CharmapHeader ).Length] ) );
                result.AddRange( DirectoryEntry.GetPsxDirectoryEntryPatches( PsxRootDirEntSector, 1, rootDirEntries ) );
            }

            return result;
        }

        private IList<PatchedByteArray> GetPsxCharMapPatches( Stream iso, GenericCharMap baseCharmap, IDictionary<byte, string> dteInfo )
        {
            var dirEnt = DirectoryEntry.GetPsxDirectoryEntries( iso, PsxRootDirEntSector, 1 );

            var myDict = new Dictionary<int, string>( baseCharmap );
            dteInfo.ForEach( kvp => myDict[kvp.Key] = kvp.Value );

            System.Text.StringBuilder myString = new System.Text.StringBuilder();
            myString.AppendLine( CharmapHeader );
            myDict.ForEach( kvp => myString.AppendFormat( "{0:X4}\t{1}" + Environment.NewLine, kvp.Key, kvp.Value ) );
            var bytes = System.Text.Encoding.UTF8.GetBytes( myString.ToString() );

            var baseDirEntry = dirEnt.Find( d => d.Filename == "SCEAP.DAT;1" );
            var charmapDirEntry = new PatcherLib.Iso.DirectoryEntry(
                PsxCharmapSector, (uint)bytes.Length, DateTime.Now, baseDirEntry.GMTOffset,
                baseDirEntry.MiddleBytes, PsxCharmapFileName, baseDirEntry.ExtendedBytes );
            AddOrReplaceCharMapDirectoryEntry( dirEnt, charmapDirEntry );

            var dirEntPatches = PatcherLib.Iso.DirectoryEntry.GetPsxDirectoryEntryPatches(
                PsxRootDirEntSector, 1, dirEnt );
            dirEntPatches.Add( new PatchedByteArray( PsxCharmapSector, 0, bytes ) );
            return dirEntPatches;
        }

        private int FindSectorToInsertPspCharmap( PspIso.PspIsoInfo info, int spaceNeeded )
        {
            return info.GetSectorWithFreeSpace( spaceNeeded );
        }

        private IList<PatchedByteArray> GetPspCharMapPatches( Stream iso, GenericCharMap baseCharmap, IDictionary<byte, string> dteInfo )
        {
            var pspIsoInfo = PspIso.PspIsoInfo.GetPspIsoInfo( iso );
            var dirEnt = DirectoryEntry.GetPspDirectoryEntries( iso, pspIsoInfo, PspIso.Sectors.PSP_GAME_USRDIR, 1 );

            var currentEntry = dirEnt.Find( de => de.Filename == PspCharmapFileName );
            if (currentEntry != null)
            {
                dirEnt.Remove( currentEntry );
            }
            pspIsoInfo.RemoveFile( PspIso.Sectors.PSP_GAME_USRDIR_CHARMAP );

            var myDict = new Dictionary<int, string>( baseCharmap );
            dteInfo.ForEach( kvp => myDict[kvp.Key] = kvp.Value );

            System.Text.StringBuilder myString = new System.Text.StringBuilder();
            myString.AppendLine( CharmapHeader );
            myDict.ForEach( kvp => myString.AppendFormat( "{0:X4}\t{1}" + Environment.NewLine, kvp.Key, kvp.Value ) );
            var bytes = System.Text.Encoding.UTF8.GetBytes( myString.ToString() );

            int insertSector = FindSectorToInsertPspCharmap( pspIsoInfo, bytes.Length );
            if (insertSector == -1)
            {
                throw new InvalidOperationException();
            }

            pspIsoInfo.AddFile( PspIso.Sectors.PSP_GAME_USRDIR_CHARMAP, insertSector, bytes.Length );

            var baseDirEntry = dirEnt.Find( d => d.Filename == "fftpack.bin" );
            var charmapDirEntry = new PatcherLib.Iso.DirectoryEntry(
                (uint)insertSector, (uint)bytes.Length, DateTime.Now, baseDirEntry.GMTOffset,
                baseDirEntry.MiddleBytes, PspCharmapFileName, baseDirEntry.ExtendedBytes );

            AddOrReplaceCharMapDirectoryEntry( dirEnt, charmapDirEntry );

            var dirEntPatches = PatcherLib.Iso.DirectoryEntry.GetPspDirectoryEntryPatches(
                (int)PspIso.Sectors.PSP_GAME_USRDIR, 1, dirEnt );
            dirEntPatches.Add( new PatchedByteArray( PspIso.Sectors.PSP_GAME_USRDIR_CHARMAP, 0, bytes ) );

            return dirEntPatches;

        }

        private IList<PatchedByteArray> GetCharMapPatches( Stream iso, Context context, GenericCharMap baseCharmap, IDictionary<byte, string> dteInfo )
        {
            if (context == Context.US_PSX)
            {
                return GetPsxCharMapPatches( iso, baseCharmap, dteInfo );
            }
            else if (context == Context.US_PSP)
            {
                return GetPspCharMapPatches( iso, baseCharmap, dteInfo );
            }
            else
                return new PatchedByteArray[0];
        }

        private void AddOrReplaceCharMapDirectoryEntry( IList<PatcherLib.Iso.DirectoryEntry> dir, PatcherLib.Iso.DirectoryEntry newDirEnt )
        {
            var currentCharmapIndex = dir.IndexOf( dir.Find( d => d.Filename == newDirEnt.Filename ) );
            if (currentCharmapIndex != -1)
            {
                dir[currentCharmapIndex] = newDirEnt;
            }
            else
            {
                dir.Add( newDirEnt );
                dir.Sort( ( a, b ) => a.Filename.CompareTo( b.Filename ) );
            }
        }

        public delegate void PatchIso( Stream iso, IEnumerable<PatchedByteArray> patches );
        public class PatchIsoArgs
        {
            public string Filename { get; set; }
            public PatchIso Patcher { get; set; }
        }

        private struct DteResult
        {
            public enum Result
            {
                Success,
                Fail,
                Cancelled
            }

            public Result ResultCode;
            public ISerializableFile FailedFile;
            public static DteResult Empty { get { return new DteResult { ResultCode = Result.Fail }; } }
        }

        private DteResult DoDteForFiles( IList<ISerializableFile> dteFiles, BackgroundWorker worker, DoWorkEventArgs args,
            out IDictionary<ISerializableFile, Set<KeyValuePair<string, byte>>> preferredPairs,
            out Set<KeyValuePair<string, byte>> dtePairs )
        {
            var filePreferredPairs =
                new Dictionary<ISerializableFile, Set<KeyValuePair<string, byte>>>( dteFiles.Count );
            Set<KeyValuePair<string, byte>> currentPairs =
                new Set<KeyValuePair<string, byte>>( ( x, y ) => x.Key.Equals( y.Key ) && (x.Value == y.Value) ? 0 : -1 );
            Stack<byte> dteBytes = DTE.GetAllowedDteBytes();
            var pairs = DTE.GetDteGroups( this.Filetype );
            foreach (var dte in dteFiles)
            {
                worker.ReportProgress( 0,
                    new ProgressForm.FileProgress { File = dte, State = ProgressForm.TaskState.Starting, Task = ProgressForm.Task.CalculateDte } );
                filePreferredPairs[dte] = dte.GetPreferredDTEPairs( pairs, currentPairs, dteBytes, worker );
                if (filePreferredPairs[dte] == null)
                {
                    dtePairs = null;
                    preferredPairs = null;
                    return new DteResult { ResultCode = DteResult.Result.Fail, FailedFile = dte };
                }
                currentPairs.AddRange( filePreferredPairs[dte] );
                worker.ReportProgress( 0,
                    new ProgressForm.FileProgress { File = dte, State = ProgressForm.TaskState.Done, Task = ProgressForm.Task.CalculateDte } );
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    dtePairs = null;
                    preferredPairs = null;
                    return new DteResult { ResultCode = DteResult.Result.Cancelled };
                }
            }

            preferredPairs = new ReadOnlyDictionary<ISerializableFile, Set<KeyValuePair<string, byte>>>( filePreferredPairs );
            dtePairs = currentPairs;
            return new DteResult { ResultCode = DteResult.Result.Success };
        }

        private IList<PatchedByteArray> DoDteCrap( IList<ISerializableFile> dteFiles, BackgroundWorker worker, DoWorkEventArgs args )
        {
            IDictionary<byte, string> dummy;
            return DoDteCrap( dteFiles, worker, args, out dummy );
        }

        private IList<PatchedByteArray> DoDteCrap( IList<ISerializableFile> dteFiles, BackgroundWorker worker, DoWorkEventArgs args, out IDictionary<byte, string> dteMapping )
        {
            List<PatchedByteArray> patches = new List<PatchedByteArray>();
            dteMapping = null;
            if (worker.CancellationPending)
            {
                args.Cancel = true;
                return null;
            }

            dteFiles.Sort( ( x, y ) => (y.ToCDByteArray().Length - y.Layout.Size).CompareTo( x.ToCDByteArray().Length - x.Layout.Size ) );
            if (worker.CancellationPending)
            {
                args.Cancel = true;
                return null;
            }

            IDictionary<ISerializableFile, Set<KeyValuePair<string, byte>>> filePreferredPairs = null;
            Set<KeyValuePair<string, byte>> currentPairs = null;

            DteResult result = DteResult.Empty;
            if (dteFiles.Count > 0)
            {
                int tries = dteFiles.Count;
                //DteResult result = DoDteForFiles( dteFiles, worker, args, out filePreferredPairs, out currentPairs );
                do
                {
                    result = DoDteForFiles( dteFiles, worker, args, out filePreferredPairs, out currentPairs );
                    switch (result.ResultCode)
                    {
                        case DteResult.Result.Cancelled:
                            args.Cancel = true;
                            return null;
                        case DteResult.Result.Fail:
                            var failedFile = result.FailedFile;
                            if (dteFiles[0] == failedFile)
                            {
                                // Failed on the first file... this is hopeless
                                throw new FFTPatcher.TextEditor.DTE.DteException( failedFile );
                            }

                            // Bump the failed file to the top of the list
                            dteFiles.Remove( failedFile );
                            dteFiles.Insert( 0, failedFile );
                            break;
                        case DteResult.Result.Success:
                            // do nothing
                            break;
                    }
                } while (result.ResultCode != DteResult.Result.Success && --tries >= 0);
            }

            switch (result.ResultCode)
            {
                case DteResult.Result.Fail:
                    throw new FFTPatcher.TextEditor.DTE.DteException( dteFiles[0] );
                case DteResult.Result.Cancelled:
                    args.Cancel = true;
                    return null;
            }

            foreach (var file in dteFiles)
            {
                worker.ReportProgress( 0,
                    new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Starting, Task = ProgressForm.Task.GeneratePatch } );
                var currentFileEncoding = PatcherLib.Utilities.Utilities.DictionaryFromKVPs( filePreferredPairs[file] );
                patches.AddRange( file.GetDtePatches( currentFileEncoding ) );
                worker.ReportProgress( 0,
                    new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Done, Task = ProgressForm.Task.GeneratePatch } );
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    return null;
                }
            }

            var myDteMapping = new Dictionary<byte, string>();
            currentPairs.ForEach( kvp => myDteMapping[kvp.Value] = kvp.Key );
            dteMapping = myDteMapping;

            patches.AddRange( DTE.GenerateDtePatches( this.Filetype, currentPairs ) );
            return patches.AsReadOnly();
        }

        public void BuildAndApplyPatches( object sender, DoWorkEventArgs args )
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            PatchIsoArgs patchArgs = args.Argument as PatchIsoArgs;
            if (patchArgs == null)
            {
                throw new Exception( "Incorrect args passed to BuildAndApplyPatches" );
            }
            if (patchArgs.Patcher == null)
            {
                throw new ArgumentNullException( "Patcher", "Patcher cannot be null" );
            }

            using (Stream stream = File.Open( patchArgs.Filename, FileMode.Open, FileAccess.ReadWrite ))
            {
                if (stream == null)
                {
                    throw new Exception( "Could not open ISO file" );
                }

                IList<ISerializableFile> files = new List<ISerializableFile>( Files.Count );
                Files.FindAll( f => f is ISerializableFile ).ForEach( f => files.Add( (ISerializableFile)f ) );

                List<ISerializableFile> dteFiles = new List<ISerializableFile>();
                List<ISerializableFile> nonDteFiles = new List<ISerializableFile>();
                List<PatchedByteArray> patches = new List<PatchedByteArray>();

                foreach (ISerializableFile file in files)
                {
                    worker.ReportProgress( 0,
                        new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Starting, Task = ProgressForm.Task.IsDteNeeded } );
                    if (file.IsDteNeeded())
                    {
                        dteFiles.Add( file );
                    }
                    else
                    {
                        nonDteFiles.Add( file );
                    }
                    worker.ReportProgress( 0,
                        new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Done, Task = ProgressForm.Task.IsDteNeeded } );
                    if (worker.CancellationPending)
                    {
                        args.Cancel = true;
                        return;
                    }
                }
                if (dteFiles.Count > 0)
                {
                    IDictionary<byte, string> dteMap;
                    var dtePatches = DoDteCrap( dteFiles, worker, args, out dteMap );
                    if (dtePatches == null)
                    {
                        args.Cancel = true;
                        return;
                    }
                    patches.AddRange( dtePatches );
                    patches.AddRange( GetCharMapPatches( stream, dteFiles[0].Context, CharMap, dteMap ) );
                }
                else
                {
                    patches.AddRange( RemoveCharMapPatches( stream, files[0].Context ) );
                }

                foreach (var file in nonDteFiles)
                {
                    worker.ReportProgress( 0,
                        new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Starting, Task = ProgressForm.Task.GeneratePatch } );
                    patches.AddRange( file.GetNonDtePatches() );
                    worker.ReportProgress( 0,
                        new ProgressForm.FileProgress { File = file, State = ProgressForm.TaskState.Done, Task = ProgressForm.Task.GeneratePatch } );
                    if (worker.CancellationPending)
                    {
                        args.Cancel = true;
                        return;
                    }
                }

                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }

                if (customGlyphs != null)
                {

                    if (this.Filetype == Context.US_PSX)
                    {
                        const int battleBinOffset = 0xE7614;
                        const int fontBinOffset = 0;
                        const int worldBinOffset = 0x5B8f8;
                        const int battleBinWidthOffset = 0xFF0FC;
                        const int worldBinWidthOffset = 0x733E0;

                        foreach (Glyph g in customGlyphs)
                        {
                            var glyphBytes = g.ToByteArray();
                            byte[] widthBytes = new byte[] { g.Width };
                            patches.Add( new PatchedByteArray( PsxIso.Sectors.BATTLE_BIN, battleBinOffset+g.Index*14*10/4, glyphBytes ) );
                            patches.Add( new PatchedByteArray( PsxIso.Sectors.EVENT_FONT_BIN, fontBinOffset+g.Index*14*10/4, glyphBytes  ) );
                            patches.Add( new PatchedByteArray( PsxIso.Sectors.WORLD_WORLD_BIN, worldBinOffset+g.Index*14*10/4, glyphBytes  ) );
                            patches.Add( new PatchedByteArray( PsxIso.Sectors.BATTLE_BIN, battleBinWidthOffset+g.Index, widthBytes ) );
                            patches.Add( new PatchedByteArray( PsxIso.Sectors.WORLD_WORLD_BIN, worldBinWidthOffset+g.Index, widthBytes ) );
                        }
                    }
                    else
                    {
                        const int fontOffset1 = 0x27B80C;
                        const int fontOffset2 = 0x2F73B8;
                        const int widthOffset1 = 0x293F40;
                        const int widthOffset2 = 0x30FAC0;

                        foreach (Glyph g in customGlyphs)
                        {
                            var glyphBytes = g.ToByteArray();
                            byte[] widthBytes = new byte[] { g.Width };
                            patches.Add( new PatchedByteArray( PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, fontOffset1 + g.Index * 14 * 10 / 4, glyphBytes ) );
                            patches.Add( new PatchedByteArray( PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, fontOffset2 + g.Index * 14 * 10 / 4, glyphBytes ) );
                            patches.Add( new PatchedByteArray( PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, widthOffset1 + g.Index, widthBytes ) );
                            patches.Add( new PatchedByteArray( PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, widthOffset2 + g.Index, widthBytes ) );
                            patches.Add( new PatchedByteArray( PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, fontOffset1 + g.Index * 14 * 10 / 4, glyphBytes ) );
                            patches.Add( new PatchedByteArray( PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, fontOffset2 + g.Index * 14 * 10 / 4, glyphBytes ) );
                            patches.Add( new PatchedByteArray( PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, widthOffset1 + g.Index, widthBytes ) );
                            patches.Add( new PatchedByteArray( PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, widthOffset2 + g.Index, widthBytes ) );
                        }
                    }
                }

                worker.ReportProgress( 0,
                    new ProgressForm.FileProgress { File = null, State = ProgressForm.TaskState.Starting, Task = ProgressForm.Task.ApplyingPatches } );
                patchArgs.Patcher( stream, patches );
                worker.ReportProgress( 0,
                    new ProgressForm.FileProgress { File = null, State = ProgressForm.TaskState.Done, Task = ProgressForm.Task.ApplyingPatches } );

            }
        }

        public IList<IFile> Files { get; private set; }
        private IList<Glyph> customGlyphs;

        internal FFTText( Context context, IDictionary<Guid, ISerializableFile> files, IList<Glyph> customGlyphs, QuickEdit quickEdit )
        {
            this.customGlyphs = customGlyphs;
            Filetype = context;
            List<IFile> filesList = new List<IFile>( files.Count + 1 );
            files.ForEach( kvp => filesList.Add( kvp.Value ) );
            filesList.Sort( ( a, b ) => a.DisplayName.CompareTo( b.DisplayName ) );
            if (quickEdit != null)
            {
                filesList.Insert(0, quickEdit);
            }
            Files = filesList.AsReadOnly();
            CharMap = filesList[0].CharMap;
        }

        public static FFTText ReadPSPIso( string filename, BackgroundWorker worker )
        {
            using (FileStream stream = File.Open( filename, FileMode.Open, FileAccess.Read ))
            {
                return ReadPSPIso( stream, worker );
            }
        }

        public static FFTText ReadPSPIso( FileStream stream, BackgroundWorker worker )
        {
            return FFTTextFactory.GetPspText( stream, worker );
        }

        public static FFTText ReadPSXIso( string filename, BackgroundWorker worker )
        {
            using (FileStream stream = File.Open( filename, FileMode.Open, FileAccess.Read ))
            {
                return ReadPSXIso( stream, worker );
            }
        }

        public static FFTText ReadPSXIso( FileStream stream, BackgroundWorker worker )
        {
            return FFTTextFactory.GetPsxText( stream, worker );
        }

        #endregion Methods
    }
}