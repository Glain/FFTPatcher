using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PatcherLib.Controls
{
    public class BGDataGridView : DataGridView
    {
        private HashSet<int> highlightedIndexes = new HashSet<int>();

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

            if (highlightedIndexes.Contains(e.RowIndex))
            {
                
            }
        }
    }
}
