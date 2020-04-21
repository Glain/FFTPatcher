using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PatcherLib.Controls
{
    public partial class BGListBox : ListBox
    {
        private SolidBrush brushForeSelected = new SolidBrush(Color.White);
        private SolidBrush brushFore = new SolidBrush(Color.Black);
        private SolidBrush brushBackSelected = new SolidBrush(Color.FromKnownColor(KnownColor.Highlight));

        private Color[] backColors;
        public Color[] BackColors
        {
            get 
            {
                if (backColors == null)
                {
                    backColors = new Color[Items.Count];
                    for (int index = 0; index < Items.Count; index++)
                        backColors[index] = Color.White;
                }
                else if (backColors.Length < Items.Count)
                {
                    int backColorCount = backColors.Length;
                    Color[] newBackColors = new Color[Items.Count];
                    for (int index = 0; index < Items.Count; index++)
                        backColors[index] = (index < backColorCount) ? backColors[index] : Color.White;
                }

                return backColors; 
            }
            set { backColors = value; }
        }

        public BGListBox() 
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Color backColor = ((backColors != null) && (e.Index < backColors.Length)) ? backColors[e.Index] : BackColor;

            bool isSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            int index = e.Index;

            e.DrawBackground();

            if (index >= 0 && index < Items.Count)
            {
                string text = Items[index].ToString();

                SolidBrush backgroundBrush = isSelected ? brushBackSelected : new SolidBrush(backColor);
                graphics.FillRectangle(backgroundBrush, e.Bounds);

                SolidBrush foregroundBrush = isSelected ? brushForeSelected : brushFore;
                graphics.DrawString(text, DefaultFont, foregroundBrush, GetItemRectangle(index).Location);
            }

            e.DrawFocusRectangle();
        }
    }
}
