using System;
using System.Collections.Generic;
using System.Text;

namespace FFTPatcher.SpriteEditor
{
    public interface ISelectablePalette4bppImage
    {
        int CurrentPalette { get; set; }
        int PaletteCount { get; set; }
        bool ImportExport8bpp { get; set; }
    }
}
