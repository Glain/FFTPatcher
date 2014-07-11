using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text;

namespace ImageQuantization
{
    public class ThumbnailGenerator
    {
        public static Bitmap GetThumbnail(int width, int height, string filter, string caption, ImageFormat format, Image b)
        {
            Bitmap source = new Bitmap(b);
            source = new Bitmap(source.GetThumbnailImage(width, height, null, IntPtr.Zero));
            if (format == ImageFormat.Gif)
            {
                source = new OctreeQuantizer(0xff, 8).Quantize(source);
            }
            if ((filter.Length > 0) && filter.ToUpper().StartsWith("SHARPEN"))
            {
                string str = filter.Remove(0, 7).Trim();
                int nWeight = (str.Length > 0) ? Convert.ToInt32(str) : 11;
                BitmapFilter.Sharpen(source, nWeight);
            }
            if (caption.Length <= 0)
            {
                return source;
            }
            using (Graphics graphics = Graphics.FromImage(source))
            {
                StringFormat format2 = new StringFormat();
                format2.Alignment = StringAlignment.Center;
                format2.LineAlignment = StringAlignment.Center;
                using (Font font = new Font("Arial", 12f))
                {
                    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; 
                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 0, source.Height - 20, source.Width, 20);
                    graphics.DrawString(caption, font, Brushes.White, 0f, (float)(source.Height - 20));
                    return source;
                }
            }
        }
    }
}
