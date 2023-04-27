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
using FFTPatcher.Controls;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class SkillSetEditor : BaseEditor
    {
		#region Instance Variables (5) 

        private List<ComboBoxWithDefault> actionComboBoxes;
        private bool ignoreChanges = false;
        private Context ourContext = Context.Default;
        private SkillSet skillSet;
        private List<ComboBoxWithDefault> theRestComboBoxes;

		#endregion Instance Variables 

        private System.Windows.Forms.ToolTip toolTip;
        public System.Windows.Forms.ToolTip ToolTip
        {
            set
            {
                toolTip = value;
            }
        }

		#region Public Properties (1) 

        public SkillSet SkillSet
        {
            get { return skillSet; }
            set
            {
                SetSkillSet(value, ourContext);
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public SkillSetEditor()
        {
            InitializeComponent();
            actionComboBoxes = new List<ComboBoxWithDefault>( new ComboBoxWithDefault[] { 
                actionComboBox1, actionComboBox2, actionComboBox3, actionComboBox4, 
                actionComboBox5, actionComboBox6, actionComboBox7, actionComboBox8, 
                actionComboBox9, actionComboBox10, actionComboBox11, actionComboBox12, 
                actionComboBox13, actionComboBox14, actionComboBox15, actionComboBox16 } );
            theRestComboBoxes = new List<ComboBoxWithDefault>( new ComboBoxWithDefault[] {
                theRestComboBox1, theRestComboBox2, theRestComboBox3,
                theRestComboBox4, theRestComboBox5, theRestComboBox6 } );

            lbl_JobUsage_2.Click += lbl_JobUsage_2_Click;
            lbl_JobUsage_4.Click += lbl_JobUsage_4_Click;
        }

		#endregion Constructors 

		#region Public Methods

        public void SetSkillSet(SkillSet value, Context context)
        {
            if (value == null)
            {
                this.Enabled = false;
                skillSet = null;
            }
            else if (skillSet != value)
            {
                this.Enabled = true;
                skillSet = value;
                UpdateView(context);
            }
        }

        public void UpdateView(Context context)
        {        	
            this.ignoreChanges = true;
            this.SuspendLayout();
            actionGroupBox.SuspendLayout();
            theRestGroupBox.SuspendLayout();

            if( ourContext != context )
            {
                ourContext = context;
                
                foreach( ComboBoxWithDefault actionComboBox in actionComboBoxes )
                {
                    actionComboBox.Items.Clear();
                    actionComboBox.Items.AddRange( AllAbilities.GetDummyAbilities(context) );
                    actionComboBox.SelectedIndexChanged += actionComboBox_SelectedIndexChanged;
                }
                foreach( ComboBoxWithDefault theRestComboBox in theRestComboBoxes )
                {
                    theRestComboBox.Items.Clear();
                    theRestComboBox.Items.AddRange( AllAbilities.GetDummyAbilities(context) );
                    theRestComboBox.SelectedIndexChanged += theRestComboBox_SelectedIndexChanged;
                }
            }
            for( int i = 0; i < 16; i++ )
            {
                actionComboBoxes[i].SetValueAndDefault( skillSet.Actions[i], skillSet.Default.Actions[i], toolTip );
            }
            for( int i = 0; i < 6; i++ )
            {
                theRestComboBoxes[i].SetValueAndDefault( skillSet.TheRest[i], skillSet.Default.TheRest[i], toolTip );
            }

            int jobReferenceCount = skillSet.ReferencingJobIDs.Count;
            bool isJobUsagePanelVisible = (jobReferenceCount > 0);
            pnl_JobUsage.Visible = isJobUsagePanelVisible;
            if (isJobUsagePanelVisible)
            {
                lbl_JobUsage_2.Text = jobReferenceCount.ToString();
                lbl_JobUsage_3.Text = (jobReferenceCount == 0) ? "jobs" : ((jobReferenceCount == 1) ? "job: " : "jobs, e.g. ");

                int jobIndex = GetFirstReferencingJobIndex();
                lbl_JobUsage_4.Text = String.Format("{0:X2} {1}", jobIndex, AllJobs.GetNames(context)[jobIndex]);
            }

            theRestGroupBox.ResumeLayout();
            actionGroupBox.ResumeLayout();
            this.ResumeLayout();
            this.ignoreChanges = false;
        }

        #endregion Public Methods 

        #region Private Methods 

        private int GetFirstReferencingIndex(HashSet<int> referenceSet)
        {
            List<int> referencingIndexList = new List<int>(referenceSet);
            referencingIndexList.Sort();
            return referencingIndexList[0];
        }

        private int GetFirstReferencingJobIndex()
        {
            return GetFirstReferencingIndex(skillSet.ReferencingJobIDs);
        }

        private void actionComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                ComboBoxWithDefault c = sender as ComboBoxWithDefault;
                int i = actionComboBoxes.IndexOf( c );
                skillSet.Actions[i] = c.SelectedItem as Ability;
                OnDataChanged( this, System.EventArgs.Empty );
            }
        }

        private void theRestComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                ComboBoxWithDefault c = sender as ComboBoxWithDefault;
                int i = theRestComboBoxes.IndexOf( c );
                skillSet.TheRest[i] = c.SelectedItem as Ability;
                OnDataChanged( this, System.EventArgs.Empty );
            }
        }

        #endregion Private Methods 

        public event EventHandler<ReferenceEventArgs> JobClicked;
        private void lbl_JobUsage_2_Click(object sender, EventArgs e)
        {
            if (JobClicked != null)
            {
                JobClicked(this, new ReferenceEventArgs(GetFirstReferencingJobIndex(), skillSet.ReferencingJobIDs));
            }
        }
        private void lbl_JobUsage_4_Click(object sender, EventArgs e)
        {
            if (JobClicked != null)
            {
                JobClicked(this, new ReferenceEventArgs(GetFirstReferencingJobIndex()));
            }
        }
    }
}
