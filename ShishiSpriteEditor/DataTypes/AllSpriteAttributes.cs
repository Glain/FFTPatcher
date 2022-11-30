using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PatcherLib.Datatypes;

namespace FFTPatcher.SpriteEditor
{
    class AllSpriteAttributes
    {
        const int numPsxSprites = 159;
        const int numPspSprites=numPsxSprites+11;

        private static PatcherLib.Iso.PsxIso.KnownPosition psxPos =
            new PatcherLib.Iso.PsxIso.KnownPosition(PatcherLib.Iso.PsxIso.Sectors.BATTLE_BIN, 0x2D748, numPsxSprites * 4);
        private static PatcherLib.Iso.PspIso.KnownPosition pspPos =
            new PatcherLib.Iso.PspIso.KnownPosition(PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x324284,
                numPspSprites * 4); //+4*4);
        private IList<SpriteAttributes> sprites;
        private Context context;

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
            result.context = Context.US_PSX;
            return result;
        }

        public static AllSpriteAttributes FromPspIso(Stream iso, PatcherLib.Iso.PspIso.PspIsoInfo info)
        {
            IList<byte> bytes = PatcherLib.Iso.PspIso.GetBlock(iso, info, pspPos);
            //List<byte> realBytes = new List<byte>(bytes.Sub(0, numPsxSprites * 4 - 1));
            //realBytes.AddRange(bytes.Sub(numPsxSprites * 4 + (4 * 4)));
            //bytes = realBytes.AsReadOnly();
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
            result.context = Context.US_PSP;
            return result;
        }

        public byte[] GetShishiPatchBytes()
        {
            int numSprites = (context == Context.US_PSX) ? numPsxSprites : ((context == Context.US_PSP) ? numPspSprites : 0);
            byte[] result = new byte[numSprites * 4];

            for (int index = 0; index < sprites.Count; index++)
            {
                Array.Copy(sprites[index].ToByteArray(), 0, result, index * 4, 4);
            }

            return result;
        }

        public void ApplyShishiPatchBytes(Stream iso, byte[] bytes)
        {
            for (int index = 0; index < sprites.Count; index++)
            {
                sprites[index].SetAttributes(bytes.SubLength(index * 4, 4));
            }

            if (context == Context.US_PSX)
            {
                PatcherLib.Iso.PsxIso.PatchPsxIso(iso, new PatchedByteArray(PatcherLib.Iso.PsxIso.Sectors.BATTLE_BIN, 0x2D748, bytes));
            }
            else if (context == Context.US_PSP)
            {
                PatcherLib.Iso.PspIso.PatchISO(iso, new List<PatchedByteArray>()
                {
                    new PatchedByteArray(PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_BOOT_BIN, 0x324284, bytes),
                    new PatchedByteArray(PatcherLib.Iso.PspIso.Sectors.PSP_GAME_SYSDIR_EBOOT_BIN, 0x324284, bytes)
                });
            }
        }
    }
}
