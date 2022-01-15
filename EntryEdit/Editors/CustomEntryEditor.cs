using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PatcherLib.Datatypes;

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
        public CustomEntry CustomEntry 
        {
            get
            {
                return _customEntry;
            }
        }

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

            txt_Entry.Enabled = true;
        }

        public void Clear()
        {
            _customEntry = null;
            txt_Entry.Text = "";
            txt_Entry.Enabled = false;
        }

        public void SaveEntry(Context context)
        {
            if (_customEntry != null)
            {
                if (_editorMode == CustomEntryEditor.EditorMode.Text)
                {
                    _customEntry.SetText(txt_Entry.Text, context);
                }
                else if (_editorMode == CustomEntryEditor.EditorMode.Data)
                {
                    _customEntry.SetHex(txt_Entry.Text);
                }
            }
        }
    }
}
