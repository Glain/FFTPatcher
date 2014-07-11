using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FFTPatcher.SpriteEditor
{
    class SeparatorComboBox : ComboBox
    {
        public class SeparatorItem
        {
            public object Data { get; private set; }
            public SeparatorItem( object data )
            {
                Data = data;
            }

            public override string ToString()
            {
                if ( Data != null )
                {
                    return Data.ToString();
                }
                return base.ToString();
            }

        }

        private const int separatorHeight = 3;
        private const int verticalItemPadding = 4;

        protected override void OnMeasureItem( MeasureItemEventArgs e )
        {
            base.OnMeasureItem( e );
            if ( e.Index != -1 )
            {
                object item = Items[e.Index];
                string text = GetDisplayText( e.Index );
                if (string.IsNullOrEmpty( text ))
                {
                    text = "WHOAH";
                }
                System.Drawing.Size textSize = TextRenderer.MeasureText(text, Font );
                e.ItemHeight = textSize.Height + verticalItemPadding;
                e.ItemWidth = textSize.Width;
                if ( item is SeparatorItem )
                {
                    e.ItemHeight += separatorHeight;
                }
            }
        }

        public SeparatorComboBox()
            : base()
        {
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
        }

        private string GetDisplayText( int index )
        {
            object item = Items[index];
            return item.ToString();
        }

        protected override void OnDrawItem( DrawItemEventArgs e )
        {
            base.OnDrawItem( e );

            if ( e.Index != -1 )
            {
                object item = Items[e.Index];
                e.DrawBackground();
                e.DrawFocusRectangle();

                bool separator = item is SeparatorItem;

                Rectangle bounds = e.Bounds;
                bool drawSeparator = separator && ( e.State & DrawItemState.ComboBoxEdit ) != DrawItemState.ComboBoxEdit;
                if ( drawSeparator )
                {
                    bounds.Height -= separatorHeight;
                }

                TextRenderer.DrawText( e.Graphics, GetDisplayText( e.Index ), Font, bounds, e.ForeColor, e.BackColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter );

                if ( drawSeparator )
                {
                    Rectangle sepRect = new Rectangle( e.Bounds.Left, e.Bounds.Bottom - separatorHeight, e.Bounds.Width, separatorHeight );
                    using ( Brush b = new SolidBrush( BackColor ) )
                    {
                        e.Graphics.FillRectangle( b, sepRect );
                    }
                    e.Graphics.DrawLine( SystemPens.ControlText, sepRect.Left + 2, sepRect.Top + 1, sepRect.Right - 2, sepRect.Top + 1 );
                }
            }
                    
        }
    }
}
