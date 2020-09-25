using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PatcherLib.Utilities;
using PatcherLib.Iso;

namespace EntryEdit
{
    internal static class Settings
    {
        private static readonly int _eventSize = Utilities.ParseInt(ConfigurationManager.AppSettings["EventSize"]); 
        public static int EventSize { get { return _eventSize; } }

        private static readonly int _battleConditionalsSize = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalsSize"]);
        public static int BattleConditionalsSize { get { return _battleConditionalsSize; } }

        private static readonly int _worldConditionalsSize = Utilities.ParseInt(ConfigurationManager.AppSettings["WorldConditionalsSize"]);
        public static int WorldConditionalsSize { get { return _worldConditionalsSize; } }

        private static readonly PsxIso.Sectors _battleConditionalsSector = PsxIso.GetSector(ConfigurationManager.AppSettings["BattleConditionalsSector"]);
        public static PsxIso.Sectors BattleConditionalsSector { get { return _battleConditionalsSector; } }

        private static readonly int _battleConditionalsOffset = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalsOffset"]);
        public static int BattleConditionalsOffset { get { return _battleConditionalsOffset; } }

        private static readonly PsxIso.Sectors _worldConditionalsSector = PsxIso.GetSector(ConfigurationManager.AppSettings["WorldConditionalsSector"]);
        public static PsxIso.Sectors WorldConditionalsSector { get { return _worldConditionalsSector; } }

        private static readonly int _worldConditionalsOffset = Utilities.ParseInt(ConfigurationManager.AppSettings["WorldConditionalsOffset"]);
        public static int WorldConditionalsOffset { get { return _worldConditionalsOffset; } }

        private static readonly PsxIso.Sectors _eventsSector = PsxIso.GetSector(ConfigurationManager.AppSettings["EventsSector"]);
        public static PsxIso.Sectors EventsSector { get { return _eventsSector; } }

        private static readonly int _eventsOffset = Utilities.ParseInt(ConfigurationManager.AppSettings["EventsOffset"]);
        public static int EventsOffset { get { return _eventsOffset; } }

        private static readonly int _worldConditionalsRepoint = Utilities.ParseInt(ConfigurationManager.AppSettings["WorldConditionalsRepoint"]);
        public static int WorldConditionalsRepoint { get { return _worldConditionalsRepoint; } }

        private static readonly PsxIso.Sectors _worldConditionalsPointerSector = PsxIso.GetSector(ConfigurationManager.AppSettings["WorldConditionalsPointerSector"]);
        public static PsxIso.Sectors WorldConditionalsPointerSector { get { return _worldConditionalsPointerSector; } }

        private static readonly int _worldConditionalsPointerOffset = Utilities.ParseInt(ConfigurationManager.AppSettings["WorldConditionalsPointerOffset"]);
        public static int WorldConditionalsPointerOffset { get { return _worldConditionalsPointerOffset; } }

        private static readonly int _battleConditionalBlockOffsetsRAMLocation = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalBlockOffsetsRAMLocation"]);
        public static int BattleConditionalBlockOffsetsRAMLocation { get { return _battleConditionalBlockOffsetsRAMLocation; } }

        private static readonly int _battleConditionalsRAMLocation = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalsRAMLocation"]);
        public static int BattleConditionalsRAMLocation { get { return _battleConditionalsRAMLocation; } }

        private static readonly int _eventRAMLocation = Utilities.ParseInt(ConfigurationManager.AppSettings["EventRAMLocation"]);
        public static int EventRAMLocation { get { return _eventRAMLocation; } }
    }
}
