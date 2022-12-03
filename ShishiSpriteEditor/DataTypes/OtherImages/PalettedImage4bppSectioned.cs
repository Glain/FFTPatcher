using PatcherLib.Utilities;
using PatcherLib.Datatypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;

namespace FFTPatcher.SpriteEditor
{
    public class PalettedImage4bppSectioned : PalettedImage4bpp
    {
        public int NumBytesBetweenRows { get; set; }

        public PalettedImage4bppSectioned(string name, int width, int height, PatcherLib.Iso.KnownPosition imagePosition, PatcherLib.Iso.KnownPosition palettePosition)
            : base(name, width, height, 1, imagePosition, palettePosition)
        {
        }

        public static new PalettedImage4bppSectioned ConstructFromXml(XmlNode node)
        {
            ImageInfo info = GetImageInfo(node);
            PatcherLib.Iso.KnownPosition palPos = GetPalettePositionFromImageNode(info.Sector, node);
            PatcherLib.Iso.KnownPosition pos = GetPositionFromImageNode(info.Sector, node);

            int numBytesBetweenRows = 0;
            XmlNode numBytesBetweenRowsNode = node.SelectSingleNode("NumBytesBetweenRows");
            if (numBytesBetweenRowsNode != null)
            {
                numBytesBetweenRows = int.Parse(numBytesBetweenRowsNode.InnerText);
            }

            PalettedImage4bppSectioned image = new PalettedImage4bppSectioned(info.Name, info.Width, info.Height, pos, palPos);
            image.PaletteCount = info.PaletteCount;
            image.DefaultPalette = info.DefaultPalette;
            image.CurrentPalette = info.CurrentPalette;
            image.OriginalFilename = info.OriginalFilename;
            image.Filesize = info.Filesize;
            image.Sector = info.Sector;
            image.NumBytesBetweenRows = numBytesBetweenRows;

            return image;
        }

        protected override PalettedImage8bpp Get8BitPalettedBitmap()
        {
            int numRemainingPalettes = PaletteCount - CurrentPalette;
            PatcherLib.Iso.KnownPosition newPalettePosition = PalettePosition.AddOffset(CurrentPalette * PalettePosition.Length, PalettePosition.Length * Math.Min((numRemainingPalettes - 1), 15));
            PalettedImage8bppSectioned image = new PalettedImage8bppSectioned(Name, Width, Height, NumPalettes, Depth, position, newPalettePosition, true);
            image.ImportFilename = ImportFilename;
            image.NumBytesBetweenRows = NumBytesBetweenRows;
            return image;
        }

        protected override byte[] GetIsoBytes(System.IO.Stream iso)
        {
            List<byte> bytes = new List<byte>(Height * Width / 2);

            for (int rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                bytes.AddRange(position.AddOffset(rowIndex * NumBytesBetweenRows, 0).ReadIso(iso));
            }

            return bytes.ToArray();
        }

        protected override void PatchIsoBytes(System.IO.Stream iso, byte[] bytes)
        {
            for (int rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                int startIndex = rowIndex * Width / 2;
                position.AddOffset(rowIndex * NumBytesBetweenRows, 0).PatchIso(iso, bytes.Sub(startIndex, startIndex + (Width / 2) - 1));
            }
        }
    }
}
