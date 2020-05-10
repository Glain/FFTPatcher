using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PatcherLib.Controls
{
    public partial class BGCheckedListBox : CheckedListBox
    {
        private Color[] backColors;
        public Color[] BackColors
        {
            get { return backColors; }
            set { backColors = value; }
        }

        public BGCheckedListBox() { }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Color backColor = ((backColors != null) && (e.Index >= 0) && (e.Index < backColors.Length)) ? backColors[e.Index] : BackColor;

            DrawItemEventArgs e2 =
                new DrawItemEventArgs
                (
                    e.Graphics,
                    e.Font,
                    new Rectangle(e.Bounds.Location, e.Bounds.Size),
                    e.Index,
                    e.State,
                    e.ForeColor,
                    backColor
                );

            base.OnDrawItem(e2);
        }
    }
}
