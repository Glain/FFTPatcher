using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FFTPatcher
{
    internal static class LabelUtility
    {
        internal static void SetControlLabelFromMap(IDictionary<string, string> map, string key, Control control)
        {
            string labelValue = null;
            map.TryGetValue(key, out labelValue);
            if (labelValue != null)
            {
                control.Text = map[key];
            }
        }
    }
}
