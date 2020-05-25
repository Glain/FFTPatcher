using FFTPatcher.Datatypes;
using PatcherLib.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFTPatcher
{
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
                            allInflictStatuses.InflictStatuses[newID].ReferencingItemIDs.Add(itemIndex);
                        }
                    }
                }
            }

            for (int abilityIndex = 0; abilityIndex < abilities.Abilities.Length; abilityIndex++)
            {
                Ability ability = abilities.Abilities[abilityIndex];
                AbilityAttributes abilityAttrs = ability.Attributes;

                int newID = 0;
                if (repointMap.TryGetValue(abilityAttrs.InflictStatus, out newID))
                {
                    abilityAttrs.InflictStatus = (byte)newID;
                    abilityAttrs.OldInflictStatus = (byte)newID;
                    allInflictStatuses.InflictStatuses[newID].ReferencingAbilityIDs.Add(abilityIndex);
                }
            }

            foreach (int index in repointMap.Keys)
            {
                InflictStatus inflictStatus = allInflictStatuses.InflictStatuses[index];
                inflictStatus.ReferencingItemIDs.Clear();
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
                        allInflictStatuses.InflictStatuses[newID].ReferencingItemIDs.Add(itemIndex);
                    }
                }
            }

            for (int abilityIndex = 0; abilityIndex < abilities.Abilities.Length; abilityIndex++)
            {
                Ability ability = abilities.Abilities[abilityIndex];
                AbilityAttributes abilityAttrs = ability.Attributes;

                if (abilityAttrs.InflictStatus == oldID)
                {
                    abilityAttrs.InflictStatus = newID;
                    abilityAttrs.OldInflictStatus = newID;
                    allInflictStatuses.InflictStatuses[newID].ReferencingAbilityIDs.Add(abilityIndex);
                }
            }

            allInflictStatuses.InflictStatuses[oldID].ReferencingItemIDs.Clear();
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
                    allItemAttributes.ItemAttributes[newID].ReferencingItemIDs.Add(itemIndex);
                }
            }

            foreach (int index in repointMap.Keys)
            {
                allItemAttributes.ItemAttributes[index].ReferencingItemIDs.Clear();
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
                    allItemAttributes.ItemAttributes[newID].ReferencingItemIDs.Add(itemIndex);
                }
            }

            allItemAttributes.ItemAttributes[oldID].ReferencingItemIDs.Clear();
        }

        public static void BuildReferenceList(AllItemAttributes itemAttributes, AllInflictStatuses inflictStatuses, AllAbilities abilities, AllItems items)
        {
            foreach (ItemAttributes itemAttr in itemAttributes.ItemAttributes)
            {
                itemAttr.ReferencingItemIDs.Clear();
            }

            foreach (InflictStatus inflictStatus in inflictStatuses.InflictStatuses)
            {
                inflictStatus.ReferencingAbilityIDs.Clear();
                inflictStatus.ReferencingItemIDs.Clear();
            }

            for (int index = 0; index < items.Items.Count; index++)
            {
                Item item = items.Items[index];

                itemAttributes.ItemAttributes[item.SIA].ReferencingItemIDs.Add(index);

                if (item is Weapon)
                {
                    Weapon weapon = (Weapon)item;
                    if (weapon.Formula.Value != 2)
                    {
                        inflictStatuses.InflictStatuses[weapon.InflictStatus].ReferencingItemIDs.Add(index);
                    }
                }
            }

            for (int index = 0; index < abilities.Abilities.Length; index++)
            {
                Ability ability = abilities.Abilities[index];
                if (ability.IsNormal)
                {
                    inflictStatuses.InflictStatuses[ability.Attributes.InflictStatus].ReferencingAbilityIDs.Add(index);
                }
            }
        }

        public static void CheckDuplicates<T>(IList<T> objects) where T : ICheckDuplicate<T>
        {
            for (int index = 0; index < objects.Count; index++)
            {
                T obj = objects[index];
                obj.IsDuplicate = false;

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
