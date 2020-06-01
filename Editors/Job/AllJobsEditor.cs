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
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class AllJobsEditor : UserControl
    {
		#region Instance Variables (2) 

        private Job cbJob = null;
        private Context ourContext = Context.Default;

		#endregion Instance Variables 

		#region Constructors (1) 

        public AllJobsEditor()
        {
            InitializeComponent();
            jobEditor.SkillSetClicked += jobEditor_SkillSetClicked;
            jobEditor.DataChanged += jobEditor_DataChanged;
            jobsListBox.ContextMenu = new ContextMenu( new MenuItem[] {
                new MenuItem("Clone", CloneClick),
                new MenuItem("Paste", PasteClick) } );
            jobsListBox.ContextMenu.Popup += new EventHandler( ContextMenu_Popup );
            jobsListBox.MouseDown += new MouseEventHandler( jobsListBox_MouseDown );

            jobEditor.ViewStatsClicked += OnViewStatsClicked;
        }

		#endregion Constructors 

        public int SelectedIndex { get { return jobsListBox.SelectedIndex; } set { jobsListBox.SelectedIndex = value; } }

		#region Public Methods

        public void UpdateView( AllJobs jobs, Context context )
        {
            if( context != ourContext )
            {
                ourContext = context;
                cbJob = null;
            }
            jobsListBox.SelectedIndexChanged -= jobsListBox_SelectedIndexChanged;
            jobsListBox.DataSource = jobs.Jobs;
            jobsListBox.SelectedIndexChanged += jobsListBox_SelectedIndexChanged;
            jobsListBox.SelectedIndex = 0;
            //jobEditor.Job = jobsListBox.SelectedItem as Job;
            jobEditor.SetJob(jobsListBox.SelectedItem as Job, context);
            jobsListBox.SetChangedColors();
        }

        public void UpdateSelectedEntry()
        {
            jobEditor.UpdateView(ourContext);
        }

        public void UpdateListBox()
        {
            jobsListBox.SetChangedColors();
        }

		#endregion Public Methods 

		#region Private Methods (7) 

        private void CloneClick( object sender, EventArgs args )
        {
            cbJob = jobsListBox.SelectedItem as Job;
        }

        void ContextMenu_Popup( object sender, EventArgs e )
        {
            jobsListBox.ContextMenu.MenuItems[1].Enabled = cbJob != null;
        }

        private void jobEditor_DataChanged( object sender, EventArgs e )
        {
            jobsListBox.BeginUpdate();
            var top = jobsListBox.TopIndex;
            CurrencyManager cm = (CurrencyManager)BindingContext[jobsListBox.DataSource];
            cm.Refresh();
            jobsListBox.TopIndex = top;
            jobsListBox.EndUpdate();
            jobsListBox.SetChangedColor();
        }

        private void jobEditor_SkillSetClicked( object sender, LabelClickedEventArgs e )
        {
            if( SkillSetClicked != null )
            {
                SkillSetClicked( this, e );
            }
        }

        void jobsListBox_MouseDown( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Right )
            {
                jobsListBox.SelectedIndex = jobsListBox.IndexFromPoint( e.Location );
            }
        }

        private void jobsListBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            Job j = jobsListBox.SelectedItem as Job;
            jobEditor.Job = j;
        }

        private void PasteClick( object sender, EventArgs args )
        {
            if( cbJob != null )
            {
                cbJob.CopyTo( jobsListBox.SelectedItem as Job );
                jobEditor.UpdateView(ourContext);
                jobEditor_DataChanged( jobEditor, EventArgs.Empty );
            }
        }

       	private void jobsListBox_KeyDown( object sender, KeyEventArgs args )
		{
			if (args.KeyCode == Keys.C && args.Control)
				CloneClick( sender, args );
			else if (args.KeyCode == Keys.V && args.Control)
				PasteClick( sender, args );
		}
        
		#endregion Private Methods 

        public event EventHandler<ReferenceEventArgs> ViewStatsClicked;
        private void OnViewStatsClicked(object sender, EventArgs e)
        {
            if (ViewStatsClicked != null)
            {
                ViewStatsClicked(this, new ReferenceEventArgs(jobsListBox.SelectedIndex));
            }
        }

        public event EventHandler<LabelClickedEventArgs> SkillSetClicked;
    }
}
