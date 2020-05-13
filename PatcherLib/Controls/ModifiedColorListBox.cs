using System;
using System.Text;
using System.Windows.Forms;

namespace PatcherLib.Controls
{
    public class ModifiedColorListBox : ColorListBox
    {
        private StringBuilder selectionCache = new StringBuilder();

        private bool _includePrefix = false;
        public bool IncludePrefix
        {
            get { return _includePrefix; }
            set { _includePrefix = value; }
        }

        protected override void OnEnter(EventArgs e)
        {
            // Reset selection cache when control receives focus
            selectionCache.Length = 0;
            base.OnEnter(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0102) // WM_CHAR
            {
                char key = (char)m.WParam;

                if (key == 27) // Escape
                {
                    // Reset cache if ESC pressed
                    selectionCache.Length = 0;
                }
                else if (key == 8) // Backspace
                {
                    // Allow backspace to delete from the cache
                    if (selectionCache.Length > 0)
                    {
                        selectionCache.Length--;
                        SelectFirstItemMatching(selectionCache.ToString());
                    }
                }
                else if ((Char.IsLetterOrDigit(key)) || (key == ' '))
                {
                    selectionCache.Append(Char.ToLower(key));
                    SelectFirstItemMatching(selectionCache.ToString());
                }

                // Do not call base.WndProc as we want to eat the message
                // so that the control does not respond to it.
                return;
            }

            base.WndProc(ref m);
        }

        private void SelectFirstItemMatching(string selectionText)
        {
            for (int index = 0; index < Items.Count; index++)
            {
                string itemText = Items[index].ToString().ToLower();

                if (string.IsNullOrEmpty(itemText))
                    continue;

                if (!_includePrefix)
                {
                    int newStartIndex = itemText.IndexOf(' ') + 1;
                    int newLength = itemText.Length - newStartIndex;
                    itemText = itemText.Substring(newStartIndex, newLength);
                }

                if (itemText.StartsWith(selectionText))
                {
                    if (index != SelectedIndex)
                        SelectedIndex = index;

                    break;
                }
            }
        }
    }
}
