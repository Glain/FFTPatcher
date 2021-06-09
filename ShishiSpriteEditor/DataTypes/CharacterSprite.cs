using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PatcherLib.Utilities;

namespace FFTPatcher.SpriteEditor
{
    public class CharacterSprite : Sprite
    {
        private SpriteAttributes attributes;
        private SpriteLocation location;

        public SpriteType SHP { get { return attributes.SHP; } }
        public SpriteType SEQ { get { return attributes.SEQ; } }
        public bool Flag1 { get { return attributes.Flag1; } }
        public bool Flag2 { get { return attributes.Flag2; } }
        public bool Flag3 { get { return attributes.Flag3; } }
        public bool Flag4 { get { return attributes.Flag4; } }
        public bool Flag5 { get { return attributes.Flag5; } }
        public bool Flag6 { get { return attributes.Flag6; } }
        public bool Flag7 { get { return attributes.Flag7; } }
        public bool Flag8 { get { return attributes.Flag8; } }
        public bool Flying { get { return attributes.Flying; } }
        private const int sp2MaxLength = 256 * 256 / 2;
        public int NumChildren { get { return location.SubSpriteLocations.Count; } }
        internal void SetSHP(Stream iso, SpriteType shp)
        {
            if (SHP != shp)
            {
                if (SHP == SpriteType.MON || shp == SpriteType.MON)
                {
                    CachedSprite = null;
                }

                attributes.SetSHP(iso, shp);
            }
        }

        internal void SetSEQ(Stream iso, SpriteType seq)
        {
            attributes.SetSEQ(iso, seq);
        }

        internal void SetFlying(Stream iso, bool flying)
        {
            attributes.SetFlying(iso, flying);
        }

        internal void SetFlag(Stream iso, int index, bool flag)
        {
            attributes.SetFlag(iso, index, flag);
        }

        private enum SpriteAlignment
        {
            Legacy,
            Correct,
            Unknown
        }

        private SpriteAlignment DetermineSpriteAlignment(System.Drawing.Bitmap bmp)
        {
            int legacyPercentage =
                DeterminePercentageOfBlackPixels(bmp, new System.Drawing.Rectangle(80, 256, 50, 32));
            int correctPercentage =
                DeterminePercentageOfBlackPixels(bmp, new System.Drawing.Rectangle(80, 256 + 200, 50, 32));
            return legacyPercentage > correctPercentage ? SpriteAlignment.Correct : SpriteAlignment.Legacy;
        }

        internal override void ImportBitmap(Stream iso, System.Drawing.Bitmap bmp)
        {
            bool bad = false;
            AbstractSprite sprite = GetAbstractSpriteFromIso(iso);
            sprite.ImportBitmap(bmp, out bad);
            byte[] sprBytes = sprite.ToByteArray(0);
            if (sprBytes.Length > Size)
            {
                throw new SpriteTooLargeException(sprBytes.Length, (int)Size);
            }

            ImportSprite(iso, sprBytes);
            for (int i = 0; i < NumChildren; i++)
            {
                ImportSp2(iso, sprite.ToByteArray(i + 1), i);
            }
        }

        internal override void ImportBitmap4bpp(Stream iso, string filename, int paletteIndex)
        {
            AbstractSprite sprite = GetAbstractSpriteFromIso(iso);
            byte[] importBytes = System.IO.File.ReadAllBytes(filename);
            const int totalPaletteBytes = 32 * 16;
            byte[] originalPaletteBytes = Position.AddOffset(0, totalPaletteBytes - Position.Length).ReadIso(iso);
            sprite.ImportBitmap4bpp(paletteIndex, importBytes, originalPaletteBytes);

            byte[] sprBytes = sprite.ToByteArray(0);
            if (sprBytes.Length > Size)
            {
                throw new SpriteTooLargeException(sprBytes.Length, (int)Size);
            }

            //System.IO.File.WriteAllBytes(@"spr4.bin", sprBytes);    // DEBUG
            ImportSprite(iso, sprBytes);
            for (int i = 0; i < NumChildren; i++)
            {
                //System.IO.File.WriteAllBytes(@"sp2_4.bin", sprite.ToByteArray(i + 1));    // DEBUG
                ImportSp2(iso, sprite.ToByteArray(i + 1), i);
            }
        }

        internal override void ImportBitmap8bpp(Stream iso, string filename)
        {
            AbstractSprite sprite = GetAbstractSpriteFromIso(iso);
            byte[] importBytes = System.IO.File.ReadAllBytes(filename);
            const int totalPaletteBytes = 512;
            byte[] originalPaletteBytes = Position.AddOffset(0, totalPaletteBytes - Position.Length).ReadIso(iso);
            sprite.ImportBitmap8bpp(importBytes, originalPaletteBytes);
            
            byte[] sprBytes = sprite.ToByteArray(0);
            if (sprBytes.Length > Size)
            {
                throw new SpriteTooLargeException(sprBytes.Length, (int)Size);
            }

            //System.IO.File.WriteAllBytes(@"spr8.bin", sprBytes);    // DEBUG
            ImportSprite(iso, sprBytes);
            for (int i = 0; i < NumChildren; i++)
            {
                //System.IO.File.WriteAllBytes(@"sp2_8.bin", sprite.ToByteArray(i + 1));    // DEBUG
                ImportSp2(iso, sprite.ToByteArray(i + 1), i);
            }
        }

        internal void ImportSp2(Stream iso, string filename, int index)
        {
            ImportSp2(iso, File.ReadAllBytes(filename), index);
        }

        internal void ImportSp2(Stream iso, byte[] bytes, int index)
        {
            if (index >= NumChildren)
            {
                throw new IndexOutOfRangeException();
            }

            if (bytes.Length > sp2MaxLength)
            {
                throw new ArgumentOutOfRangeException("bytes", "SP2 file is too large");
            }

            var loc = location.SubSpriteLocations[index];
            if (Context == PatcherLib.Datatypes.Context.US_PSX)
            {
                PatcherLib.Iso.PsxIso.PatchPsxIso(
                    iso,
                    new PatcherLib.Datatypes.PatchedByteArray((PatcherLib.Iso.PsxIso.Sectors)loc.Sector, 0, bytes));
            }
            else if (Context == PatcherLib.Datatypes.Context.US_PSP)
            {
                PatcherLib.Iso.PspIso.ApplyPatch(
                    iso,
                    PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo(iso),
                    new PatcherLib.Datatypes.PatchedByteArray((PatcherLib.Iso.FFTPack.Files)loc.Sector, 0, bytes));
            }
            else
            {
                throw new InvalidOperationException();
            }
            CachedSprite = null;
        }

        internal CharacterSprite(PatcherLib.Datatypes.Context context, string name, SpriteAttributes attributes, SpriteLocation location) :
            base(context, name,
                context == PatcherLib.Datatypes.Context.US_PSP ? (PatcherLib.Iso.KnownPosition)new PatcherLib.Iso.PspIso.KnownPosition((PatcherLib.Iso.FFTPack.Files)location.Sector, 0, (int)location.Size) :
                                                                 (PatcherLib.Iso.KnownPosition)new PatcherLib.Iso.PsxIso.KnownPosition((PatcherLib.Iso.PsxIso.Sectors)location.Sector, 0, (int)location.Size))
        {
            this.location = location;
            this.attributes = attributes;
        }

        protected override AbstractSprite GetAbstractSpriteFromPspIso(System.IO.Stream iso, PatcherLib.Iso.PspIso.PspIsoInfo info, bool ignoreCache)
        {
            if (CachedSprite == null || ignoreCache)
            {
                if (Position.Length == 0)
                {
                    CachedSprite = null;
                    return CachedSprite;
                }

                IList<byte> bytes = Position.ReadIso(iso);

                System.Diagnostics.Debug.Assert(bytes.Count == this.Size);

                switch (SHP)
                {
                    case SpriteType.TYPE1:
                        CachedSprite = new TYPE1Sprite(bytes);
                        break;
                    case SpriteType.TYPE2:
                        CachedSprite = new TYPE2Sprite(bytes);
                        break;
                    case SpriteType.RUKA:
                        CachedSprite = new MonsterSprite(bytes);
                        break;
                    case SpriteType.MON:
                        byte[][] sp2Bytes = new byte[location.SubSpriteLocations.Count][];
                        if (location.SubSpriteLocations.Count > 0)
                        {
                            for (int i = 0; i < location.SubSpriteLocations.Count; i++)
                            {
                                sp2Bytes[i] = PatcherLib.Iso.PspIso.GetFile(
                                    iso,
                                    info,
                                    (PatcherLib.Iso.FFTPack.Files)location.SubSpriteLocations[i].Sector,
                                    0,
                                    (int)location.SubSpriteLocations[i].Size).ToArray();
                            }
                        }
                        CachedSprite = new MonsterSprite(bytes, sp2Bytes);
                        break;
                    case SpriteType.KANZEN:
                        CachedSprite = new KANZEN(bytes);
                        break;
                    case SpriteType.CYOKO:
                        CachedSprite = new CYOKO(bytes);
                        break;
                    case SpriteType.ARUTE:
                        CachedSprite = new ARUTE(bytes);
                        break;
                    case SpriteType.WEP1:
                    case SpriteType.WEP2:
                        CachedSprite = new WEPSprite(bytes);
                        break;
                    //case SpriteType.WEP3:
                    //    cachedSprite = new WEP3Sprite(bytes);
                    //    break;
                    case SpriteType.FOUR:
                        CachedSprite = new TYPE1Sprite(bytes);
                        break;
                    default:
                        CachedSprite = null;
                        break;
                }
            }

            return CachedSprite;
        }

        protected override AbstractSprite GetAbstractSpriteFromPsxIso(System.IO.Stream iso, bool ignoreCache)
        {
            if (CachedSprite == null || ignoreCache)
            {
                if (Position.Length == 0)
                {
                    CachedSprite = null;
                    return CachedSprite;
                }
                
                IList<byte> bytes = Position.ReadIso(iso);

                switch (SHP)
                {
                    case SpriteType.TYPE1:
                        CachedSprite = new TYPE1Sprite(bytes);
                        break;
                    case SpriteType.TYPE2:
                        CachedSprite = new TYPE2Sprite(bytes);
                        break;
                    case SpriteType.RUKA:
                        CachedSprite = new MonsterSprite(bytes);
                        break;
                    case SpriteType.MON:
                        byte[][] sp2Bytes = new byte[location.SubSpriteLocations.Count][];
                        if (location.SubSpriteLocations.Count > 0)
                        {
                            for (int i = 0; i < location.SubSpriteLocations.Count; i++)
                            {
                                sp2Bytes[i] = PatcherLib.Iso.PsxIso.ReadFile(
                                    iso,
                                    (PatcherLib.Iso.PsxIso.Sectors)location.SubSpriteLocations[i].Sector,
                                    0,
                                    (int)location.SubSpriteLocations[i].Size);
                            }
                        }
                        CachedSprite = new MonsterSprite(bytes, sp2Bytes);
                        break;
                    case SpriteType.KANZEN:
                        CachedSprite = new KANZEN(bytes);
                        break;
                    case SpriteType.CYOKO:
                        CachedSprite = new CYOKO(bytes);
                        break;
                    case SpriteType.ARUTE:
                        CachedSprite = new ARUTE(bytes);
                        break;
                    case SpriteType.WEP1:
                    case SpriteType.WEP2:
                        CachedSprite = new WEPSprite(bytes);
                        break;
                    case SpriteType.EFF1:
                        CachedSprite = new WEP3Sprite(bytes);
                        break;
                    case SpriteType.FOUR:
                        CachedSprite = new TYPE1Sprite(bytes);
                        break;
                    default:
                        CachedSprite = null;
                        break;
                }
            }

            return CachedSprite;
        }

    }
}
