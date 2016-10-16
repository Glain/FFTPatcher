using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class AllAnimationsEditor : UserControl
    {
        class MyBindingList : System.ComponentModel.BindingList<Animation>
        {
            public MyBindingList(IList<Animation> otherList)
                : base(otherList)
            {
                this.AllowNew = false;
                this.AllowRemove = false;

            }

            Comparison<Animation> nameComparer = (a, b) => a.Name.CompareTo(b.Name);
            Comparison<Animation> nameCompererReverse = (a, b) => b.Name.CompareTo(a.Name);
            Comparison<Animation> indexComparer = (a, b) => a.Index.CompareTo(b.Index);
            Comparison<Animation> indexComparerReverse = (a, b) => b.Index.CompareTo(a.Index);

            protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
            {
                Comparison<Animation> comparer = null;
                if (prop.Name == "Name")
                {
                    comparer = direction == ListSortDirection.Ascending ? nameComparer : nameCompererReverse;
                }
                else if (prop.Name.StartsWith("Bool"))
                {
                    comparer =
                        direction == ListSortDirection.Ascending ?
                        (Comparison<Animation>)((a, b) => ReflectionHelpers.GetFieldOrProperty<bool>(a, prop.Name).CompareTo(ReflectionHelpers.GetFieldOrProperty<bool>(b, prop.Name))) :
                        (Comparison<Animation>)((a, b) => ReflectionHelpers.GetFieldOrProperty<bool>(b, prop.Name).CompareTo(ReflectionHelpers.GetFieldOrProperty<bool>(a, prop.Name)));
                }
                else if (prop.Name.StartsWith("Byte"))
                {
                    comparer =
                        direction == ListSortDirection.Ascending ?
                        (Comparison<Animation>)((a, b) => ReflectionHelpers.GetFieldOrProperty<byte>(a, prop.Name).CompareTo(ReflectionHelpers.GetFieldOrProperty<byte>(b, prop.Name))) :
                        (Comparison<Animation>)((a, b) => ReflectionHelpers.GetFieldOrProperty<byte>(b, prop.Name).CompareTo(ReflectionHelpers.GetFieldOrProperty<byte>(a, prop.Name)));
                }
                else
                {
                    comparer = direction == ListSortDirection.Ascending ? indexComparer : indexComparerReverse;
                }
                PatcherLib.Utilities.Utilities.SortList(Items, comparer);
            }

            protected override bool SupportsSortingCore
            {
                get
                {
                    return true;
                }
            }
        }

        private Animation copiedAnimation;
        const int cloneIndex = 0;
        const int pasteIndex = 1;

        public AllAnimationsEditor()
        {
            InitializeComponent();
            dataGridView1.AutoSize = true;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView1_CellFormatting);
            dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
            dataGridView1.CellParsing += new DataGridViewCellParsingEventHandler(dataGridView1_CellParsing);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
            dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);

            dataGridView1.MouseUp += new MouseEventHandler(dataGridView1_MouseUp);
            dataGridView1.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Clone", copyAll),
                new MenuItem("Paste", pasteAll)
            });
            dataGridView1.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);

            dataGridView1.KeyDown += new KeyEventHandler(dataGridView1_KeyDown);
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hitTest = dataGridView1.HitTest(e.Location.X, e.Location.Y);
                dataGridView1.CurrentCell = dataGridView1[hitTest.ColumnIndex, hitTest.RowIndex];
                dataGridView1.ContextMenu.Show(dataGridView1, new Point(e.X, e.Y));
            }
        }

        void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == Byte1Column.Index ||
                e.ColumnIndex == Byte2Column.Index ||
                e.ColumnIndex == Byte3Column.Index)
            {
                byte val = 0;
                if (byte.TryParse((string)e.Value, System.Globalization.NumberStyles.HexNumber, null, out val))
                {
                    e.ParsingApplied = true;
                    e.Value = val;
                }
            }
        }

        void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == Byte1Column.Index ||
                e.ColumnIndex == Byte2Column.Index ||
                e.ColumnIndex == Byte3Column.Index)
            {
                byte result;
                e.Cancel = !byte.TryParse((string)e.FormattedValue, System.Globalization.NumberStyles.HexNumber, null, out result);
            }
        }

        void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = dataGridView1[e.ColumnIndex, e.RowIndex] as DataGridViewCheckBoxCell;
            if (cell != null && dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dataGridView1.InvalidateCell(Byte1Column.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Byte2Column.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Byte3Column.Index, e.RowIndex);
            }
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (e.ColumnIndex == Byte1Column.Index)
            {
                dataGridView1.InvalidateCell(Column1.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column2.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column3.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column4.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column5.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column6.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column7.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column8.Index, e.RowIndex);
            }
            else if (e.ColumnIndex == Byte2Column.Index)
            {
                dataGridView1.InvalidateCell(Column9.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column10.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column11.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column12.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column13.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column14.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column15.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column16.Index, e.RowIndex);
            }
            else if (e.ColumnIndex == Byte3Column.Index)
            {
                dataGridView1.InvalidateCell(Column17.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column18.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column19.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column20.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column21.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column22.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column23.Index, e.RowIndex);
                dataGridView1.InvalidateCell(Column24.Index, e.RowIndex);
            }
            */
        }

        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == IndexColumn.Index)
            {
                e.FormattingApplied = true;
                e.Value = ((ushort)e.Value).ToString("X3");
            }
            else if (e.ColumnIndex == Byte1Column.Index ||
                e.ColumnIndex == Byte2Column.Index ||
                e.ColumnIndex == Byte3Column.Index)
            {
                var a = dataGridView1.Rows[e.RowIndex].DataBoundItem as Animation;
                byte defaultValue =
                    ReflectionHelpers.GetFieldOrProperty<byte>(a.Default, dataGridView1.Columns[e.ColumnIndex].DataPropertyName);
                byte newValue =
                    ReflectionHelpers.GetFieldOrProperty<byte>(a, dataGridView1.Columns[e.ColumnIndex].DataPropertyName);
                if (defaultValue != newValue)
                {
                    e.CellStyle.BackColor = Color.Blue;
                    e.CellStyle.ForeColor = Color.White;
                }
                e.FormattingApplied = true;
                e.Value = ((byte)e.Value).ToString("X2");
            }
            else if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
            {
                var a = dataGridView1.Rows[e.RowIndex].DataBoundItem as Animation;
                bool defaultValue =
                    ReflectionHelpers.GetFieldOrProperty<bool>(a.Default, dataGridView1.Columns[e.ColumnIndex].DataPropertyName);
                bool newValue =
                    ReflectionHelpers.GetFieldOrProperty<bool>(a, dataGridView1.Columns[e.ColumnIndex].DataPropertyName);
                if (defaultValue != newValue)
                {
                    e.CellStyle.BackColor = Color.Blue;
                    e.CellStyle.ForeColor = Color.White;
                }
            }
        }

        public void UpdateView(AllAnimations animations)
        {
            dataGridView1.DataSource = null;

            dataGridView1.DataSource = new MyBindingList(new List<Animation>(animations.Animations));
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            dataGridView1.ContextMenu.MenuItems[pasteIndex].Enabled = (copiedAnimation != null);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.C && args.Control)
                copyAll(sender, args);
            else if (args.KeyCode == Keys.V && args.Control)
                pasteAll(sender, args);
        }

        private void copyAll(object sender, EventArgs e)
        {            
            copiedAnimation = dataGridView1.CurrentRow.DataBoundItem as Animation;
        }

        private void pasteAll(object sender, EventArgs e)
        {
            if (copiedAnimation != null)
            {
                Animation destAnimation = dataGridView1.CurrentRow.DataBoundItem as Animation;
                copiedAnimation.CopyAllTo(destAnimation);
                dataGridView1.Invalidate();
            }
        }
    }
}
