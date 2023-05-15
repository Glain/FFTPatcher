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
using PatcherLib;

namespace FFTPatcher.Editors
{
    public partial class AbilityEditor : BaseEditor
    {
		#region Instance Variables

        private Ability ability;
        private List<ComboBoxWithDefault> comboBoxes;
        private bool ignoreChanges = false;
        private Context ourContext = Context.Default;
        private List<Item> pspItems = new List<Item>( 256 );
        private List<ItemSubType> pspItemTypes = new List<ItemSubType>( (ItemSubType[])Enum.GetValues( typeof( ItemSubType ) ) );
        private List<Item> psxItems = new List<Item>( 256 );
        private List<ItemSubType> psxItemTypes = new List<ItemSubType>( (ItemSubType[])Enum.GetValues( typeof( ItemSubType ) ) );
        private List<NumericUpDownWithDefault> spinners;

		#endregion Instance Variables 

        private System.Windows.Forms.ToolTip toolTip;
        public System.Windows.Forms.ToolTip ToolTip
        {
            set
            {
                toolTip = value;
                abilityAttributesEditor.ToolTip = value;
                commonAbilitiesEditor.ToolTip = value;
            }
        }

		#region Public Properties 

        public Ability Ability
        {
            get { return ability; }
            set
            {
                SetAbility(value, ourContext);
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public AbilityEditor()
        {
            InitializeComponent();
            spinners = new List<NumericUpDownWithDefault>( new NumericUpDownWithDefault[] { 
                arithmeticksSpinner, ticksSpinner, powerSpinner, horizontalSpinner, verticalSpinner, idSpinner } );
            comboBoxes = new List<ComboBoxWithDefault>( new ComboBoxWithDefault[] { itemUseComboBox, throwingComboBox, effectComboBox } );

            arithmeticksSpinner.Tag = "ArithmetickSkill";
            ticksSpinner.Tag = "ChargeTicks";
            powerSpinner.Tag = "ChargeBonus";
            horizontalSpinner.Tag = "JumpHorizontal";
            verticalSpinner.Tag = "JumpVertical";
            itemUseComboBox.Tag = "Item";
            throwingComboBox.Tag = "Throwing";
            idSpinner.Tag = "OtherID";

            foreach( NumericUpDownWithDefault spinner in spinners )
            {
                spinner.ValueChanged += spinner_ValueChanged;
            }
            foreach( ComboBoxWithDefault combo in comboBoxes )
            {
                combo.SelectedIndexChanged += combo_SelectedIndexChanged;
            }

            foreach( Item i in Item.PSPDummies )
            {
                if( i.Offset <= 0xFF )
                {
                    pspItems.Add( i );
                }
            }
            foreach( Item i in Item.PSXDummies )
            {
                if( i.Offset <= 0xFF )
                {
                    psxItems.Add( i );
                }
            }
            psxItemTypes.Remove( ItemSubType.LipRouge );
            psxItemTypes.Remove( ItemSubType.FellSword );

            abilityAttributesEditor.LinkClicked += abilityAttributesEditor_LinkClicked;
            commonAbilitiesEditor.DataChanged += OnDataChanged;
            abilityAttributesEditor.DataChanged += OnDataChanged;

            lbl_SkillSetUsage_2.Click += lbl_SkillSetUsage_2_Click;
            lbl_SkillSetUsage_4.Click += lbl_SkillSetUsage_4_Click;
            lbl_MonsterSkillUsage_2.Click += lbl_MonsterSkillUsage_2_Click;
            lbl_MonsterSkillUsage_4.Click += lbl_MonsterSkillUsage_4_Click;
            lbl_ENTDUsage_2.Click += lbl_ENTDUsage_2_Click;
            lbl_ENTDUsage_4.Click += lbl_ENTDUsage_4_Click;
            lbl_JobUsage_2.Click += lbl_JobUsage_2_Click;
            lbl_JobUsage_4.Click += lbl_JobUsage_4_Click;
        }

		#endregion Constructors 

		#region Public Methods 

        public void SetAbility(Ability value, Context context)
        {
            if (value == null)
            {
                ability = value;
                abilityAttributesEditor.Attributes = null;
                commonAbilitiesEditor.Ability = null;
                Enabled = false;
            }
            else if (ability != value)
            {
                ability = value;
                UpdateView(context);
                Enabled = true;
            }
        }

        public void UpdateView(Context context)
        {
            ignoreChanges = true;

            commonAbilitiesEditor.SetAbility(ability, context);

            abilityAttributesEditor.Visible = ability.IsNormal;
            bool showEffect = ability.Effect != null && ability.Default.Effect != null;
            effectComboBox.Visible = showEffect;
            effectLabel.Visible = showEffect;
            //abilityAttributesEditor.Attributes = ability.Attributes;
            abilityAttributesEditor.SetAttributes(ability.Attributes, context);

            foreach( NumericUpDownWithDefault spinner in spinners )
            {
                spinner.SetValueAndDefault(
                    ReflectionHelpers.GetFieldOrProperty<byte>( ability, spinner.Tag.ToString() ),
                    ReflectionHelpers.GetFieldOrProperty<byte>( ability.Default, spinner.Tag.ToString() ),
                    toolTip);
            }
            arithmeticksPanel.Visible = ability.IsArithmetick;

            chargingPanel.Visible = ability.IsCharging;

            jumpingPanel.Visible = ability.IsJumping;

            itemUsePanel.Visible = ability.IsItem;
            abilityIdPanel.Visible = ability.IsOther;
            throwingPanel.Visible = ability.IsThrowing;

            if( context == Context.US_PSP && ourContext != Context.US_PSP)
            {
                ourContext = Context.US_PSP;
                itemUseComboBox.Items.Clear();
                itemUseComboBox.Items.AddRange( pspItems.ToArray() );
                effectComboBox.DataSource = new List<Effect>( Effect.PSPEffects.Values );
                throwingComboBox.DataSource = new List<ItemSubType>(pspItemTypes);
            }
            else if( context == Context.US_PSX && ourContext != Context.US_PSX )
            {
                ourContext = Context.US_PSX;
                itemUseComboBox.Items.Clear();
                itemUseComboBox.Items.AddRange( psxItems.ToArray() );
                effectComboBox.DataSource = new List<Effect>( Effect.PSXEffects.Values );
                throwingComboBox.DataSource = new List<ItemSubType>(psxItemTypes);
            }

            if (showEffect)
            {
                effectComboBox.SetValueAndDefault( ability.Effect, ability.Default.Effect, toolTip );
            }

            if( ability.IsItem )
            {
                itemUseComboBox.SetValueAndDefault( ability.Item, ability.Default.Item, toolTip );
            }

            if( ability.IsThrowing )
            {
                throwingComboBox.SetValueAndDefault( ability.Throwing, ability.Default.Throwing, toolTip );
            }

            int skillSetReferenceCount = ability.ReferencingSkillSetIDs.Count;
            bool isSkillSetUsagePanelVisible = (skillSetReferenceCount > 0);
            pnl_SkillSetUsage.Visible = isSkillSetUsagePanelVisible;
            if (isSkillSetUsagePanelVisible)
            {
                lbl_SkillSetUsage_2.Text = skillSetReferenceCount.ToString();
                lbl_SkillSetUsage_3.Text = (skillSetReferenceCount == 0) ? "skill sets" : ((skillSetReferenceCount == 1) ? "skill set: " : "skill sets, e.g. ");

                int skillSetIndex = GetFirstReferencingSkillSetIndex();
                lbl_SkillSetUsage_4.Text = String.Format("{0:X2} {1}", skillSetIndex, SkillSet.GetNames(context)[skillSetIndex]);
            }

            int monsterSkillReferenceCount = ability.ReferencingMonsterSkillIDs.Count;
            bool isMonsterSkillUsagePanelVisible = (monsterSkillReferenceCount > 0);
            pnl_MonsterSkillUsage.Visible = isMonsterSkillUsagePanelVisible;
            if (isMonsterSkillUsagePanelVisible)
            {
                lbl_MonsterSkillUsage_2.Text = monsterSkillReferenceCount.ToString();
                lbl_MonsterSkillUsage_3.Text = (monsterSkillReferenceCount == 0) ? "monster skills" : ((monsterSkillReferenceCount == 1) ? "monster skill: " : "monster skills, e.g. ");

                int monsterSkillIndex = GetFirstReferencingMonsterSkillIndex();
                lbl_MonsterSkillUsage_4.Text = String.Format("{0:X2} {1}", (monsterSkillIndex + 0xB0),  AllMonsterSkills.GetNames(context)[monsterSkillIndex]);
            }

            int ENTDReferenceCount = ability.ReferencingENTDs.Count;
            bool isENTDUsagePanelVisible = (ENTDReferenceCount > 0);
            pnl_ENTDUsage.Visible = isENTDUsagePanelVisible;
            if (isENTDUsagePanelVisible)
            {
                lbl_ENTDUsage_2.Text = ENTDReferenceCount.ToString();
                lbl_ENTDUsage_3.Text = (ENTDReferenceCount == 0) ? "ENTDs" : ((ENTDReferenceCount == 1) ? "ENTD: " : "ENTDs, e.g. ");

                int entdIndex = GetFirstReferencingENTDIndex();
                lbl_ENTDUsage_4.Text = String.Format("{0:X3} {1}", entdIndex, Event.GetEventNames(context)[entdIndex]);
            }

            int jobReferenceCount = ability.ReferencingJobIDs.Count;
            bool isJobUsagePanelVisible = (jobReferenceCount > 0);
            pnl_JobUsage.Visible = isJobUsagePanelVisible;
            if (isJobUsagePanelVisible)
            {
                lbl_JobUsage_2.Text = jobReferenceCount.ToString();
                lbl_JobUsage_3.Text = (jobReferenceCount == 0) ? "jobs (innate)" : ((jobReferenceCount == 1) ? "job (innate): " : "jobs (innate), e.g. ");

                int jobIndex = GetFirstReferencingJobIndex();
                lbl_JobUsage_4.Text = String.Format("{0:X2} {1}", jobIndex, AllJobs.GetNames(context)[jobIndex]);
            }

            ignoreChanges = false;
        }

        public void UpdateAttributesView(Context context)
        {
            abilityAttributesEditor.UpdateView(context);
        }

        #endregion Public Methods 

        #region Private Methods

        private int GetFirstReferencingIndex(HashSet<int> referenceSet)
        {
            List<int> referencingIndexList = new List<int>(referenceSet);
            referencingIndexList.Sort();
            return referencingIndexList[0];
        }

        private int GetFirstReferencingSkillSetIndex()
        {
            return GetFirstReferencingIndex(ability.ReferencingSkillSetIDs);
        }

        private int GetFirstReferencingMonsterSkillIndex()
        {
            return GetFirstReferencingIndex(ability.ReferencingMonsterSkillIDs);
        }

        private int GetFirstReferencingENTDIndex()
        {
            return GetFirstReferencingIndex(ability.ReferencingENTDs);
        }

        private int GetFirstReferencingJobIndex()
        {
            return GetFirstReferencingIndex(ability.ReferencingJobIDs);
        }

        private void abilityAttributesEditor_LinkClicked( object sender, LabelClickedEventArgs e )
        {
            if( InflictStatusLabelClicked != null )
            {
                InflictStatusLabelClicked( this, e );
            }
        }

        private void combo_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                ComboBoxWithDefault c = sender as ComboBoxWithDefault;
                ReflectionHelpers.SetFieldOrProperty( ability, c.Tag as string, c.SelectedItem );
                OnDataChanged( this, EventArgs.Empty );
            }
        }

        private void spinner_ValueChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                NumericUpDownWithDefault c = sender as NumericUpDownWithDefault;
                ReflectionHelpers.SetFieldOrProperty( ability, c.Tag as string, (byte)c.Value );
                OnDataChanged( this, EventArgs.Empty );
            }
        }

		#endregion Private Methods 

        public event EventHandler<LabelClickedEventArgs> InflictStatusLabelClicked;

        private void effectComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (!ignoreChanges)
            {
                ability.Effect = effectComboBox.SelectedItem as Effect;
            }
        }

        public event EventHandler<ReferenceEventArgs> SkillSetClicked;
        private void lbl_SkillSetUsage_2_Click(object sender, EventArgs e)
        {
            if (SkillSetClicked != null)
            {
                SkillSetClicked(this, new ReferenceEventArgs(GetFirstReferencingSkillSetIndex(), ability.ReferencingSkillSetIDs));
            }
        }
        private void lbl_SkillSetUsage_4_Click(object sender, EventArgs e)
        {
            if (SkillSetClicked != null)
            {
                SkillSetClicked(this, new ReferenceEventArgs(GetFirstReferencingSkillSetIndex()));
            }
        }

        public event EventHandler<ReferenceEventArgs> MonsterSkillClicked;
        private void lbl_MonsterSkillUsage_2_Click(object sender, EventArgs e)
        {
            if (MonsterSkillClicked != null)
            {
                MonsterSkillClicked(this, new ReferenceEventArgs(GetFirstReferencingMonsterSkillIndex(), ability.ReferencingMonsterSkillIDs));
            }
        }
        private void lbl_MonsterSkillUsage_4_Click(object sender, EventArgs e)
        {
            if (MonsterSkillClicked != null)
            {
                MonsterSkillClicked(this, new ReferenceEventArgs(GetFirstReferencingMonsterSkillIndex()));
            }
        }

        public event EventHandler<ReferenceEventArgs> ENTDClicked;
        private void lbl_ENTDUsage_2_Click(object sender, EventArgs e)
        {
            if (ENTDClicked != null)
            {
                ENTDClicked(this, new ReferenceEventArgs(GetFirstReferencingENTDIndex(), ability.ReferencingENTDs));
            }
        }
        private void lbl_ENTDUsage_4_Click(object sender, EventArgs e)
        {
            if (ENTDClicked != null)
            {
                ENTDClicked(this, new ReferenceEventArgs(GetFirstReferencingENTDIndex()));
            }
        }

        public event EventHandler<ReferenceEventArgs> JobClicked;
        private void lbl_JobUsage_2_Click(object sender, EventArgs e)
        {
            if (JobClicked != null)
            {
                JobClicked(this, new ReferenceEventArgs(GetFirstReferencingJobIndex(), ability.ReferencingJobIDs));
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
