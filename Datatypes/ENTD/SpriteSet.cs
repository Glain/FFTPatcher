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
    /// Represents the sprite set of a unit.
    /// </summary>
    public class SpriteSet
    {
		#region Instance Variables (2) 

        private static SpriteSet[] pspSpriteSets = new SpriteSet[256];
        private static SpriteSet[] psxSpriteSets = new SpriteSet[256];

		#endregion Instance Variables 

		#region Public Properties (3) 

        public string Name { get; private set; }

        public static SpriteSet[] SpriteSets
        {
            get { return FFTPatch.Context == Context.US_PSP ? pspSpriteSets : psxSpriteSets; }
        }

        public byte Value { get; private set; }

		#endregion Public Properties 

		#region Constructors (2) 

        static SpriteSet()
        {
            IList<string> pspSpriteNames = PSPResources.Lists.SpriteSets;
            IList<string> psxSpriteNames = PSXResources.Lists.SpriteSets;

            for( int i = 0; i < 256; i++ )
            {
                pspSpriteSets[i] = new SpriteSet( (byte)i, pspSpriteNames[i] );
                psxSpriteSets[i] = new SpriteSet( (byte)i, psxSpriteNames[i] );
            }
        }

        private SpriteSet( byte value, string name )
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
