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

using FFTPatcher.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class MapMoveFindItemEditor : BaseEditor
    {
		#region Instance Variables

        private MapMoveFindItems mapMoveFindItems;
        private PatcherLib.Datatypes.Context ourContext = PatcherLib.Datatypes.Context.Default;

		#endregion Instance Variables 

        private System.Windows.Forms.ToolTip toolTip;
        public System.Windows.Forms.ToolTip ToolTip
        {
            set
            {
                toolTip = value;
                moveFindItemEditor1.ToolTip = value;
                moveFindItemEditor2.ToolTip = value;
                moveFindItemEditor3.ToolTip = value;
                moveFindItemEditor4.ToolTip = value;
            }
        }

		#region Public Properties

        public MapMoveFindItems MapMoveFindItems
        {
            get { return mapMoveFindItems; }
            set
            {
                SetMapMoveFindItems(value, ourContext);
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public MapMoveFindItemEditor()
        {
            InitializeComponent();
            moveFindItemEditor1.DataChanged += OnDataChanged;
            moveFindItemEditor2.DataChanged += OnDataChanged;
            moveFindItemEditor3.DataChanged += OnDataChanged;
            moveFindItemEditor4.DataChanged += OnDataChanged;
        }

		#endregion Constructors 

		#region Methods 

        public void SetMapMoveFindItems(MapMoveFindItems value, PatcherLib.Datatypes.Context context)
        {
            if (value == null)
            {
                this.Enabled = false;
                mapMoveFindItems = null;
            }
            else if (mapMoveFindItems != value)
            {
                mapMoveFindItems = value;
                this.Enabled = true;
                UpdateView(context);
            }
        }

        public void UpdateView(PatcherLib.Datatypes.Context context)
        {
            ourContext = context;

            //moveFindItemEditor1.MoveFindItem = mapMoveFindItems.Items[0];
            //moveFindItemEditor2.MoveFindItem = mapMoveFindItems.Items[1];
            //moveFindItemEditor3.MoveFindItem = mapMoveFindItems.Items[2];
            //moveFindItemEditor4.MoveFindItem = mapMoveFindItems.Items[3];

            moveFindItemEditor1.SetMoveFindItem(mapMoveFindItems.Items[0], context);
            moveFindItemEditor2.SetMoveFindItem(mapMoveFindItems.Items[1], context);
            moveFindItemEditor3.SetMoveFindItem(mapMoveFindItems.Items[2], context);
            moveFindItemEditor4.SetMoveFindItem(mapMoveFindItems.Items[3], context);

            moveFindItemEditor1.UpdateView(context);
            moveFindItemEditor2.UpdateView(context);
            moveFindItemEditor3.UpdateView(context);
            moveFindItemEditor4.UpdateView(context);
        }

		#endregion Methods 
    }
}
