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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PatcherLib.Controls
{
    [System.ComponentModel.ToolboxItem( true )]
    [ToolboxBitmap( typeof( ProgressBar ) )]
    [System.ComponentModel.Designer( typeof( ProgressBarWithTextDesigner ), typeof( System.ComponentModel.Design.IDesigner ) )]
    public class ProgressBarWithText : Label
    {
        private const int maxBarWidth = 20;
        private const int maxBarSpace = 10;

        private int value = 0;
        private int maximum = 100;

        private int progressBarBlockWidth = 6;
        private int progressBarBlockSpace = 1;
        private string progressBarText = string.Empty;
        private bool progressBarMarginOffset = true;

        private SolidBrush progressBarFillBrush;

        public ProgressBarWithText()
        {
            progressBarFillBrush = new SolidBrush( Color.Coral );

            BackColor = Color.White;
            ForeColor = Color.Blue;
            base.AutoSize = false;
            base.TextAlign = ContentAlignment.MiddleCenter;
        }

        protected override void Dispose( bool disposing )
        {
            try
            {
                if ( disposing )
                {
                }
                progressBarFillBrush.Dispose();
            }
            finally
            {
                base.Dispose( disposing );
            }
        }

        [System.ComponentModel.Category( "Custom" )]
        [System.ComponentModel.DefaultValue( typeof( Color ), "Coral" )]
        public Color ProgressBarFillColor
        {
            get { return progressBarFillBrush.Color; }
            set
            {
                if ( progressBarFillBrush.Color != value )
                {
                    progressBarFillBrush.Color = value;
                    Invalidate();
                }
            }
        }

        [System.ComponentModel.Category( "Custom" )]
        [System.ComponentModel.DefaultValue( 6 )]
        public int ProgressBarBlockWidth
        {
            get { return progressBarBlockWidth; }
            set
            {
                if ( progressBarBlockWidth != value )
                {
                    progressBarBlockWidth = Math.Min( Math.Max( 1, value ), value );
                    Invalidate();
                }
            }
        }

        [System.ComponentModel.Category( "Custom" )]
        [System.ComponentModel.DefaultValue( 1 )]
        public int ProgressBarBlockSpace
        {
            get { return progressBarBlockSpace; }
            set
            {
                if ( progressBarBlockSpace != value )
                {
                    progressBarBlockSpace = Math.Min( Math.Max( 0, value ), maxBarSpace );
                    Invalidate();
                }
            }
        }

        [System.ComponentModel.Category( "Custom" )]
        [System.ComponentModel.DefaultValue( "" )]
        public string ProgressBarText
        {
            get { return progressBarText; }
            set
            {
                if ( progressBarText != value )
                {
                    progressBarText = value;
                    Invalidate();
                }
            }
        }

        [System.ComponentModel.Category( "Custom" )]
        [System.ComponentModel.DefaultValue( true )]
        public bool ProgressBarMarginOffset
        {
            get { return progressBarMarginOffset; }
            set
            {
                if ( progressBarMarginOffset != value )
                {
                    progressBarMarginOffset = value;
                    Invalidate();
                }
            }
        }

        [System.ComponentModel.Category( "Custom" )]
        [System.ComponentModel.DefaultValue( 100 )]
        public int Maximum
        {
            get { return maximum; }
            set
            {
                if ( maximum != value )
                {
                    maximum = Math.Max( 1, value );
                    this.value = Math.Min( this.value, maximum );
                    Invalidate();
                }
            }
        }

        [System.ComponentModel.Category( "Custom" )]
        [System.ComponentModel.DefaultValue( 0 )]
        public int Value
        {
            get { return value; }
            set
            {
                this.value = Math.Min( Math.Max( 0, value ), maximum );
                Invalidate();
                Update();
            }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new bool AutoEllipsis
        {
            get { return base.AutoEllipsis; }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new System.Drawing.ContentAlignment TextAlign
        {
            get
            {
                return base.TextAlign;
            }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new bool CausesValidation
        {
            get { return base.CausesValidation; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new bool AllowDrop
        {
            get { return base.AllowDrop; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new Padding Padding
        {
            get { return base.Padding; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new ImeMode ImeMode
        {
            get { return base.ImeMode; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new bool TabStop
        {
            get { return base.TabStop; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new bool UseCompatibleTextRendering
        {
            get { return base.UseCompatibleTextRendering; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new bool UseMnemonic
        {
            get { return base.UseMnemonic; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new Image Image
        {
            get { return base.Image; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new System.Drawing.ContentAlignment ImageAlign
        {
            get { return base.ImageAlign; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new int ImageIndex
        {
            get { return base.ImageIndex; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new string ImageKey
        {
            get { return base.ImageKey; }
        }

        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new ImageList ImageList
        {
            get { return base.ImageList; }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        [Browsable( false ), EditorBrowsable( EditorBrowsableState.Never )]
        public new string Text
        {
            get { return base.Text; }
        }

        protected override void OnPaint( PaintEventArgs e )
        {
            DrawProgressBar( e.Graphics );
            if ( progressBarText != string.Empty )
            {
                base.Text = ProgressBarText;
            }

            base.OnPaint( e );
        }

        private int GetTopOffSet()
        {
            if ( !progressBarMarginOffset )
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        private int GetLeftOffSet()
        {
            if ( !progressBarMarginOffset )
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        private void DrawProgressBar( Graphics g )
        {
            int leftOffset = GetLeftOffSet();
            int topOffset = GetTopOffSet();

            decimal percent = (decimal)value / (decimal)maximum;
            int valueWidth = (int)( ( ClientRectangle.Width - leftOffset * 2 ) * percent );
            int oneBlockWidth = progressBarBlockWidth + progressBarBlockSpace;
            int blockWidth = ( valueWidth / oneBlockWidth ) * oneBlockWidth;

            if ( percent > 0.99m && ( ClientRectangle.Width - leftOffset * 2 - blockWidth ) > 0 )
            {
                blockWidth += ( ClientRectangle.Width - leftOffset * 2 - blockWidth ) / oneBlockWidth;
            }

            int left = ClientRectangle.Left + leftOffset;
            int top = ClientRectangle.Top + topOffset;
            int height = ClientRectangle.Height - topOffset * 2;

            int drawnBlockWidth = oneBlockWidth;
            while ( drawnBlockWidth <= blockWidth )
            {
                g.FillRectangle( progressBarFillBrush, left, top, progressBarBlockWidth, height );
                left += oneBlockWidth;
                drawnBlockWidth += oneBlockWidth;
            }

            int tailBarWidth = ClientRectangle.Width - left - leftOffset;
            if ( tailBarWidth > 0 && tailBarWidth < oneBlockWidth )
            {
                drawnBlockWidth = ClientRectangle.Width - left - leftOffset;
                if ( drawnBlockWidth > 0 )
                {
                    g.FillRectangle( progressBarFillBrush, left, top, drawnBlockWidth, height );
                }
            }
        }

        public class ProgressBarWithTextDesigner : System.Windows.Forms.Design.ControlDesigner
        {
            private System.ComponentModel.Design.DesignerActionListCollection actionLists;
            public ProgressBarWithTextDesigner() { }

            public override System.ComponentModel.Design.DesignerActionListCollection ActionLists
            {
                get
                {
                    if ( actionLists == null )
                    {
                        actionLists = new System.ComponentModel.Design.DesignerActionListCollection();
                        actionLists.Add( new ProgressBarWithTextDesignerActionList( Component ) );
                    }

                    return actionLists;
                }
            }
        }

        public class ProgressBarWithTextDesignerActionList : System.ComponentModel.Design.DesignerActionList
        {
            public ProgressBarWithTextDesignerActionList( IComponent component ) : base( component ) { }

            private ProgressBarWithText ProgressBarWithText
            {
                get { return Component as ProgressBarWithText; }
            }

            public string ProgressBarText
            {
                get { return ProgressBarWithText.ProgressBarText; }
                set { SetProperty( "ProgressBarText", value ); }
            }

            public Color ProgressBarFillColor
            {
                get { return this.ProgressBarWithText.ProgressBarFillColor; }
                set { this.SetProperty( "ProgressBarFillColor", value ); }
            }

            public bool ProgressBarMarginOffset
            {
                get { return this.ProgressBarWithText.ProgressBarMarginOffset; }
                set { this.SetProperty( "ProgressBarMarginOffset", value ); }
            }

            public int ProgressBarBlockWidth
            {
                get { return this.ProgressBarWithText.ProgressBarBlockWidth; }
                set { this.SetProperty( "ProgressBarBlockWidth", value ); }
            }

            public int ProgressBarBlockSpace
            {
                get { return this.ProgressBarWithText.ProgressBarBlockSpace; }
                set { this.SetProperty( "ProgressBarBlockSpace", value ); }
            }

            public Color BackColor
            {
                get { return this.ProgressBarWithText.BackColor; }
                set { this.SetProperty( "BackColor", value ); }
            }

            public Color ForeColor
            {
                get { return this.ProgressBarWithText.ForeColor; }
                set { this.SetProperty( "ForeColor", value ); }
            }

            private void SetProperty( string propertyName, object value )
            {
                PropertyDescriptorCollection propertyCollection = TypeDescriptor.GetProperties( this.ProgressBarWithText );
                PropertyDescriptor property = propertyCollection[propertyName];
                property.SetValue( this.ProgressBarWithText, value );
            }
        }
    }


}
