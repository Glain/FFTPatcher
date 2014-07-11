using System;

namespace FFTPatcher.Controls
{
    class DualListActionEventArgs : EventArgs
    {
        public DualListAction Action { get; private set; }
        public object Item { get; private set; }
        public int Index { get; private set; }

        public DualListActionEventArgs( DualListAction action, object item, int index )
        {
            Action = action;
            Item = item;
            Index = index;
        }
    }
}
