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

namespace FFTPatcher.Editors
{
    public class BaseEditor : UserControl, INotifyDataChanged
    {
		#region Protected Methods (1) 

        protected void OnDataChanged( object sender, EventArgs args )
        {
            if( DataChanged != null )
            {
                DataChanged( this, args );
            }
        }

        protected void FireDataChanged()
        {
            OnDataChanged( this, EventArgs.Empty );
        }

		#endregion Protected Methods 

        /// <summary>
        /// Occurs when data in the object has changed.
        /// </summary>
        public event EventHandler DataChanged;
    }
}
