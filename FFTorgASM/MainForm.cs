using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using PatcherLib.Utilities;
using ASMEncoding;

namespace FFTorgASM
{
    public partial class MainForm : Form
    {
        AsmPatch[] patches;
        private bool ignoreChanges = true;

        ASMEncodingUtility asmUtility;

        public MainForm()
        {
            InitializeComponent();

            asmUtility = new ASMEncodingUtility(ASMEncodingMode.PSX);

            versionLabel.Text = string.Format( "v0.{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString() );
            XmlDocument doc = new XmlDocument();
            reloadButton_Click( reloadButton, EventArgs.Empty );

            if ( patches == null || patches.Length == 0 )
            {
                IList<AsmPatch> temp;
                if ( PatchXmlReader.TryGetPatches( FFTorgASM.Properties.Resources.DefaultHacks, Application.ExecutablePath, asmUtility, out temp ) )
                {
                    LoadPatches( temp );
                }
            }
            patchButton.Click += new EventHandler( patchButton_Click );
            reloadButton.Click += new EventHandler( reloadButton_Click );
            checkedListBox1.ItemCheck += new ItemCheckEventHandler( checkedListBox1_ItemCheck );
            patchButton.Enabled = false;
            checkedListBox1.SelectedIndexChanged += new EventHandler( checkedListBox1_SelectedIndexChanged );
            variableSpinner.ValueChanged += new EventHandler( variableSpinner_ValueChanged );
            variableComboBox.SelectedIndexChanged += new EventHandler( variableComboBox_SelectedIndexChanged );
        }

        void variableComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( !ignoreChanges )
            {
            	AsmPatch patch = ( checkedListBox1.SelectedItem as AsmPatch );
            	Byte[] byteArray = patch.Variables[variableComboBox.SelectedIndex].content.Value.GetBytes();
            	variableSpinner.Maximum = (decimal)Math.Pow(256,patch.Variables[variableComboBox.SelectedIndex].bytes) - 1;
            	variableSpinner.Value = patch.GetUnsignedByteArrayValue_LittleEndian(byteArray);
            }
        }

        void variableSpinner_ValueChanged( object sender, EventArgs e )
        {
        	AsmPatch patch = (checkedListBox1.SelectedItem as AsmPatch);
            if ( !ignoreChanges )
            {
            	UInt32 def = (UInt32)variableSpinner.Value;
                for (int i=0; i < patch.Variables[variableComboBox.SelectedIndex].bytes; i++)
                {
                	patch.Variables[variableComboBox.SelectedIndex].content.Value.GetBytes()[i] = (Byte)((def >> (i * 8)) & 0xff);
                }
            }
        }

        void checkedListBox1_SelectedIndexChanged( object sender, EventArgs e )
        {
            AsmPatch p = checkedListBox1.SelectedItem as AsmPatch;
            textBox1.Text = p.Description;
            if ( p.Variables.Count > 0 )
            {
                ignoreChanges = true;
                variableComboBox.Items.Clear();
                p.Variables.ForEach( varType => variableComboBox.Items.Add( varType.content.Key ) );
                variableComboBox.SelectedIndex = 0;
                
                //variableSpinner.Value = p.Variables[0].content.Value.GetBytes()[0];
                Byte[] byteArray = p.Variables[0].content.Value.GetBytes();
                variableSpinner.Maximum = (decimal)Math.Pow(256,p.Variables[0].bytes) - 1;
            	variableSpinner.Value = p.GetUnsignedByteArrayValue_LittleEndian(byteArray);
                
                ignoreChanges = false;
                variableSpinner.Visible = true;
                variableComboBox.Visible = true;
            }
            else
            {
                ignoreChanges = true;
                variableComboBox.Visible = false;
                variableSpinner.Visible = false;
            }
        }

        private void LoadPatches( IList<AsmPatch> patches )
        {
            this.patches = patches.ToArray();
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange( this.patches );
            patchButton.Enabled = false;
        }

        private void LoadFiles( IList<string> files )
        {
            List<AsmPatch> result = new List<AsmPatch>();
            foreach ( string file in files )
            {
                IList<AsmPatch> tryPatches;
                if ( PatchXmlReader.TryGetPatches( File.ReadAllText( file, Encoding.UTF8 ), file, asmUtility, out tryPatches ) )
                {
                    result.AddRange( tryPatches );
                }
            }
            LoadPatches( result );
        }

        void reloadButton_Click( object sender, EventArgs e )
        {
            string[] files = Directory.GetFiles( Application.StartupPath, "*.xml", SearchOption.TopDirectoryOnly );
            LoadFiles( files );
        }

        void checkedListBox1_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            if ( e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked &&
                !( checkedListBox1.Items[e.Index] as AsmPatch ).ValidatePatch() )
            {
                e.NewValue = CheckState.Unchecked;
            }

            patchButton.Enabled = ( checkedListBox1.CheckedItems.Count > 0 || e.NewValue == CheckState.Checked ) &&
                                  !( checkedListBox1.CheckedItems.Count == 1 && e.NewValue == CheckState.Unchecked );
        }

        void patchButton_Click( object sender, EventArgs e )
        {
            saveFileDialog1.Filter = "ISO files (*.bin, *.iso, *.img)|*.bin;*.iso;*.img";
            saveFileDialog1.FileName = string.Empty;
            if ( saveFileDialog1.ShowDialog( this ) == DialogResult.OK )
            {
                using ( Stream file = File.Open( saveFileDialog1.FileName, FileMode.Open, FileAccess.ReadWrite ) )
                {
                    foreach ( AsmPatch patch in checkedListBox1.CheckedItems )
                    {
                        PatcherLib.Iso.PsxIso.PatchPsxIso( file, patch );
                    }
                }
            }
        }

        private void PatchSaveStbutton_Click(object sender, EventArgs e)
        {
            //Patchbutton copy. Modify to patch byte array right to savestate.
            saveFileDialog1.Filter = "PSV files (*.psv)|*.psv;*";
            saveFileDialog1.FileName = string.Empty;
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                byte[] filecopy = File.ReadAllBytes(saveFileDialog1.FileName);
                using (BinaryReader b = new BinaryReader(File.Open(saveFileDialog1.FileName, FileMode.Open)))
                {
                    foreach (AsmPatch patch in checkedListBox1.CheckedItems)
                    {
                        PatcherLib.Iso.PsxIso.PatchPsxSaveState(b, patch, filecopy);
                    }
                }
            }
        }

        private void checkedListBox1_DragEnter( object sender, DragEventArgs e )
        {
            if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
            {
                string[] files = (string[])e.Data.GetData( DataFormats.FileDrop );
                if ( files.Length >= 1 && System.IO.File.Exists( files[0] ) )
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void checkedListBox1_DragDrop( object sender, DragEventArgs e )
        {
            if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
            {
                string[] paths = (string[])e.Data.GetData( DataFormats.FileDrop );
                var filesToLoad = paths.FindAll( s => System.IO.File.Exists( s ) );
                LoadFiles( filesToLoad );
            }
        }

        private void checkAllButton_Click( object sender, EventArgs e )
        {
            for ( int i = 0; i < checkedListBox1.Items.Count; i++ )
            {
                // never check a FileAsmPatch
                if ( !( checkedListBox1.Items[i] is FileAsmPatch ) )
                    checkedListBox1.SetItemChecked( i, true );
            }
        }

        private void toggleButton_Click( object sender, EventArgs e )
        {
            for ( int i = 0; i < checkedListBox1.Items.Count; i++ )
            {
                // never check a FileAsmPatch
                if ( !( checkedListBox1.Items[i] is FileAsmPatch ) || checkedListBox1.GetItemChecked( i ) )
                    checkedListBox1.SetItemChecked( i, !checkedListBox1.GetItemChecked( i ) );
            }
        }

    }
}
