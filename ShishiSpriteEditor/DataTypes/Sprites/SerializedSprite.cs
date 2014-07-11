using System.Collections.Generic;
using PatcherLib.Utilities;
namespace FFTPatcher.SpriteEditor
{
    internal class SerializedSprite
    {
        private IList<byte> pixels;
        private IList<byte> palettes; 

        public string Name { get; private set; }
        public IList<string> Filenames { get; private set; }
        public IList<byte> Pixels { get { return pixels.AsReadOnly(); } }
        public IList<byte> Palettes { get { return palettes.AsReadOnly(); } }
        public int OriginalSize { get; private set; }

        internal SerializedSprite( string name, int originalSize, IList<string> filenames, IList<byte> pixels, IList<byte> palettes )
        {
            Name = name;
            Filenames = filenames;
            OriginalSize = originalSize;
            this.pixels = pixels;
            this.palettes = palettes;
        }
    }
}
