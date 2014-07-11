using System;
using System.ComponentModel;

namespace FFTPatcher.Controls
{
    class DualListCancelEventArgs : CancelEventArgs
    {
        public DualListAction Action { get; private set; }
        public Object Item { get; private set; }

        public DualListCancelEventArgs( DualListAction action, object item )
        {
            Action = action;
            Item = item;
            Cancel = false;
        }

    }
}
