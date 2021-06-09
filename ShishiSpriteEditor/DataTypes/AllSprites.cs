using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PatcherLib.Datatypes;
using PatcherLib;
using PatcherLib.Utilities;

namespace FFTPatcher.SpriteEditor
{
    public class AllSprites
    {
        public class AllSpritesDoWorkData
        {
            public Stream ISO { get; private set; }
            public string Path { get; private set; }
            public bool ImportExport8bpp { get; private set; }
            public int PaletteIndex { get; private set; }
            public AllSpritesDoWorkData(Stream iso, string path, bool importExport8bpp, int paletteIndex)
            {
                ISO = iso;
                Path = path;
                ImportExport8bpp = importExport8bpp;
                PaletteIndex = paletteIndex;
            }
        }

        public class AllSpritesDoWorkResult
        {
            public enum Result
            {
                Success,
                Failure
            }

            public Result DoWorkResult { get; private set; }
            public int ImagesProcessed { get; private set; }
            public AllSpritesDoWorkResult(Result result, int images)
            {
                DoWorkResult = result;
                ImagesProcessed = images;
            }

        }

        private IList<Sprite> sprites;
        private AllSpriteAttributes attrs;
        private SpriteFileLocations locs;

        //private HashSet<int> shortSpriteIndexes = new HashSet<int>() { 0x1C, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x50, 0x51, 0x52, 0x53 };

        public const int NumPsxSprites = 159;   // 154;
        //const long defaultIsoLength = 541315152;
        //const long expandedIsoLength = 0x20F18D00;                  // 552701184
        //const long defaultSectorCount = 230151;
        //const long expandedSectorCount = 0x20F18D00 / 2352;       // 234992
        //const long expandedSectorCount = 235270;                    // 235120 + 150

        public int Count { get; private set; }

        public Sprite this[int i]
        {
            get { return sprites[i]; }
        }

        public IDictionary<Sprite, IList<int>> SharedSPRs { get; private set; }

        public static AllSprites FromIso( Stream iso, bool expand )
        {
            if (iso.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode2Form1] == 0)
            {
                // assume psx
                return FromPsxIso( iso, expand );
            }
            else if (iso.Length % PatcherLib.Iso.IsoPatch.SectorSizes[PatcherLib.Iso.IsoPatch.IsoType.Mode1] == 0)
            {
                // assume psp
                return FromPspIso( iso, expand );
            }
            else
            {
                throw new ArgumentException( "iso" );
            }
        }

        public static AllSprites FromPsxIso( Stream iso, bool expand )
        {
            if (expand && !DetectExpansionOfPsxIso( iso ))
            {
                ExpandPsxIso( iso );
            }

            return new AllSprites( Context.US_PSX, AllSpriteAttributes.FromPsxIso( iso ), SpriteFileLocations.FromPsxIso( iso ),
                new Sprite[] {
                    new WepSprite(Context.US_PSX, WepSprite.Wep.WEP1, "WEP1", new PatcherLib.Iso.PsxIso.KnownPosition( PatcherLib.Iso.PsxIso.Sectors.BATTLE_WEP_SPR,0,256*256/2+0x200)),
                    new WepSprite(Context.US_PSX, WepSprite.Wep.WEP2, "WEP2", new PatcherLib.Iso.PsxIso.KnownPosition( PatcherLib.Iso.PsxIso.Sectors.BATTLE_WEP_SPR,0,256*256/2+0x200)),
                    new WepSprite(Context.US_PSX, WepSprite.Wep.EFF1, "EFF1", new PatcherLib.Iso.PsxIso.KnownPosition( PatcherLib.Iso.PsxIso.Sectors.BATTLE_WEP_SPR,256*256/2+0x200,256*256/2+0x200)) ,
                    new WepSprite(Context.US_PSX, WepSprite.Wep.EFF2, "EFF2", new PatcherLib.Iso.PsxIso.KnownPosition( PatcherLib.Iso.PsxIso.Sectors.BATTLE_WEP_SPR,256*256/2+0x200,256*256/2+0x200)) ,
                    new WepSprite(Context.US_PSX, WepSprite.Wep.EFF1, "TRAP1", new PatcherLib.Iso.PsxIso.KnownPosition( PatcherLib.Iso.PsxIso.Sectors.BATTLE_WEP_SPR,(256*256/2+0x200)*2,144*256/2+0x200)) ,}
                   );

        }

        public static AllSprites FromPspIso( Stream iso, bool expand )
        {
            PatcherLib.Iso.PspIso.PspIsoInfo info = PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo( iso );

            return new AllSprites( Context.US_PSP, AllSpriteAttributes.FromPspIso( iso, info ), SpriteFileLocations.FromPspIso( iso, info ),
                new Sprite[] {
                    new WepSprite(Context.US_PSP, WepSprite.Wep.WEP1, "WEP1", new PatcherLib.Iso.PspIso.KnownPosition( PatcherLib.Iso.FFTPack.Files.BATTLE_WEP_SPR,0,256*256/2+0x200)),
                    new WepSprite(Context.US_PSP, WepSprite.Wep.WEP2, "WEP2", new PatcherLib.Iso.PspIso.KnownPosition( PatcherLib.Iso.FFTPack.Files.BATTLE_WEP_SPR,0,256*256/2+0x200)),
                    new WepSprite(Context.US_PSP, WepSprite.Wep.EFF1, "EFF1", new PatcherLib.Iso.PspIso.KnownPosition( PatcherLib.Iso.FFTPack.Files.BATTLE_WEP_SPR,256*256/2+0x200,256*256/2+0x200)) ,
                    new WepSprite(Context.US_PSP, WepSprite.Wep.EFF2, "EFF2", new PatcherLib.Iso.PspIso.KnownPosition( PatcherLib.Iso.FFTPack.Files.BATTLE_WEP_SPR,256*256/2+0x200,256*256/2+0x200)),
                    new WepSprite(Context.US_PSP, WepSprite.Wep.EFF2, "TRAP1", new PatcherLib.Iso.PspIso.KnownPosition( PatcherLib.Iso.FFTPack.Files.BATTLE_WEP_SPR,(256*256/2+0x200)*2,144*256/2+0x200)),}
                     );
        }

        struct Time
        {
            private byte min;
            private byte sec;
            private byte f;
            public byte Minutes { get { return min; } }
            public byte Seconds { get { return sec; } }
            public byte Frames { get { return f; } }
            public Time( byte m, byte s, byte f )
            {
                min = m;
                sec = s;
                this.f = f;
            }

            public Time AddFrame()
            {
                byte newF = (byte)(f + 1);
                byte newS = sec;
                byte newMin = min;

                if (newF == 75)
                {
                    newF = 0;
                    newS++;
                    if (newS == 60)
                    {
                        newS = 0;
                        newMin++;
                    }
                }
                return new Time( newMin, newS, newF );
            }
            public byte[] ToBCD()
            {
                return new byte[] {
                    (byte)(min/10 * 16 + min%10),
                    (byte)(sec/10 * 16 + sec%10),
                    (byte)(f/10 * 16 + f%10) };
            }
        }

        private static void ExpandPspIso( Stream iso )
        {
            throw new InvalidOperationException( "This method doesn't work." );
            return;

            string tempPath = Path.GetTempPath();
            string guid = Path.GetRandomFileName();
            string tempDirPath = Path.Combine( tempPath, guid );
            DirectoryInfo temp = Directory.CreateDirectory( tempDirPath );

            PatcherLib.Iso.PspIso.PspIsoInfo info = PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo( iso );
            long fftpackSector = info[PatcherLib.Iso.PspIso.Sectors.PSP_GAME_USRDIR_fftpack_bin];
            iso.Seek( 2048 * fftpackSector, SeekOrigin.Begin );

            // Dump the fftpack
            PatcherLib.Iso.FFTPack.DumpToDirectory( iso, tempDirPath, info.GetFileSize( PatcherLib.Iso.PspIso.Sectors.PSP_GAME_USRDIR_fftpack_bin ), null );

            // Decrypt the ISO  while we have it open...
            PatcherLib.Iso.PspIso.DecryptISO( iso, info );

            string battleDirPath = Path.Combine( tempDirPath, "BATTLE" );

            // Read the sector -> fftpack map
            IList<byte> fftpackMap =
                PatcherLib.Iso.PspIso.GetBlock(
                    iso,
                    info,
                    new PatcherLib.Iso.PspIso.KnownPosition( PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x252f34, 0x3e00 ) );

            // Convert the fake "Sectors" into FFTPack indices
            Dictionary<uint, int> sectorToFftPackMap = new Dictionary<uint, int>();
            Dictionary<int, uint> fftPackToSectorMap = new Dictionary<int, uint>();
            for (int i = 3; i < PatcherLib.Iso.FFTPack.NumFftPackFiles - 1; i++)
            {
                UInt32 sector = fftpackMap.Sub( (i - 3) * 4, (i - 3) * 4 + 4 - 1 ).ToUInt32();
                sectorToFftPackMap.Add( sector, i );
                fftPackToSectorMap.Add( i, sector );
            }

            //const int numPspSp2 = 0x130 / 8;
            const int numPspSprites = 170; // 0x4d0 / 8 + 0x58 / 8;
            byte[][] oldSpriteBytes = new byte[numPspSprites][];

            // Save the old sprites
            var locs = SpriteFileLocations.FromPspIso( iso, PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo( iso ) );
            for (int i = 0; i < numPspSprites; i++)
            {
                oldSpriteBytes[i] = new byte[65536];

                PatcherLib.Iso.FFTPack.GetFileFromIso( iso, info, (PatcherLib.Iso.FFTPack.Files)sectorToFftPackMap[locs[i].Sector] ).CopyTo( oldSpriteBytes[i], 0 );
            }

            byte[] emptyByteArray = new byte[0];
            // Replace old sprites
            //for (int i = 78; i <= 213; i++)
            //{
            //    string currentFile = Path.Combine(tempDirPath, PatcherLib.Iso.FFTPack.FFTPackFiles[i]);
            //    File.Delete(currentFile);
            //    File.WriteAllBytes(currentFile, emptyByteArray);
            //}

            for (int i = 0; i < numPspSprites; i++)
            {
                File.Delete( Path.Combine( tempDirPath, string.Format( "unknown/fftpack.{0}.dummy", i + 1340 ) ) );
                File.WriteAllBytes( Path.Combine( tempDirPath, string.Format( "unknown/fftpack.{0}", i + 1340 ) ), oldSpriteBytes[i] );
                locs[i].Sector = fftPackToSectorMap[i + 1340];
                locs[i].Size = 65536;
            }

            List<byte> newSpriteLocations = new List<byte>();
            for (int i = 0; i < 159; i++)
            {
                newSpriteLocations.AddRange( locs[i].Sector.ToBytes() );
                newSpriteLocations.AddRange( locs[i].Size.ToBytes() );
            }
            newSpriteLocations.AddRange( new byte[32] );
            for (int i = 159; i < numPspSprites; i++)
            {
                newSpriteLocations.AddRange( locs[i].Sector.ToBytes() );
                newSpriteLocations.AddRange( locs[i].Size.ToBytes() );
            }

            byte[] newSpriteLocationsArray = newSpriteLocations.ToArray();
            string outputPath = Path.Combine( Path.GetTempPath(), Path.GetRandomFileName() );
            PatcherLib.Iso.FFTPack.MergeDumpedFiles( tempDirPath, outputPath, null );

            long oldFftPackSize = info.GetFileSize( PatcherLib.Iso.PspIso.Sectors.PSP_GAME_USRDIR_fftpack_bin );
            byte[] oldFftPackSizeBytes = oldFftPackSize.ToBytes();
            oldFftPackSizeBytes = new byte[8] { 
                oldFftPackSizeBytes[0], oldFftPackSizeBytes[1], oldFftPackSizeBytes[2], oldFftPackSizeBytes[3], 
                oldFftPackSizeBytes[3], oldFftPackSizeBytes[2], oldFftPackSizeBytes[1], oldFftPackSizeBytes[0] };

            using (Stream newFftPack = File.OpenRead( outputPath ))
            {
                long newFftPackSize = newFftPack.Length;
                byte[] newFftPackSizeBytes = newFftPackSize.ToBytes();
                newFftPackSizeBytes = new byte[8] { 
                    newFftPackSizeBytes[0], newFftPackSizeBytes[1], newFftPackSizeBytes[2], newFftPackSizeBytes[3], 
                    newFftPackSizeBytes[3], newFftPackSizeBytes[2], newFftPackSizeBytes[1], newFftPackSizeBytes[0] };

                ReplaceBytesInStream( iso, oldFftPackSizeBytes, newFftPackSizeBytes );
                CopyStream( newFftPack, 0, iso, info[PatcherLib.Iso.PspIso.Sectors.PSP_GAME_USRDIR_fftpack_bin] * 2048, newFftPack.Length );
                long oldLength = info.GetFileSize( PatcherLib.Iso.PspIso.Sectors.PSP_GAME_USRDIR_fftpack_bin );
                if (newFftPack.Length < oldLength)
                {
                    iso.Write( new byte[oldLength - newFftPack.Length], 0, (int)(oldLength - newFftPack.Length) );
                }
            }
            Directory.Delete( tempDirPath, true );
            File.Delete( outputPath );

            PatcherLib.Iso.PspIso.PatchISO( iso, new PatchedByteArray[] { 
                new PatchedByteArray(PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x324824, newSpriteLocationsArray),
                new PatchedByteArray(PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x324824, newSpriteLocationsArray)} );

        }

        private static void ReplaceBytesInStream( Stream stream, byte[] bytesToReplace, byte[] newBytes )
        {
            System.Diagnostics.Debug.Assert( bytesToReplace.Length == newBytes.Length );
            byte[] buffer = new byte[bytesToReplace.Length];
            stream.Seek( 0, SeekOrigin.Begin );
            int bytesRead = stream.Read( buffer, 0, buffer.Length );
            while (bytesRead == buffer.Length)
            {
                if (PatcherLib.Utilities.Utilities.CompareArrays( buffer, bytesToReplace ))
                {
                    stream.Seek( -buffer.Length, SeekOrigin.Current );
                    stream.Write( newBytes, 0, newBytes.Length );
                    break;
                }
                else
                {
                    stream.Seek( -(buffer.Length - 1), SeekOrigin.Current );
                    bytesRead = stream.Read( buffer, 0, buffer.Length );
                }
            }
        }

        private static void CopyStream( Stream source, long sourcePosition, Stream destination, long destinationPosition, long count )
        {
            long copied = 0;
            byte[] buffer = new byte[2048];
            source.Seek( sourcePosition, SeekOrigin.Begin );
            destination.Seek( destinationPosition, SeekOrigin.Begin );

            while (copied < count)
            {
                int bytesCopied = source.Read( buffer, 0, 2048 );
                destination.Write( buffer, 0, bytesCopied );
                copied += bytesCopied;
            }
        }

        internal void LoadAllSprites(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            AllSpritesDoWorkData data = e.Argument as AllSpritesDoWorkData;
            if (data == null)
                return;
            e.Result = LoadAllSprites(data.ISO, data.Path, data.ImportExport8bpp, data.PaletteIndex, worker.WorkerReportsProgress ? (Action<int>)worker.ReportProgress : null);
        }

        internal void DumpAllSprites(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
            AllSpritesDoWorkData data = e.Argument as AllSpritesDoWorkData;
            if (data == null)
                return;
            e.Result = DumpAllSprites(data.ISO, data.Path, data.ImportExport8bpp, data.PaletteIndex, worker.WorkerReportsProgress ? (Action<int>)worker.ReportProgress : null);
        }

        private AllSpritesDoWorkResult LoadAllSprites(Stream iso, string path, bool importExport8bpp, int paletteIndex, Action<int> progressReporter)
        {
            bool progress = progressReporter != null;
            int total = 0;
            int complete = 0;
            int imagesProcessed = 0;

            Dictionary<string, Sprite> fileMap = new Dictionary<string, Sprite>();
            foreach (Sprite sprite in sprites)
            {
                if (sprite.Size > 0)
                {
                    string name = sprite.GetSaveFileName();
                    if (!fileMap.ContainsKey(name))
                    {
                        fileMap.Add(name, sprite);
                        total = total + 1;
                    }
                }
            }

            /*
            if (progress)
            {
                sprites.ForEach(i => total += 1);
            }
            */

            //foreach (Sprite sprite in sprites)
            foreach (KeyValuePair<string, Sprite> singleFileMap in fileMap)
            {
                //string name = string.Empty;
                //name = sprite.GetSaveFileName();
                //name = Path.Combine(path, name);

                string name = Path.Combine(path, singleFileMap.Key);
                if (File.Exists(name))
                {
                    Sprite sprite = singleFileMap.Value;

                    try
                    {
                        if (importExport8bpp)
                            sprite.ImportBitmap8bpp(iso, name);
                        else
                            sprite.ImportBitmap4bpp(iso, name, paletteIndex);

                        imagesProcessed++;
                    }
                    catch (Exception ex) 
                    {
                        //MyMessageBox.Show(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));      // DEBUG
                    }
                }
                if (progress)
                {
                    progressReporter((100 * (complete++)) / total);
                }
            }

            return new AllSpritesDoWorkResult(AllSpritesDoWorkResult.Result.Success, imagesProcessed);
        }

        private AllSpritesDoWorkResult DumpAllSprites(Stream iso, string path, bool importExport8bpp, int paletteIndex, Action<int> progressReporter)
        {
            bool progress = progressReporter != null;
            int total = 0;
            int complete = 0;
            int imagesProcessed = 0;

            /*
            if (progress)
                sprites.ForEach(i => total += 1);
            */

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Dictionary<string, Sprite> fileMap = new Dictionary<string, Sprite>();
            foreach (Sprite sprite in sprites)
            {
                if ((sprite != null) && (sprite.Size > 0))
                {
                    string name = sprite.GetSaveFileName();
                    if (!fileMap.ContainsKey(name))
                    {
                        fileMap.Add(name, sprite);
                        total = total + 1;
                    }
                }
            }

            //foreach (Sprite sprite in sprites)
            foreach (KeyValuePair<string, Sprite> singleFileMap in fileMap)
            {
                //string name = string.Empty;
                //name = sprite.GetSaveFileName();
                string name = singleFileMap.Key;

                if (!string.IsNullOrEmpty(name))
                {
                    //Bitmap bmp = img.GetImageFromIso( iso );
                    //bmp.Save( Path.Combine( path, name ), System.Drawing.Imaging.ImageFormat.Bmp );
                    string fullPath = Path.Combine(path, name);
                    Sprite sprite = singleFileMap.Value;
                    AbstractSprite abstractSprite = sprite.GetAbstractSpriteFromIso(iso, true);

                    if (abstractSprite != null)
                    {
                        System.Drawing.Bitmap bitmap = importExport8bpp ? abstractSprite.ToBitmap() : abstractSprite.To4bppBitmapUncached(paletteIndex);
                        bitmap.Save(fullPath, System.Drawing.Imaging.ImageFormat.Bmp);

                        imagesProcessed++;
                    }
                    /*
                    else
                    {
                        int x = 4;
                    }
                    */
                }

                if (progress)
                {
                    progressReporter((100 * (complete++)) / total);
                }
            }

            return new AllSpritesDoWorkResult(AllSpritesDoWorkResult.Result.Success, imagesProcessed);
        }

        public static void ExpandPsxIso(Stream iso)
        {
            //byte[] expandedBytes = expandedSectorCount.ToBytes();
            //byte[] reverseBytes = new byte[4] { expandedBytes[3], expandedBytes[2], expandedBytes[1], expandedBytes[0] };
            //PatcherLib.Iso.PsxIso.PatchPsxIso(iso, PatcherLib.Iso.PsxIso.NumberOfSectorsLittleEndian.GetPatchedByteArray(expandedBytes));
            //PatcherLib.Iso.PsxIso.PatchPsxIso(iso, PatcherLib.Iso.PsxIso.NumberOfSectorsBigEndian.GetPatchedByteArray(reverseBytes));

            // Read old sprites
            var locs = SpriteFileLocations.FromPsxIso(iso);
            byte[][] oldSprites = new byte[NumPsxSprites][];
            for (int i = 0; i < NumPsxSprites; i++)
            {
                var loc = locs[i];
                oldSprites[i] = PatcherLib.Iso.PsxIso.ReadFile(iso, (PatcherLib.Iso.PsxIso.Sectors)loc.Sector, 0, (int)loc.Size);
            }

            Set<string> allowedEntries = new Set<string>(new string[] {
                "\0", "\x01", "ARUTE.SEQ;1", "ARUTE.SHP;1", "CYOKO.SEQ;1", "CYOKO.SHP;1", "EFC_FNT.TIM;1", "EFF1.SEQ;1", "EFF1.SHP;1", "EFF2.SEQ;1", "EFF2.SHP;1", 
                "ENTD1.ENT;1", "ENTD2.ENT;1", "ENTD3.ENT;1", "ENTD4.ENT;1", "KANZEN.SEQ;1", "KANZEN.SHP;1", "MON.SEQ;1", "MON.SHP;1", "OTHER.SEQ;1", "OTHER.SHP;1", "OTHER.SPR;1", 
                "RUKA.SEQ;1", "TYPE1.SEQ;1", "TYPE1.SHP;1", "TYPE2.SEQ;1", "TYPE2.SHP;1", "TYPE3.SEQ;1", "TYPE4.SEQ;1", "WEP.SPR;1", "WEP1.SEQ;1", "WEP1.SHP;1", "WEP2.SEQ;1", "WEP2.SHP;1", 
                "ZODIAC.BIN;1"});

            List<PatcherLib.Iso.DirectoryEntry> battleDir = new List<PatcherLib.Iso.DirectoryEntry>(PatcherLib.Iso.DirectoryEntry.GetPsxBattleEntries(iso));
            byte[] extBytes = battleDir[2].ExtendedBytes;
            System.Diagnostics.Debug.Assert(battleDir.Sub(2).TrueForAll(ent => PatcherLib.Utilities.Utilities.CompareArrays(extBytes, ent.ExtendedBytes)));
            byte[] midBytes = battleDir[2].MiddleBytes;
            System.Diagnostics.Debug.Assert(battleDir.Sub(2).TrueForAll(ent => PatcherLib.Utilities.Utilities.CompareArrays(midBytes, ent.MiddleBytes)));
            battleDir.RemoveAll(dirent => !allowedEntries.Contains(dirent.Filename));

            // Expand length of ISO
            //byte[] anchorBytes = new byte[] { 
            //        0x00, 0xFF, 0xFF, 0xFF, 
            //        0xFF, 0xFF, 0xFF, 0xFF, 
            //        0xFF, 0xFF, 0xFF, 0x00 };
            //byte[] sectorBytes = new byte[] {
            //    0x00, 0x00, 0x08, 0x00,
            //    0x00, 0x00, 0x08, 0x00 };
            //byte[] endOfFileBytes = new byte[] {
            //    0x00, 0x00, 0x89, 0x00,
            //    0x00, 0x00, 0x89, 0x00 };
            //byte[] sectorBytes = new byte[8];
            //byte[] endOfFileBytes = new byte[8];
            //byte[] emptySector = new byte[2328];
            //Time t = new Time( 51, 9, 39 );
            //Time t = new Time(51, 9, 7);

            /*
            const long startSector = 230032;
            const long endSector = 235270; // 235120 + 150;     // final sector + 1
            long startLoc = startSector * 2352;
            long endLoc = endSector * 2352;                     // first location out of range of ISO

            //for (long l = 0x2040B100; l < 0x20F18D00; l += 2352)
            for (long l = startLoc; l < endLoc; l += 2352)
            {
                // write 0x00FFFFFF FFFFFFFF FFFFFF00 MM SS FF 02
                // write 0x00000800 00000800 for sector of file
                // write 0x00008900 00008900 for last sector of file
                iso.Seek(l, SeekOrigin.Begin);
                iso.Write(anchorBytes, 0, anchorBytes.Length);
                iso.Write(t.ToBCD(), 0, 3);
                t = t.AddFrame();
                iso.WriteByte(0x02);
                if ((l - startLoc + 2352) % 0x12600 != 0)
                {
                    iso.Write(sectorBytes, 0, 8);
                }
                else
                {
                    iso.Write(endOfFileBytes, 0, 8);
                }
                iso.Write(emptySector, 0, 2328);
            }
            */

            // Copy old sprites to new locations
            const int startSector = 219250;
            List<byte> posBytes = new List<byte>(NumPsxSprites * 8);
            //const long startSector = 0x2040B100 / 2352;
            for (int i = 0; i < NumPsxSprites; i++)
            {
                uint sector = (uint)(startSector + (i * (65536 / 2048)));
                byte[] bytes = oldSprites[i];
                byte[] realBytes = new byte[65536];
                bytes.CopyTo(realBytes, 0);
                PatcherLib.Iso.PsxIso.PatchPsxIso(iso, new PatchedByteArray((int)sector, 0, realBytes));
                posBytes.AddRange(sector.ToBytes());
                posBytes.AddRange(((uint)realBytes.Length).ToBytes());

                battleDir.Add(new PatcherLib.Iso.DirectoryEntry(sector, 65536, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                    string.Format("{0:X2}.SPR;1", i), battleDir[2].ExtendedBytes));
            }

            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_ARLI2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "8D.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_BIBU2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "96.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_BOM2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "88.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_BEHI2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "93.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_DEMON2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "99.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_DORA22_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "95.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_HYOU2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "89.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_IRON5_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "9A_2.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_IRON4_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "9A_3.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_IRON2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "9A_4.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_IRON3_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "9A_5.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_MINOTA2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "91.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_MOL2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "92.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_TORI2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "8E.SP2;1", battleDir[2].ExtendedBytes));
            battleDir.Add(new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_URI2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "8F.SP2;1", battleDir[2].ExtendedBytes));

            //"ARLI2.SP2;1",  // 0x8c     
            //"BIBU2.SP2;1", // 0x95
            //"BOM2.SP2;1", // 0x87
            //"BEHI2.SP2;1", // 0x92
            //"DEMON2.SP2;1", // 0x98         
            //"DORA22.SP2;1", // 0x94
            //"HYOU2.SP2;1",  // 0x88
            //"IRON5.SP2;1",                
            //"IRON4.SP2;1",                
            //"IRON2.SP2;1",                
            //"IRON3.SP2;1",
            //"MINOTA2.SP2;1", // 0x90         
            //"MOL2.SP2;1",  // 0x91  
            //"TORI2.SP2;1", // 0x8d
            //"UR2.SP2;1", // 0x8e

            battleDir.Sort((a, b) => a.Filename.CompareTo(b.Filename));

            // Patch directory entry
            PatcherLib.Iso.DirectoryEntry.WritePsxDirectoryEntries(
                iso,
                PatcherLib.Iso.PsxIso.BattleDirectoryEntrySector,
                PatcherLib.Iso.PsxIso.BattleDirectoryEntryLength,
                battleDir);

            // Update battle.bin
            PatcherLib.Iso.PsxIso.PatchPsxIso(iso, SpriteFileLocations.SpriteLocationsPosition.GetPatchedByteArray(posBytes.ToArray()));
        }

        public static void ExpandPsxIso_Old( Stream iso )
        {
            const long expandedSectorCount = 235270;

            byte[] expandedBytes = expandedSectorCount.ToBytes();
            byte[] reverseBytes = new byte[4] { expandedBytes[3], expandedBytes[2], expandedBytes[1], expandedBytes[0] };
            PatcherLib.Iso.PsxIso.PatchPsxIso( iso, PatcherLib.Iso.PsxIso.NumberOfSectorsLittleEndian.GetPatchedByteArray( expandedBytes ) );
            PatcherLib.Iso.PsxIso.PatchPsxIso( iso, PatcherLib.Iso.PsxIso.NumberOfSectorsBigEndian.GetPatchedByteArray( reverseBytes ) );
            //PatcherLib.Iso.PsxIso.PatchPsxIso( iso, 
            //    new PatchedByteArray( 
            //        (PatcherLib.Iso.PsxIso.Sectors)22, 
            //        0xDC, 
            //        new byte[] { 0x00, 0x38, 0x00, 0x00, 0x00, 0x00, 0x38, 0x00 } ) );

            // Build directory entry for /DUMMY
            //iso.Seek(0x203E6500, SeekOrigin.Begin);
            //iso.Write(Properties.Resources.PatchedDummyFolder, 0, Properties.Resources.PatchedDummyFolder.Length);

            // Read old sprites
            var locs = SpriteFileLocations.FromPsxIso( iso );
            byte[][] oldSprites = new byte[NumPsxSprites][];
            for (int i = 0; i < NumPsxSprites; i++)
            {
                var loc = locs[i];
                oldSprites[i] = PatcherLib.Iso.PsxIso.ReadFile( iso, (PatcherLib.Iso.PsxIso.Sectors)loc.Sector, 0, (int)loc.Size );
            }

            Set<string> allowedEntries = new Set<string>( new string[] {
                "\0", "\x01",
                "ARUTE.SEQ;1",                "ARUTE.SHP;1",                
                "CYOKO.SEQ;1",                "CYOKO.SHP;1",                
                "EFC_FNT.TIM;1",                "EFF1.SEQ;1",                "EFF1.SHP;1",                "EFF2.SEQ;1",
                "EFF2.SHP;1",                "ENTD1.ENT;1",                "ENTD2.ENT;1",                "ENTD3.ENT;1",
                "ENTD4.ENT;1",                
                
                "KANZEN.SEQ;1",                "KANZEN.SHP;1",
                "MON.SEQ;1",                "MON.SHP;1",
                "OTHER.SEQ;1",                "OTHER.SHP;1",                "OTHER.SPR;1",                "RUKA.SEQ;1",
                "TYPE1.SEQ;1",                "TYPE1.SHP;1",                "TYPE2.SEQ;1",                "TYPE2.SHP;1",
                "TYPE3.SEQ;1",                "TYPE4.SEQ;1",                "WEP.SPR;1",                "WEP1.SEQ;1",
                "WEP1.SHP;1",                "WEP2.SEQ;1",                "WEP2.SHP;1",                "ZODIAC.BIN;1"} );

            List<PatcherLib.Iso.DirectoryEntry> battleDir = new List<PatcherLib.Iso.DirectoryEntry>( PatcherLib.Iso.DirectoryEntry.GetPsxBattleEntries( iso ) );
            byte[] extBytes = battleDir[2].ExtendedBytes;
            System.Diagnostics.Debug.Assert( battleDir.Sub( 2 ).TrueForAll( ent => PatcherLib.Utilities.Utilities.CompareArrays( extBytes, ent.ExtendedBytes ) ) );
            byte[] midBytes = battleDir[2].MiddleBytes;
            System.Diagnostics.Debug.Assert( battleDir.Sub( 2 ).TrueForAll( ent => PatcherLib.Utilities.Utilities.CompareArrays( midBytes, ent.MiddleBytes ) ) );
            battleDir.RemoveAll( dirent => !allowedEntries.Contains( dirent.Filename ) );

            // Expand length of ISO
            byte[] anchorBytes = new byte[] { 
                    0x00, 0xFF, 0xFF, 0xFF, 
                    0xFF, 0xFF, 0xFF, 0xFF, 
                    0xFF, 0xFF, 0xFF, 0x00 };
            byte[] sectorBytes = new byte[] {
                0x00, 0x00, 0x08, 0x00,
                0x00, 0x00, 0x08, 0x00 };
            byte[] endOfFileBytes = new byte[] {
                0x00, 0x00, 0x89, 0x00,
                0x00, 0x00, 0x89, 0x00 };
            //byte[] sectorBytes = new byte[8];
            //byte[] endOfFileBytes = new byte[8];
            byte[] emptySector = new byte[2328];
            //Time t = new Time( 51, 9, 39 );
            Time t = new Time(51, 9, 7);

            const long startSector = 230032;
            const long endSector = 235270; // 235120 + 150;     // final sector + 1
            long startLoc = startSector * 2352;
            long endLoc = endSector * 2352;                     // first location out of range of ISO
            
            //for (long l = 0x2040B100; l < 0x20F18D00; l += 2352)
            for (long l = startLoc; l < endLoc; l += 2352)
            {
                // write 0x00FFFFFF FFFFFFFF FFFFFF00 MM SS FF 02
                // write 0x00000800 00000800 for sector of file
                // write 0x00008900 00008900 for last sector of file
                iso.Seek( l, SeekOrigin.Begin );
                iso.Write( anchorBytes, 0, anchorBytes.Length );
                iso.Write( t.ToBCD(), 0, 3 );
                t = t.AddFrame();
                iso.WriteByte( 0x02 );
                if ((l - startLoc + 2352) % 0x12600 != 0)
                {
                    iso.Write( sectorBytes, 0, 8 );
                }
                else
                {
                    iso.Write( endOfFileBytes, 0, 8 );
                }
                iso.Write( emptySector, 0, 2328 );
            }


            // Copy old sprites to new locations
            List<byte> posBytes = new List<byte>( NumPsxSprites * 8 );
            //const long startSector = 0x2040B100 / 2352;
            for (int i = 0; i < NumPsxSprites; i++)
            {
                uint sector = (uint)(startSector + i * 65536 / 2048);
                byte[] bytes = oldSprites[i];
                byte[] realBytes = new byte[65536];
                bytes.CopyTo( realBytes, 0 );
                PatcherLib.Iso.PsxIso.PatchPsxIso( iso, new PatchedByteArray( (int)sector, 0, realBytes ) );
                posBytes.AddRange( sector.ToBytes() );
                posBytes.AddRange( ((uint)realBytes.Length).ToBytes() );

                battleDir.Add( new PatcherLib.Iso.DirectoryEntry( sector, 65536, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                    string.Format( "{0:X2}.SPR;1", i), battleDir[2].ExtendedBytes ) );
            }

            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_ARLI2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "8D.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_BIBU2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "96.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_BOM2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "88.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_BEHI2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "93.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_DEMON2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "99.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_DORA22_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "95.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_HYOU2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "89.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_IRON5_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "9A_2.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_IRON4_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "9A_3.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_IRON2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "9A_4.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_IRON3_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "9A_5.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_MINOTA2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "91.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_MOL2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "92.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_TORI2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "8E.SP2;1", battleDir[2].ExtendedBytes ) );
            battleDir.Add( new PatcherLib.Iso.DirectoryEntry(
                (uint)PatcherLib.Iso.PsxIso.Sectors.BATTLE_URI2_SP2, 32768, DateTime.Now, battleDir[2].GMTOffset, battleDir[2].MiddleBytes,
                "8F.SP2;1", battleDir[2].ExtendedBytes ) );

            //"ARLI2.SP2;1",  // 0x8c     
            //"BIBU2.SP2;1", // 0x95
            //"BOM2.SP2;1", // 0x87
            //"BEHI2.SP2;1", // 0x92
            //"DEMON2.SP2;1", // 0x98         
            //"DORA22.SP2;1", // 0x94
            //"HYOU2.SP2;1",  // 0x88
            //"IRON5.SP2;1",                
            //"IRON4.SP2;1",                
            //"IRON2.SP2;1",                
            //"IRON3.SP2;1",
            //"MINOTA2.SP2;1", // 0x90         
            //"MOL2.SP2;1",  // 0x91  
            //"TORI2.SP2;1", // 0x8d
            //"UR2.SP2;1", // 0x8e

            battleDir.Sort( ( a, b ) => a.Filename.CompareTo( b.Filename ) );

            // Patch direntry
            PatcherLib.Iso.DirectoryEntry.WritePsxDirectoryEntries(
                iso,
                PatcherLib.Iso.PsxIso.BattleDirectoryEntrySector,
                PatcherLib.Iso.PsxIso.BattleDirectoryEntryLength,
                battleDir );

            // Erase the dummy directory, just to be sure
            //PatcherLib.Iso.DirectoryEntry.WritePsxDirectoryEntries(
            //    iso,
            //    PatcherLib.Iso.PsxIso.DummyDirectoryEntrySector,
            //    PatcherLib.Iso.PsxIso.DummyDirectoryEntryLength,
            //    new PatcherLib.Iso.DirectoryEntry[0] );

            // Update battle.bin
            PatcherLib.Iso.PsxIso.PatchPsxIso( iso, SpriteFileLocations.SpriteLocationsPosition.GetPatchedByteArray( posBytes.ToArray() ) );

        }

        public static bool DetectExpansionOfPsxIso( Stream iso )
        {
            UInt32 sectors = PatcherLib.Iso.PsxIso.ReadFile( iso, PatcherLib.Iso.PsxIso.NumberOfSectorsLittleEndian ).ToUInt32();

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
            //00  00 00  00 00 // reserved 

            //30 
            //00 
            //90 82 03 00 00 03 82 90 
            //00 00 01 00 00 01 00 00 
            //61 
            //0A 
            //11 
            //12 
            //25 
            //15 
            //24 
            //00 
            //00 00 
            //01 00 00 01 
            //0E 
            //53 50 52 49 54 45 30 30 2E 53 50 52 3B 31 
            //00 

            return //iso.Length > defaultIsoLength &&
                //iso.Length >= expandedIsoLength &&
                //sectors > defaultSectorCount &&
                //sectors >= expandedSectorCount &&
                //!SpriteFileLocations.IsoHasDefaultSpriteLocations( iso ) &&
                SpriteFileLocations.IsoHasPatchedSpriteLocations( iso );
        }

        private AllSprites( Context context, AllSpriteAttributes attrs, SpriteFileLocations locs, IList<Sprite> otherSprites )
        {
            Count = attrs.Count + otherSprites.Count;
            sprites = new Sprite[Count];
            IList<string> spriteNames = context == Context.US_PSP ? PSPResources.Lists.SpriteFiles : PSXResources.Lists.SpriteFiles;
            for (int i = 0; i < attrs.Count; i++)
            {
                sprites[i] = new CharacterSprite(
                    context,
                    string.Format( "{0:X2} - {1}", i, spriteNames[i] ),
                    attrs[i],
                    locs[i]);
            }
            otherSprites.CopyTo( sprites, attrs.Count );

            this.attrs = attrs;
            this.locs = locs;

            Dictionary<Sprite, IList<int>> sharedSPRs = new Dictionary<Sprite, IList<int>>();
            for (int i = 0; i < sprites.Count; i++)
            {
                sharedSPRs.Add( sprites[i], new List<int>() );
            }

            for (int i = 0; i < attrs.Count; i++)
            {
                for (int j = i + 1; j < attrs.Count; j++)
                {
                    if (locs[i].Sector == locs[j].Sector)
                    {
                        sharedSPRs[sprites[i]].Add( j );
                        sharedSPRs[sprites[j]].Add( i );
                    }
                }
            }

            for (int i = 0; i < sprites.Count; i++)
            {
                sharedSPRs[sprites[i]].Sort();
                sharedSPRs[sprites[i]] = sharedSPRs[sprites[i]].AsReadOnly();
            }

            SharedSPRs = new ReadOnlyDictionary<Sprite, IList<int>>( sharedSPRs );
        }

    }
}
