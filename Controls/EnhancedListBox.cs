using FFTPatcher.Datatypes;
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
            SetChangedColors<IChangeable>();
        }

        public void SetChangedColors<T>() where T : IChangeable
        {
            if (Enabled)
            {
                for (int index = 0; index < Items.Count; index++)
                {
                    SetChangedColor<T>(index);
                }
            }
        }

        public void SetChangedColor()
        {
            SetChangedColor<IChangeable>();
        }

        public void SetChangedColor<T>() where T : IChangeable
        {
            SetChangedColor<T>(SelectedIndex);
        }

        public void SetChangedColor(int index)
        {
            SetChangedColor<IChangeable>(index);
        }

        public void SetChangedColor<T>(int index) where T : IChangeable
        {
            if ((Enabled) && (index >= 0) && (index < Items.Count))
            {
                T item = (T)Items[index];

                bool useDuplicateColor = false;
                bool useUnreferencedColor = false;
                if (item is ICheckDuplicate<T>)
                {
                    ICheckDuplicate<T> checkItem = (ICheckDuplicate<T>)item;
                    useUnreferencedColor = ((Settings.UnreferencedColor.UseColor) && (!checkItem.IsInUse));
                    useDuplicateColor = ((Settings.DuplicateColor.UseColor) && (checkItem.IsInUse) && (checkItem.IsDuplicate));
                }

                if (useDuplicateColor)
                {
                    SetColor(index, Settings.DuplicateColor.ForegroundColor, Settings.DuplicateColor.BackgroundColor);
                }
                else if (useUnreferencedColor)
                {
                    SetColor(index, Settings.UnreferencedColor.ForegroundColor, Settings.UnreferencedColor.BackgroundColor);
                }
                else if ((item.HasChanged) && (Settings.ModifiedColor.UseColor))
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
