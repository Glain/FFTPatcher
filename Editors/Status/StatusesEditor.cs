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
using PatcherLib.Utilities;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class StatusesEditor : BaseEditor
    {
		#region Instance Variables (3) 

        private bool ignoreChanges = false;
        private Context ourContext = Context.Default;
        private Statuses statuses;

		#endregion Instance Variables 

		#region Public Properties (2) 

        public string Status { get { return statusGroupBox.Text; } set { statusGroupBox.Text = value; } }

        public Statuses Statuses
        {
            get { return statuses; }
            set
            {
                SetStatuses(value, ourContext);
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public StatusesEditor()
        {
            InitializeComponent();
            statusesCheckedListBox.ItemCheck += statusesCheckedListBox_ItemCheck;
        }

		#endregion Constructors 

		#region Methods 

        private void statusesCheckedListBox_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            if( !ignoreChanges )
            {
                ReflectionHelpers.SetFlag( statuses, Statuses.FieldNames[e.Index], e.NewValue == CheckState.Checked );
                OnDataChanged( this, System.EventArgs.Empty );
            }
        }

        public void SetStatuses(Statuses value, Context context)
        {
            if (value == null)
            {
                this.Enabled = false;
                statuses = null;
            }
            else if (statuses != value)
            {
                statuses = value;
                UpdateView(context);
                this.Enabled = true;
            }
        }

        public void UpdateView(Context context)
        {
            this.SuspendLayout();
            statusesCheckedListBox.SuspendLayout();

            ignoreChanges = true;

            if( ourContext != context )
            {
                ourContext = context;
                statusesCheckedListBox.Items.Clear();
                statusesCheckedListBox.Items.AddRange( ourContext == Context.US_PSP ? PSPResources.Lists.StatusNames.ToArray() : PSXResources.Lists.StatusNames.ToArray() );
            }
        
            if( statuses.Default != null )
            {
                statusesCheckedListBox.SetValuesAndDefaults( ReflectionHelpers.GetFieldsOrProperties<bool>( statuses, Statuses.FieldNames ), statuses.Default.ToBoolArray() );
            }

            ignoreChanges = false;
            statusesCheckedListBox.ResumeLayout();
            this.ResumeLayout();
        }

		#endregion Private Methods 
    }
}
