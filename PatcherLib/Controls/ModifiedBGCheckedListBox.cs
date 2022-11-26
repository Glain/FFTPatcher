using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PatcherLib.Controls
{
    public class ModifiedBGCheckedListBox : BGCheckedListBox
    {
        private StringBuilder selectionCache = new StringBuilder();

        private bool _includePrefix = false;
        public bool IncludePrefix
        {
            get { return _includePrefix; }
            set { _includePrefix = value; }
        }

        private bool AuthorizeCheck { get; set; }
        private bool AuthorizeSingleCheck { get; set; }

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
                bool isHandled = false;
                char key = (char)m.WParam;

                if (key == 27) // Escape
                {
                    // Reset cache if ESC pressed
                    selectionCache.Length = 0;
                    isHandled = true;
                }
                else if (key == 8) // Backspace
                {
                    // Allow backspace to delete from the cache
                    if (selectionCache.Length > 0)
                    {
                        selectionCache.Length--;
                        SelectFirstItemMatching(selectionCache.ToString());
                    }
                    isHandled = true;
                }
                else if ((Char.IsLetterOrDigit(key)) || ((key == ' ') && (selectionCache.Length > 0)))
                {
                    selectionCache.Append(Char.ToLower(key));
                    SelectFirstItemMatching(selectionCache.ToString());
                    isHandled = true;
                }
                else if (key == ' ')
                {
                    AuthorizeSingleCheck = true;
                }

                if (isHandled)
                {
                    // Do not call base.WndProc as we want to eat the message
                    // so that the control does not respond to it.
                    return;
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnItemCheck(ItemCheckEventArgs e)
        {
            if (AuthorizeCheck)
            {
                base.OnItemCheck(e);
            }
            else if (AuthorizeSingleCheck)
            {
                AuthorizeSingleCheck = false;
                base.OnItemCheck(e);
            }
            else
            {
                e.NewValue = e.CurrentValue;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Point loc = PointToClient(Cursor.Position);
            for (int i = 0; i < Items.Count; i++)
            {
                Rectangle rec = GetItemRectangle(i);
                rec.X = 1;
                rec.Width = 13;

                if (rec.Contains(loc))
                {
                    AuthorizeCheck = true;
                    SetItemChecked(i, !GetItemChecked(i));
                    AuthorizeCheck = false;
                    break;
                }
            }
        }

        public void ForceSetItemChecked(int index, bool isChecked)
        {
            AuthorizeCheck = true;
            SetItemChecked(index, isChecked);
            AuthorizeCheck = false;
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
