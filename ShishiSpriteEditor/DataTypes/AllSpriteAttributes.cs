using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PatcherLib.Datatypes;

namespace FFTPatcher.SpriteEditor
{
    class AllSpriteAttributes
    {
        const int numPsxSprites = 154;
        const int numPspSprites=numPsxSprites+11;

        private static PatcherLib.Iso.PsxIso.KnownPosition psxPos =
            new PatcherLib.Iso.PsxIso.KnownPosition(PatcherLib.Iso.PsxIso.Sectors.BATTLE_BIN, 0x2d74c, numPsxSprites * 4);
        private static PatcherLib.Iso.PspIso.KnownPosition pspPos =
            new PatcherLib.Iso.PspIso.KnownPosition(PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x324288,
                numPspSprites * 4+4*4);
        private IList<SpriteAttributes> sprites;

        public int Count { get; private set; }

        public SpriteAttributes this[int i]
        {
            get
            {
                if (i >= Count)
                    throw new IndexOutOfRangeException(string.Format("index must be less than {0}", numPsxSprites));
                return sprites[i];
            }
        }

        public static AllSpriteAttributes FromPsxIso(Stream iso)
        {
            byte[] bytes = PatcherLib.Iso.PsxIso.ReadFile(iso, psxPos);
            AllSpriteAttributes result = new AllSpriteAttributes();
            IList<SpriteAttributes> sprites = new SpriteAttributes[numPsxSprites];
            for (int i = 0; i < numPsxSprites; i++)
            {
                sprites[i] = SpriteAttributes.BuildPsx(
                    new PatcherLib.Iso.PsxIso.KnownPosition(psxPos.Sector, psxPos.StartLocation + i * 4, 4),
                    bytes.Sub(i * 4, (i + 1) * 4 - 1));
            }
            result.sprites = sprites;
            result.Count = numPsxSprites;
            return result;
        }

        public static AllSpriteAttributes FromPspIso(Stream iso, PatcherLib.Iso.PspIso.PspIsoInfo info)
        {
            IList<byte> bytes = PatcherLib.Iso.PspIso.GetBlock(iso, info, pspPos);
            List<byte> realBytes = new List<byte>(bytes.Sub(0, numPsxSprites * 4 - 1));
            realBytes.AddRange(bytes.Sub(numPsxSprites * 4 + (4 * 4)));
            bytes = realBytes.AsReadOnly();
            AllSpriteAttributes result = new AllSpriteAttributes();
            IList<SpriteAttributes> sprites = new SpriteAttributes[numPspSprites];
            for (int i = 0; i < numPspSprites; i++)
            {
                sprites[i] = SpriteAttributes.BuildPsp(
                    new PatcherLib.Iso.PspIso.KnownPosition(pspPos.Sector.Value, pspPos.StartLocation + i * 4, 4),
                    info,
                    bytes.Sub(i * 4, (i + 1) * 4 - 1));

            }
            result.sprites = sprites;
            result.Count = numPspSprites;
            return result;
        }
    }
}
