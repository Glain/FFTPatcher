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
using System.IO;
using System.Text;
using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using PatcherLib.Utilities;

namespace FFTPatcher
{
    /// <summary>
    /// Utilities for generating cheat codes.
    /// </summary>
    public static class Codes
    {
		#region Instance Variables (2) 

        private const string euHeader = "_S ULES-00850\n_G Final Fantasy Tactics: The War of the Lions";
        private const string usHeader = "_S ULUS-10297\n_G Final Fantasy Tactics: The War of the Lions";

        private static Dictionary<CodeEnabledOnlyWhen, string> ConditionalCodeMap = new Dictionary<CodeEnabledOnlyWhen, string>
        {
            { CodeEnabledOnlyWhen.Battle, "D0067000 8888" },
            { CodeEnabledOnlyWhen.World, "D0067000 8A44" },
            { CodeEnabledOnlyWhen.AttackOut, "D01BF004 8016" },
            { CodeEnabledOnlyWhen.RequireOut, "D01BF020 5F88" }
        };

		#endregion Instance Variables 

		#region Public Methods (3) 
        public static IList<string> GenerateCodes( Context context, IList<byte> oldBytes, IList<byte> newBytes, UInt32 offset )
        {
            return GenerateCodes(context, oldBytes, newBytes, offset, CodeEnabledOnlyWhen.Any);
        }

        /// <summary>
        /// Generates codes based on context.
        /// </summary>
        public static IList<string> GenerateCodes( Context context, IList<byte> oldBytes, IList<byte> newBytes, UInt32 offset, CodeEnabledOnlyWhen when )
        {
            switch( context )
            {
                case Context.US_PSP:
                    return GeneratePSPCodes( oldBytes, newBytes, offset );
                case Context.US_PSX:
                    return GeneratePSXCodes( oldBytes, newBytes, offset, when );
            }

            return new List<string>();
        }

        /// <summary>
        /// Gets all codes.
        /// </summary>
        public static string GetAllCodes()
        {
            StringBuilder sb = new StringBuilder();

            Context context = FFTPatch.Context;

            IGenerateCodes[] generators = new IGenerateCodes[] {
                FFTPatch.Abilities,
                FFTPatch.Jobs,
                FFTPatch.SkillSets,
                FFTPatch.MonsterSkills,
                FFTPatch.ActionMenus,
                FFTPatch.StatusAttributes,
                FFTPatch.PoachProbabilities,
                FFTPatch.JobLevels,
                FFTPatch.Items,
                FFTPatch.ItemAttributes,
                FFTPatch.InflictStatuses,
                FFTPatch.MoveFind,
                FFTPatch.AbilityAnimations,
                FFTPatch.StoreInventories,
                FFTPatch.Propositions
            };
            foreach ( var gen in generators )
            {
                if ( gen != null )
                {
                    AddGroups( sb, 24, gen.GetCodeHeader( context ), gen.GenerateCodes( context ) );
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Adds lines of text in groups of a specified size to the StringBuilder.
        /// </summary>
        /// <param name="groupSize">Number of strings in each group</param>
        /// <param name="groupName">What to name each group.</param>
        /// <param name="lines">Lines to add</param>
        private static void AddGroups(StringBuilder sb, int groupSize, string groupName, IList<string> lines)
        {
            if (lines.Count == 0)
            {
                return;
            }
            else if (lines.Count <= groupSize)
            {
                if (groupName != string.Empty)
                    sb.Append(groupName + "\n");
                sb.AppendLines(lines);
            }
            else
            {
                int i = 0;
                int j = 1;
                for (i = 0; (i + 1) * groupSize < lines.Count; i++)
                {
                    if (groupName != string.Empty)
                        sb.Append(string.Format("{0}\\(part {1})\n", groupName, j++));
                    sb.AppendLines(lines.Sub(i * groupSize, (i + 1) * groupSize - 1));
                }

                if (groupName != string.Empty)
                    sb.Append(string.Format("{0}\\(part {1})\n", groupName, j++));
                sb.AppendLines(lines.Sub(i * groupSize, lines.Count - 1));
            }
        }


        /// <summary>
        /// Saves CWCheat codes to a file.
        /// </summary>
        public static void SaveToFile( string path )
        {
            string codes = GetAllCodes();
            using (StreamWriter stream = new StreamWriter(path, false))
            {
                stream.NewLine = "\n";
                stream.WriteLine(usHeader);
                stream.WriteLine(codes);
                stream.WriteLine(euHeader);
                stream.WriteLine(codes);
            }
        }

		#endregion Public Methods 

		#region Private Methods (2) 

        private static IList<string> GeneratePSPCodes(IList<byte> oldBytes, IList<byte> newBytes, UInt32 offset)
        {
            List<string> codes = new List<string>();
            bool[] patched = new bool[newBytes.Count];

            int i = 0;
            if( offset % 4 > 0 )
            {
                i = (int)(4 - (offset % 4));
            }

            // Generate 32bit codes
            for( ; i < newBytes.Count; i += 4 )
            {
                if( ((i + 3) < newBytes.Count) &&
                    ((newBytes[i] != oldBytes[i]) &&
                    (newBytes[i + 1] != oldBytes[i + 1]) &&
                    (newBytes[i + 2] != oldBytes[i + 2]) &&
                    (newBytes[i + 3] != oldBytes[i + 3])) &&
                    (!patched[i]) &&
                    (!patched[i + 1]) &&
                    (!patched[i + 2]) &&
                    (!patched[i + 3]) )
                {
                    UInt32 addy = (UInt32)(offset + i);
                    string code = string.Format( "_L 0x2{0:X7} 0x{4:X2}{3:X2}{2:X2}{1:X2}",
                        addy, newBytes[i], newBytes[i + 1], newBytes[i + 2], newBytes[i + 3] );
                    codes.Add( code );
                    patched[i] = true;
                    patched[i + 1] = true;
                    patched[i + 2] = true;
                    patched[i + 3] = true;
                }
            }

            // Generate 16bit codes
            for( i = (int)(offset % 2); i < newBytes.Count; i += 2 )
            {
                if( ((i + 1) < newBytes.Count) &&
                    ((newBytes[i] != oldBytes[i]) &&
                    (newBytes[i + 1] != oldBytes[i + 1])) &&
                    (!patched[i]) && (!patched[i + 1]) )
                {
                    UInt32 addy = (UInt32)(offset + i);
                    string code = string.Format( "_L 0x1{0:X7} 0x0000{2:X2}{1:X2}",
                        addy, newBytes[i], newBytes[i + 1] );
                    codes.Add( code );
                    patched[i] = true;
                    patched[i + 1] = true;
                }
            }

            // Generate 8bit codes
            for( i = 0; i < newBytes.Count; i++ )
            {
                if( (newBytes[i] != oldBytes[i]) && (!patched[i]) )
                {
                    UInt32 addy = (UInt32)(offset + i);
                    string code = string.Format( "_L 0x0{0:X7} 0x000000{1:X2}",
                        addy, newBytes[i] );
                    codes.Add( code );
                    patched[i] = true;
                }
            }

            // Sort them
            codes.Sort( ( s, t ) => s.Substring( 6 ).CompareTo( t.Substring( 6 ) ) );

            return codes.AsReadOnly();
        }

        public enum CodeEnabledOnlyWhen
        {
            Any = 0,
            Battle = 1,
            World = 2,
            AttackOut = 3,
            RequireOut = 4
        }

        private static IList<string> GeneratePSXCodes(IList<byte> oldBytes, IList<byte> newBytes, UInt32 offset, CodeEnabledOnlyWhen when)
        {
            List<string> codes = new List<string>();
            bool[] patched = new bool[newBytes.Count];

            // Generate 80h codes
            for (int i = (int)(offset % 2); i < newBytes.Count; i += 2)
            {
                if (((i + 1) < newBytes.Count) &&
                    ((newBytes[i] != oldBytes[i]) &&
                    (newBytes[i + 1] != oldBytes[i + 1])) &&
                    (!patched[i]) && (!patched[i + 1]))
                {
                    UInt32 addy = (UInt32)(offset + i);
                    string code = string.Format("80{0:X6} {2:X2}{1:X2}",
                        addy, newBytes[i], newBytes[i + 1]);
                    codes.Add(code);
                    patched[i] = true;
                    patched[i + 1] = true;
                }
            }

            // Generate 40h codes
            for (int i = 0; i < newBytes.Count; i++)
            {
                if ((newBytes[i] != oldBytes[i]) && (!patched[i]))
                {
                    UInt32 addy = (UInt32)(offset + i);
                    string code = string.Format("30{0:X6} 00{1:X2}",
                        addy, newBytes[i]);
                    codes.Add(code);
                    patched[i] = true;
                }
            }

            // Sort them
            codes.Sort((s, t) => s.Substring(2).CompareTo(t.Substring(2)));

            // Insert conditionals if necessary
            //const string worldConditional = "7013B900 F400";
            //const string battleConditional = "7014E61C F400";

            if (when != CodeEnabledOnlyWhen.Any)
            {
                //string conditional = (when == CodeEnabledOnlyWhen.Battle) ? battleConditional : worldConditional;
                string conditional = ConditionalCodeMap[when];

                List<string> realCodes = new List<string>(codes.Count * 2);
                foreach (string code in codes)
                {
                    realCodes.Add(conditional);
                    realCodes.Add(code);
                }
                codes = realCodes;
            }
            return codes.AsReadOnly();
        }

        private static IList<string> GeneratePSXCodes(IList<byte> oldBytes, IList<byte> newBytes, UInt32 offset)
        {
            return GeneratePSXCodes(oldBytes, newBytes, offset, CodeEnabledOnlyWhen.Any);
        }

		#endregion Private Methods 
    }
}
