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

using System.Reflection;
using System.Windows.Forms;

namespace FFTPatcher
{
    public partial class About : Form
    {
		#region Constructors (1) 

        public About()
        {
            InitializeComponent();
            textBox.Text =
@"Credits:
    ZodiacFFTM: For his awesome Excel spreadsheets on which this application is based.
    Dakitty: Found the locations of the Dark Knight, Onion Knight, Balthier, Luso, Argath, Aliste, and Bremondt sprites in the fftpack.bin file
    NeXaR: Gave me the hint that I could just overwrite the encrypted PSP binary with a decrypted version
    Raijinili: Figured out what some of the checkboxes in the ENTD editor do
    Weltall: CWCheat
    aerostar: Final Fantasy Tactics Battle Mechanics Guide
    element109: Used some code from his ImageMaster (http://imagemaster.codeplex.com/)

Copyright 2007, Joe Davidson <joedavidson@gmail.com>

FFTPatcher is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

FFTPatcher is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.";
            versionLabel.Text = string.Format( "v0.{0}", Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString() );
        }

		#endregion Constructors 
    }
}

