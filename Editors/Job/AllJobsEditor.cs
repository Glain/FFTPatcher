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
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class AllJobsEditor : UserControl, IHandleSelectedIndex
    {
		#region Instance Variables

        private Job cbJob = null;
        private Context ourContext = Context.Default;

        private AllSkillSets _skillSets;
        private AllAbilities _abilities;

		#endregion Instance Variables 

        private ToolTip toolTip;
        public ToolTip ToolTip
        {
            set
            {
                toolTip = value;
                jobEditor.ToolTip = value;
            }
        }

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

            jobEditor.ENTDClicked += OnENTDClicked;
        }

		#endregion Constructors 

        public int SelectedIndex { get { return jobsListBox.SelectedIndex; } set { jobsListBox.SelectedIndex = value; } }

		#region Public Methods

        public void UpdateView( AllJobs jobs, AllSkillSets skillSets, AllAbilities abilities, Context context )
        {
            if( context != ourContext )
            {
                ourContext = context;
                cbJob = null;
            }

            _skillSets = skillSets;
            _abilities = abilities;

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

        public void SetListBoxHighlightedIndexes(IEnumerable<int> highlightedIndexes)
        {
            jobsListBox.SetHighlightedIndexes(highlightedIndexes);
        }

        #endregion Public Methods 

        #region Private Methods

        public void ClearListBoxHighlightedIndexes()
        {
            jobsListBox.ClearHighlightedIndexes();
        }

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

            UpdateReferences(jobsListBox.SelectedIndex);
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
            if (args.KeyCode == Keys.Escape)
            {
                ClearListBoxHighlightedIndexes();
                jobsListBox.SetChangedColors();
                jobsListBox.Invalidate();
            }
            else if (args.KeyCode == Keys.C && args.Control)
				CloneClick( sender, args );
			else if (args.KeyCode == Keys.V && args.Control)
				PasteClick( sender, args );
		}

        #endregion Private Methods 

        private void UpdateReferences(int jobIndex)
        {
            if (jobIndex < 0)
                return;

            Job job = ((Job[])jobsListBox.DataSource)[jobIndex];

            if (job.OldSkillSet != job.SkillSet)
            {
                if (job.OldSkillSet.Value < 0xB0)
                    _skillSets.SkillSets[job.OldSkillSet.Value].ReferencingJobIDs.Remove(jobIndex);
                if (job.SkillSet.Value < 0xB0)
                    _skillSets.SkillSets[job.SkillSet.Value].ReferencingJobIDs.Add(jobIndex);
            }

            if (job.OldInnateA != job.InnateA)
            {
                if (job.InnateA.Offset != 0)
                    _abilities.Abilities[job.InnateA.Offset].ReferencingJobIDs.Add(jobIndex);

                if ((job.OldInnateA.Offset != 0) && (job.InnateB != job.OldInnateA) && (job.InnateC != job.OldInnateA) && (job.InnateD != job.OldInnateA))
                {
                    _abilities.Abilities[job.OldInnateA.Offset].ReferencingJobIDs.Remove(jobIndex);
                }
            }
            if (job.OldInnateB != job.InnateB)
            {
                if (job.InnateB.Offset != 0)
                    _abilities.Abilities[job.InnateB.Offset].ReferencingJobIDs.Add(jobIndex);

                if ((job.OldInnateB.Offset != 0) && (job.InnateA != job.OldInnateB) && (job.InnateC != job.OldInnateB) && (job.InnateD != job.OldInnateB))
                {
                    _abilities.Abilities[job.OldInnateB.Offset].ReferencingJobIDs.Remove(jobIndex);
                }
            }
            if (job.OldInnateC != job.InnateC)
            {
                if (job.InnateC.Offset != 0)
                    _abilities.Abilities[job.InnateC.Offset].ReferencingJobIDs.Add(jobIndex);

                if ((job.OldInnateC.Offset != 0) && (job.InnateA != job.OldInnateC) && (job.InnateB != job.OldInnateC) && (job.InnateD != job.OldInnateC))
                {
                    _abilities.Abilities[job.OldInnateC.Offset].ReferencingJobIDs.Remove(jobIndex);
                }
            }
            if (job.OldInnateD != job.InnateD)
            {
                if (job.InnateD.Offset != 0)
                    _abilities.Abilities[job.InnateD.Offset].ReferencingJobIDs.Add(jobIndex);

                if ((job.OldInnateD.Offset != 0) && (job.InnateA != job.OldInnateD) && (job.InnateB != job.OldInnateD) && (job.InnateC != job.OldInnateD))
                {
                    _abilities.Abilities[job.OldInnateD.Offset].ReferencingJobIDs.Remove(jobIndex);
                }
            }

            job.OldSkillSet = job.SkillSet;

            job.OldInnateA = job.InnateA;
            job.OldInnateB = job.InnateB;
            job.OldInnateC = job.InnateC;
            job.OldInnateD = job.InnateD;
        }

        public void HandleSelectedIndexChange(int offset)
        {
            int newIndex = jobsListBox.SelectedIndex + offset;
            if ((newIndex >= 0) && (newIndex < jobsListBox.Items.Count))
                jobsListBox.SelectedIndex = newIndex;
        }

        public event EventHandler<ReferenceEventArgs> ViewStatsClicked;
        private void OnViewStatsClicked(object sender, EventArgs e)
        {
            if (ViewStatsClicked != null)
            {
                ViewStatsClicked(this, new ReferenceEventArgs(jobsListBox.SelectedIndex));
            }
        }

        public event EventHandler<LabelClickedEventArgs> SkillSetClicked;

        public event EventHandler<ReferenceEventArgs> ENTDClicked;
        private void OnENTDClicked(object sender, ReferenceEventArgs e)
        {
            if (ENTDClicked != null)
            {
                ENTDClicked(this, e);
            }
        }
    }
}
