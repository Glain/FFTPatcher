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

        private static readonly int _numEvents = Utilities.ParseInt(ConfigurationManager.AppSettings["NumEvents"]);
        public static int NumEvents { get { return _numEvents; } }

        private static readonly int _battleConditionalSetMaxBlocks = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalSetMaxBlocks"]);
        public static int BattleConditionalSetMaxBlocks { get { return _battleConditionalSetMaxBlocks; } }

        private static readonly int _battleConditionalSetMaxCommands = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalSetMaxCommands"]);
        public static int BattleConditionalSetMaxCommands { get { return _battleConditionalSetMaxCommands; } }

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

        private static readonly bool _worldConditionalsRepoint = Utilities.ParseBool(ConfigurationManager.AppSettings["WorldConditionalsRepoint"]);
        public static bool WorldConditionalsRepoint { get { return _worldConditionalsRepoint; } }

        private static readonly PsxIso.Sectors _worldConditionalsPointerSector = PsxIso.GetSector(ConfigurationManager.AppSettings["WorldConditionalsPointerSector"]);
        public static PsxIso.Sectors WorldConditionalsPointerSector { get { return _worldConditionalsPointerSector; } }

        private static readonly int _worldConditionalsPointerOffset = Utilities.ParseInt(ConfigurationManager.AppSettings["WorldConditionalsPointerOffset"]);
        public static int WorldConditionalsPointerOffset { get { return _worldConditionalsPointerOffset; } }

        private static readonly int _battleConditionalBlockOffsetsRAMLocation = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalBlockOffsetsRAMLocation"]);
        public static int BattleConditionalBlockOffsetsRAMLocation { get { return _battleConditionalBlockOffsetsRAMLocation; } }

        private static readonly int _battleConditionalsRAMLocation = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalsRAMLocation"]);
        public static int BattleConditionalsRAMLocation { get { return _battleConditionalsRAMLocation; } }

        private static readonly int _battleConditionalBlockOffsetsRAMLength = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalBlockOffsetsRAMLength"]);
        public static int BattleConditionalBlockOffsetsRAMLength { get { return _battleConditionalBlockOffsetsRAMLength; } }

        private static readonly int _battleConditionalsRAMLength = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalsRAMLength"]);
        public static int BattleConditionalsRAMLength { get { return _battleConditionalsRAMLength; } }

        private static readonly int _eventRAMLocation = Utilities.ParseInt(ConfigurationManager.AppSettings["EventRAMLocation"]);
        public static int EventRAMLocation { get { return _eventRAMLocation; } }

        private static readonly int _eventIDRAMLocation = Utilities.ParseInt(ConfigurationManager.AppSettings["EventIDRAMLocation"]);
        public static int EventIDRAMLocation { get { return _eventIDRAMLocation; } }

        private static readonly bool _battleConditionalsApplyLimitPatch = Utilities.ParseBool(ConfigurationManager.AppSettings["BattleConditionalsApplyLimitPatch"]);
        public static bool BattleConditionalsApplyLimitPatch { get { return _battleConditionalsApplyLimitPatch; } }

        private static readonly PsxIso.Sectors _battleConditionalsLimitPatchSector = PsxIso.GetSector(ConfigurationManager.AppSettings["BattleConditionalsLimitPatchSector"]);
        public static PsxIso.Sectors BattleConditionalsLimitPatchSector { get { return _battleConditionalsLimitPatchSector; } }

        private static readonly int _battleConditionalsLimitPatchOffset = Utilities.ParseInt(ConfigurationManager.AppSettings["BattleConditionalsLimitPatchOffset"]);
        public static int BattleConditionalsLimitPatchOffset { get { return _battleConditionalsLimitPatchOffset; } }

        private static readonly byte[] _battleConditionalsLimitPatchBytes = Utilities.GetBytesFromHexString(ConfigurationManager.AppSettings["BattleConditionalsLimitPatchBytes"].Replace("0x", ""));
        public static byte[] BattleConditionalsLimitPatchBytes { get { return _battleConditionalsLimitPatchBytes; } }

        private static readonly PsxIso.Sectors _scenariosSector = PsxIso.GetSector(ConfigurationManager.AppSettings["ScenariosSector"]);
        public static PsxIso.Sectors ScenariosSector { get { return _scenariosSector; } }

        private static readonly int _scenariosOffset = Utilities.ParseInt(ConfigurationManager.AppSettings["ScenariosOffset"]);
        public static int ScenariosOffset { get { return _scenariosOffset; } }

        private static readonly int _textOffsetRAMLocation = Utilities.ParseInt(ConfigurationManager.AppSettings["TextOffsetRAMLocation"]);
        public static int TextOffsetRAMLocation { get { return _textOffsetRAMLocation; } }

        private static readonly int _maxSectors = Utilities.ParseInt(ConfigurationManager.AppSettings["MaxSectors"]);
        public static int MaxSectors { get { return _maxSectors; } }

        public static readonly int _totalEventSize = Settings.EventSize * Settings.NumEvents;
        public static int TotalEventSize { get { return _totalEventSize; } }

        private static readonly int _worldConditionalsPointerRAMLocation = PsxIso.GetRamOffset(Settings.WorldConditionalsPointerSector) + Settings.WorldConditionalsPointerOffset;
        public static int WorldConditionalsPointerRAMLocation { get { return _worldConditionalsPointerRAMLocation; } }

        private static readonly int _battleConditionalsLimitPatchRAMLocation = PsxIso.GetRamOffset(Settings.BattleConditionalsLimitPatchSector) + Settings.BattleConditionalsLimitPatchOffset;
        public static int BattleConditionalsLimitPatchRAMLocation { get { return _battleConditionalsLimitPatchRAMLocation; } }

        private static readonly int _scenariosRAMLocation = PsxIso.GetRamOffset(Settings.ScenariosSector) + Settings.ScenariosOffset;
        public static int ScenariosRAMLocation { get { return _scenariosRAMLocation; } }

        private static readonly int _worldConditionalsCalcRAMLocation = PsxIso.GetRamOffset(Settings.WorldConditionalsSector) + Settings.WorldConditionalsOffset;
        public static int WorldConditionalsCalcRAMLocation { get { return _worldConditionalsCalcRAMLocation; } }
    }
}
