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
    public partial class CustomSectionEditor : UserControl
    {
        private IList<CustomSection> _customSections;
        private CustomEntryEditor.EditorMode _editorMode;
        private int _customSectionIndex = 0;
        private int _customEntryIndex = -1;

        public CustomSectionEditor()
        {
            InitializeComponent();
        }

        public void Populate(IList<CustomSection> customSections)
        {
            _customSections = customSections;
            _customSectionIndex = 0;
            _editorMode = CustomEntryEditor.EditorMode.Data;

            cmb_Section.Items.Clear();
            cmb_Section.Items.Add("Text");
            cmb_Section.Items.Add("Data");
            cmb_Section.SelectedIndex = _customSectionIndex;
        }

        private void PopulateSection()
        {
            if (_customSections[_customSectionIndex].CustomEntryList.Count > 0)
            {
                cmb_Entry.Items.Clear();
                cmb_Entry.Items.AddRange(_customSections[_customSectionIndex].CustomEntryList.ToArray());
                cmb_Entry.SelectedIndex = 0;
            }
            else
            {
                entryEditor.Clear();
            }

            cmb_Entry.Visible = (_customSections[_customSectionIndex].CustomEntryList.Count > 1);
        }

        private void cmb_Section_SelectedIndexChanged(object sender, EventArgs e)
        {
            _customSectionIndex = cmb_Section.SelectedIndex;
            _editorMode = (CustomEntryEditor.EditorMode)(_customSectionIndex);
            PopulateSection();
        }

        private void cmb_Entry_SelectedIndexChanged(object sender, EventArgs e)
        {
            _customEntryIndex = cmb_Entry.SelectedIndex;
            entryEditor.Populate(_customSections[_customSectionIndex].CustomEntryList[_customEntryIndex], _editorMode);
        }
    }
}
