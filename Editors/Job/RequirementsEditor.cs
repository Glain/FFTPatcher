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
using System.Drawing;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using PatcherLib;

namespace FFTPatcher.Editors
{
    public partial class RequirementsEditor : BaseEditor
    {
		#region Instance Variables (1) 

        private List<Requirements> requirements;

		#endregion Instance Variables 

		#region Public Properties (1) 

        public List<Requirements> Requirements
        {
            get { return requirements; }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public RequirementsEditor()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.CellParsing += dataGridView1_CellParsing;
            dataGridView1.CellValidating += dataGridView1_CellValidating;
            dataGridView1.CellToolTipTextNeeded += dataGridView1_CellToolTipTextNeeded;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
            dataGridView1.CellValidated += OnDataChanged;
        }

		#endregion Constructors 

        public void SetRequirements(List<Requirements> value, Context context)
        {
            if (value == null)
            {
                this.Enabled = false;
                this.requirements = null;
                dataGridView1.DataSource = null;
            }
            else if (value != requirements)
            {
                bool isPsp = (context == Context.US_PSP);
                OnionKnight.Visible = isPsp;
                DarkKnight.Visible = isPsp;
                Unknown1.Visible = isPsp;
                Unknown2.Visible = isPsp;
                this.Enabled = true;
                this.requirements = value;
                dataGridView1.DataSource = value;
            }
        }

		#region Private Methods (7) 

        private void Control_KeyDown( object sender, KeyEventArgs e )
        {
            if( (e.KeyData == Keys.F12) &&
                (dataGridView1.CurrentCell != null) &&
                (dataGridView1.CurrentCell.ColumnIndex >= 0) &&
                (dataGridView1.CurrentRow.DataBoundItem is Requirements) )
            {
                Requirements reqs = dataGridView1.CurrentRow.DataBoundItem as Requirements;
                dataGridView1.EditingControl.Text = ReflectionHelpers.GetFieldOrProperty<int>( reqs.Default, dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].DataPropertyName ).ToString();
                dataGridView1.EndEdit();
                if ( !PatcherLib.Utilities.Utilities.IsRunningOnMono() )
                {
                    InvalidateCell( dataGridView1.CurrentCell );
                }
            }
        }

        private void dataGridView1_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e )
        {
            if (e.ColumnIndex >= 0)
            {
                if( (e.RowIndex >= 0) && (e.ColumnIndex >= 0) &&
                    (dataGridView1.Rows[e.RowIndex].DataBoundItem is Requirements) )
                {
                    Requirements reqs = dataGridView1.Rows[e.RowIndex].DataBoundItem as Requirements;

                    if( reqs.Default != null )
                    {
                        int defaultValue = ReflectionHelpers.GetFieldOrProperty<int>( reqs.Default, dataGridView1.Columns[e.ColumnIndex].DataPropertyName );
                        int value = (int)(e.Value);

                        if ((dataGridView1.CurrentCell == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]) && (Settings.HighlightColor.UseColor))
                        {
                            e.CellStyle.BackColor = Settings.HighlightColor.BackgroundColor;
                            e.CellStyle.ForeColor = Settings.HighlightColor.ForegroundColor;
                        }
                        else if (value != defaultValue)
                        {
                            e.CellStyle.BackColor = Settings.ModifiedColor.BackgroundColor;
                            e.CellStyle.ForeColor = Settings.ModifiedColor.ForegroundColor;
                        }
                        else if (value == 0)
                        {
                            int colorValue = 190;
                            e.CellStyle.BackColor = Color.FromArgb(colorValue, colorValue, colorValue);
                        }
                        else
                        {
                            //int colorValue = Math.Min(255 - ((8 - value) * 6), 255);
                            //int colorValue = Math.Min(223 + (value * 4), 255);
                            //int addendValue = (value * 6);
                            //int redValue = Math.Min(200 + addendValue, 255);
                            //int otherValue = Math.Max(200 - addendValue, 150);
                            //e.CellStyle.BackColor = Color.White;
                            int redValue = 255;
                            int otherValue = redValue - (value * 8);
                            e.CellStyle.BackColor = Color.FromArgb(redValue, otherValue, otherValue);
                            //e.CellStyle.BackColor = Color.FromArgb(colorValue, colorValue, colorValue);
                            e.CellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellParsing( object sender, DataGridViewCellParsingEventArgs e )
        {
            int result;
            if( Int32.TryParse( e.Value.ToString(), out result ) )
            {
                if( result > 8 )
                    result = 8;
                if( result < 0 )
                    result = 0;
                e.Value = result;

                e.ParsingApplied = true;
            }
        }

        private void dataGridView1_CellToolTipTextNeeded( object sender, DataGridViewCellToolTipTextNeededEventArgs e )
        {
            if( (e.RowIndex >= 0) && (e.ColumnIndex >= 0) &&
                (dataGridView1.Rows[e.RowIndex].DataBoundItem is Requirements) )
            {
                Requirements reqs = dataGridView1.Rows[e.RowIndex].DataBoundItem as Requirements;
                if( reqs.Default != null )
                {
                    int i = ReflectionHelpers.GetFieldOrProperty<int>( reqs.Default, dataGridView1.Columns[e.ColumnIndex].DataPropertyName );
                    e.ToolTipText = "Default: " + i.ToString();
                }
            }
        }

        private void dataGridView1_CellValidating( object sender, DataGridViewCellValidatingEventArgs e )
        {
            int result;
            if( !Int32.TryParse( e.FormattedValue.ToString(), out result ) || (result < 0) || (result > 8) )
            {
                e.Cancel = true;
            }
        }

        private void dataGridView1_EditingControlShowing( object sender, DataGridViewEditingControlShowingEventArgs e )
        {
            e.Control.KeyDown += Control_KeyDown;
        }

        private void InvalidateCell( DataGridViewCell cell )
        {
            dataGridView1.InvalidateCell( cell );
        }

		#endregion Private Methods 
    }
}
