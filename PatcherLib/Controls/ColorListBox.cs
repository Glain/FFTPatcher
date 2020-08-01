using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PatcherLib.Controls
{
    public partial class ColorListBox : ListBox
    {
        private class ColorPair
        {
            public Color ForeColor { get; set; }
            public Color BackColor { get; set; }

            public ColorPair(Color foreColor, Color backColor)
            {
                this.ForeColor = foreColor;
                this.BackColor = backColor;
            }
        }

        private static Color colorForeSelected = Color.White;
        private static Color colorBackSelected = Color.FromKnownColor(KnownColor.Highlight);
        private static Color colorForeDefault = Color.Black;
        private static Color colorBackDefault = Color.White;

        private SolidBrush brushForeSelected = new SolidBrush(colorForeSelected);
        private SolidBrush brushBackSelected = new SolidBrush(colorBackSelected);
        //private SolidBrush brushFore = new SolidBrush(Color.Black);

        private ColorPair defaultColorPair = new ColorPair(colorForeDefault, colorBackDefault);

        private ColorPair[] colors;
        private ColorPair[] Colors
        {
            get 
            {
                if (colors == null)
                {
                    colors = new ColorPair[Items.Count];
                    for (int index = 0; index < Items.Count; index++)
                        colors[index] = defaultColorPair;
                }
                else if (colors.Length < Items.Count)
                {
                    SetColorCapacity(Items.Count);
                }

                return colors; 
            }
        }

        public ColorListBox() 
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        public void SetColorCapacity(int numColors)
        {
            int colorCount = (colors == null) ? 0 : colors.Length;
            ColorPair[] newColors = new ColorPair[Items.Count];
            for (int index = 0; index < Items.Count; index++)
                newColors[index] = (index < colorCount) ? colors[index] : defaultColorPair;
            colors = newColors;
        }

        public void SetColor(Color foreColor, Color backColor)
        {
            SetColor(SelectedIndex, foreColor, backColor);
        }

        public void SetColor(int index, Color foreColor, Color backColor)
        {
            if (index >= 0)
                Colors[index] = new ColorPair(foreColor, backColor);
        }

        public void SetDefaultColor(int index)
        {
            if (index >= 0)
                Colors[index] = defaultColorPair;
        }

        public void SetForeColor(int index, Color foreColor)
        {
            SetColor(index, foreColor, Colors[index].BackColor);
        }

        public void SetBackColor(int index, Color backColor)
        {
            SetColor(index, Colors[index].ForeColor, backColor);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int index = e.Index;
            Graphics graphics = e.Graphics;
            ColorPair colorPair = ((colors != null) && (index < colors.Length) && (index >= 0)) ? colors[index] : new ColorPair(ForeColor, BackColor);

            bool isSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            e.DrawBackground();

            if (index >= 0 && index < Items.Count)
            {
                string text = Items[index].ToString();

                SolidBrush backgroundBrush = isSelected ? brushBackSelected : new SolidBrush(colorPair.BackColor);
                graphics.FillRectangle(backgroundBrush, e.Bounds);

                //SolidBrush foregroundBrush = isSelected ? brushForeSelected : new SolidBrush(colorPair.ForeColor);
                //graphics.DrawString(text, Font, foregroundBrush, GetItemRectangle(index).Location);
                TextRenderer.DrawText(e.Graphics, text, Font, GetItemRectangle(index).Location, (isSelected ? colorForeSelected : colorPair.ForeColor), TextFormatFlags.NoPrefix);
            }

            e.DrawFocusRectangle();
        }
    }
}
