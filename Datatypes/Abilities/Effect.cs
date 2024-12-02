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

        public static readonly IDictionary<UInt16, Effect> PSPEffects = CreateEffectMap(PSPResources.Lists.AbilityEffects);

        public static readonly IDictionary<UInt16, Effect> PSXEffects = CreateEffectMap(PSXResources.Lists.AbilityEffects);

        public UInt16 Value { get; private set; }

		#endregion Public Properties 

		#region Constructors

        private Effect( UInt16 value, string name )
        {
            Value = value;
            Name = name;
        }

        #endregion Constructors 

        private static IDictionary<UInt16, Effect> CreateEffectMap(IList<string> abilityEffects)
        {
            Dictionary<UInt16, Effect> effectMap = new Dictionary<UInt16, Effect>(Settings.Prefs.ShowAllAbilityEffects ? 65536 : 513);

            for (UInt16 i = 0; i < 512; i++)
            {
                effectMap[i] = new Effect(i, abilityEffects[i]);
            }

            if (Settings.Prefs.ShowAllAbilityEffects)
            {
                for (UInt16 i = 512; i < 65535; i++)
                {
                    effectMap[i] = new Effect(i, "");
                }   
            }

            effectMap[0xFFFF] = new Effect(0xFFFF, "");
            return new PatcherLib.Datatypes.ReadOnlyDictionary<UInt16, Effect>(effectMap);
        }

		#region Public Methods

        public override string ToString()
        {
            return string.Format( "{0:X3} {1}", Value, Name );
        }

		#endregion Public Methods 
    }
}
