using System.Drawing.Imaging;

namespace FFTPatcher.SpriteEditor.Helpers
{
    internal static class ImageFormatHelper
    {
        internal static string GetExtension(ImageFormat format)
        {
            if (format == ImageFormat.Png)
                return "png";
            else
                return "bmp";
        }
        
        internal static ImageFormat GetFormat(string extension)
        {
            extension = extension.ToLower().Trim();

            if (extension == "png")
                return ImageFormat.Png;
            else
                return ImageFormat.Bmp;
        }
    }
}
