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
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class FFTPatchEditor : UserControl
    {
		#region Static Properties 

        public static ToolTip ToolTip { get; private set; }

		#endregion

		#region Constructors

        public FFTPatchEditor()
        {
            InitializeComponent();
            this.Enabled = false;

            allAbilitiesEditor1.InflictStatusClicked += InflictStatusClicked;
            allItemsEditor1.InflictStatusClicked += InflictStatusClicked;
            allItemsEditor1.ItemAttributesClicked += ItemAttributesClicked;
            allJobsEditor1.SkillSetClicked += SkillSetClicked;

            allItemAttributesEditor1.ItemClicked += ItemClicked;
            allInflictStatusesEditor1.AbilityClicked += AbilityClicked;
            allInflictStatusesEditor1.ItemClicked += ItemClicked;

            allJobsEditor1.ViewStatsClicked += ViewStatsClicked;

            tabControl.Selected += tabControl_Selected;

            allInflictStatusesEditor1.RepointHandler += OnInflictStatusRepoint;
            allItemAttributesEditor1.RepointHandler += OnItemAttributesRepoint;
        }

        static FFTPatchEditor()
        {
            ToolTip = new ToolTip();
        }

		#endregion Constructors 

        private FFTPatch fftPatch;

		#region Methods

        public void LoadFFTPatch(FFTPatch FFTPatch)
        {
            this.Enabled = true;

            PatchUtility.BuildReferenceList(FFTPatch.ItemAttributes, FFTPatch.InflictStatuses, FFTPatch.Abilities, FFTPatch.Items);

            allAbilitiesEditor1.UpdateView( FFTPatch.Abilities, FFTPatch.InflictStatuses, FFTPatch.Context );
            allActionMenusEditor1.UpdateView( FFTPatch.ActionMenus );
            allInflictStatusesEditor1.UpdateView(FFTPatch.InflictStatuses, FFTPatch.Context);
            allItemAttributesEditor1.UpdateView( FFTPatch.ItemAttributes, FFTPatch.Context );
            allItemsEditor1.UpdateView( FFTPatch.Items, FFTPatch.StoreInventories, FFTPatch.InflictStatuses, FFTPatch.ItemAttributes, FFTPatch.Context );
            allJobsEditor1.UpdateView( FFTPatch.Jobs, FFTPatch.Context );
            allMonsterSkillsEditor1.UpdateView( FFTPatch.MonsterSkills, FFTPatch.Context );
            allPoachProbabilitiesEditor1.UpdateView( FFTPatch.PoachProbabilities, FFTPatch.Context );
            allSkillSetsEditor1.UpdateView( FFTPatch.SkillSets, FFTPatch.Context );
            allStatusAttributesEditor1.UpdateView(FFTPatch.StatusAttributes, FFTPatch.Context);
            jobLevelsEditor1.UpdateView( FFTPatch.JobLevels, FFTPatch.Context );
            entdEditor1.UpdateView( FFTPatch.ENTDs, FFTPatch.Context );
            allMoveFindItemsEditor1.UpdateView(FFTPatch.MoveFind, FFTPatch.Context);
            allStoreInventoryEditor1.UpdateView( FFTPatch.StoreInventories, FFTPatch.Items, FFTPatch.Context );
            allAnimationsEditor1.UpdateView(FFTPatch.AbilityAnimations);
            allPropositionsEditor1.UpdateView( FFTPatch.Propositions, FFTPatch.Context );

            codeCreator1.UpdateView(FFTPatch);
            codesTab.Text = FFTPatch.Context == Context.US_PSP ? "CWCheat" : "Gameshark";
            propositionsTabPage.Text = FFTPatch.Context == Context.US_PSP ? "Errands" : "Propositions";

            fftPatch = FFTPatch;
        }

        private void InflictStatusClicked( object sender, LabelClickedEventArgs e )
        {
            allInflictStatusesEditor1.SelectedIndex = e.Value;
            tabControl.SelectedTab = inflictStatusesTabPage;
        }

        private void ItemAttributesClicked( object sender, LabelClickedEventArgs e )
        {
            allItemAttributesEditor1.SelectedIndex = e.Value;
            tabControl.SelectedTab = itemAttributesTabPage;
        }

        private void SkillSetClicked( object sender, LabelClickedEventArgs e )
        {
            if( e.Value >= 0xE0 )
            {
                allSkillSetsEditor1.SelectedIndex = e.Value - 0xE0 + 0xB0;
                tabControl.SelectedTab = skillSetsPage;
            }
            else if( e.Value >= 0xB0 )
            {
                allMonsterSkillsEditor1.SelectedIndex = e.Value - 0xB0;
                tabControl.SelectedTab = monsterSkillsTab;
            }
            else
            {
                allSkillSetsEditor1.SelectedIndex = e.Value;
                tabControl.SelectedTab = skillSetsPage;
            }
        }

        private void AbilityClicked(object sender, ReferenceEventArgs e)
        {
            if (e.ReferencingIndexes != null)
            {
                allAbilitiesEditor1.SetListBoxHighlightedIndexes(e.ReferencingIndexes);
            }

            allAbilitiesEditor1.SelectedIndex = e.Index;
            allAbilitiesEditor1.UpdateListBox();
            tabControl.SelectedTab = abilitiesPage;
        }

        private void ItemClicked(object sender, ReferenceEventArgs e)
        {
            if (e.ReferencingIndexes != null)
            {
                allItemsEditor1.SetListBoxHighlightedIndexes(e.ReferencingIndexes);
            }

            allItemsEditor1.SelectedIndex = e.Index;
            allItemsEditor1.UpdateListBox();
            tabControl.SelectedTab = itemsTabPage;
        }

        private void ViewStatsClicked(object sender, ReferenceEventArgs e)
        {
            int jobID = e.Index;

            ViewStatForm viewStatForm = new ViewStatForm(fftPatch, jobID);
            viewStatForm.SaveToJobClicked += SaveToJobClicked;
            viewStatForm.Show();
        }

        private void SaveToJobClicked(object sender, ReferenceEventArgs e)
        {
            int jobID = e.Index;

            if (jobID < fftPatch.Jobs.Jobs.Length)
            {
                allJobsEditor1.SelectedIndex = jobID;
                allJobsEditor1.UpdateSelectedEntry();
                allJobsEditor1.UpdateListBox();
            }
        }

        public void ConsolidateInflictStatuses()
        {
            PatchUtility.RepointInflictStatus(fftPatch.Items, fftPatch.Abilities, fftPatch.InflictStatuses);
            allInflictStatusesEditor1.UpdateSelectedEntry();
            allInflictStatusesEditor1.UpdateListBox();
            allItemsEditor1.UpdateSelectedEntry();
            allItemsEditor1.UpdateListBox();
            allAbilitiesEditor1.UpdateSelectedEntry();
            allAbilitiesEditor1.UpdateListBox();
        }

        public void ConsolidateItemAttributes()
        {
            PatchUtility.RepointItemAttributes(fftPatch.Items, fftPatch.ItemAttributes);
            allItemAttributesEditor1.UpdateSelectedEntry();
            allItemAttributesEditor1.UpdateListBox();
            allItemsEditor1.UpdateSelectedEntry();
            allItemsEditor1.UpdateListBox();
        }

		#endregion Methods 

        void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == inflictStatusesTabPage)
            {
                allInflictStatusesEditor1.UpdateListBox();
            }
            else if (e.TabPage == itemAttributesTabPage)
            {
                allItemAttributesEditor1.UpdateListBox();
            }
        }

        private void OnInflictStatusRepoint(object sender, RepointEventArgs e)
        {
            if (fftPatch != null)
            {
                PatchUtility.RepointSpecificInflictStatus(fftPatch.Items, fftPatch.Abilities, fftPatch.InflictStatuses, (byte)e.OldID, (byte)e.NewID);
                allInflictStatusesEditor1.UpdateSelectedEntry();
                allInflictStatusesEditor1.UpdateListBox();
                allItemsEditor1.UpdateSelectedEntry();
                allItemsEditor1.UpdateListBox();
                allAbilitiesEditor1.UpdateSelectedEntry();
                allAbilitiesEditor1.UpdateListBox();
            }
        }

        private void OnItemAttributesRepoint(object sender, RepointEventArgs e)
        {
            if (fftPatch != null)
            {
                PatchUtility.RepointSpecificItemAttributes(fftPatch.Items, fftPatch.ItemAttributes, (byte)e.OldID, (byte)e.NewID);
                allItemAttributesEditor1.UpdateSelectedEntry();
                allItemAttributesEditor1.UpdateListBox();
                allItemsEditor1.UpdateSelectedEntry();
                allItemsEditor1.UpdateListBox();
            }
        }
    }
}
