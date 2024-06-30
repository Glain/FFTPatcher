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
using PatcherLib.Datatypes;
using PatcherLib;
using System.Collections.Generic;

namespace FFTPatcher.Editors
{
    public partial class JobEditor : BaseEditor
    {
		#region Instance Variables 

        private ComboBoxWithDefault[] valueComboBoxes;
        private ComboBoxWithDefault[] indexComboBoxes;
        private bool ignoreChanges;
        private Job job;
        private Context ourContext = Context.Default;
        private NumericUpDownWithDefault[] spinners;

		#endregion Instance Variables 

        private System.Windows.Forms.ToolTip toolTip;
        public System.Windows.Forms.ToolTip ToolTip
        {
            set
            {
                toolTip = value;
            }
        }

		#region Public Properties

        public Job Job
        {
            get { return job; }
            set
            {
                SetJob(value, ourContext);
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public JobEditor()
        {
            InitializeComponent();
            spinners = new NumericUpDownWithDefault[] {
                hpGrowthSpinner, hpMultiplierSpinner, mpGrowthSpinner, mpMultiplierSpinner,
                speedGrowthSpinner, speedMultiplierSpinner, paGrowthSpinner, paMultiplierSpinner,
                maGrowthSpinner, maMultiplierSpinner, moveSpinner, jumpSpinner,
                cevSpinner, mPaletteSpinner, 
                spinner_FormationSprites1, spinner_FormationSprites2 };
            valueComboBoxes = new ComboBoxWithDefault[] {
                skillsetComboBox, innateAComboBox, innateBComboBox, innateCComboBox, innateDComboBox,
            };
            indexComboBoxes = new ComboBoxWithDefault[] {
                cmb_MPortrait, cmb_MType
            };

            foreach( NumericUpDownWithDefault spinner in spinners )
            {
                spinner.ValueChanged += spinner_ValueChanged;
            }
            foreach (ComboBoxWithDefault comboBox in valueComboBoxes)
            {
                comboBox.SelectedIndexChanged += valueComboBox_SelectedIndexChanged;
            }
            foreach (ComboBoxWithDefault comboBox in indexComboBoxes)
            {
                comboBox.SelectedIndexChanged += indexComboBox_SelectedIndexChanged;
            }

            absorbElementsEditor.DataChanged += OnDataChanged;
            halfElementsEditor.DataChanged += OnDataChanged;
            cancelElementsEditor.DataChanged += OnDataChanged;
            weakElementsEditor.DataChanged += OnDataChanged;
            equipmentEditor.DataChanged += OnDataChanged;
            innateStatusesEditor.DataChanged += OnDataChanged;
            statusImmunityEditor.DataChanged += OnDataChanged;
            startingStatusesEditor.DataChanged += OnDataChanged;

            skillSetLabel.TabStop = false;
            skillSetLabel.Click += skillSetLabel_Click;

            lbl_ENTDUsage_2.Click += lbl_ENTDUsage_2_Click;
            lbl_ENTDUsage_4.Click += lbl_ENTDUsage_4_Click;
        }

		#endregion Constructors 

		#region Public Methods

        public void SetJob(Job value, Context context)
        {
            if (value == null)
            {
                this.Enabled = false;
                job = null;
            }
            else if (job != value)
            {
                job = value;
                UpdateView(context);
                this.Enabled = true;
            }
        }

        public void UpdateView(Context context)
        {
            ignoreChanges = true;
            this.SuspendLayout();
            absorbElementsEditor.SuspendLayout();
            cancelElementsEditor.SuspendLayout();
            halfElementsEditor.SuspendLayout();
            weakElementsEditor.SuspendLayout();
            equipmentEditor.SuspendLayout();
            innateStatusesEditor.SuspendLayout();
            statusImmunityEditor.SuspendLayout();
            startingStatusesEditor.SuspendLayout();

            if( ourContext != context )
            {
                ourContext = context;
                skillsetComboBox.Items.Clear();
                skillsetComboBox.Items.AddRange( SkillSet.GetDummySkillSets(context) );
                foreach( ComboBoxWithDefault cb in new ComboBoxWithDefault[] { innateAComboBox, innateBComboBox, innateCComboBox, innateDComboBox } )
                {
                    cb.Items.Clear();
                    cb.Items.AddRange( AllAbilities.GetDummyAbilities(context) );
                }

                cmb_MPortrait.Items.Clear();
                cmb_MType.Items.Clear();
                System.Collections.Generic.IList<string> spriteNames = (ourContext == Context.US_PSX) ? PSXResources.Lists.SpriteFiles : PSPResources.Lists.SpriteFiles;
                int spriteNameCount = spriteNames.Count;
                cmb_MPortrait.Items.Add("00");
                cmb_MType.Items.Add("00");
                for (int index = 1; index < spriteNameCount; index++)
                {
                    string spriteName = spriteNames[index];

                    cmb_MPortrait.Items.Add(String.Format("{0} {1}", (index).ToString("X2"), spriteName));
                    if ((index >= 0x86) && (index <= 0x9A))
                    {
                        cmb_MType.Items.Add(String.Format("{0} {1}", (index - 0x85).ToString("X2"), spriteName));
                    }
                }
                for (int index = cmb_MType.Items.Count; index <= spriteNameCount; index++)
                {
                    cmb_MType.Items.Add(index.ToString("X2"));
                }
                for (int index = (spriteNameCount + 1); index < 0x100; index++)
                {
                    cmb_MPortrait.Items.Add(index.ToString("X2"));
                    cmb_MType.Items.Add(index.ToString("X2"));
                }
            }

            skillsetComboBox.SetValueAndDefault( job.SkillSet, job.Default.SkillSet, toolTip );
            foreach( NumericUpDownWithDefault s in spinners )
            {
                // TODO Update Default
                s.SetValueAndDefault(
                    ReflectionHelpers.GetFieldOrProperty<byte>( job, s.Tag.ToString() ),
                    ReflectionHelpers.GetFieldOrProperty<byte>( job.Default, s.Tag.ToString() ),
                    toolTip);
            }
            innateAComboBox.SetValueAndDefault( job.InnateA, job.Default.InnateA, toolTip );
            innateBComboBox.SetValueAndDefault(job.InnateB, job.Default.InnateB, toolTip);
            innateCComboBox.SetValueAndDefault(job.InnateC, job.Default.InnateC, toolTip);
            innateDComboBox.SetValueAndDefault(job.InnateD, job.Default.InnateD, toolTip);
            cmb_MPortrait.SetValueAndDefault(cmb_MPortrait.Items[job.MPortrait], cmb_MPortrait.Items[job.Default.MPortrait], toolTip);
            cmb_MType.SetValueAndDefault(cmb_MType.Items[job.MGraphic], cmb_MPortrait.Items[job.Default.MGraphic], toolTip);

            absorbElementsEditor.SetValueAndDefaults( job.AbsorbElement, job.Default.AbsorbElement );
            halfElementsEditor.SetValueAndDefaults( job.HalfElement, job.Default.HalfElement);
            cancelElementsEditor.SetValueAndDefaults( job.CancelElement, job.Default.CancelElement);
            weakElementsEditor.SetValueAndDefaults( job.WeakElement, job.Default.WeakElement );

            equipmentEditor.Equipment = null;
            //equipmentEditor.Equipment = job.Equipment;
            equipmentEditor.SetEquipment(job.Equipment, context);

            innateStatusesEditor.Statuses = null;
            statusImmunityEditor.Statuses = null;
            startingStatusesEditor.Statuses = null;
            //innateStatusesEditor.Statuses = job.PermanentStatus;
            //statusImmunityEditor.Statuses = job.StatusImmunity;
            //startingStatusesEditor.Statuses = job.StartingStatus;
            innateStatusesEditor.SetStatuses(job.PermanentStatus, context);
            statusImmunityEditor.SetStatuses(job.StatusImmunity, context);
            startingStatusesEditor.SetStatuses(job.StartingStatus, context);

            pnl_FormationSprites.Visible = (job.Value < 0x4A);

            int ENTDReferenceCount = job.ReferencingENTDs.Count;
            bool isENTDUsagePanelVisible = (ENTDReferenceCount > 0);
            pnl_ENTDUsage.Visible = isENTDUsagePanelVisible;
            if (isENTDUsagePanelVisible)
            {
                lbl_ENTDUsage_2.Text = ENTDReferenceCount.ToString();
                lbl_ENTDUsage_3.Text = (ENTDReferenceCount == 0) ? "ENTDs" : ((ENTDReferenceCount == 1) ? "ENTD: " : "ENTDs, e.g. ");

                int ENTDIndex = GetFirstReferencingENTDIndex();
                lbl_ENTDUsage_4.Text = String.Format("{0:X3} {1}", ENTDIndex, Event.GetEventNames(context)[ENTDIndex]);
            }

            ignoreChanges = false;
            absorbElementsEditor.ResumeLayout();
            cancelElementsEditor.ResumeLayout();
            halfElementsEditor.ResumeLayout();
            weakElementsEditor.ResumeLayout();
            equipmentEditor.ResumeLayout();
            innateStatusesEditor.ResumeLayout();
            statusImmunityEditor.ResumeLayout();
            startingStatusesEditor.ResumeLayout();
            this.ResumeLayout();
        }

        #endregion Public Methods 

        #region Private Methods

        private int GetFirstReferencingIndex(HashSet<int> referenceSet)
        {
            List<int> referencingIndexList = new List<int>(referenceSet);
            referencingIndexList.Sort();
            return referencingIndexList[0];
        }

        private int GetFirstReferencingENTDIndex()
        {
            return GetFirstReferencingIndex(job.ReferencingENTDs);
        }

        private void ChangeValueFromComboBox(ComboBoxWithDefault control, bool useIndex)
        {
            if (!ignoreChanges)
            {
                ReflectionHelpers.SetFieldOrProperty(job, control.Tag.ToString(), (useIndex ? (byte)(control.SelectedIndex) : control.SelectedItem));
                OnDataChanged(this, System.EventArgs.Empty);
            }
        }

        private void valueComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueFromComboBox((sender as ComboBoxWithDefault), false);
        }

        private void indexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeValueFromComboBox((sender as ComboBoxWithDefault), true);
        }

        /*
        private void valueComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                ComboBoxWithDefault c = sender as ComboBoxWithDefault;
                ReflectionHelpers.SetFieldOrProperty( job, c.Tag.ToString(), c.SelectedItem );
                OnDataChanged( this, System.EventArgs.Empty );
            }
        }
        */

        private void skillSetLabel_Click( object sender, EventArgs e )
        {
            if( SkillSetClicked != null )
            {
                SkillSetClicked( this, new LabelClickedEventArgs( (byte)skillsetComboBox.SelectedIndex ) );
            }
        }

        private void spinner_ValueChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                NumericUpDownWithDefault spinner = sender as NumericUpDownWithDefault;
                ReflectionHelpers.SetFieldOrProperty( job, spinner.Tag.ToString(), (byte)spinner.Value );
                OnDataChanged( this, System.EventArgs.Empty );
            }
        }

        #endregion Private Methods

        public event EventHandler<ReferenceEventArgs> ENTDClicked;
        private void lbl_ENTDUsage_2_Click(object sender, EventArgs e)
        {
            if (ENTDClicked != null)
            {
                ENTDClicked(this, new ReferenceEventArgs(GetFirstReferencingENTDIndex(), job.ReferencingENTDs));
            }
        }
        private void lbl_ENTDUsage_4_Click(object sender, EventArgs e)
        {
            if (ENTDClicked != null)
            {
                ENTDClicked(this, new ReferenceEventArgs(GetFirstReferencingENTDIndex()));
            }
        }

        public event EventHandler ViewStatsClicked;
        private void btn_ViewStats_Click(object sender, EventArgs e)
        {
            if (ViewStatsClicked != null)
            {
                ViewStatsClicked(this, e);
            }
        }

        public event EventHandler<LabelClickedEventArgs> SkillSetClicked;
    }
}
