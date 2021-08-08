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
using System.Windows.Forms;
using FFTPatcher.Controls;
using FFTPatcher.Datatypes;
using PatcherLib;

namespace FFTPatcher.Editors
{
    public partial class StatusAttributeEditor : BaseEditor
    {
		#region Instance Variables (4) 

        private bool ignoreChanges = false;
        private static readonly string[] PropertyNames = new string[] {
            "FreezeCT", "Unknown1", "Unknown2", "Unknown3", "Unknown4", "Unknown5", "Unknown6", "CountsAsKO",
            "CanReact", "Blank", "IgnoreAttacks", "IgnoredIfMount", "Unknown8", "Unknown9", "CancelledByImmortal", "LowerTargetPriority" };
        private NumericUpDownWithDefault[] spinners;
        private StatusAttribute statusAttribute;
        private PatcherLib.Datatypes.Context ourContext = PatcherLib.Datatypes.Context.Default;

		#endregion Instance Variables 

        private ToolTip toolTip;
        public ToolTip ToolTip
        {
            set
            {
                toolTip = value;
            }
        }

		#region Public Properties

        public StatusAttribute StatusAttribute
        {
            get { return statusAttribute; }
            set
            {
                SetStatusAttribute(value, ourContext);
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public StatusAttributeEditor()
        {
            InitializeComponent();
            spinners = new NumericUpDownWithDefault[4] { unknown1Spinner, unknown2Spinner, orderSpinner, ticksSpinner };
            foreach( NumericUpDownWithDefault spinner in spinners )
            {
                spinner.ValueChanged += spinner_ValueChanged;
            }
            checkedListBox.ItemCheck += checkedListBox_ItemCheck;
            checkedListBox.ItemCheck += OnDataChanged;
            cantStackStatusesEditor.DataChanged += OnDataChanged;
            cancelStatusesEditor.DataChanged += OnDataChanged;
        }

		#endregion Constructors 

		#region Private Methods 

        private void checkedListBox_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            if( !ignoreChanges )
            {
                ReflectionHelpers.SetFieldOrProperty( statusAttribute, PropertyNames[e.Index], e.NewValue == CheckState.Checked );
                OnDataChanged( this, System.EventArgs.Empty );
            }
        }

        private void spinner_ValueChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                NumericUpDownWithDefault spinner = sender as NumericUpDownWithDefault;
                ReflectionHelpers.SetFieldOrProperty( statusAttribute, spinner.Tag.ToString(), (byte)spinner.Value );
                OnDataChanged( this, System.EventArgs.Empty );
            }
        }

        public void SetStatusAttribute(StatusAttribute value, PatcherLib.Datatypes.Context context)
        {
            if (value == null)
            {
                statusAttribute = null;
                this.Enabled = false;
            }
            else if (statusAttribute != value)
            {
                statusAttribute = value;
                this.Enabled = true;
                UpdateView(context);
            }
        }

        public void UpdateView(PatcherLib.Datatypes.Context context)
        {
            this.ignoreChanges = true;

            ourContext = context;

            foreach( NumericUpDownWithDefault spinner in spinners )
            {
                spinner.SetValueAndDefault(
                    ReflectionHelpers.GetFieldOrProperty<byte>( statusAttribute, spinner.Tag.ToString() ),
                    ReflectionHelpers.GetFieldOrProperty<byte>( statusAttribute.Default, spinner.Tag.ToString() ),
                    toolTip);
            }

            if( statusAttribute.Default != null )
            {
                checkedListBox.SetValuesAndDefaults( ReflectionHelpers.GetFieldsOrProperties<bool>( statusAttribute, PropertyNames ), statusAttribute.Default.ToBoolArray() );
            }

            //cantStackStatusesEditor.Statuses = statusAttribute.CantStackOn;
            //cancelStatusesEditor.Statuses = statusAttribute.Cancels;
            cantStackStatusesEditor.SetStatuses(statusAttribute.CantStackOn, context);
            cancelStatusesEditor.SetStatuses(statusAttribute.Cancels, context);

            cantStackStatusesEditor.UpdateView(context);
            cancelStatusesEditor.UpdateView(context);

            this.ignoreChanges = false;
        }

		#endregion Private Methods 
    }
}
