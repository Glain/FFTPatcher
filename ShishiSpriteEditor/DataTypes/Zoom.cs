using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace FFTPatcher.SpriteEditor.DataTypes
{
    public class Zoom
    {
        public int Multiplier { get; set; }
        public string DisplayString { get; set; }

        public Zoom(int multiplier, string displayString)
        {
            this.Multiplier = multiplier;
            this.DisplayString = displayString;
        }

        public override string ToString()
        {
            return DisplayString;
        }

        public System.Drawing.Bitmap GetZoomedBitmap(Bitmap image)
        {
            if (image == null)
                return null;

            int zoomWidth = image.Width * Multiplier;
            int zoomHeight = image.Height * Multiplier;
            Bitmap newImage = new Bitmap(zoomWidth, zoomHeight, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(newImage);
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.DrawImage(image, new Rectangle(0, 0, zoomWidth, zoomHeight));
            graphics.Dispose();
            return newImage;
        }
    }
}
