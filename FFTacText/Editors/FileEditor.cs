using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PatcherLib.Utilities;

namespace FFTPatcher.TextEditor.Editors
{
    partial class FileEditor : UserControl
    {
        private IFile boundFile;
        private Dictionary<int, int> localToFileIndexMapping;

        bool ignoreChanges;
        public void BindTo( IFile file )
        {
            ignoreChanges = true;
            localToFileIndexMapping = new Dictionary<int, int>();

            IList<string> sectionNames = file.SectionNames;
            sectionComboBox.Items.Clear();
            int localIndex = 0;
            for ( int i = 0; i < sectionNames.Count; i++ )
            {
                if ( !file.HiddenEntries[i] )
                {
                    localToFileIndexMapping[localIndex++] = i;
                    sectionComboBox.Items.Add( sectionNames[i] );
                }
            }

            sectionComboBox.SelectedIndex = 0;
            sectionComboBox.Enabled = file.NumberOfSections > 1;
            sectionNotesTextbox.Text = file.SectionComments[localToFileIndexMapping[sectionComboBox.SelectedIndex]] ?? string.Empty;
            fileNotesTextBox.Text = file.FileComments ?? string.Empty;

            stringListEditor1.BindTo( file.EntryNames[localToFileIndexMapping[0]], file, localToFileIndexMapping[0] );
            boundFile = file;
            restoreButton.Visible = boundFile is ISerializableFile;
            commentsTable.Enabled = boundFile is ISerializableFile;
            ignoreChanges = false;
        }

        public void RefreshText()
        {
            BindTo(boundFile);
        }

        public FileEditor()
        {
            InitializeComponent();
            sectionComboBox.SelectedIndexChanged += new EventHandler( sectionComboBox_SelectedIndexChanged );
            stringListEditor1.CellValidating += new DataGridViewCellValidatingEventHandler( stringListEditor1_CellValidating );
        }

        private void stringListEditor1_CellValidating( object sender, DataGridViewCellValidatingEventArgs e )
        {
            e.Cancel = !boundFile.CharMap.ValidateString( e.FormattedValue.ToString(), boundFile.SelectedTerminator );
            if ( e.Cancel )
            {
                errorLabel.Text = boundFile.CharMap.LastError;
                errorLabel.Visible = true;
            }
            else
            {
                errorLabel.Visible = false;
            }
        }

        private void sectionComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( !ignoreChanges && boundFile != null )
            {
                stringListEditor1.BindTo( 
                    boundFile.EntryNames[localToFileIndexMapping[sectionComboBox.SelectedIndex]], 
                    boundFile, 
                    localToFileIndexMapping[sectionComboBox.SelectedIndex] );
                sectionNotesTextbox.Text = boundFile.SectionComments[localToFileIndexMapping[sectionComboBox.SelectedIndex]] ?? string.Empty;
            }
        }

        private void restoreButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ISO files (*.bin, *.iso, *.img)|*.bin;*.iso;*.img";
            openFileDialog1.FileName = string.Empty;
            if (openFileDialog1.ShowDialog(this.TopLevelControl as Form) == DialogResult.OK)
            {
                using (System.IO.Stream stream = System.IO.File.OpenRead(openFileDialog1.FileName))
                {
                    (boundFile as AbstractFile).RestoreFile(stream);
                }
                BindTo(boundFile);
                //AbstractFile.ConstructFile(f.Layout.FileType, f.CharMap, f.Layout, 
            }
        }

        private void fileNotesTextBox_TextChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                boundFile.FileComments = fileNotesTextBox.Text;
            }
        }

        private void sectionNotesTextbox_TextChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                boundFile.SectionComments[localToFileIndexMapping[sectionComboBox.SelectedIndex]] = 
                    sectionNotesTextbox.Text;
            }
        }
    }
}
