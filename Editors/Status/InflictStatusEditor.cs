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

using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib;
using System;
using System.Collections.Generic;

namespace FFTPatcher.Editors
{
    public partial class InflictStatusEditor : BaseEditor
    {
		#region Instance Variables (3) 

        private readonly string[] flags = new string[] { 
            "AllOrNothing", "Random", "Separate", "Cancel", 
            "Blank1", "Blank2", "Blank3", "Blank4" };
        private bool ignoreChanges = false;
        private InflictStatus status;
        private PatcherLib.Datatypes.Context ourContext = PatcherLib.Datatypes.Context.Default;

		#endregion Instance Variables 

		#region Public Properties (1) 

        public InflictStatus InflictStatus
        {
            get { return status; }
            set
            {
                SetInflictStatus(value, ourContext);
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public InflictStatusEditor()
        {
            InitializeComponent();
            flagsCheckedListBox.ItemCheck += flagsCheckedListBox_ItemCheck;
            inflictStatusesEditor.DataChanged += OnDataChanged;

            lbl_AbilityUsage_2.Click += lbl_AbilityUsage_2_Click;
            lbl_AbilityUsage_4.Click += lbl_AbilityUsage_4_Click;
            lbl_ItemUsage_2.Click += lbl_ItemUsage_2_Click;
            lbl_ItemUsage_4.Click += lbl_ItemUsage_4_Click;
        }

		#endregion Constructors 

		#region Methods 

        private void flagsCheckedListBox_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            if( !ignoreChanges )
            {
                ReflectionHelpers.SetFlag( status, flags[e.Index], e.NewValue == CheckState.Checked );
                OnDataChanged( this, System.EventArgs.Empty );
            }
        }

        private void spinner_Repoint_ValueChanged(object sender, System.EventArgs e)
        {
            if( !ignoreChanges )
            {
                btnRepoint.Enabled = (spinner_Repoint.Value != status.Index);
            }
        }

        public void SetInflictStatus(InflictStatus value, PatcherLib.Datatypes.Context context)
        {
            if (value == null)
            {
                status = null;
                this.Enabled = false;
            }
            else if (value != status)
            {
                status = value;
                this.Enabled = true;
                UpdateView(context);
            }
        }

        public void UpdateView(PatcherLib.Datatypes.Context context)
        {
            ignoreChanges = true;
            SuspendLayout();
            flagsCheckedListBox.SuspendLayout();
            inflictStatusesEditor.SuspendLayout();

            ourContext = context;

            if (status.Default != null)
            {
                flagsCheckedListBox.SetValuesAndDefaults(ReflectionHelpers.GetFieldsOrProperties<bool>(status, flags), status.Default.ToBoolArray());
            }

            if (status.IsInUse)
            {
                btnRepoint.Enabled = true;
                spinner_Repoint.Enabled = true;
            }
            else
            {
                btnRepoint.Enabled = false;
                spinner_Repoint.Enabled = false;
            }

            if (status.IsDuplicate)
            {
                spinner_Repoint.Value = status.DuplicateIndex;
            }
            else
            {
                spinner_Repoint.Value = status.Index;
                btnRepoint.Enabled = false;
            }

            spinner_Repoint.Maximum = 0x7f;

            int abilityReferenceCount = status.ReferencingAbilityIDs.Count;
            bool isAbilityUsagePanelVisible = (abilityReferenceCount > 0);
            pnl_AbilityUsage.Visible = isAbilityUsagePanelVisible;
            if (isAbilityUsagePanelVisible)
            {
                lbl_AbilityUsage_2.Text = abilityReferenceCount.ToString();
                lbl_AbilityUsage_3.Text = (abilityReferenceCount == 0) ? "abilities" : ((abilityReferenceCount == 1) ? "ability: " : "abilities, e.g. ");

                int abilityIndex = GetFirstReferencingAbilityIndex();
                lbl_AbilityUsage_4.Text = String.Format("{0:X2} {1}", abilityIndex, AllAbilities.GetNames(context)[abilityIndex]);
            }

            int itemReferenceCount = status.ReferencingItemIndexes.Count;
            bool isItemUsagePanelVisible = (itemReferenceCount > 0);
            pnl_ItemUsage.Visible = isItemUsagePanelVisible;
            if (isItemUsagePanelVisible)
            {
                lbl_ItemUsage_2.Text = itemReferenceCount.ToString();
                lbl_ItemUsage_3.Text = (itemReferenceCount == 0) ? "items" : ((itemReferenceCount == 1) ? "item: " : "items, e.g. ");

                int itemIndex = GetFirstReferencingItemIndex();
                int itemID = (itemIndex > 0xFD) ? (itemIndex + 2) : itemIndex;
                lbl_ItemUsage_4.Text = String.Format("{0:X2} {1}", itemID, Item.GetItemNames(context)[itemID]);
            }

            //inflictStatusesEditor.Statuses = status.Statuses;
            inflictStatusesEditor.SetStatuses(status.Statuses, context);
            inflictStatusesEditor.UpdateView(context);

            inflictStatusesEditor.ResumeLayout();
            flagsCheckedListBox.ResumeLayout();
            ResumeLayout();
            ignoreChanges = false;
        }

        public event System.EventHandler<RepointEventArgs> RepointHandler;
        protected void OnRepoint(object sender, RepointEventArgs e)
        {
            if (RepointHandler != null)
            {
                RepointHandler(this, e);
            }
        }
        private void btnRepoint_Click(object sender, System.EventArgs e)
        {
            RepointHandler(this, new RepointEventArgs(-1, (int)spinner_Repoint.Value));
        }

        private int GetFirstReferencingAbilityIndex()
        {
            List<int> referencingAbilityIndexList = new List<int>(status.ReferencingAbilityIDs);
            referencingAbilityIndexList.Sort();
            return referencingAbilityIndexList[0];
        }

        private int GetFirstReferencingItemIndex()
        {
            List<int> referencingItemIndexList = new List<int>(status.ReferencingItemIndexes);
            referencingItemIndexList.Sort();
            return referencingItemIndexList[0];
        }

        public event EventHandler<ReferenceEventArgs> AbilityClicked;
        private void lbl_AbilityUsage_2_Click(object sender, EventArgs e)
        {
            if (AbilityClicked != null)
            {
                AbilityClicked(this, new ReferenceEventArgs(GetFirstReferencingAbilityIndex(), status.ReferencingAbilityIDs));
            }
        }
        private void lbl_AbilityUsage_4_Click(object sender, EventArgs e)
        {
            if (AbilityClicked != null)
            {
                AbilityClicked(this, new ReferenceEventArgs(GetFirstReferencingAbilityIndex()));
            }
        }

        public event EventHandler<ReferenceEventArgs> ItemClicked;
        private void lbl_ItemUsage_2_Click(object sender, EventArgs e)
        {
            if (ItemClicked != null)
            {
                ItemClicked(this, new ReferenceEventArgs(GetFirstReferencingItemIndex(), status.ReferencingItemIndexes));
            }
        }
        private void lbl_ItemUsage_4_Click(object sender, EventArgs e)
        {
            if (ItemClicked != null)
            {
                ItemClicked(this, new ReferenceEventArgs(GetFirstReferencingItemIndex()));
            }
        }

        #endregion Methods
    }
}
