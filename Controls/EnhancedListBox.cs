using PatcherLib.Datatypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FFTPatcher.Controls
{
    public class EnhancedListBox : PatcherLib.Controls.ModifiedColorListBox
    {
        public void SetChangedColors()
        {
            if (Enabled)
            {
                for (int index = 0; index < Items.Count; index++)
                {
                    SetChangedColor(index);
                }
            }
        }

        public void SetChangedColor()
        {
            SetChangedColor(SelectedIndex);
        }

        public void SetChangedColor(int index)
        {
            if ((Enabled) && (index >= 0) && (index < Items.Count))
            {
                IChangeable item = (IChangeable)Items[index];

                if (item.HasChanged)
                {
                    SetColor(index, Settings.ModifiedColor.ForegroundColor, Settings.ModifiedColor.BackgroundColor);
                }
                else
                {
                    SetDefaultColor(index);
                }
            }
        }
    }
}
