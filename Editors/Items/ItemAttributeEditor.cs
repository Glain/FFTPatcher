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
using FFTPatcher.Controls;
using FFTPatcher.Datatypes;
using PatcherLib;
using System.Collections.Generic;

namespace FFTPatcher.Editors
{
    public partial class ItemAttributeEditor : BaseEditor
    {
		#region Instance Variables

        private PatcherLib.Datatypes.Context ourContext; 
        private ItemAttributes attributes;
        private bool ignoreChanges = false;
        private NumericUpDownWithDefault[] spinners;

		#endregion Instance Variables 

		#region Public Properties (1) 

        public ItemAttributes ItemAttributes
        {
            get { return attributes; }
            set
            {
                SetItemAttributes(value, ourContext);
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public ItemAttributeEditor()
        {
            InitializeComponent();
            spinners = new NumericUpDownWithDefault[] { maSpinner, paSpinner, speedSpinner, moveSpinner, jumpSpinner };
            foreach( NumericUpDownWithDefault spinner in spinners )
            {
                spinner.ValueChanged += spinner_ValueChanged;
            }

            statusImmunityEditor.DataChanged += OnDataChanged;
            startingStatusesEditor.DataChanged += OnDataChanged;
            permanentStatusesEditor.DataChanged += OnDataChanged;

            strongElementsEditor.DataChanged += OnDataChanged;
            weakElementsEditor.DataChanged += OnDataChanged;
            halfElementsEditor.DataChanged += OnDataChanged;
            absorbElementsEditor.DataChanged += OnDataChanged;
            cancelElementsEditor.DataChanged += OnDataChanged;

            lbl_Usage_2.Click += lbl_Usage_2_Click;
            lbl_Usage_4.Click += lbl_Usage_4_Click;
        }

		#endregion Constructors 

		#region Public Methods

        public void SetItemAttributes(ItemAttributes value, PatcherLib.Datatypes.Context context)
        {
            if (value == null)
            {
                this.Enabled = false;
                this.attributes = null;
            }
            else if (value != attributes)
            {
                attributes = value;
                UpdateView(context);
                this.Enabled = true;
            }
        }

        public void UpdateView(PatcherLib.Datatypes.Context context)
        {
            this.ignoreChanges = true;
            ourContext = context;

            SuspendLayout();
            statusImmunityEditor.SuspendLayout();
            startingStatusesEditor.SuspendLayout();
            permanentStatusesEditor.SuspendLayout();
            strongElementsEditor.SuspendLayout();
            weakElementsEditor.SuspendLayout();
            halfElementsEditor.SuspendLayout();
            absorbElementsEditor.SuspendLayout();
            cancelElementsEditor.SuspendLayout();

            foreach( NumericUpDownWithDefault spinner in spinners )
            {
                spinner.SetValueAndDefault(
                    ReflectionHelpers.GetFieldOrProperty<byte>( attributes, spinner.Tag.ToString() ),
                    ReflectionHelpers.GetFieldOrProperty<byte>( attributes.Default, spinner.Tag.ToString() ) );
            }

            statusImmunityEditor.Statuses = null;
            startingStatusesEditor.Statuses = null;
            permanentStatusesEditor.Statuses = null;
            //statusImmunityEditor.Statuses = attributes.StatusImmunity;
            //startingStatusesEditor.Statuses = attributes.StartingStatuses;
            //permanentStatusesEditor.Statuses = attributes.PermanentStatuses;
            statusImmunityEditor.SetStatuses(attributes.StatusImmunity, context);
            startingStatusesEditor.SetStatuses(attributes.StartingStatuses, context);
            permanentStatusesEditor.SetStatuses(attributes.PermanentStatuses, context);

            strongElementsEditor.SetValueAndDefaults( attributes.Strong, attributes.Default.Strong );
            weakElementsEditor.SetValueAndDefaults( attributes.Weak, attributes.Default.Weak );
            halfElementsEditor.SetValueAndDefaults( attributes.Half, attributes.Default.Half );
            absorbElementsEditor.SetValueAndDefaults( attributes.Absorb, attributes.Default.Absorb );
            cancelElementsEditor.SetValueAndDefaults( attributes.Cancel, attributes.Default.Cancel );

            if (attributes.IsInUse)
            {
                btnRepoint.Enabled = true;
                spinner_Repoint.Enabled = true;
            }
            else
            {
                btnRepoint.Enabled = false;
                spinner_Repoint.Enabled = false;
            }

            if (attributes.IsDuplicate)
            {
                spinner_Repoint.Value = attributes.DuplicateIndex;
            }
            else
            {
                spinner_Repoint.Value = attributes.Index;
                btnRepoint.Enabled = false;
            }

            spinner_Repoint.Maximum = (context == PatcherLib.Datatypes.Context.US_PSX) ? 0x4f : 0x64;

            pnl_Usage.Visible = attributes.IsInUse;
            if (attributes.IsInUse)
            {
                int referencingItemsCount = attributes.ReferencingItemIndexes.Count;

                lbl_Usage_2.Text = referencingItemsCount.ToString();
                lbl_Usage_3.Text = (referencingItemsCount == 0) ? "items" : ((referencingItemsCount == 1) ? "item: " : "items, e.g. ");

                int itemIndex = GetFirstReferencingItemIndex();
                int itemID = (itemIndex > 0xFD) ? (itemIndex + 2) : itemIndex;
                lbl_Usage_4.Text = String.Format("{0:X2} {1}", itemID, Item.GetItemNames(context)[itemID]);
            }

            cancelElementsEditor.ResumeLayout();
            absorbElementsEditor.ResumeLayout();
            halfElementsEditor.ResumeLayout();
            weakElementsEditor.ResumeLayout();
            strongElementsEditor.ResumeLayout();
            permanentStatusesEditor.ResumeLayout();
            startingStatusesEditor.ResumeLayout();
            statusImmunityEditor.ResumeLayout();
            ResumeLayout();
            this.ignoreChanges = false;
        }

		#endregion Public Methods 

		#region Private Methods 

        private void spinner_ValueChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                NumericUpDownWithDefault spinner = sender as NumericUpDownWithDefault;
                ReflectionHelpers.SetFieldOrProperty( attributes, spinner.Tag.ToString(), (byte)spinner.Value );
                OnDataChanged( sender, System.EventArgs.Empty );
            }
        }

        private void spinner_Repoint_ValueChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                btnRepoint.Enabled = (spinner_Repoint.Value != attributes.Index);
            }
        }

        private int GetFirstReferencingItemIndex()
        {
            List<int> referencingItemIndexList = new List<int>(attributes.ReferencingItemIndexes);
            referencingItemIndexList.Sort();
            return referencingItemIndexList[0];
        }

		#endregion Private Methods 

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

        public event EventHandler<ReferenceEventArgs> ItemClicked;
        private void lbl_Usage_2_Click(object sender, EventArgs e)
        {
            if (ItemClicked != null)
            {
                ItemClicked(this, new ReferenceEventArgs(GetFirstReferencingItemIndex(), attributes.ReferencingItemIndexes));
            }
        }
        private void lbl_Usage_4_Click(object sender, EventArgs e)
        {
            if (ItemClicked != null)
            {
                ItemClicked(this, new ReferenceEventArgs(GetFirstReferencingItemIndex()));
            }
        }
    }
}
