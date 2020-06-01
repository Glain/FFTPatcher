using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FFTPatcher.Controls;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class AdditionalRewardsEditor : UserControl
    {
        private bool ignoreChanges = false;
        private AllPropositions props;
        private ComboBoxWithDefault[] bonusCashGilComboBoxes = null;
        private ComboBoxWithDefault[] bonusCashJpComboBoxes = null;
        private ComboBoxWithDefault[] treasureLandGilComboBoxes = null;
        private ComboBoxWithDefault[] treasureLandJpComboBoxes = null;
        private List<ComboBoxWithDefault> jpComboBoxes = null;
        private List<ComboBoxWithDefault> gilComboBoxes = null;

        private List<ComboBoxWithDefault> allComboBoxes = null;

        private ToolTip toolTip;
        public ToolTip ToolTip
        {
            set
            {
                toolTip = value;
            }
        }

        public void UpdateView( AllPropositions props )
        {
            this.props = props;
            RefreshDataSources();
            UpdateAllComboBoxes();
        }

        private void UpdateAllComboBoxes()
        {
            SetValuesTreasureLandGilComboBoxes();
            SetValuesTreasureLandJpComboBoxes();
            SetValuesBonusCashJpComboBoxes();
            SetValuesBonusCashGilComboBoxes();
        }

        private void SetValuesBonusCashGilComboBoxes()
        {
            SetValuesBonusCashComboBoxes(
                props.BonusCashGilBonuses,
                props.Default.BonusCashGilBonuses,
                bonusCashGilComboBoxes );
        }

        private void SetValuesBonusCashJpComboBoxes()
        {
            SetValuesBonusCashComboBoxes( 
                props.BonusCashJpBonuses, 
                props.Default.BonusCashJpBonuses,
                bonusCashJpComboBoxes );
        }

        private void SetValuesBonusCashComboBoxes(
            TupleDictionary<PropositionType, BonusPercent, BonusIndex> dict, 
            TupleDictionary<PropositionType, BonusPercent, BonusIndex> def, 
            IList<ComboBoxWithDefault> boxes )
        {
            foreach (var box in boxes)
            {
                string tag = box.Tag.ToString();

                string[] sides = tag.Split( '.' );
                string errandType = sides[0];
                string percentage = sides[1];

                PropositionType type = (PropositionType)Enum.Parse( typeof( PropositionType ), errandType );
                BonusPercent percent = (BonusPercent)Enum.Parse( typeof( BonusPercent ), percentage );

                box.SetIndexAndDefault((int)dict[type, percent], (int)def[type, percent], toolTip);
            }
        }

        private void SetValuesTreasureLandGilComboBoxes()
        {
            SetValuesTreasureLandComboBoxes( 
                props.TreasureLandGilBonuses, 
                props.Default.TreasureLandGilBonuses,
                treasureLandGilComboBoxes );
        }

        private void SetValuesTreasureLandJpComboBoxes()
        {
            SetValuesTreasureLandComboBoxes( 
                props.TreasureLandJpBonuses, 
                props.Default.TreasureLandJpBonuses,
                treasureLandJpComboBoxes );
        }

        private void SetValuesTreasureLandComboBoxes(
            IDictionary<PropositionType, BonusIndex> dict,
            IDictionary<PropositionType, BonusIndex> def,
            IList<ComboBoxWithDefault> boxes )
        {
            foreach (var box in boxes)
            {
                string tag = box.Tag.ToString();

                PropositionType type = (PropositionType)Enum.Parse( typeof( PropositionType ), tag );

                box.SetIndexAndDefault((int)dict[type], (int)def[type], toolTip);
            }
        }

        public AdditionalRewardsEditor()
        {
            InitializeComponent();
            treasureLandJpComboBoxes = new ComboBoxWithDefault[] {
                jpTreasureCombatComboBox, jpTreasureContestComboBox, jpTreasureExplorationComboBox, jpTreasureInvestigation1ComboBox,
                jpTreasureInvestigation2ComboBox, jpTreasureMiningComboBox, jpTreasureOddJobsComboBox, jpTreasureSalvageComboBox
            };
            treasureLandGilComboBoxes = new ComboBoxWithDefault[] {
                gilTreasureCombatComboBox, gilTreasureContestComboBox, gilTreasureExplorationComboBox, gilTreasureInvestigation1ComboBox,
                gilTreasureInvestigation2ComboBox, gilTreasureMiningComboBox, gilTreasureOddJobsComboBox, gilTreasureSalvageComboBox,
            };

            bonusCashJpComboBoxes = new ComboBoxWithDefault[] {
                jpGil10CombatComboBox, jpGil10ContestComboBox, jpGil10ExplorationComboBox, jpGil10Investigation1ComboBox,
                jpGil10Investigation2ComboBox, jpGil10MiningComboBox, jpGil10OddJobsComboBox, jpGil10SalvageComboBox,
                jpGil40CombatComboBox, jpGil40ContestComboBox, jpGil40ExplorationComboBox, jpGil40Investigation1ComboBox,
                jpGil40Investigation2ComboBox, jpGil40MiningComboBox, jpGil40OddJobsComboBox, jpGil40SalvageComboBox,
                jpGil50CombatComboBox, jpGil50ContestComboBox, jpGil50ExplorationComboBox, jpGil50Investigation1ComboBox,
                jpGil50Investigation2ComboBox, jpGil50MiningComboBox, jpGil50OddJobsComboBox, jpGil50SalvageComboBox
            };
            bonusCashGilComboBoxes = new ComboBoxWithDefault[] {
                gilGil10CombatComboBox, gilGil10ContestComboBox, gilGil10ExplorationComboBox, gilGil10Investigation1ComboBox,
                gilGil10Investigation2ComboBox, gilGil10MiningComboBox, gilGil10OddJobsComboBox, gilGil10SalvageComboBox,
                gilGil40CombatComboBox, gilGil40ContestComboBox, gilGil40ExplorationComboBox, gilGil40Investigation1ComboBox,
                gilGil40Investigation2ComboBox, gilGil40MiningComboBox, gilGil40OddJobsComboBox, gilGil40SalvageComboBox,
                gilGil50CombatComboBox, gilGil50ContestComboBox, gilGil50ExplorationComboBox, gilGil50Investigation1ComboBox,
                gilGil50Investigation2ComboBox, gilGil50MiningComboBox, gilGil50OddJobsComboBox, gilGil50SalvageComboBox,
            };

            allComboBoxes = new List<ComboBoxWithDefault>();
            allComboBoxes.AddRange( treasureLandJpComboBoxes );
            allComboBoxes.AddRange( treasureLandGilComboBoxes );
            allComboBoxes.AddRange( bonusCashGilComboBoxes );
            allComboBoxes.AddRange( bonusCashJpComboBoxes );

            jpComboBoxes = new List<ComboBoxWithDefault>();
            jpComboBoxes.AddRange( treasureLandJpComboBoxes );
            jpComboBoxes.AddRange( bonusCashJpComboBoxes );

            gilComboBoxes = new List<ComboBoxWithDefault>();
            gilComboBoxes.AddRange( treasureLandGilComboBoxes );
            gilComboBoxes.AddRange( bonusCashGilComboBoxes );

        }

        private void RefreshDataSources()
        {
            UpdateJpComboBoxDataSources();
            UpdateGilComboBoxDataSources();
        }

        private void UpdateJpComboBoxDataSources()
        {
            UpdateComboBoxDataSources( jpComboBoxes, props.SmallBonuses );
        }

        private void UpdateGilComboBoxDataSources()
        {
            UpdateComboBoxDataSources( gilComboBoxes, props.LargeBonuses );
        }

        private void UpdateComboBoxDataSources( IList<ComboBoxWithDefault> boxes, IList<UInt16> values )
        {
            ignoreChanges = true;
            foreach (var box in boxes)
            {
                int index = box.SelectedIndex;
                box.BeginUpdate();
                box.Items.Clear();
                box.Items.Add( 0 );
                foreach (var val in values)
                {
                    box.Items.Add( val );
                }
                box.EndUpdate();
                box.SelectedIndex = index;
            }
            ignoreChanges = false;
        }

        private void BonusCashGilSelectedIndexChanged( object sender, EventArgs e )
        {
            ComboBoxWithDefault box = sender as ComboBoxWithDefault;
            if (box != null && !ignoreChanges)
            {
                UpdateBonusCashDict( box, props.BonusCashGilBonuses );
            }
        }
        private void BonusCashJpSelectedIndexChanged( object sender, EventArgs e )
        {
            ComboBoxWithDefault box = sender as ComboBoxWithDefault;
            if (box != null && !ignoreChanges)
            {
                UpdateBonusCashDict( box, props.BonusCashJpBonuses );
            }
        }

        protected override void OnVisibleChanged( EventArgs e )
        {
            if (props != null)
                RefreshDataSources();
            base.OnVisibleChanged( e );
        }

        private void UpdateBonusCashDict( ComboBoxWithDefault sender, TupleDictionary<PropositionType, BonusPercent, BonusIndex> dict )
        {
            string changed = sender.Tag.ToString();

            string[] sides = changed.Split( '.' );
            string errandType = sides[0];
            string percentage = sides[1];

            BonusPercent percent = (BonusPercent)Enum.Parse( typeof( BonusPercent ), percentage );
            PropositionType type = (PropositionType)Enum.Parse( typeof( PropositionType ), errandType );

            dict[type, percent] = (BonusIndex)(sender.SelectedIndex);
        }

        private void TreasureLandGilSelectedIndexChanged( object sender, EventArgs e )
        {
            ComboBoxWithDefault box = sender as ComboBoxWithDefault;
            if (box != null && !ignoreChanges)
            {
                UpdateTreasureLandDict( box, props.TreasureLandGilBonuses );
            }
        }

        private void TreasureLandJpSelectedIndexChanged( object sender, EventArgs e )
        {
            ComboBoxWithDefault box = sender as ComboBoxWithDefault;
            if (box != null && !ignoreChanges)
            {
                UpdateTreasureLandDict( box, props.TreasureLandJpBonuses );
            }
        }

        private void UpdateTreasureLandDict( ComboBoxWithDefault sender, IDictionary<PropositionType, BonusIndex> dict )
        {
            string changed = sender.Tag.ToString();
            PropositionType type = (PropositionType)Enum.Parse( typeof( PropositionType ), changed );
            dict[type] = (BonusIndex)(sender.SelectedIndex);
        }


    }
}
