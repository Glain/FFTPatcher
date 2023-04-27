using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FFTPatcher.Controls
{
    public class BGDataGridView : DataGridView
    {
        private HashSet<int> highlightedIndexes = new HashSet<int>();

        private static Color colorForeDefault = Color.Black;
        private static Color colorBackDefault = Color.White;

        public void ClearHighlightedIndexes()
        {
            highlightedIndexes.Clear();
        }
        public void SetHighlightedIndexes(IEnumerable<int> indexes)
        {
            highlightedIndexes = new HashSet<int>(indexes);
        }

        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            base.OnCellFormatting(e);
            bool useHighlightColor = ((Settings.HighlightColor.UseColor) && (highlightedIndexes.Contains(e.RowIndex)));

            if (highlightedIndexes.Contains(e.RowIndex))
            {
                if (useHighlightColor)
                {
                    Rows[e.RowIndex].DefaultCellStyle.ForeColor = Settings.HighlightColor.ForegroundColor;
                    Rows[e.RowIndex].DefaultCellStyle.BackColor = Settings.HighlightColor.BackgroundColor;
                }
                else
                {
                    Rows[e.RowIndex].DefaultCellStyle.ForeColor = colorForeDefault;
                    Rows[e.RowIndex].DefaultCellStyle.BackColor = colorBackDefault;
                }
            }
            else
            {
                Rows[e.RowIndex].DefaultCellStyle.ForeColor = colorForeDefault;
                Rows[e.RowIndex].DefaultCellStyle.BackColor = colorBackDefault;
            }
        }
    }
}
