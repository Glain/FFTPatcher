using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFTPatcher
{
    public class ReferenceEventArgs : EventArgs
    {
        public int Index { get; set; }
        public IEnumerable<int> ReferencingIndexes { get; set; }

        public ReferenceEventArgs(int index) : this(index, null) { }

        public ReferenceEventArgs(int Index, IEnumerable<int> ReferencingIndexes)
        {
            this.Index = Index;
            this.ReferencingIndexes = ReferencingIndexes;
        }
    }

    public class RepointEventArgs : EventArgs
    {
        public int OldID { get; set; }
        public int NewID { get; set; }

        public RepointEventArgs(int OldID, int NewID)
        {
            this.OldID = OldID;
            this.NewID = NewID;
        }
    }

    public static class PatchUtility
    {
        public static void RepointInflictStatus(AllItems items, AllAbilities abilities, AllInflictStatuses allInflictStatuses)
        {
            Dictionary<int, int> repointMap = GetRepointMap<InflictStatus>(allInflictStatuses.InflictStatuses);

            for (int itemIndex = 0; itemIndex < items.Items.Count; itemIndex++)
            {
                Item item = items.Items[itemIndex];
                if (item is Weapon)
                {
                    Weapon weapon = (Weapon)item;
                    if (weapon.Formula.Value != 2)
                    {
                        int newID = 0;
                        if (repointMap.TryGetValue(weapon.InflictStatus, out newID))
                        {
                            weapon.InflictStatus = (byte)newID;
                            weapon.OldInflictStatus = (byte)newID;
                            allInflictStatuses.InflictStatuses[newID].ReferencingItemIndexes.Add(itemIndex);
                        }
                    }
                }
                else if (item is ChemistItem)
                {
                    ChemistItem chemistItem = (ChemistItem)item;
                    if (chemistItem.Formula != 2)
                    {
                        int newID = 0;
                        if (repointMap.TryGetValue(chemistItem.InflictStatus, out newID))
                        {
                            chemistItem.InflictStatus = (byte)newID;
                            chemistItem.OldInflictStatus = (byte)newID;
                            allInflictStatuses.InflictStatuses[newID].ReferencingItemIndexes.Add(itemIndex);
                        }
                    }
                }
            }

            for (int abilityIndex = 0; abilityIndex < abilities.Abilities.Length; abilityIndex++)
            {
                Ability ability = abilities.Abilities[abilityIndex];
                if (!ability.IsNormal)
                    continue;

                AbilityAttributes abilityAttrs = ability.Attributes;
                if (abilityAttrs.Formula.Value != 2)
                {
                    int newID = 0;
                    if (repointMap.TryGetValue(abilityAttrs.InflictStatus, out newID))
                    {
                        abilityAttrs.InflictStatus = (byte)newID;
                        abilityAttrs.OldInflictStatus = (byte)newID;
                        allInflictStatuses.InflictStatuses[newID].ReferencingAbilityIDs.Add(abilityIndex);
                    }
                }
            }

            foreach (int index in repointMap.Keys)
            {
                InflictStatus inflictStatus = allInflictStatuses.InflictStatuses[index];
                inflictStatus.ReferencingItemIndexes.Clear();
                inflictStatus.ReferencingAbilityIDs.Clear();
            }
        }

        public static void RepointSpecificInflictStatus(AllItems items, AllAbilities abilities, AllInflictStatuses allInflictStatuses, byte oldID, byte newID)
        {
            for (int itemIndex = 0; itemIndex < items.Items.Count; itemIndex++)
            {
                Item item = items.Items[itemIndex];
                if (item is Weapon)
                {
                    Weapon weapon = (Weapon)item;
                    if ((weapon.Formula.Value != 2) && (weapon.InflictStatus == oldID))
                    {
                        weapon.InflictStatus = newID;
                        weapon.OldInflictStatus = newID;
                        allInflictStatuses.InflictStatuses[newID].ReferencingItemIndexes.Add(itemIndex);
                    }
                }
                else if (item is ChemistItem)
                {
                    ChemistItem chemistItem = (ChemistItem)item;
                    if ((chemistItem.Formula != 2) && (chemistItem.InflictStatus == oldID))
                    {
                        chemistItem.InflictStatus = newID;
                        chemistItem.OldInflictStatus = newID;
                        allInflictStatuses.InflictStatuses[newID].ReferencingItemIndexes.Add(itemIndex);
                    }
                }
            }

            for (int abilityIndex = 0; abilityIndex < abilities.Abilities.Length; abilityIndex++)
            {
                Ability ability = abilities.Abilities[abilityIndex];
                if (!ability.IsNormal)
                    continue;

                AbilityAttributes abilityAttrs = ability.Attributes;
                if ((abilityAttrs.Formula.Value != 2) && (abilityAttrs.InflictStatus == oldID))
                {
                    abilityAttrs.InflictStatus = newID;
                    abilityAttrs.OldInflictStatus = newID;
                    allInflictStatuses.InflictStatuses[newID].ReferencingAbilityIDs.Add(abilityIndex);
                }
            }

            allInflictStatuses.InflictStatuses[oldID].ReferencingItemIndexes.Clear();
            allInflictStatuses.InflictStatuses[oldID].ReferencingAbilityIDs.Clear();
        }

        public static void RepointItemAttributes(AllItems items, AllItemAttributes allItemAttributes)
        {
            Dictionary<int, int> repointMap = GetRepointMap<ItemAttributes>(allItemAttributes.ItemAttributes);

            for (int itemIndex = 0; itemIndex < items.Items.Count; itemIndex++)
            {
                Item item = items.Items[itemIndex];
                int newID = 0;
                if (repointMap.TryGetValue((int)item.SIA, out newID))
                {
                    item.SIA = (byte)newID;
                    item.OldSIA = (byte)newID;
                    allItemAttributes.ItemAttributes[newID].ReferencingItemIndexes.Add(itemIndex);
                }
            }

            foreach (int index in repointMap.Keys)
            {
                allItemAttributes.ItemAttributes[index].ReferencingItemIndexes.Clear();
            }
        }

        public static void RepointSpecificItemAttributes(AllItems items, AllItemAttributes allItemAttributes, byte oldID, byte newID)
        {
            for (int itemIndex = 0; itemIndex < items.Items.Count; itemIndex++)
            {
                Item item = items.Items[itemIndex];
                if (item.SIA == oldID)
                {
                    item.SIA = newID;
                    item.OldSIA = newID;
                    allItemAttributes.ItemAttributes[newID].ReferencingItemIndexes.Add(itemIndex);
                }
            }

            allItemAttributes.ItemAttributes[oldID].ReferencingItemIndexes.Clear();
        }

        public static void BuildReferenceList(AllItemAttributes itemAttributes, AllInflictStatuses inflictStatuses, AllAbilities abilities, AllItems items,
            AllSkillSets skillSets, AllMonsterSkills monsterSkills, AllJobs jobs)
        {
            foreach (ItemAttributes itemAttr in itemAttributes.ItemAttributes)
            {
                itemAttr.ReferencingItemIndexes.Clear();
            }

            foreach (InflictStatus inflictStatus in inflictStatuses.InflictStatuses)
            {
                inflictStatus.ReferencingAbilityIDs.Clear();
                inflictStatus.ReferencingItemIndexes.Clear();
            }

            foreach (Ability ability in abilities.Abilities)
            {
                ability.ReferencingSkillSetIDs.Clear();
                ability.ReferencingMonsterSkillIDs.Clear();
            }

            foreach (SkillSet skillSet in skillSets.SkillSets)
            {
                skillSet.ReferencingJobIDs.Clear();
            }

            for (int index = 0; index < items.Items.Count; index++)
            {
                Item item = items.Items[index];

                if (item.SIA < itemAttributes.ItemAttributes.Length)
                    itemAttributes.ItemAttributes[item.SIA].ReferencingItemIndexes.Add(index);

                if (item is Weapon)
                {
                    Weapon weapon = (Weapon)item;
                    if (weapon.Formula.Value != 2)
                    {
                        if (weapon.InflictStatus < inflictStatuses.InflictStatuses.Length)
                            inflictStatuses.InflictStatuses[weapon.InflictStatus].ReferencingItemIndexes.Add(index);
                    }
                }
                else if (item is ChemistItem)
                {
                    ChemistItem chemistItem = (ChemistItem)item;
                    if (chemistItem.Formula != 2)
                    {
                        if (chemistItem.InflictStatus < inflictStatuses.InflictStatuses.Length)
                            inflictStatuses.InflictStatuses[chemistItem.InflictStatus].ReferencingItemIndexes.Add(index);
                    }
                }
            }

            for (int index = 0; index < abilities.Abilities.Length; index++)
            {
                Ability ability = abilities.Abilities[index];
                if (ability.IsNormal)
                {
                    if ((ability.Attributes.Formula.Value != 2) && (ability.Attributes.InflictStatus < inflictStatuses.InflictStatuses.Length))
                        inflictStatuses.InflictStatuses[ability.Attributes.InflictStatus].ReferencingAbilityIDs.Add(index);
                }
            }

            for (int index = 0; index < skillSets.SkillSets.Length; index++)
            {
                SkillSet skillSet = skillSets.SkillSets[index];

                foreach (Ability ability in skillSet.Actions)
                {
                    abilities.Abilities[ability.Offset].ReferencingSkillSetIDs.Add(index);
                }

                foreach (Ability ability in skillSet.TheRest)
                {
                    abilities.Abilities[ability.Offset].ReferencingSkillSetIDs.Add(index);
                }
            }

            for (int index = 0; index < monsterSkills.MonsterSkills.Length; index++)
            {
                MonsterSkill monsterSkill = monsterSkills.MonsterSkills[index];
                abilities.Abilities[monsterSkill.Ability1.Offset].ReferencingMonsterSkillIDs.Add(index);
                abilities.Abilities[monsterSkill.Ability2.Offset].ReferencingMonsterSkillIDs.Add(index);
                abilities.Abilities[monsterSkill.Ability3.Offset].ReferencingMonsterSkillIDs.Add(index);
                abilities.Abilities[monsterSkill.Beastmaster.Offset].ReferencingMonsterSkillIDs.Add(index);
            }

            for (int index = 0; index < jobs.Jobs.Length; index++)
            {
                Job job = jobs.Jobs[index];

                if (job.SkillSet.Value < 0xB0)
                    skillSets.SkillSets[job.SkillSet.Value].ReferencingJobIDs.Add(index);
            }
        }

        public static void CheckDuplicates<T>(IList<T> objects) where T : ICheckDuplicate<T>
        {
            for (int index = 0; index < objects.Count; index++)
            {
                T obj = objects[index];
                obj.IsDuplicate = false;
                obj.Index = index;

                for (int innerIndex = 0; innerIndex < index; innerIndex++)
                {
                    if (obj.CheckDuplicate(objects[innerIndex]))
                    {
                        obj.IsDuplicate = true;
                        obj.DuplicateIndex = innerIndex;
                        break;
                    }
                }
            }
        }

        private static Dictionary<int, int> GetRepointMap<T>(IList<T> list) where T : ICheckDuplicate<T>
        {
            Dictionary<int, int> repointMap = new Dictionary<int, int>();
            for (int index = 0; index < list.Count; index++)
            {
                T entry = list[index];
                if (entry.IsDuplicate)
                {
                    repointMap.Add((int)index, (int)entry.DuplicateIndex);
                }
            }

            return repointMap;
        }
    }
}
