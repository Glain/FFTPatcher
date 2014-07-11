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
    public partial class CodeCreator : UserControl
    {
		#region Constructors (1) 

        public CodeCreator()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public void UpdateView()
        {
            OnVisibleChanged( EventArgs.Empty );
        }

		#endregion Public Methods 

		#region Protected Methods (1) 

        protected override void OnVisibleChanged( EventArgs e )
        {
            textBox1.Text = Codes.GetAllCodes();
            base.OnVisibleChanged( e );
        }

		#endregion Protected Methods 
    }
}
