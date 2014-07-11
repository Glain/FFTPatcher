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
using System.Collections.Generic;
using PatcherLib;

namespace FFTPatcher.Datatypes
{
    public class Effect
    {
		#region Public Properties (4) 

        public string Name { get; private set; }

        public static IDictionary<UInt16, Effect> PSPEffects { get; private set; }

        public static IDictionary<UInt16, Effect> PSXEffects { get; private set; }

        public UInt16 Value { get; private set; }

		#endregion Public Properties 

		#region Constructors (2) 

        static Effect()
        {
            PSXEffects = new Dictionary<UInt16, Effect>( 513 );
            PSPEffects = new Dictionary<UInt16, Effect>( 513 );

            for( UInt16 i = 0; i < 512; i++ )
            {
                PSPEffects[i] = new Effect( i, PSPResources.Lists.AbilityEffects[i] );
                PSXEffects[i] = new Effect( i, PSXResources.Lists.AbilityEffects[i] );
            }

            PSPEffects[0xFFFF] = new Effect( 0xFFFF, "" );
            PSXEffects[0xFFFF] = new Effect( 0xFFFF, "" );

            PSPEffects = new PatcherLib.Datatypes.ReadOnlyDictionary<UInt16, Effect>( PSPEffects );
            PSXEffects = new PatcherLib.Datatypes.ReadOnlyDictionary<UInt16, Effect>( PSXEffects );
        }

        private Effect( UInt16 value, string name )
        {
            Value = value;
            Name = name;
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public override string ToString()
        {
            return string.Format( "{0:X3} {1}", Value, Name );
        }

		#endregion Public Methods 
    }
}
