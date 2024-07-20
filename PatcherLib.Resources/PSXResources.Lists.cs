using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PatcherLib
{
    using PatcherLib.Datatypes;
    using PatcherLib.Utilities;
    using Paths = ResourcesClass.Paths.PSX;
    using ResourceListInfo = ResourcesClass.ResourceListInfo;
    using ListType = ResourcesClass.ListType;
    using System.Xml;

    public static partial class PSXResources
    {
        public static class Lists
        {


            private static IDictionary<ListType, IList<string>> typeToStringDict = new Dictionary<ListType, IList<string>>();

            private static IList<string> GetListForType( ListType type )
            {
                if ( !typeToStringDict.ContainsKey( type ) )
                {
                    typeToStringDict[type] = 
                        ResourcesClass.GetStringsFromNumberedXmlNodes( typeInfo[type] ).AsReadOnly();
                }
                return typeToStringDict[type];
            }

            public static IList<string> StatusNames
            {
                get
                {
                    return GetListForType( ListType.StatusNames );
                }
            }

            public static IList<string> SkillSets
            {
                get { return GetListForType( ListType.SkillSetNames ); }
            }

            public static IList<string> JobNames
            {
                get { return GetListForType( ListType.JobNames ); }
            }

            public static IList<string> UnitNames
            {
                get { return GetListForType( ListType.UnitNames ); }
            }

            public static IList<string> MonsterNames
            {
                get { return GetListForType( ListType.MonsterNames ); }
            }

            public static IList<string> EventNames
            {
                get { return GetListForType( ListType.EventNames ); }
            }

            public static IList<string> Items
            {
                get { return GetListForType( ListType.ItemNames ); }
            }

            public static IList<string> AbilityNames
            {
                get { return GetListForType( ListType.AbilityNames ); }
            }

            public static IList<string> AbilityAI
            {
                get
                {
                    return GetListForType( ListType.AbilityAI );
                }
            }

            public static IList<string> AbilityAttributes
            {
                get
                {
                    return GetListForType( ListType.AbilityAttributes );
                }
            }


            public static IList<string> AbilityTypes
            {
                get
                {
                    return GetListForType( ListType.AbilityTypes );
                }
            }

            public static IList<string> AbilityProperties
            {
                get
                {
                    return GetListForType(ListType.AbilityProperties);
                }
            }

            public static IList<string> MapNames
            {
                get
                {
                    return GetListForType(ListType.MapNames);
                }
            }
            public static IList<string> ShopAvailabilities
            {
                get
                {
                    return GetListForType(ListType.ShopAvailabilities);
                }
            }

            public static IList<string> AbilityEffects
            {
                get
                {
                    return GetListForType( ListType.AbilityEffects );
                }
            }
            public static IList<string> SpriteFiles { get { return GetListForType( ListType.SpriteFiles ); } }


            public static IList<string> SpecialNames
            {
                get
                {
                    return GetListForType( ListType.SpecialNames );
                }
            }

            public static IList<string> SpriteSets
            {
                get
                {
                    return GetListForType( ListType.SpriteSets );
                }
            }

            public static IList<string> Treasures
            {
                get
                {
                    return GetListForType( ListType.Treasures );
                }
            }

            public static IList<string> UnexploredLands
            {
                get
                {
                    return GetListForType( ListType.UnexploredLands );
                }
            }
            public static IList<string> Propositions
            {
                get { return GetListForType( ListType.Propositions ); }
            }


            private static IDictionary<ListType, ResourceListInfo> typeInfo = new Dictionary<ListType, ResourceListInfo> {
                { 
                    ListType.Propositions,
                    new ResourceListInfo {
                        Doc = PSXResources.propositionsDoc,
                        XPath = "/Propositions/entry[@value='{0}']/@name",
                        Length = 96,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.Treasures,
                    new ResourceListInfo {
                        Doc = braveStoryDoc,
                        XPath = "/BS/Treasures/entry[@value='{0}']/@name",
                        Length = 45,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.UnexploredLands,
                    new ResourceListInfo {
                        Doc = braveStoryDoc,
                        XPath = "/BS/UnexploredLands/entry[@value='{0}']/@name",
                        Length = 16,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.AbilityAI, 
                    new ResourceListInfo { 
                        Doc = abilitiesStringsDoc, 
                        XPath = "/AbilityStrings/AI/string[@value='{0}']/@name",
                        Length = 24, 
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.AbilityAttributes, 
                    new ResourceListInfo { 
                        Doc = abilitiesStringsDoc, 
                        XPath = "/AbilityStrings/Attributes/string[@value='{0}']/@name",
                        Length = 32,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.AbilityEffects, 
                    new ResourceListInfo {
                        Doc = abilityEffectsDoc,
                        XPath = "/Effects/Effect[@value='{0:X3}']/@name",
                        Length = 512,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.UnitNames, 
                    new ResourceListInfo {
                        Doc = PSXResources.unitNamesDoc,
                        XPath = "/Names/n[@id='{0}']",
                        Length = 768,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.MonsterNames,
                    new ResourceListInfo {
                        Doc = PSXResources.jobsDoc,
                        XPath = "/Jobs/Job[@offset='{0:X2}']/@name",
                        Length = 48,
                        StartIndex = 0x5E 
                    } 
                },
                { 
                    ListType.SkillSetNames,
                    new ResourceListInfo {
                        Doc = PSXResources.skillSetsDoc,
                        XPath = "/SkillSets/SkillSet[@byte='{0:X2}']/@name",
                        Length = 0xE0,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.JobNames,
                    new ResourceListInfo {
                        Doc = PSXResources.jobsDoc,
                        XPath = "/Jobs/Job[@offset='{0:X2}']/@name",
                        Length = 0xA0,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.EventNames, 
                    new ResourceListInfo {
                        Doc = PSXResources.eventNamesDoc,
                        XPath = "/Events/Event[@value='{0:X3}']/@name",
                        Length = 0x200,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.ItemNames,
                    new ResourceListInfo {
                        Doc = PSXResources.itemsDoc,
                        XPath = "/Items/Item[@offset='{0}']/@name",
                        Length = 256,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.AbilityNames,
                    new ResourceListInfo {
                        Doc = abilitiesDoc,
                        XPath = "/Abilities/Ability[@value='{0}']/@name",
                        Length = 512,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.AbilityTypes,
                    new ResourceListInfo {
                        Doc = abilitiesStringsDoc,
                        XPath = "/AbilityStrings/Types/string[@value='{0}']/@name",
                        Length = 16,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.MapNames,
                    new ResourceListInfo {
                        Doc = PSXResources.mapNamesDoc,
                        XPath = "/MapNames/Map[@value='{0}']",
                        Length = 128,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.ShopAvailabilities,
                    new ResourceListInfo {
                        Doc = PSXResources.itemsStringsDoc,
                        XPath = "/ItemStrings/ShopAvailabilities/string[@value='{0}']/@name",
                        Length = 21,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.StatusNames,
                    new ResourceListInfo {
                        Doc = PSXResources.statusNamesDoc,
                        XPath = "/Statuses/Status[@offset='{0}']/@name",
                        Length = 40,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.SpecialNames,
                    new ResourceListInfo {
                        Doc = PSXResources.specialNamesDoc,
                        XPath = "/SpecialNames/SpecialName[@byte='{0:X2}']/@name",
                        Length = 256,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.SpriteSets,
                    new ResourceListInfo {
                        Doc = PSXResources.spriteSetsDoc,
                        XPath = "/Sprites/Sprite[@byte='{0:X2}']/@name",
                        Length = 256,
                        StartIndex = 0 
                    } 
                },
                { 
                    ListType.SpriteFiles,
                    new ResourceListInfo {
                        Doc = PSXResources.spriteFilesDoc,
                        XPath = "/sprites/s[@n='{0:X2}']",
                        Length = 0x9F,
                        StartIndex = 0 
                    } 
                },
                {
                    ListType.AbilityProperties,
                    new ResourceListInfo {
                        Doc = PSXResources.abilitiesStringsDoc,
                        XPath = "/AbilityStrings/Properties/string[@value='{0}']/@name",
                        Length = 12,
                        StartIndex = 0
                    }
                }
            };


            private static IDictionary<Town, string> readOnlyTownNames;

            public static IDictionary<Town, string> TownNames
            {
                get
                {
                    if (readOnlyTownNames == null)
                    {
                        Dictionary<Town, string> storeNames = new Dictionary<Town, string>();
                        System.Xml.XmlDocument doc = shopNamesDoc;

                        foreach (System.Xml.XmlNode node in doc.SelectNodes( "/ShopNames/Shop" ))
                        {
                            string val = node.Attributes["value"].Value;
                            if (System.Enum.IsDefined( typeof( Town ), val ))
                            {
                                storeNames[(Town)System.Enum.Parse( typeof( Town ), node.Attributes["value"].Value )] =
                                    node.Attributes["name"].Value;
                            }
                        }

                        readOnlyTownNames = new ReadOnlyDictionary<Town, string>( storeNames );
                    }

                    return readOnlyTownNames;
                }
            }


            public static IDictionary<ShopsFlags, string> ShopNames
            {
                get
                {
                    if ( readOnlyStoreNames == null )
                    {
                        Dictionary<ShopsFlags, string> storeNames = new Dictionary<ShopsFlags, string>();
                        System.Xml.XmlDocument doc = PSXResources.shopNamesDoc;

                        foreach ( System.Xml.XmlNode node in doc.SelectNodes( "/ShopNames/Shop" ) )
                        {
                            storeNames[(ShopsFlags)System.Enum.Parse( typeof( ShopsFlags ), node.Attributes["value"].Value )] =
                                node.Attributes["name"].Value;
                        }
                        readOnlyStoreNames = new ReadOnlyDictionary<ShopsFlags, string>( storeNames );
                    }

                    return readOnlyStoreNames;
                }
            }



        }

    }
}
