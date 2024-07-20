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
using PatcherLib;
using PatcherLib.Utilities;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class CommonAbilitiesEditor : BaseEditor
    {
		#region Instance Variables

        private Ability ability;
        private static readonly string[] AIPropertyNames = new string[] {
            "AIHP", "AIMP", "AICancelStatus", "AIAddStatus", "AIStats", "AIUnequip", "AITargetEnemies", "AITargetAllies",
            "AILineOfSight", "AIReflectable", "AIUndeadReverse", "AIUnknown1", "AIAllowRandomly", "AIUnknown2", "AIUnknown3", "AISilence",
            "AIBlank", "AIUnknown4", "AIUnknown5", "AIUnknown6", "AIUnknown7", "AIUnknown8", "AIUnknown9", "AIUnknown10" };
        bool ignoreChanges = false;
        private Context ourContext = Context.Default;
        private static readonly string[] PropertiesNames = new string[] { 
            "LearnWithJP", "DisplayAbilityName", "LearnOnHit", "Blank1", 
            "UsedByEnemies", "Unknown2", "Unknown3", "Blank2", 
            "Blank3", "Blank4", "Blank5", "Unknown4" };

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

        public CommonAbilitiesEditor()
        {
            InitializeComponent();

            jpCostSpinner.ValueChanged +=
                delegate( object sender, EventArgs e )
                {
                    if( !ignoreChanges )
                    {
                        ability.JPCost = (UInt16)jpCostSpinner.Value;
                        OnDataChanged( this, EventArgs.Empty );
                    }
                };
            chanceSpinner.ValueChanged +=
                delegate( object sender, EventArgs e )
                {
                    if( !ignoreChanges )
                    {
                        ability.LearnRate = (byte)chanceSpinner.Value;
                        OnDataChanged( this, EventArgs.Empty );
                    }
                };
            abilityTypeComboBox.SelectedIndexChanged +=
                delegate( object sender, EventArgs e )
                {
                    if( !ignoreChanges )
                    {
                        ability.AbilityType = (AbilityType)abilityTypeComboBox.SelectedIndex;
                        OnDataChanged( this, EventArgs.Empty );
                    }
                };
            propertiesCheckedListBox.ItemCheck += CheckedListBox_ItemCheck;
            aiCheckedListBox.ItemCheck += CheckedListBox_ItemCheck;
        }

		#endregion Constructors 

        public void SetAbility(Ability value, Context context)
        {
            if (value == null)
            {
                ability = null;
                Enabled = false;
            }
            else if( ability != value )
            {
                ability = value;
                UpdateView(context);
                Enabled = true;
            }
        }

		#region Private Methods (4) 

        private void CheckedListBox_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            if( !ignoreChanges )
            {
                CheckedListBox clb = sender as CheckedListBox;
                if( clb == propertiesCheckedListBox )
                {
                    SetAbilityFlag( PropertiesNames[e.Index], e.NewValue == CheckState.Checked );
                }
                else if( clb == aiCheckedListBox )
                {
                    SetAbilityFlag( AIPropertyNames[e.Index], e.NewValue == CheckState.Checked );
                }
                OnDataChanged( this, EventArgs.Empty );
            }
        }

        private bool GetFlagFromAbility( string name )
        {
            return ReflectionHelpers.GetFlag( ability, name );
        }

        private void SetAbilityFlag( string name, bool newValue )
        {
            ReflectionHelpers.SetFlag( ability, name, newValue );
        }

        private void UpdateView(Context context)
        {
            this.SuspendLayout();
            ignoreChanges = true;

            if( ourContext != context )
            {
                ourContext = context;
                aiCheckedListBox.Items.Clear();
                aiCheckedListBox.Items.AddRange( ourContext == Context.US_PSP ? PSPResources.Lists.AbilityAI.ToArray() : PSXResources.Lists.AbilityAI.ToArray() );
                abilityTypeComboBox.DataSource = ourContext == Context.US_PSP ? PSPResources.Lists.AbilityTypes : PSXResources.Lists.AbilityTypes;
                propertiesCheckedListBox.DataSource = ourContext == Context.US_PSP ? PSPResources.Lists.AbilityProperties : PSXResources.Lists.AbilityProperties;
            }

            jpCostSpinner.SetValueAndDefault( ability.JPCost, ability.Default.JPCost, toolTip );
            chanceSpinner.SetValueAndDefault( ability.LearnRate, ability.Default.LearnRate, toolTip );

            abilityTypeComboBox.SetValueAndDefault(
                abilityTypeComboBox.Items[(byte)ability.AbilityType],
                abilityTypeComboBox.Items[(byte)ability.Default.AbilityType],
                toolTip);

            if( ability.Default != null )
            {
                propertiesCheckedListBox.SetValuesAndDefaults( ReflectionHelpers.GetFieldsOrProperties<bool>( ability, PropertiesNames ), ability.Default.PropertiesToBoolArray() );
                bool[] bools = ability.Default.AIFlags.ToBoolArray();

                aiCheckedListBox.SetValuesAndDefaults( ReflectionHelpers.GetFieldsOrProperties<bool>( ability, AIPropertyNames ), ability.Default.AIFlags.ToBoolArray() );
            }

            ignoreChanges = false;
            this.ResumeLayout();
        }

		#endregion Private Methods 
    }
}
