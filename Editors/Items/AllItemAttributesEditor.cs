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

using System;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;

namespace FFTPatcher.Editors
{
    public partial class AllItemAttributesEditor : UserControl, IHandleSelectedIndex
    {
		#region Instance Variables

        private ItemAttributes ClipBoardAttributes;
        private Context ourContext = Context.Default;

		#endregion Instance Variables 

        private ToolTip toolTip;
        public ToolTip ToolTip
        {
            set
            {
                toolTip = value;
                itemAttributeEditor.ToolTip = value;
            }
        }

		#region Public Properties 

        public int SelectedIndex { get { return offsetListBox.SelectedIndex; } set { offsetListBox.SelectedIndex = value; } }

		#endregion Public Properties 

		#region Constructors (1) 

        public AllItemAttributesEditor()
        {
            InitializeComponent();
            itemAttributeEditor.DataChanged += new EventHandler( itemAttributeEditor_DataChanged );
            itemAttributeEditor.RepointHandler += OnRepoint;

            offsetListBox.ContextMenu = new ContextMenu(
                new MenuItem[] { new MenuItem( "Clone", CopyClickEventHandler ), new MenuItem( "Paste clone", PasteClickEventHandler ) } );
            offsetListBox.ContextMenu.MenuItems[1].Enabled = false;
            offsetListBox.MouseDown += new MouseEventHandler( offsetListBox_MouseDown );

            itemAttributeEditor.ItemClicked += OnItemClicked;
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public void UpdateView( AllItemAttributes attributes, Context context )
        {
            if ( ourContext != context )
            {
                ourContext = context;
                ClipBoardAttributes = null;
                offsetListBox.ContextMenu.MenuItems[1].Enabled = false;
            }

            offsetListBox.SelectedIndexChanged -= offsetListBox_SelectedIndexChanged;
            offsetListBox.DataSource = attributes.ItemAttributes;
            offsetListBox.SelectedIndexChanged += offsetListBox_SelectedIndexChanged;
            offsetListBox.SelectedIndex = 0;

            PatchUtility.CheckDuplicates<ItemAttributes>(attributes.ItemAttributes);
            offsetListBox.SetChangedColors<ItemAttributes>();

            //itemAttributeEditor.ItemAttributes = offsetListBox.SelectedItem as ItemAttributes;
            itemAttributeEditor.SetItemAttributes(offsetListBox.SelectedItem as ItemAttributes, context);
        }

        public void UpdateSelectedEntry()
        {
            itemAttributeEditor.UpdateView(ourContext);
        }

        public void UpdateListBox()
        {
            PatchUtility.CheckDuplicates<ItemAttributes>((ItemAttributes[])offsetListBox.DataSource);
            offsetListBox.SetChangedColors<ItemAttributes>();
            offsetListBox.Invalidate();
        }

		#endregion Public Methods 

		#region Private Methods (5) 

        private void CopyClickEventHandler( object sender, System.EventArgs args )
        {
            offsetListBox.ContextMenu.MenuItems[1].Enabled = true;
            ClipBoardAttributes = offsetListBox.SelectedItem as ItemAttributes;
        }

        private void itemAttributeEditor_DataChanged( object sender, EventArgs e )
        {
            offsetListBox.BeginUpdate();
            int top = offsetListBox.TopIndex;
            PatchUtility.CheckDuplicates<ItemAttributes>((ItemAttributes[])offsetListBox.DataSource);
            CurrencyManager cm = (CurrencyManager)BindingContext[offsetListBox.DataSource];
            cm.Refresh();
            offsetListBox.TopIndex = top;
            offsetListBox.EndUpdate();
            //offsetListBox.SetChangedColor();
            offsetListBox.SetChangedColors<ItemAttributes>();
        }

        void offsetListBox_MouseDown( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Right )
            {
                offsetListBox.SelectedIndex = offsetListBox.IndexFromPoint( e.Location );
            }
        }

        private void offsetListBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            itemAttributeEditor.ItemAttributes = offsetListBox.SelectedItem as ItemAttributes;
        }

        private void PasteClickEventHandler( object sender, System.EventArgs args )
        {
            if ( ClipBoardAttributes != null )
            {
                ClipBoardAttributes.CopyTo( offsetListBox.SelectedItem as ItemAttributes );
                itemAttributeEditor.ItemAttributes = null;
                itemAttributeEditor.ItemAttributes = offsetListBox.SelectedItem as ItemAttributes;
                itemAttributeEditor.Invalidate(true);
                //itemAttributeEditor.UpdateView();
                itemAttributeEditor_DataChanged( itemAttributeEditor, System.EventArgs.Empty );
                itemAttributeEditor.PerformLayout();
            }
        }
        
        private void offsetListBox_KeyDown( object sender, KeyEventArgs args )
		{
			if (args.KeyCode == Keys.C && args.Control)
				CopyClickEventHandler( sender, args );
			else if (args.KeyCode == Keys.V && args.Control)
				PasteClickEventHandler( sender, args );
		}

		#endregion Private Methods 

        public void HandleSelectedIndexChange(int offset)
        {
            int newIndex = offsetListBox.SelectedIndex + offset;
            if ((newIndex >= 0) && (newIndex < offsetListBox.Items.Count))
                offsetListBox.SelectedIndex = newIndex;
        }

        public event System.EventHandler<RepointEventArgs> RepointHandler;
        protected void OnRepoint(object sender, RepointEventArgs e)
        {
            if (RepointHandler != null)
            {
                e.OldID = offsetListBox.SelectedIndex;
                RepointHandler(this, e);
            }
        }

        public event EventHandler<ReferenceEventArgs> ItemClicked;
        private void OnItemClicked(object sender, ReferenceEventArgs e)
        {
            if (ItemClicked != null)
            {
                ItemClicked(this, e);
            }
        }
    }
}
