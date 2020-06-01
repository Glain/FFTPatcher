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

namespace FFTPatcher.Editors
{
    public partial class JobLevelsEditor : BaseEditor
    {
		#region Instance Variables

        private JobLevels levels;
        Label[] labels;
        VerticalLabel[] verticalLabels;

		#endregion Instance Variables 

        private ToolTip toolTip;
        public ToolTip ToolTip
        {
            set
            {
                toolTip = value;
            }
        }

		#region Constructors (1) 

        public JobLevelsEditor()
        {
            InitializeComponent();
            requirementsEditor1.DataChanged += OnDataChanged;
            labels = new Label[19] { 
                chemistLabel, knightLabel, archerLabel, monkLabel, 
                whiteLabel, blackLabel, timeLabel, summonerLabel, 
                thiefLabel, oratorLabel, mysticLabel, geomancerLabel, 
                dragoonLabel, samuraiLabel, ninjaLabel, calcLabel, 
                bardLabel, dancerLabel, mimeLabel };
            verticalLabels = new VerticalLabel[20] {
                verticalSquireLabel, verticalChemistLabel, verticalKnightLabel, 
                verticalArcherLabel, verticalMonkLabel, verticalWhiteLabel, 
                verticalBlackLabel, verticalTimeLabel, verticalSummonerLabel,
                verticalThiefLabel, verticalOratorLabel, verticalMysticLabel, 
                verticalGeomancerLabel, verticalDragoonLabel, 
                verticalSamuraiLabel, verticalNinjaLabel, verticalCalcLabel,
                verticalBardLabel, verticalDancerLabel, verticalMimeLabel };
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public void UpdateView( JobLevels levels, Context context )
        {
            this.levels = levels;
            foreach( Control c in Controls )
            {
                if( c is NumericUpDownWithDefault )
                {
                    NumericUpDownWithDefault spinner = c as NumericUpDownWithDefault;
                    spinner.ValueChanged -= spinner_ValueChanged;
                    spinner.SetValueAndDefault(
                        ReflectionHelpers.GetFieldOrProperty<UInt16>( levels, spinner.Tag.ToString() ),
                        ReflectionHelpers.GetFieldOrProperty<UInt16>( levels.Default, spinner.Tag.ToString() ),
                        toolTip);
                    spinner.ValueChanged += spinner_ValueChanged;
                }
            }

            List<Requirements> reqs = new List<Requirements>( new Requirements[] {
                levels.Chemist, levels.Knight, levels.Archer, levels.Monk,
                levels.WhiteMage, levels.BlackMage, levels.TimeMage, levels.Summoner,
                levels.Thief, levels.Orator, levels.Mystic, levels.Geomancer,
                levels.Dragoon, levels.Samurai, levels.Ninja, levels.Arithmetician,
                levels.Bard, levels.Dancer, levels.Mime } );
            if( context == Context.US_PSP )
            {
                reqs.Add( levels.DarkKnight );
                reqs.Add( levels.OnionKnight );
                reqs.Add( levels.Unknown );
            }
            IList<string> names = context == Context.US_PSP ? PSPResources.Lists.JobNames : PSXResources.Lists.JobNames;
            IList<string> sideNames = names.Sub( 0x4B, 0x5D );
            IList<string> topNames = names.Sub( 0x4A, 0x5D );
            for ( int i = 0; i < sideNames.Count; i++ )
            {
                labels[i].Text = sideNames[i];
                verticalLabels[i].Text = topNames[i];
            }
            verticalLabels[topNames.Count - 1].Text = topNames[topNames.Count - 1];

            bool psp = context == Context.US_PSP;
            if ( psp )
            {
                darkKnightLabel.Text = names[0xA0];
                darkKnightVerticalLabel.Text = names[0xA0];
                onionKnightLabel.Text = names[0xA4];
                onionKnightVerticalLabel.Text = names[0xA4];
            }
            darkKnightLabel.Visible = psp;
            darkKnightVerticalLabel.Visible = psp;
            unknown1VerticalLabel.Visible = psp;
            unknown2VerticalLabel.Visible = psp;
            unknownLabel.Visible = psp;
            onionKnightLabel.Visible = psp;
            onionKnightVerticalLabel.Visible = psp;

            //requirementsEditor1.Requirements = reqs;
            requirementsEditor1.SetRequirements(reqs, context);
        }

		#endregion Public Methods 

		#region Private Methods (1) 

        private void spinner_ValueChanged( object sender, EventArgs e )
        {
            NumericUpDownWithDefault spinner = sender as NumericUpDownWithDefault;
            ReflectionHelpers.SetFieldOrProperty( levels, spinner.Tag.ToString(), (UInt16)spinner.Value );
            OnDataChanged( this, System.EventArgs.Empty );
        }

		#endregion Private Methods 
    }
}
