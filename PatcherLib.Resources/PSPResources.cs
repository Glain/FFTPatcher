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
        private static XmlDocument eventNamesDoc;

        private static XmlDocument abilityEffectsDoc;
        private static XmlDocument itemsDoc;
        private static XmlDocument jobsDoc;
        private static XmlDocument skillSetsDoc;
        private static XmlDocument specialNamesDoc;
        private static XmlDocument spriteSetsDoc;
        private static XmlDocument statusNamesDoc;

        private static XmlDocument itemsStringsDoc;
        private static XmlDocument mapNamesDoc;
        private static XmlDocument abilitiesDoc;

        private static XmlDocument abilitiesStringsDoc;

        private static XmlDocument shopNamesDoc;
        private static XmlDocument unitNamesDoc;

        private static XmlDocument spriteFilesDoc;
        private static XmlDocument chronicleDoc;
        private static XmlDocument propositionsDoc;

        public static IList<string> CharacterSet { get; private set; }

        //static Dictionary<string, object> dict = new Dictionary<string, object>();

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

        static PSPResources()
        {
            Binaries.Propositions = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.Propositions].AsReadOnly();
            var defaultProps = ResourcesClass.DefaultZipFileContents[ResourcesClass.Paths.PSP.Binaries.Propositions].AsReadOnly();
            if (Binaries.Propositions.Count < defaultProps.Count)
            {
                List<byte> newProps = new List<byte>( defaultProps.Count );
                newProps.AddRange( Binaries.Propositions );
                newProps.AddRange(
                    defaultProps.Sub( Binaries.Propositions.Count ) );
                Binaries.Propositions = newProps.AsReadOnly();
            }
            Binaries.StoreInventories = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.StoreInventories].AsReadOnly();
            Binaries.ENTD1 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ENTD1].AsReadOnly();
            Binaries.ENTD2 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ENTD2].AsReadOnly();
            Binaries.ENTD3 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ENTD3].AsReadOnly();
            Binaries.ENTD4 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ENTD4].AsReadOnly();
            Binaries.ENTD5 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ENTD5].AsReadOnly();
            Binaries.MoveFind = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.MoveFind].AsReadOnly();
            Binaries.Abilities = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.Abilities].AsReadOnly();
            Binaries.AbilityAnimations = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.AbilityAnimations].AsReadOnly();
            Binaries.AbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.AbilityEffects].AsReadOnly();
            Binaries.ItemAbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ItemAbilityEffects].AsReadOnly();
            Binaries.ReactionAbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ReactionAbilityEffects].AsReadOnly();
            Binaries.ActionEvents = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ActionEvents].AsReadOnly();
            Binaries.Font = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.Font].AsReadOnly();
            Binaries.FontWidths = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.FontWidths].AsReadOnly();
            Binaries.ICON0 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ICON0].AsReadOnly();
            Binaries.InflictStatuses = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.InflictStatuses].AsReadOnly();
            Binaries.JobLevels = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.JobLevels].AsReadOnly();
            Binaries.JobFormationSprites1 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.JobFormationSprites1].AsReadOnly();
            Binaries.JobFormationSprites2 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.JobFormationSprites2].AsReadOnly();
            Binaries.Jobs = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.Jobs].AsReadOnly();
            Binaries.MonsterSkills = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.MonsterSkills].AsReadOnly();
            Binaries.NewItemAttributes = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.NewItemAttributes].AsReadOnly();
            Binaries.NewItems = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.NewItems].AsReadOnly();
            Binaries.OldItemAttributes = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.OldItemAttributes].AsReadOnly();
            Binaries.OldItems = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.OldItems].AsReadOnly();
            Binaries.PoachProbabilities = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.PoachProbabilities].AsReadOnly();
            Binaries.SkillSets = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.SkillSets].AsReadOnly();
            Binaries.StatusAttributes = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.StatusAttributes].AsReadOnly();


            propositionsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.PropositionsXML].ToUTF8String().ToXmlDocument();
            eventNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.EventNamesXML].ToUTF8String().ToXmlDocument();
            jobsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.JobsXML].ToUTF8String().ToXmlDocument();
            skillSetsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.SkillSetsXML].ToUTF8String().ToXmlDocument();
            specialNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.SpecialNamesXML].ToUTF8String().ToXmlDocument();
            spriteSetsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.SpriteSetsXML].ToUTF8String().ToXmlDocument();
            statusNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.StatusNamesXML].ToUTF8String().ToXmlDocument();
            abilitiesStringsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.AbilitiesStringsXML].ToUTF8String().ToXmlDocument();
            abilityEffectsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.AbilityEffectsXML].ToUTF8String().ToXmlDocument();
            //dict[Resources.Paths.PSP.ItemAttributesXML] = Resources.ZipFileContents[Resources.Paths.PSP.ItemAttributesXML].ToUTF8String();
            itemsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.ItemsXML].ToUTF8String().ToXmlDocument();
            itemsStringsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.ItemsStringsXML].ToUTF8String().ToXmlDocument();
            shopNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.ShopNamesXML].ToUTF8String().ToXmlDocument();
            mapNamesDoc = ResourcesClass.ZipFileContents[Paths.MapNamesXML].ToUTF8String().ToXmlDocument();

            abilitiesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.AbilitiesNamesXML].ToUTF8String().ToXmlDocument();
            unitNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.UnitNamesXML].ToUTF8String().ToXmlDocument();
            spriteFilesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.SpriteFilesXML].ToUTF8String().ToXmlDocument();
            chronicleDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.ChroniclesXML].ToUTF8String().ToXmlDocument();

            string[] characterSet = new string[2200];
            PSXResources.CharacterSet.CopyTo( characterSet, 0 );
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
            CharacterSet = characterSet.AsReadOnly();
        }

    }
}
