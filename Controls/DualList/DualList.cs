using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FFTPatcher.Controls
{
    public enum DualListAction
    {
        MoveSelected,
        CopySelected,
    }

    delegate void BeforeActionEventHandler( object sender, DualListCancelEventArgs e );
    delegate void AfterActionEventHandler( object sender, DualListActionEventArgs e );
    
    [Designer(typeof(DualListDesigner))]
    class DualList : Component
    {
        private Container components = null;

        public DualList( IContainer container )
        {
            container.Add( this );
            InitializeComponent();
        }

        public DualList()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            components = new Container();
        }

        private ListBox listBoxFrom;
        private Button button;

        private bool autoDisableButton = false;
        private bool busy = false;
        private IButtonControl previousButton;

        [Category("Action")]
        [Description("Occurs before action")]
        public event BeforeActionEventHandler BeforeAction;
        [Category("Action")]
        [Description("Occurs after action")]
        public event AfterActionEventHandler AfterAction;

        private void ButtonClick( object sender, EventArgs e )
        {
            Click();
        }

        private void FromEnter( object sender, EventArgs e )
        {
            if ( LicenseManager.UsageMode != LicenseUsageMode.Designtime && DoubleClickSupport && listBoxFrom != null )
            {
                Form f = listBoxFrom.FindForm();
                previousButton = f.AcceptButton;
                f.AcceptButton = button;
            }
        }

        private void FromLeave( object sender, EventArgs e )
        {
            if ( LicenseManager.UsageMode != LicenseUsageMode.Designtime && DoubleClickSupport && listBoxFrom != null )
            {
                Form f = listBoxFrom.FindForm();
                f.AcceptButton = previousButton;
            }
        }

        private void FromDoubleClick( object sender, EventArgs e )
        {
            if ( DoubleClickSupport )
            {
                Click();
            }
        }

        private void SelectedIndexChanged( object sender, EventArgs e )
        {
            EnableDisable();
        }

        [Category("Behavior")]
        [Bindable(true)]
        public ListBox ListBoxFrom
        {
            get { return listBoxFrom; }
            set
            {
                if ( listBoxFrom != value )
                {
                    if ( listBoxFrom != null )
                    {
                        listBoxFrom.Enter -= FromEnter;
                        listBoxFrom.Leave -= FromLeave;
                        listBoxFrom.DoubleClick -= FromDoubleClick;
                        listBoxFrom.SelectedIndexChanged -= SelectedIndexChanged;
                    }

                    listBoxFrom = value;
                    if ( listBoxFrom != null )
                    {
                        listBoxFrom.Enter += FromEnter;
                        listBoxFrom.Leave += FromLeave;
                        listBoxFrom.DoubleClick += FromDoubleClick;
                        listBoxFrom.SelectedIndexChanged += SelectedIndexChanged;
                    }
                }

                EnableDisable();
            }
        }

        [Category("Behavior")]
        [Bindable(true)]
        public ListBox ListBoxTo
        {
            get;
            set;
        }

        [Category("Behavior")]
        [Bindable(true)]
        public Button Button
        {
            get { return button; }
            set
            {
                if ( button != value )
                {
                    if ( button != null )
                    {
                        button.Click -= ButtonClick;
                    }
                    button = value;
                    if ( button != null )
                    {
                        button.Click += ButtonClick;
                        EnableDisable();
                    }
                }
            }
        }

        [Category("Behavior")]
        [Bindable(true)]
        [DefaultValue(false)]
        public bool DoubleClickSupport { get; set; }

        [Category("Behavior")]
        [Bindable(true)]
        [DefaultValue(false)]
        public bool AutoDisableButton
        {
            get { return autoDisableButton; }
            set
            {
                autoDisableButton = value;
                if ( autoDisableButton )
                {
                    EnableDisable();
                }
                else if ( Button != null )
                {
                    Button.Enabled = true;
                }
            }
        }

        [Category("Behavior")]
        [Bindable(true)]
        [DefaultValue(DualListAction.MoveSelected)]
        public DualListAction Action { get; set; }

        public void MoveSelected()
        {
            busy = true;
            DoSelected( DualListAction.MoveSelected );
            busy = false;
            EnableDisable();
        }

        public void CopySelected()
        {
            busy = true;
            DoSelected( DualListAction.CopySelected );
            busy = false;
            EnableDisable();
        }

        private void DoSelected( DualListAction dualListAction )
        {
            ListBoxTo.SelectedIndices.Clear();
            if ( ListBoxFrom.SelectionMode == SelectionMode.One )
            {
                if ( ListBoxFrom.SelectedIndex > -1 )
                {
                    int i = ListBoxFrom.SelectedIndex;
                    if ( dualListAction == DualListAction.MoveSelected )
                    {
                        DoAction( dualListAction, true, i );
                    }
                    else // copy
                    {
                        DoAction( dualListAction, false, i );
                        i++;
                    }

                    if ( i >= listBoxFrom.Items.Count )
                    {
                        i = listBoxFrom.Items.Count - 1;
                    }
                    listBoxFrom.SelectedIndex = i;
                }
            }
            else // multi
            {
                foreach ( int x in listBoxFrom.SelectedIndices )
                {
                    DoAction( dualListAction, false, x );
                }

                int i;
                if ( listBoxFrom.SelectedIndices.Count > 0 )
                {
                    i = listBoxFrom.SelectedIndices[listBoxFrom.SelectedIndices.Count - 1] + 1;
                }
                else
                {
                    return;
                }

                for ( int t = listBoxFrom.Items.Count - 1; t >= 0; t-- )
                {
                    if ( dualListAction == DualListAction.MoveSelected )
                    {
                        if ( listBoxFrom.SelectedIndices.Contains( t ) )
                        {
                            i = t;
                            listBoxFrom.Items.RemoveAt( t );
                        }
                    }
                    else // copy
                    {
                        listBoxFrom.SetSelected( t, false );
                    }
                }

                if ( i > listBoxFrom.Items.Count )
                {
                    i = listBoxFrom.Items.Count - 1;
                }
                if ( i > -1 && listBoxFrom.Items.Count > i )
                {
                    listBoxFrom.SetSelected( i, true );
                }

            }
        }

        private void DoAction( DualListAction dualListAction, bool remove, int index )
        {
            DualListCancelEventArgs e = new DualListCancelEventArgs( dualListAction, ListBoxFrom.Items[index] );
            if ( BeforeAction != null )
            {
                BeforeAction( this, e );
            }

            if ( !e.Cancel )
            {
                int newIndex = ListBoxTo.Items.Add( e.Item );
                ListBoxTo.SelectedIndices.Add( newIndex );

                if ( remove )
                {
                    listBoxFrom.Items.RemoveAt( index );
                }

                if ( AfterAction != null )
                {
                    DualListActionEventArgs f = new DualListActionEventArgs( dualListAction, e.Item, newIndex );
                    AfterAction( this, f );
                }
            }
        }

        private void EnableDisable()
        {
            if ( AutoDisableButton && !busy && Button != null && ListBoxFrom != null )
            {
                Button.Enabled = ListBoxFrom.SelectedIndex > -1;
            }
        }

        private void Click()
        {
            if ( Button != null && ListBoxFrom != null && ListBoxTo != null )
            {
                switch ( Action )
                {
                    case DualListAction.MoveSelected:
                        MoveSelected();
                        break;
                    case DualListAction.CopySelected:
                        CopySelected();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
