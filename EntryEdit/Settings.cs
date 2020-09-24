using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PatcherLib.Utilities;
using PatcherLib.Iso;

namespace EntryEdit
{
    internal class Settings
    {
        private static Dictionary<string, object> _valueMap = new Dictionary<string, object>();

        private static int GetIntValue(string name)
        {
            object lookupResult = default(int);
            if (_valueMap.TryGetValue(name, out lookupResult))
                return (int)lookupResult;
            else
            {
                int parseResult = default(int);
                Utilities.TryParseInt(ConfigurationManager.AppSettings[name], out parseResult);
                _valueMap.Add(name, parseResult);
                return parseResult;
            }
        }

        private static PsxIso.Sectors GetSectorValue(string name)
        {
            object lookupResult = (PsxIso.Sectors)(default(int));
            if (_valueMap.TryGetValue(name, out lookupResult))
                return (PsxIso.Sectors)lookupResult;
            else
            {
                PsxIso.Sectors sector = PsxIso.GetSector(ConfigurationManager.AppSettings[name]);
                _valueMap.Add(name, sector);
                return sector;
            }
        }

        public static int EventSize { get { return GetIntValue("EventSize"); } }
        public static int BattleConditionalsSize { get { return GetIntValue("BattleConditionalsSize"); } }
        public static int WorldConditionalsSize { get { return GetIntValue("WorldConditionalsSize"); } }
        public static PsxIso.Sectors BattleConditionalsSector { get { return GetSectorValue("BattleConditionalsSector"); } }
        public static int BattleConditionalsOffset { get { return GetIntValue("BattleConditionalsOffset"); } }
        public static PsxIso.Sectors WorldConditionalsSector { get { return GetSectorValue("WorldConditionalsSector"); } }
        public static int WorldConditionalsOffset { get { return GetIntValue("WorldConditionalsOffset"); } }
        public static PsxIso.Sectors EventsSector { get { return GetSectorValue("EventsSector"); } }
        public static int EventsOffset { get { return GetIntValue("EventsOffset"); } }
        public static int WorldConditionalsRepoint { get { return GetIntValue("WorldConditionalsRepoint"); } }
        public static PsxIso.Sectors WorldConditionalsPointerSector { get { return GetSectorValue("WorldConditionalsPointerSector"); } }
        public static int WorldConditionalsPointerOffset { get { return GetIntValue("WorldConditionalsPointerOffset"); } }
    }
}
