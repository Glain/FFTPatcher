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

        private bool _isPopulate = false;
        private bool _isPopulateSection = false;

        public CustomSectionEditor()
        {
            InitializeComponent();
        }

        public void Populate(IList<CustomSection> customSections)
        {
            _isPopulate = true;

            _customSections = customSections;
            _customSectionIndex = 0;
            _editorMode = CustomEntryEditor.EditorMode.Data;

            cmb_Section.Items.Clear();
            cmb_Section.Items.Add("Text");
            cmb_Section.Items.Add("Data");
            cmb_Section.SelectedIndex = _customSectionIndex;
            SetSectionIndex(_customSectionIndex);

            _isPopulate = false;
        }

        private void PopulateSection()
        {
            _isPopulateSection = true;

            if (_customSections[_customSectionIndex].CustomEntryList.Count > 0)
            {
                cmb_Entry.Items.Clear();
                cmb_Entry.Items.AddRange(_customSections[_customSectionIndex].CustomEntryList.ToArray());
                cmb_Entry.SelectedIndex = 0;
                SetEntryIndex(0);
            }
            else
            {
                entryEditor.Clear();
            }

            cmb_Entry.Visible = (_customSections[_customSectionIndex].CustomEntryList.Count > 1);

            _isPopulateSection = false;
        }

        private void SetSectionIndex(int index)
        {
            _customSectionIndex = index;
            _editorMode = (CustomEntryEditor.EditorMode)(index);
            PopulateSection();
        }

        private void SetEntryIndex(int index)
        {
            _customEntryIndex = index;
            entryEditor.Populate(_customSections[_customSectionIndex].CustomEntryList[index], _editorMode);
        }

        private void cmb_Section_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isPopulate)
                SetSectionIndex(cmb_Section.SelectedIndex);
        }

        private void cmb_Entry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isPopulateSection)
                SetEntryIndex(cmb_Entry.SelectedIndex);
        }
    }
}
