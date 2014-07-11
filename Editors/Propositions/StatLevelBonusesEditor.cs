using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using PatcherLib.Datatypes;
using FFTPatcher.Datatypes;
using Lokad;

namespace FFTPatcher.Editors
{
    public partial class StatLevelBonusesEditor : BaseEditor
    {
        private DataGridViewRow copiedEntry;
        const int cloneIndex = 0;
        const int pasteIndex = 1;

        private TupleDictionary<BraveFaithNeutral, LevelRange, byte> levelBonuses;
        private TupleDictionary<BraveFaithNeutral, LevelRange, byte> levelBonusesDefault;

        private TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte> braveBonuses;
        private TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte> braveBonusesDefault;

        private TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte> faithBonuses;
        private TupleDictionary<BraveFaithNeutral, BraveFaithRange, byte> faithBonusesDefault;

        AllPropositions props;
        private Context ourContext = Context.Default;

        bool ignoreChanges = false;
        const int braveBonusesFirstRow = 0;
        const int faithBonusesFirstRow = 5;
        const int levelBonusesFirstRow = 10;

        public void UpdateView(
            AllPropositions props )
        {
            ignoreChanges = true;
            this.props = props;
            levelBonuses = props.LevelBonuses;
            levelBonusesDefault = props.Default.LevelBonuses;

            braveBonuses = props.BraveStatBonuses;
            braveBonusesDefault = props.Default.BraveStatBonuses;

            faithBonuses = props.FaithStatBonuses;
            faithBonusesDefault = props.Default.FaithStatBonuses;

            UpdateRowHeaders( FFTPatch.Context );

            foreach (BraveFaithNeutral bfn in (BraveFaithNeutral[])Enum.GetValues( typeof( BraveFaithNeutral ) ))
            {
                foreach (BraveFaithRange bfr in (BraveFaithRange[])Enum.GetValues( typeof( BraveFaithRange ) ))
                {
                    var thisTuple = Tuple.From( bfn, bfr );
                    if (braveBonuses.ContainsKey( thisTuple ))
                    {
                        dataGridView1[(int)bfn, (int)bfr + braveBonusesFirstRow].Value = braveBonuses[bfn, bfr];
                    }

                    if (faithBonuses.ContainsKey( thisTuple ))
                    {
                        dataGridView1[(int)bfn, (int)bfr + faithBonusesFirstRow].Value = faithBonuses[bfn, bfr];
                    }
                }

                foreach (LevelRange lr in (LevelRange[])Enum.GetValues( typeof( LevelRange ) ))
                {
                    if (levelBonuses.ContainsKey( Tuple.From( bfn, lr ) ))
                    {
                        dataGridView1[(int)bfn, (int)lr + levelBonusesFirstRow].Value = levelBonuses[bfn, lr];
                    }
                }
            }

            ignoreChanges = false;
        }

        public StatLevelBonusesEditor()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Rows.Add( 20 );
            dataGridView1.CellParsing += dataGridView1_CellParsing;
            dataGridView1.CellValidating += dataGridView1_CellValidating;
            dataGridView1.CellToolTipTextNeeded += dataGridView1_CellToolTipTextNeeded;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
            dataGridView1.CellValidated += new DataGridViewCellEventHandler( dataGridView1_CellValidated );

            dataGridView1.MouseUp += new MouseEventHandler(dataGridView_MouseUp);
            dataGridView1.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Clone", copyAll),
                new MenuItem("Paste", pasteAll)
            });
            dataGridView1.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);

            dataGridView1.KeyDown += new KeyEventHandler(dataGridView_KeyDown);
        }

        void dataGridView1_CellValidated( object sender, DataGridViewCellEventArgs e )
        {
            if (!ignoreChanges && e.ColumnIndex >= braveColumn.Index && e.ColumnIndex <= neutralColumn.Index )
            {
                int row = e.RowIndex;
                int column = e.ColumnIndex;
                
                if (row >= braveBonusesFirstRow && row < faithBonusesFirstRow)
                {
                    byte value = byte.Parse(dataGridView1[column, row].Value.ToString());
                    braveBonuses[(BraveFaithNeutral)column, (BraveFaithRange)(row - braveBonusesFirstRow)] = value;
                    if (props.MirrorLevelRanges)
                    {
                        ignoreChanges = true;
                        if (column == braveColumn.Index && row >= braveBonusesFirstRow && row < faithBonusesFirstRow)
                        {
                            levelBonuses[BraveFaithNeutral.Brave, (LevelRange)(row - braveBonusesFirstRow)] = value;
                            dataGridView1[braveColumn.Index, row - braveBonusesFirstRow + levelBonusesFirstRow].Value = value;
                        }
                        else if (column == faithColumn.Index && row >= braveBonusesFirstRow && row < faithBonusesFirstRow)
                        {
                            levelBonuses[BraveFaithNeutral.Brave, (LevelRange)(row - braveBonusesFirstRow + 5)] = value;
                            dataGridView1[braveColumn.Index, row - braveBonusesFirstRow + levelBonusesFirstRow + 5].Value = value;
                        }
                        else if (column == neutralColumn.Index && row >= braveBonusesFirstRow && row < faithBonusesFirstRow)
                        {
                            levelBonuses[BraveFaithNeutral.Faith, (LevelRange)(row - braveBonusesFirstRow)] = value;
                            dataGridView1[faithColumn.Index, row - braveBonusesFirstRow + levelBonusesFirstRow].Value = value;
                        }
                        ignoreChanges = false;
                    }
                }
                else if (row >= faithBonusesFirstRow && row < levelBonusesFirstRow)
                {
                    byte value = byte.Parse( dataGridView1[column, row].Value.ToString() );
                    faithBonuses[(BraveFaithNeutral)column, (BraveFaithRange)(row - faithBonusesFirstRow)] = value;
                    if (props.MirrorLevelRanges)
                    {
                        ignoreChanges = true;

                        if (column == braveColumn.Index && (row - faithBonusesFirstRow) >= 0 && (row - faithBonusesFirstRow) <= 3)
                        {
                            levelBonuses[BraveFaithNeutral.Faith, (LevelRange)(row - faithBonusesFirstRow + 6)] = value;
                            dataGridView1[faithColumn.Index, row - faithBonusesFirstRow + 6 + levelBonusesFirstRow].Value = value;
                        }
                        else if (column == braveColumn.Index && (row - faithBonusesFirstRow) == 4)
                        {
                            levelBonuses[BraveFaithNeutral.Neutral, LevelRange._1to10] = value;
                            dataGridView1[neutralColumn.Index, (int)LevelRange._1to10 + levelBonusesFirstRow].Value = value;
                        }
                        else if (column == faithColumn.Index)
                        {
                            levelBonuses[BraveFaithNeutral.Neutral, (LevelRange)(row - faithBonusesFirstRow + 1)] = value;
                            dataGridView1[neutralColumn.Index, row - faithBonusesFirstRow + levelBonusesFirstRow + 1].Value = value;
                        }
                        else if (column == neutralColumn.Index && (row - faithBonusesFirstRow) <= 3)
                        {
                            levelBonuses[BraveFaithNeutral.Neutral, (LevelRange)(row - faithBonusesFirstRow + 6)] = value;
                            dataGridView1[neutralColumn.Index, row - faithBonusesFirstRow + levelBonusesFirstRow + 6].Value = value;
                        }
                        ignoreChanges = false;
                    }
                //    return faithBonuses[(BraveFaithNeutral)column, (BraveFaithRange)(row - faithBonusesFirstRow)];
                }
                else if (row >= levelBonusesFirstRow)
                {
                    byte value = byte.Parse( dataGridView1[column, row].Value.ToString() );
                    levelBonuses[(BraveFaithNeutral)column, (LevelRange)(row - levelBonusesFirstRow)] = value;
                    if (props.MirrorLevelRanges)
                    {
                        ignoreChanges = true;
                        if (column == braveColumn.Index && (row - levelBonusesFirstRow) >= 0 && (row - levelBonusesFirstRow) <= 4)
                        {
                            braveBonuses[BraveFaithNeutral.Brave, (BraveFaithRange)(row - levelBonusesFirstRow)] = value;
                            dataGridView1[braveColumn.Index, row - levelBonusesFirstRow + braveBonusesFirstRow].Value = value;
                        }
                        else if (column == braveColumn.Index && (row - levelBonusesFirstRow) >= 5 && (row - levelBonusesFirstRow) <= 9)
                        {
                            braveBonuses[BraveFaithNeutral.Faith, (BraveFaithRange)(row - levelBonusesFirstRow - 5)] = value;
                            dataGridView1[faithColumn.Index, row - levelBonusesFirstRow - 5 + braveBonusesFirstRow].Value = value;
                        }
                        else if (column == faithColumn.Index && (row-levelBonusesFirstRow) >= 0 && (row-levelBonusesFirstRow) <= 4)
                        {
                            braveBonuses[BraveFaithNeutral.Neutral, (BraveFaithRange)(row - levelBonusesFirstRow)] = value;
                            dataGridView1[neutralColumn.Index, row - levelBonusesFirstRow + braveBonusesFirstRow].Value = value;
                        }
                        else if (column == faithColumn.Index && (row - levelBonusesFirstRow) >= 6 && (row - levelBonusesFirstRow) <= 9)
                        {
                            faithBonuses[BraveFaithNeutral.Brave, (BraveFaithRange)(row - levelBonusesFirstRow - 6)] = value;
                            dataGridView1[braveColumn.Index, row - levelBonusesFirstRow - 6 + faithBonusesFirstRow].Value = value;
                        }
                        else if (column == neutralColumn.Index && (row - levelBonusesFirstRow) == 0)
                        {
                            faithBonuses[BraveFaithNeutral.Brave, BraveFaithRange._81to100] = value;
                            dataGridView1[braveColumn.Index, row - levelBonusesFirstRow + faithBonusesFirstRow + 4].Value = value;
                        }
                        else if (column == neutralColumn.Index && (row - levelBonusesFirstRow) >= 1 && (row - levelBonusesFirstRow) <= 5)
                        {
                            faithBonuses[BraveFaithNeutral.Faith, (BraveFaithRange)(row - levelBonusesFirstRow - 1)] = value;
                            dataGridView1[faithColumn.Index, row - levelBonusesFirstRow - 1 + faithBonusesFirstRow].Value = value;
                        }
                        else if (column == neutralColumn.Index && (row - levelBonusesFirstRow) >= 6 && (row - levelBonusesFirstRow) <= 9)
                        {
                            faithBonuses[BraveFaithNeutral.Neutral, (BraveFaithRange)(row - levelBonusesFirstRow - 6)] = value;
                            dataGridView1[neutralColumn.Index, row - levelBonusesFirstRow - 6 + faithBonusesFirstRow].Value = value;
                        }
                        ignoreChanges = false;
                    }
                }
                else
                {
                //    return 0;
                }
                FireDataChanged();
            }
        }

        private void UpdateRowHeaders(Context context)
        {
            const string faithString = "Faith";
            const string levelString = "Level";
            const string oneThroughTwenty = "1-20";
            const string twentyOneThroughForty = "21-40";
            const string fortyOneThroughSixty = "41-60";
            const string sixtyOneThroughEighty = "61-80";
            const string eightyOneThroughHundred = "81-100";

            const string _1_10 = "1-10";
            const string _11_20 = "11-20";
            const string _21_30 = "21-30";
            const string _31_40 = "31-40";
            const string _41_50 = "41-50";
            const string _51_60 = "51-60";
            const string _61_70 = "61-70";
            const string _71_80 = "71-80";
            const string _81_90 = "81-90";
            const string _91_100 = "91-100";

            if (context != ourContext)
            {
                string[] strings = new string[] {
                    oneThroughTwenty, 
                    twentyOneThroughForty, 
                    fortyOneThroughSixty, 
                    sixtyOneThroughEighty, 
                    eightyOneThroughHundred };
                string[] levelStrings = new string[] {
                    _1_10,
                    _11_20,
                    _21_30,
                    _31_40,
                    _41_50,
                    _51_60,
                    _61_70,
                    _71_80,
                    _81_90,
                    _91_100 };

                string braveString = context == Context.US_PSP ? "Bravery" : "Brave";
                ourContext = context;

                for (int i = 0; i < 5; i++)
                {
                    dataGridView1[0, braveBonusesFirstRow + i].Value = string.Format( braveString + ": " + strings[i] );
                    dataGridView1[0, faithBonusesFirstRow + i].Value = string.Format( faithString + ": " + strings[i] );
                }

                for (int i = 0; i < 10; i++)
                {
                    dataGridView1[0, levelBonusesFirstRow + i].Value = string.Format( levelString + ": " + levelStrings[i] );
                }
            }
        }

        private void Control_KeyDown( object sender, KeyEventArgs e )
        {
            if ((e.KeyData == Keys.F12) &&
                (dataGridView1.CurrentCell != null) &&
                 dataGridView1.CurrentCell.ColumnIndex >= braveColumn.Index &&
                  dataGridView1.CurrentCell.ColumnIndex <= neutralColumn.Index &&
                  levelBonuses != null && braveBonuses != null && faithBonuses != null &&
                  levelBonusesDefault != null && braveBonusesDefault != null && faithBonusesDefault != null)
            {
                byte defaultValue = 
                    GetBfnTypeDefaultValue( dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex );

                dataGridView1.EditingControl.Text = defaultValue.ToString();
                dataGridView1.EndEdit();
                if (!PatcherLib.Utilities.Utilities.IsRunningOnMono())
                {
                    InvalidateCell( dataGridView1.CurrentCell );
                }
            }
        }

        private byte GetBfnTypeDefaultValue( int row, int column )
        {
            if (row >= braveBonusesFirstRow && row < faithBonusesFirstRow)
            {
                return braveBonusesDefault[(BraveFaithNeutral)column, (BraveFaithRange)(row - braveBonusesFirstRow)];
            }
            else if (row >= faithBonusesFirstRow && row < levelBonusesFirstRow)
            {
                return faithBonusesDefault[(BraveFaithNeutral)column, (BraveFaithRange)(row - faithBonusesFirstRow)];
            }
            else if (row >= levelBonusesFirstRow)
            {
                return levelBonusesDefault[(BraveFaithNeutral)column, (LevelRange)(row - levelBonusesFirstRow)];
            }
            else
            {
                return 0;
            }
        }


        private void dataGridView1_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e )
        {
            if (e.RowIndex >= 0 && 
                 (e.ColumnIndex >= braveColumn.Index &&
                  e.ColumnIndex <= neutralColumn.Index &&
                  levelBonuses != null && braveBonuses != null && faithBonuses != null &&
                  levelBonusesDefault != null && braveBonusesDefault != null && faithBonusesDefault != null ))
            {
                byte defaultValue = 
                    GetBfnTypeDefaultValue( e.RowIndex, e.ColumnIndex );
                if (byte.Parse( e.Value.ToString() ) != defaultValue)
                {
                    e.CellStyle.BackColor = Color.Blue;
                    e.CellStyle.ForeColor = Color.White;
                }
            }
        }

        private void dataGridView1_CellParsing( object sender, DataGridViewCellParsingEventArgs e )
        {
            int result;
            if (Int32.TryParse( e.Value.ToString(), out result ))
            {
                if (result > 255)
                    result = 255;
                if (result < 0)
                    result = 0;
                e.Value = result;

                e.ParsingApplied = true;
            }
        }

        private void dataGridView1_CellToolTipTextNeeded( object sender, DataGridViewCellToolTipTextNeededEventArgs e )
        {
            if (e.RowIndex >= 0 && 
                 (e.ColumnIndex >= braveColumn.Index &&
                  e.ColumnIndex <= neutralColumn.Index &&
                  levelBonuses != null && braveBonuses != null && faithBonuses != null &&
                  levelBonusesDefault != null && braveBonusesDefault != null && faithBonusesDefault != null))
            {
                byte defaultValue =
                    GetBfnTypeDefaultValue( e.RowIndex, e.ColumnIndex );
                e.ToolTipText = "Default: " + defaultValue.ToString();
            }
        }

        private void dataGridView1_CellValidating( object sender, DataGridViewCellValidatingEventArgs e )
        {
            int result;
            if ( e.ColumnIndex >= braveColumn.Index &&
                  e.ColumnIndex <= neutralColumn.Index  && 
                  e.RowIndex >=0 && (!Int32.TryParse( e.FormattedValue.ToString(), out result ) || (result < 0) || (result > 255)))
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

        private void dataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hitTest = dataGridView1.HitTest(e.Location.X, e.Location.Y);
                dataGridView1.CurrentCell = dataGridView1[hitTest.ColumnIndex, hitTest.RowIndex];
                dataGridView1.ContextMenu.Show(dataGridView1, new Point(e.X, e.Y));
            }
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            dataGridView1.ContextMenu.MenuItems[pasteIndex].Enabled = (copiedEntry != null);
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.C && args.Control)
                copyAll(sender, args);
            else if (args.KeyCode == Keys.V && args.Control)
                pasteAll(sender, args);
        }

        private void copyAll(object sender, EventArgs e)
        {
            copiedEntry = dataGridView1.CurrentRow;
        }

        private void pasteAll(object sender, EventArgs e)
        {
            if (copiedEntry != null)
            {
                DataGridViewRow destEntry = dataGridView1.CurrentRow;
                CopyFromRow(destEntry.Index, copiedEntry);
                UpdateView(props);
                dataGridView1.Invalidate();
            }
        }

        private void CopyFromRow(int destRowIndex, DataGridViewRow source)
        {
            BraveFaithRange destRowBFR = (BraveFaithRange)destRowIndex;
            LevelRange destRowLR = (LevelRange)destRowIndex;
            int sourceRowIndex = source.Index;

            foreach (BraveFaithNeutral bfn in (BraveFaithNeutral[])Enum.GetValues(typeof(BraveFaithNeutral)))
            {
                //var thisTuple = Tuple.From(bfn, destRowBFR);

                //if (braveBonuses.ContainsKey(thisTuple))
                if (destRowIndex < faithBonusesFirstRow)
                {
                    braveBonuses[bfn, destRowBFR] = (byte)(dataGridView1[(int)bfn, sourceRowIndex].Value);
                }
                else if (destRowIndex < levelBonusesFirstRow)
                //if (faithBonuses.ContainsKey(thisTuple))
                {
                    faithBonuses[bfn, destRowBFR - faithBonusesFirstRow] = (byte)(dataGridView1[(int)bfn, sourceRowIndex].Value);
                }
                else
                //if (levelBonuses.ContainsKey(Tuple.From(bfn, destRowLR)))
                {
                    levelBonuses[bfn, destRowLR - levelBonusesFirstRow] = (byte)(dataGridView1[(int)bfn, sourceRowIndex].Value);
                }
            }
        }
    }
}

/*
           foreach (BraveFaithNeutral bfn in (BraveFaithNeutral[])Enum.GetValues( typeof( BraveFaithNeutral ) ))
            {
                foreach (BraveFaithRange bfr in (BraveFaithRange[])Enum.GetValues( typeof( BraveFaithRange ) ))
                {
                    var thisTuple = Tuple.From( bfn, bfr );
                    if (braveBonuses.ContainsKey( thisTuple ))
                    {
                        dataGridView1[(int)bfn, (int)bfr + braveBonusesFirstRow].Value = braveBonuses[bfn, bfr];
                    }

                    if (faithBonuses.ContainsKey( thisTuple ))
                    {
                        dataGridView1[(int)bfn, (int)bfr + faithBonusesFirstRow].Value = faithBonuses[bfn, bfr];
                    }
                }

                foreach (LevelRange lr in (LevelRange[])Enum.GetValues( typeof( LevelRange ) ))
                {
                    if (levelBonuses.ContainsKey( Tuple.From( bfn, lr ) ))
                    {
                        dataGridView1[(int)bfn, (int)lr + levelBonusesFirstRow].Value = levelBonuses[bfn, lr];
                    }
                }
            }
*/