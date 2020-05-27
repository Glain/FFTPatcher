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
        }

		#endregion Constructors 

		#region Private Methods 

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

		#endregion Private Methods
    }
}
