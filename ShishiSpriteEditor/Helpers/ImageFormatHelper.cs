using System.Drawing;
using System.Drawing.Imaging;

namespace FFTPatcher.SpriteEditor.Helpers
{
    internal static class ImageFormatHelper
    {
        internal static string GetExtension(ImageFormat format)
        {
            if (format == ImageFormat.Png)
                return ".png";
            else
                return ".bmp";
        }
        
        internal static ImageFormat GetFormat(string extension)
        {
            if (extension == null)
                return ImageFormat.Bmp;

            extension = extension.ToLower().Trim();
            if (extension.StartsWith("."))
                extension = extension.Substring(1);

            if (extension == "png")
                return ImageFormat.Png;
            else
                return ImageFormat.Bmp;
        }

        internal static Bitmap GetImageFileBitmap(string filepath)
        {
            byte[] importBytes = System.IO.File.ReadAllBytes(filepath);
            string extension = System.IO.Path.GetExtension(filepath);
            ImageFormat format = ImageFormatHelper.GetFormat(extension);

            Bitmap image;
            if (format == ImageFormat.Png)
            {
                image = PNGHelper.LoadBitmap(importBytes);
            }
            else
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream(importBytes);
                image = new Bitmap(stream);
            }

            return image;
        }
    }
}
