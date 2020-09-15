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

        public void SaveEntry()
        {
            entryEditor.SaveEntry();
        }

        private void PopulateSection(int index = 0)
        {
            _isPopulateSection = true;

            if (_customSections[_customSectionIndex].CustomEntryList.Count > 0)
            {
                SetEntryComboBoxEntries();
                cmb_Entry.SelectedIndex = index;
                SetEntryIndex(index);
            }
            else
            {
                ClearEntry();
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
            btn_Delete.Enabled = true;
        }

        private void ClearEntry()
        {
            _customEntryIndex = -1;
            cmb_Entry.Visible = false;
            btn_Delete.Enabled = false;
            cmb_Entry.Items.Clear();
            entryEditor.Clear();
        }

        private void SetEntryComboBoxEntries()
        {
            cmb_Entry.Items.Clear();
            cmb_Entry.Items.AddRange(_customSections[_customSectionIndex].CustomEntryList.ToArray());
        }

        private void SetEntryComboBoxIndex(int index, bool triggerEventHandler = false)
        {
            bool oldIsPopulateSection = _isPopulateSection;
            _isPopulateSection = !triggerEventHandler;
            cmb_Entry.SelectedIndex = index;
            _isPopulateSection = oldIsPopulateSection;
        }

        private void cmb_Section_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isPopulate)
            {
                entryEditor.SaveEntry();
                SetSectionIndex(cmb_Section.SelectedIndex);
            }
        }

        private void cmb_Entry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isPopulateSection)
            {
                entryEditor.SaveEntry();
                SetEntryIndex(cmb_Entry.SelectedIndex);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            List<CustomEntry> entryList = _customSections[_customSectionIndex].CustomEntryList;
            entryList.Remove(entryEditor.CustomEntry);

            if (entryList.Count > 0)
            {
                bool isFirstIndex = (_customEntryIndex > 0);
                int newIndex = isFirstIndex ? (_customEntryIndex - 1) : 0;
                int startIndex = isFirstIndex ? (newIndex + 1) : 0;

                for (int index = startIndex; index < entryList.Count; index++)
                    entryList[index].DecrementIndex();

                PopulateSection(newIndex);
                /*
                SetEntryComboBoxEntries();
                SetEntryComboBoxIndex(newIndex, false);
                SetEntryIndex(newIndex);
                cmb_Entry.Visible = (entryList.Count > 1);
                */
            }
            else
            {
                ClearEntry();
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            entryEditor.SaveEntry();

            List<CustomEntry> entryList = _customSections[_customSectionIndex].CustomEntryList;
            int newIndex = _customEntryIndex + 1;
            entryList.Insert(newIndex, new CustomEntry(newIndex));

            for (int index = newIndex + 1; index < entryList.Count; index++)
                entryList[index].IncrementIndex();

            PopulateSection(newIndex);
            /*
            SetEntryComboBoxEntries();
            SetEntryComboBoxIndex(newIndex, false);
            SetEntryIndex(newIndex);
            cmb_Entry.Visible = (entryList.Count > 1);
            */
        }
    }
}
