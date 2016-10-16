using PatcherLib.Datatypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace FFTPatcher.SpriteEditor
{
    public class PalettedImage8bppSectioned : PalettedImage8bpp
    {
        public int NumBytesBetweenRows { get; set; }

        public PalettedImage8bppSectioned(
            string name, int width, int height, int numPalettes, Palette.ColorDepth depth, PatcherLib.Iso.KnownPosition imagePosition, PatcherLib.Iso.KnownPosition palettePosition, 
            bool ignoreAssert = false)
            : base(name, width, height, numPalettes, depth, imagePosition, palettePosition, ignoreAssert)
        {
        }

        public static PalettedImage8bpp ConstructFromXml(XmlNode node)
        {
            ImageInfo info = GetImageInfo(node);

            var palPos = GetPalettePositionFromImageNode(info.Sector, node);
            var pos = GetPositionFromImageNode(info.Sector, node);
            var depth = GetColorDepth(node);

            int numBytesBetweenRows = 0;
            XmlNode numBytesBetweenRowsNode = node.SelectSingleNode("NumBytesBetweenRows");
            if (numBytesBetweenRowsNode != null)
            {
                numBytesBetweenRows = int.Parse(numBytesBetweenRowsNode.InnerText);
            }

            PalettedImage8bppSectioned image = new PalettedImage8bppSectioned(info.Name, info.Width, info.Height, 1, depth, pos, palPos);
            image.PaletteCount = info.PaletteCount;
            image.DefaultPalette = info.DefaultPalette;
            image.CurrentPalette = info.CurrentPalette;
            image.OriginalFilename = info.OriginalFilename;
            image.Filesize = info.Filesize;
            image.Sector = info.Sector;
            image.NumBytesBetweenRows = numBytesBetweenRows;

            return image;
        }

        protected override byte[] GetIsoBytes(System.IO.Stream iso)
        {
            List<byte> bytes = new List<byte>(Height * Width);

            for (int rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                bytes.AddRange(position.AddOffset(rowIndex * NumBytesBetweenRows, 0).ReadIso(iso));
            }

            return bytes.ToArray();
        }

        protected override void PatchIsoBytes(System.IO.Stream iso, byte[] bytes, bool is4bpp)
        {
            int byteWidth = is4bpp ? (Width / 2) : Width;
            for (int rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                int startIndex = rowIndex * byteWidth;
                position.AddOffset(rowIndex * NumBytesBetweenRows, 0).PatchIso(iso, bytes.Sub(startIndex, startIndex + (byteWidth) - 1));
            }
        }
    }
}
