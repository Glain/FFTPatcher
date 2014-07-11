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

namespace FFTPatcher.Editors
{
    public partial class JobEditor : BaseEditor
    {
		#region Instance Variables (5) 

        private ComboBoxWithDefault[] comboBoxes;
        private bool ignoreChanges;
        private Job job;
        private Context ourContext = Context.Default;
        private NumericUpDownWithDefault[] spinners;

		#endregion Instance Variables 

		#region Public Properties (1) 

        public Job Job
        {
            get { return job; }
            set
            {
                if( value == null )
                {
                    this.Enabled = false;
                    job = null;
                }
                else if( job != value )
                {
                    job = value;
                    UpdateView();
                    this.Enabled = true;
                }
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
                cevSpinner, mPortraitSpinner, mPaletteSpinner, mGraphicSpinner };
            comboBoxes = new ComboBoxWithDefault[] {
                skillsetComboBox, innateAComboBox, innateBComboBox, innateCComboBox, innateDComboBox };

            foreach( NumericUpDownWithDefault spinner in spinners )
            {
                spinner.ValueChanged += spinner_ValueChanged;
            }
            foreach( ComboBoxWithDefault comboBox in comboBoxes )
            {
                comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
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
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public void UpdateView()
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

            if( ourContext != FFTPatch.Context )
            {
                ourContext = FFTPatch.Context;
                skillsetComboBox.Items.Clear();
                skillsetComboBox.Items.AddRange( SkillSet.DummySkillSets );
                foreach( ComboBoxWithDefault cb in new ComboBoxWithDefault[] { innateAComboBox, innateBComboBox, innateCComboBox, innateDComboBox } )
                {
                    cb.Items.Clear();
                    cb.Items.AddRange( AllAbilities.DummyAbilities );
                }
            }

            skillsetComboBox.SetValueAndDefault( job.SkillSet, job.Default.SkillSet );
            foreach( NumericUpDownWithDefault s in spinners )
            {
                // TODO Update Default
                s.SetValueAndDefault(
                    ReflectionHelpers.GetFieldOrProperty<byte>( job, s.Tag.ToString() ),
                    ReflectionHelpers.GetFieldOrProperty<byte>( job.Default, s.Tag.ToString() ) );
            }
            innateAComboBox.SetValueAndDefault( job.InnateA, job.Default.InnateA );
            innateBComboBox.SetValueAndDefault( job.InnateB, job.Default.InnateB );
            innateCComboBox.SetValueAndDefault( job.InnateC, job.Default.InnateC );
            innateDComboBox.SetValueAndDefault( job.InnateD, job.Default.InnateD );

            absorbElementsEditor.SetValueAndDefaults( job.AbsorbElement, job.Default.AbsorbElement );
            halfElementsEditor.SetValueAndDefaults( job.HalfElement, job.Default.HalfElement);
            cancelElementsEditor.SetValueAndDefaults( job.CancelElement, job.Default.CancelElement);
            weakElementsEditor.SetValueAndDefaults( job.WeakElement, job.Default.WeakElement );

            equipmentEditor.Equipment = null;
            equipmentEditor.Equipment = job.Equipment;

            innateStatusesEditor.Statuses = null;
            startingStatusesEditor.Statuses = null;
            statusImmunityEditor.Statuses = null;
            innateStatusesEditor.Statuses = job.PermanentStatus;
            statusImmunityEditor.Statuses = job.StatusImmunity;
            startingStatusesEditor.Statuses = job.StartingStatus;

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

		#region Private Methods (3) 

        private void comboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                ComboBoxWithDefault c = sender as ComboBoxWithDefault;
                ReflectionHelpers.SetFieldOrProperty( job, c.Tag.ToString(), c.SelectedItem );
                OnDataChanged( this, System.EventArgs.Empty );
            }
        }

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

        public event EventHandler<LabelClickedEventArgs> SkillSetClicked;
    }
}
