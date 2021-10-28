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

using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class ENTDEditor : UserControl, IHandleSelectedIndex
    {
		#region Instance Variables 

        private Context ourContext = Context.Default;

		#endregion Instance Variables 

        private ToolTip toolTip;
        public ToolTip ToolTip
        {
            set
            {
                toolTip = value;
                eventEditor1.ToolTip = value;
            }
        }

		#region Public Properties

        public Event ClipBoardEvent { get; private set; }

		#endregion Public Properties 

		#region Constructors (1) 

        public ENTDEditor()
        {
            InitializeComponent();
            eventEditor1.DataChanged += new System.EventHandler( eventEditor1_DataChanged );
            eventListBox.ContextMenu = new ContextMenu(
                new MenuItem[] { new MenuItem( "Clone", CopyClickEventHandler ), new MenuItem( "Paste clone", PasteClickEventHandler ) } );
            eventListBox.ContextMenu.MenuItems[1].Enabled = false;
            eventListBox.MouseDown += new MouseEventHandler( eventListBox_MouseDown );
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public void UpdateView( AllENTDs entds, Context context )
        {
            if( ourContext != context )
            {
                ourContext = context;
                ClipBoardEvent = null;
                eventListBox.ContextMenu.MenuItems[1].Enabled = false;
            }

            eventListBox.SelectedIndexChanged -= eventListBox_SelectedIndexChanged;
            eventListBox.DataSource = entds.Events;
            eventListBox.SelectedIndex = 0;
            //eventEditor1.Event = eventListBox.SelectedItem as Event;
            eventEditor1.SetEvent(eventListBox.SelectedItem as Event, context);
            eventListBox.SelectedIndexChanged += eventListBox_SelectedIndexChanged;

            eventListBox.SetChangedColors();
        }

		#endregion Public Methods 

		#region Private Methods (5) 

        private void CopyClickEventHandler( object sender, System.EventArgs args )
        {
            eventListBox.ContextMenu.MenuItems[1].Enabled = true;
            ClipBoardEvent = eventListBox.SelectedItem as Event;
        }

        private void eventEditor1_DataChanged( object sender, System.EventArgs e )
        {
            eventListBox.BeginUpdate();
            var top = eventListBox.TopIndex;
            CurrencyManager cm = (CurrencyManager)BindingContext[eventListBox.DataSource];
            cm.Refresh();
            eventListBox.TopIndex = top;
            eventListBox.EndUpdate();
            eventListBox.SetChangedColor();
        }

        private void eventListBox_MouseDown( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Right )
            {
                eventListBox.SelectedIndex = eventListBox.IndexFromPoint( e.Location );
            }
        }

        private void eventListBox_SelectedIndexChanged( object sender, System.EventArgs e )
        {
            eventEditor1.Event = eventListBox.SelectedItem as Event;
        }

        private void PasteClickEventHandler( object sender, System.EventArgs args )
        {
            if( ClipBoardEvent != null )
            {
                ClipBoardEvent.CopyTo( eventListBox.SelectedItem as Event );
                eventEditor1.Event = eventListBox.SelectedItem as Event;
                eventEditor1.UpdateView(ourContext);
                eventEditor1_DataChanged( eventEditor1, System.EventArgs.Empty );
            }
        }

        private void eventListBox_KeyDown( object sender, KeyEventArgs args )
		{
			if (args.KeyCode == Keys.C && args.Control)
				CopyClickEventHandler( sender, args );
			else if (args.KeyCode == Keys.V && args.Control)
				PasteClickEventHandler( sender, args );
		}
        
		#endregion Private Methods 

        public void HandleSelectedIndexChange(int offset)
        {
            int newIndex = eventListBox.SelectedIndex + offset;
            if ((newIndex >= 0) && (newIndex < eventListBox.Items.Count))
                eventListBox.SelectedIndex = newIndex;
        }
    }
}
