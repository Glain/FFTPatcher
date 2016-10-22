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
using FFTPatcher.Editors;

namespace FFTPatcher.Controls
{
    /// <summary>
    /// Represents a <see cref="NumericUpDown"/> that allows a default value to be set.
    /// </summary>
    public class NumericUpDownWithDefault : NumericUpDown
    {
		#region Public Properties (3) 

        public bool Default { get { return Value == DefaultValue; } }

        public decimal DefaultValue { get; private set; }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ReadOnly(true)]
        public new decimal Value
        {
            get { return base.Value; }
            private set { base.Value = value; }
        }

		#endregion Public Properties 

		#region Public Methods (1) 

        /// <summary>
        /// Sets the value and its default.
        /// </summary>
        public void SetValueAndDefault( decimal value, decimal defaultValue )
        {
            if( Hexadecimal )
                FFTPatchEditor.ToolTip.SetToolTip( this, string.Format( "Default: 0x{0:X2}", (int)defaultValue ) );
            else
                FFTPatchEditor.ToolTip.SetToolTip( this, string.Format( "Default: {0}", defaultValue ) );
            
            DefaultValue = defaultValue;
            Value = value > Maximum ? Maximum : value;
            OnValueChanged( EventArgs.Empty );
        }

		#endregion Public Methods 

		#region Private Methods (1) 

        private void SetColors()
        {
            if( Enabled && (Value != DefaultValue))
            {
                BackColor = Settings.ModifiedColor.BackgroundColor;
                ForeColor = Settings.ModifiedColor.ForegroundColor;
            }
            else if( Enabled && (Value==DefaultValue) )
            {
                BackColor = SystemColors.Window;
                ForeColor = SystemColors.WindowText;
            }
            else
            {
                BackColor = SystemColors.Window;
                ForeColor = SystemColors.WindowText;
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            SetColors();
            base.OnEnabledChanged(e);
        }

		#endregion Private Methods 

		#region Protected Methods (3) 

        protected override void OnEnter( EventArgs e )
        {
            Select( 0, Text.Length );
            base.OnEnter( e );
        }

        protected override void OnKeyDown( KeyEventArgs e )
        {
            if( e.KeyData == Keys.F12 )
            {
                SetValueAndDefault( DefaultValue, DefaultValue );
            }
            base.OnKeyDown( e );
        }

        protected override void OnValueChanged( EventArgs e )
        {
            base.OnValueChanged( e );
            SetColors();
        }

		#endregion Protected Methods 
    }
}
