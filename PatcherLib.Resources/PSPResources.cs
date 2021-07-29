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
using System.Collections.ObjectModel;

namespace PatcherLib
{
    using PatcherLib.Datatypes;
    using PatcherLib.Utilities;
    using Paths = ResourcesClass.Paths.PSP;
    using System.Xml;

    public static partial class PSPResources
    {
        private static IList<string> _characterSet = null;
        public static IList<string> CharacterSet 
        {
            get
            {
                if (_characterSet == null)
                {
                    string[] characterSet = new string[2200];
                    PSXResources.CharacterSet.CopyTo(characterSet, 0);
                    characterSet[0x95] = " ";
                    characterSet[0x880] = "á";
                    characterSet[0x881] = "à";
                    characterSet[0x882] = "é";
                    characterSet[0x883] = "è";
                    characterSet[0x884] = "í";
                    characterSet[0x885] = "ú";
                    characterSet[0x886] = "ù";
                    characterSet[0x887] = "-";
                    characterSet[0x888] = "—";
                    _characterSet = characterSet.AsReadOnly();
                }

                return _characterSet;
            }
        }

        public static System.Drawing.Image ICON0_PNG
        {
            get
            {
                byte[] mem = Binaries.ICON0.ToArray();
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream(mem, false))
                {
                    return System.Drawing.Image.FromStream(stream);
                }
            }
        }

        public static FFTFont PSPFont
        {
            get
            {
                return new FFTFont(PatcherLib.PSPResources.Binaries.Font, PatcherLib.PSPResources.Binaries.FontWidths);
            }
        }

        private static readonly XmlDocument eventNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.EventNamesXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument abilityEffectsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.AbilityEffectsXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument itemsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.ItemsXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument jobsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.JobsXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument skillSetsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.SkillSetsXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument specialNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.SpecialNamesXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument spriteSetsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.SpriteSetsXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument statusNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.StatusNamesXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument itemsStringsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.ItemsStringsXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument mapNamesDoc = ResourcesClass.ZipFileContents[Paths.MapNamesXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument abilitiesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.AbilitiesNamesXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument abilitiesStringsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.AbilitiesStringsXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument shopNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.ShopNamesXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument unitNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.UnitNamesXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument spriteFilesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.SpriteFilesXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument chronicleDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.ChroniclesXML].ToUTF8String().ToXmlDocument();
        private static readonly XmlDocument propositionsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.PropositionsXML].ToUTF8String().ToXmlDocument();
    }
}
