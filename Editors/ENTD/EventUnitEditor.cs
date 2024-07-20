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
using FFTPatcher.Controls;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using PatcherLib;
using PatcherLib.Resources;

namespace FFTPatcher.Editors
{
    public partial class EventUnitEditor : BaseEditor
    {
        #region Instance Variables 

        private ComboBoxWithDefault[] comboBoxes;
        private EventUnit eventUnit = null;
        private bool ignoreChanges = false;
        private static readonly string[] levelStrings = new string[] {
            "Party level - Random",
            "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99",
            "Party level",
            "Party level + 1", "Party level + 2", "Party level + 3", "Party level + 4", "Party level + 5", "Party level + 6", "Party level + 7", "Party level + 8", "Party level + 9",
            "Party level + 10", "Party level + 11", "Party level + 12", "Party level + 13", "Party level + 14", "Party level + 15", "Party level + 16", "Party level + 17", "Party level + 18", "Party level + 19",
            "Party level + 20", "Party level + 21", "Party level + 22", "Party level + 23", "Party level + 24", "Party level + 25", "Party level + 26", "Party level + 27", "Party level + 28", "Party level + 29",
            "Party level + 30", "Party level + 31", "Party level + 32", "Party level + 33", "Party level + 34", "Party level + 35", "Party level + 36", "Party level + 37", "Party level + 38", "Party level + 39",
            "Party level + 40", "Party level + 41", "Party level + 42", "Party level + 43", "Party level + 44", "Party level + 45", "Party level + 46", "Party level + 47", "Party level + 48", "Party level + 49",
            "Party level + 50", "Party level + 51", "Party level + 52", "Party level + 53", "Party level + 54", "Party level + 55", "Party level + 56", "Party level + 57", "Party level + 58", "Party level + 59",
            "Party level + 60", "Party level + 61", "Party level + 62", "Party level + 63", "Party level + 64", "Party level + 65", "Party level + 66", "Party level + 67", "Party level + 68", "Party level + 69",
            "Party level + 70", "Party level + 71", "Party level + 72", "Party level + 73", "Party level + 74", "Party level + 75", "Party level + 76", "Party level + 77", "Party level + 78", "Party level + 79",
            "Party level + 80", "Party level + 81", "Party level + 82", "Party level + 83", "Party level + 84", "Party level + 85", "Party level + 86", "Party level + 87", "Party level + 88", "Party level + 89",
            "Party level + 90", "Party level + 91", "Party level + 92", "Party level + 93", "Party level + 94", "Party level + 95", "Party level + 96", "Party level + 97", "Party level + 98", "Party level + 99",

            "Party level - Random"
        };
        private Context ourContext = Context.Default;
        private NumericUpDownWithDefault[] spinners;
        private static readonly string[] zeroTo100 = new string[] {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99",
            "100", "Random" };
        private static readonly string[] zeroTo31 = new string[] {
            "Unknown", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "Random" };

        private string[] byteNumberWithRandom = new string[256];

        private Dictionary<int, int> preReqJobIndexMap;

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

        public EventUnit EventUnit
        {
            get { return eventUnit; }
            set
            {
                SetEventUnit(value, ourContext);
            }
        }

        #endregion Public Properties 

        #region Constructors (1) 

        public EventUnitEditor()
        {
            InitializeComponent();
            /*
            spinners = new NumericUpDownWithDefault[] {
                paletteSpinner, xSpinner, ySpinner,
                unknown10Spinner, unknown11Spinner, unknown12Spinner, jobLevelSpinner,
                bonusMoneySpinner, unitIDSpinner, 
                targetXSpinner, targetYSpinner, unknown8Spinner, targetSpinner };
            */
            spinners = new NumericUpDownWithDefault[] {
                paletteSpinner, xSpinner, ySpinner,
                unknown10Spinner, unknown12Spinner, jobLevelSpinner,
                bonusMoneySpinner, unitIDSpinner,
                targetXSpinner, targetYSpinner, targetSpinner };
            comboBoxes = new ComboBoxWithDefault[] {
                spriteSetComboBox, specialNameComboBox, monthComboBox, jobComboBox,
                primarySkillComboBox, secondaryActionComboBox, reactionComboBox, supportComboBox, movementComboBox,
                rightHandComboBox, leftHandComboBox, headComboBox, bodyComboBox, accessoryComboBox,
                preRequisiteJobComboBox, facingDirectionComboBox, warTrophyComboBox, teamColorComboBox };

            foreach (NumericUpDownWithDefault spinner in spinners)
            {
                spinner.ValueChanged += spinner_ValueChanged;
            }
            foreach (ComboBoxWithDefault comboBox in comboBoxes)
            {
                comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            }

            braveryComboBox.SelectedIndexChanged += zeroTo99ComboBox_SelectedIndexChanged;
            faithComboBox.SelectedIndexChanged += zeroTo99ComboBox_SelectedIndexChanged;
            dayComboBox.SelectedIndexChanged += zeroTo31ComboBox_SelectedIndexChanged;
            levelComboBox.SelectedIndexChanged += levelComboBox_SelectedIndexChanged;
            experienceComboBox.SelectedIndexChanged += byteNumberWithRandomComboBox_SelectedIndexChanged;
            flags1CheckedListBox.ItemCheck += flagsCheckedListBox_ItemCheck;
            flags2CheckedListBox.ItemCheck += flagsCheckedListBox_ItemCheck;
            clbAIFlags1.ItemCheck += flagsCheckedListBox_ItemCheck;
            clbAIFlags2.ItemCheck += flagsCheckedListBox_ItemCheck;
            upperLevelCheckBox.CheckedChanged += upperLevelCheckBox_CheckedChanged;

            for (int index = 0; index < 256; index++)
            {
                byteNumberWithRandom[index] = (index == 254) ? "Random" : index.ToString();
            }
        }

        #endregion Constructors 

        #region Public Methods

        public void SetEventUnit(EventUnit value, Context context)
        {
            if (value == null)
            {
                eventUnit = null;
                Enabled = false;
            }
            else if (value != eventUnit)
            {
                eventUnit = value;
                UpdateView(context);
                Enabled = true;
            }
        }

        public void UpdateView(Context context)
        {
            ignoreChanges = true;

            if (ourContext != context)
            {
                ourContext = context;
                UpdateDataSources();
            }

            if (eventUnit.Default != null)
            {
                foreach (NumericUpDownWithDefault spinner in spinners)
                {
                    spinner.SetValueAndDefault(
                        ReflectionHelpers.GetFieldOrProperty<byte>(eventUnit, spinner.Tag.ToString()),
                        ReflectionHelpers.GetFieldOrProperty<byte>(eventUnit.Default, spinner.Tag.ToString()),
                        toolTip);
                }

                foreach (ComboBoxWithDefault comboBox in comboBoxes)
                {
                    if (comboBox.Name == "preRequisiteJobComboBox")
                    {
                        if (preReqJobIndexMap != null)
                        {
                            int index = preReqJobIndexMap[(int)eventUnit.PrerequisiteJob];
                            int defaultIndex = preReqJobIndexMap[(int)eventUnit.Default.PrerequisiteJob];
                            string defaultValue = ((KeyValuePair<int, string>)comboBox.Items[defaultIndex]).Value;
                            comboBox.SetIndexAndDefault(index, defaultIndex, toolTip);
                            toolTip.SetToolTip(comboBox, "Default: " + defaultValue);
                        }
                    }
                    else
                    {
                        comboBox.SetValueAndDefault(
                            ReflectionHelpers.GetFieldOrProperty<object>(eventUnit, comboBox.Tag.ToString()),
                            ReflectionHelpers.GetFieldOrProperty<object>(eventUnit.Default, comboBox.Tag.ToString()),
                            toolTip);
                    }
                }

                flags1CheckedListBox.SetValuesAndDefaults(
                    ReflectionHelpers.GetFieldsOrProperties<bool>(eventUnit, EventUnit.Flags1FieldNames),
                    ReflectionHelpers.GetFieldsOrProperties<bool>(eventUnit.Default, EventUnit.Flags1FieldNames));

                flags2CheckedListBox.SetValuesAndDefaults(
                    ReflectionHelpers.GetFieldsOrProperties<bool>(eventUnit, EventUnit.Flags2FieldNames),
                    ReflectionHelpers.GetFieldsOrProperties<bool>(eventUnit.Default, EventUnit.Flags2FieldNames));

                clbAIFlags1.SetValuesAndDefaults(
                    ReflectionHelpers.GetFieldsOrProperties<bool>(eventUnit, EventUnit.AIFlags1FieldNames),
                    ReflectionHelpers.GetFieldsOrProperties<bool>(eventUnit.Default, EventUnit.AIFlags1FieldNames)
                );

                clbAIFlags2.SetValuesAndDefaults(
                    ReflectionHelpers.GetFieldsOrProperties<bool>(eventUnit, EventUnit.AIFlags2FieldNames),
                    ReflectionHelpers.GetFieldsOrProperties<bool>(eventUnit.Default, EventUnit.AIFlags2FieldNames)
                );

                teamColorComboBox.SetValueAndDefault(
                    eventUnit.TeamColor,
                    eventUnit.Default.TeamColor,
                    toolTip);
                faithComboBox.SetValueAndDefault(
                    eventUnit.Faith == 254 ? zeroTo100[101] : zeroTo100[eventUnit.Faith],
                    eventUnit.Default.Faith == 254 ? zeroTo100[101] : zeroTo100[eventUnit.Default.Faith],
                    toolTip);
                braveryComboBox.SetValueAndDefault(
                    eventUnit.Bravery == 254 ? zeroTo100[101] : zeroTo100[eventUnit.Bravery],
                    eventUnit.Default.Bravery == 254 ? zeroTo100[101] : zeroTo100[eventUnit.Default.Bravery],
                    toolTip);
                dayComboBox.SetValueAndDefault(
                    eventUnit.Day == 254 ? zeroTo31[32] : zeroTo31[eventUnit.Day],
                    eventUnit.Default.Day == 254 ? zeroTo31[32] : zeroTo31[eventUnit.Default.Day],
                    toolTip);
                levelComboBox.SetValueAndDefault(
                    eventUnit.Level == 254 ? levelStrings[200] : levelStrings[eventUnit.Level],
                    eventUnit.Default.Level == 254 ? levelStrings[200] : levelStrings[eventUnit.Default.Level],
                    toolTip);
                experienceComboBox.SetValueAndDefault(
                    (eventUnit.Experience == 254) ? "Random" : eventUnit.Experience.ToString(),
                    (eventUnit.Default.Experience == 254) ? "Random" : eventUnit.Default.Experience.ToString(),
                    toolTip
                );

                upperLevelCheckBox.Checked = eventUnit.UpperLevel;
            }
            ignoreChanges = false;
        }

        #endregion Public Methods 

        #region Private Methods (8) 

        private void comboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!ignoreChanges)
            {
                ComboBoxWithDefault c = sender as ComboBoxWithDefault;

                if (c.Name == "preRequisiteJobComboBox")
                    ReflectionHelpers.SetFieldOrProperty(eventUnit, c.Tag.ToString(), (PreRequisiteJob)((KeyValuePair<int, string>)c.SelectedItem).Key);
                else
                    ReflectionHelpers.SetFieldOrProperty(eventUnit, c.Tag.ToString(), c.SelectedItem);

                OnDataChanged(this, EventArgs.Empty);
            }
        }

        private void flagsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!ignoreChanges)
            {
                if (sender == flags1CheckedListBox)
                    ReflectionHelpers.SetFieldOrProperty(eventUnit, EventUnit.Flags1FieldNames[e.Index], e.NewValue == CheckState.Checked);
                else if (sender == flags2CheckedListBox)
                    ReflectionHelpers.SetFieldOrProperty(eventUnit, EventUnit.Flags2FieldNames[e.Index], e.NewValue == CheckState.Checked);
                else if (sender == clbAIFlags1)
                    ReflectionHelpers.SetFieldOrProperty(eventUnit, EventUnit.AIFlags1FieldNames[e.Index], e.NewValue == CheckState.Checked);
                else if (sender == clbAIFlags2)
                    ReflectionHelpers.SetFieldOrProperty(eventUnit, EventUnit.AIFlags2FieldNames[e.Index], e.NewValue == CheckState.Checked);

                OnDataChanged(this, EventArgs.Empty);
            }
        }

        private void levelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                ComboBoxWithDefault combo = sender as ComboBoxWithDefault;
                ReflectionHelpers.SetFieldOrProperty(
                    eventUnit,
                    combo.Tag.ToString(),
                    (byte)(combo.SelectedIndex > 199 ? 254 : combo.SelectedIndex));
                OnDataChanged(this, EventArgs.Empty);
            }
        }

        private void byteNumberWithRandomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                ComboBoxWithDefault combo = sender as ComboBoxWithDefault;
                ReflectionHelpers.SetFieldOrProperty(
                    eventUnit,
                    combo.Tag.ToString(),
                    (byte)((combo.SelectedIndex == 254) ? 254 : combo.SelectedIndex));
                OnDataChanged(this, EventArgs.Empty);
            }
        }

        private void spinner_ValueChanged(object sender, System.EventArgs e)
        {
            if (!ignoreChanges)
            {
                NumericUpDownWithDefault c = sender as NumericUpDownWithDefault;
                ReflectionHelpers.SetFieldOrProperty(eventUnit, c.Tag.ToString(), (byte)c.Value);
                OnDataChanged(this, EventArgs.Empty);
            }
        }

        private void UpdateDataSources()
        {
            foreach (ComboBoxWithDefault itemComboBox in
                new ComboBoxWithDefault[] { rightHandComboBox, leftHandComboBox, headComboBox, bodyComboBox, accessoryComboBox, warTrophyComboBox })
            {
                itemComboBox.BindingContext = new BindingContext();
                itemComboBox.DataSource = Item.GetEventItems(ourContext);
            }

            primarySkillComboBox.BindingContext = new BindingContext();
            primarySkillComboBox.DataSource = new List<SkillSet>(SkillSet.GetEventSkillSets(ourContext).Values);
            secondaryActionComboBox.BindingContext = new BindingContext();
            secondaryActionComboBox.DataSource = new List<SkillSet>(SkillSet.GetEventSkillSets(ourContext).Values);
            foreach (ComboBoxWithDefault abilityComboBox in
                new ComboBoxWithDefault[] { reactionComboBox, supportComboBox, movementComboBox })
            {
                abilityComboBox.BindingContext = new BindingContext();
                abilityComboBox.DataSource = AllAbilities.GetEventAbilities(ourContext);
            }

            faithComboBox.BindingContext = new BindingContext();
            faithComboBox.DataSource = zeroTo100;
            braveryComboBox.BindingContext = new BindingContext();
            braveryComboBox.DataSource = zeroTo100;
            dayComboBox.DataSource = zeroTo31;
            levelComboBox.DataSource = levelStrings;
            experienceComboBox.DataSource = byteNumberWithRandom;

            spriteSetComboBox.DataSource = SpriteSet.GetSpriteSets(ourContext);
            specialNameComboBox.DataSource = SpecialName.GetSpecialNames(ourContext);
            jobComboBox.DataSource = AllJobs.GetDummyJobs(ourContext);
            monthComboBox.DataSource = Enum.GetValues(typeof(Month));
            teamColorComboBox.DataSource = Enum.GetValues(typeof(TeamColor));
            facingDirectionComboBox.DataSource = Enum.GetValues(typeof(Facing));

            //preRequisiteJobComboBox.DataSource = Enum.GetValues( typeof( PreRequisiteJob ) );
            List<KeyValuePair<int, string>> preReqJobDataSource = GetPreReqJobDataSource();
            preReqJobIndexMap = GetPreReqJobIndexMap(preReqJobDataSource);
            preRequisiteJobComboBox.DataSource = GetPreReqJobDataSource();
            preRequisiteJobComboBox.ValueMember = "Key";
            preRequisiteJobComboBox.DisplayMember = "Value";

            IDictionary<string, string> unusedLabels = (ourContext == Context.US_PSP) ? PSPResources.Labels.ENTDUnused : PSXResources.Labels.ENTDUnused;

            LabelUtility.SetControlLabelFromMap(unusedLabels, ResourcesLabels.ENTDUnused.SectionLabel, unknownGroupBox);
            LabelUtility.SetControlLabelFromMap(unusedLabels, ResourcesLabels.ENTDUnused.FirstLabel, lblUnknown10);
            LabelUtility.SetControlLabelFromMap(unusedLabels, ResourcesLabels.ENTDUnused.SecondLabel, lblUnknown12);
        }

        private List<KeyValuePair<int, string>> GetPreReqJobDataSource()
        {
            List<KeyValuePair<int, string>> preReqJobs = new List<KeyValuePair<int, string>>();
            preReqJobs.Add(new KeyValuePair<int, string>(0, "Base"));

            Job[] jobs = AllJobs.GetDummyJobs(ourContext);
            for (int index = 0x4B; index < 0x5E; index++)
            {
                string name = (jobs[index] != null) ? jobs[index].Name : index.ToString();
                preReqJobs.Add(new KeyValuePair<int, string>(index - 0x4A, name));
            }

            Job[] pspJobs = (ourContext == Context.US_PSP) ? jobs : AllJobs.GetDummyJobs(Context.US_PSP);
            for (int index = 0xA0; index < 0xA2; index++)
            {
                string name = (pspJobs[index] != null) ? pspJobs[index].Name : index.ToString();
                preReqJobs.Add(new KeyValuePair<int, string>(index - 0x8C, name));
            }

            preReqJobs.Add(new KeyValuePair<int, string>(0xA9, "Unknown"));

            return preReqJobs;
        }

        private Dictionary<int, int> GetPreReqJobIndexMap(List<KeyValuePair<int, string>> dataSource)
        {
            Dictionary<int, int> indexMap = new Dictionary<int, int>();
            for (int index = 0; index < dataSource.Count; index++)
            {
                indexMap.Add(dataSource[index].Key, index);
            }

            return indexMap;
        }

        private void upperLevelCheckBox_CheckedChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                eventUnit.UpperLevel = upperLevelCheckBox.Checked;
                OnDataChanged( this, EventArgs.Empty );
            }
        }

        private void zeroTo31ComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                ComboBoxWithDefault combo = sender as ComboBoxWithDefault;
                ReflectionHelpers.SetFieldOrProperty(
                    eventUnit,
                    combo.Tag.ToString(),
                    (byte)(combo.SelectedIndex > 31 ? 254 : combo.SelectedIndex) );
                OnDataChanged( this, EventArgs.Empty );
            }
        }

        private void zeroTo99ComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if( !ignoreChanges )
            {
                ComboBoxWithDefault combo = sender as ComboBoxWithDefault;
                ReflectionHelpers.SetFieldOrProperty(
                    eventUnit,
                    combo.Tag.ToString(),
                    (byte)(combo.SelectedIndex > 100 ? 254 : combo.SelectedIndex) );
                OnDataChanged( this, EventArgs.Empty );
            }
        }

		#endregion Private Methods 
    }
}
