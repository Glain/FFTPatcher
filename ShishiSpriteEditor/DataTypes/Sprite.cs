using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using PatcherLib.Utilities;

namespace FFTPatcher.SpriteEditor
{
    public class SpriteTooLargeException : Exception
    {
        public int Size { get; private set; }
        public int MaxSize { get; private set; }

        public SpriteTooLargeException(int size, int maxSize)
        {
            this.Size = size;
            this.MaxSize = maxSize;
        }

        public override string Message
        {
            get
            {
                return string.Format("Sprite size is {0} bytes. Max size is {1} bytes.", Size, MaxSize);
            }
        }
    }

    [System.Diagnostics.DebuggerDisplay("SHP: {SHP}, SEQ: {SEQ}, Size: {Size}")]
    public abstract class Sprite
    {
        private string name;
        protected AbstractSprite CachedSprite { get; set; }

        //private UInt32 Sector { get { return location.Sector; } }
        public virtual UInt32 Size { get; private set; }

        protected PatcherLib.Iso.KnownPosition Position { get; set; }

        public PatcherLib.Datatypes.Context Context { get; private set; }

        internal void ImportBitmap( Stream iso, string filename )
        {
            using ( Stream s = File.OpenRead( filename ) )
            using ( System.Drawing.Bitmap b = new System.Drawing.Bitmap( s ) )
            {
                ImportBitmap( iso, b );
            }
        }

        internal virtual void ImportBitmap4bpp(Stream iso, string filename, int paletteIndex)
        {
            AbstractSprite sprite = GetAbstractSpriteFromIso(iso);
            byte[] importBytes = System.IO.File.ReadAllBytes(filename);
            const int totalPaletteBytes = 32 * 16;
            byte[] originalPaletteBytes = Position.AddOffset(0, totalPaletteBytes - Position.Length).ReadIso(iso);
            sprite.ImportBitmap4bpp(paletteIndex, importBytes, originalPaletteBytes);
            byte[] sprBytes = sprite.ToByteArray(0);

            byte[] newPaletteBytes = sprite.GetPaletteBytes(sprite.Palettes, originalPaletteBytes, Palette.ColorDepth._16bit).ToArray();
            Array.Copy(newPaletteBytes, sprBytes, totalPaletteBytes);

            if (sprBytes.Length > Size)
            {
                throw new SpriteTooLargeException(sprBytes.Length, (int)Size);
            }

            ImportSprite(iso, sprBytes);
        }

        internal virtual void ImportBitmap8bpp(Stream iso, string filename)
        {
            AbstractSprite sprite = GetAbstractSpriteFromIso(iso);
            byte[] importBytes = System.IO.File.ReadAllBytes(filename);
            const int totalPaletteBytes = 512;
            byte[] originalPaletteBytes = Position.AddOffset(0, totalPaletteBytes - Position.Length).ReadIso(iso);
            sprite.ImportBitmap8bpp(importBytes, originalPaletteBytes);
            byte[] sprBytes = sprite.ToByteArray(0);

            byte[] newPaletteBytes = sprite.GetPaletteBytes(sprite.Palettes, originalPaletteBytes, Palette.ColorDepth._16bit).ToArray();
            Array.Copy(newPaletteBytes, sprBytes, totalPaletteBytes);

            if (sprBytes.Length > Size)
            {
                throw new SpriteTooLargeException(sprBytes.Length, (int)Size);
            }

            ImportSprite(iso, sprBytes);
        }

        protected static int DeterminePercentageOfBlackPixels(System.Drawing.Bitmap bmp, System.Drawing.Rectangle rect)
        {
            System.Diagnostics.Debug.Assert(bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            System.Drawing.Imaging.BitmapData bmd = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            int xOffset = rect.X;
            int yOffset = rect.Y;
            int count = 0;
            for (int x = 0; x < rect.Width; x++)
            {
                for (int y = 0; y < rect.Height; y++)
                {
                    int pix = bmd.GetPixel(x, y);
                    if (pix == 0) count++;
                }
            }
            bmp.UnlockBits(bmd);
            return (count * 100 / (rect.Width * rect.Height));
        }

        internal virtual void ImportBitmap(Stream iso, System.Drawing.Bitmap bmp)
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
        }

        internal void ImportSprite( Stream iso, string filename )
        {
            ImportSprite( iso, File.ReadAllBytes( filename ) );
        }

        internal void ImportSprite( Stream iso, byte[] bytes )
        {
            if (bytes.Length > Size)
            {
                throw new SpriteTooLargeException(bytes.Length, (int)Size);
            }
            if (Context == PatcherLib.Datatypes.Context.US_PSX)
            {
                PatcherLib.Iso.PsxIso.PatchPsxIso(
                    iso,
                    Position.GetPatchedByteArray(bytes));
            }
            else if (Context == PatcherLib.Datatypes.Context.US_PSP)
            {
                PatcherLib.Iso.PspIso.ApplyPatch(
                    iso,
                    PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo(iso),
                    Position.GetPatchedByteArray(bytes));
            }
            else
            {
                throw new InvalidOperationException();
            }
            CachedSprite = null;
        }

        internal Sprite(PatcherLib.Datatypes.Context context, string name, PatcherLib.Iso.KnownPosition pos )
        {
            this.Context = context;
            this.name = name;
            Position = pos;
            Size = (uint)pos.Length;
        }

        public AbstractSprite GetAbstractSpriteFromIso(System.IO.Stream iso)
        {
            return GetAbstractSpriteFromIso(iso, false);
        }

        public AbstractSprite GetAbstractSpriteFromIso(System.IO.Stream iso, bool ignoreCache)
        {
            if (Context == PatcherLib.Datatypes.Context.US_PSX)
            {
                return GetAbstractSpriteFromPsxIso(iso, ignoreCache);
            }
            else if (Context == PatcherLib.Datatypes.Context.US_PSP)
            {
                return GetAbstractSpriteFromPspIso(iso, PatcherLib.Iso.PspIso.PspIsoInfo.GetPspIsoInfo(iso), ignoreCache);
            }
            else
            {
                return null;
            }
        }

        protected abstract AbstractSprite GetAbstractSpriteFromPspIso(System.IO.Stream iso, PatcherLib.Iso.PspIso.PspIsoInfo info, bool ignoreCache);

        protected abstract AbstractSprite GetAbstractSpriteFromPsxIso(System.IO.Stream iso, bool ignoreCache);

        private AbstractSprite GetAbstractSpriteFromPsxIso( System.IO.Stream iso )
        {
            return GetAbstractSpriteFromPsxIso( iso, false );
        }

        public override string ToString()
        {
            return name;
        }

        public string GetSaveFileName()
        {
            if (Position is PatcherLib.Iso.PsxIso.KnownPosition)
            {
                PatcherLib.Iso.PsxIso.KnownPosition pos = Position as PatcherLib.Iso.PsxIso.KnownPosition;
                return string.Format("{0}_{1}_{2}.bmp", pos.Sector, pos.StartLocation, pos.Length);
            }
            else if (Position is PatcherLib.Iso.PspIso.KnownPosition)
            {
                PatcherLib.Iso.PspIso.KnownPosition pos = Position as PatcherLib.Iso.PspIso.KnownPosition;
                return string.Format("{0}_{1}_{2}.bmp", pos.SectorEnum, pos.StartLocation, pos.Length);
            }
            else
            {
                return name;
            }
        }
    }
}
