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
using FFTPatcher.Datatypes;
using FFTPatcher.Editors;
using PatcherLib;

namespace FFTPatcher
{
    static class Program
    {
		#region Private Methods (1) 

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Access some class members to force the static constructors to run.
            object dummy = AllAbilities.PSPNames;
            dummy = PSPResources.Lists.AbilityAttributes;
            dummy = PSXResources.Lists.AbilityAttributes;
            dummy = FFTPatchEditor.ToolTip;
            dummy = SkillSet.DummySkillSets;
            dummy = AllMonsterSkills.PSXNames;
            dummy = AllJobs.Names;
            dummy = ActionMenuEntry.AllActionMenuEntries;
            dummy = ShopAvailability.AllAvailabilities;
            dummy = SpriteSet.SpriteSets;
            dummy = SpecialName.SpecialNames;
            dummy = Event.EventNames;

            Application.Run(new MainForm());
        }

		#endregion Private Methods 
    }
}
