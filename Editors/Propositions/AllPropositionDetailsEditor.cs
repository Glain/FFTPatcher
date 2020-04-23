using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FFTPatcher.Datatypes;
using PatcherLib.Utilities;
namespace FFTPatcher.Editors
{
    public partial class AllPropositionDetailsEditor : UserControl
    {
        private Proposition copiedEntry;
        const int cloneIndex = 0;
        const int pasteIndex = 1;

        private AllPropositions props;

        public AllPropositionDetailsEditor()
        {
            InitializeComponent();

            listBox1.MouseDown += new MouseEventHandler(itemListBox_MouseDown);
            listBox1.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Clone", copyAll),
                new MenuItem("Paste", pasteAll),
            });
            listBox1.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);

            listBox1.KeyDown += new KeyEventHandler(itemListBox_KeyDown);
            listBox1.IncludePrefix = true;

            propositionEditor1.DataChanged += editor_DataChanged;
        }

        public void UpdateView( AllPropositions props )
        {
            this.props = props;
            UpdateListBox();
            listBox1.SelectedIndex = 0;
            propositionEditor1.BindTo(
                props.Propositions[0],
                props.Prices,
                props.SmallBonuses,
                props.LargeBonuses );
        }

        private void editor_DataChanged(object sender, EventArgs e)
        {
            UpdateListBox();
        }

        public void itemListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                listBox1.SelectedIndex = listBox1.IndexFromPoint(e.Location);
            }
        }

        private void UpdateListBox()
        {
            listBox1.BeginUpdate();
            int index = listBox1.SelectedIndex;
            listBox1.Items.Clear();
            props.Propositions.ForEach( p => listBox1.Items.Add( p ) );
            listBox1.SelectedIndex = index;
            listBox1.EndUpdate();
        }

        protected override void OnVisibleChanged( EventArgs e )
        {
            if (propositionEditor1 != null && props != null)
            {
                propositionEditor1.NotifyNewPrices( props.Prices, props.SmallBonuses, props.LargeBonuses );
            }
            base.OnVisibleChanged( e );
        }

        private void listBox1_SelectedIndexChanged( object sender, EventArgs e )
        {
            if (listBox1.SelectedIndex != -1)
            {
                propositionEditor1.BindTo( props.Propositions[listBox1.SelectedIndex], props.Prices, props.SmallBonuses, props.LargeBonuses );
            }
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            listBox1.ContextMenu.MenuItems[pasteIndex].Enabled = (copiedEntry != null);
        }

        private void itemListBox_KeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.C && args.Control)
                copyAll(sender, args);
            else if (args.KeyCode == Keys.V && args.Control)
                pasteAll(sender, args);

            if (args.Control)
                args.SuppressKeyPress = true;
        }

        private void copyAll(object sender, EventArgs args)
        {
            copiedEntry = listBox1.SelectedItem as Proposition;
        }

        private void pasteAll(object sender, EventArgs args)
        {
            if (copiedEntry != null)
            {
                Proposition destEntry = listBox1.SelectedItem as Proposition;
                copiedEntry.CopyTo(destEntry);
                propositionEditor1.BindTo(props.Propositions[listBox1.SelectedIndex], props.Prices, props.SmallBonuses, props.LargeBonuses);
                propositionEditor1.Invalidate(true);
                editor_DataChanged(propositionEditor1, EventArgs.Empty);
            }
        }
    }
}
