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

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FFTPatcher.Controls
{
    /// <summary>
    /// Represents a <see cref="CheckedListBox"/> that allows default values to be specified.
    /// </summary>
    public partial class CheckedListBoxNoHighlightWithDefault : CheckedListBox
    {
		#region Instance Variables (1) 

        private bool[] defaults;

		#endregion Instance Variables 

		#region Public Properties (2) 

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        public new bool CheckOnClick { get { return base.CheckOnClick; } private set { base.CheckOnClick = value; } }

        /// <summary>
        /// Gets the default values for the items in this <see cref="CheckedListBoxNoHighlightWithDefault"/>.
        /// </summary>
        public bool[] Defaults
        {
            get { return defaults; }
            private set { defaults = value; }
        }

		#endregion Public Properties 

		#region Constructors (1) 

        public CheckedListBoxNoHighlightWithDefault()
        {
            CheckOnClick = true;
        }

		#endregion Constructors 

		#region Public Methods (1) 

        /// <summary>
        /// Sets a list of values and their defaults.
        /// </summary>
        public void SetValuesAndDefaults( bool[] values, bool[] defaults )
        {
            if( (values != null) && (defaults != null) && (this.defaults == null) )
            {
                this.defaults = defaults;
                for( int i = 0; i < Items.Count; i++ )
                {
                    SetItemChecked( i, values[i] );
                    RefreshItem( i );
                }
            }
            else if( (values != null) && (defaults != null) && (this.defaults != null) )
            {
                List<int> itemsToRefresh = new List<int>( values.Length );
                for( int i = 0; i < Items.Count; i++ )
                {
                    if( (i >= this.defaults.Length) || (i >= values.Length) || (i >= defaults.Length) || (i >= Items.Count) ||
                        ((GetItemChecked( i ) ^ this.defaults[i]) && !(values[i] ^ defaults[i])) ||
                        (!(GetItemChecked( i ) ^ this.defaults[i]) && (values[i] ^ defaults[i])) )
                    {
                        itemsToRefresh.Add( i );
                    }
                }

                this.defaults = defaults;
                for( int i = 0; i < Items.Count; i++ )
                {
                    SetItemChecked( i, values[i] );
                }

                foreach( int i in itemsToRefresh )
                {
                    SetItemChecked( i, !values[i] );
                    SetItemChecked( i, values[i] );
                }
            }
            Invalidate();
        }

		#endregion Public Methods 

		#region Protected Methods (3) 

        protected override void OnEnabledChanged(System.EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnDrawItem( DrawItemEventArgs e )
        {
            bool changed =
                (Defaults != null) &&
                (Defaults.Length == Items.Count) &&
                (e.Index != -1) &&
                (e.Index < Defaults.Length) &&
                (e.Index < Items.Count) &&
                GetItemChecked( e.Index ) ^ Defaults[e.Index];

            Color backColor = Color.Empty;
            Color foreColor = Color.Empty;
            if( !Enabled && changed )
            {
                backColor = Color.LightBlue;
                foreColor = SystemColors.GrayText;
            }
            else if( Enabled && changed )
            {
                backColor = Settings.ModifiedColor.BackgroundColor;
                foreColor = Settings.ModifiedColor.ForegroundColor;
            }
            else if( !Enabled && !changed )
            {
                backColor = BackColor;
                foreColor = SystemColors.GrayText;
            }
            else
            {
                backColor = BackColor;
                foreColor = ForeColor;
            }

            using( Brush backColorBrush = new SolidBrush( backColor ) )
            using( Brush foreColorBrush = new SolidBrush( foreColor ) )
            {
                e.Graphics.FillRectangle( backColorBrush, e.Bounds );
                if( (e.Index < Items.Count) && (e.Index != -1) )
                {
                    CheckBoxState state = this.GetItemChecked( e.Index ) ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                    Size checkBoxSize = CheckBoxRenderer.GetGlyphSize( e.Graphics, state );
                    Point loc = new Point( 1, (e.Bounds.Height - (checkBoxSize.Height + 1)) / 2 + 1 );
                    CheckBoxRenderer.DrawCheckBox( e.Graphics, new Point( loc.X + e.Bounds.X, loc.Y + e.Bounds.Y ), state );
                    e.Graphics.DrawString( this.Items[e.Index].ToString(), e.Font, foreColorBrush, new PointF( loc.X + checkBoxSize.Width + 1 + e.Bounds.X, loc.Y + e.Bounds.Y ) );
                }
            }
        }

        protected override void OnItemCheck( ItemCheckEventArgs e )
        {
            RefreshItem( e.Index );
            base.OnItemCheck( e );
        }

        protected override void OnKeyDown( KeyEventArgs e )
        {
            SetValuesAndDefaults( Defaults, Defaults );
            base.OnKeyDown( e );
        }

		#endregion Protected Methods 
    }
}
