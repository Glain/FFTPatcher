/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher
{
    public partial class PatchPSXForm : Form, IGeneratePatchList
    {
		#region Instance Variables (4) 

        private bool[] battlePatchable = new bool[Enum.GetValues( typeof( BATTLEPatchable ) ).Length];
        private byte[] SCEAP_DAT = new byte[20480];
        private Bitmap sceapPreview = new Bitmap( 320, 32 );
        private bool[] scusPatchable = new bool[Enum.GetValues( typeof( SCUSPatchable ) ).Length];

        public bool patchISO = true;        //used to tell wether patching to savestate or ISO

		#endregion Instance Variables 

		#region Public Properties (26) 

        public bool Abilities
        {
            get { return scusPatchable[(int)SCUSPatchable.Abilities]; }
            set { scusPatchable[(int)SCUSPatchable.Abilities] = value; }
        }

        public bool AbilityEffects
        {
            get { return battlePatchable[(int)BATTLEPatchable.AbilityEffects]; }
            set { battlePatchable[(int)BATTLEPatchable.AbilityEffects] = value; }
        }
        public bool AbilityAnimations
        {
            get { return battlePatchable[(int)BATTLEPatchable.AbilityAnimations]; }
            set { battlePatchable[(int)BATTLEPatchable.AbilityAnimations] = value; }
        }

        public bool ActionMenus
        {
            get { return scusPatchable[(int)SCUSPatchable.ActionMenus]; }
            set { scusPatchable[(int)SCUSPatchable.ActionMenus] = value; }
        }

        public string CustomSCEAPFileName
        {
            get { return sceapFileNameTextBox.Text; }
        }

        public bool[] ENTD { get { return new bool[] { ENTD1, ENTD2, ENTD3, ENTD4 }; } }

        public bool ENTD1 { get; private set; }

        public bool ENTD2 { get; private set; }

        public bool ENTD3 { get; private set; }

        public bool ENTD4 { get; private set; }

        public string FileName { get { return isoPathTextBox.Text; } }

        public bool InflictStatus
        {
            get { return scusPatchable[(int)SCUSPatchable.InflictStatus]; }
            set { scusPatchable[(int)SCUSPatchable.InflictStatus] = value; }
        }

        public bool ItemAttributes
        {
            get { return scusPatchable[(int)SCUSPatchable.ItemAttributes]; }
            set { scusPatchable[(int)SCUSPatchable.ItemAttributes] = value; }
        }

        public bool Items
        {
            get { return scusPatchable[(int)SCUSPatchable.Items]; }
            set { scusPatchable[(int)SCUSPatchable.Items] = value; }
        }

        public bool JobLevels
        {
            get { return scusPatchable[(int)SCUSPatchable.JobLevels]; }
            set { scusPatchable[(int)SCUSPatchable.JobLevels] = value; }
        }

        public bool Jobs
        {
            get { return scusPatchable[(int)SCUSPatchable.Jobs]; }
            set { scusPatchable[(int)SCUSPatchable.Jobs] = value; }
        }

        public bool MonsterSkills
        {
            get { return scusPatchable[(int)SCUSPatchable.MonsterSkills]; }
            set { scusPatchable[(int)SCUSPatchable.MonsterSkills] = value; }
        }

        public bool MoveFindItems
        {
            get { return battlePatchable[(int)BATTLEPatchable.MoveFindItems]; }
            set { battlePatchable[(int)BATTLEPatchable.MoveFindItems] = value; }
        }

        public IList<PatchedByteArray> OtherPatches
        {
            get
            {
                if ( SCEAP != CustomSCEAP.NoChange )
                {
                    return new PatchedByteArray[] { new PatchedByteArray( PatcherLib.Iso.PsxIso.Sectors.SCEAP_DAT, 0, SCEAP_DAT ) };
                }
                else
                {
                    return new PatchedByteArray[0];
                }
            }
        }

        public int PatchCount
        {
            get
            {
                int result = 0;
                ENTD.ForEach( b => result += b ? 1 : 0 );
                bool[] bb = new bool[] { RegenECC, Abilities, AbilityEffects, MoveFindItems,
                    Items, ItemAttributes, Jobs, JobLevels, Skillsets, MonsterSkills, ActionMenus,
                    StatusAttributes,InflictStatus,Poach, SCEAP != CustomSCEAP.NoChange, StoreInventory, 
                    AbilityAnimations, Propositions};
                bb.ForEach( b => result += b ? 1 : 0 );
                return result;
            }
        }

        public bool Poach
        {
            get { return scusPatchable[(int)SCUSPatchable.Poach]; }
            set { scusPatchable[(int)SCUSPatchable.Poach] = value; }
        }

        public bool RegenECC { get; private set; }

        public CustomSCEAP SCEAP
        {
            get
            {
                return
                    useCustomSceapRadioButton.Checked ? CustomSCEAP.Custom :
                    dontChangeSceapRadioButton.Checked ? CustomSCEAP.NoChange :
                                                         CustomSCEAP.Default;

            }
        }

        public bool Skillsets
        {
            get { return scusPatchable[(int)SCUSPatchable.Skillsets]; }
            set { scusPatchable[(int)SCUSPatchable.Skillsets] = value; }
        }

        public bool StatusAttributes
        {
            get { return scusPatchable[(int)SCUSPatchable.StatusAttributes]; }
            set { scusPatchable[(int)SCUSPatchable.StatusAttributes] = value; }
        }

        public string FilePath = "";

		#endregion Public Properties 

		#region Constructors (1) 

        public PatchPSXForm()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public DialogResult CustomShowDialog( IWin32Window owner )
        {
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.Abilities, FFTPatch.Abilities.HasChanged );
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.ActionMenus, FFTPatch.ActionMenus.HasChanged );
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.InflictStatus, FFTPatch.InflictStatuses.HasChanged );
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.ItemAttributes, FFTPatch.ItemAttributes.HasChanged );
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.Items, FFTPatch.Items.HasChanged );
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.JobLevels, FFTPatch.JobLevels.HasChanged );
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.Jobs, FFTPatch.Jobs.HasChanged );
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.MonsterSkills, FFTPatch.MonsterSkills.HasChanged );
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.Poach, FFTPatch.PoachProbabilities.HasChanged );
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.Skillsets, FFTPatch.SkillSets.HasChanged );
            scusCheckedListBox.SetItemChecked( (int)SCUSPatchable.StatusAttributes, FFTPatch.StatusAttributes.HasChanged );

            battleCheckedListBox.SetItemChecked((int)BATTLEPatchable.AbilityEffects, FFTPatch.Abilities.AllEffects.HasChanged);
            battleCheckedListBox.SetItemChecked((int)BATTLEPatchable.AbilityAnimations, FFTPatch.AbilityAnimations.HasChanged);
            battleCheckedListBox.SetItemChecked((int)BATTLEPatchable.MoveFindItems, FFTPatch.MoveFind.HasChanged);

            storeInventoryCheckBox.Checked = FFTPatch.StoreInventories.HasChanged;
            propositionsCheckBox.Checked = FFTPatch.Propositions.HasChanged;

            dontChangeSceapRadioButton.Checked = true;

            entd1CheckBox.Checked = FFTPatch.ENTDs.ENTDs[0].HasChanged;
            entd2CheckBox.Checked = FFTPatch.ENTDs.ENTDs[1].HasChanged;
            entd3CheckBox.Checked = FFTPatch.ENTDs.ENTDs[2].HasChanged;
            entd4CheckBox.Checked = FFTPatch.ENTDs.ENTDs[3].HasChanged;
            eccCheckBox.Enabled = false;
            eccCheckBox.Checked = true;

            UpdateNextEnabled();

            return ShowDialog( owner );
        }

		#endregion Public Methods 

		#region Private Methods (10) 

        private void BuildSCEAPPreview()
        {
            for ( int i = 0; i < 20480; i += 2 )
            {
                sceapPreview.SetPixel( 
                    ( i / 2 ) % 320, 
                    ( i / 2 ) / 320, 
                    BytesToColor( SCEAP_DAT[i], SCEAP_DAT[i + 1] ) );
            }
            pictureBox1.Image = sceapPreview;
            pictureBox1.Invalidate();
        }

        private static Color BytesToColor( byte first, byte second )
        {
            int b = (second & 0x7C) << 1;
            int g = (second & 0x03) << 6 | (first & 0xE0) >> 2;
            int r = (first & 0x1F) << 3;

            return Color.FromArgb( r, g, b );
        }

        private void checkedListBox1_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            CheckedListBox clb = (CheckedListBox)sender;
            if ( (string)clb.Tag == "SCUS_942.21" )
            {
                scusPatchable[e.Index] = e.NewValue == CheckState.Checked;
            }
            else if ( (string)clb.Tag == "BATTLE.BIN" )
            {
                battlePatchable[e.Index] = e.NewValue == CheckState.Checked;
            }

            UpdateNextEnabled();
        }

        private void entd2CheckBox_CheckedChanged( object sender, EventArgs e )
        {
            CheckBox box = (CheckBox)sender;
            Checkboxes cb = (Checkboxes)Enum.Parse( typeof( Checkboxes ), box.Tag as string );
            switch ( cb )
            {
                case Checkboxes.ENTD1:
                    ENTD1 = box.Checked;
                    break;
                case Checkboxes.ENTD2:
                    ENTD2 = box.Checked;
                    break;
                case Checkboxes.ENTD3:
                    ENTD3 = box.Checked;
                    break;
                case Checkboxes.ENTD4:
                    ENTD4 = box.Checked;
                    break;
                case Checkboxes.RegenECC:
                    RegenECC = box.Checked;
                    break;
                case Checkboxes.StoreInventory:
                    StoreInventory = box.Checked;
                    break;
                case Checkboxes.Propositions:
                    Propositions = box.Checked;
                    break;
                default:
                    break;
            }
            UpdateNextEnabled();
        }

        private void isoBrowseButton_Click( object sender, EventArgs e )
        {
            if (patchISO)
                patchIsoDialog.Filter = "ISO images (*.iso, *.bin, *.img)|*.iso;*.bin;*.img|All files|*.*";
            else
                patchIsoDialog.Filter = "PSV images (*.psv)|*.psv|All files|*.*";

            patchIsoDialog.FileName = isoPathTextBox.Text;
            while (
                patchIsoDialog.ShowDialog( this ) == DialogResult.OK &&
                !ValidateISO( patchIsoDialog.FileName ) )
                ;
            isoPathTextBox.Text = patchIsoDialog.FileName;
            FilePath = patchIsoDialog.FileName;
            UpdateNextEnabled();
        }

        private void sceapBrowseButton_Click( object sender, EventArgs e )
        {
            sceapOpenFileDialog.FileName = sceapFileNameTextBox.Text;
            while (
                sceapOpenFileDialog.ShowDialog( this ) == DialogResult.OK &&
                !ValidateSCEAP( sceapOpenFileDialog.FileName ) )
                ;
            sceapFileNameTextBox.Text = sceapOpenFileDialog.FileName;

            UpdateNextEnabled();
        }

        private void sceapRadioButton_CheckedChanged( object sender, EventArgs e )
        {
            if ( sender == useCustomSceapRadioButton && useCustomSceapRadioButton.Checked )
            {
                sceapFileNameTextBox.Enabled = true;
                sceapBrowseButton.Enabled = true;
                if ( !ValidateSCEAP( sceapFileNameTextBox.Text ) )
                {
                    sceapBrowseButton_Click( sceapBrowseButton, EventArgs.Empty );
                }

                if ( ValidateSCEAP( sceapOpenFileDialog.FileName ) )
                {
                    using ( FileStream stream = new FileStream( sceapFileNameTextBox.Text, FileMode.Open ) )
                    {
                        stream.Read( SCEAP_DAT, 0, 20480 );
                    }
                    BuildSCEAPPreview();
                }
            }
            else if ( sender == dontChangeSceapRadioButton && dontChangeSceapRadioButton.Checked )
            {
                sceapFileNameTextBox.Enabled = false;
                sceapBrowseButton.Enabled = false;
                SCEAP_DAT = new byte[20480];
                BuildSCEAPPreview();
            }
            else if ( sender == useDefaultSceapRadioButton && useDefaultSceapRadioButton.Checked )
            {
                sceapFileNameTextBox.Enabled = false;
                sceapBrowseButton.Enabled = false;
                PSXResources.Binaries.SCEAPDAT.CopyTo( SCEAP_DAT, 0 );
                BuildSCEAPPreview();
            }

            UpdateNextEnabled();
        }

private void UpdateNextEnabled()
        {
            bool enabled = true;
            enabled = enabled &&
                ( !useCustomSceapRadioButton.Checked ||
                 ValidateSCEAP( sceapFileNameTextBox.Text ) );
            enabled = enabled && ValidateISO( isoPathTextBox.Text );
            enabled = enabled &&
                (ENTD1 || ENTD2 || ENTD3 || ENTD4 || RegenECC || Abilities || Items ||
                  ItemAttributes || Jobs || JobLevels || Skillsets || MonsterSkills || ActionMenus ||
                  StatusAttributes || InflictStatus || Poach || (SCEAP != CustomSCEAP.NoChange) ||
                  AbilityEffects || MoveFindItems || StoreInventory || AbilityAnimations || Propositions);

            okButton.Enabled = enabled;
        }

        private bool ValidateISO( string filename )
        {
            return
                filename != string.Empty &&
                File.Exists( filename );
        }

        private bool ValidateSCEAP( string filename )
        {
            return 
                filename != string.Empty &&
                File.Exists( filename ) &&
                new FileInfo( filename ).Length == 20480;
        }

		#endregion Private Methods 

        public enum CustomSCEAP
        {
            NoChange,
            Default,
            Custom
        }
        private enum Checkboxes
        {
            ENTD1,
            ENTD2,
            ENTD3,
            ENTD4,
            RegenECC,
            StoreInventory,
            Propositions
        }
        private enum SCUSPatchable
        {
            Abilities,
            Items,
            ItemAttributes,
            Jobs,
            JobLevels,
            Skillsets,
            MonsterSkills,
            ActionMenus,
            StatusAttributes,
            InflictStatus,
            Poach
        }
        private enum BATTLEPatchable
        {
            AbilityEffects,
            AbilityAnimations,
            MoveFindItems
        }

        #region IGeneratePatchList Members


        public bool StoreInventory
        {
            get; private set;
        }

        public bool Propositions { get; private set; }

        #endregion
    }
}
