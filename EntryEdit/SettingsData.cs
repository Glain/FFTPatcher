using PatcherLib.Datatypes;
using PatcherLib.Helpers;
using PatcherLib.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace EntryEdit
{
    internal class SettingsData
    {
        public string Mod { get; private set; }
        public bool IgnoreParameterTypes { get; private set; }
        public int EventSize { get; private set; }
        public int BattleConditionalsSize { get; private set; }
        public int WorldConditionalsSize { get; private set; }
        public int NumEvents { get; private set; }
        public int BattleConditionalSetMaxBlocks { get; private set; }
        public int BattleConditionalSetMaxCommands { get; private set; }
        public Enum BattleConditionalsSector { get; private set; }
        public int BattleConditionalsOffset { get; private set; }
        public Enum WorldConditionalsSector { get; private set; }
        public int WorldConditionalsOffset { get; private set; }
        public Enum EventsSector { get; private set; }
        public int EventsOffset { get; private set; }

        public bool WorldConditionalsRepoint { get; private set; }
        public Enum WorldConditionalsPointerSector { get; private set; }
        public int WorldConditionalsPointerOffset { get; private set; }
        public int WorldConditionalsWorkingPointerRAMLocation { get; private set; }
        public int BattleConditionalBlockOffsetsRAMLocation { get; private set; }
        public int BattleConditionalsRAMLocation { get; private set; }
        public int BattleConditionalBlockOffsetsRAMLength { get; private set; }
        public int BattleConditionalsRAMLength { get; private set; }
        public int EventRAMLocation { get; private set; }
        public int EventIDRAMLocation { get; private set; }
        public bool BattleConditionalsApplyLimitPatch { get; private set; }
        public Enum BattleConditionalsLimitPatchSector { get; private set; }
        public int BattleConditionalsLimitPatchOffset { get; private set; }
        public byte[] BattleConditionalsLimitPatchBytes { get; private set; }
        public Enum ScenariosSector { get; private set; }
        public int ScenariosOffset { get; private set; }
        public int TextOffsetRAMLocation { get; private set; }
        public int MaxSectors { get; private set; }
        public string FilepathDefaultBattleConditionals { get; private set; }
        public string FilepathTrimmedBattleConditionals { get; private set; }
        public string FilepathDefaultWorldConditionals { get; private set; }
        public string FilepathDefaultEvents { get; private set; }
        public string FilepathCommandBattleConditionals { get; private set; }
        public string FilepathCommandWorldConditionals { get; private set; }
        public string FilepathCommandEvents { get; private set; }
        public string FilepathNamesBattleConditionals { get; private set; }
        public string FilepathNamesWorldConditionals { get; private set; }
        public string FilepathNamesEvents { get; private set; }


        public Context Context { get; private set; }
        public int TotalEventSize { get; private set; }
        public int WorldConditionalsPointerRAMLocation { get; private set; }
        public int BattleConditionalsLimitPatchRAMLocation { get; private set; }
        public int ScenariosRAMLocation { get; private set; }
        public int WorldConditionalsCalcRAMLocation { get; private set; }

        public static SettingsData LoadSettings(XmlNode settingsNode, Context context)
        {
            SettingsData data = new SettingsData();
            data.Context = context;

            PropertyInfo[] properties = typeof(SettingsData).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                Type type = property.PropertyType;
                string xpath = "add[@key='" + property.Name + "']";
                XmlNode node = settingsNode.SelectSingleNode(xpath);

                if (node != null)
                {
                    string strValue = node.Attributes["value"].InnerText;
                    object value = 0;
                    bool isSet = true;

                    if (type == typeof(string))
                    {
                        if (property.Name.ToLower().Trim().StartsWith("filepath"))
                            value = data.GetFilepath(strValue);
                        else
                            value = strValue;
                    }
                    else if (type == typeof(int))
                    {
                        value = Utilities.ParseInt(strValue);
                    }
                    else if (type == typeof(bool))
                    {
                        value = Utilities.ParseBool(strValue);
                    }
                    else if (type == typeof(byte[]))
                    {
                        value = Utilities.GetBytesFromHexString(strValue.Replace("0x", ""));
                    }
                    else if (type == typeof(Enum))
                    {
                        value = ISOHelper.GetSector(strValue, context);
                    }
                    else
                    {
                        isSet = false;
                    }

                    if (isSet)
                        property.SetValue(data, value, null);
                }
            }

            data.TotalEventSize = data.EventSize * data.NumEvents;
            data.WorldConditionalsPointerRAMLocation = ISOHelper.GetRamOffset(data.WorldConditionalsPointerSector, context) + data.WorldConditionalsPointerOffset;
            data.BattleConditionalsLimitPatchRAMLocation = ISOHelper.GetRamOffset(data.BattleConditionalsLimitPatchSector, context) + data.BattleConditionalsLimitPatchOffset;
            data.ScenariosRAMLocation = ISOHelper.GetRamOffset(data.ScenariosSector, context) + data.ScenariosOffset;
            data.WorldConditionalsCalcRAMLocation = ISOHelper.GetRamOffset(data.WorldConditionalsSector, context) + data.WorldConditionalsOffset;

            return data;
        }

        public string GetFilepath(string filepath)
        {
            string dirPath = Path.Combine(Settings.EntryDirectory, ISOHelper.GetTypeString(Context));

            if (!string.IsNullOrEmpty(Mod))
            {
                string modDirPath = Path.Combine(dirPath, Mod);
                string modPath = Path.Combine(modDirPath, filepath);

                if (File.Exists(modPath))
                    return modPath;
            }

            string path = Path.Combine(dirPath, filepath);
            return path;
        }
    }
}
