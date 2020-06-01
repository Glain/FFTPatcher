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
                arithmeticksSpinner, ctSpinner, powerSpinner, horizontalSpinner, verticalSpinner, idSpinner } );
            comboBoxes = new List<ComboBoxWithDefault>( new ComboBoxWithDefault[] { itemUseComboBox, throwingComboBox, effectComboBox } );

            arithmeticksSpinner.Tag = "ArithmetickSkill";
            ctSpinner.Tag = "ChargeCT";
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

            ignoreChanges = false;
        }

        public void UpdateAttributesView(Context context)
        {
            abilityAttributesEditor.UpdateView(context);
        }

		#endregion Public Methods 

		#region Private Methods (3) 

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
    }
}
