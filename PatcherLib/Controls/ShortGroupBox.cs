using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PatcherLib.Controls
{
    public class ShortGroupBox : GroupBox
    {
        const int HeightReductionAmount = 12;

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size size = base.GetPreferredSize(proposedSize);

            int reqNameWidth = TextRenderer.MeasureText(Text, Font).Width + 20;
            if (size.Width < reqNameWidth)
                size.Width = reqNameWidth;

            return new Size(size.Width, size.Height - HeightReductionAmount);
        }
    }
}
