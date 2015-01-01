using System;
using System.Collections.Generic;
using System.Text;

namespace LEDecoder
{
    public class SubData
    {
        public int size = 1;
        public int offsetaddress;
        public string description;
        public string[] flags;
        public string type;
        public bool IsSubset = false;
        public string SubsetDescription = "";
        public IList<string> list;
        public Value[] Values;
      
        public SubData()
        {

        }
        public SubData(string line)
        {
         
        }
        public void GetList(string listname)
        {
            if(listname == "SpriteSets")
            {
                list = PatcherLib.PSXResources.Lists.SpriteSets;
            }
            if (listname == "AbilityTypes ")
            {
                list = PatcherLib.PSXResources.Lists.AbilityTypes;
            }
            if (listname == "AbilityAttributes")
            {
                list = PatcherLib.PSXResources.Lists.AbilityAttributes;
            }
            if (listname == "AbilityAI")
            {
                list = PatcherLib.PSXResources.Lists.AbilityAI;
            }
            if (listname == "AbilityEffects")
            {
                list = PatcherLib.PSXResources.Lists.AbilityEffects;
            }
            if (listname == "Abilities")
            {
                list = PatcherLib.PSXResources.Lists.AbilityNames;
            }
            if (listname == "ShopAvailabilities")
            {
                list = PatcherLib.PSXResources.Lists.ShopAvailabilities;
            }
            if (listname == "Propositions")
            {
                list = PatcherLib.PSXResources.Lists.Propositions;
            }
            if (listname == "UnexploredLands")
            {
                list = PatcherLib.PSXResources.Lists.UnexploredLands;
            }
            if (listname == "Treasures")
            {
                list = PatcherLib.PSXResources.Lists.Treasures;
            }
            if (listname == "SpecialNames")
            {
                list = PatcherLib.PSXResources.Lists.SpecialNames;
            }
            if (listname == "MapNames")
            {
                list = PatcherLib.PSXResources.Lists.MapNames;
            }
            if (listname == "Items")
            {
                list = PatcherLib.PSXResources.Lists.Items;
            }
            if (listname == "EventNames")
            {
                list = PatcherLib.PSXResources.Lists.EventNames;
            }
            if (listname == "MonsterNames")
            {
                list = PatcherLib.PSXResources.Lists.MonsterNames;
            }
            if (listname == "UnitNames")
            {
                list = PatcherLib.PSXResources.Lists.UnitNames;
            }
            if (listname == "Jobs")
            {
                list = PatcherLib.PSXResources.Lists.JobNames;
            }
            if (listname == "StatusNames")
            {
                list = PatcherLib.PSXResources.Lists.StatusNames;
            }
            if (listname == "SkillSets")
            {
                list = PatcherLib.PSXResources.Lists.SkillSets;
            }

        }
      
        public class Value
        {
            public int value;
            public string description;

            public Value()
            {

            }
        }

        public void Initialize()
        {
            if(Values == null)
            {
                Values = new Value[1];
            }
        }
        public void AddValue()
        {
            Array.Resize(ref Values, Values.Length);
            Values[Values.Length - 1] = new Value();

        }
    }
}
