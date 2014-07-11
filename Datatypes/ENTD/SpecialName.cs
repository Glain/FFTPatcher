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

using PatcherLib;
using PatcherLib.Datatypes;
using System.Collections.Generic;
namespace FFTPatcher.Datatypes
{
    /// <summary>
    /// Represents a special "name" a unit can have.
    /// </summary>
    public class SpecialName
    {
		#region Instance Variables (2) 

        private static SpecialName[] pspNames = new SpecialName[256];
        private static SpecialName[] psxNames = new SpecialName[256];

		#endregion Instance Variables 

		#region Public Properties (3) 

        public string Name { get; private set; }

        public static SpecialName[] SpecialNames
        {
            get { return FFTPatch.Context == Context.US_PSP ? pspNames : psxNames; }
        }

        public byte Value { get; private set; }

		#endregion Public Properties 

		#region Constructors (2) 

        static SpecialName()
        {
            IList<string> pspStrings = PSPResources.Lists.SpecialNames;
            IList<string> psxStrings = PSXResources.Lists.SpecialNames;

            for( int i = 0; i < 256; i++ )
            {
                pspNames[i] = new SpecialName( (byte)i, pspStrings[i] );
                psxNames[i] = new SpecialName( (byte)i, psxStrings[i] );
            }
        }

        private SpecialName( byte value, string name )
        {
            Value = value;
            Name = name;
        }

		#endregion Constructors 

		#region Public Methods (2) 

        public byte ToByte()
        {
            return Value;
        }

        public override string ToString()
        {
            return string.Format( "{0:X2} {1}", Value, Name );
        }

		#endregion Public Methods 
    }
}
