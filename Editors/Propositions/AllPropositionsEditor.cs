using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FFTPatcher.Controls;
using PatcherLib.Utilities;
using FFTPatcher.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class AllPropositionsEditor : UserControl
    {
        private NumericUpDownWithDefault[] jpMultiplierSpinners;
        private NumericUpDownWithDefault[] gilMultiplierSpinners;
        private NumericUpDownWithDefault[] gilBonusSpinners;
        private NumericUpDownWithDefault[] jpBonusSpinners;

        private ToolTip toolTip;
        public ToolTip ToolTip 
        {
            set
            {
                toolTip = value;
                allPropositionDetailsEditor1.ToolTip = value;
                additionalRewardsEditor1.ToolTip = value;
            }
        }

        public AllPropositionsEditor()
        {
            InitializeComponent();

            jpBonusSpinners = new NumericUpDownWithDefault[] {
                jpReward1, 
                jpReward2, 
                jpReward3, 
                jpReward4, 
                jpReward5, 
                jpReward6, 
                jpReward7, 
                jpReward8 };
            gilBonusSpinners = new NumericUpDownWithDefault[] {
                gilReward1, 
                gilReward2, 
                gilReward3, 
                gilReward4, 
                gilReward5, 
                gilReward6, 
                gilReward7, 
                gilReward8 };
            jpMultiplierSpinners = new NumericUpDownWithDefault[] {
                jpMultiplier0, 
                jpMultiplier1, 
                jpMultiplier2, 
                jpMultiplier3, 
                jpMultiplier4, 
                jpMultiplier5, 
                jpMultiplier6, 
                jpMultiplier7, 
                jpMultiplier8, 
                jpMultiplier9 };
            gilMultiplierSpinners = new NumericUpDownWithDefault[] {
                gilMultiplier0, 
                gilMultiplier1, 
                gilMultiplier2, 
                gilMultiplier3, 
                gilMultiplier4, 
                gilMultiplier5, 
                gilMultiplier6, 
                gilMultiplier7, 
                gilMultiplier8, 
                gilMultiplier9 };

            jpBonusSpinners.ForEach( s => s.ValueChanged += new EventHandler( jpBonusSpinners_ValueChanged ) );
            jpMultiplierSpinners.ForEach( s => s.ValueChanged += new EventHandler( jpMultipliersSpinners_ValueChanged ) );
            gilBonusSpinners.ForEach( s => s.ValueChanged += new EventHandler( gilBonusSpinners_ValueChanged ) );
            gilMultiplierSpinners.ForEach( s => s.ValueChanged += new EventHandler( gilMultipliersSpinners_ValueChanged ) );
        }

        bool ignoreChanges = false;

        public void UpdateView( AllPropositions allProps, PatcherLib.Datatypes.Context context )
        {
            ignoreChanges = true;
            props = allProps;
            for (int i = 0; i < 8; i++)
            {
                jpBonusSpinners[i].SetValueAndDefault( allProps.SmallBonuses[i], allProps.Default.SmallBonuses[i], toolTip );
                gilBonusSpinners[i].SetValueAndDefault( allProps.LargeBonuses[i], allProps.Default.LargeBonuses[i], toolTip );
            }
            for (int i = 0; i < 10; i++)
            {
                jpMultiplierSpinners[i].SetValueAndDefault( allProps.JPMultipliers[i], allProps.Default.JPMultipliers[i], toolTip );
                gilMultiplierSpinners[i].SetValueAndDefault( allProps.GilMultipliers[i], allProps.Default.GilMultipliers[i], toolTip );
            }
            classBonusesEditor1.UpdateView( allProps, context );
            statLevelBonusesEditor1.UpdateView( allProps, context );
            additionalRewardsEditor1.UpdateView( allProps );
            allPropositionDetailsEditor1.UpdateView(allProps, context);
            ignoreChanges = false;
        }

        private AllPropositions props;

        void jpBonusSpinners_ValueChanged( object sender, EventArgs e )
        {
            HandleValueChanged( sender as NumericUpDownWithDefault, props.SmallBonuses );
        }

        void jpMultipliersSpinners_ValueChanged( object sender, EventArgs e )
        {
            HandleValueChanged( sender as NumericUpDownWithDefault, props.JPMultipliers );
        }

        void gilBonusSpinners_ValueChanged( object sender, EventArgs e )
        {
            HandleValueChanged( sender as NumericUpDownWithDefault, props.LargeBonuses );
        }

        void gilMultipliersSpinners_ValueChanged( object sender, EventArgs e )
        {
            HandleValueChanged( sender as NumericUpDownWithDefault, props.GilMultipliers );
        }

        private void HandleValueChanged( NumericUpDownWithDefault spinner, IList<UInt16> valuesToChange )
        {
            if (!ignoreChanges)
            {
                int i = int.Parse( (string)(spinner.Tag) );
                valuesToChange[i] = (ushort)spinner.Value;
            }
        }

    }
}
