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

using System.Collections.Generic;
using PatcherLib;
using PatcherLib.Datatypes;

namespace FFTPatcher.Datatypes
{
    public class AbilityFormula
    {
        #region Public Properties (6) 

        public string Formula { get; private set; }

        public static Dictionary<byte, AbilityFormula> PSPAbilityFormulaHash { get; private set; }

        public static List<AbilityFormula> PSPAbilityFormulas { get; private set; }

        public static Dictionary<byte, AbilityFormula> PSXAbilityFormulaHash { get; private set; }

        public static List<AbilityFormula> PSXAbilityFormulas { get; private set; }

        public byte Value { get; private set; }

        #endregion Public Properties 

        public static Dictionary<byte, AbilityFormula> GetAbilityFormulaHash(Context context)
        {
            if (context == Context.US_PSP)
                return PSPAbilityFormulaHash;
            else
                return PSXAbilityFormulaHash;
        }

        public static void InitAbilityFormulas()
        {
            IDictionary<byte, string> psxMap = ResourcesClass.GetAbilityFormulas(Context.US_PSX);
            IDictionary<byte, string> pspMap = ResourcesClass.GetAbilityFormulas(Context.US_PSP);

            PSXAbilityFormulas = new List<AbilityFormula>(psxMap.Count);
            PSXAbilityFormulaHash = new Dictionary<byte, AbilityFormula>(psxMap.Count);

            PSPAbilityFormulas = new List<AbilityFormula>(pspMap.Count);
            PSPAbilityFormulaHash = new Dictionary<byte, AbilityFormula>(pspMap.Count);

            foreach (KeyValuePair<byte, string> kvp in psxMap)
            {
                AbilityFormula a = new AbilityFormula();
                a.Value = kvp.Key;
                a.Formula = kvp.Value;

                PSXAbilityFormulas.Add(a);
                PSXAbilityFormulaHash.Add(a.Value, a);
            }

            foreach (KeyValuePair<byte, string> kvp in pspMap)
            {
                AbilityFormula a = new AbilityFormula();
                a.Value = kvp.Key;
                a.Formula = kvp.Value;
                PSPAbilityFormulas.Add(a);
                PSPAbilityFormulaHash.Add(a.Value, a);
            }
        }

        #region Constructors

        static AbilityFormula()
        {
            InitAbilityFormulas();
        }

        private AbilityFormula()
        {
        }

		#endregion Constructors 

		#region Public Methods (1) 

        public override string ToString()
        {
            return string.Format( "{0:X2} {1}", Value, Formula );
        }

		#endregion Public Methods 
    }
}
