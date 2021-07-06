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
        private readonly Label[] labels;
        private readonly VerticalLabel[] verticalLabels;
        private readonly Label[] pspExtraLabels;
        private readonly VerticalLabel[] pspExtraVerticalLabels;
        private readonly System.Drawing.Font font;
        private readonly System.Drawing.Font fontBold;
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
            requirementsEditor1.SelectionChangedEvent += RequirementsEditor1_SelectionChangedEvent;
            font = chemistLabel.Font;
            fontBold = new System.Drawing.Font(font, System.Drawing.FontStyle.Bold);

            labels = new Label[19] { 
                chemistLabel, knightLabel, archerLabel, monkLabel, 
                whiteLabel, blackLabel, timeLabel, summonerLabel, 
                thiefLabel, oratorLabel, mysticLabel, geomancerLabel, 
                dragoonLabel, samuraiLabel, ninjaLabel, calcLabel, 
                bardLabel, dancerLabel, mimeLabel };
            pspExtraLabels = new Label[]
            {
                darkKnightLabel,
                onionKnightLabel,
                unknownLabel
            };
            verticalLabels = new VerticalLabel[20] {
                verticalSquireLabel, verticalChemistLabel, verticalKnightLabel, 
                verticalArcherLabel, verticalMonkLabel, verticalWhiteLabel, 
                verticalBlackLabel, verticalTimeLabel, verticalSummonerLabel,
                verticalThiefLabel, verticalOratorLabel, verticalMysticLabel, 
                verticalGeomancerLabel, verticalDragoonLabel, 
                verticalSamuraiLabel, verticalNinjaLabel, verticalCalcLabel,
                verticalBardLabel, verticalDancerLabel, verticalMimeLabel };
            pspExtraVerticalLabels = new VerticalLabel[]
            {
                darkKnightVerticalLabel,
                onionKnightVerticalLabel,
                unknown1VerticalLabel,
                unknown2VerticalLabel
            };
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
            // Clear any bolded labels
            foreach( var label in labels )
			{
                if( label.Font.Bold )
                    label.Font = font;
            }
            foreach( var label in verticalLabels )
            {
                if( label.Font.Bold )
                    label.Font = font;
            }
            foreach( var label in pspExtraLabels)
            {
                if( label.Font.Bold )
                    label.Font = font;
            }
            foreach( var label in pspExtraVerticalLabels )
            {
                if( label.Font.Bold )
                    label.Font = font;
            }

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

        #region Private Methods (2) 

        private void RequirementsEditor1_SelectionChangedEvent(object sender, EventArgs e)
        {
            var grid = sender as DataGridView;
            if( grid != null && grid.RowCount > 1 && grid.SelectedCells.Count == 1 )
            {
                requirementsEditor1.DisableRedraw();
                var selectedCell = grid.SelectedCells[0];
                // Highlight the selected cell's row and column up to it.
                for( int row = 0; row < grid.RowCount; row++ )
                {
                    for( int col = 0; col < grid.ColumnCount; col++ )
                    {
                        var cell = grid[col, row];
                        if( row == selectedCell.RowIndex && col < selectedCell.ColumnIndex )
                            cell.Style.BackColor = System.Drawing.Color.LightGray;
                        else if( col == selectedCell.ColumnIndex && row < selectedCell.RowIndex )
                            cell.Style.BackColor = System.Drawing.Color.LightGray;
                        else if( cell.InheritedStyle.BackColor != System.Drawing.Color.White )
                            cell.Style.BackColor = System.Drawing.Color.White;
                    }
                }

                // Set the vertical label(column label) to bold based on the selection
                int labelCount = darkKnightVerticalLabel.Visible ? verticalLabels.Length + pspExtraVerticalLabels.Length : verticalLabels.Length;
                for( int i = 0; i < labelCount; i++ )
                {
                    VerticalLabel vLabel;
                    if( i < verticalLabels.Length )
                        vLabel = verticalLabels[i];
                    else
                        vLabel = pspExtraVerticalLabels[i - verticalLabels.Length];

                    if( selectedCell.ColumnIndex == i )
                        vLabel.Font = fontBold;
                    else if( vLabel.Font.Bold )
                        vLabel.Font = font;
                }

                // Set the row label to bold based on the selection
                labelCount = darkKnightLabel.Visible ? labels.Length + pspExtraLabels.Length : labels.Length;
                for( int i = 0; i < labelCount; i++ )
                {
                    Label label;
                    if( i < labels.Length )
                        label = labels[i];
                    else
                        label = pspExtraLabels[i - labels.Length];

                    if( selectedCell.RowIndex == i )
                        label.Font = fontBold;
                    else if( label.Font.Bold )
                        label.Font = font;
                }
                requirementsEditor1.EnableRedraw();
                grid.Invalidate();
            }
        }

        private void spinner_ValueChanged( object sender, EventArgs e )
        {
            NumericUpDownWithDefault spinner = sender as NumericUpDownWithDefault;
            ReflectionHelpers.SetFieldOrProperty( levels, spinner.Tag.ToString(), (UInt16)spinner.Value );
            OnDataChanged( this, System.EventArgs.Empty );
        }

		#endregion Private Methods 
    }
}
