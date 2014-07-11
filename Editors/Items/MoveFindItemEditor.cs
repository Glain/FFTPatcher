using System;
using System.Windows.Forms;
using FFTPatcher.Controls;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher.Editors
{
    public partial class MoveFindItemEditor : BaseEditor
    {
		#region Instance Variables (3) 

        private  bool ignoreChanges = false;
        private MoveFindItem moveFindItem;
        private Context ourContext = Context.Default;

		#endregion Instance Variables 

		#region Public Properties (2) 

        public string Label
        {
            get { return groupBox1.Text; }
            set { groupBox1.Text = value; }
        }

        public MoveFindItem MoveFindItem
        {
            get { return moveFindItem; }
            set
            {
                if ( value == null )
                {
                    this.Enabled = false;
                    moveFindItem = null;
                }
                else if ( moveFindItem != value )
                {
                    moveFindItem = value;
                    this.Enabled = true;
                    UpdateView();
                }
            }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public MoveFindItemEditor()
        {
            InitializeComponent();
            xSpinner.ValueChanged += new EventHandler( xSpinner_ValueChanged );
            ySpinner.ValueChanged += new EventHandler( ySpinner_ValueChanged );
            rareComboBox.SelectedIndexChanged += new EventHandler( rareComboBox_SelectedIndexChanged );
            commonComboBox.SelectedIndexChanged += new EventHandler( commonComboBox_SelectedIndexChanged );
            trapsCheckedListBox.ItemCheck += new ItemCheckEventHandler( trapsCheckedListBox_ItemCheck );
        }

		#endregion Constructors 

		#region Private Methods (6) 

        void commonComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( !ignoreChanges )
            {
                moveFindItem.CommonItem = commonComboBox.SelectedItem as Item;
                OnDataChanged( this, EventArgs.Empty );
            }
        }

        void rareComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( !ignoreChanges )
            {
                moveFindItem.RareItem = rareComboBox.SelectedItem as Item;
                OnDataChanged( this, EventArgs.Empty );
            }
        }

        void trapsCheckedListBox_ItemCheck( object sender, ItemCheckEventArgs e )
        {
            if ( !ignoreChanges )
            {
                switch ( e.Index )
                {
                    case 0:
                        moveFindItem.Unknown1 = e.NewValue == CheckState.Checked;
                        break;
                    case 1:
                        moveFindItem.Unknown2 = e.NewValue == CheckState.Checked;
                        break;
                    case 2:
                        moveFindItem.Unknown3 = e.NewValue == CheckState.Checked;
                        break;
                    case 3:
                        moveFindItem.Unknown4 = e.NewValue == CheckState.Checked;
                        break;
                    case 4:
                        moveFindItem.SteelNeedle = e.NewValue == CheckState.Checked;
                        break;
                    case 5:
                        moveFindItem.SleepingGas = e.NewValue == CheckState.Checked;
                        break;
                    case 6:
                        moveFindItem.Deathtrap = e.NewValue == CheckState.Checked;
                        break;
                    case 7:
                        moveFindItem.Degenerator = e.NewValue == CheckState.Checked;
                        break;
                }
                OnDataChanged( this, EventArgs.Empty );
            }
        }

        public void UpdateView()
        {
            ignoreChanges = true;
            this.SuspendLayout();
            trapsCheckedListBox.SuspendLayout();

            if ( ourContext != FFTPatch.Context )
            {
                ourContext = FFTPatch.Context;
                foreach ( ComboBoxWithDefault cb in new ComboBoxWithDefault[] { rareComboBox, commonComboBox } )
                {
                    cb.Items.Clear();
                    cb.Items.AddRange( Item.DummyItems.Sub( 0, 0xFF ).ToArray() );
                }
            }

            rareComboBox.SetValueAndDefault( moveFindItem.RareItem, moveFindItem.Default.RareItem );
            commonComboBox.SetValueAndDefault( moveFindItem.CommonItem, moveFindItem.Default.CommonItem );
            xSpinner.SetValueAndDefault( moveFindItem.X, moveFindItem.Default.X );
            ySpinner.SetValueAndDefault( moveFindItem.Y, moveFindItem.Default.Y );
            trapsCheckedListBox.SetValuesAndDefaults(
                new bool[] {moveFindItem.Unknown1, moveFindItem.Unknown2, moveFindItem.Unknown3, moveFindItem.Unknown4,
                    moveFindItem.SteelNeedle, moveFindItem.SleepingGas, moveFindItem.Deathtrap, moveFindItem.Degenerator},
                new bool[] {moveFindItem.Default.Unknown1, moveFindItem.Default.Unknown2, moveFindItem.Default.Unknown3, moveFindItem.Default.Unknown4,
                    moveFindItem.Default.SteelNeedle, moveFindItem.Default.SleepingGas, moveFindItem.Default.Deathtrap, moveFindItem.Default.Degenerator} );

            ignoreChanges = false;
            trapsCheckedListBox.ResumeLayout();
            this.ResumeLayout();
        }

        void xSpinner_ValueChanged( object sender, EventArgs e )
        {
            if ( !ignoreChanges )
            {
                moveFindItem.X = (byte)xSpinner.Value;
                OnDataChanged( this, EventArgs.Empty );
            }
        }

        void ySpinner_ValueChanged( object sender, EventArgs e )
        {
            if ( !ignoreChanges )
            {
                moveFindItem.Y = (byte)ySpinner.Value;
                OnDataChanged( this, EventArgs.Empty );
            }
        }

		#endregion Private Methods 
    }
}
