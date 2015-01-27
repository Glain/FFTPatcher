using System;
using System.Collections.Generic;
using System.Text;

namespace LEDecoder
{
    public class Value
    {
        public int size;
        public int startnumeric;
        public int endnumeric;
        public int startID;
        public int EndID;
        public int StartFlags;
        public int EndFlags;
        public string type;
        public string listtype;
        public IList<string> ValueDescriptions;

        public string[] ValueLines;

        public Value(string TypeInput, string SizeInput)
        {
            type = TypeInput;
            switch (SizeInput)
            {
                case "b":
                    size = 1;
                    break;
                case "h":
                    size = 2;
                    break;
                case "w":
                    size = 4;
                    break;
            }
            ValueDescriptions = new string[0x100*size];

        }
        public Value()
        {
            ValueDescriptions = new string[0x10000];
        }

        public void SetValueDescriptions(string listtype,long startindex,long endindex)
        {
            IList<string> list = new string[1];

            switch (type)
            {
                case "numeric":
                    if(endindex == 0)
                    {
                        for (long i = 0; i < size*256; i++)
                        {
                            ValueDescriptions[(int)i] = i.ToString();
                        }
                    }
                    else
                    {
                        for (long i = startindex; i < endindex; i++)
                        {
                            ValueDescriptions[(int)i] = i.ToString();
                        }
                    }
                   
                    break;
                default:
                    list = GetList(listtype);
                    if(endindex == 0)
                    {
                        ValueDescriptions = list;
                    }
                    else
                    {
                        for (long index = startindex; index < endindex; index++)
                        {
                            ValueDescriptions[(int)index] = list[(int)index];
                        }
                    }
                
                        break;
                case "birthdays":
                        #region Months
                        for (int i = 0; i < 31;i++ )
                        {
                            ValueDescriptions[i] = "January " + i.ToString();
                        }
                        for (int i = 0; i < 29; i++)
                        {
                            ValueDescriptions[i] = "February " + i.ToString();
                        }
                        for (int i = 0; i < 31; i++)
                        {
                            ValueDescriptions[i] = "March " + i.ToString();
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            ValueDescriptions[i] = "April " + i.ToString();
                        }
                        for (int i = 0; i < 31; i++)
                        {
                            ValueDescriptions[i] = "May " + i.ToString();
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            ValueDescriptions[i] = "June " + i.ToString();
                        }
                        for (int i = 0; i < 31; i++)
                        {
                            ValueDescriptions[i] = "July " + i.ToString();
                        }
                        for (int i = 0; i < 31; i++)
                        {
                            ValueDescriptions[i] = "August " + i.ToString();
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            ValueDescriptions[i] = "September " + i.ToString();
                        }
                        for (int i = 0; i < 31; i++)
                        {
                            ValueDescriptions[i] = "October " + i.ToString();
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            ValueDescriptions[i] = "November " + i.ToString();
                        }
                        for (int i = 0; i < 31; i++)
                        {
                            ValueDescriptions[i] = "December " + i.ToString();
                        }
                        #endregion
                            break;
            }


        }
        public IList<string> GetList(string listname)
        {
            IList<string> list = new string[0x100];

            for (int i = 0; i < 0x100;i++ )
            {
                list[i] = "";
            }

                if (listname == "SpriteSets")
                {
                    list = PatcherLib.PSXResources.Lists.SpriteSets;
                    size = 1;
                }
                else if (listname == "AbilityTypes ")
                {
                    list = PatcherLib.PSXResources.Lists.AbilityTypes;
                    size = 1;
                }
                else if (listname == "AbilityAttributes")
                {
                    list = PatcherLib.PSXResources.Lists.AbilityAttributes;
                    size = 1;
                }
                else if (listname == "AbilityAI")
                {
                    list = PatcherLib.PSXResources.Lists.AbilityAI;
                    size = 1;
                }
                else if (listname == "AbilityEffects")
                {
                    list = PatcherLib.PSXResources.Lists.AbilityEffects;
                    size = 2;
                }
                else if (listname == "Abilities")
                {
                    list = PatcherLib.PSXResources.Lists.AbilityNames;
                    size = 2;
                }
                else if (listname == "ShopAvailabilities")
                {
                    list = PatcherLib.PSXResources.Lists.ShopAvailabilities;
                    size = 1;
                }
                else if (listname == "Propositions")
                {
                    list = PatcherLib.PSXResources.Lists.Propositions;
                    size = 1;
                }
                else if (listname == "UnexploredLands")
                {
                    list = PatcherLib.PSXResources.Lists.UnexploredLands;
                    size = 1;
                }
                else if (listname == "Treasures")
                {
                    list = PatcherLib.PSXResources.Lists.Treasures;
                    size = 1;
                }
                else if (listname == "SpecialNames")
                {
                    list = PatcherLib.PSXResources.Lists.SpecialNames;
                    size = 2;
                }
                else if (listname == "MapNames")
                {
                    list = PatcherLib.PSXResources.Lists.MapNames;
                    size = 1;
                }
                else if (listname == "Items")
                {
                    list = PatcherLib.PSXResources.Lists.Items;
                    size = 1;
                }
                else if (listname == "EventNames")
                {
                    list = PatcherLib.PSXResources.Lists.EventNames;
                    size = 2;
                }
                else if (listname == "MonsterNames")
                {
                    list = PatcherLib.PSXResources.Lists.MonsterNames;
                    size = 2;
                }
                else if (listname == "UnitNames")
                {
                    list = PatcherLib.PSXResources.Lists.UnitNames;
                    size = 2;
                }
                else if (listname == "Jobs")
                {
                    list = PatcherLib.PSXResources.Lists.JobNames;
                    size = 1;
                }
                else if (listname == "StatusNames")
                {
                    list = PatcherLib.PSXResources.Lists.StatusNames;
                    size = 1;
                }
                else if (listname == "SkillSets")
                {
                    list = PatcherLib.PSXResources.Lists.SkillSets;
                    size = 1;
                }

            return list;
        }

    }
}
