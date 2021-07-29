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
        private static SpriteSet[] _pspSpriteSets;
        public static SpriteSet[] PspSpriteSets
        {
            get
            {
                if (_pspSpriteSets == null)
                {
                    _pspSpriteSets = new SpriteSet[256];

                    for (int i = 0; i < 256; i++)
                    {
                        _pspSpriteSets[i] = new SpriteSet((byte)i, PSPResources.Lists.SpriteSets[i]);
                    }
                }

                return _pspSpriteSets;
            }
        }

        private static SpriteSet[] _psxSpriteSets;
        public static SpriteSet[] PsxSpriteSets
        {
            get
            {
                if (_psxSpriteSets == null)
                {
                    _psxSpriteSets = new SpriteSet[256];

                    for (int i = 0; i < 256; i++)
                    {
                        _psxSpriteSets[i] = new SpriteSet((byte)i, PSXResources.Lists.SpriteSets[i]);
                    }
                    
                }

                return _psxSpriteSets;
            }
        }

		#region Public Properties

        public string Name { get; private set; }

        /*
        public static SpriteSet[] SpriteSets
        {
            get { return FFTPatch.Context == Context.US_PSP ? pspSpriteSets : psxSpriteSets; }
        }
        */

        public static SpriteSet[] GetSpriteSets(Context context)
        {
            return (context == Context.US_PSP) ? PspSpriteSets : PsxSpriteSets;
        }

        public byte Value { get; private set; }

		#endregion Public Properties 

		#region Constructors

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
