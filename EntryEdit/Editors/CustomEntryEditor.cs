using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EntryEdit.Editors
{
    public partial class CustomEntryEditor : UserControl
    {
        public enum EditorMode
        {
            Text = 0,
            Data = 1
        }

        private CustomEntry _customEntry;
        private EditorMode _editorMode;

        public CustomEntryEditor()
        {
            InitializeComponent();
        }

        public void Populate(CustomEntry customEntry, EditorMode editorMode)
        {
            _customEntry = customEntry;
            _editorMode = editorMode;

            if (_editorMode == EditorMode.Data)
            {
                txt_Entry.Text = PatcherLib.Utilities.Utilities.ByteArrayToHexString(customEntry.Bytes.ToArray());
            }
            else if (_editorMode == EditorMode.Text)
            {
                txt_Entry.Text = customEntry.Text;
            }
        }

        public void Clear()
        {
            _customEntry = null;
            txt_Entry.Text = "";
        }
    }
}
