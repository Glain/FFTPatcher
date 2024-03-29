﻿using System;
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
    public partial class ClassBonusesEditor : BaseEditor
    {
        private DataGridViewRow copiedEntry;
        const int cloneIndex = 0;
        const int pasteIndex = 1;

        private AllPropositions allProps;

        private TupleDictionary<PropositionType, PropositionClass, byte> propTypeBonuses;
        private TupleDictionary<PropositionType, PropositionClass, byte> propTypeBonusesDefault;

        private TupleDictionary<BraveFaithNeutral, PropositionClass, byte> bfnBonuses;
        private TupleDictionary<BraveFaithNeutral, PropositionClass, byte> bfnBonusesDefault;

        private Context ourContext = Context.Default;
        bool ignoreChanges = false;

        public void UpdateView( AllPropositions props, Context context )
        {
            ignoreChanges = true;
            allProps = props;
            
            propTypeBonuses = props.PropositionTypeBonuses;
            propTypeBonusesDefault = props.Default.PropositionTypeBonuses;

            bfnBonuses = props.BraveFaithBonuses;
            bfnBonusesDefault = props.Default.BraveFaithBonuses;

            UpdateRowHeaders(context);

            foreach (PropositionClass clas in (PropositionClass[])Enum.GetValues( typeof( PropositionClass ) ))
            {
                foreach (PropositionType type in (PropositionType[])Enum.GetValues( typeof( PropositionType ) ))
                {
                    if (propTypeBonuses.ContainsKey( Tuple.From( type, clas ) ))
                    {
                        dataGridView1[(int)type, (int)clas].Value = propTypeBonuses[type, clas];
                    }
                }

                foreach (BraveFaithNeutral bfn in (BraveFaithNeutral[])Enum.GetValues( typeof( BraveFaithNeutral ) ))
                {
                    if (bfnBonuses.ContainsKey( Tuple.From( bfn, clas ) ))
                    {
                        dataGridView1[(int)bfn - 1 + braveColumn.Index, (int)clas].Value = bfnBonuses[bfn, clas];
                    }
                }
            }
            ignoreChanges = false;
        }

        public ClassBonusesEditor()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Rows.Add( 22 );
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
            if (!ignoreChanges && ((e.ColumnIndex >= salvageColumn.Index && e.ColumnIndex <= contestColumn.Index) || (e.ColumnIndex >= braveColumn.Index && e.ColumnIndex <= neutralColumn.Index)))
            {
                int column = e.ColumnIndex;
                int row = e.RowIndex;
                byte value = byte.Parse( dataGridView1[column, row].Value.ToString() );
                if (column >= salvageColumn.Index && column <= contestColumn.Index)
                {
                    propTypeBonuses[(PropositionType)(column - salvageColumn.Index + 1), (PropositionClass)row] = value;
                }
                else if (column >= braveColumn.Index && column <= neutralColumn.Index)
                {
                    bfnBonuses[(BraveFaithNeutral)(column - braveColumn.Index + 1), (PropositionClass)row] = value;
                }
                FireDataChanged();
            }
        }

        private void UpdateRowHeaders(Context context)
        {
            if (context != ourContext)
            {
                ourContext = context;
                IList<string> jobNames = ourContext == Context.US_PSP ? PatcherLib.PSPResources.Lists.JobNames : PatcherLib.PSXResources.Lists.JobNames;
                const int squireIndex = 0x4A;
                for (int i = 0; i < 20; i++)
                {
                    dataGridView1[0, i].Value = jobNames[i + squireIndex];
                }
                dataGridView1[0, 20].Value = "Specials";
                dataGridView1[0, 21].Value = "Monsters";
            }
        }

        private void Control_KeyDown( object sender, KeyEventArgs e )
        {
            if ((e.KeyData == Keys.F12) &&
                (dataGridView1.CurrentCell != null) &&
                ((dataGridView1.CurrentCell.ColumnIndex >= salvageColumn.Index &&
                 dataGridView1.CurrentCell.ColumnIndex <= contestColumn.Index &&
                 propTypeBonuses != null && propTypeBonusesDefault != null ) ||
                 (dataGridView1.CurrentCell.ColumnIndex >= braveColumn.Index &&
                  dataGridView1.CurrentCell.ColumnIndex <= neutralColumn.Index &&
                  bfnBonuses != null && bfnBonusesDefault != null )) &&
                propTypeBonusesDefault != null &&
                propTypeBonuses != null )
            {
                byte defaultValue = dataGridView1.CurrentCell.ColumnIndex <= contestColumn.Index ?
                    GetPropTypeDefaultValue( dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex ) :
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
            column -= braveColumn.Index;
            return bfnBonusesDefault[(BraveFaithNeutral)(column + 1), (PropositionClass)row];
        }

        private byte GetPropTypeDefaultValue( int row, int column )
        {
            return propTypeBonusesDefault[(PropositionType)column, (PropositionClass)row];
        }

        private void dataGridView1_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e )
        {
            if (e.RowIndex >= 0 && 
                ((e.ColumnIndex >= salvageColumn.Index &&
                 e.ColumnIndex <= contestColumn.Index &&
                 propTypeBonuses != null && propTypeBonusesDefault != null ) ||
                 (e.ColumnIndex >= braveColumn.Index &&
                  e.ColumnIndex <= neutralColumn.Index &&
                  bfnBonuses != null && bfnBonusesDefault != null )))
            {
                byte defaultValue = e.ColumnIndex <= contestColumn.Index ?
                    GetPropTypeDefaultValue( e.RowIndex, e.ColumnIndex ) :
                    GetBfnTypeDefaultValue( e.RowIndex, e.ColumnIndex );
                if (byte.Parse( e.Value.ToString() ) != defaultValue)
                {
                    e.CellStyle.BackColor = Settings.ModifiedColor.BackgroundColor;
                    e.CellStyle.ForeColor = Settings.ModifiedColor.ForegroundColor;
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
                ((e.ColumnIndex >= salvageColumn.Index &&
                 e.ColumnIndex <= contestColumn.Index &&
                 propTypeBonuses != null && propTypeBonusesDefault != null ) ||
                 (e.ColumnIndex >= braveColumn.Index &&
                  e.ColumnIndex <= neutralColumn.Index &&
                  bfnBonuses != null && bfnBonusesDefault != null )))
            {
                byte defaultValue = e.ColumnIndex <= contestColumn.Index ?
                    GetPropTypeDefaultValue( e.RowIndex, e.ColumnIndex ) :
                    GetBfnTypeDefaultValue( e.RowIndex, e.ColumnIndex );
                e.ToolTipText = "Default: " + defaultValue.ToString();
            }
        }

        private void dataGridView1_CellValidating( object sender, DataGridViewCellValidatingEventArgs e )
        {
            int result;
            if ( ((e.ColumnIndex >= salvageColumn.Index &&
                 e.ColumnIndex <= contestColumn.Index ) ||
                 (e.ColumnIndex >= braveColumn.Index &&
                  e.ColumnIndex <= neutralColumn.Index )) && 
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
                UpdateView(allProps, ourContext);
                dataGridView1.Invalidate();
            }
        }

        private void CopyFromRow(int destRowIndex, DataGridViewRow source)
        {
            PropositionClass destRowClass = (PropositionClass)destRowIndex;
            int sourceRowIndex = source.Index;

            foreach (PropositionType type in (PropositionType[])Enum.GetValues(typeof(PropositionType)))
            {
                if (propTypeBonuses.ContainsKey(Tuple.From(type, destRowClass)))
                {
                    propTypeBonuses[type, destRowClass] = (byte)(dataGridView1[(int)type, (int)sourceRowIndex].Value);
                }
            }
                
            foreach (BraveFaithNeutral bfn in (BraveFaithNeutral[])Enum.GetValues(typeof(BraveFaithNeutral)))
            {
                if (bfnBonuses.ContainsKey(Tuple.From(bfn, destRowClass)))
                {
                    bfnBonuses[bfn, destRowClass] = (byte)(dataGridView1[(int)bfn - 1 + braveColumn.Index, (int)sourceRowIndex].Value);
                }
            }
        }

    }
}
